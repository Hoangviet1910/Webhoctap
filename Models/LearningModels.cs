using System.ComponentModel.DataAnnotations;

namespace HoangViet.Models;

public class SkillMetric
{
    [Key]
    public int Id { get; set; }

    [MaxLength(120)]
    public string SkillName { get; set; } = string.Empty;

    // Mặc định ban đầu = 0%
    public int CurrentScore { get; set; } = 0;
    public int MaxScore { get; set; } = 100;
    public int TargetInternScore { get; set; } = 85;

    [MaxLength(80)]
    public string Category { get; set; } = string.Empty;

    public string AIAdvice { get; set; } = string.Empty;
}

public class TimelineStudyPoint
{
    public string DateLabel { get; set; } = string.Empty;
    public int CompletedLessonsCount { get; set; }
    public int CompletedExercisesCount { get; set; }
}

public class MockInterviewQuestion
{
    public int Id { get; set; }
    public string TopicTag { get; set; } = string.Empty;
    public string Question { get; set; } = string.Empty;
    public string KeyPointsToAnswer { get; set; } = string.Empty;
    public string SampleInternAnswer { get; set; } = string.Empty;
}

public class ImportTopicPayloadDto
{
    public string TrackCode { get; set; } = "INTERN_TRACK";
    public int? TopicId { get; set; }
    public int TopicNumber { get; set; }
    public string? TopicTitle { get; set; }
    public string? TopicDescription { get; set; }
    public bool ReplaceExisting { get; set; } = false;
    public List<ImportSubLessonDto> SubLessons { get; set; } = new();
}

public class ImportSubLessonDto
{
    public string LessonCode { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Part1_Theory { get; set; } = string.Empty;
    public string OfficialDocUrl { get; set; } = "https://learn.microsoft.com/vi-vn/dotnet/csharp/";
    public List<InteractiveFlowStep> FlowSteps { get; set; } = new();
    public string? FlowStepsJson { get; set; }
    public string Part3_Question { get; set; } = string.Empty;
    public List<string> Part3_Options { get; set; } = new();
    public string? Part3_OptionsJson { get; set; }
    public string Part3_CorrectAnswer { get; set; } = string.Empty;
    public string Part3_Explanation { get; set; } = string.Empty;
    public string SkillCategory { get; set; } = "Tư duy đọc luồng";
}
