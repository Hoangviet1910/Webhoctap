using System.Text;
using System.Text.Json;
using HoangViet.Data;
using HoangViet.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HoangViet.Controllers;

public class AIStudioController : Controller
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;
    private readonly AppDbContext _db;

    public AIStudioController(
        IConfiguration config,
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache,
        AppDbContext db)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var allLessons = await _db.SubLessons.ToListAsync();
        int totalCount = allLessons.Count;
        int completedCount = allLessons.Count(s => s.IsCompleted);

        ViewData["CurrentTrack"] = "AISTUDIO";
        ViewData["OverallProgressPercent"] = totalCount == 0 ? 0 : (int)Math.Round((double)completedCount * 100 / totalCount);
        ViewData["CompletedLessonsCount"] = completedCount;
        ViewData["TotalLessonsCount"] = totalCount;

        var vm = new AIStudioViewModel();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> AskMentor([FromBody] MentorChatRequest request)
    {
        var userQuestion = (request?.Message ?? string.Empty).Trim();
        var lessonCode = (request?.LessonCode ?? "1.1").Trim();
        var lessonTitle = (request?.LessonTitle ?? "Bài học C# .NET 10").Trim();
        var activeStep = (request?.ActiveStepTitle ?? "Bước 1").Trim();

        if (string.IsNullOrWhiteSpace(userQuestion))
        {
            return Json(new
            {
                mood = "happy",
                reply = "Chào **Hoàng Việt**! Mình là **ViệtBot 2D - Trợ giảng C#**. Bạn hãy chọn một chế độ bên trái hoặc nhập câu hỏi vào khung chat nhé!",
                timestamp = DateTime.Now.ToString("HH:mm")
            });
        }

        string cacheKey = $"AI_MENTOR_CACHE::{lessonCode}::{activeStep}::{userQuestion.ToLowerInvariant()}";
        if (_cache.TryGetValue(cacheKey, out string? cachedReply) && !string.IsNullOrWhiteSpace(cachedReply))
        {
            return Json(new
            {
                mood = "happy",
                reply = cachedReply,
                timestamp = DateTime.Now.ToString("HH:mm")
            });
        }

        var apiKey = _config["GeminiApiKey"];
        if (!string.IsNullOrWhiteSpace(apiKey) && apiKey != "YOUR_GEMINI_API_KEY_HERE")
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={apiKey}";

                var systemPrompt =
                    "Bạn là trợ lý dạy C# của Hoàng Việt. Chỉ trả lời trọng tâm trong 3 gạch đầu dòng kèm 1 ví dụ code ngắn dưới 5 dòng. " +
                    $"Bối cảnh hiện tại: Bài {lessonCode} ({lessonTitle}), Hộp bước đang chọn: {activeStep}.";

                var payload = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = $"{systemPrompt}\nCâu hỏi của học viên: {userQuestion}" }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        maxOutputTokens = 280,
                        temperature = 0.4
                    }
                };

                var response = await client.PostAsync(
                    url,
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    var geminiText = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();

                    if (!string.IsNullOrWhiteSpace(geminiText))
                    {
                        _cache.Set(cacheKey, geminiText, TimeSpan.FromHours(24));
                        return Json(new
                        {
                            mood = "happy",
                            reply = geminiText,
                            timestamp = DateTime.Now.ToString("HH:mm")
                        });
                    }
                }
            }
            catch
            {
                // Tự động chuyển sang chế độ phản hồi thông minh nội bộ
            }
        }

        string lower = userQuestion.ToLowerInvariant();
        string replyText;
        string mood = "explaining";

        if (lower.Contains("phỏng vấn thử") || lower.Contains("c# intern"))
        {
            mood = "interview";
            replyText =
                "• **Câu hỏi phỏng vấn C# Backend Intern #1:** *Sự khác nhau cốt lõi giữa `IQueryable<T>` và `IEnumerable<T>` trong Entity Framework Core là gì?*\n" +
                "• **Câu hỏi #2:** *Tại sao Controller lại tiêm `private readonly IOrderService` qua Constructor thay vì dùng `new OrderService()`?*\n" +
                "• **Gợi ý trả lời ăn điểm:** Hãy trả lời dựa trên vị trí lọc dữ liệu (SQL Server vs RAM) và nguyên lý Dependency Injection!\n" +
                "```csharp\n// IQueryable lọc trực tiếp dưới SQL Server:\nvar list = await _db.Orders.AsNoTracking().Where(x => x.IsPaid).ToListAsync();\n```";
        }
        else if (lower.Contains("bài tập lớn mvc") || lower.Contains("giải thích luồng"))
        {
            mood = "happy";
            replyText =
                "• **Luồng chuẩn dự án ASP.NET Core MVC / Web API:** `Request` ➔ `Controller` (Kiểm tra DTO & Model Binding) ➔ `Service` (Xử lý quy tắc nghiệp vụ) ➔ `Repository / DbContext` (Truy vấn SQL Server) ➔ `ViewModel / Response DTO`.\n" +
                "• **Mẹo đọc project nhanh:** Mở `Program.cs` xem đăng ký DI trước, sau đó đặt Breakpoint (`F9`) tại Controller và bấm `Ctrl + F12` để nhảy thẳng vào Service.\n" +
                "• **Nguyên tắc sống còn:** Tuyệt đối không trả thẳng Entity ra ngoài API mà phải map sang DTO/ViewModel.\n" +
                "```csharp\npublic async Task<IActionResult> Details(int id) =>\n    Ok(await _service.GetByIdAsync(id));\n```";
        }
        else if (lower.Contains("review") || lower.Contains("tìm lỗi"))
        {
            mood = "thinking";
            replyText =
                "• **3 lỗi phổ biến nhất khi Review Code thực tập sinh:** (1) Quên kiểm tra `null` dẫn tới `NullReferenceException`, (2) Gọi `.ToListAsync()` trước `.Where()` gây tràn RAM, và (3) Quên `await _db.SaveChangesAsync()`.\n" +
                "• **Cách khắc phục chuẩn Senior:** Áp dụng Guard Clauses (`if (item is null) return NotFound();`) ngay đầu hàm và dùng `.AsNoTracking()` cho các truy vấn chỉ đọc.\n" +
                "• **Bạn hãy dán đoạn code C# cần kiểm tra vào ô chat bên dưới** để mình chỉ ra từng dòng cần tối ưu nhé!\n" +
                "```csharp\nif (string.IsNullOrWhiteSpace(dto.Email)) return BadRequest();\nbool exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);\n```";
        }
        else if (lower.Contains("đời thường") || lower.Contains("ví dụ"))
        {
            mood = "happy";
            replyText =
                $"• **Ví dụ đời thường ({lessonCode} - {lessonTitle}):** Hãy hình dung luồng code giống như quầy giao dịch ngân hàng: Lễ tân (Controller) chỉ kiểm tra giấy tờ (DTO), Chuyên viên (Service) tính toán nghiệp vụ, còn Thủ quỹ (Repository) mới mở két sắt (SQL Server).\n" +
                $"• **Vai trò của {activeStep}:** Nhận đúng dữ liệu đầu vào từ bước trước, xử lý gọn nhiệm vụ của mình rồi chuyển tiếp sang bước kế tiếp.\n" +
                "• **Lợi ích khi đi làm:** Khi lỗi xảy ra ở mắt xích nào, bạn chỉ cần mở đúng file của mắt xích đó để sửa.\n" +
                "```csharp\nvar dto = await _service.ExecuteStepAsync(input);\nreturn Ok(dto);\n```";
        }
        else if (lower.Contains("bước này") || lower.Contains("đoạn code"))
        {
            mood = "explaining";
            replyText =
                $"• **Luồng chạy tại {activeStep} (Bài {lessonCode}):** Chương trình nhận dữ liệu đầu vào đã được kiểm tra kiểu tĩnh, thực thi câu lệnh C# và đẩy kết quả sang bước kế tiếp.\n" +
                "• **Điểm cần chú ý khi đọc code:** Quan sát kỹ chữ ký hàm (kiểu trả về `Task<T>`, tham số truyền vào) và từ khóa `await`.\n" +
                "• **Mẹo Debug nhanh:** Đặt Breakpoint (`F9`) ngay dòng đầu tiên của bước này và bấm `F10` / `F11` để xem giá trị biến trực tiếp.\n" +
                "```csharp\nif (request is null) return BadRequest();\nvar result = await _handler.ProcessAsync(request);\n```";
        }
        else if (lower.Contains("phỏng vấn") || lower.Contains("hay hỏi"))
        {
            mood = "interview";
            replyText =
                $"• **Câu hỏi phỏng vấn cốt lõi ({lessonCode}):** Nhà tuyển dụng thường hỏi *\"Tại sao phải tách {lessonTitle} theo cách này mà không viết gộp lại?\"*\n" +
                "• **Cách trả lời ghi điểm Intern/Fresher:** Nhấn mạnh vào tính dễ đọc luồng (Input ➔ Process ➔ Output), dễ bảo trì, chống rò rỉ dữ liệu nhạy cảm và tối ưu bộ nhớ/SQL.\n" +
                "• **Từ khóa ăn điểm:** Nhắc tới `Separation of Concerns`, `Dependency Injection`, `Async/Await` và `Clean Code`.\n" +
                "```csharp\nprivate readonly IAppService _service;\n```";
        }
        else
        {
            mood = "happy";
            replyText =
                $"• **Trọng tâm ({lessonCode} - {lessonTitle}):** Xác định rõ dữ liệu đi vào (Input), quy tắc xử lý tại **{activeStep}** (Process) và kết quả trả ra (Output).\n" +
                "• **Nguyên tắc thực chiến:** Luôn kiểm tra điều kiện biên (Guard Clauses / Null check) ở đầu hàm trước khi xử lý.\n" +
                "• **Đối chiếu tài liệu:** Bạn có thể bấm nút *Tài liệu chuẩn Microsoft Learn* ở đầu bài học để xem thêm đặc tả chính thức.\n" +
                "```csharp\npublic async Task<IActionResult> HandleAsync(int id) =>\n    id <= 0 ? BadRequest() : Ok(await _service.GetByIdAsync(id));\n```";
        }

        _cache.Set(cacheKey, replyText, TimeSpan.FromHours(24));

        return Json(new
        {
            mood,
            reply = replyText,
            timestamp = DateTime.Now.ToString("HH:mm")
        });
    }
}

public class MentorChatRequest
{
    public string Message { get; set; } = string.Empty;
    public string LessonCode { get; set; } = "1.1";
    public string LessonTitle { get; set; } = string.Empty;
    public string ActiveStepTitle { get; set; } = string.Empty;
}
