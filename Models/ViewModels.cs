namespace HoangViet.Models;

public class CoursePlayerViewModel
{
    public string CurrentTrackCode { get; set; } = "INTERN_TRACK"; // INTERN_TRACK hoặc DETAILED_TRACK
    public Track CurrentTrack { get; set; } = new();
    public List<Track> AllTracks { get; set; } = new();
    public List<Topic> TopicsInTrack { get; set; } = new();
    public Topic CurrentTopic { get; set; } = new();
    public SubLesson CurrentSubLesson { get; set; } = new();

    public int GlobalLessonNumber { get; set; }
    public int TotalLessonsCount { get; set; }
    public int CompletedLessonsCount { get; set; }
    public int OverallProgressPercent => TotalLessonsCount == 0
        ? 0
        : (int)Math.Round((double)CompletedLessonsCount * 100 / TotalLessonsCount);

    public List<SkillMetric> SkillMetrics { get; set; } = new();
    public List<TimelineStudyPoint> TimelinePoints { get; set; } = new();

    public int? PreviousSubLessonId { get; set; }
    public int? NextSubLessonId { get; set; }

    public List<string> ContextQuickQuestions { get; set; } = new();
}

public class StatsViewModel
{
    public List<SkillMetric> SkillMetrics { get; set; } = new();
    public List<TimelineStudyPoint> TimelinePoints { get; set; } = new();
    public int TotalLessonsCount { get; set; }
    public int CompletedLessonsCount { get; set; }
    public int InternTrackTotal { get; set; }
    public int InternTrackCompleted { get; set; }
    public int DetailedTrackTotal { get; set; }
    public int DetailedTrackCompleted { get; set; }

    public int OverallProgressPercent => TotalLessonsCount == 0
        ? 0
        : (int)Math.Round((double)CompletedLessonsCount * 100 / TotalLessonsCount);
}

public class ImportTopicViewModel
{
    public List<Track> Tracks { get; set; } = new();
    public List<Topic> AllTopics { get; set; } = new();
    public string JsonPayload { get; set; } = string.Empty;
    public string? StatusMessage { get; set; }
    public bool IsSuccess { get; set; }
    public int ImportedCount { get; set; }
}

public class CompleteLessonRequest
{
    public int SubLessonId { get; set; }
    public int LessonId { get; set; }
    public string CurrentTrack { get; set; } = "INTERN_TRACK";
    public string CurrentTrackCode { get; set; } = "INTERN_TRACK";

    public int EffectiveSubLessonId => SubLessonId > 0 ? SubLessonId : LessonId;
}

public class AIStudioViewModel
{
    public List<MockInterviewQuestion> InterviewQuestions { get; set; } = new();
    public List<InteractiveFlowStep> SampleArchitectureSteps { get; set; } = new();
}
