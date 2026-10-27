using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic9Seed
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
            // BÀI 9.1
            // =====================================================================
            Build(
                topicId,
                "9.1",
                "Bản chất Kiến trúc RESTful API, Quy chuẩn đặt tên Endpoint (URI) & Tính chất Idempotency",
                "Web API",
                "https://learn.microsoft.com/vi-vn/azure/architecture/best-practices/api-design",
                """
                • **💡 1. Hiểu bản chất của `RESTful API` trong 30 giây:**
                  - **API (Application Programming Interface):** Là "Người bồi bàn" đứng giữa nhận thực đơn từ Khách hàng (Web React / Blazor / App Mobile / Postman) đem vào Bếp (Backend C# + SQL Server) và bưng món ăn (dữ liệu **JSON**) ra trả.
                  - **REST (Representational State Transfer):** Không phải là một thư viện code, mà là **Bộ quy tắc thiết kế chuẩn quốc tế** quy định cách đặt tên đường dẫn (URI) và cách dùng các phương thức HTTP (`GET`, `POST`, `PUT`, `DELETE`) sao cho bất kỳ lập trình viên nào trên thế giới nhìn vào cũng hiểu ngay không cần giải thích!
                • **🖼️ 2. Sơ đồ Quy Chuẩn Đặt Tên Đường Dẫn (URI) Trong RESTful API (Dùng Danh từ số nhiều, CẤM dùng Động từ!):**
                ```
                ❌ CÁCH ĐẶT TÊN SAI CHUẨN REST (Nhét động từ vào URL):
                   GET  /api/getAllBooks
                   POST /api/createNewBook
                   POST /api/deleteBookById/5

                ✅ CÁCH ĐẶT TÊN ĐÚNG CHUẨN RESTFUL (URL chỉ dùng DANH TỪ SỐ NHIỀU, Hành động nằm ở HTTP Method!):
                   GET    /api/books          ──► Lấy danh sách sách
                   GET    /api/books/5        ──► Lấy chi tiết cuốn sách có Id = 5
                   POST   /api/books          ──► Tạo mới 1 cuốn sách
                   PUT    /api/books/5        ──► Cập nhật toàn bộ cuốn sách số 5
                   PATCH  /api/books/5        ──► Cập nhật một phần nhỏ (VD: chỉ đổi giá) cuốn sách số 5
                   DELETE /api/books/5        ──► Xóa cuốn sách số 5
                   GET    /api/books/5/authors──► Tài nguyên lồng nhau: Lấy tác giả của cuốn sách số 5
                ```
                • **📊 3. Khái niệm Phỏng vấn Quan trọng: Tính chất `Idempotent` (Lũy đẳng) của các HTTP Methods:**
                  *Một phương thức gọi là **Idempotent** nếu người dùng lỡ bấm nút gọi lặp lại 1 lần hay 10 lần liên tiếp với cùng 1 dữ liệu thì trạng thái trong Database vẫn giữ nguyên như chỉ gọi 1 lần!*
                | Phương thức | Ý nghĩa hành động | Có tính `Idempotent` không? | Giải thích bản chất |
                | :--- | :--- | :--- | :--- |
                | **`GET`** | Đọc dữ liệu | ✅ **CÓ** (An toàn tuyệt đối) | Đọc 100 lần cũng không làm thay đổi dữ liệu trong DB. |
                | **`PUT`** | Ghi đè toàn bộ bản ghi | ✅ **CÓ** | Gán `Price = 50000` dù chạy 1 lần hay 10 lần thì giá cuối cùng vẫn là `50000`. |
                | **`DELETE`** | Xóa bản ghi | ✅ **CÓ** | Lần 1 xóa mất bản ghi, 9 lần sau trả về `404` nhưng kết quả trong DB vẫn là bản ghi đó đã biến mất. |
                | **`POST`** | Tạo mới bản ghi / Thanh toán| ❌ **KHÔNG!** | ⚠️ Bấm `POST /api/orders` 5 lần sẽ đẻ ra **5 đơn hàng trùng nhau** và trừ tiền 5 lần! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết kế Controller tuân thủ chuẩn danh từ số nhiều",
                        "📥 Đầu vào: Quản lý tài nguyên Độc giả (Reader). ⚙️ Cơ chế ngầm: Đặt tên class là ReadersController (số nhiều) kết hợp [Route(\"api/[controller]\")] để tự động sinh URI chuẩn '/api/readers'. 📤 Đầu ra: Tiền tố URL chuẩn REST.",
                        "[ApiController]\n[Route(\"api/[controller]\")] // Tự động ánh xạ thành: /api/readers\npublic class ReadersController : ControllerBase\n{\n}"),
                    Step(2,
                        "2. Phân biệt hành động bằng HTTP Verb thay vì đổi tên URL",
                        "📥 Đầu vào: Cùng một đường dẫn gốc '/api/readers'. ⚙️ Cơ chế ngầm: Dùng [HttpGet] cho thao tác đọc và [HttpPost] cho thao tác thêm mới trên cùng một URL. 📤 Đầu ra: Bộ API sạch sẽ, nhất quán.",
                        "[HttpGet] // GET /api/readers\npublic async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());\n\n[HttpPost] // POST /api/readers\npublic async Task<IActionResult> Create(CreateReaderDto dto) => Ok(await _service.CreateAsync(dto));"),
                    Step(3,
                        "3. Biểu diễn quan hệ Cha - Con qua Sub-resource URI",
                        "📥 Đầu vào: Lấy danh sách các phiếu mượn của một độc giả cụ thể có mã readerId. ⚙️ Cơ chế ngầm: Thiết kế Route dạng '/api/readers/{readerId}/borrows' để thể hiện rõ phân cấp tài nguyên. 📤 Đầu ra: Đọc URL hiểu ngay mối quan hệ.",
                        "[HttpGet(\"{readerId:int}/borrows\")] // GET /api/readers/10/borrows\npublic async Task<IActionResult> GetBorrowsByReader(int readerId) {\n    return Ok(await _borrowService.GetByReaderIdAsync(readerId));\n}")
                },
                "Theo tiêu chuẩn thiết kế RESTful API, đường dẫn (URI) và phương thức HTTP nào dưới đây là ĐÚNG CHUẨN NHẤT cho chức năng \"Xóa cuốn sách có mã ID bằng 15\"?",
                new List<string>
                {
                    "A. Gọi phương thức GET tới đường dẫn /api/books/deleteBook/15",
                    "B. Gọi phương thức DELETE tới đường dẫn /api/books/15 (Sử dụng danh từ số nhiều kết hợp HTTP Method DELETE)",
                    "C. Gọi phương thức POST tới đường dẫn /api/remove-book?id=15",
                    "D. Gọi phương thức PUT tới đường dẫn /api/delete/15"
                },
                "B",
                "Chuẩn RESTful quy định URI chỉ dùng danh từ số nhiều đại diện cho tài nguyên (/api/books/15), còn hành động xóa được biểu đạt thông qua phương thức HTTP DELETE."
            ),

            // =====================================================================
            // BÀI 9.2
            // =====================================================================
            Build(
                topicId,
                "9.2",
                "Giải phẫu Attribute [ApiController], Cơ chế Content Negotiation & Cấu hình JSON Serializer (CamelCase, ReferenceHandler)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/web-api/",
                """
                • **💡 1. Sức mạnh ngầm của nhãn `[ApiController]` đặt trên đầu Class:**
                  - Nhiều bạn viết `[ApiController]` theo thói quen mà không biết nếu bỏ nhãn này đi, Web API sẽ mất đi **4 siêu năng lực tự động** sau:
                    1. **Tự động kiểm tra `ModelState.IsValid`:** Nếu DTO vi phạm `[Required]`, nó tự trả về lỗi `400 Bad Request` kèm danh sách lỗi chuẩn `ProblemDetails` mà bạn không cần viết `if (!ModelState.IsValid)`!
                    2. **Tự động suy luận nguồn dữ liệu (`Binding Source Inference`):** Tự hiểu class `DTO` phức tạp lấy từ `[FromBody]`, còn biến `int id` đơn giản lấy từ `[FromRoute]` / `[FromQuery]`.
                    3. **Bắt buộc dùng Attribute Routing (`[Route(...)]`).**
                • **🖼️ 2. Cơ chế `Content Negotiation` (Đàm phán định dạng dữ liệu) & 2 Lỗi JSON Kinh Điển:**
                ```
                Trong C# bạn viết hoa chữ đầu (PascalCase):   public string FullName { get; set; }
                Khi trả ra JSON, ASP.NET Core tự đổi thành chữ thường (camelCase): { "fullName": "Hoàng Việt" }

                ⚠️ LỖI KINH ĐIỂN KHI TRẢ ENTITY CÓ LIÊN KẾT BẢNG RA JSON (Vòng lặp vô tận - Object Cycle!):
                Book ──(chứa)──► Category ──(lại chứa danh sách)──► Books ──(lại chứa)──► Category...
                ──► Nổ lỗi 500: "System.Text.Json.JsonException: A possible object cycle was detected!"
                ──► Giải pháp: Dùng DTO (Tốt nhất) HOẶC cấu hình ReferenceHandler.IgnoreCycles trong Program.cs!
                ```
                • **📊 3. Bảng Các Cấu Hình `AddControllers().AddJsonOptions(...)` Quan Trọng Trong `Program.cs`:**
                | Cấu hình trong `JsonSerializerOptions` | Tác dụng thực tế |
                | :--- | :--- |
                | **`ReferenceHandler.IgnoreCycles`** | Tự động cắt đứt vòng lặp vô tận khi 2 bảng trỏ qua lại lẫn nhau, ngăn chặn lỗi sập JSON! |
                | **`JsonStringEnumConverter()`** | Mặc định `Enum` trả ra JSON là con số `0, 1, 2`. Thêm cấu hình này giúp trả ra chữ rõ nghĩa `"Pending"`, `"Completed"`! |
                | **`DefaultIgnoreCondition.WhenWritingNull`**| Tự động ẩn bỏ các thuộc tính đang bị `null` khỏi chuỗi JSON trả về cho gọn nhẹ! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kích hoạt [ApiController] trên lớp kế thừa ControllerBase",
                        "📥 Đầu vào: Một class Controller mới. ⚙️ Cơ chế ngầm: Kế thừa ControllerBase (nhẹ hơn Controller của MVC vì bỏ phần Razor View) và gắn [ApiController] để bật tự động kiểm tra lỗi Validation 400. 📤 Đầu ra: Chuẩn Web API Controller.",
                        "[ApiController]\n[Route(\"api/[controller]\")]\npublic class BooksController : ControllerBase\n{\n    // Không cần viết if (!ModelState.IsValid) return BadRequest(); nữa!\n}"),
                    Step(2,
                        "2. Cấu hình chống lỗi vòng lặp JSON (IgnoreCycles) trong Program.cs",
                        "📥 Đầu vào: Cấu hình dịch vụ AddControllers() tại Program.cs. ⚙️ Cơ chế ngầm: Thiết lập ReferenceHandler.IgnoreCycles cho bộ chuyển đổi System.Text.Json. 📤 Đầu ra: Không bao giờ bị sập lỗi vòng lặp khi Include bảng con.",
                        "builder.Services.AddControllers()\n    .AddJsonOptions(options => {\n        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;\n    });"),
                    Step(3,
                        "3. Cấu hình hiển thị Enum dạng chữ và bỏ qua trường Null",
                        "📥 Đầu vào: Muốn JSON trả về sạch đẹp, dễ đọc cho Frontend. ⚙️ Cơ chế ngầm: Thêm JsonStringEnumConverter để biến số Enum thành tên chữ. 📤 Đầu ra: JSON chuẩn mực quốc tế.",
                        "builder.Services.AddControllers()\n    .AddJsonOptions(options => {\n        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());\n        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;\n    });")
                },
                "Khi gọi một API có sử dụng .Include(b => b.Category) và trả về trực tiếp đối tượng đó, hệ thống bị sập và báo lỗi: \"JsonException: A possible object cycle was detected\". Nguyên nhân của lỗi này là gì và cách xử lý triệt để nhất là gì?",
                new List<string>
                {
                    "A. Do đối tượng Book chứa Category, và bên trong Category lại chứa danh sách Books trỏ ngược lại tạo thành vòng lặp vô tận khi dịch sang JSON; xử lý tốt nhất bằng cách chuyển sang DTO (Projection) hoặc bật ReferenceHandler.IgnoreCycles trong Program.cs",
                    "B. Do SQL Server bị mất điện",
                    "C. Do máy tính thiếu RAM",
                    "D. Do đặt tên biến bằng tiếng Anh"
                },
                "A",
                "Bộ tuần tự hóa System.Text.Json khi đi theo các thuộc tính điều hướng 2 chiều (Book <-> Category) sẽ bị lặp vô hạn. Chuyển sang DTO không có tham chiếu ngược hoặc bật ReferenceHandler.IgnoreCycles sẽ giải quyết dứt điểm lỗi này."
            ),

            // =====================================================================
            // BÀI 9.3
            // =====================================================================
            Build(
                topicId,
                "9.3",
                "Định tuyến Nâng cao (Attribute Routing), Ràng buộc tham số đường dẫn (Route Constraints) & Giải quyết trùng lặp Route",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/routing#route-constraints",
                """
                • **💡 1. Tại sao cần `Route Constraints` (Ràng buộc kiểu dữ liệu ngay trên URL)?**
                  - Giả sử trong `BooksController` của bạn có **2 hàm lấy chi tiết sách**:
                    1. Hàm 1 tìm theo khóa chính `Id` (số nguyên): `[HttpGet("{id}")]` (VD: `/api/books/15`).
                    2. Hàm 2 tìm theo mã `BookCode` (chuỗi chữ): `[HttpGet("{code}")]` (VD: `/api/books/CNTT-01`).
                  - Nếu bạn viết như trên và gọi `/api/books/15`, ASP.NET Core sẽ **nổ lỗi sập ngay lập tức (`AmbiguousMatchException`)** vì nó không biết số `15` nên chạy vào hàm `{id}` hay hàm `{code}`!
                  - **Giải pháp `Route Constraints`:** Ghi rõ kiểu dữ liệu sau dấu hai chấm `:` ngay trong ngoặc nhọn: **`[HttpGet("{id:int}")]`** và **`[HttpGet("{code:alpha}")]`**!
                • **🖼️ 2. Sơ đồ Phân Luồng Thông Minh Nhờ `Route Constraints`:**
                ```
                Request gửi lên: GET /api/books/...
                      │
                      ├── Nếu là "/api/books/105" (Toàn chữ số, >= 1)
                      │       ──► Khớp với [HttpGet("{id:int:min(1)}")]  ──► Chạy hàm GetById(int id)
                      │
                      ├── Nếu là "/api/books/3fa85f64-5717..." (Định dạng UUID)
                      │       ──► Khớp với [HttpGet("{guidId:guid}")]    ──► Chạy hàm GetByGuid(Guid guidId)
                      │
                      └── Nếu là "/api/books/lap-trinh-csharp" (Chuỗi chữ)
                              ──► Khớp với [HttpGet("{slug}")]           ──► Chạy hàm GetBySlug(string slug)
                ```
                • **📊 3. Bảng Các Ràng Buộc Route (`Route Constraints`) Phổ Biến Nhất Trong ASP.NET Core:**
                | Cú pháp Constraint | Ý nghĩa kiểm tra trên URL | Ví dụ thực tế |
                | :--- | :--- | :--- |
                | **`{id:int}`** | Bắt buộc phải là số nguyên `int` hợp lệ. | `[HttpGet("{id:int}")]` |
                | **`{id:int:min(1)}`** | Phải là số nguyên **lớn hơn hoặc bằng 1** (chặn luôn `id = 0` hoặc số âm!). | `[HttpDelete("{id:int:min(1)}")]` |
                | **`{id:guid}`** | Bắt buộc phải đúng định dạng chuỗi `Guid` 36 ký tự. | `[HttpGet("{id:guid}")]` |
                | **`{code:maxlength(20)}`**| Chuỗi có độ dài tối đa 20 ký tự. | `[HttpGet("by-code/{code:maxlength(20)}")]`|
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Giải quyết lỗi trùng Route (AmbiguousMatchException) bằng :int",
                        "📥 Đầu vào: Cần 2 API lấy sách theo ID số nguyên và theo đường dẫn tĩnh Slug. ⚙️ Cơ chế ngầm: Thêm ràng buộc ':int:min(1)' vào hàm tìm theo ID để bộ định tuyến phân biệt rạch ròi với chuỗi chữ. 📤 Đầu ra: Hai hàm chung sống hòa bình.",
                        "[HttpGet(\"{id:int:min(1)}\")] // Chỉ bắt khi sau dấu / là số nguyên >= 1\npublic async Task<IActionResult> GetById(int id) => Ok(await _service.GetByIdAsync(id));\n\n[HttpGet(\"{slug}\")] // Bắt các chuỗi văn bản như: /api/books/hoc-csharp-net10\npublic async Task<IActionResult> GetBySlug(string slug) => Ok(await _service.GetBySlugAsync(slug));"),
                    Step(2,
                        "2. Sử dụng dấu gạch chéo đầu (/ hoặc ~/) để ghi đè Route gốc của Controller",
                        "📥 Đầu vào: Controller đang có Route gốc là 'api/books', nhưng có 1 hàm đặc biệt muốn đặt đường dẫn là '/api/dashboard-stats'. ⚙️ Cơ chế ngầm: Đặt ký hiệu '~/' ở đầu chuỗi Route của hàm để thoát khỏi tiền tố của Controller. 📤 Đầu ra: Đường dẫn tùy biến linh hoạt.",
                        "[HttpGet(\"~/api/dashboard-stats\")] // Bỏ qua 'api/books', nhận thẳng '/api/dashboard-stats'\npublic async Task<IActionResult> GetDashboardStats() => Ok();"),
                    Step(3,
                        "3. Đặt tên Route (Name = \"...\") để dùng trong CreatedAtRoute / CreatedAtAction",
                        "📥 Đầu vào: Sau khi POST tạo sách mới xong, cần trả về Header 'Location' chứa link xem chi tiết cuốn sách vừa tạo. ⚙️ Cơ chế ngầm: Dùng CreatedAtAction trỏ vào hàm GetById kèm ID mới. 📤 Đầu ra: Trả về chuẩn mã 201 Created.",
                        "[HttpPost]\npublic async Task<IActionResult> Create(CreateBookDto dto) {\n    var created = await _service.CreateAsync(dto);\n    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); // 201 Created\n}")
                },
                "Trong cùng một ProductsController, bạn viết 2 hàm: hàm thứ nhất gắn [HttpGet(\"{id}\")] nhận int id, và hàm thứ hai gắn [HttpGet(\"{productCode}\")] nhận string productCode. Khi chạy và gọi /api/products/123, hệ thống báo lỗi AmbiguousMatchException. Bạn sửa lại như thế nào để hết lỗi?",
                new List<string>
                {
                    "A. Thêm ràng buộc kiểu dữ liệu Route Constraint cho hàm thứ nhất thành [HttpGet(\"{id:int}\")] hoặc đặt thêm tiền tố phân biệt như [HttpGet(\"by-code/{productCode}\")]",
                    "B. Xóa một trong hai hàm đi vì mỗi Controller chỉ được phép có 1 hàm HttpGet",
                    "C. Đổi kiểu trả về thành void",
                    "D. Tắt tường lửa của máy tính"
                },
                "A",
                "Khi không có Route Constraint, cả {id} và {productCode} đều là các biến giữ chỗ (placeholders) có độ ưu tiên ngang nhau đối với chuỗi \"123\", gây ra lỗi mơ hồ định tuyến (AmbiguousMatchException)."
            ),

            // =====================================================================
            // BÀI 9.4
            // =====================================================================
            Build(
                topicId,
                "9.4",
                "Kiểm tra dữ liệu đầu vào Chuyên sâu: Data Annotations, IValidatableObject và Thư viện FluentValidation",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/validation",
                """
                • **💡 1. Ba cấp độ kiểm tra dữ liệu đầu vào (`Validation`) trong dự án .NET:**
                  - **Cấp độ 1 - `Data Annotations` cơ bản (`[Required]`, `[StringLength]`, `[Range]`, `[RegularExpression]`):** Kiểm tra từng trường đơn lẻ độc lập.
                  - **Cấp độ 2 - `IValidatableObject` (Kiểm tra chéo giữa nhiều trường trong cùng 1 DTO):**
                    + *Bài toán thực tế:* Trong form *"Tạo phiếu mượn sách"*, có `NgayMuon` và `NgayHenTra`. Làm sao kiểm tra quy tắc: **`NgayHenTra` bắt buộc phải lớn hơn hoặc bằng `NgayMuon`**? Các thẻ `[Required]` đơn lẻ không thể so sánh 2 trường với nhau được!
                    + *Giải pháp:* Cho class DTO thực thi giao diện **`IValidatableObject`** và viết hàm `Validate()` ngay bên trong DTO!
                  - **Cấp độ 3 - Thư viện `FluentValidation` (Chuẩn Doanh nghiệp lớn):** Tách toàn bộ luật kiểm tra ra một class `Validator` riêng biệt, viết bằng biểu thức Lambda (`RuleFor(x => x.Price).GreaterThan(0)`) cực kỳ sạch đẹp và dễ test!
                • **🖼️ 2. Sơ đồ So Sánh 3 Phương Pháp Validation Trong ASP.NET Core:**
                ```
                Request JSON ──► 1. Data Annotations ([Required], [Range])  ──► Kiểm tra từng trường đơn lẻ
                             ──► 2. IValidatableObject (Hàm Validate)       ──► Kiểm tra chéo (Ngày trả >= Ngày mượn)
                             ──► 3. FluentValidation (AbstractValidator<T>) ──► Tách file riêng, kiểm tra phức tạp
                                    │
                                    ▼ (Nếu có bất kỳ lỗi nào)
                         Trả thẳng 400 Bad Request kèm danh sách lỗi chi tiết cho từng trường!
                ```
                • **📊 3. Bảng Các Thẻ `Data Annotations` & Cú Pháp `FluentValidation` Tương Đương:**
                | Mục đích kiểm tra | Dùng `Data Annotations` | Dùng `FluentValidation` (`RuleFor`) |
                | :--- | :--- | :--- |
                | **Không được để trống**| `[Required(ErrorMessage = "...")]` | `RuleFor(x => x.Title).NotEmpty().WithMessage("...");` |
                | **Giới hạn độ dài chuỗi**| `[StringLength(100, MinimumLength = 3)]`| `RuleFor(x => x.Title).Length(3, 100);` |
                | **Giới hạn khoảng số** | `[Range(1, 50000000)]` | `RuleFor(x => x.Price).InclusiveBetween(1, 50000000);` |
                | **Định dạng Email / SĐT**| `[EmailAddress]`, `[Phone]` | `RuleFor(x => x.Email).EmailAddress();` |
                | **So sánh chéo 2 trường**| Thực thi `IValidatableObject` | `RuleFor(x => x.DueDate).GreaterThan(x => x.BorrowDate);` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm tra từng trường bằng Data Annotations kèm thông báo Tiếng Việt",
                        "📥 Đầu vào: Class CreateBookDto. ⚙️ Cơ chế ngầm: Gắn các Attribute kiểm tra kèm ErrorMessage rõ ràng để hiển thị thẳng lên giao diện người dùng. 📤 Đầu ra: Chặn dữ liệu rỗng hoặc sai khoảng giá.",
                        "public class CreateBookDto {\n    [Required(ErrorMessage = \"Tên sách không được bỏ trống\")]\n    [StringLength(200, MinimumLength = 2, ErrorMessage = \"Tên sách từ 2 đến 200 ký tự\")]\n    public string Title { get; set; } = string.Empty;\n\n    [Range(1000, 10000000, ErrorMessage = \"Giá sách phải từ 1.000đ đến 10.000.000đ\")]\n    public decimal Price { get; set; }\n}"),
                    Step(2,
                        "2. Kiểm tra chéo 2 trường ngày tháng bằng IValidatableObject",
                        "📥 Đầu vào: CreateBorrowDto có BorrowDate (Ngày mượn) và DueDate (Ngày hẹn trả). ⚙️ Cơ chế ngầm: Thực thi IValidatableObject. Nếu DueDate < BorrowDate thì dùng yield return new ValidationResult để báo lỗi chính xác tại trường DueDate! 📤 Đầu ra: Tự động trả về 400 Bad Request nếu ngày trả nhỏ hơn ngày mượn.",
                        "public class CreateBorrowDto : IValidatableObject {\n    public DateTime BorrowDate { get; set; }\n    public DateTime DueDate { get; set; }\n\n    public IEnumerable<ValidationResult> Validate(ValidationContext context) {\n        if (DueDate < BorrowDate) {\n            yield return new ValidationResult(\n                \"Ngày hẹn trả phải lớn hơn hoặc bằng ngày mượn!\",\n                new[] { nameof(DueDate) });\n        }\n    }\n}"),
                    Step(3,
                        "3. Viết bộ kiểm tra tách biệt bằng FluentValidation (Chuẩn Senior)",
                        "📥 Đầu vào: Muốn giữ cho class DTO sạch bóng không dính Attribute. ⚙️ Cơ chế ngầm: Tạo class kế thừa AbstractValidator<CreateBookDto> và định nghĩa chuỗi quy tắc bằng RuleFor. 📤 Đầu ra: Kiến trúc sạch sẽ, dễ viết Unit Test.",
                        "public class CreateBookValidator : AbstractValidator<CreateBookDto> {\n    public CreateBookValidator() {\n        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);\n        RuleFor(x => x.Price).GreaterThan(0).WithMessage(\"Giá phải lớn hơn 0\");\n    }\n}")
                },
                "Khi sử dụng các thẻ Data Annotations chuẩn sẵn có của .NET (mà chưa cài thư viện ngoài), nếu bạn muốn viết một quy tắc kiểm tra so sánh chéo giữa 2 thuộc tính trong cùng một DTO (ví dụ: kiểm tra EndDate phải lớn hơn StartDate), cách làm chuẩn nhất là gì?",
                new List<string>
                {
                    "A. Cho class DTO đó thực thi interface IValidatableObject và viết logic so sánh bên trong hàm Validate(ValidationContext)",
                    "B. Gắn 10 thẻ [Required] lên cùng 1 thuộc tính",
                    "C. Không thể kiểm tra được trong C#",
                    "D. Phải lưu dữ liệu sai vào Database trước rồi mới kiểm tra"
                },
                "A",
                "Giao diện IValidatableObject có sẵn trong System.ComponentModel.DataAnnotations được ASP.NET Core tự động kích hoạt ngay sau khi các thẻ [Required] cơ bản đã hợp lệ, giúp kiểm tra các quy tắc nghiệp vụ phụ thuộc giữa nhiều trường trong cùng một Model/DTO."
            ),

            // =====================================================================
            // BÀI 9.5
            // =====================================================================
            Build(
                topicId,
                "9.5",
                "Chuẩn hóa Phản hồi Lỗi Toàn cục với IExceptionHandler (.NET 8/9/10) và Chuẩn RFC 7807 ProblemDetails",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/error-handling#iexceptionhandler",
                """
                • **💡 1. Tại sao cần chuẩn hóa lỗi theo `ProblemDetails (RFC 7807)`?**
                  - Nếu trong dự án có 5 lập trình viên: người thứ nhất trả lỗi dạng `"Lỗi rồi"`, người thứ hai trả `{ "err": "Không tìm thấy" }`, người thứ ba trả `{ "msg": "Sai ID", "code": 404 }` $\rightarrow$ Đội làm Frontend/Mobile sẽ phát điên vì không biết viết code bắt lỗi kiểu gì!
                  - Chuẩn quốc tế **RFC 7807 (`ProblemDetails`)** được Microsoft tích hợp sẵn vào ASP.NET Core quy định mọi lỗi trả về đều có chung 1 khung JSON chuẩn gồm:
                    + `type`: Đường link mã lỗi.
                    + `title`: Tiêu đề lỗi ngắn gọn (VD: `"Not Found"` hoặc `"Bad Request"`).
                    + `status`: Mã số HTTP (`400`, `404`, `500`).
                    + `detail`: Giải thích chi tiết tiếng Việt cho người dùng hiểu (VD: `"Cuốn sách có mã 99 không tồn tại trong thư viện"`).
                    + `instance`: Đường dẫn URL vừa bị lỗi.
                • **🖼️ 2. Cách xử lý lỗi mới nhất trong .NET 8 / 9 / 10: Giao diện `IExceptionHandler`:**
                ```
                Thay vì phải tự viết Middleware thủ công phức tạp như các bản .NET cũ,
                trong .NET mới (8/9/10), Microsoft cung cấp giao diện chuẩn IExceptionHandler:

                [Tầng Service ném lỗi] ──► [GlobalExceptionHandler : IExceptionHandler]
                                                 │ 1. Tự động ghi Log lỗi bằng ILogger
                                                 │ 2. Ánh xạ loại Exception sang mã Status (400, 404, 500)
                                                 ▼ 3. Đóng gói vào đối tượng ProblemDetails trả về JSON!
                ```
                • **📊 3. Bảng Ánh Xạ Từ `Exception` Trong Code C# Sang Mã `HTTP Status Code` Chuẩn:**
                | Loại Exception ném ra từ Service | Mã HTTP Status tương ứng | Tiêu đề `ProblemDetails.Title` |
                | :--- | :--- | :--- |
                | `KeyNotFoundException` / `NotFoundException` | **`404 Not Found`** | `"Không tìm thấy dữ liệu"` |
                | `ArgumentException` / `BadRequestException` | **`400 Bad Request`** | `"Dữ liệu yêu cầu không hợp lệ"` |
                | `UnauthorizedAccessException` | **`401 Unauthorized` / `403`**| `"Không có quyền truy cập"` |
                | Các lỗi hệ thống khác (`Exception`) | **`500 Internal Server Error`**| `"Lỗi hệ thống máy chủ"` (Ẩn chi tiết code thật để bảo mật!) |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo class GlobalExceptionHandler thực thi IExceptionHandler",
                        "📥 Đầu vào: Bất kỳ Exception nào chưa được bắt trong dự án. ⚙️ Cơ chế ngầm: Hàm TryHandleAsync nhận HttpContext và Exception, ghi Log lỗi và phân loại mã trạng thái HTTP. 📤 Đầu ra: Trạm xử lý lỗi tập trung hiện đại nhất của .NET.",
                        "public class GlobalExceptionHandler : IExceptionHandler {\n    private readonly ILogger<GlobalExceptionHandler> _logger;\n    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;\n\n    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct) {\n        _logger.LogError(exception, \"Lỗi xảy ra: {Message}\", exception.Message);"),
                    Step(2,
                        "2. Đóng gói lỗi vào đối tượng ProblemDetails chuẩn RFC 7807",
                        "📥 Đầu vào: Đối tượng exception. ⚙️ Cơ chế ngầm: Dùng biểu thức switch để quyết định Status Code (404, 400 hay 500) và tạo đối tượng ProblemDetails trả ra JSON. 📤 Đầu ra: Cấu trúc JSON lỗi đồng nhất 100%.",
                        "        var statusCode = exception switch {\n            KeyNotFoundException => StatusCodes.Status404NotFound,\n            ArgumentException => StatusCodes.Status400BadRequest,\n            _ => StatusCodes.Status500InternalServerError\n        };\n        var problem = new ProblemDetails {\n            Status = statusCode,\n            Title = statusCode == 500 ? \"Lỗi hệ thống\" : \"Lỗi xử lý yêu cầu\",\n            Detail = statusCode == 500 ? \"Vui lòng liên hệ quản trị viên\" : exception.Message,\n            Instance = httpContext.Request.Path\n        };\n        httpContext.Response.StatusCode = statusCode;\n        await httpContext.Response.WriteAsJsonAsync(problem, ct);\n        return true;\n    }\n}"),
                    Step(3,
                        "3. Đăng ký AddExceptionHandler và AddProblemDetails trong Program.cs",
                        "📥 Đầu vào: Class GlobalExceptionHandler vừa tạo. ⚙️ Cơ chế ngầm: Đăng ký dịch vụ và kích hoạt app.UseExceptionHandler() ở đầu Pipeline. 📤 Đầu ra: Toàn bộ dự án được bảo vệ hoàn hảo.",
                        "builder.Services.AddExceptionHandler<GlobalExceptionHandler>();\nbuilder.Services.AddProblemDetails();\n// ...\nvar app = builder.Build();\napp.UseExceptionHandler();")
                },
                "Khi xảy ra một lỗi hệ thống bất ngờ (500 Internal Server Error, ví dụ lỗi câu lệnh SQL hoặc NullReferenceException) trên môi trường máy chủ thật (Production), tại sao trong đối tượng ProblemDetails trả về cho người dùng cuối, chúng ta TUYỆT ĐỐI KHÔNG ĐƯỢC in chi tiết exception.StackTrace ra ngoài JSON mà chỉ nên ghi vào ILogger?",
                new List<string>
                {
                    "A. Vì StackTrace làm lộ cấu trúc thư mục máy chủ, tên file code, tên bảng Database và dòng lệnh nội bộ, giúp tin tặc (Hacker) dễ dàng khai thác lỗ hổng bảo mật; chi tiết đó chỉ dành cho lập trình viên đọc trong file Log nội bộ",
                    "B. Vì JSON không chứa được chuỗi dài",
                    "C. Vì trình duyệt không hiển thị được tiếng Anh",
                    "D. Vì in StackTrace làm mất dữ liệu trong RAM"
                },
                "A",
                "Nguyên tắc bảo mật OWASP quy định: với lỗi 500 trên Production, chỉ trả về thông báo thân thiện chung chung cho Client (\"Đã xảy ra lỗi hệ thống, vui lòng thử lại sau\"), còn toàn bộ StackTrace và InnerException phải được ghi kín đáo vào hệ thống Logging (Serilog/ILogger) của máy chủ."
            ),

            // =====================================================================
            // BÀI 9.6
            // =====================================================================
            Build(
                topicId,
                "9.6",
                "Bộ lọc Action Filters (IActionFilter / ActionFilterAttribute) - Chặn kiểm tra trước & sau khi hàm Controller chạy",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/controllers/filters",
                """
                • **💡 1. Hiểu bản chất của `Action Filter` và sự khác biệt với `Middleware` trong 30 giây:**
                  - Ở Chủ đề 4, bạn đã biết **Middleware** là cổng an ninh đứng ở ngoài cùng chặn **tất cả mọi Request** (lúc đó Request chưa vào tới Controller nên chưa biết hàm nào sẽ chạy, chưa có dữ liệu `ModelState` hay `DTO`).
                  - Còn **Action Filter** là **"Trạm kiểm soát gắn ngay trước cửa từng hàm Action trong Controller"**:
                    + Nó chạy **SAU khi dữ liệu JSON đã được `Model Binding` thành đối tượng C#**.
                    + Bạn có thể gắn nó dưới dạng một thẻ `[Attribute]` (như `[LogActivity]`, `[ValidateEntityExists]`) lên **chỉ những hàm hoặc Controller nào bạn muốn** chứ không bắt tất cả mọi API đều phải đi qua như Middleware!
                • **🖼️ 2. Sơ đồ Vị Trí Của `Action Filter` Trong Đường Ống ASP.NET Core:**
                ```
                Middleware Pipeline (Ngoài cùng)
                     │
                     ▼
                Routing & Model Binding (Đã dịch xong JSON ra đối tượng DTO C#)
                     │
                     ▼
                ╔════════════════════════════ ACTION FILTER ════════════════════════════╗
                ║  1. OnActionExecuting() : Chạy NGAY TRƯỚC khi vào thân hàm Controller ║
                ║            │              (Đo thời gian bắt đầu, kiểm tra DTO/Header) ║
                ║            ▼                                                          ║
                ║     [ THÂN HÀM CONTROLLER CHẠY (Gọi Service -> Lưu DB) ]              ║
                ║            │                                                          ║
                ║            ▼                                                          ║
                ║  2. OnActionExecuted()  : Chạy NGAY SAU khi hàm Controller vừa xong!  ║
                ║                           (Tính tổng số mili-giây chạy, ghi Audit Log)║
                ╚═══════════════════════════════════════════════════════════════════════╝
                ```
                • **📊 3. Ứng dụng thực tế của `IAsyncActionFilter` trong các dự án công ty:**
                  1. **Đo hiệu năng từng API (Performance Profiling):** Dùng `Stopwatch` bấm giờ xem hàm Controller chạy mất bao nhiêu mili-giây; nếu lâu quá `1000ms` thì tự động ghi cảnh báo `LogWarning("API chạy chậm!")`.
                  2. **Ghi lịch sử thao tác của Quản trị viên (Audit Logging):** Tự động lưu lại *"Ai vừa gọi hàm Xóa/Sửa nào, vào lúc mấy giờ, truyền tham số gì"* mà không cần viết code lặp lại trong từng hàm Controller!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo bộ lọc đo thời gian chạy API bằng IAsyncActionFilter",
                        "📥 Đầu vào: Giao diện IAsyncActionFilter. ⚙️ Cơ chế ngầm: Trước lệnh await next() là code chạy TRƯỚC khi vào Controller; sau lệnh await next() là code chạy SAU khi Controller đã thực thi xong! 📤 Đầu ra: Bộ lọc bất đồng bộ chuẩn.",
                        "public class PerformanceTimeFilter : IAsyncActionFilter {\n    private readonly ILogger<PerformanceTimeFilter> _logger;\n    public PerformanceTimeFilter(ILogger<PerformanceTimeFilter> logger) => _logger = logger;\n\n    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {\n        var timer = System.Diagnostics.Stopwatch.StartNew(); // 1. Bấm giờ trước khi vào hàm\n\n        var resultContext = await next(); // 2. Cho phép chạy vào hàm Controller\n\n        timer.Stop(); // 3. Dừng đồng hồ ngay khi hàm Controller chạy xong\n        var actionName = context.ActionDescriptor.DisplayName;\n        _logger.LogInformation(\"API {Action} thực thi mất {Elapsed} ms\", actionName, timer.ElapsedMilliseconds);\n    }\n}"),
                    Step(2,
                        "2. Chặn sớm (Short-circuiting) ngay trong Action Filter",
                        "📥 Đầu vào: Kiểm tra một điều kiện đặc biệt trước khi vào Controller. ⚙️ Cơ chế ngầm: Nếu phát hiện vi phạm, chỉ cần gán giá trị cho context.Result = new BadRequestObjectResult(...) và KHÔNG gọi await next() là Request dừng lại trả về Client luôn! 📤 Đầu ra: Ngăn không cho chạy vào Controller.",
                        "if (!context.HttpContext.Request.Headers.ContainsKey(\"X-Client-Version\")) {\n    context.Result = new BadRequestObjectResult(\"Thiếu Header phiên bản ứng dụng!\");\n    return; // Thoát luôn, không gọi await next() nữa!\n}"),
                    Step(3,
                        "3. Đăng ký Filter toàn cục hoặc gắn riêng lên từng Controller",
                        "📥 Đầu vào: Class PerformanceTimeFilter. ⚙️ Cơ chế ngầm: Có thể gắn cho toàn bộ các Controller trong AddControllers, hoặc dùng [ServiceFilter(typeof(...))] trên từng hàm cụ thể. 📤 Đầu ra: Áp dụng linh hoạt.",
                        "// Cách 1 - Áp dụng cho mọi Controller:\nbuilder.Services.AddControllers(options => {\n    options.Filters.Add<PerformanceTimeFilter>();\n});\n\n// Cách 2 - Chỉ gắn lên 1 hàm cụ thể:\n[ServiceFilter(typeof(PerformanceTimeFilter))]\n[HttpGet]")
                },
                "Bên trong phương thức OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) của một IAsyncActionFilter, nếu bạn gán context.Result = new UnauthorizedResult(); và return; luôn mà không gọi await next();, chuyện gì sẽ xảy ra?",
                new List<string>
                {
                    "A. Quá trình xử lý bị ngắt ngắn (Short-circuit): Hệ thống lập tức trả về mã 401 Unauthorized cho Client và KHÔNG chạy vào bên trong hàm Action của Controller nữa",
                    "B. Hàm trong Controller vẫn tiếp tục chạy bình thường",
                    "C. Chương trình bị treo vĩnh viễn",
                    "D. Tự động chuyển sang trang chủ"
                },
                "A",
                "Trong Action Filter, lời gọi await next() chính là cánh cửa đưa luồng thực thi vào thân hàm Controller. Nếu bạn gán context.Result và bỏ qua await next(), phản hồi sẽ được trả về ngay lập tức (Short-circuiting)."
            ),

            // =====================================================================
            // BÀI 9.7
            // =====================================================================
            Build(
                topicId,
                "9.7",
                "Cơ chế CORS (Cross-Origin Resource Sharing) - Giải quyết triệt để lỗi chặn kết nối khi ghép nối Frontend & Backend",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/cors",
                """
                • **💡 1. Hiểu bản chất lỗi `CORS Policy` trong 30 giây (100% lập trình viên làm Web đều gặp!):**
                  - Giả sử Backend ASP.NET Core Web API của bạn đang chạy ở địa chỉ `https://localhost:7001`.
                  - Trong khi đó, trang Frontend (React / Vue / Angular / Blazor WebAssembly hoặc 1 trang HTML khác) đang chạy ở địa chỉ `http://localhost:3000`.
                  - Khi code JavaScript ở cổng `3000` gọi `fetch('https://localhost:7001/api/books')`, trình duyệt Chrome/Edge phát hiện **2 cổng khác nhau (Khác nguồn - Cross-Origin)** nên tính năng bảo mật của trình duyệt lập tức **CHẶN ĐỨNG LẠI** và báo dòng chữ đỏ lòm trong Console (F12):
                    ❌ *"Access to fetch at 'https://localhost:7001/...' from origin 'http://localhost:3000' has been blocked by **CORS policy**"*!
                  - **Lưu ý cực kỳ thú vị:** Lỗi CORS này là do **Trình duyệt tự chặn để bảo vệ người dùng** (chứ gọi bằng Postman vẫn chạy ầm ầm vì Postman không phải là trình duyệt!). Để trình duyệt cho phép gọi, **Backend C# phải cấp "Giấy phép thông hành CORS"** cho địa chỉ `http://localhost:3000`!
                • **🖼️ 2. Sơ đồ Cơ Chế Cấp Phép `CORS` Giữa Trình Duyệt (Frontend) và Backend ASP.NET Core:**
                ```
                Frontend (http://localhost:3000)                           Backend API (https://localhost:7001)
                ┌──────────────────────────────┐                           ┌──────────────────────────────────┐
                │ 1. Trình duyệt hỏi thăm trước│ ──(OPTIONS Preflight)───► │ Kiểm tra chính sách AddCors()    │
                │    "Cổng 3000 gọi được không?"│                           │ trong Program.cs                 │
                │                              │ ◄─(Header: Allow-Origin)─ │ "OK! Tôi cho phép cổng 3000 gọi!"│
                │ 2. Được phép -> Gửi Request  │ ──(GET / POST dữ liệu)──► │ Xử lý và trả về JSON bình thường │
                └──────────────────────────────┘                           └──────────────────────────────────┘
                ```
                • **📊 3. Ba Quy Tắc Cấu Hình `CORS` Chuẩn Trong `Program.cs`:**
                  1. **`WithOrigins("http://localhost:3000", "https://hoangviet.vn")`:** Liệt kê danh sách các địa chỉ Frontend được phép gọi vào API (Chú ý: **Không có dấu gạch chéo `/` ở cuối địa chỉ!**).
                  2. **`AllowAnyHeader()` & `AllowAnyMethod()`:** Cho phép gửi kèm Header (như `Authorization: Bearer ...`) và mọi phương thức (`GET, POST, PUT, DELETE`).
                  3. **Vị trí đặt `app.UseCors(...)` trong Middleware Pipeline:** Bắt buộc phải đặt **SAU `UseRouting()`** và **TRƯỚC `UseAuthentication()` / `UseAuthorization()`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Đăng ký chính sách CORS (AddCors) trong Program.cs",
                        "📥 Đầu vào: Địa chỉ của trang web Frontend (ví dụ cổng 3000 hoặc 5173 của React/Vite/Blazor). ⚙️ Cơ chế ngầm: Tạo một Policy có tên 'AllowFrontendApp' quy định rõ nguồn Origin, Header và Method được cấp phép. 📤 Đầu ra: Chính sách bảo mật CORS rõ ràng.",
                        "builder.Services.AddCors(options => {\n    options.AddPolicy(\"AllowFrontendApp\", policy => {\n        policy.WithOrigins(\"http://localhost:3000\", \"https://localhost:7123\") // Không có dấu / ở cuối!\n              .AllowAnyHeader()\n              .AllowAnyMethod()\n              .AllowCredentials();\n    });\n});"),
                    Step(2,
                        "2. Kích hoạt Middleware app.UseCors đúng vị trí",
                        "📥 Đầu vào: Tên chính sách 'AllowFrontendApp' vừa đăng ký ở Bước 1. ⚙️ Cơ chế ngầm: Đặt app.UseCors(\"AllowFrontendApp\") nằm TRƯỚC app.UseAuthentication() và app.UseAuthorization() để các gói tin kiểm tra Preflight OPTIONS được thông qua ngay. 📤 Đầu ra: Trình duyệt hết sạch lỗi đỏ CORS.",
                        "var app = builder.Build();\n\napp.UseHttpsRedirection();\napp.UseCors(\"AllowFrontendApp\"); // BẮT BUỘC đặt trước UseAuthentication & UseAuthorization!\napp.UseAuthentication();\napp.UseAuthorization();\napp.MapControllers();"),
                    Step(3,
                        "3. Cấu hình mở tự do (AllowAll) chỉ dành riêng cho lúc học tập / test nội bộ",
                        "📥 Đầu vào: Muốn cho phép mọi trang web đều gọi thử vào API của mình lúc đang học. ⚙️ Cơ chế ngầm: Dùng AllowAnyOrigin() kết hợp AllowAnyMethod() và AllowAnyHeader(). 📤 Đầu ra: Bất kỳ cổng nào gọi cũng được.",
                        "options.AddPolicy(\"AllowAllDev\", policy => {\n    policy.AllowAnyOrigin()\n          .AllowAnyMethod()\n          .AllowAnyHeader();\n});")
                },
                "Khi bạn viết xong một ASP.NET Core Web API chạy ở cổng 7001, bạn test trên Postman thì mọi API đều chạy thành công (200 OK), nhưng khi dùng code JavaScript (fetch/axios) từ trang Frontend chạy ở cổng 3000 gọi sang thì lại bị chặn và báo lỗi blocked by CORS policy trên trình duyệt. Bạn phải sửa lỗi này ở đâu?",
                new List<string>
                {
                    "A. Sửa ở phía Backend (file Program.cs của ASP.NET Core Web API): Đăng ký dịch vụ builder.Services.AddCors(...) cho phép Origin 'http://localhost:3000' và gọi app.UseCors(...) trước UseAuthorization()",
                    "B. Cài lại trình duyệt Chrome",
                    "C. Xóa bảng trong SQL Server",
                    "D. Sửa trong Postman"
                },
                "A",
                "Nhiều người mới lầm tưởng lỗi hiện trên trình duyệt Frontend thì phải sửa ở Frontend, nhưng thực chất trình duyệt đòi hỏi máy chủ Backend phải trả về Header Access-Control-Allow-Origin hợp lệ thông qua cấu hình AddCors và UseCors trong Program.cs."
            ),

            // =====================================================================
            // BÀI 9.8
            // =====================================================================
            Build(
                topicId,
                "9.8",
                "Quản lý Phiên bản API (API Versioning) & Tài liệu hóa chuẩn OpenAPI / Swagger nâng cao",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/tutorials/web-api-help-pages-using-swagger",
                """
                • **💡 1. Tại sao các dự án thực tế luôn có `/api/v1/products` và `/api/v2/products` (`API Versioning`)?**
                  - Giả sử ứng dụng Mobile của công ty bạn đã có **50.000 khách hàng cài trên điện thoại**, đang gọi vào API `GET /api/v1/orders` nhận về trường `TotalMoney`.
                  - Hôm nay sếp yêu cầu nâng cấp cấu trúc đơn hàng mới hoàn toàn (tách `TotalMoney` thành `SubTotal`, `Tax`, `ShippingFee`).
                  - Nếu bạn đập đi sửa trực tiếp vào API cũ: **50.000 người dùng chưa kịp cập nhật App trên điện thoại sẽ bị sập ứng dụng ngay lập tức!**
                  - **Giải pháp `API Versioning`:** Giữ nguyên API cũ ở **`/api/v1/orders`** cho các máy điện thoại cũ chạy bình thường, đồng thời tạo phiên bản mới ở **`/api/v2/orders`** cho Website và App đời mới gọi!
                • **🖼️ 2. Ba cách đánh số Phiên bản API phổ biến trong thực tế:**
                ```
                1. URL Path Versioning (Phổ biến nhất - 90% công ty dùng vì nhìn thấy ngay trên URL):
                   GET /api/v1/books   vs   GET /api/v2/books

                2. Query String Versioning (Truyền qua tham số sau dấu ?):
                   GET /api/books?api-version=1.0   vs   GET /api/books?api-version=2.0

                3. Header Versioning (Đặt trong HTTP Header):
                   Header: X-Api-Version: 2.0
                ```
                • **📊 3. Làm đẹp Tài liệu Swagger / OpenAPI chuẩn Chuyên nghiệp:**
                  - Gắn thẻ **`[ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]`** và **`[ProducesResponseType(StatusCodes.Status404NotFound)]`** lên hàm Controller để giao diện Swagger hiển thị rõ ràng từng mã trạng thái và mẫu JSON trả về cho đội Frontend đọc.
                  - Bật tính năng đọc chú thích XML (`/// <summary>...</summary>`) để những dòng giải thích tiếng Việt bạn gõ trên đầu hàm C# hiện thẳng lên giao diện web Swagger!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết lập URL Versioning cho Controller",
                        "📥 Đầu vào: Cần duy trì song song phiên bản v1 và v2 của API Sách. ⚙️ Cơ chế ngầm: Khai báo [Route(\"api/v1/books\")] và [Route(\"api/v2/books\")] (hoặc dùng thư viện Asp.Versioning.Http với api/v{version:apiVersion}/[controller]). 📤 Đầu ra: Hai phiên bản chạy độc lập, không làm hỏng ứng dụng cũ.",
                        "[ApiController]\n[Route(\"api/v1/books\")]\npublic class BooksV1Controller : ControllerBase {\n    [HttpGet]\n    public IActionResult Get() => Ok(new { version = \"1.0\", data = \"Cấu trúc cũ\" });\n}\n\n[ApiController]\n[Route(\"api/v2/books\")]\npublic class BooksV2Controller : ControllerBase {\n    [HttpGet]\n    public IActionResult Get() => Ok(new { version = \"2.0\", items = new[] { \"Cấu trúc mới nâng cấp\" } });\n}"),
                    Step(2,
                        "2. Mô tả rõ các mã trả về bằng [ProducesResponseType]",
                        "📥 Đầu vào: Một hàm API lấy chi tiết sách có thể trả về 200 OK hoặc 404 NotFound. ⚙️ Cơ chế ngầm: Gắn ProducesResponseType để Swagger UI hiển thị chính xác cấu trúc JSON của cả trường hợp thành công lẫn trường hợp lỗi. 📤 Đầu ra: Tài liệu API rõ ràng cho Frontend.",
                        "[HttpGet(\"{id:int}\")]\n[ProducesResponseType(typeof(BookDto), StatusCodes.Status200OK)]\n[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]\npublic async Task<IActionResult> GetById(int id) {\n    var book = await _service.GetByIdAsync(id);\n    return book == null ? NotFound() : Ok(book);\n}"),
                    Step(3,
                        "3. Cấu hình nút ổ khóa Authorize (Bearer JWT) trên giao diện Swagger",
                        "📥 Đầu vào: Muốn bấm nút nhập Token đăng nhập ngay trên góc phải trang Swagger UI. ⚙️ Cơ chế ngầm: Thêm AddSecurityDefinition(\"Bearer\", ...) và AddSecurityRequirement trong AddSwaggerGen tại Program.cs. 📤 Đầu ra: Hiện nút 'Authorize' màu xanh lá trên Swagger.",
                        "builder.Services.AddSwaggerGen(c => {\n    c.AddSecurityDefinition(\"Bearer\", new OpenApiSecurityScheme {\n        Description = \"Nhập theo cú pháp: Bearer {token_của_bạn}\",\n        Name = \"Authorization\",\n        In = ParameterLocation.Header,\n        Type = SecuritySchemeType.ApiKey,\n        Scheme = \"Bearer\"\n    });\n});")
                },
                "Mục đích chính của việc áp dụng kỹ thuật API Versioning (ví dụ phân tách /api/v1/orders và /api/v2/orders) trong các hệ thống Web API thực tế là gì?",
                new List<string>
                {
                    "A. Cho phép nâng cấp, thay đổi cấu trúc dữ liệu ở phiên bản mới (v2) mà không làm sập hay ảnh hưởng đến các ứng dụng Mobile/Web của khách hàng cũ vẫn đang gọi vào phiên bản cũ (v1) (Đảm bảo tính tương thích ngược - Backward Compatibility)",
                    "B. Giúp câu lệnh SQL chạy nhanh gấp đôi",
                    "C. Để ẩn API không cho ai gọi được",
                    "D. Để tự động dịch tiếng Việt sang tiếng Anh"
                },
                "A",
                "Đảm bảo tính tương thích ngược (Backward Compatibility) là sống còn khi ứng dụng đã có người dùng thực tế. API Versioning giúp hệ thống tiến hóa sang v2, v3 êm ái trong khi khách hàng cũ ở v1 vẫn hoạt động bình thường."
            ),

            // =====================================================================
            // BÀI 9.9
            // =====================================================================
            Build(
                topicId,
                "9.9",
                "Tối ưu Tốc độ & Bảo vệ Web API với Output Caching / MemoryCache và Rate Limiting (Chống spam/DDoS)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/performance/caching/output",
                """
                • **💡 1. Hiểu bản chất của `Caching` (Lưu bộ nhớ đệm) và `Rate Limiting` (Giới hạn tần suất gọi) trong 30 giây:**
                  - **1. `Caching` (`IMemoryCache` / `OutputCache`):**
                    + Giả sử trang chủ có API *"Lấy danh sách 10 Thể loại sách"* (`GET /api/categories`). Dữ liệu thể loại cả tháng mới thay đổi 1 lần, nhưng mỗi giây có 500 người vào trang chủ!
                    + Nếu không có Cache: Mỗi giây bạn bắt SQL Server phải chạy câu lệnh `SELECT` lặp đi lặp lại 500 lần giống hệt nhau!
                    + **Có Cache:** Lần đầu tiên lấy từ SQL Server xong, bạn cất danh sách đó lên **RAM (`IMemoryCache` / `[OutputCache]`) trong 10 phút**. 499 người tiếp theo gọi vào sẽ nhận kết quả từ RAM trả về trong **`0.1 mili-giây`** mà SQL Server được nghỉ ngơi hoàn toàn!
                  - **2. `Rate Limiting` (Giới hạn số lần gọi trong 1 khoảng thời gian):**
                    + Nếu có kẻ xấu viết vòng lặp bắn **10.000 Request/giây** vào API Đăng nhập hoặc API Chat AI của bạn để phá hoại (hoặc đốt sạch tiền Token AI!), bộ lọc **Rate Limiter** sẽ chặn lại ngay từ lần gọi thứ 11 trong 1 phút và trả về mã lỗi **`429 Too Many Requests`** (*"Bạn thao tác quá nhanh, vui lòng chờ 1 phút!"*)!
                • **🖼️ 2. Sơ đồ Lá Chắn Kép `Rate Limiting` + `Caching` Bảo Vệ Server & Database:**
                ```
                1.000 Requests/giây ──► [Trạm 1: Rate Limiter] ──► Ai gọi quá 30 lần/phút ──► Chặn ngay (429 Too Many Requests)!
                                                 │
                                                 ▼ (Hợp lệ)
                                        [Trạm 2: Memory Cache] ──► Đã có sẵn trên RAM?    ──► Trả về ngay trong 0.1ms!
                                                 │
                                                 ▼ (Chưa có trong Cache - Cache Miss)
                                        [Trạm 3: SQL Server]   ──► Truy vấn DB ──► Lưu vào Cache 10 phút ──► Trả kết quả!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bật Output Caching siêu gọn chỉ với 1 Attribute",
                        "📥 Đầu vào: API lấy danh sách thể loại ít khi thay đổi. ⚙️ Cơ chế ngầm: Đăng ký AddOutputCache() và gắn thẻ [OutputCache(Duration = 300)] lên hàm GET. Suốt 300 giây (5 phút) tiếp theo, kết quả JSON được trả thẳng từ RAM mà không cần chạy lại hàm Controller! 📤 Đầu ra: Tốc độ phản hồi cực đại.",
                        "// Trong Program.cs: builder.Services.AddOutputCache(); ... app.UseOutputCache();\n\n[HttpGet]\n[OutputCache(Duration = 300)] // Lưu đệm kết quả JSON này trên RAM trong 300 giây (5 phút)!\npublic async Task<IActionResult> GetCategories() {\n    return Ok(await _service.GetAllCategoriesAsync());\n}"),
                    Step(2,
                        "2. Sử dụng IMemoryCache linh hoạt trong tầng Service",
                        "📥 Đầu vào: Cần kiểm soát xóa Cache (Cache Invalidation) ngay khi Admin thêm/sửa thể loại mới. ⚙️ Cơ chế ngầm: Dùng _cache.GetOrCreateAsync() để lấy hoặc lưu Cache, và dùng _cache.Remove(\"key\") khi có thay đổi dữ liệu. 📤 Đầu ra: Dữ liệu vừa nhanh vừa luôn tươi mới.",
                        "public async Task<List<CategoryDto>> GetCachedCategoriesAsync() {\n    return await _cache.GetOrCreateAsync(\"ALL_CATEGORIES\", async entry => {\n        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);\n        return await _repo.GetAllDtosAsync(); // Chỉ chạy vào DB khi chưa có Cache!\n    }) ?? new();\n}\n// Khi Admin thêm/sửa thể loại -> Gọi: _cache.Remove(\"ALL_CATEGORIES\");"),
                    Step(3,
                        "3. Thiết lập Rate Limiting chống Spam / DDoS (Mã 429)",
                        "📥 Đầu vào: Bảo vệ API tránh bị gọi liên tục quá mức. ⚙️ Cơ chế ngầm: Cấu hình FixedWindowLimiter cho phép tối đa 20 requests trong mỗi 1 phút; vượt ngưỡng tự động trả về HTTP 429 Too Many Requests. 📤 Đầu ra: Hệ thống an toàn trước các cuộc tấn công spam.",
                        "builder.Services.AddRateLimiter(options => {\n    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;\n    options.AddFixedWindowLimiter(\"AntiSpamPolicy\", opt => {\n        opt.Window = TimeSpan.FromMinutes(1);\n        opt.PermitLimit = 20; // Tối đa 20 lần gọi / 1 phút\n    });\n});")
                },
                "Khi tích hợp bộ nhớ đệm IMemoryCache để lưu danh sách sản phẩm (CacheKey = \"PRODUCTS_LIST\") trong 30 phút nhằm tăng tốc cho API GET, nếu ở phút thứ 5 người Quản trị viên bấm Thêm mới hoặc Sửa giá một sản phẩm, bạn phải làm gì trong hàm Thêm/Sửa đó để khách hàng không bị nhìn thấy giá cũ lưu trong Cache?",
                new List<string>
                {
                    "A. Gọi lệnh _cache.Remove(\"PRODUCTS_LIST\"); (Cache Invalidation - Xóa khóa cache cũ đi) ngay sau khi lưu Database thành công để lần GET tiếp theo hệ thống tự nạp lại dữ liệu mới nhất",
                    "B. Bắt khách hàng phải chờ hết 25 phút còn lại",
                    "C. Khởi động lại máy chủ SQL Server mỗi khi sửa giá",
                    "D. Không dùng Cache nữa"
                },
                "A",
                "Kỹ thuật xóa bộ nhớ đệm khi dữ liệu gốc thay đổi gọi là Cache Invalidation: đọc ở hàm GET thì lưu vào Cache (GetOrCreateAsync), còn ghi/sửa/xóa ở các hàm POST/PUT/DELETE thì gọi _cache.Remove(key) để làm mới dữ liệu."
            ),

            // =====================================================================
            // BÀI 9.10
            // =====================================================================
            Build(
                topicId,
                "9.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 9 (Thiết kế Bộ RESTful Web API Hoàn chỉnh Chuẩn Doanh Nghiệp)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/web-api/",
                """
                • **💡 1. Bản Thiết Kế Tiêu Chuẩn Của Một Module RESTful API Hoàn Hảo Trong Thực Tế:**
                  Khi bạn tự tay xây dựng một Module API hoàn chỉnh (ví dụ: Module Quản lý Sách `BooksController`), hãy đối chiếu xem nó đã hội tụ đủ **6 tiêu chuẩn vàng** của Chủ đề 9 chưa:
                  1. **Định tuyến chuẩn RESTful:** `[Route("api/v1/books")]` kết hợp các ràng buộc `{id:int:min(1)}`.
                  2. **Phân tách DTO rõ ràng:** Đầu vào dùng `CreateBookDto` / `UpdateBookDto` (có gắn Validation), đầu ra trả về `BookDto` / `PagedResult<BookDto>` bọc trong `ApiResponse<T>`.
                  3. **Mã trạng thái HTTP chính xác:**
                     - `GET` danh sách / chi tiết $\rightarrow$ `200 OK` (hoặc `404 Not Found` nếu không có).
                     - `POST` tạo mới $\rightarrow$ `201 Created` (dùng `CreatedAtAction`).
                     - `PUT` / `DELETE` thành công $\rightarrow$ `200 OK` (kèm thông báo) hoặc `204 No Content`.
                     - Lỗi dữ liệu $\rightarrow$ `400 Bad Request` (`ProblemDetails`).
                  4. **Truyền `CancellationToken ct`** vào tất cả các hàm bất đồng bộ.
                  5. **Xử lý lỗi tập trung** qua `GlobalExceptionHandler` (`IExceptionHandler`).
                  6. **Bật `CORS`, `Caching` và `Rate Limiting`** tại `Program.cs`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. API GET Danh sách (Tìm kiếm + Phân trang + CancellationToken)",
                        "📥 Đầu vào: Query parameters (keyword, page, pageSize). ⚙️ Cơ chế ngầm: Nhận qua [FromQuery], truyền kèm CancellationToken xuống Service và trả về mã 200 OK. 📤 Đầu ra: PagedResult<BookDto>.",
                        "[HttpGet]\n[ProducesResponseType(typeof(PagedResult<BookDto>), StatusCodes.Status200OK)]\npublic async Task<IActionResult> GetAll([FromQuery] string? keyword, [FromQuery] int page = 1, CancellationToken ct = default) {\n    var result = await _bookService.GetPagedAsync(keyword, page, 10, ct);\n    return Ok(result);\n}"),
                    Step(2,
                        "2. API GET Chi tiết theo ID có Route Constraint (:int:min(1))",
                        "📥 Đầu vào: Tham số id trên đường dẫn. ⚙️ Cơ chế ngầm: Ràng buộc {id:int:min(1)} chặn mọi id <= 0 hoặc chuỗi chữ ngay từ vòng gửi xe. 📤 Đầu ra: 200 OK hoặc 404 Not Found.",
                        "[HttpGet(\"{id:int:min(1)}\")]\npublic async Task<IActionResult> GetById(int id, CancellationToken ct) {\n    var dto = await _bookService.GetByIdAsync(id, ct);\n    return dto == null ? NotFound() : Ok(dto);\n}"),
                    Step(3,
                        "3. API POST Tạo mới trả về 201 Created kèm Header Location",
                        "📥 Đầu vào: CreateBookDto trong Body JSON. ⚙️ Cơ chế ngầm: [ApiController] tự động kiểm tra Validation. Sau khi tạo xong, dùng CreatedAtAction trả về mã 201 cùng đường dẫn tới hàm GetById. 📤 Đầu ra: Chuẩn mực RESTful 201 Created.",
                        "[HttpPost]\n[ProducesResponseType(typeof(BookDto), StatusCodes.Status201Created)]\n[ProducesResponseType(StatusCodes.Status400BadRequest)]\npublic async Task<IActionResult> Create([FromBody] CreateBookDto dto, CancellationToken ct) {\n    var created = await _bookService.CreateAsync(dto, ct);\n    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);\n}"),
                    Step(4,
                        "4. API PUT Cập nhật & DELETE Xóa bản ghi",
                        "📥 Đầu vào: ID trên Route và dữ liệu cập nhật. ⚙️ Cơ chế ngầm: Thực hiện cập nhật/xóa mềm tại Service, xóa Cache liên quan và trả về kết quả. 📤 Đầu ra: Bộ CRUD API hoàn chỉnh 100%.",
                        "[HttpDelete(\"{id:int:min(1)}\")]\npublic async Task<IActionResult> Delete(int id, CancellationToken ct) {\n    await _bookService.SoftDeleteAsync(id, ct);\n    return NoContent(); // 204 No Content\n}")
                },
                "(Câu hỏi tổng hợp Chủ đề 9) Trong hàm [HttpPost] Create của một RESTful Web API chuẩn mực, tại sao các lập trình viên chuyên nghiệp thường dùng lệnh return CreatedAtAction(nameof(GetById), new { id = result.Id }, result); thay vì chỉ viết return Ok(result);?",
                new List<string>
                {
                    "A. Vì CreatedAtAction trả về đúng mã trạng thái chuẩn cho thao tác tạo mới tài nguyên là '201 Created' (thay vì 200 OK) đồng thời tự động đính kèm đường dẫn URL xem chi tiết bản ghi vừa tạo vào trong Header 'Location' của HTTP Response",
                    "B. Vì hàm Ok() không trả về được dữ liệu JSON",
                    "C. Vì CreatedAtAction tự động lưu dữ liệu vào SQL Server mà không cần gọi Service",
                    "D. Hai hàm hoàn toàn giống hệt nhau"
                },
                "A",
                "Theo đặc tả HTTP/RESTful, khi một tài nguyên mới được khởi tạo thành công qua POST, máy chủ nên phản hồi mã 201 Created kèm theo Header Location chỉ dẫn đường link (GET /api/books/{id}) tới tài nguyên mới đó. Hàm CreatedAtAction thực hiện tự động cả 2 việc này."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (9.1 -> 9.10) cho Chủ đề 9 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic9 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 9);
        if (topic9 == null)
            return;

        topic9.Title = "Chủ đề 9: Xây dựng ứng dụng với ASP.NET Core Web API";
        topic9.Description = "10 Bài học con siêu chi tiết từ RESTful API & Idempotency, [ApiController] & Content Negotiation, Route Constraints, IValidatableObject & FluentValidation, IExceptionHandler & ProblemDetails (RFC 7807), Action Filters, CORS Policy, API Versioning & OpenAPI/Swagger đến Output Caching & Rate Limiting.";

        var targetLessons = GetLessons(topic9.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic9.Id)
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
