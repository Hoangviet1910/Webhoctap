using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace HoangViet.Models;

public class SubLesson
{
    [Key]
    public int Id { get; set; }

    public int TopicId { get; set; }

    [ForeignKey(nameof(TopicId))]
    public Topic? Topic { get; set; }

    [Required]
    [MaxLength(20)]
    public string LessonCode { get; set; } = string.Empty; // VD: "1.1", "1.2", "2.1"

    [Required]
    [MaxLength(260)]
    public string Title { get; set; } = string.Empty;

    // PHẦN 1: Lý thuyết chuẩn xác, trình bày thoáng, dễ đọc
    public string Part1_Theory { get; set; } = string.Empty;

    // Link tài liệu Microsoft Learn đối chiếu
    [MaxLength(500)]
    public string OfficialDocUrl { get; set; } = "https://learn.microsoft.com/vi-vn/dotnet/csharp/";

    // PHẦN 2: Mảng JSON các Hộp Bước Tương Tác (StepNumber, StepTitle, StepDetail, CodeSnippet)
    public string FlowStepsJson { get; set; } = "[]";

    // PHẦN 3: Dữ liệu bài tập kiểm tra
    public string Part3_Question { get; set; } = string.Empty;
    public string Part3_OptionsJson { get; set; } = "[]";

    [MaxLength(300)]
    public string Part3_CorrectAnswer { get; set; } = string.Empty;

    public string Part3_Explanation { get; set; } = string.Empty;

    // Nhóm kỹ năng: Tư duy đọc luồng, C# toàn diện, OOP thực tế, SQL & EF Core, Web API, Kiểm thử & Debug
    [MaxLength(120)]
    public string SkillCategory { get; set; } = "Tư duy đọc luồng";

    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }

    [NotMapped]
    public List<InteractiveFlowStep> FlowSteps
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FlowStepsJson))
                return new List<InteractiveFlowStep>();
            try
            {
                return JsonSerializer.Deserialize<List<InteractiveFlowStep>>(FlowStepsJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<InteractiveFlowStep>();
            }
            catch
            {
                return new List<InteractiveFlowStep>();
            }
        }
    }

    [NotMapped]
    public List<string> Part3_Options
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Part3_OptionsJson))
                return new List<string>();
            try
            {
                return JsonSerializer.Deserialize<List<string>>(Part3_OptionsJson) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}

public class InteractiveFlowStep
{
    public int StepNumber { get; set; }
    public string StepTitle { get; set; } = string.Empty;
    public string StepDetail { get; set; } = string.Empty; // Đầu vào nhận gì -> Xử lý thế nào -> Đẩy kết quả đi đâu
    public string CodeSnippet { get; set; } = string.Empty;
}
