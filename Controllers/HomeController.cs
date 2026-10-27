using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HoangViet.Data;
using HoangViet.Models;

namespace HoangViet.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db)
    {
        _db = db;
    }

    private static string NormalizeTrackCode(string? track)
    {
        if (string.IsNullOrWhiteSpace(track))
            return "INTERN_TRACK";

        var t = track.Trim().ToUpperInvariant();
        if (t == "DETAILED_TRACK" || t == "DETAILED")
            return "DETAILED_TRACK";

        return "INTERN_TRACK";
    }

    private static List<TimelineStudyPoint> BuildTimelinePoints(IEnumerable<SubLesson> allSubLessons)
    {
        var completedByDate = allSubLessons
            .Where(s => s.IsCompleted && s.CompletedAt.HasValue)
            .GroupBy(s => s.CompletedAt!.Value.ToLocalTime().Date)
            .ToDictionary(g => g.Key, g => g.Count());

        var today = DateTime.Now.Date;
        var points = new List<TimelineStudyPoint>();

        for (int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            int count = completedByDate.TryGetValue(date, out int c) ? c : 0;
            points.Add(new TimelineStudyPoint
            {
                DateLabel = date.ToString("dd/MM"),
                CompletedLessonsCount = count,
                CompletedExercisesCount = count
            });
        }

        return points;
    }

    /// <summary>
    /// Màn hình chính: Course Player đa cấp (Sidebar Accordion Chủ đề -> Bài học con 1.1, 1.2... + Bảng Thống kê Năng lực)
    /// </summary>
    public async Task<IActionResult> Index(string track = "INTERN_TRACK", int? subLessonId = null, int? lessonId = null, int? topicId = null)
    {
        await DbInitializer.InitializeAsync(_db);

        var normalizedTrack = NormalizeTrackCode(track);
        int? targetSubLessonId = subLessonId ?? lessonId;

        var allTracks = await _db.Tracks
            .Include(tr => tr.Topics)
                .ThenInclude(tp => tp.SubLessons)
            .OrderBy(tr => tr.Id)
            .ToListAsync();

        var skills = await _db.SkillMetrics.OrderBy(s => s.Id).ToListAsync();

        foreach (var tr in allTracks)
        {
            tr.Topics = tr.Topics.OrderBy(tp => tp.TopicNumber).ToList();
            foreach (var tp in tr.Topics)
            {
                tp.SubLessons = tp.SubLessons.OrderBy(s => s.Id).ToList();
            }
        }

        var currentTrack = allTracks.FirstOrDefault(tr => tr.Code == normalizedTrack) ?? allTracks.First();

        // Nếu người dùng truyền subLessonId thuộc Track kia, tự động chuyển sang Track chứa bài học đó
        if (targetSubLessonId.HasValue)
        {
            var ownerTrack = allTracks.FirstOrDefault(tr =>
                tr.Topics.SelectMany(tp => tp.SubLessons).Any(s => s.Id == targetSubLessonId.Value));
            if (ownerTrack != null)
            {
                currentTrack = ownerTrack;
                normalizedTrack = ownerTrack.Code;
            }
        }

        var topicsInTrack = currentTrack.Topics.OrderBy(tp => tp.TopicNumber).ToList();
        var flatSubLessons = topicsInTrack.SelectMany(tp => tp.SubLessons).ToList();

        SubLesson currentSubLesson;
        if (targetSubLessonId.HasValue)
        {
            currentSubLesson = flatSubLessons.FirstOrDefault(s => s.Id == targetSubLessonId.Value)
                               ?? flatSubLessons.First();
        }
        else if (topicId.HasValue)
        {
            var requestedTopic = topicsInTrack.FirstOrDefault(tp => tp.Id == topicId.Value);
            currentSubLesson = requestedTopic?.SubLessons.FirstOrDefault()
                               ?? flatSubLessons.First();
        }
        else
        {
            currentSubLesson = flatSubLessons.FirstOrDefault(s => !s.IsCompleted)
                               ?? flatSubLessons.First();
        }

        var currentTopic = topicsInTrack.First(tp => tp.Id == currentSubLesson.TopicId);
        var currentIdx = flatSubLessons.FindIndex(s => s.Id == currentSubLesson.Id);

        var allLessonsAcrossTracks = allTracks.SelectMany(tr => tr.Topics).SelectMany(tp => tp.SubLessons).ToList();
        var timelinePoints = BuildTimelinePoints(allLessonsAcrossTracks);

        var vm = new CoursePlayerViewModel
        {
            CurrentTrackCode = normalizedTrack,
            CurrentTrack = currentTrack,
            AllTracks = allTracks,
            TopicsInTrack = topicsInTrack,
            CurrentTopic = currentTopic,
            CurrentSubLesson = currentSubLesson,
            GlobalLessonNumber = currentIdx + 1,
            TotalLessonsCount = flatSubLessons.Count,
            CompletedLessonsCount = flatSubLessons.Count(s => s.IsCompleted),
            SkillMetrics = skills,
            TimelinePoints = timelinePoints,
            PreviousSubLessonId = currentIdx > 0 ? flatSubLessons[currentIdx - 1].Id : null,
            NextSubLessonId = currentIdx >= 0 && currentIdx < flatSubLessons.Count - 1 ? flatSubLessons[currentIdx + 1].Id : null
        };

        ViewData["CurrentTrack"] = normalizedTrack;
        ViewData["OverallProgressPercent"] = vm.OverallProgressPercent;
        ViewData["CompletedLessonsCount"] = vm.CompletedLessonsCount;
        ViewData["TotalLessonsCount"] = vm.TotalLessonsCount;
        ViewData["CurrentSubLessonCode"] = currentSubLesson.LessonCode;
        ViewData["CurrentSubLessonTitle"] = currentSubLesson.Title;

        return View(vm);
    }

    /// <summary>
    /// API lưu Database khi học viên chọn đúng đáp án ở Phần 3 và bấm "Hoàn thành bài học"
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CompleteLesson([FromBody] CompleteLessonRequest request)
    {
        int targetId = request?.EffectiveSubLessonId ?? 0;
        if (targetId <= 0)
        {
            return BadRequest(new { success = false, message = "Mã bài học con không hợp lệ." });
        }

        var subLesson = await _db.SubLessons
            .Include(s => s.Topic)
                .ThenInclude(tp => tp!.Track)
            .FirstOrDefaultAsync(s => s.Id == targetId);

        if (subLesson == null)
        {
            return NotFound(new { success = false, message = "Không tìm thấy bài học con trong Database." });
        }

        int addedPoints = 0;
        string skillName = subLesson.SkillCategory;
        int updatedSkillScore = 0;

        if (!subLesson.IsCompleted)
        {
            subLesson.IsCompleted = true;
            subLesson.CompletedAt = DateTime.UtcNow;
            addedPoints = 10;

            var skill = await _db.SkillMetrics
                .FirstOrDefaultAsync(s => s.SkillName == subLesson.SkillCategory)
                ?? await _db.SkillMetrics.FirstOrDefaultAsync();

            if (skill != null)
            {
                skill.CurrentScore = Math.Min(100, skill.CurrentScore + addedPoints);
                skillName = skill.SkillName;
                updatedSkillScore = skill.CurrentScore;
            }

            await _db.SaveChangesAsync();
        }
        else
        {
            var existingSkill = await _db.SkillMetrics.FirstOrDefaultAsync(s => s.SkillName == subLesson.SkillCategory);
            updatedSkillScore = existingSkill?.CurrentScore ?? 0;
        }

        var trackCode = NormalizeTrackCode(request?.CurrentTrack ?? subLesson.Topic?.Track?.Code);
        var trackSubLessons = await _db.SubLessons
            .Include(s => s.Topic)
                .ThenInclude(tp => tp!.Track)
            .Where(s => s.Topic != null && s.Topic.Track != null && s.Topic.Track.Code == trackCode)
            .ToListAsync();

        int totalCount = trackSubLessons.Count;
        int completedCount = trackSubLessons.Count(s => s.IsCompleted);
        int overallPercent = totalCount == 0 ? 0 : (int)Math.Round((double)completedCount * 100 / totalCount);

        var allSkills = await _db.SkillMetrics.OrderBy(s => s.Id).ToListAsync();
        var allLessons = await _db.SubLessons.ToListAsync();
        var timeline = BuildTimelinePoints(allLessons);

        return Json(new
        {
            success = true,
            subLessonId = subLesson.Id,
            lessonCode = subLesson.LessonCode,
            isCompleted = subLesson.IsCompleted,
            addedPoints,
            skillName,
            updatedSkillScore,
            completedLessonsCount = completedCount,
            totalLessonsCount = totalCount,
            overallProgressPercent = overallPercent,
            skillScores = allSkills.Select(s => s.CurrentScore).ToList(),
            timelineLabels = timeline.Select(t => t.DateLabel).ToList(),
            timelineCounts = timeline.Select(t => t.CompletedLessonsCount).ToList()
        });
    }

    /// <summary>
    /// Màn hình Thống Kê Năng Lực chuyên sâu (Radar/Doughnut Chart 6 kỹ năng + Line Chart tiến độ theo ngày)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Stats()
    {
        await DbInitializer.InitializeAsync(_db);

        var skills = await _db.SkillMetrics.OrderBy(s => s.Id).ToListAsync();
        var allSubLessons = await _db.SubLessons
            .Include(s => s.Topic)
                .ThenInclude(tp => tp!.Track)
            .ToListAsync();

        var internLessons = allSubLessons.Where(s => s.Topic?.Track?.Code == "INTERN_TRACK").ToList();
        var detailedLessons = allSubLessons.Where(s => s.Topic?.Track?.Code == "DETAILED_TRACK").ToList();

        var vm = new StatsViewModel
        {
            SkillMetrics = skills,
            TimelinePoints = BuildTimelinePoints(allSubLessons),
            TotalLessonsCount = allSubLessons.Count,
            CompletedLessonsCount = allSubLessons.Count(s => s.IsCompleted),
            InternTrackTotal = internLessons.Count,
            InternTrackCompleted = internLessons.Count(s => s.IsCompleted),
            DetailedTrackTotal = detailedLessons.Count,
            DetailedTrackCompleted = detailedLessons.Count(s => s.IsCompleted)
        };

        ViewData["CurrentTrack"] = "STATS";
        ViewData["OverallProgressPercent"] = vm.OverallProgressPercent;
        ViewData["CompletedLessonsCount"] = vm.CompletedLessonsCount;
        ViewData["TotalLessonsCount"] = vm.TotalLessonsCount;

        return View(vm);
    }

    /// <summary>
    /// Đặt lại toàn bộ tiến độ bài học và điểm năng lực về 0% ban đầu
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ResetProgress(string? returnTrack = "INTERN_TRACK")
    {
        var subLessons = await _db.SubLessons.ToListAsync();
        foreach (var s in subLessons)
        {
            s.IsCompleted = false;
            s.CompletedAt = null;
        }

        var skills = await _db.SkillMetrics.ToListAsync();
        foreach (var sk in skills)
        {
            sk.CurrentScore = 0;
        }

        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { track = NormalizeTrackCode(returnTrack) });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
