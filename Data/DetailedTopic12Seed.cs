using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic12Seed
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
            // BÀI 12.1
            // =====================================================================
            Build(
                topicId,
                "12.1",
                "Kiến trúc Ứng dụng Web Full Stack Hiện đại: So sánh Monolithic MVC vs Tách rời Frontend - Backend (API-Driven)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures",
                """
                • **💡 1. Hiểu bản chất 2 mô hình kiến trúc Web Full Stack trong 30 giây:**
                  Khi xây dựng một trang web hoàn chỉnh từ giao diện đến cơ sở dữ liệu (Full Stack) trong .NET, có **2 mô hình kiến trúc chính** mà bạn sẽ gặp:
                  1. **Mô hình 1 - `Monolithic MVC / Razor` (Tất cả trong một Project):**
                     - Cả code Backend C#, kết nối Database và giao diện `.cshtml` nằm chung trong 1 project duy nhất. Khi người dùng bấm chuyển trang, `Controller` lấy dữ liệu rồi trộn với file `.cshtml` trên Server tạo ra trang HTML hoàn chỉnh gửi về trình duyệt (**Server-Side Rendering - SSR**).
                     - *Ưu điểm:* Làm cực nhanh, không lo lỗi CORS, rất hợp cho **Đồ án môn học, Web quản trị nội bộ, Trang tin tức chuẩn SEO**.
                  2. **Mô hình 2 - `Decoupled Frontend & Backend API` (Tách rời 2 đầu độc lập):**
                     - **Backend (`ASP.NET Core Web API`):** Chỉ làm nhiệm vụ xử lý nghiệp vụ, Database và trả về dữ liệu **JSON thuần túy** (không có một dòng HTML nào).
                     - **Frontend (`Blazor WebAssembly`, `React`, `Angular`, `Vue` hoặc `MVC gọi API`):** Là một project riêng biệt chuyên vẽ giao diện và gọi HTTP Request sang Backend lấy JSON về hiển thị (**Client-Side Rendering - CSR**).
                     - *Ưu điểm:* **1 Backend API duy nhất** có thể phục vụ cùng lúc cho cả **Website**, **App Mobile iOS/Android** và **Phần mềm Desktop**!
                • **🖼️ 2. Sơ đồ So Sánh Luồng Chạy Của 2 Mô Hình Full Stack:**
                ```
                [Mô hình 1 - Monolithic MVC (Server-Side Rendering)]:
                Trình duyệt ──(GET /Books)──► [ BooksController ──► DbContext ──► Trộn vào View Index.cshtml ]
                            ◄──(Trả về nguyên trang HTML đã vẽ sẵn)──┘

                [Mô hình 2 - Tách rời Frontend & Backend Web API (Chuẩn Doanh nghiệp lớn)]:
                ┌── Web Frontend (Blazor/React) ──┐
                ├── App Mobile (iOS / Android)  ──┼──(Gửi/Nhận JSON qua HTTP)──► [ ASP.NET Core Web API ] ──► [ SQL Server ]
                └── Phần mềm Desktop (WinForms) ──┘
                ```
                • **📊 3. Bảng Tổ Chức Cấu Trúc Solution Chuẩn Cho Dự Án Full Stack Đa Tầng:**
                | Tên Project trong Solution (`.sln`) | Vai trò trong hệ thống Full Stack | Phụ thuộc vào đâu? |
                | :--- | :--- | :--- |
                | **`1. HoangViet.Shared` (`Contracts/DTOs`)**| Chứa các class `DTO`, `Enum`, `ApiResponse<T>` dùng chung cho cả Frontend và Backend! | Độc lập, không phụ thuộc ai. |
                | **`2. HoangViet.BackendApi`** | Chứa `Controllers`, `Services`, `Repositories`, `AppDbContext`, `Migrations`. | Tham chiếu tới `HoangViet.Shared`. |
                | **`3. HoangViet.FrontendClient`** | Chứa giao diện người dùng (`.razor` hoặc `.cshtml` / JS), gọi API sang Backend. | Tham chiếu tới `HoangViet.Shared` (để lấy sẵn DTO không phải gõ lại 2 lần!). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Project Shared chứa DTO dùng chung cho cả 2 đầu",
                        "📥 Đầu vào: Cần truyền BookDto từ Backend sang Frontend (.NET Blazor/MVC). ⚙️ Cơ chế ngầm: Đặt BookDto vào một Class Library tên là 'Shared'. Cả project Backend và Frontend cùng Add Reference tới 'Shared' để dùng chung 100% kiểu dữ liệu! 📤 Đầu ra: Không bao giờ bị lệch tên trường giữa FE và BE.",
                        "// Trong Project HoangViet.Shared:\nnamespace HoangViet.Shared.DTOs;\npublic record BookDto(int Id, string Title, decimal Price, string CategoryName);"),
                    Step(2,
                        "2. Backend Web API trả về dữ liệu theo chuẩn DTO chung",
                        "📥 Đầu vào: Yêu cầu GET /api/books tại Backend. ⚙️ Cơ chế ngầm: Backend truy vấn SQL Server, ánh xạ sang BookDto (của project Shared) và trả về JSON. 📤 Đầu ra: Nguồn cung cấp dữ liệu chuẩn.",
                        "// Trong Project HoangViet.BackendApi:\n[HttpGet]\npublic async Task<ActionResult<List<BookDto>>> GetBooks() {\n    var books = await _bookService.GetAllAsync();\n    return Ok(books);\n}"),
                    Step(3,
                        "3. Cấu hình Solution chạy đồng thời cả Backend và Frontend (Multiple Startup Projects)",
                        "📥 Đầu vào: Solution có 2 project web (Backend cổng 7001 và Frontend cổng 7002). ⚙️ Cơ chế ngầm: Chuột phải vào Solution trong Visual Studio -> Chọn 'Configure Startup Projects...' -> Chọn 'Multiple startup projects' và đặt cả 2 thành 'Start'. 📤 Đầu ra: Bấm F5 một lần chạy cả 2 đầu lên cùng lúc!",
                        "// Trên Visual Studio:\n// Chuột phải vào Solution -> Properties -> Multiple startup projects:\n// 1. HoangViet.BackendApi     -> Action: Start (https://localhost:7001)\n// 2. HoangViet.FrontendClient -> Action: Start (https://localhost:7002)")
                },
                "Trong một Solution .NET Full Stack tách rời giữa project Backend (Web API) và project Frontend (Blazor hoặc MVC Client), việc tạo thêm một project thứ ba là Class Library (Shared) để chứa các class DTO mang lại lợi ích lớn nhất là gì?",
                new List<string>
                {
                    "A. Giúp cả project Backend và project Frontend cùng tham chiếu và sử dụng chung một bộ class DTO duy nhất, không phải copy-paste khai báo lại DTO ở 2 nơi và tránh tuyệt đối lỗi gõ sai tên thuộc tính khi giao tiếp JSON",
                    "B. Giúp không cần cài đặt SQL Server nữa",
                    "C. Giúp trang web tự động đổi màu nền",
                    "D. Vì Visual Studio bắt buộc mọi Solution phải có tối thiểu 3 project"
                },
                "A",
                "Đây là lợi thế cực lớn của hệ sinh thái Full Stack .NET: dùng chung project Shared chứa DTOs và Validators giúp đồng bộ kiểu dữ liệu (Type-safe) tuyệt đối giữa tầng Giao diện và tầng API."
            ),

            // =====================================================================
            // BÀI 12.2
            // =====================================================================
            Build(
                topicId,
                "12.2",
                "Giao tiếp HTTP từ Frontend tới Backend: Làm chủ Typed HttpClient (C#) & Fetch API (JavaScript)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/fundamentals/networking/http/httpclient",
                """
                • **💡 1. Hiểu bản chất cách Frontend gọi sang Backend API trong 30 giây:**
                  - Khi Frontend và Backend tách rời, chúng nói chuyện với nhau qua mạng bằng các gói tin HTTP (`GET`, `POST`, `PUT`, `DELETE`).
                  - Trong .NET, bạn có **2 vũ khí chính** để gọi từ Giao diện sang Web API:
                    1. **Gọi bằng C# (`HttpClient` với `GetFromJsonAsync` / `PostAsJsonAsync`):** Dùng trong Blazor hoặc khi MVC Controller gọi sang Web API. Bạn đưa đối tượng C# vào, nó **tự động dịch sang JSON gửi đi, và tự động dịch JSON trả về thành `List<BookDto>`** chỉ trong 1 dòng lệnh!
                    2. **Gọi bằng JavaScript (`fetch()` với `async/await`):** Dùng trực tiếp bên trong các trang `.cshtml` của dự án MVC để cập nhật dữ liệu động mà không cần tải lại trang!
                • **🖼️ 2. Sơ đồ Luồng Biến Đổi Dữ Liệu Tự Động Của `GetFromJsonAsync` & `PostAsJsonAsync`:**
                ```
                [1. Khi ĐỌC dữ liệu (GET)]:
                Frontend C# gọi: await _http.GetFromJsonAsync<List<BookDto>>("api/books")
                     │
                     └──► Tự gửi GET ──► Nhận chuỗi JSON ──► Tự dịch (Deserialize) ra List<BookDto> cho bạn dùng luôn!

                [2. Khi GỬI dữ liệu tạo mới (POST)]:
                Frontend C# gọi: await _http.PostAsJsonAsync("api/books", newBookDto)
                     │
                     └──► Tự dịch newBookDto sang chuỗi JSON ──► Gắn Header application/json ──► Bắn POST lên API!
                ```
                • **📊 3. Bảng Các Hàm Mở Rộng (`System.Net.Http.Json`) Cực Tiện Lợi Của `HttpClient`:**
                | Thao tác CRUD | Cú pháp gọi cực gọn bằng `HttpClient` trong C# | Cú pháp tương đương bằng `fetch()` trong JS |
                | :--- | :--- | :--- |
                | **1. Đọc (`GET`)** | `var list = await _http.GetFromJsonAsync<List<BookDto>>("api/books");`| `const res = await fetch('/api/books'); const list = await res.json();` |
                | **2. Tạo mới (`POST`)**| `var res = await _http.PostAsJsonAsync("api/books", createDto);` | `await fetch('/api/books', { method: 'POST', headers: {...}, body: JSON.stringify(dto) });`|
                | **3. Cập nhật (`PUT`)**| `var res = await _http.PutAsJsonAsync($"api/books/{id}", updateDto);`| `await fetch(\`/api/books/\${id}\`, { method: 'PUT', ... });` |
                | **4. Xóa (`DELETE`)** | `var res = await _http.DeleteAsync($"api/books/{id}");` | `await fetch(\`/api/books/\${id}\`, { method: 'DELETE' });` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Đăng ký Typed HttpClient kèm địa chỉ BaseAddress ở Frontend",
                        "📥 Đầu vào: Địa chỉ của Backend API (https://localhost:7001/). ⚙️ Cơ chế ngầm: Đăng ký AddHttpClient<IBookApiClient, BookApiClient> với BaseAddress cố định để trong các hàm bên dưới chỉ cần gõ đường dẫn con ngắn gọn 'api/books'. 📤 Đầu ra: Dịch vụ gọi API chuyên nghiệp.",
                        "// Tại Program.cs của Project Frontend:\nbuilder.Services.AddHttpClient<IBookApiClient, BookApiClient>(client => {\n    client.BaseAddress = new Uri(\"https://localhost:7001/\");\n});"),
                    Step(2,
                        "2. Viết hàm gọi GET và POST siêu gọn bằng GetFromJsonAsync / PostAsJsonAsync",
                        "📥 Đầu vào: Class BookApiClient nhận HttpClient qua Constructor. ⚙️ Cơ chế ngầm: Sử dụng thư viện System.Net.Http.Json có sẵn của .NET để gọi và tự động ép kiểu JSON sang DTO. 📤 Đầu ra: Lấy và gửi dữ liệu chỉ với 2 dòng code.",
                        "public class BookApiClient : IBookApiClient {\n    private readonly HttpClient _http;\n    public BookApiClient(HttpClient http) => _http = http;\n\n    public async Task<List<BookDto>> GetBooksAsync() {\n        return await _http.GetFromJsonAsync<List<BookDto>>(\"api/v1/books\") ?? new();\n    }\n\n    public async Task<bool> CreateBookAsync(CreateBookDto dto) {\n        HttpResponseMessage response = await _http.PostAsJsonAsync(\"api/v1/books\", dto);\n        return response.IsSuccessStatusCode; // Trả về true nếu Status 200-299\n    }\n}"),
                    Step(3,
                        "3. Đọc dữ liệu trả về từ Body của một lệnh POST thành công",
                        "📥 Đầu vào: Sau khi gọi PostAsJsonAsync tạo sách mới, Backend trả về đối tượng BookDto vừa tạo (có chứa Id mới). ⚙️ Cơ chế ngầm: Dùng response.Content.ReadFromJsonAsync<BookDto>() để đọc kết quả từ gói tin trả về. 📤 Đầu ra: Lấy được đối tượng vừa tạo.",
                        "var response = await _http.PostAsJsonAsync(\"api/v1/books\", dto);\nif (response.IsSuccessStatusCode) {\n    BookDto? createdBook = await response.Content.ReadFromJsonAsync<BookDto>();\n}")
                },
                "Khi sử dụng HttpClient trong C# (.NET hiện đại), phương thức mở rộng nào dưới đây giúp bạn vừa tự động chuyển đổi đối tượng CreateBookDto sang chuỗi JSON, vừa tự động gắn Header Content-Type: application/json và gửi yêu cầu HTTP POST lên Web API chỉ trong 1 dòng lệnh?",
                new List<string>
                {
                    "A. await _httpClient.PostAsJsonAsync(\"api/books\", createBookDto);",
                    "B. await _httpClient.GetStringAsync(\"api/books\");",
                    "C. await _httpClient.DeleteAsync(\"api/books\");",
                    "D. _httpClient.Dispose();"
                },
                "A",
                "PostAsJsonAsync (nằm trong System.Net.Http.Json) gộp toàn bộ 3 bước: JsonSerializer.Serialize(dto), tạo StringContent với application/json và gọi PostAsync thành một lệnh duy nhất cực kỳ sạch gọn."
            ),

            // =====================================================================
            // BÀI 12.3
            // =====================================================================
            Build(
                topicId,
                "12.3",
                "Luồng Xác thực Full Stack: Lưu trữ JWT trên Client & Tự động đính kèm Token vào mọi Request bằng DelegatingHandler (HTTP Interceptor)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/http-requests#outgoing-request-middleware",
                """
                • **💡 1. Bài toán thực tế khi ghép nối Đăng nhập giữa Frontend và Backend:**
                  - Sau khi người dùng nhập Email + Mật khẩu trên màn hình Frontend và gọi API `POST /api/auth/login`, Backend trả về chuỗi `accessToken (JWT)`.
                  - **2 câu hỏi lớn đặt ra cho lập trình viên Full Stack:**
                    1. *Câu hỏi 1:* Frontend phải cất giữ chuỗi `accessToken` đó ở đâu để khi chuyển sang trang khác không bị mất?
                    2. *Câu hỏi 2:* Dự án có 100 hàm gọi API, chẳng lẽ ở cả 100 hàm bạn đều phải viết tay dòng code lấy Token gắn vào Header `Authorization: Bearer ...`? Làm sao để nó **tự động đính kèm Token vào MỌI cuộc gọi API**?
                • **🖼️ 2. Sơ đồ Cơ Chế "Trạm Gác Tự Động Đính Kèm Token" (`DelegatingHandler` / `HTTP Interceptor`):**
                ```
                [1. Lúc Login thành công] ──► Lưu chuỗi JWT vào bộ nhớ (HttpOnly Cookie / Session / LocalStorage)

                [2. Khi bất kỳ trang nào gọi _http.GetFromJsonAsync("api/books")]:
                     │
                     ▼  Đi qua trạm trung gian tự động: [ AuthHeaderHandler : DelegatingHandler ]
                        ├── Tự móc chuỗi JWT đang lưu ra
                        ├── Tự gắn vào Header: request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token)
                        └── Cho phép gói tin bay tiếp sang Backend API!
                *(Nhờ trạm này, 100 hàm gọi API của bạn không cần viết 1 dòng xử lý Token nào cả!)*
                ```
                • **📊 3. Bảng So Sánh 3 Nơi Lưu Trữ JWT Token Ở Phía Client (Câu hỏi phỏng vấn Full Stack):**
                | Nơi lưu trữ Token | Mức độ an toàn | Đặc điểm & Khi nào dùng? |
                | :--- | :--- | :--- |
                | **1. `HttpOnly Cookie`** | ⭐ **An toàn nhất (Khuyên dùng)** | JavaScript không đọc trộm được (chống XSS 100%), trình duyệt tự quản lý. |
                | **2. `SessionStorage`** | ✅ **Khá an toàn** | Chỉ sống trong tab hiện tại; đóng tab trình duyệt là tự xóa sạch Token. |
                | **3. `LocalStorage`** | ⚠️ **Cần cẩn thận XSS** | Giữ đăng nhập ngay cả khi tắt máy mở lại, nhưng cần đảm bảo web không bị lỗ hổng chèn mã JS (XSS). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo lớp AuthHeaderHandler kế thừa từ DelegatingHandler",
                        "📥 Đầu vào: Mọi HTTP Request chuẩn bị bay từ Frontend sang Backend API. ⚙️ Cơ chế ngầm: Ghi đè hàm SendAsync, tự động đọc chuỗi JWT Token đang lưu và gắn vào Header Authorization: Bearer <token> trước khi gửi đi! 📤 Đầu ra: Tự động hóa 100% việc gắn Token.",
                        "public class AuthHeaderHandler : DelegatingHandler {\n    private readonly ITokenStorageService _tokenStorage;\n    public AuthHeaderHandler(ITokenStorageService tokenStorage) => _tokenStorage = tokenStorage;\n\n    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct) {\n        string? token = await _tokenStorage.GetTokenAsync();\n        if (!string.IsNullOrEmpty(token)) {\n            request.Headers.Authorization = new AuthenticationHeaderValue(\"Bearer\", token);\n        }\n        return await base.SendAsync(request, ct); // Cho Request bay tiếp lên Server!\n    }\n}"),
                    Step(2,
                        "2. Gắn AuthHeaderHandler vào đường ống của HttpClient trong Program.cs",
                        "📥 Đầu vào: Đăng ký AddHttpClient ở Frontend. ⚙️ Cơ chế ngầm: Thêm lệnh .AddHttpMessageHandler<AuthHeaderHandler>() để mọi cuộc gọi của BookApiClient đều tự động chạy qua trạm gắn Token. 📤 Đầu ra: Cấu hình tập trung tại 1 nơi duy nhất.",
                        "builder.Services.AddTransient<AuthHeaderHandler>();\n\nbuilder.Services.AddHttpClient<IBookApiClient, BookApiClient>(client => {\n    client.BaseAddress = new Uri(\"https://localhost:7001/\");\n}).AddHttpMessageHandler<AuthHeaderHandler>(); // Tự động gắn Bearer Token!"),
                    Step(3,
                        "3. Tự động bắt lỗi 401 (Hết hạn Token) ngay trong Handler để gọi Refresh Token",
                        "📥 Đầu vào: Gói tin trả về từ Backend có StatusCode == HttpStatusCode.Unauthorized (401). ⚙️ Cơ chế ngầm: Nếu phát hiện 401, Handler tự động gọi API làm mới Token (Refresh Token) rồi tự gửi lại Request vừa nãy một lần nữa! 📤 Đầu ra: Người dùng không bao giờ bị gián đoạn.",
                        "var response = await base.SendAsync(request, ct);\nif (response.StatusCode == HttpStatusCode.Unauthorized) {\n    // Tự động gọi Refresh Token hoặc chuyển hướng người dùng về trang /Login!\n}\nreturn response;")
                },
                "Khi phát triển tầng Frontend bằng C# (Blazor hoặc MVC gọi sang Web API), việc tạo một class kế thừa từ DelegatingHandler và gắn vào HttpClient thông qua .AddHttpMessageHandler<T>() dùng để giải quyết bài toán gì?",
                new List<string>
                {
                    "A. Đóng vai trò như một trạm trung gian (Interceptor) chặn trước mọi HTTP Request gửi đi để tự động đính kèm Header 'Authorization: Bearer <token>' (và tự xử lý khi gặp lỗi 401), giúp không phải viết code gắn Token thủ công ở từng hàm gọi API",
                    "B. Dùng để thiết kế bảng trong SQL Server",
                    "C. Dùng để thay thế cho HTML và CSS",
                    "D. Dùng để tắt kết nối mạng"
                },
                "A",
                "Nếu như ở phía Backend có Middleware chặn các Request đi vào, thì ở phía Client (HttpClient) có DelegatingHandler chặn các Request đi ra (Outgoing Request Middleware). Đây là kỹ thuật chuẩn Senior để tự động gắn JWT Token và xử lý Refresh Token tập trung."
            ),

            // =====================================================================
            // BÀI 12.4
            // =====================================================================
            Build(
                topicId,
                "12.4",
                "Quản lý Trạng thái Giao diện (State Management): Loading Spinner, Thông báo Toast, Giỏ hàng & Xử lý Lỗi từ API",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/blazor/state-management",
                """
                • **💡 1. Ba trạng thái bắt buộc phải có của mọi màn hình Full Stack chuyên nghiệp:**
                  - Người mới học thường chỉ viết code cho trường hợp "Mọi thứ thành công". Nhưng một màn hình thực tế khi gọi API luôn trải qua **3 trạng thái (UI States)** mà bạn phải hiển thị rõ ràng cho người dùng:
                    1. **`IsLoading = true` (Đang tải dữ liệu):** Ngay khi vừa bấm nút gọi API, phải hiện vòng xoay **Loading Spinner** (hoặc khung xương *Skeleton*) và **khóa nút Submit (`disabled`)** để người dùng không bấm nhầm 5 lần liên tiếp!
                    2. **`Success State` (Thành công):** Tắt Loading, vẽ bảng dữ liệu (hoặc nếu danh sách rỗng `Count == 0` thì hiện hình minh họa *"Chưa có cuốn sách nào"* thay vì để trang trắng trơn!). Hiện thông báo **Toast màu xanh lá** góc màn hình.
                    3. **`Error State` (Thất bại):** Nếu API trả về lỗi `400 / 500` (cấu trúc `ProblemDetails` ở Chủ đề 9), phải đọc câu thông báo `detail` trong JSON và hiện lên cho người dùng biết vì sao lỗi!
                • **🖼️ 2. Sơ đồ Vòng Đời Trạng Thái Giao Diện Khi Người Dùng Bấm Nút Lưu Form:**
                ```
                Người dùng bấm nút [Lưu Sách]
                      │
                      ▼
                1. Bật cờ isSubmitting = true ──► Khóa nút bấm (disabled) + Đổi chữ thành "⏳ Đang lưu..."
                      │
                      ▼  Gọi await BookApi.CreateAsync(dto)
                2. Nhận phản hồi từ Backend API:
                      ├── Nếu Thành công (201 Created):
                      │     ──► Hiện Toast Xanh: "✓ Thêm sách thành công!" ──► Chuyển về danh sách
                      └── Nếu Thất bại (400 Bad Request - ProblemDetails):
                            ──► Đọc lỗi từ JSON ──► Hiện hộp Alert Đỏ báo rõ nguyên nhân!
                      │
                      ▼
                3. Khối finally: Đặt lại isSubmitting = false ──► Mở khóa lại nút bấm!
                ```
                • **📊 3. Quản lý Trạng thái Dùng chung giữa các màn hình (`State Container` / `Cart State`):**
                  - Ví dụ khi người dùng bấm nút *"Thêm sách vào giỏ mượn"* ở danh sách bên dưới, làm sao con số trên **Biểu tượng Giỏ sách ở thanh Header** tự động nhảy từ `0` lên `1` ngay lập tức?
                  - Giải pháp: Tạo một dịch vụ **`CartStateService`** (đăng ký `Scoped`) chứa danh sách sách đang chọn và một sự kiện `public event Action? OnChange;`. Mỗi khi thêm sách, nó phát tín hiệu `OnChange?.Invoke()` để thanh Header tự động cập nhật lại con số!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm soát trạng thái isLoading và khóa nút chống bấm đúp (Double Submit)",
                        "📥 Đầu vào: Nút bấm Lưu thông tin sách. ⚙️ Cơ chế ngầm: Dùng biến bool isSaving. Khi bắt đầu gọi API gán isSaving = true để khóa thuộc tính disabled của nút bấm; trong khối finally gán lại isSaving = false. 📤 Đầu ra: Không bao giờ bị lỗi tạo trùng 2 bản ghi do người dùng click đúp chuột!",
                        "<button class=\"btn btn-primary\" disabled=\"@isSaving\" @onclick=\"HandleSaveAsync\">\n    @if (isSaving) {\n        <span class=\"spinner-border spinner-border-sm me-2\"></span>\n        <span>Đang xử lý...</span>\n    } else {\n        <span>Lưu Sách</span>\n    }\n</button>"),
                    Step(2,
                        "2. Bắt và hiển thị thông báo lỗi chuẩn ProblemDetails từ Backend API",
                        "📥 Đầu vào: Phản hồi lỗi 400 Bad Request từ Backend gửi về. ⚙️ Cơ chế ngầm: Nếu !response.IsSuccessStatusCode, đọc nội dung JSON ra đối tượng ProblemDetails để lấy đúng dòng thông báo tiếng Việt trong thuộc tính .Detail hiển thị lên màn hình. 📤 Đầu ra: Người dùng biết chính xác mình nhập sai ở đâu.",
                        "var response = await _http.PostAsJsonAsync(\"api/v1/books\", dto);\nif (!response.IsSuccessStatusCode) {\n    var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();\n    errorMessage = problem?.Detail ?? \"Có lỗi xảy ra, vui lòng kiểm tra lại!\";\n    return;\n}"),
                    Step(3,
                        "3. Tạo State Container cập nhật số lượng trên Header theo thời gian thực",
                        "📥 Đầu vào: Người dùng bấm chọn mượn sách ở trang con. ⚙️ Cơ chế ngầm: CartStateService thêm sách vào danh sách và kích hoạt sự kiện OnChange?.Invoke() để thanh Header tự vẽ lại số lượng ngay lập tức. 📤 Đầu ra: Đồng bộ trạng thái toàn ứng dụng.",
                        "public class BorrowCartState {\n    public List<BookDto> SelectedBooks { get; } = new();\n    public event Action? OnCartChanged;\n\n    public void AddBook(BookDto book) {\n        if (!SelectedBooks.Any(b => b.Id == book.Id)) {\n            SelectedBooks.Add(book);\n            OnCartChanged?.Invoke(); // Báo cho Header cập nhật số lượng ngay!\n        }\n    }\n}")
                },
                "Khi người dùng mạng yếu bấm vào nút \"Thanh toán / Đặt mượn sách\", vì thấy màn hình chưa phản hồi ngay nên họ sốt ruột bấm liên tiếp 3 lần vào nút đó, dẫn đến hệ thống tạo ra 3 phiếu mượn trùng nhau. Kỹ thuật xử lý giao diện (UI State) chuẩn nhất để ngăn chặn tình trạng này là gì?",
                new List<string>
                {
                    "A. Ngay khi người dùng vừa bấm nút lần đầu tiên, lập tức bật cờ trạng thái (isSubmitting = true) để vô hiệu hóa nút bấm (disabled) và hiển thị biểu tượng xoay Loading cho đến khi API trả kết quả về xong (trong khối finally mới mở lại nút)",
                    "B. Cấm người dùng sử dụng chuột",
                    "C. Xóa nút Thanh toán đi",
                    "D. Bắt người dùng phải giải toán trước khi bấm"
                },
                "A",
                "Vô hiệu hóa nút bấm (disabled) kết hợp hiển thị Spinner Loading ngay khi bắt đầu gửi Request (và mở lại trong khối finally) là tiêu chuẩn UX bắt buộc của mọi ứng dụng Full Stack chuyên nghiệp để chống lỗi Double-Submit."
            ),

            // =====================================================================
            // BÀI 12.5
            // =====================================================================
            Build(
                topicId,
                "12.5",
                "Thực chiến Luồng Tìm kiếm, Lọc đa tiêu chí (Debounce Search) & Phân trang Full Stack (Từ Giao diện xuống tận SQL Server)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/aspnet/core/data/ef-mvc/sort-filter-page",
                """
                • **💡 1. Kỹ thuật `Debounce Search` (Trì hoãn tìm kiếm khi đang gõ phím) là gì mà mọi trang web lớn đều dùng?**
                  - Giả sử bạn làm ô tìm kiếm thông minh: người dùng gõ đến đâu thì gọi API lọc sách đến đó.
                  - Nếu người dùng gõ chữ `"Lập trình C#"` (gồm 12 ký tự trong 1 giây):
                    + **Không có Debounce:** Cứ mỗi 1 phím bấm là trình duyệt bắn 1 Request lên Server ➔ Gõ 1 từ bắn **12 câu lệnh SQL liên tiếp** làm nghẽn mạng và giật màn hình!
                    + **Có `Debounce (300ms)`:** Trong lúc tay người dùng đang gõ liên tục thì **chưa gọi API vội**. Chỉ khi người dùng **ngừng tay nghỉ `300 mili-giây` (`0.3 giây`)**, hệ thống mới bắn **ĐÚNG 1 Request duy nhất** với từ khóa hoàn chỉnh `"Lập trình C#"` lên Server!
                • **🖼️ 2. Sơ đồ Khép Kín Luồng Tìm Kiếm + Lọc + Phân Trang Full Stack:**
                ```
                [1. Giao diện (Frontend)]:
                    Người dùng gõ từ khóa (Debounce 300ms) + Chọn Dropdown Thể loại + Bấm trang số 2
                          │  (Lưu ý vàng: Khi đổi từ khóa tìm kiếm, luôn phải RESET số trang về lại Page = 1!)
                          ▼
                [2. Gọi API]: GET /api/v1/books?keyword=csharp&categoryId=3&pageIndex=1&pageSize=10
                          │
                          ▼
                [3. Backend (Service + EF Core)]:
                    Khởi tạo IQueryable ──► Nối .Where() ──► Đếm TotalCount ──► .Skip().Take() ──► Trả về PagedResult<T>
                          │
                          ▼
                [4. Giao diện vẽ Thanh Phân Trang]:
                    Tính tổng số trang: TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize)
                    ──► Vẽ các nút: [Trước] [1] [2] [3] [Sau]
                ```
                • **📊 3. Cấu trúc Class `PagedResult<T>` Chuẩn Mực Dùng Chung Cho Mọi Bảng:**
                ```csharp
                public class PagedResult<T>
                {
                    public List<T> Items { get; set; } = new(); // Danh sách 10 bản ghi của trang hiện tại
                    public int TotalCount { get; set; }         // Tổng số bản ghi tìm thấy trong DB (VD: 85 cuốn)
                    public int PageIndex { get; set; }          // Trang hiện tại đang xem (VD: Trang 1)
                    public int PageSize { get; set; }           // Số dòng trên 1 trang (VD: 10)
                    // Tự động tính tổng số trang (VD: 85 / 10 = 8.5 -> Làm tròn lên bằng Math.Ceiling thành 9 trang!):
                    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
                    public bool HasPreviousPage => PageIndex > 1;
                    public bool HasNextPage => PageIndex < TotalPages;
                }
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kỹ thuật Debounce 300ms chặn gọi API liên thanh khi gõ phím",
                        "📥 Đầu vào: Người dùng gõ liên tục vào ô tìm kiếm. ⚙️ Cơ chế ngầm: Dùng bộ hẹn giờ (clearTimeout / CancellationTokenSource): nếu người dùng gõ phím tiếp theo trước khi đủ 300ms thì hủy bộ đếm cũ đi đếm lại; khi dừng gõ đủ 300ms mới thực sự gọi API! 📤 Đầu ra: Giảm 90% số lần gọi Server vô ích.",
                        "// Kỹ thuật Debounce siêu gọn trong JavaScript (MVC) hoặc C# (Blazor):\nlet debounceTimer;\nfunction onSearchInput(keyword) {\n    clearTimeout(debounceTimer); // Hủy lệnh gọi cũ nếu vẫn đang gõ tiếp\n    debounceTimer = setTimeout(() => {\n        currentPage = 1; // Đổi từ khóa tìm kiếm thì phải đưa về lại Trang 1!\n        loadBooksFromApi(keyword, currentPage);\n    }, 300); // Chờ ngừng gõ 300ms mới gọi API!\n}"),
                    Step(2,
                        "2. Xử lý Lọc động và Phân trang tại Backend trả về PagedResult<T>",
                        "📥 Đầu vào: keyword, categoryId, pageIndex, pageSize. ⚙️ Cơ chế ngầm: Đếm TotalCount trước khi cắt trang, sau đó dùng Skip/Take lấy đúng dữ liệu của trang hiện tại đóng gói vào PagedResult. 📤 Đầu ra: Phản hồi có đủ cả Items lẫn TotalPages.",
                        "var query = _db.Books.AsNoTracking();\nif (!string.IsNullOrWhiteSpace(keyword))\n    query = query.Where(b => b.Title.Contains(keyword));\n\nint total = await query.CountAsync();\nvar items = await query.OrderByDescending(b => b.Id)\n    .Skip((pageIndex - 1) * pageSize).Take(pageSize)\n    .Select(b => new BookDto(b.Id, b.Title, b.Price)).ToListAsync();\n\nreturn new PagedResult<BookDto> { Items = items, TotalCount = total, PageIndex = pageIndex, PageSize = pageSize };"),
                    Step(3,
                        "3. Vẽ các nút chuyển trang [Trước] [1] [2] [3] [Sau] bằng Bootstrap",
                        "📥 Đầu vào: Đối tượng PagedResult<BookDto> trên giao diện. ⚙️ Cơ chế ngầm: Dùng vòng lặp for từ 1 đến Model.TotalPages để vẽ các nút trang; trang nào trùng với PageIndex hiện tại thì thêm class 'active' màu xanh nổi bật! 📤 Đầu ra: Thanh phân trang chuẩn mực.",
                        "<ul class=\"pagination justify-content-center\">\n    <li class=\"page-item @(Model.HasPreviousPage ? \"\" : \"disabled\")\">\n        <a class=\"page-link\" onclick=\"changePage(@(Model.PageIndex - 1))\">Trước</a>\n    </li>\n    @for (int i = 1; i <= Model.TotalPages; i++) {\n        <li class=\"page-item @(i == Model.PageIndex ? \"active\" : \"\")\">\n            <a class=\"page-link\" onclick=\"changePage(@i)\">@i</a>\n        </li>\n    }\n    <li class=\"page-item @(Model.HasNextPage ? \"\" : \"disabled\")\">\n        <a class=\"page-link\" onclick=\"changePage(@(Model.PageIndex + 1))\">Sau</a>\n    </li>\n</ul>")
                },
                "Trong chức năng kết hợp Tìm kiếm từ khóa và Phân trang, giả sử người dùng đang đứng ở Trang số 5 (pageIndex = 5), sau đó họ gõ một từ khóa mới vào ô tìm kiếm (mà kết quả của từ khóa mới này chỉ có tổng cộng 3 cuốn sách, tức là chỉ có 1 trang). Nếu bạn quên không đặt lại pageIndex = 1 khi người dùng đổi từ khóa tìm kiếm thì chuyện gì sẽ xảy ra?",
                new List<string>
                {
                    "A. API sẽ thực hiện .Skip((5 - 1) * 10) (bỏ qua 40 cuốn đầu), dẫn đến kết quả trả về bị RỖNG (0 cuốn) dù thực tế trong Database có 3 cuốn sách khớp từ khóa đó!",
                    "B. Hệ thống tự động xóa 3 cuốn sách đó",
                    "C. SQL Server bị treo",
                    "D. Không ảnh hưởng gì cả"
                },
                "A",
                "Đây là một lỗi logic thực tế rất hay gặp: khi người dùng thay đổi bất kỳ điều kiện lọc nào (gõ từ khóa mới hoặc chọn lại Dropdown Thể loại), giao diện bắt buộc phải reset pageIndex = 1 trước khi gọi API."
            ),

            // =====================================================================
            // BÀI 12.6
            // =====================================================================
            Build(
                topicId,
                "12.6",
                "Luồng Upload File & Hình ảnh Full Stack: Xem trước ảnh (Image Preview), MultipartFormDataContent và Hiển thị ảnh",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/blazor/file-uploads",
                """
                • **💡 1. Khác biệt cốt lõi khi gửi Form có kèm File ảnh so với gửi JSON thông thường:**
                  - Bình thường khi thêm mới sách chỉ có chữ và số, bạn gửi định dạng **`application/json`**.
                  - Nhưng khi trong Form có thêm **ô chọn File ảnh bìa sách (`<input type="file" />`)**: Định dạng JSON thuần không truyền trực tiếp luồng byte nhị phân của file ảnh hiệu quả được, mà bắt buộc phải đóng gói theo chuẩn **`multipart/form-data`**!
                • **🖼️ 2. Sơ đồ Luồng Upload Ảnh Bìa Sách Full Stack (Từ Lúc Chọn File Đến Khi Lưu DB):**
                ```
                [1. Trên Giao diện (Frontend)]:
                    Người dùng chọn ảnh từ máy tính ──► Kiểm tra đuôi (.jpg, .png) và dung lượng (< 5MB)
                    ──► Hiển thị "Ảnh xem trước (Preview)" ngay trên màn hình để người dùng ngắm trước khi bấm Lưu!
                         │
                         ▼  Đóng gói vào FormData (JS) hoặc MultipartFormDataContent (C#)
                [2. Gửi HTTP POST (Content-Type: multipart/form-data) lên Backend API]
                         │
                         ▼
                [3. Tại Backend Controller ([FromForm] CreateBookWithImageDto)]:
                    ├── Lưu file vật lý vào thư mục: wwwroot/uploads/books/{Guid}_ten_anh.jpg
                    └── Lưu chuỗi đường dẫn "/uploads/books/..." vào cột CoverImageUrl trong SQL Server!
                ```
                • **📊 3. Ba Quy Tắc Bảo Mật Bắt Buộc Khi Làm Chức Năng Upload File:**
                  1. **Chỉ cho phép danh sách đuôi file an toàn (`Whitelist Extensions`):** Chỉ nhận `.jpg`, `.jpeg`, `.png`, `.webp`, `.pdf`. **Tuyệt đối chặn** các đuôi thực thi như `.exe`, `.bat`, `.cs`, `.html`, `.js`!
                  2. **Giới hạn kích thước file (`Max File Size`):** Chặn ngay nếu file ảnh nặng quá `5 MB` (`file.Length > 5 * 1024 * 1024`).
                  3. **Không dùng nguyên tên file gốc của người dùng:** Luôn tạo tên mới bằng `Guid.NewGuid() + Path.GetExtension(file.FileName)` để tránh trùng tên và chống lỗi ký tự đặc biệt!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Hiển thị Ảnh xem trước (Live Preview) ngay khi chọn file trên Giao diện",
                        "📥 Đầu vào: Thẻ <input type=\"file\" accept=\"image/*\" />. ⚙️ Cơ chế ngầm: Dùng URL.createObjectURL(file) để hiển thị ngay bức ảnh người dùng vừa chọn từ ổ cứng lên thẻ <img> chỉ trong 1 mili-giây mà chưa cần gửi lên Server! 📤 Đầu ra: Trải nghiệm người dùng cực kỳ trực quan.",
                        "<input type=\"file\" id=\"coverInput\" accept=\".jpg,.png,.webp\" onchange=\"previewImage(this)\" class=\"form-control\" />\n<img id=\"imgPreview\" src=\"/images/default-book.png\" style=\"max-height: 180px;\" class=\"mt-2 rounded\" />\n\n<script>\nfunction previewImage(input) {\n    if (input.files && input.files[0]) {\n        document.getElementById('imgPreview').src = URL.createObjectURL(input.files[0]);\n    }\n}\n</script>"),
                    Step(2,
                        "2. Đóng gói MultipartFormData để gửi cả Chữ và File lên API",
                        "📥 Đầu vào: Tên sách, Giá tiền và File ảnh bìa. ⚙️ Cơ chế ngầm: Dùng đối tượng FormData (trong JS) hoặc MultipartFormDataContent (trong C# HttpClient) để gom cả các trường văn bản lẫn file nhị phân gửi đi. 📤 Đầu ra: Gói tin multipart/form-data chuẩn.",
                        "// Cách gửi bằng C# HttpClient (Blazor / MVC Client):\nusing var formContent = new MultipartFormDataContent();\nformContent.Add(new StringContent(dto.Title), \"Title\");\nformContent.Add(new StringContent(dto.Price.ToString()), \"Price\");\nformContent.Add(new StreamContent(fileStream), \"CoverImage\", fileName);\n\nvar response = await _http.PostAsync(\"api/v1/books\", formContent);"),
                    Step(3,
                        "3. Backend kiểm tra đuôi file, lưu vào wwwroot và trả về URL ảnh",
                        "📥 Đầu vào: [FromForm] CreateBookFormDto chứa IFormFile CoverImage. ⚙️ Cơ chế ngầm: Kiểm tra đuôi file nằm trong mảng cho phép (.jpg, .png), kiểm tra dung lượng <= 5MB, lưu vào wwwroot/uploads và lưu chuỗi đường dẫn vào DB. 📤 Đầu ra: Hiển thị ảnh lên web qua thẻ <img src=\"@book.CoverUrl\" />.",
                        "var allowedExtensions = new[] { \".jpg\", \".jpeg\", \".png\", \".webp\" };\nstring ext = Path.GetExtension(dto.CoverImage.FileName).ToLowerInvariant();\n\nif (!allowedExtensions.Contains(ext) || dto.CoverImage.Length > 5 * 1024 * 1024)\n    return BadRequest(\"Chỉ chấp nhận ảnh (.jpg, .png, .webp) dung lượng tối đa 5MB!\");\n\nstring safeFileName = $\"{Guid.NewGuid()}{ext}\";\nstring fullPath = Path.Combine(_env.WebRootPath, \"uploads\", safeFileName);\nusing (var stream = new FileStream(fullPath, FileMode.Create)) {\n    await dto.CoverImage.CopyToAsync(stream);\n}")
                },
                "Khi xây dựng tính năng cho phép người dùng upload ảnh đại diện hoặc ảnh bìa sách lên Server, tại sao bạn BẮT BUỘC phải kiểm tra phần mở rộng của file (Path.GetExtension) chỉ nằm trong danh sách cho phép (như .jpg, .png, .webp) và đổi lại tên file bằng Guid.NewGuid()?",
                new List<string>
                {
                    "A. Để ngăn chặn kẻ tấn công tải lên các file mã độc thực thi (.exe, .sh, .html, .aspx) làm chiếm quyền điều khiển máy chủ, đồng thời dùng Guid để đảm bảo không bao giờ bị ghi đè mất ảnh khi 2 người dùng tình cờ tải lên 2 bức ảnh có cùng tên 'avatar.jpg'",
                    "B. Để bức ảnh tự động tăng độ phân giải lên 4K",
                    "C. Vì ổ cứng Windows chỉ lưu được file có tên là Guid",
                    "D. Để giảm giá tiền của cuốn sách"
                },
                "A",
                "Kiểm tra Whitelist đuôi file, giới hạn dung lượng và đổi tên file ngẫu nhiên bằng Guid là 3 nguyên tắc an toàn bắt buộc đối với mọi chức năng Upload File trong thực tế."
            ),

            // =====================================================================
            // BÀI 12.7
            // =====================================================================
            Build(
                topicId,
                "12.7",
                "Giao tiếp Thời gian thực (Real-time Web) với ASP.NET Core SignalR (Thông báo tức thì, Chat & Live Dashboard)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/signalr/introduction",
                """
                • **💡 1. Hiểu bản chất của `SignalR (WebSockets)` trong 30 giây:**
                  - Với **HTTP truyền thống**: Server luôn ở thế bị động — *Trình duyệt không hỏi (không F5 hoặc không gọi API) thì Server không bao giờ tự gửi dữ liệu về cho trình duyệt được!*
                  - Nhưng với các tính năng như **Thông báo tức thì** (*"Thủ thư vừa duyệt phiếu mượn của bạn!"*), **Phòng Chat trực tuyến**, hoặc **Biểu đồ nhảy số trực tiếp**: Ngay khi Thủ thư ở máy tính A bấm nút *"Duyệt"*, màn hình của Sinh viên ở máy tính B đang mở phải **tự động bật lên tiếng chuông thông báo ngay trong `0.1 giây` mà Sinh viên không hề bấm F5 tải lại trang**!
                  - **`ASP.NET Core SignalR`** giải quyết điều đó bằng cách thiết lập một **"Đường ống kết nối 2 chiều thường trực" (`WebSockets`)** giữa Server và tất cả các Trình duyệt đang mở!
                • **🖼️ 2. Sơ đồ Cơ Chế "Trạm Phát Sóng Trung Tâm" (`SignalR Hub`) Trong ASP.NET Core:**
                ```
                Máy Thủ thư (Bấm nút Duyệt phiếu mượn của sinh viên Hoàng Việt)
                     │
                     ▼  Gọi API / Hub trên Backend C#
                ╔═══════════════════════ SIGNALR HUB (NotificationHub : Hub) ═══════════════════════╗
                ║  Server chủ động phát tín hiệu (Push) xuống thẳng trình duyệt qua ống WebSockets: ║
                ║   ├── Clients.All.SendAsync(...)         ──► Phát loa cho TẤT CẢ mọi người đang mở║
                ║   ├── Clients.Group("Admins")...         ──► Chỉ phát cho nhóm Quản trị viên      ║
                ║   └── Clients.User(vietUserId)...        ──► Chỉ phát riêng cho đúng Hoàng Việt!  ║
                ╚═════════════════════════════════════════╤═════════════════════════════════════════╝
                                                          │ (Tốc độ 0.05 giây - Không cần F5!)
                                                          ▼
                Màn hình trình duyệt của Hoàng Việt tự động hiện chuông báo: "🔔 Phiếu mượn đã được duyệt!"
                ```
                • **📊 3. Ba Đối Tượng Gửi Tin Cốt Lõi Trong `Hub` Của SignalR:**
                | Lệnh gửi từ Server (`IHubContext` / `Hub`) | Ai sẽ nhận được tín hiệu ngay lập tức? | Ứng dụng thực tế |
                | :--- | :--- | :--- |
                | **`Clients.All.SendAsync("Sự_Kiện", data)`** | Tất cả mọi trình duyệt đang mở trang web. | Thông báo bảo trì hệ thống, cập nhật số lượng sách tồn kho trực tiếp. |
                | **`Clients.User(userId).SendAsync(...)`** | Chỉ gửi tới các tab trình duyệt của đúng 1 người dùng có `userId` đó. | Thông báo duyệt đơn mượn sách, tin nhắn riêng tư 1-1. |
                | **`Clients.Group("Tên_Nhóm").SendAsync(...)`**| Chỉ gửi tới những người đã tham gia vào nhóm đó (`Groups.AddToGroupAsync`). | Thông báo có đơn hàng mới cho toàn bộ nhóm nhân viên Thủ thư trực ca! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo trạm phát sóng NotificationHub kế thừa từ Hub và đăng ký tại Program.cs",
                        "📥 Đầu vào: Cần tính năng thông báo thời gian thực. ⚙️ Cơ chế ngầm: Tạo class NotificationHub : Hub, đăng ký builder.Services.AddSignalR() và mở đường dẫn kết nối app.MapHub<NotificationHub>(\"/hubs/notification\"). 📤 Đầu ra: Cổng WebSockets sẵn sàng.",
                        "// 1. Tạo class Hub:\npublic class NotificationHub : Hub {\n    // Có thể thêm hàm cho Client gọi lên hoặc dùng IHubContext để bắn từ Service xuống\n}\n\n// 2. Đăng ký trong Program.cs:\nbuilder.Services.AddSignalR();\n// ...\napp.MapHub<NotificationHub>(\"/hubs/notification\");"),
                    Step(2,
                        "2. Bắn thông báo tức thì từ bên trong Service/Controller bằng IHubContext",
                        "📥 Đầu vào: Thủ thư vừa bấm duyệt phiếu mượn sách trong BorrowService. ⚙️ Cơ chế ngầm: Tiêm IHubContext<NotificationHub> vào BorrowService, ngay sau khi SaveChangesAsync() xong thì bắn tín hiệu 'ReceiveNotification' tới đúng sinh viên đó! 📤 Đầu ra: Tín hiệu bay thẳng xuống trình duyệt.",
                        "public class BorrowService {\n    private readonly IHubContext<NotificationHub> _hubContext;\n    // ...\n    public async Task ApproveBorrowAsync(int borrowId, string readerUserId) {\n        // Lưu DB xong -> Bắn thông báo Real-time ngay lập tức:\n        await _hubContext.Clients.User(readerUserId)\n            .SendAsync(\"ReceiveNotification\", \"Phiếu mượn sách của bạn vừa được Thủ thư phê duyệt!\");\n    }\n}"),
                    Step(3,
                        "3. Trình duyệt lắng nghe và bật thông báo ngay khi nhận tín hiệu",
                        "📥 Đầu vào: Thư viện signalr.min.js trên giao diện. ⚙️ Cơ chế ngầm: Mở kết nối tới '/hubs/notification' và lắng nghe sự kiện connection.on(\"ReceiveNotification\", ...). 📤 Đầu ra: Hiện hộp Toast và nhảy số chuông thông báo không cần tải lại trang!",
                        "const connection = new signalR.HubConnectionBuilder()\n    .withUrl(\"/hubs/notification\")\n    .withAutomaticReconnect()\n    .build();\n\nconnection.on(\"ReceiveNotification\", function (message) {\n    showToastSuccess(\"🔔 Thông báo mới: \" + message);\n});\n\nconnection.start();")
                },
                "Khi bạn muốn từ bên trong một hàm của OrderService (hoặc Controller) chủ động đẩy một thông báo thời gian thực (Real-time) xuống trình duyệt của người dùng thông qua một NotificationHub đã tạo, bạn phải tiêm (Inject) giao diện nào của SignalR vào Constructor của OrderService?",
                new List<string>
                {
                    "A. Tiêm giao diện IHubContext<NotificationHub>",
                    "B. Tiêm trực tiếp class NotificationHub",
                    "C. Tiêm giao diện IDisposable",
                    "D. Tiêm giao diện IQueryable"
                },
                "A",
                "Bạn không bao giờ khởi tạo hay tiêm trực tiếp class Hub vào Service/Controller, mà luôn tiêm thông qua giao diện IHubContext<THub> để truy cập vào danh sách Clients và Groups đang kết nối."
            ),

            // =====================================================================
            // BÀI 12.8
            // =====================================================================
            Build(
                topicId,
                "12.8",
                "Tích hợp Biểu đồ Thống kê Trực quan (Chart.js / ApexCharts) kết nối Dữ liệu Thật từ SQL Server",
                "Web API",
                "https://www.chartjs.org/docs/latest/",
                """
                • **💡 1. Hiểu bản chất cách vẽ Biểu đồ Động (Dashboard Charts) trong 30 giây:**
                  - Mọi trang Quản trị (Admin Dashboard) hoặc trang Cá nhân hóa học tập (như trang *Bảng Thống Kê Năng Lực* của dự án *Hoàng Việt* của bạn) đều cần các biểu đồ trực quan: **Biểu đồ Cột (`Bar`)**, **Biểu đồ Đường theo thời gian (`Line`)**, **Biểu đồ Tròn (`Doughnut / Pie`)** và **Biểu đồ Mạng nhện 6 cạnh (`Radar`)**.
                  - Bản chất dữ liệu mà thư viện vẽ biểu đồ (`Chart.js`) cần từ Backend C# chỉ gồm **2 mảng song song nhau**:
                    1. **`Labels` (`string[]` - Mảng nhãn trục ngang):** Ví dụ `["Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11"]`.
                    2. **`Values` (`int[]` hoặc `decimal[]` - Mảng con số tương ứng):** Ví dụ `[12, 25, 40, 68]`.
                • **🖼️ 2. Sơ đồ Luồng Biến Dữ Liệu `GROUP BY` Trong SQL Thành Biểu Đồ `Chart.js` Trên Màn Hình:**
                ```
                [1. SQL Server / EF Core]:
                    _db.SubLessons.Where(x => x.IsCompleted)
                       .GroupBy(x => x.CompletedAt.Value.Date)
                       .Select(g => new { DateLabel = g.Key.ToString("dd/MM"), Count = g.Count() })
                              │
                              ▼  (Tách thành 2 mảng Labels và Data trong ChartDataDto)
                [2. JSON trả về cho Giao diện]:
                    {
                      "labels": ["20/10", "22/10", "25/10", "26/10"],
                      "data":   [   3   ,    5   ,    8   ,   12   ]
                    }
                              │
                              ▼  (Đổ vào cấu hình new Chart(ctx, { type: 'line', ... }))
                [3. Trình duyệt vẽ Biểu đồ Đường / Biểu đồ Radar chuyển động mượt mà!]
                ```
                • **📊 3. Bảng Chọn Đúng Loại Biểu Đồ (`type`) Trong `Chart.js` Cho Từng Bài Toán:**
                | Loại Biểu đồ (`type`) | Khi nào sử dụng trong Đồ án & Thực tế? |
                | :--- | :--- |
                | **`'line'` (Biểu đồ Đường)** | Thể hiện **xu hướng tiến độ theo thời gian** (Số bài học hoàn thành theo từng ngày, Doanh thu 12 tháng). |
                | **`'radar'` (Biểu đồ Mạng nhện)**| Thể hiện **đa giác năng lực cá nhân** (6 kỹ năng: `Tư duy luồng`, `C#`, `OOP`, `SQL/EF`, `Web API`, `Testing`). |
                | **`'bar'` (Biểu đồ Cột)** | So sánh số lượng giữa các danh mục (Số đầu sách của từng Thể loại). |
                | **`'doughnut'` (Biểu đồ Vòng tròn)**| Thể hiện tỷ lệ phần trăm hoàn thành (`Đã học 45%` vs `Chưa học 55%`). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo DTO chuẩn cho Biểu đồ và Truy vấn Gom nhóm bằng EF Core",
                        "📥 Đầu vào: Cần thống kê số lượng sách theo từng Thể loại để vẽ biểu đồ. ⚙️ Cơ chế ngầm: Dùng GroupBy hoặc Select đếm số sách trong mỗi Category, sau đó tách ra mảng Labels (tên thể loại) và mảng Values (số lượng). 📤 Đầu ra: ChartDataDto chuẩn chỉnh.",
                        "public record ChartDataDto(List<string> Labels, List<int> Values);\n\n// Tại Service:\nvar rawData = await _db.Categories.AsNoTracking()\n    .Select(c => new { c.Name, Count = c.Books.Count() })\n    .ToListAsync();\n\nvar chartDto = new ChartDataDto(\n    rawData.Select(x => x.Name).ToList(),\n    rawData.Select(x => x.Count).ToList()\n);"),
                    Step(2,
                        "2. Đặt thẻ <canvas> trên Giao diện và gọi API lấy dữ liệu",
                        "📥 Đầu vào: Thẻ <canvas id=\"myChart\"></canvas> trong file .cshtml / .razor. ⚙️ Cơ chế ngầm: Dùng hàm fetch() gọi lên Action lấy ChartDataDto về. 📤 Đầu ra: Dữ liệu JSON sẵn sàng để vẽ.",
                        "<div class=\"card p-3\">\n    <canvas id=\"categoryChart\" height=\"250\"></canvas>\n</div>\n\n<script src=\"https://cdn.jsdelivr.net/npm/chart.js\"></script>"),
                    Step(3,
                        "3. Khởi tạo đối tượng Chart.js hiển thị biểu đồ sống động",
                        "📥 Đầu vào: Dữ liệu labels và values từ Bước 1 & 2. ⚙️ Cơ chế ngầm: Truyền trực tiếp response.labels và response.values vào datasets của Chart.js. 📤 Đầu ra: Biểu đồ tự động cập nhật theo dữ liệu thật trong SQL Server!",
                        "async function renderChart() {\n    const res = await fetch('/Admin/GetCategoryChartData');\n    const chartData = await res.json();\n\n    new Chart(document.getElementById('categoryChart'), {\n        type: 'bar',\n        data: {\n            labels: chartData.labels,\n            datasets: [{\n                label: 'Số lượng đầu sách',\n                data: chartData.values,\n                backgroundColor: '#3B82F6',\n                borderRadius: 6\n            }]\n        }\n    });\n}")
                },
                "Khi sử dụng thư viện Chart.js trên giao diện để vẽ biểu đồ thống kê lấy dữ liệu động từ Backend ASP.NET Core, cấu trúc dữ liệu JSON tối giản và chuẩn nhất mà Backend nên trả về để khớp ngay vào thuộc tính data của Chart.js gồm 2 thành phần nào?",
                new List<string>
                {
                    "A. Gồm một mảng chuỗi 'labels' (chứa tên các mốc thời gian hoặc tên danh mục) và một mảng số 'values / data' (chứa các con số thống kê tương ứng với từng nhãn)",
                    "B. Trả về một file ảnh .bmp nặng 50MB",
                    "C. Trả về toàn bộ các bảng thô chưa qua GroupBy trong Database",
                    "D. Chỉ trả về một biến bool true/false"
                },
                "A",
                "Mọi loại biểu đồ trong Chart.js (line, bar, radar, doughnut) đều hoạt động dựa trên sự ánh xạ 1-1 giữa mảng labels (nhãn hiển thị) và mảng datasets.data (giá trị số tương ứng)."
            ),

            // =====================================================================
            // BÀI 12.9
            // =====================================================================
            Build(
                topicId,
                "12.9",
                "Xuất Báo cáo File Excel (.xlsx) & Hóa đơn/Phiếu mượn PDF (ClosedXML / EPPlus & QuestPDF) trong .NET",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/api/microsoft.aspnetcore.mvc.controllerbase.file",
                """
                • **💡 1. Hai tính năng "Ăn điểm tuyệt đối" trong mọi Đồ án Tốt nghiệp & Dự án Thực tế:**
                  Khi đi bảo vệ đồ án hoặc làm hệ thống quản lý cho doanh nghiệp, giảng viên và khách hàng luôn hỏi 2 nút bấm:
                  1. **Nút *"Xuất danh sách ra Excel (`.xlsx`)"*:** Để kế toán/thủ thư tải bảng thống kê về máy tính lọc, in ấn.
                     - *Thư viện chuẩn khuyên dùng:* **`ClosedXML`** (miễn phí hoàn toàn, cú pháp dễ như viết C#) hoặc `EPPlus`.
                  2. **Nút *"In Phiếu mượn / Xuất Hóa đơn PDF (`.pdf`)"*:** Để in ra giấy có đóng khung, logo công ty giao cho khách hàng.
                     - *Thư viện chuẩn hiện đại nhất:* **`QuestPDF`** (thiết kế bố cục PDF bằng code C# Fluent API cực nhanh và đẹp!).
                • **🖼️ 2. Sơ đồ Luồng Tạo File Trên RAM (`MemoryStream`) Và Trả Về Cho Trình Duyệt Tải Xuống (`FileResult`):**
                ```
                Người dùng bấm nút [📥 Xuất Báo Cáo Excel / PDF]
                     │
                     ▼
                1. Controller gọi Service lấy danh sách dữ liệu từ SQL Server (AsNoTracking)
                     │
                     ▼
                2. Dùng ClosedXML (Excel) hoặc QuestPDF (PDF) vẽ bảng trực tiếp vào bộ nhớ RAM (MemoryStream)
                   *(Tuyệt đối KHÔNG cần lưu thành file rác trên ổ cứng của Server!)*
                     │
                     ▼
                3. Controller trả về bằng hàm:
                   return File(stream.ToArray(), "application/vnd.openxmlformats...", "BaoCaoSach_2026.xlsx");
                     │
                     ▼
                4. Trình duyệt tự động bật hộp thoại tải file "BaoCaoSach_2026.xlsx" về máy người dùng ngay lập tức!
                ```
                • **📊 3. Bảng Mã Định Dạng `Content-Type (MIME Type)` Chuẩn Khi Trả Về File (`return File(...)`):**
                | Loại File trả về | Chuỗi `Content-Type` truyền vào hàm `File(bytes, contentType, fileName)` | Đuôi file |
                | :--- | :--- | :--- |
                | **File Excel hiện đại**| **`"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"`** | `.xlsx` |
                | **File Tài liệu PDF** | **`"application/pdf"`** | `.pdf` |
                | **File CSV nhẹ** | **`"text/csv"`** | `.csv` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Xuất danh sách ra File Excel (.xlsx) trên RAM bằng ClosedXML",
                        "📥 Đầu vào: Danh sách List<BookDto> từ Database. ⚙️ Cơ chế ngầm: Khởi tạo XLWorkbook, tạo trang tính Worksheet, điền dòng tiêu đề in đậm và đổ dữ liệu vào từng ô, sau đó lưu ra MemoryStream. 📤 Đầu ra: Mảng byte[] của file Excel.",
                        "using var workbook = new ClosedXML.Excel.XLWorkbook();\nvar ws = workbook.Worksheets.Add(\"DanhSachSach\");\n\n// Tạo dòng tiêu đề:\nws.Cell(1, 1).Value = \"Mã Sách\";\nws.Cell(1, 2).Value = \"Tên Sách\";\nws.Cell(1, 3).Value = \"Giá Tiền\";\nws.Range(\"A1:C1\").Style.Font.Bold = true;\n\n// Đổ dữ liệu từ dòng số 2:\nint row = 2;\nforeach (var item in books) {\n    ws.Cell(row, 1).Value = item.Id;\n    ws.Cell(row, 2).Value = item.Title;\n    ws.Cell(row, 3).Value = item.Price;\n    row++;\n}\nws.Columns().AdjustToContents(); // Tự động căn vừa độ rộng cột!"),
                    Step(2,
                        "2. Xuất Hóa đơn / Phiếu mượn ra File PDF bằng QuestPDF",
                        "📥 Đầu vào: Thông tin chi tiết Phiếu mượn (BorrowDto). ⚙️ Cơ chế ngầm: Dùng cú pháp Fluent API của QuestPDF thiết kế Header, Bảng chi tiết sách và Footer chữ ký, rồi gọi GeneratePdf() trả thẳng ra mảng byte[]. 📤 Đầu ra: File PDF sắc nét chuẩn in ấn.",
                        "byte[] pdfBytes = QuestPDF.Fluent.Document.Create(container => {\n    container.Page(page => {\n        page.Margin(40);\n        page.Header().Text($\"PHIẾU MƯỢN SÁCH #{borrow.Id}\").FontSize(20).Bold();\n        page.Content().PaddingVertical(20).Text($\"Độc giả: {borrow.ReaderName}\");\n        page.Footer().AlignCenter().Text(\"Thư viện Đại học UNETI - 2026\");\n    });\n}).GeneratePdf();"),
                    Step(3,
                        "3. Trả File từ Controller về Trình duyệt bằng return File(...)",
                        "📥 Đầu vào: MemoryStream (Excel) hoặc mảng pdfBytes (PDF). ⚙️ Cơ chế ngầm: Hàm File() của ControllerBase tự động gắn Header Content-Disposition: attachment kèm tên file để trình duyệt tải xuống ngay lập tức. 📤 Đầu ra: Tải file hoàn tất.",
                        "[HttpGet(\"export-excel\")]\npublic async Task<IActionResult> ExportExcel() {\n    using var stream = new MemoryStream();\n    workbook.SaveAs(stream);\n    string contentType = \"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet\";\n    return File(stream.ToArray(), contentType, $\"BaoCaoSach_{DateTime.Now:yyyyMMdd}.xlsx\");\n}")
                },
                "Khi viết chức năng xuất báo cáo Excel hoặc PDF trong ASP.NET Core Controller, tại sao các lập trình viên chuyên nghiệp luôn kết xuất nội dung file vào một đối tượng MemoryStream (trên RAM) rồi gọi return File(stream.ToArray(), contentType, fileName); thay vì lưu thành một file vật lý trên ổ cứng của Server?",
                new List<string>
                {
                    "A. Vì tạo trực tiếp trên RAM (MemoryStream) rồi truyền thẳng về trình duyệt có tốc độ cực nhanh, không cần cấp quyền ghi ổ cứng trên Server và không để lại hàng nghìn file rác làm đầy ổ cứng máy chủ sau mỗi lần người dùng bấm tải báo cáo",
                    "B. Vì C# không cho phép lưu file Excel vào ổ cứng",
                    "C. Vì MemoryStream tự động dịch tiếng Việt sang tiếng Anh",
                    "D. Vì trình duyệt không tải được file từ ổ cứng"
                },
                "A",
                "Các file báo cáo xuất theo yêu cầu tức thời (On-demand Export) chỉ cần truyền về cho người dùng tải xuống rồi giải phóng ngay. Sử dụng MemoryStream giúp luồng xuất file hoàn toàn sạch sẽ, không phát sinh rác trên đĩa cứng của Web Server."
            ),

            // =====================================================================
            // BÀI 12.10
            // =====================================================================
            Build(
                topicId,
                "12.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 12 (Checklist Ghép nối Hoàn chỉnh 1 Tính năng Full Stack từ A-Z)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/",
                """
                • **💡 1. Bản Đồ 7 Bước Khi Bạn Tự Tay Xây Dựng 1 Tính Năng Full Stack Hoàn Chỉnh (Từ DB Lên Tới Màn Hình):**
                  Khi làm bất kỳ chức năng mới nào (ví dụ: *"Quản lý Phiếu Mượn Trả Sách"* hoặc *"Hệ thống Học Tập Hoàng Việt"*), hãy đi tuần tự theo **7 bước chuẩn công nghiệp** này để không bao giờ bị rối:
                  ```
                  Bước 1 [Database & Entity]  : Thiết kế class Entity, cấu hình Fluent API (PK, FK, Index, DeleteBehavior) -> Chạy Migration.
                  Bước 2 [Shared DTOs]        : Tạo Record/Class DTO cho Request (có Validation) và Response (kèm PagedResult<T>).
                  Bước 3 [Repository & UoW]   : Viết truy vấn EF Core tối ưu (.AsNoTracking, .Include/.Select, .Skip/.Take).
                  Bước 4 [Service Layer]      : Viết logic nghiệp vụ, Guard Clauses, Caching, bắn thông báo SignalR, xuất Excel/PDF.
                  Bước 5 [Web API Controller] : Mở Endpoint chuẩn RESTful, gắn [Authorize], trả đúng Status Code (200, 201, 400, 404).
                  Bước 6 [Frontend API Client]: Cấu hình HttpClient + AuthHeaderHandler (hoặc Fetch JS) gọi sang Backend.
                  Bước 7 [Giao diện UI/UX]    : Dựng trang Bootstrap 5, xử lý 3 trạng thái (Loading, Empty, Error), Debounce Search & Chart.js!
                  ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tầng Backend: Mở API Endpoint chuẩn mực có Bảo mật & Phân trang",
                        "📥 Đầu vào: Yêu cầu từ phía Client. ⚙️ Cơ chế ngầm: Xác thực JWT, nhận tham số lọc, gọi Service lấy PagedResult và trả về JSON đã nén Brotli. 📤 Đầu ra: Nguồn dữ liệu tốc độ cao.",
                        "[Authorize]\n[HttpGet(\"api/v1/borrows\")]\npublic async Task<ActionResult<PagedResult<BorrowDto>>> GetBorrows([FromQuery] BorrowFilterRequest req, CancellationToken ct) {\n    return Ok(await _borrowService.GetPagedAsync(req, ct));\n}"),
                    Step(2,
                        "2. Tầng Giao tiếp: Frontend gọi API qua Typed HttpClient (Kèm Token tự động)",
                        "📥 Đầu vào: Người dùng mở trang Danh sách phiếu mượn. ⚙️ Cơ chế ngầm: AuthHeaderHandler tự động gắn Bearer Token vào Request, gọi GetFromJsonAsync lấy dữ liệu về. 📤 Đầu ra: Đối tượng PagedResult<BorrowDto> trên giao diện.",
                        "public async Task<PagedResult<BorrowDto>?> GetBorrowsAsync(string keyword, int page) {\n    string url = $\"api/v1/borrows?keyword={Uri.EscapeDataString(keyword)}&pageIndex={page}\";\n    return await _http.GetFromJsonAsync<PagedResult<BorrowDto>>(url);\n}"),
                    Step(3,
                        "3. Tầng Giao diện: Hiển thị Loading, Bảng dữ liệu, Phân trang & Nút Xuất Excel",
                        "📥 Đầu vào: Dữ liệu PagedResult từ Bước 2. ⚙️ Cơ chế ngầm: Kiểm tra cờ isLoading, vẽ bảng Bootstrap, thanh phân trang và nút tải báo cáo Excel/PDF. 📤 Đầu ra: Màn hình Full Stack hoàn thiện.",
                        "@if (isLoading) {\n    <div class=\"spinner-border text-primary\"></div>\n} else if (pagedData.Items.Count == 0) {\n    <div class=\"alert alert-info\">Không tìm thấy phiếu mượn nào phù hợp!</div>\n} else {\n    <!-- Vẽ bảng dữ liệu và thanh Pagination -->\n}"),
                    Step(4,
                        "4. Tầng Tương tác Thời gian thực: Cập nhật thông báo qua SignalR",
                        "📥 Đầu vào: Sự kiện duyệt phiếu mượn hoặc hoàn thành bài học. ⚙️ Cơ chế ngầm: Bắn thông báo qua SignalR Hub và cập nhật lại Biểu đồ Chart.js ngay lập tức. 📤 Đầu ra: Hệ thống Full Stack hiện đại đạt điểm A+!",
                        "// Kết nối khép kín: UI ➔ API ➔ Service ➔ DB ➔ SignalR Push ➔ UI & Chart.js!")
                },
                "(Câu hỏi tổng hợp Chủ đề 12) Khi ghép nối chuỗi tìm kiếm tiếng Việt hoặc ký tự đặc biệt (ví dụ keyword = \"C# & .NET\") vào đường dẫn URL để gọi API GET /api/books?keyword=..., tại sao ở phía Frontend chúng ta phải dùng hàm Uri.EscapeDataString(keyword) (trong C#) hoặc encodeURIComponent(keyword) (trong JavaScript)?",
                new List<string>
                {
                    "A. Vì các ký tự đặc biệt như dấu '#' (Anchor) hay dấu '&' (Ngăn cách tham số) nếu để trần trên URL sẽ làm đứt gãy đường dẫn truy vấn khiến Backend nhận thiếu dữ liệu (chỉ nhận được mỗi chữ 'C'); hàm mã hóa URL giúp chuyển chúng thành '%23' và '%26' để truyền nguyên vẹn lên Server",
                    "B. Để tự động xóa các ký tự chữ cái",
                    "C. Để dịch từ khóa sang tiếng Nhật",
                    "D. Vì HTTP GET không cho phép truyền chữ C"
                },
                "A",
                "Đây là một lỗi thực tế kinh điển khi tìm kiếm từ khóa \"C#\": trên thanh địa chỉ URL, dấu # mang ý nghĩa đánh dấu vị trí trang (Fragment) nên toàn bộ phần từ dấu # trở về sau sẽ bị cắt bỏ không gửi lên Server! Dùng Uri.EscapeDataString(\"C#\") sẽ đổi nó thành \"C%23\", giúp Backend nhận chính xác 100% chữ \"C#\"."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (12.1 -> 12.10) cho Chủ đề 12 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic12 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 12);
        if (topic12 == null)
            return;

        topic12.Title = "Chủ đề 12: Phát triển ứng dụng Web Full Stack";
        topic12.Description = "10 Bài học con siêu chi tiết từ Kiến trúc Monolithic MVC vs Decoupled Frontend-Backend, Typed HttpClient & Fetch API, DelegatingHandler tự gắn JWT, State Management (Loading/Toast/Cart), Debounce Search & Phân trang, Upload File (Multipart), SignalR Real-time, Chart.js Dashboard, Xuất Excel/PDF (ClosedXML/QuestPDF) đến Checklist ghép nối Full Stack từ A-Z.";

        var targetLessons = GetLessons(topic12.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic12.Id)
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
