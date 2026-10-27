using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic15Seed
{
    private static string ToJson<T>(T obj) => JsonSerializer.Serialize(obj);

    private static InteractiveFlowStep Step(int stepNumber, string stepTitle, string stepDetail, string codeSnippet) => new()
    {
        StepNumber = stepNumber,
        StepTitle = stepTitle,
        StepDetail = stepDetail,
        CodeSnippet = codeSnippet
    };

    private static SubLesson Build(
        int topicId,
        string code,
        string title,
        string skillCategory,
        string docUrl,
        string theory,
        List<InteractiveFlowStep> steps,
        string question,
        List<string> options,
        string correctAnswer,
        string explanation)
    {
        return new SubLesson
        {
            TopicId = topicId,
            LessonCode = code,
            Title = title,
            SkillCategory = skillCategory,
            OfficialDocUrl = docUrl,
            Part1_Theory = theory,
            FlowStepsJson = ToJson(steps),
            Part3_Question = question,
            Part3_OptionsJson = ToJson(options),
            Part3_CorrectAnswer = correctAnswer,
            Part3_Explanation = explanation,
            IsCompleted = false,
            CompletedAt = null
        };
    }

    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 15.1
            // =====================================================================
            Build(
                topicId,
                "15.1",
                "Bức tranh Toàn cảnh AI trong Hệ sinh thái .NET (Generative AI, LLM, Microsoft.Extensions.AI, Semantic Kernel & ML.NET)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/ai/",
                """
                • **💡 1. Hiểu bản chất 2 nhánh AI trong lập trình .NET trong 30 giây:**
                  Khi tích hợp AI vào ứng dụng C# .NET hiện nay, bạn cần phân biệt rõ **2 nhánh công nghệ** để chọn đúng công cụ:
                  1. **Nhánh 1 - `Generative AI & LLM` (AI Tạo sinh & Mô hình Ngôn ngữ Lớn — Như Google Gemini, OpenAI GPT, Claude):**
                     - Bạn không cần tự đi thu thập triệu dòng dữ liệu để huấn luyện từ đầu! Bạn kết nối qua API tới các "Bộ não khổng lồ" đã học hết kiến thức nhân loại để làm: **Trợ giảng thông minh (như ViệtBot 2D của bạn)**, **Tóm tắt tài liệu**, **Tư vấn chọn sách**, **Dịch thuật**, **Kiểm tra lỗi code**.
                  2. **Nhánh 2 - `Classical Machine Learning` (`ML.NET`):**
                     - Thư viện của Microsoft dùng để tự huấn luyện mô hình dự đoán số liệu nội bộ từ bảng SQL/CSV của công ty (như *Dự đoán giá nhà*, *Phân loại email rác*, *Dự đoán doanh số tháng tới*).
                • **🖼️ 2. Sơ đồ Kiến trúc Chuẩn `Microsoft.Extensions.AI` (`IChatClient`) Trong .NET 9 / .NET 10:**
                ```
                [Tầng Ứng dụng C# của bạn (Controller / AIService)]
                                        │
                                        ▼  (Chỉ gọi qua 1 Interface chuẩn duy nhất của Microsoft!)
                           ╔══════════════════════════╗
                           ║       IChatClient        ║  (Microsoft.Extensions.AI)
                           ║   / Semantic Kernel      ║
                           ╚════════════╤═════════════╝
                                        │ (Dễ dàng chuyển đổi mô hình AI chỉ bằng 1 dòng cấu hình!)
                     ┌──────────────────┼──────────────────┐
                     ▼                  ▼                  ▼
                [Google Gemini]    [OpenAI GPT-4o]    [Ollama Local AI]
                (gemini-2.5-flash)  (Azure OpenAI)    (Chạy miễn phí trên GPU RTX 4050 của máy bạn!)
                ```
                • **📊 3. Giải Mã Các Khái Niệm Cốt Lõi Khi Làm Việc Với LLM API:**
                | Thuật ngữ AI | Giải thích dễ hiểu cho Lập trình viên C# | Lưu ý thực tế |
                | :--- | :--- | :--- |
                | **`Token` (Đơn vị tính phí)**| AI không đọc từng chữ nguyên vẹn mà cắt thành các mảnh từ gọi là `Token` (`1.000 tokens ≈ 700 từ tiếng Việt`). | Cả câu hỏi gửi đi (`Input Tokens`) và câu trả lời nhận về (`Output Tokens`) đều tính vào giới hạn! |
                | **`Context Window`** | Sức chứa bộ nhớ ngắn hạn của 1 lần gọi AI (Ví dụ Gemini Flash có Context Window lên tới 1 triệu 토큰). | Gửi kèm lịch sử chat càng dài thì càng tốn nhiều `Input Tokens`. |
                | **`Temperature` (`0.0` - `2.0`)**| **Độ sáng tạo / Ngẫu nhiên:** Đặt **`0.1 - 0.3`** khi cần trả lời kiến thức lập trình/số liệu chính xác; đặt `0.8 - 1.0` khi cần viết văn, làm thơ sáng tạo. | Với Chatbot học tập hoặc tra cứu sách, luôn đặt `Temperature ≈ 0.2`! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo giao diện IAIService độc lập với nhà cung cấp AI",
                        "📥 Đầu vào: Yêu cầu tích hợp Chatbot vào dự án Web. ⚙️ Cơ chế ngầm: Định nghĩa interface IAIService (hoặc dùng IChatClient của Microsoft.Extensions.AI) để tầng Controller không bị phụ thuộc cứng vào Google hay OpenAI. 📤 Đầu ra: Kiến trúc linh hoạt.",
                        "public interface IAIService {\n    Task<string> AskMentorAsync(string lessonContext, string userQuestion, CancellationToken ct = default);\n}"),
                    Step(2,
                        "2. Cấu hình thông số mô hình AI (Model, Temperature, MaxOutputTokens)",
                        "📥 Đầu vào: Mục \"GeminiSettings\" trong appsettings.json. ⚙️ Cơ chế ngầm: Đặt Model = \"gemini-2.5-flash\" (nhanh và tiết kiệm nhất), Temperature = 0.2 (chính xác cao) và MaxOutputTokens = 300 (kiểm soát độ dài câu trả lời). 📤 Đầu ra: Cấu hình tối ưu chi phí.",
                        "\"GeminiSettings\": {\n  \"Model\": \"gemini-2.5-flash\",\n  \"Temperature\": 0.2,\n  \"MaxOutputTokens\": 300\n}"),
                    Step(3,
                        "3. Đăng ký HttpClient kèm Resilience bảo vệ kết nối AI",
                        "📥 Đầu vào: Dịch vụ GeminiAiService. ⚙️ Cơ chế ngầm: Đăng ký qua AddHttpClient kèm AddStandardResilienceHandler để tự động thử lại nếu máy chủ AI bị quá tải tạm thời. 📤 Đầu ra: Kết nối AI ổn định.",
                        "builder.Services.AddHttpClient<IAIService, GeminiAiService>()\n    .AddStandardResilienceHandler();")
                },
                "Khi cấu hình tham số gọi API cho một mô hình ngôn ngữ lớn (LLM như Gemini hoặc OpenAI) làm Trợ giảng giải thích kiến thức lập trình C# hoặc Tra cứu thông tin sách trong thư viện, bạn nên thiết lập tham số Temperature ở mức nào để AI trả lời chuẩn xác, bám sát sự thật và hạn chế \"bịa đặt\" (Hallucination)?",
                new List<string>
                {
                    "A. Thiết lập Temperature ở mức thấp (từ 0.0 đến 0.3) để mô hình ưu tiên các câu trả lời có xác suất chính xác và nhất quán cao nhất",
                    "B. Thiết lập Temperature ở mức cực đại (2.0) để AI tự do sáng tác",
                    "C. Đặt Temperature bằng số âm (-100)",
                    "D. Tham số Temperature dùng để đo nhiệt độ quạt tản nhiệt của Laptop"
                },
                "A",
                "Temperature điều chỉnh mức độ ngẫu nhiên khi chọn từ tiếp theo của LLM. Với các bài toán kỹ thuật, lập trình, tra cứu dữ liệu thực tế, luôn đặt Temperature thấp (0.0 - 0.3) để đảm bảo tính chính xác và ổn định."
            ),

            // =====================================================================
            // BÀI 15.2
            // =====================================================================
            Build(
                topicId,
                "15.2",
                "Tích hợp Trực tiếp Google Gemini API vào ASP.NET Core bằng Typed HttpClient (Giải phẫu Request & Response JSON)",
                "Web API",
                "https://ai.google.dev/gemini-api/docs/text-generation",
                """
                • **💡 1. Hiểu bản chất cách gọi Google Gemini API từ C# trong 30 giây:**
                  - Thực chất, gọi AI Gemini không có gì huyền bí cả! Nó chỉ đơn giản là một lệnh **`HTTP POST`** gửi từ `HttpClient` của C# tới địa chỉ:
                    `https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={API_KEY}`
                  - Bạn gửi đi một cục JSON chứa câu hỏi (`contents`), và sau 1 giây Google trả về một cục JSON chứa câu trả lời nằm trong `candidates[0].content.parts[0].text`!
                • **🖼️ 2. Sơ đồ Giải Phẫu Cấu Trúc JSON Gửi Đi (Request) & Nhận Về (Response) Của Gemini API:**
                ```json
                // 1. CỤC JSON BẠN GỬI LÊN GEMINI (Request Body):
                {
                  "systemInstruction": {
                    "parts": [{ "text": "Bạn là ViệtBot 2D, trợ giảng C# .NET ngắn gọn, dễ hiểu." }]
                  },
                  "contents": [
                    { "role": "user", "parts": [{ "text": "Giải thích từ khóa async/await trong 3 dòng" }] }
                  ],
                  "generationConfig": { "temperature": 0.2, "maxOutputTokens": 280 }
                }

                // 2. CỤC JSON GEMINI TRẢ VỀ CHO C# (Response Body):
                {
                  "candidates": [
                    {
                      "content": {
                        "parts": [{ "text": "async/await giúp giải phóng luồng xử lý trong lúc chờ I/O..." }]
                      }
                    }
                  ]
                }
                ```
                • **📊 3. Bốn Quy Tắc Vàng Khi Gọi Gemini API Trong Dự Án Thực Tế:**
                  1. **Tuyệt đối giấu `ApiKey` trong `User Secrets` hoặc Biến môi trường**, không đẩy lên GitHub.
                  2. **Luôn truyền `systemInstruction`:** Để định hình vai trò của AI ngay từ đầu.
                  3. **Luôn giới hạn `maxOutputTokens`:** Tránh việc người dùng hỏi 1 câu mà AI viết bài văn dài 3.000 chữ vừa lâu vừa tốn hạn mức.
                  4. **Luôn kiểm tra `response.IsSuccessStatusCode`** và bắt trường hợp hết hạn mức miễn phí (`429 Too Many Requests`).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo các Record DTO ánh xạ chuẩn cấu trúc JSON của Gemini",
                        "📥 Đầu vào: Cấu trúc tài liệu REST API của Google Gemini. ⚙️ Cơ chế ngầm: Dùng từ khóa record của C# để khai báo các lớp Request/Response siêu gọn chỉ trong vài dòng. 📤 Đầu ra: Bộ DTO chuẩn bị cho PostAsJsonAsync.",
                        "public record GeminiPart(string text);\npublic record GeminiContent(string role, GeminiPart[] parts);\npublic record GeminiConfig(double temperature, int maxOutputTokens);\npublic record GeminiRequest(GeminiContent systemInstruction, GeminiContent[] contents, GeminiConfig generationConfig);"),
                    Step(2,
                        "2. Gửi HTTP POST lên Gemini API bằng HttpClient",
                        "📥 Đầu vào: Câu hỏi của người dùng và ngữ cảnh bài học hiện tại. ⚙️ Cơ chế ngầm: Đóng gói vào GeminiRequest, gắn ApiKey và gọi _httpClient.PostAsJsonAsync bất đồng bộ kèm CancellationToken. 📤 Đầu ra: Nhận về HttpResponseMessage từ máy chủ Google.",
                        "string url = $\"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_settings.ApiKey}\";\n\nvar requestBody = new GeminiRequest(\n    systemInstruction: new GeminiContent(\"user\", new[] { new GeminiPart(\"Bạn là ViệtBot 2D, trợ giảng C# .NET súc tích.\") }),\n    contents: new[] { new GeminiContent(\"user\", new[] { new GeminiPart(userQuestion) }) },\n    generationConfig: new GeminiConfig(0.2, 280)\n);\n\nvar response = await _http.PostAsJsonAsync(url, requestBody, ct);"),
                    Step(3,
                        "3. Bóc tách câu trả lời từ JSON trả về an toàn với JsonDocument",
                        "📥 Đầu vào: Phản hồi JSON từ Gemini. ⚙️ Cơ chế ngầm: Dùng JsonDocument.ParseAsync đọc thẳng vào đường dẫn candidates[0] -> content -> parts[0] -> text để lấy chuỗi câu trả lời nhanh và nhẹ nhất. 📤 Đầu ra: Chuỗi câu trả lời hiển thị lên khung Chat.",
                        "if (!response.IsSuccessStatusCode)\n    return \"ViệtBot đang bận một chút, bạn thử lại sau ít giây nhé!\";\n\nusing var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync(ct), cancellationToken: ct);\nstring? answer = doc.RootElement\n    .GetProperty(\"candidates\")[0]\n    .GetProperty(\"content\")\n    .GetProperty(\"parts\")[0]\n    .GetProperty(\"text\").GetString();\n\nreturn answer ?? \"Không có phản hồi.\";")
                },
                "Khi gọi trực tiếp REST API của Google Gemini (generateContent) từ C#, việc thiết lập thuộc tính maxOutputTokens bên trong đối tượng generationConfig của Request gửi đi có tác dụng gì?",
                new List<string>
                {
                    "A. Đặt trần giới hạn số lượng Token tối đa mà AI được phép sinh ra trong một câu trả lời, giúp câu trả lời luôn súc tích đúng trọng tâm, phản hồi nhanh hơn và bảo vệ tài khoản không bị tiêu tốn quá nhiều hạn mức Token",
                    "B. Dùng để tăng tốc độ mạng Internet",
                    "C. Dùng để đổi mật khẩu tài khoản Google",
                    "D. Dùng để xóa lịch sử chat trong SQL Server"
                },
                "A",
                "Kiểm soát maxOutputTokens (ví dụ đặt từ 250 đến 500 cho trợ lý học tập) vừa giúp thời gian chờ của người dùng giảm xuống dưới 1.5 giây, vừa tiết kiệm tối đa hạn mức API miễn phí hàng ngày."
            ),

            // =====================================================================
            // BÀI 15.3
            // =====================================================================
            Build(
                topicId,
                "15.3",
                "Nghệ thuật Prompt Engineering cho Lập trình viên Backend: System Prompt, Context Injection & Few-Shot Prompting",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/semantic-kernel/concepts/prompts/",
                """
                • **💡 1. Tại sao cùng dùng chung 1 mô hình AI nhưng Chatbot của người này trả lời cực kỳ thông minh, còn của người kia lại trả lời lan man?**
                  - Bí mật nằm ở **Prompt Engineering (Kỹ thuật thiết kế câu lệnh chỉ dẫn ngầm ở Backend)**: Người dùng trên web chỉ gõ vỏn vẹn 5 chữ: *"Giải thích đoạn code này"*. Nếu Backend của bạn bê nguyên 5 chữ đó gửi thẳng cho AI, AI sẽ không biết người dùng đang học bài nào, trình độ ra sao!
                  - Một Backend Developer giỏi sẽ **tiêm thêm Ngữ cảnh bài học hiện tại (`Context Injection`) + Vai trò hệ thống (`System Prompt`) + Định dạng đầu ra** bọc xung quanh câu hỏi của người dùng trước khi gửi cho AI!
                • **🖼️ 2. Sơ đồ Công Thức "Bánh Mì Kẹp" (`Prompt Template`) Chuẩn Mực Tại Tầng Service C#:**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🍞 LỚP VỎ TRÊN (System Instruction - Khóa chặt vai trò & Quy tắc):         │
                │    - Vai trò: "Bạn là ViệtBot 2D, Mentor C# .NET của sinh viên Hoàng Việt."│
                │    - Quy tắc: "Trả lời ngắn gọn dưới 150 chữ, luôn kèm 1 ví dụ code C#."   │
                │    - Rào chắn (Guardrail): "Chỉ trả lời về lập trình, từ chối chủ đề khác."│
                ├────────────────────────────────────────────────────────────────────────────┤
                │ 🥩 LỚP NHÂN Ở GIỮA (Context Injection - Tự động lấy từ SQL Server nhét vào)│
                │    - Ngữ cảnh động: "Người dùng đang mở Bài {lesson.Code}: {lesson.Title}. │
                │      Tóm tắt bài học hiện tại: {lesson.ShortSummary}."                     │
                ├────────────────────────────────────────────────────────────────────────────┤
                │ 🍞 LỚP VỎ DƯỚI (User Question - Câu hỏi thực tế người dùng vừa gõ):        │
                │    - Câu hỏi: "{userInput}"                                                │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Ba Kỹ Thuật Thiết Kế Prompt Hiệu Quả Nhất Trong Code C#:**
                | Kỹ thuật | Cách thực hiện | Hiệu quả đạt được |
                | :--- | :--- | :--- |
                | **1. `Role & Guardrail Prompting`**| Quy định rõ AI là ai và **những gì AI KHÔNG ĐƯỢC PHÉP làm** (không trả lời chuyện chính trị, game, ngoài lề). | Ngăn chặn người dùng lợi dụng Chatbot của web học tập để hỏi chuyện linh tinh tốn Token! |
                | **2. `Dynamic Context Injection`** | Lấy thông tin bài học/cuốn sách mà người dùng đang xem trên màn hình ghép tự động vào Prompt. | Người dùng chỉ cần hỏi *"Bài này nghĩa là sao?"*, AI vẫn biết chính xác đó là bài nào! |
                | **3. `Few-Shot Prompting`** | Đưa sẵn 1-2 cặp *"Ví dụ Câu hỏi mẫu ➔ Câu trả lời mẫu"* vào trong System Prompt. | Ép AI bắt chước đúng 100% văn phong và bố cục trình bày mà bạn mong muốn! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết kế System Prompt có Rào chắn bảo vệ (Guardrails)",
                        "📥 Đầu vào: Định nghĩa nhân vật trợ giảng ViệtBot 2D. ⚙️ Cơ chế ngầm: Khai báo hằng số chuỗi quy định rõ vai trò, phong cách trình bày (dùng gạch đầu dòng thoáng mắt) và quy tắc từ chối lịch sự nếu câu hỏi không liên quan đến CNTT. 📤 Đầu ra: AI không bao giờ đi chệch hướng.",
                        "private const string SystemPrompt = @\"\nBạn là ViệtBot 2D - Trợ giảng C# .NET thông minh trên nền tảng học tập của Hoàng Việt.\nQUY TẮC BẮT BUỘC:\n1. Giải thích cực kỳ dễ hiểu bằng ví dụ đời thường trước, sau đó đưa ra code C# minh họa ngắn gọn.\n2. Trình bày tách ý thoáng mắt, tối đa 150 từ.\n3. Nếu người dùng hỏi các chủ đề KHÔNG liên quan đến Lập trình / CNTT, hãy lịch sự từ chối và hướng họ quay lại bài học.\";"),
                    Step(2,
                        "2. Tự động tiêm Ngữ cảnh bài học đang mở (Context Injection)",
                        "📥 Đầu vào: lessonId mà người dùng đang đứng trên giao diện và câu hỏi userQuestion. ⚙️ Cơ chế ngầm: Truy vấn tiêu đề bài học từ DB/Cache rồi ghép chung với câu hỏi của người dùng thành một bản Prompt hoàn chỉnh. 📤 Đầu ra: AI hiểu rõ ngữ cảnh hiện tại.",
                        "var currentLesson = await _lessonRepo.GetByIdAsync(lessonId);\n\nstring fullPrompt = $@\"\n[NGỮ CẢNH BÀI HỌC ĐANG MỞ]: Bài {currentLesson?.LessonCode} - {currentLesson?.Title}\n[CÂU HỎI CỦA HỌC VIÊN]: {userQuestion}\nHãy giải đáp trực tiếp vào trọng tâm câu hỏi trên:\";"),
                    Step(3,
                        "3. Làm sạch và giới hạn độ dài câu hỏi đầu vào (Input Sanitization)",
                        "📥 Đầu vào: Chuỗi câu hỏi từ người dùng gửi lên. ⚙️ Cơ chế ngầm: Cắt khoảng trắng, chặn nếu người dùng cố tình dán một đoạn văn bản dài 50.000 ký tự nhằm phá hoại hạn mức Token! 📤 Đầu ra: Bảo vệ an toàn ví Token.",
                        "if (string.IsNullOrWhiteSpace(userQuestion) || userQuestion.Length > 500) {\n    return BadRequest(\"Câu hỏi không được để trống và tối đa 500 ký tự!\");\n}")
                },
                "Kỹ thuật \"Context Injection\" (Tiêm ngữ cảnh động) khi xây dựng tính năng Trợ lý AI trên trang web học lập trình hoặc trang web thư viện là gì?",
                new List<string>
                {
                    "A. Là việc Backend C# tự động lấy thông tin của trang hiện tại mà người dùng đang xem (như Tên bài học, Đoạn code mẫu, hoặc Thông tin cuốn sách) từ Database ghép thêm vào trước câu hỏi của người dùng rồi mới gửi cho AI, giúp AI trả lời chính xác ngữ cảnh mà người dùng không cần gõ lại từ đầu",
                    "B. Là việc tiêm mã độc SQL Injection vào AI",
                    "C. Là việc dùng Dependency Injection trong Program.cs",
                    "D. Là việc tắt kết nối mạng của AI"
                },
                "A",
                "Nhờ Context Injection, khi người dùng đang mở Bài 2.5: Tính Kế thừa và chỉ bấm nút hỏi ngắn gọn \"Cho tôi ví dụ bài này\", Backend đã tự động đính kèm thông tin Bài 2.5: Tính Kế thừa vào Prompt nên AI biết chính xác cần lấy ví dụ về Kế thừa!"
            ),

            // =====================================================================
            // BÀI 15.4
            // =====================================================================
            Build(
                topicId,
                "15.4",
                "Quản lý Bộ nhớ Hội thoại Nhiều lượt (Multi-turn Chat History) & Chiến thuật Cắt tỉa Ngữ cảnh (Sliding Window)",
                "Tư duy đọc luồng",
                "https://ai.google.dev/gemini-api/docs/text-generation#multi-turn-conversations",
                """
                • **💡 1. Bí mật lớn nhất về các API AI (Gemini / GPT): "Bản thân API của AI hoàn toàn KHÔNG NHỚ câu trước bạn vừa hỏi gì!"**
                  - Nhiều bạn ngạc nhiên: *"Tại sao em hỏi câu 1 là 'Class là gì?', rồi câu 2 em hỏi tiếp 'Cho ví dụ về nó đi', mà AI lại hỏi ngược lại 'Nó là cái gì?'!"*.
                  - **Lý do:** Mọi REST API của LLM đều là **Stateless (Phi trạng thái)**. Mỗi lần bạn gọi `PostAsJsonAsync`, đối với AI đó là một cuộc gặp gỡ mới tinh!
                  - **Làm sao để AI nhớ được mạch chuyện?** Bạn phải lưu lịch sử các câu hỏi (`role: "user"`) và câu trả lời trước đó (`role: "model"`) vào một danh sách, và ở lần hỏi tiếp theo bạn **gửi kèm các tin nhắn gần nhất trong danh sách đó lên cho AI**!
                • **🖼️ 2. Sơ đồ Kỹ Thuật "Cửa Sổ Trượt" (`Sliding Window Chat History`) Chống Tràn Token:**
                ```
                Nếu cuộc trò chuyện kéo dài 50 câu mà bạn gửi lại cả 50 câu ở mỗi lần bấm nút ──► Tốn gấp 50 lần Token!
                ──► Giải pháp chuẩn: Chỉ giữ lại "Cửa sổ 4 đến 6 tin nhắn gần nhất (2-3 cặp Hỏi - Đáp vừa xong)"!

                Lịch sử cuộc hội thoại:
                  [Tin 1: Cũ] [Tin 2: Cũ]  │  [Tin 3: User] ──► [Tin 4: Model] ──► [Tin 5: User] ──► [Tin 6: Model] ──► [Tin 7: Câu hỏi mới!]
                  (Bỏ qua để tiết kiệm!)   │  ◄─────────────── CỬA SỔ TRƯỢT (Chỉ gửi 4 tin gần nhất + Câu mới) ───────────────►
                ```
                • **📊 3. Phân biệt Vai trò (`role`) Trong Mảng `contents` Của Gemini API:**
                | Giá trị `role` trong Gemini | Đại diện cho ai? | Lưu ý khi ghép mảng hội thoại |
                | :--- | :--- | :--- |
                | **`"user"`** | Học viên / Người dùng | Chứa câu hỏi người dùng gửi lên. |
                | **`"model"`** *(OpenAI gọi là `"assistant"`)*| Trợ lý AI (ViệtBot) | Chứa câu trả lời của AI ở các lượt trước đó. Hai vai `user` và `model` xếp xen kẽ nhau! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo cấu trúc ChatMessage lưu trữ lượt hỏi và trả lời",
                        "📥 Đầu vào: Các câu hỏi của User và câu trả lời của AI. ⚙️ Cơ chế ngầm: Lưu Role (\"user\" hoặc \"model\") và Content vào danh sách lịch sử phiên chat (trong IMemoryCache hoặc Session theo UserId). 📤 Đầu ra: Danh sách List<ChatTurn>.",
                        "public record ChatTurn(string Role, string Content);\n\n// Lấy lịch sử chat của học viên từ MemoryCache:\nvar history = _cache.GetOrCreate(\"CHAT_HISTORY_USER_1\", entry => {\n    entry.SlidingExpiration = TimeSpan.FromMinutes(30);\n    return new List<ChatTurn>();\n})!;"),
                    Step(2,
                        "2. Áp dụng kỹ thuật Sliding Window (.TakeLast(4)) để tiết kiệm Token",
                        "📥 Đầu vào: Danh sách history có thể đã dài hàng chục câu. ⚙️ Cơ chế ngầm: Dùng hàm LINQ .TakeLast(4) để chỉ nhặt ra đúng 4 lượt tin nhắn gần nhất (2 cặp hỏi-đáp vừa xong) cộng với câu hỏi hiện tại gửi lên Gemini! 📤 Đầu ra: AI vẫn hiểu mạch chuyện mà lượng Token tiêu thụ luôn siêu thấp.",
                        "// Chỉ lấy tối đa 4 tin nhắn gần nhất trong lịch sử + câu hỏi mới:\nvar recentTurns = history.TakeLast(4).ToList();\nrecentTurns.Add(new ChatTurn(\"user\", newQuestion));\n\nvar geminiContents = recentTurns\n    .Select(t => new GeminiContent(t.Role, new[] { new GeminiPart(t.Content) }))\n    .ToArray();"),
                    Step(3,
                        "3. Cập nhật câu trả lời mới của AI vào lại Lịch sử",
                        "📥 Đầu vào: Câu trả lời aiReply vừa nhận về từ Gemini. ⚙️ Cơ chế ngầm: Thêm cả câu hỏi mới (\"user\") và câu trả lời mới (\"model\") vào history để chuẩn bị cho lượt hỏi tiếp theo. 📤 Đầu ra: Hội thoại liền mạch tự nhiên.",
                        "history.Add(new ChatTurn(\"user\", newQuestion));\nhistory.Add(new ChatTurn(\"model\", aiReply));")
                },
                "Khi xây dựng tính năng Chat nhiều lượt (Multi-turn Conversation) với LLM API, tại sao các lập trình viên luôn áp dụng kỹ thuật \"Sliding Window\" (ví dụ dùng .TakeLast(4) hoặc .TakeLast(6)) thay vì gửi toàn bộ lịch sử hàng trăm tin nhắn từ đầu buổi chat lên API ở mỗi lần hỏi?",
                new List<string>
                {
                    "A. Vì mỗi lần gọi API sẽ tính phí dựa trên tổng số Input Tokens gửi lên; chỉ lấy 4-6 tin nhắn gần nhất giúp AI vẫn nắm bắt trọn vẹn mạch ngữ cảnh vừa trò chuyện mà không làm phình to lượng Token tiêu thụ theo cấp số cộng",
                    "B. Vì C# không cho phép mảng có nhiều hơn 6 phần tử",
                    "C. Vì gửi nhiều tin nhắn sẽ làm xóa Database",
                    "D. Để làm cho AI quên mất câu hỏi hiện tại"
                },
                "A",
                "Nếu gửi toàn bộ lịch sử không giới hạn, đến câu hỏi thứ 30 bạn sẽ phải gửi kèm cả 29 câu trước đó — tốn gấp 30 lần Token cho một câu hỏi đơn giản! Sliding Window (lấy N lượt gần nhất) là giải pháp cân bằng hoàn hảo giữa độ thông minh và chi phí."
            ),

            // =====================================================================
            // BÀI 15.5
            // =====================================================================
            Build(
                topicId,
                "15.5",
                "Ép AI trả về đúng cấu trúc C# DTO với Structured Outputs (Response Schema / JSON Mode)",
                "Web API",
                "https://ai.google.dev/gemini-api/docs/structured-output",
                """
                • **💡 1. Bài toán thực tế cực hay: "Khi bạn không muốn AI trả về đoạn văn bản tự do, mà muốn AI trả về đúng đối tượng C# `QuizQuestionDto`!"**
                  - Giả sử trong ứng dụng học tập của bạn có nút: **"✨ Nhờ AI tạo thêm 1 câu hỏi trắc nghiệm mới cho bài này"** hoặc **"✨ Nhờ AI chấm điểm và nhận xét đoạn code C# của tôi"**.
                  - Nếu AI trả về một đoạn văn bản tự do lúc thì gạch đầu dòng, lúc thì in đậm, code C# của bạn sẽ **không thể cắt chuỗi để tách riêng đâu là `Câu hỏi`, đâu là `4 Đáp án A, B, C, D`, đâu là `Đáp án đúng`** để lưu vào bảng SQL Server được!
                  - **Giải pháp `Structured Outputs` (`responseMimeType = "application/json"` + `responseSchema`):**
                    Bạn ra lệnh cho Gemini/OpenAI: *"Cấm trả lời bằng văn bản thường! Bắt buộc phải trả kết quả về dưới dạng một chuỗi JSON khớp 100% với các thuộc tính của class C# `GeneratedQuizDto` này!"*. Khi nhận về, bạn chỉ cần gọi `JsonSerializer.Deserialize<GeneratedQuizDto>(aiText)` là biến thẳng thành đối tượng C# ngay lập tức!
                • **🖼️ 2. Sơ đồ Biến AI Thành "Cỗ Máy Sinh Dữ Liệu C# Tự Động" (`Structured JSON Output`):**
                ```
                Yêu cầu: "Hãy tạo 1 câu trắc nghiệm về LINQ Where" + Cấu hình: responseMimeType = "application/json"
                      │
                      ▼  (AI bị khóa chặt định dạng đầu ra, chỉ trả về chuỗi JSON thuần khiết 100%):
                {
                  "question": "Hàm Where trong LINQ trả về kiểu dữ liệu gì?",
                  "options": ["A. int", "B. IEnumerable<T> / IQueryable<T>", "C. void", "D. bool"],
                  "correctAnswer": "B",
                  "explanation": "Hàm Where lọc danh sách và trả về tập hợp thỏa mãn điều kiện."
                }
                      │
                      ▼  JsonSerializer.Deserialize<QuizDto>(json)
                Đối tượng C# QuizDto sẵn sàng lưu thẳng vào SQL Server hoặc vẽ lên giao diện!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Định nghĩa Class/Record C# đại diện cho cấu trúc muốn AI trả về",
                        "📥 Đầu vào: Bài toán nhờ AI chấm điểm code C# của sinh viên (gồm Điểm số từ 0-10, Nhận xét ưu điểm, Lỗi cần sửa và Code gợi ý). ⚙️ Cơ chế ngầm: Tạo record CodeReviewResultDto với tên các thuộc tính rõ ràng. 📤 Đầu ra: Khuôn mẫu dữ liệu đích.",
                        "public record CodeReviewResultDto(\n    int Score,\n    string Strengths,\n    List<string> BugsFound,\n    string ImprovedCode\n);"),
                    Step(2,
                        "2. Bật chế độ responseMimeType = \"application/json\" khi gọi Gemini",
                        "📥 Đầu vào: Cấu hình generationConfig của Gemini API. ⚙️ Cơ chế ngầm: Đặt responseMimeType = \"application/json\" và mô tả rõ khung JSON yêu cầu trong Prompt. Gemini sẽ đảm bảo 100% chuỗi trả về là JSON hợp lệ, không kèm các dấu nháy ```json thừa! 📤 Đầu ra: Chuỗi JSON sạch tuyệt đối.",
                        "var generationConfig = new {\n    temperature = 0.1,\n    responseMimeType = \"application/json\" // Ép Gemini chỉ trả về JSON chuẩn!\n};"),
                    Step(3,
                        "3. Giải mã trực tiếp kết quả của AI sang đối tượng C# bằng JsonSerializer",
                        "📥 Đầu vào: Chuỗi jsonText do AI sinh ra. ⚙️ Cơ chế ngầm: Dùng JsonSerializer.Deserialize với PropertyNameCaseInsensitive = true để biến chuỗi của AI thành đối tượng CodeReviewResultDto trong C#. 📤 Đầu ra: Đổ từng thuộc tính lên giao diện Card đẹp mắt!",
                        "var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };\nCodeReviewResultDto? review = JsonSerializer.Deserialize<CodeReviewResultDto>(aiJsonText, options);\n\n// Sử dụng như một đối tượng C# bình thường:\nConsole.WriteLine($\"Điểm của bạn: {review?.Score}/10\");")
                },
                "Khi bạn muốn tích hợp AI vào chức năng \"Tự động tạo câu hỏi trắc nghiệm mới lưu vào Database\" hoặc \"Chấm điểm bài tập trả về Điểm số (int) và Nhận xét (string) riêng biệt\", tính năng nào của Gemini / OpenAI API giúp đảm bảo kết quả trả về luôn có thể Deserialize thẳng sang class DTO trong C# mà không bị lỗi định dạng?",
                new List<string>
                {
                    "A. Sử dụng Structured Outputs / JSON Mode (thiết lập responseMimeType = \"application/json\" kèm Schema) để ép mô hình AI bắt buộc trả về đúng cấu trúc JSON khớp với class DTO của C#",
                    "B. Tăng Temperature lên 2.0",
                    "C. Dùng vòng lặp for cắt từng chữ cái",
                    "D. Không thể ép AI trả về JSON được"
                },
                "A",
                "Structured Outputs (JSON Mode) là kỹ thuật quan trọng bậc nhất khi nhúng AI vào các tính năng nghiệp vụ Backend, biến LLM từ một công cụ chat tự do thành một hàm xử lý dữ liệu trả về đúng kiểu DTO của C#."
            ),

            // =====================================================================
            // BÀI 15.6
            // =====================================================================
            Build(
                topicId,
                "15.6",
                "Truyền tải Câu trả lời AI theo Thời gian thực (Streaming Response với IAsyncEnumerable & Server-Sent Events)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/ai/quickstarts/build-chat-app",
                """
                • **💡 1. Tại sao trên ChatGPT hay Gemini Web, chữ lại "chạy ra từng từ một" ngay lập tức thay vì bắt người dùng đợi 5 giây rồi mới hiện cả cục?**
                  - Nếu AI cần viết một đoạn giải thích dài 200 từ (tổng thời gian AI suy nghĩ hết câu là `4 giây`):
                    + **Cách thông thường (`Wait for Full Response`):** Người dùng nhìn màn hình đứng im suốt `4 giây`, đến giây thứ 4 mới bụp một phát hiện ra cả đoạn văn ➔ Cảm giác web bị đơ rất lâu!
                    + **Cách `Streaming` (Truyền luồng từng từ - `streamGenerateContent`):** Ngay ở **`0.3 giây` đầu tiên**, AI nghĩ ra chữ đầu tiên là *"Chào"* ➔ Đẩy luôn chữ *"Chào"* về màn hình! Nghĩ tiếp chữ *"Hoàng Việt"* ➔ Đẩy tiếp về màn hình! Người dùng đọc đến đâu chữ hiện ra mượt mà đến đó, cảm giác phản hồi **tức thì 0 giây**!
                • **🖼️ 2. Sơ đồ Luồng Truyền Dữ Liệu Từng Mảnh (`Streaming`) Từ AI ➔ C# (`IAsyncEnumerable<string>`) ➔ Trình Duyệt:**
                ```
                [Máy chủ AI Gemini (streamGenerateContent)]
                      │
                      ▼  (Trả về từng mảnh chữ nhỏ ngay khi vừa sinh ra)
                [Backend ASP.NET Core: Hàm async IAsyncEnumerable<string> StreamChatAsync()]
                      │  Dùng từ khóa: yield return textChunk;
                      ▼  (Truyền qua HTTP Stream / Server-Sent Events hoặc SignalR)
                [Giao diện Trình duyệt (ReadableStream / for await)]
                      └──► Nối từng mảnh chữ vào khung Chat: "Trong..." -> "Trong C#..." -> "Trong C#, LINQ là..."
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Sử dụng IAsyncEnumerable<string> và yield return ở tầng Service C#",
                        "📥 Đầu vào: Luồng dữ liệu trả về từ AI (IChatClient.GetStreamingResponseAsync hoặc streamGenerateContent). ⚙️ Cơ chế ngầm: Dùng vòng lặp await foreach đọc từng mảnh văn bản (chunk) ngay khi AI vừa nhả ra và dùng lệnh yield return đẩy thẳng lên Controller mà không chờ gom hết câu! 📤 Đầu ra: Luồng chữ thời gian thực.",
                        "public async IAsyncEnumerable<string> StreamAnswerAsync(\n    string prompt,\n    [EnumeratorCancellation] CancellationToken ct = default)\n{\n    await foreach (var update in _chatClient.GetStreamingResponseAsync(prompt, cancellationToken: ct))\n    {\n        if (!string.IsNullOrEmpty(update.Text))\n        {\n            yield return update.Text; // Có chữ nào đẩy về trình duyệt ngay chữ đó!\n        }\n    }\n}"),
                    Step(2,
                        "2. Mở Endpoint Streaming tại Controller",
                        "📥 Đầu vào: Hàm StreamAnswerAsync trả về IAsyncEnumerable<string>. ⚙️ Cơ chế ngầm: ASP.NET Core hỗ trợ trả về trực tiếp IAsyncEnumerable hoặc ghi từng mảnh vào Response.Body kèm FlushAsync(). 📤 Đầu ra: Gói tin HTTP truyền theo từng đợt (Chunked transfer).",
                        "[HttpPost(\"stream-chat\")]\npublic async Task StreamChat([FromBody] ChatRequestDto dto, CancellationToken ct) {\n    Response.ContentType = \"text/plain; charset=utf-8\";\n    await foreach (string chunk in _aiService.StreamAnswerAsync(dto.Question, ct)) {\n        await Response.WriteAsync(chunk, ct);\n        await Response.Body.FlushAsync(ct); // Đẩy ngay mảnh chữ về trình duyệt!\n    }\n}"),
                    Step(3,
                        "3. Đọc luồng chữ chạy mượt mà trên Giao diện bằng Fetch ReadableStream",
                        "📥 Đầu vào: Gọi fetch('/api/ai/stream-chat'). ⚙️ Cơ chế ngầm: Dùng response.body.getReader() và TextDecoder trong vòng lặp while (true) để nhận từng mảnh chữ và cộng dồn (+=) trực tiếp vào thẻ div của khung chat! 📤 Đầu ra: Hiệu ứng gõ chữ (Typewriter) y hệt ChatGPT!",
                        "const response = await fetch('/api/ai/stream-chat', { method: 'POST', ... });\nconst reader = response.body.getReader();\nconst decoder = new TextDecoder();\n\nwhile (true) {\n    const { done, value } = await reader.read();\n    if (done) break;\n    const chunkText = decoder.decode(value, { stream: true });\n    chatBubble.innerHTML += chunkText; // Chữ hiện ra từng từ ngay lập tức!\n}")
                },
                "Trong C# (.NET hiện đại), kiểu trả về nào kết hợp với từ khóa yield return được sử dụng để truyền từng mảnh văn bản (Token chunks) từ AI về cho người dùng theo thời gian thực (Streaming) ngay khi chúng vừa được sinh ra mà không cần đợi toàn bộ câu trả lời hoàn tất?",
                new List<string>
                {
                    "A. Kiểu IAsyncEnumerable<string> (duyệt bằng await foreach và trả từng mảnh qua yield return)",
                    "B. Kiểu int",
                    "C. Kiểu bool",
                    "D. Kiểu void"
                },
                "A",
                "IAsyncEnumerable<T> là chuẩn bất đồng bộ luồng (Async Streaming) trong C# 8+ và .NET hiện đại, cho phép phát (yield) từng phần tử dữ liệu qua mạng ngay khi có sẵn."
            ),

            // =====================================================================
            // BÀI 15.7
            // =====================================================================
            Build(
                topicId,
                "15.7",
                "Kiến trúc RAG (Retrieval-Augmented Generation) - Giúp AI Trả lời Chính xác Dựa trên Dữ liệu SQL Server Của Riêng Bạn!",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/ai/conceptual/rag",
                """
                • **💡 1. Hiểu bản chất của `RAG` (Tạo sinh tăng cường truy xuất) trong 30 giây (Kiến trúc AI quan trọng số 1 hiện nay!):**
                  - *Vấn đề:* Bộ não của Gemini/ChatGPT trên mạng biết rất nhiều thứ trên đời, NHƯNG nó **hoàn toàn KHÔNG BIẾT trong bảng `Books` của thư viện trường bạn hiện đang có những cuốn sách nào, còn mấy cuốn trong kho**, hay trong Database bài học của bạn viết nội dung gì! Nếu khách hỏi *"Thư viện còn sách Lập trình C# không?"*, AI thông thường sẽ đoán mò hoặc trả lời không biết!
                  - **Giải pháp `RAG (Retrieval-Augmented Generation)` — "Cho phép AI mở sách giáo khoa nội bộ của bạn ra tra cứu trước khi trả lời":**
                    1. **Bước 1 (`Retrieval` - Truy xuất từ DB của bạn):** Khi người dùng hỏi *"Tìm sách học C# dưới 150k"*, Backend C# của bạn **chạy câu lệnh tìm kiếm trong SQL Server trước** để nhặt ra 3 cuốn sách phù hợp nhất đang có trong kho.
                    2. **Bước 2 (`Augmented` - Bổ sung dữ liệu thật vào Prompt):** Backend C# kẹp thông tin 3 cuốn sách thật đó (Tên, Giá, Số lượng tồn kho) vào trong Prompt gửi cho AI.
                    3. **Bước 3 (`Generation` - AI tổng hợp câu trả lời):** AI đọc đúng dữ liệu thật từ DB của bạn vừa đưa và trả lời ngọt ngào: *"Dạ hiện thư viện đang còn 2 cuốn sách C# dưới 150k là: 1. C# Căn bản (120.000đ - còn 5 cuốn)..."* **chính xác 100% không bao giờ bịa đặt**!
                • **🖼️ 2. Sơ đồ Luồng Hoạt Động Của Kiến Trúc `RAG` Kết Hợp `SQL Server` + `Gemini AI`:**
                ```
                Học viên / Độc giả hỏi: "Thư viện mình có cuốn sách nào về Entity Framework còn hàng không?"
                      │
                      ▼
                [Bước 1 - RETRIEVAL (Tại Backend C# & SQL Server)]:
                    Chạy EF Core tìm trong bảng Books các sách có chứa từ khóa "Entity Framework" và Stock > 0
                    ──► Tìm thấy 2 cuốn thật trong DB: [ "Làm chủ EF Core 10 (Giá: 145k, Tồn: 4)", "ORM Thực chiến (Giá: 110k, Tồn: 2)" ]
                      │
                      ▼
                [Bước 2 - AUGMENTATION (Ghép dữ liệu thật vào Prompt)]:
                    "Dựa DUY NHẤT vào danh sách sách thực tế trong kho dưới đây:
                     [1. Làm chủ EF Core 10 - 145k - Còn 4 cuốn; 2. ORM Thực chiến - 110k - Còn 2 cuốn].
                     Hãy trả lời câu hỏi của độc giả: {câu_hỏi}. Nếu không có trong danh sách thì báo hết sách."
                      │
                      ▼
                [Bước 3 - GENERATION (Gọi Gemini API)]:
                    AI tổng hợp câu trả lời tự nhiên, chuẩn xác 100% theo đúng số lượng tồn kho trong SQL Server của bạn!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bước Retrieval: Truy vấn dữ liệu liên quan từ SQL Server bằng EF Core",
                        "📥 Đầu vào: Câu hỏi của người dùng trên trang web Thư viện hoặc Học tập. ⚙️ Cơ chế ngầm: Tách từ khóa chính và truy vấn trong bảng Books (hoặc SubLessons) lấy ra Top 5 bản ghi sát nhất với câu hỏi. 📤 Đầu ra: Dữ liệu sự thật (Ground Truth) lấy từ chính Database của bạn.",
                        "var matchingBooks = await _db.Books\n    .AsNoTracking()\n    .Where(b => b.Title.Contains(keyword) || b.Category.Name.Contains(keyword))\n    .Take(5)\n    .Select(b => $\"- Mã: {b.BookCode} | Tên: {b.Title} | Giá: {b.Price:N0}đ | Tồn kho: {b.Stock} cuốn\")\n    .ToListAsync(ct);"),
                    Step(2,
                        "2. Bước Augmentation: Đóng gói dữ liệu DB vào Prompt có ràng buộc chặt",
                        "📥 Đầu vào: Danh sách matchingBooks vừa lấy từ SQL Server. ⚙️ Cơ chế ngầm: Ghép danh sách thành khối [DỮ LIỆU NỘI BỘ TRONG DATABASE] và ra lệnh cho AI chỉ được phép trả lời dựa trên dữ liệu này, tuyệt đối không tự bịa thêm sách bên ngoài. 📤 Đầu ra: Bản RAG Prompt hoàn chỉnh.",
                        "string dbContextText = matchingBooks.Any()\n    ? string.Join(\"\\n\", matchingBooks)\n    : \"Không tìm thấy cuốn sách nào khớp trong kho.\";\n\nstring ragPrompt = $@\"\nBạn là trợ lý ảo của Thư viện. Hãy trả lời câu hỏi của độc giả DỰA TRÊN DỮ LIỆU KHO SÁCH THỰC TẾ dưới đây:\n--- DỮ LIỆU KHO SÁCH ---\n{dbContextText}\n------------------------\nCâu hỏi độc giả: {userQuestion}\";"),
                    Step(3,
                        "3. Bước Generation: Gửi RAG Prompt tới Gemini và nhận câu trả lời chuẩn xác",
                        "📥 Đầu vào: Chuỗi ragPrompt ở Bước 2. ⚙️ Cơ chế ngầm: Gửi lên Gemini API. Nhờ đã có sẵn dữ liệu thật từ SQL Server trong Prompt, AI trả lời chính xác từng con số tồn kho và giá tiền! 📤 Đầu ra: Trợ lý AI thông thái hiểu rõ toàn bộ Database của bạn.",
                        "string answer = await _geminiService.GenerateTextAsync(ragPrompt, ct);\nreturn Ok(new { reply = answer });")
                },
                "Trong kiến trúc tích hợp AI hiện đại, kỹ thuật RAG (Retrieval-Augmented Generation) giải quyết bài toán cốt lõi nào mà một mô hình LLM thông thường (như Gemini hay GPT nguyên bản) không tự làm được?",
                new List<string>
                {
                    "A. Giúp AI có thể trả lời chính xác các câu hỏi về dữ liệu nội bộ, riêng tư và cập nhật theo thời gian thực trong Database SQL Server của dự án (như số lượng sách tồn kho, tiến độ học tập cá nhân, nội dung bài học riêng) bằng cách truy xuất dữ liệu từ DB đính kèm vào Prompt trước khi AI sinh câu trả lời",
                    "B. Giúp AI chạy không cần điện",
                    "C. Giúp xóa bỏ hoàn toàn cơ sở dữ liệu SQL Server",
                    "D. Giúp tăng dung lượng ổ cứng"
                },
                "A",
                "Khi đi phỏng vấn hoặc bảo vệ đồ án có tích hợp AI, nhắc tới từ khóa RAG (Retrieval-Augmented Generation) để giải thích cách Chatbot của bạn đọc dữ liệu từ bảng SQL Server để tư vấn cho người dùng sẽ giúp bạn ghi điểm tuyệt đối về kiến thức kiến trúc hiện đại!"
            ),

            // =====================================================================
            // BÀI 15.8
            // =====================================================================
            Build(
                topicId,
                "15.8",
                "Tìm kiếm Ngữ nghĩa (Semantic Search) với Vector Embeddings & Độ tương đồng Cosine (Cosine Similarity)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/ai/conceptual/embeddings",
                """
                • **💡 1. Khi nào tìm kiếm từ khóa `LIKE '%...%'` truyền thống bị bó tay và cần đến `Vector Embeddings`?**
                  - Giả sử trong thư viện có cuốn sách tên là: **"Xây dựng ứng dụng Web với ASP.NET Core"**.
                  - Nhưng một sinh viên vào ô tìm kiếm lại gõ: *"Sách dạy làm trang mạng bằng ngôn ngữ của Microsoft"*.
                  - Nếu dùng `WHERE Title LIKE '%trang mạng%'`: Câu lệnh SQL sẽ trả về **`0 kết quả`** vì không có chữ *"trang mạng"* nào trùng khớp ký tự cả!
                  - **Giải pháp `Vector Embeddings` (Tìm kiếm theo Ý nghĩa - Semantic Search):**
                    Mô hình AI Embedding (`text-embedding-004`) sẽ biến mỗi câu văn thành một **Mảng các con số tọa độ ý nghĩa (`float[]` gồm 768 con số)** gọi là **Vector**. Hai câu văn dù dùng từ ngữ khác hẳn nhau nhưng **có cùng ý nghĩa** sẽ nằm **sát cạnh nhau trong không gian tọa độ**!
                • **🖼️ 2. Sơ đồ Biến Câu Chữ Thành Tọa Độ Số (`Vector Embedding`) & Đo Khoảng Cách Ý Nghĩa (`Cosine Similarity`):**
                ```
                Câu 1: "Lập trình Web ASP.NET Core"     ──(Embedding Model)──► Vector A: [ 0.82,  0.15, -0.44, ... ] ┐
                                                                                                                     ├─► Góc cực gần nhau!
                Câu 2: "Làm trang mạng bằng C# .NET"    ──(Embedding Model)──► Vector B: [ 0.80,  0.17, -0.41, ... ] ┘   (Cosine = 0.94 -> CÙNG Ý NGHĨA!)

                Câu 3: "Công thức nấu canh ngao chua"   ──(Embedding Model)──► Vector C: [-0.65,  0.91,  0.72, ... ] ──► Xa tít tắp! (Cosine = 0.05)
                ```
                • **📊 3. Công cụ Tính Độ Tương Đồng Vector Có Sẵn Cực Nhanh Trong .NET (`TensorPrimitives`):**
                  - Trong .NET 8 / 9 / 10, Microsoft cung cấp sẵn lớp **`TensorPrimitives.CosineSimilarity(vectorA, vectorB)`** (trong thư viện `System.Numerics.Tensors`) sử dụng tập lệnh phần cứng SIMD của CPU để tính điểm giống nhau về ý nghĩa (từ `-1` đến `1`, càng gần `1.0` là càng giống nghĩa nhau!) chỉ trong vài nano-giây!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Gọi API Embedding biến chuỗi văn bản thành mảng tọa độ float[]",
                        "📥 Đầu vào: Tên và mô tả của cuốn sách (hoặc câu hỏi của người dùng). ⚙️ Cơ chế ngầm: Gọi model 'text-embedding-004' của Gemini (hoặc IEmbeddingGenerator của Microsoft.Extensions.AI) để nhận về mảng float[] biểu diễn ý nghĩa của văn bản. 📤 Đầu ra: Mảng float[] gồm 768 con số.",
                        "public async Task<float[]> GetEmbeddingVectorAsync(string text) {\n    var result = await _embeddingGenerator.GenerateEmbeddingVectorAsync(text);\n    return result.ToArray(); // Mảng 768 số thực biểu diễn ý nghĩa ngữ nghĩa\n}"),
                    Step(2,
                        "2. Đo độ giống nhau về ý nghĩa bằng TensorPrimitives.CosineSimilarity",
                        "📥 Đầu vào: Vector câu hỏi của người dùng (queryVector) và Vector của các cuốn sách trong kho (bookVector). ⚙️ Cơ chế ngầm: Hàm TensorPrimitives.CosineSimilarity tính góc giữa 2 vector trả về điểm từ -1.0 đến 1.0. 📤 Đầu ra: Điểm số tương đồng ngữ nghĩa.",
                        "using System.Numerics.Tensors;\n\nfloat similarityScore = TensorPrimitives.CosineSimilarity(queryVector, bookVector);\n// Nếu similarityScore >= 0.75f -> Hai nội dung rất sát ý nghĩa với nhau!"),
                    Step(3,
                        "3. Sắp xếp lấy Top 3 cuốn sách có ý nghĩa sát nhất với câu hỏi",
                        "📥 Đầu vào: Danh sách sách kèm Vector của chúng. ⚙️ Cơ chế ngầm: Dùng LINQ OrderByDescending theo điểm CosineSimilarity và lấy Take(3) để đưa vào bước RAG ở Bài 15.7! 📤 Đầu ra: Tìm thấy đúng sách dù người dùng không nhớ chính xác tên sách!",
                        "var bestMatches = allBooks\n    .Select(b => new {\n        Book = b,\n        Score = TensorPrimitives.CosineSimilarity(queryVector, b.EmbeddingVector)\n    })\n    .Where(x => x.Score >= 0.70f)\n    .OrderByDescending(x => x.Score)\n    .Take(3)\n    .ToList();")
                },
                "Sự khác biệt bản chất giữa Tìm kiếm theo từ khóa truyền thống (SQL LIKE / .Contains()) và Tìm kiếm ngữ nghĩa bằng Vector Embeddings (Semantic Search) là gì?",
                new List<string>
                {
                    "A. Tìm kiếm LIKE chỉ khớp khi có các chữ cái giống hệt nhau; còn Vector Embeddings chuyển văn bản thành các tọa độ số biểu diễn ý nghĩa và đo độ tương đồng (Cosine Similarity), giúp tìm ra đúng kết quả có cùng ý nghĩa ngay cả khi người dùng dùng từ đồng nghĩa hoặc diễn đạt bằng câu chữ khác",
                    "B. Vector Embeddings chỉ tìm được các con số nguyên",
                    "C. Tìm kiếm LIKE luôn thông minh hơn Vector Embeddings",
                    "D. Vector Embeddings dùng để nén file ảnh"
                },
                "A",
                "Kết hợp Vector Embeddings với TensorPrimitives.CosineSimilarity (hoặc kiểu dữ liệu VECTOR mới trong SQL Server / PostgreSQL pgvector) là nền tảng của các hệ thống tìm kiếm AI thông minh hiện nay."
            ),

            // =====================================================================
            // BÀI 15.9
            // =====================================================================
            Build(
                topicId,
                "15.9",
                "Kỹ thuật Function Calling / Tool Use (Semantic Kernel Plugins) - Cho phép AI Tự Gọi Hàm C# Trong Dự Án!",
                "Web API",
                "https://learn.microsoft.com/vi-vn/semantic-kernel/concepts/plugins/",
                """
                • **💡 1. Hiểu bản chất của `Function Calling / AI Agents` trong 30 giây:**
                  - Ở Bài 15.7 (`RAG`), code C# của bạn luôn cố định tìm sách trước rồi mới gọi AI.
                  - Nhưng nếu người dùng lúc thì hỏi kiến thức C# bình thường, lúc thì bảo: *"ViệtBot ơi, kiểm tra giúp tôi xem hiện tại tôi đã hoàn thành bao nhiêu phần trăm lộ trình học tập rồi?"*, lúc lại bảo: *"Gia hạn giúp tôi phiếu mượn sách mã #105 thêm 7 ngày nhé!"*.
                  - Với kỹ thuật **`Function Calling` (Sử dụng Công cụ / Semantic Kernel Plugins)**: Bạn cung cấp cho AI một **"Menu danh sách các hàm C#"** mà bạn cho phép AI sử dụng (kèm mô tả `[Description]`). Khi đọc câu nói của người dùng, **AI sẽ tự suy luận xem cần gọi hàm C# nào, truyền tham số gì**, báo cho C# chạy hàm đó lấy kết quả thật từ Database rồi trả lời lại cho người dùng!
                • **🖼️ 2. Sơ đồ Cơ Chế `Function Calling` Giữa Người Dùng ➔ AI ➔ Hàm C# (`Plugin`):**
                ```
                1. Bạn đăng ký các hàm C# cho AI biết:
                   [KernelFunction, Description("Lấy tiến độ học tập hiện tại của học viên")]
                   public async Task<UserStatsDto> GetMyProgressAsync(int userId) { ... }

                2. Người dùng chat: "Tôi học được bao nhiêu bài rồi nhỉ?"
                      │
                      ▼
                3. AI đọc câu hỏi ──► Thấy khớp với mô tả của hàm GetMyProgressAsync!
                      │
                      ▼  (AI tự động kích hoạt gọi hàm C# GetMyProgressAsync() lấy dữ liệu từ SQL Server)
                4. Hàm C# trả về: { "completed": 42, "total": 160, "gpaSkill": "OOP: 85%" }
                      │
                      ▼
                5. AI đọc kết quả của hàm C# và trả lời: "Chúc mừng Hoàng Việt! Bạn đã hoàn thành 42/160 bài học..."
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Class Plugin chứa các hàm C# gắn thẻ [KernelFunction] và [Description]",
                        "📥 Đầu vào: Các hàm C# truy vấn tiến độ học tập hoặc tra cứu sách. ⚙️ Cơ chế ngầm: Thẻ [Description(\"...\")] cực kỳ quan trọng vì AI sẽ đọc chính dòng mô tả tiếng Việt/tiếng Anh này để biết khi nào cần gọi hàm đó và từng tham số có ý nghĩa gì! 📤 Đầu ra: Bộ công cụ (Tools) cho AI.",
                        "using Microsoft.SemanticKernel;\nusing System.ComponentModel;\n\npublic class LibraryPlugin {\n    private readonly IBookService _bookService;\n    public LibraryPlugin(IBookService bookService) => _bookService = bookService;\n\n    [KernelFunction(\"check_book_stock\")]\n    [Description(\"Kiểm tra số lượng tồn kho thực tế của một cuốn sách theo tên sách\")]\n    public async Task<int> CheckStockAsync(\n        [Description(\"Tên cuốn sách cần kiểm tra\")] string bookTitle)\n    {\n        return await _bookService.GetStockByNameAsync(bookTitle);\n    }\n}"),
                    Step(2,
                        "2. Đăng ký Plugin vào Semantic Kernel / IChatClient trong Program.cs",
                        "📥 Đầu vào: Class LibraryPlugin vừa viết. ⚙️ Cơ chế ngầm: Đưa Plugin vào danh sách công cụ của Kernel để AI nắm được danh mục các hàm C# có thể sử dụng. 📤 Đầu ra: AI được trang bị cánh tay nối dài vào Database.",
                        "var kernelBuilder = Kernel.CreateBuilder();\nkernelBuilder.Plugins.AddFromType<LibraryPlugin>(\"LibraryTools\");\nvar kernel = kernelBuilder.Build();"),
                    Step(3,
                        "3. Bật chế độ AutoInvokeKernelFunctions để AI tự động gọi hàm C# khi cần",
                        "📥 Đầu vào: Câu hỏi bất kỳ của người dùng. ⚙️ Cơ chế ngầm: Nếu câu hỏi là lý thuyết chung -> AI trả lời ngay. Nếu câu hỏi cần tra cứu tồn kho -> AI tự động gọi hàm CheckStockAsync, lấy con số thật rồi mới trả lời! 📤 Đầu ra: Trợ lý AI Agent thực thụ.",
                        "var settings = new PromptExecutionSettings {\n    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() // Cho phép AI tự quyết định gọi hàm C#!\n};\nvar response = await chatService.GetChatMessageContentAsync(chatHistory, settings, kernel);")
                },
                "Khi xây dựng một Plugin / Tool trong C# (với Semantic Kernel hoặc Microsoft.Extensions.AI) để cho phép mô hình AI tự động gọi hàm C# (Function Calling), tại sao việc gắn thẻ [Description(\"...\")] rõ ràng lên trên tên hàm và các tham số đầu vào lại đóng vai trò quyết định?",
                new List<string>
                {
                    "A. Vì mô hình AI không đọc toàn bộ code bên trong thân hàm của bạn, mà nó đọc chính nội dung văn bản trong thẻ [Description] để hiểu hàm đó dùng để làm gì và cần trích xuất tham số nào từ câu nói của người dùng để truyền vào hàm",
                    "B. Để đổi màu chữ trên Visual Studio",
                    "C. Để tạo bảng mới trong SQL Server",
                    "D. Vì không có [Description] thì C# không biên dịch được"
                },
                "A",
                "Nội dung trong [Description] sẽ được đóng gói thành Tool Schema gửi lên cho LLM. Mô tả càng rõ ràng, chính xác thì AI càng biết kích hoạt đúng hàm C# vào đúng thời điểm người dùng cần."
            ),

            // =====================================================================
            // BÀI 15.10
            // =====================================================================
            Build(
                topicId,
                "15.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 15 (Kiến trúc AI Trợ giảng 2D Kép Tối ưu Chi phí, Cache & Chống Lạm dụng)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/ai/",
                """
                • **💡 1. Bản Thiết Kế Hoàn Chỉnh Hệ Thống AI Thực Chiến Trong Dự Án "Hoàng Việt" (Vừa Thông Minh Vừa Tiết Kiệm 90% Token!):**
                  Khi tích hợp AI vào sản phẩm thực tế, một kỹ sư giỏi không chỉ làm cho AI chạy được mà phải giải quyết **Bài toán Chi phí & Bảo mật**. Dưới đây là **5 Lớp Tối Ưu Vàng** chúng ta đã áp dụng cho **ViệtBot 2D**:
                ```
                Học viên bấm hỏi ViệtBot 2D:
                  │
                  ├─► Lớp 1 [Validate & Rate Limit]: Chặn câu hỏi rỗng, > 500 ký tự, giới hạn tối đa 15 câu/phút.
                  │
                  ├─► Lớp 2 [Smart Hash Cache 24h] : Băm SHA256(Mã_Bài_Học + Câu_Hỏi_Chuẩn_Hóa) kiểm tra trong IMemoryCache:
                  │     ├── Nếu đã từng hỏi câu này trong 24h qua ──► Trả kết quả ngay trong 0.001s (Tốn 0 Token!)
                  │     └── Nếu là câu hỏi mới hoàn toàn ──► Đi tiếp xuống Lớp 3!
                  │
                  ├─► Lớp 3 [RAG + Context Inject] : Chỉ đính kèm tiêu đề + tóm tắt ngắn của bài học đang mở (Không gửi thừa).
                  │
                  ├─► Lớp 4 [Sliding Window (4)]   : Chỉ gửi kèm tối đa 4 lượt tin nhắn gần nhất để giữ mạch chuyện.
                  │
                  └─► Lớp 5 [Gemini 2.5 Flash API] : Đặt Temperature = 0.2, MaxOutputTokens = 280 -> Lưu vào Cache 24h & Trả về!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo khóa Cache thông minh từ Ngữ cảnh bài học và Câu hỏi chuẩn hóa",
                        "📥 Đầu vào: lessonId và câu hỏi rawQuestion của học viên. ⚙️ Cơ chế ngầm: Đưa câu hỏi về chữ thường, cắt khoảng trắng thừa và băm SHA256 để các câu hỏi giống nhau (dù viết hoa/thường khác nhau) đều trúng Cache 100%! 📤 Đầu ra: Chuỗi cacheKey duy nhất.",
                        "string normalizedQuestion = rawQuestion.Trim().ToLowerInvariant();\nstring rawKey = $\"AI_LESSON_{lessonId}_Q_{normalizedQuestion}\";\nbyte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));\nstring cacheKey = $\"VIETBOT_{Convert.ToHexString(hashBytes)}\";"),
                    Step(2,
                        "2. Kiểm tra IMemoryCache trước khi gọi API bên ngoài (Tiết kiệm 100% Token lặp)",
                        "📥 Đầu vào: Chuỗi cacheKey vừa tạo ở Bước 1. ⚙️ Cơ chế ngầm: Đặc biệt với 3 nút bấm gợi ý sẵn ở mỗi bài học ('1. Giải thích đời thường', '2. Giải thích code', '3. Câu hỏi phỏng vấn'), chỉ cần gọi AI đúng 1 lần đầu tiên, mọi lần bấm sau trả thẳng từ RAM! 📤 Đầu ra: Phản hồi 0ms.",
                        "if (_cache.TryGetValue(cacheKey, out string? cachedAnswer) && !string.IsNullOrEmpty(cachedAnswer)) {\n    return cachedAnswer; // Trúng Cache -> Không tốn 1 Token nào!\n}"),
                    Step(3,
                        "3. Ghép RAG Context bài học hiện tại & Gọi Gemini 2.5 Flash",
                        "📥 Đầu vào: Khi chưa có trong Cache (Cache Miss). ⚙️ Cơ chế ngầm: Lấy thông tin bài học hiện tại từ DB, ghép với SystemPrompt của ViệtBot 2D, giới hạn maxOutputTokens = 280 và gọi Gemini API. 📤 Đầu ra: Câu trả lời chất lượng cao từ AI.",
                        "var lesson = await _lessonRepo.GetByIdAsync(lessonId, ct);\nstring prompt = $\"[Bài học đang mở: {lesson?.Title}]\\nHọc viên hỏi: {rawQuestion}\";\nstring aiReply = await CallGeminiFlashApiAsync(prompt, maxTokens: 280, ct);"),
                    Step(4,
                        "4. Lưu câu trả lời mới vào Cache 24 giờ và trả về cho Giao diện Kép",
                        "📥 Đầu vào: Câu trả lời aiReply hợp lệ. ⚙️ Cơ chế ngầm: Cất vào IMemoryCache trong 24 giờ và trả về JSON cho cả Phòng Mentor AI 2D toàn màn hình lẫn Widget nổi góc phải! 📤 Đầu ra: Hệ thống AI hoàn chỉnh, siêu tiết kiệm và bền bỉ.",
                        "_cache.Set(cacheKey, aiReply, TimeSpan.FromHours(24));\nreturn aiReply;")
                },
                "(Câu hỏi tổng hợp Chủ đề 15) Trong thiết kế hệ thống Trợ giảng AI (như ViệtBot 2D có sẵn 3 nút bấm câu hỏi gợi ý nhanh ở mỗi bài học), việc kết hợp chuẩn hóa câu hỏi + băm khóa SHA256(LessonId + Question) để lưu đệm câu trả lời vào IMemoryCache / Redis trong 24 giờ mang lại hiệu quả thực tế gì?",
                new List<string>
                {
                    "A. Giúp những câu hỏi phổ biến hoặc các nút gợi ý nhanh ở mỗi bài học chỉ cần gọi API AI đúng 1 lần đầu tiên; các lần bấm sau được trả về tức thì từ RAM (0.1ms) mà không tốn thêm bất kỳ Token nào và không lo chạm giới hạn Rate Limit của nhà cung cấp AI",
                    "B. Làm cho câu trả lời bị xóa sau 1 giây",
                    "C. Bắt người dùng phải tự trả lời câu hỏi",
                    "D. Giúp không cần kết nối Database cho toàn bộ trang web"
                },
                "A",
                "Kỹ thuật Semantic / Exact Prompt Caching là bí quyết sống còn khi triển khai các ứng dụng AI thực tế, giúp giảm tới 70-80% chi phí gọi API LLM cho các câu hỏi trùng lặp của người dùng."
            )
        };
    }

    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic15 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 15);
        if (topic15 == null)
            return;

        topic15.Title = "Chủ đề 15: Tích hợp Trí tuệ Nhân tạo (AI & LLM) trong ứng dụng .NET";
        topic15.Description = "10 Bài học con siêu chi tiết từ Microsoft.Extensions.AI & Semantic Kernel, Gọi trực tiếp Google Gemini API (Typed HttpClient), Prompt Engineering & Context Injection, Multi-turn Chat History (Sliding Window), Structured Outputs (JSON Schema), Streaming (IAsyncEnumerable), RAG với SQL Server, Vector Embeddings & CosineSimilarity, Function Calling đến Kiến trúc ViệtBot 2D thực chiến.";

        var targetLessons = GetLessons(topic15.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic15.Id)
            .ToListAsync();

        foreach (var target in targetLessons)
        {
            var existing = existingLessons.FirstOrDefault(e => e.LessonCode == target.LessonCode);
            if (existing == null)
            {
                context.SubLessons.Add(target);
            }
            else
            {
                existing.Title = target.Title;
                existing.SkillCategory = target.SkillCategory;
                existing.OfficialDocUrl = target.OfficialDocUrl;
                existing.Part1_Theory = target.Part1_Theory;
                existing.FlowStepsJson = target.FlowStepsJson;
                existing.Part3_Question = target.Part3_Question;
                existing.Part3_OptionsJson = target.Part3_OptionsJson;
                existing.Part3_CorrectAnswer = target.Part3_CorrectAnswer;
                existing.Part3_Explanation = target.Part3_Explanation;
            }
        }

        await context.SaveChangesAsync();
    }
}
