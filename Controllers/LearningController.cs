using Microsoft.AspNetCore.Mvc;

namespace HoangViet.Controllers;

public class LearningController : Controller
{
    public IActionResult InternshipTrack()
    {
        return RedirectToAction("Index", "Home", new { track = "INTERN_TRACK" });
    }

    public IActionResult DetailedTrack()
    {
        return RedirectToAction("Index", "Home", new { track = "DETAILED_TRACK" });
    }

    public IActionResult LessonDetail(int? subLessonId = null, int? lessonId = null)
    {
        return RedirectToAction("Index", "Home", new { subLessonId = subLessonId ?? lessonId });
    }
}
