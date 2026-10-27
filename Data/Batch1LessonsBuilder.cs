using System.Text.Json;
using HoangViet.Models;

namespace HoangViet.Data;

public static class Batch1LessonsBuilder
{
    private static string ToJson<T>(T obj) => JsonSerializer.Serialize(obj);

    private static SubLesson BuildSubLesson(
        int id,
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

    // =========================================================================
    // MENU 1 (INTERN_TRACK) - CHỦ ĐỀ 1: 10 BÀI HỌC CON RIÊNG BIỆT (1.1 -> 1.10)
    // =========================================================================
    public static List<SubLesson> GetInternTopic1Lessons(int topicId)
    {
        return new List<SubLesson>
        {
            BuildSubLesson(101, topicId, "1.1",
                "Cách đọc cấu trúc Solution dự án công ty & Luồng chạy của file Program.cs",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/",
                """
                Khi nhận source code ngày đầu đi thực tập, bạn đừng mở ngẫu nhiên từng file. Hãy đọc từ file gốc của Solution (`.sln`) và điểm khởi chạy `Program.cs`.
                • **Cấu trúc thư mục chuẩn:** `Controllers/` (Cổng đón API), `Services/` (Xử lý nghiệp vụ), `Repositories/` (Giao tiếp DB), `Models/` hoặc `Entities/` (Bảng dữ liệu) và `DTOs/` (Thùng hàng JSON).
                • **Hai nửa của file `Program.cs`:**
                  1. Nửa trên `builder.Build()`: Đăng ký các dịch vụ vào Dependency Injection Container (`builder.Services.AddScoped...`, `AddDbContext...`).
                  2. Nửa dưới `builder.Build()`: Thiết lập đường ống Middleware (`app.UseHttpsRedirection()`, `app.UseAuthentication()`, `app.UseAuthorization()`, `app.MapControllers()`).
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Khởi tạo WebApplicationBuilder", StepDetail = "Đầu vào: Tham số dòng lệnh args & file appsettings.json ➔ Xử lý: Nạp cấu hình kết nối DB, Logging ➔ Đẩy ra: Đối tượng builder.", CodeSnippet = "var builder = WebApplication.CreateBuilder(args);\nvar conn = builder.Configuration.GetConnectionString(\"DefaultConnection\");" },
                    new() { StepNumber = 2, StepTitle = "Đăng ký DI Container (Services)", StepDetail = "Đầu vào: builder.Services ➔ Xử lý: Đăng ký DbContext, Controller, Service và Repository vào bộ nhớ quản lý ➔ Đẩy ra: Danh sách dịch vụ sẵn sàng cấp phát.", CodeSnippet = "builder.Services.AddControllers();\nbuilder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn));\nbuilder.Services.AddScoped<IOrderService, OrderService>();" },
                    new() { StepNumber = 3, StepTitle = "Đóng gói ứng dụng (builder.Build)", StepDetail = "Đầu vào: Cấu hình đã đăng ký ➔ Xử lý: Khởi tạo WebApplication ➔ Đẩy ra: Biến app để gắn các trạm kiểm soát Middleware.", CodeSnippet = "var app = builder.Build();" },
                    new() { StepNumber = 4, StepTitle = "Chạy Middleware & Lắng nghe Request", StepDetail = "Đầu vào: HTTP Request ➔ Xử lý: Đi qua UseAuthentication -> UseAuthorization -> MapControllers ➔ Đẩy ra: Mở cổng Server với app.Run().", CodeSnippet = "app.UseAuthentication();\napp.UseAuthorization();\napp.MapControllers();\napp.Run();" }
                },
                "Trong file Program.cs của ASP.NET Core, các dòng lệnh `builder.Services.AddScoped<...>()` dùng để làm gì?",
                new List<string>
                {
                    "A. Đăng ký các Service/Repository vào Dependency Injection Container trước khi gọi builder.Build()",
                    "B. Xóa cơ sở dữ liệu SQL Server",
                    "C. Đổi màu giao diện trang web",
                    "D. Trả về mã lỗi 404 Not Found"
                },
                "A. Đăng ký các Service/Repository vào Dependency Injection Container trước khi gọi builder.Build()",
                "Chính xác! Mọi dịch vụ (DbContext, Service, Repository) đều được đăng ký vào `builder.Services` ở nửa trên của `Program.cs` trước dòng `var app = builder.Build();`."
            ),

            BuildSubLesson(102, topicId, "1.2",
                "Đọc hiểu dữ liệu JSON từ Client ánh xạ vào các kiểu C# (string, int, decimal, DateTime, Guid)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/built-in-types",
                """
                Khi Frontend hoặc Postman gửi một gói JSON lên Backend, ASP.NET Core sẽ tự động dịch từng trường JSON sang kiểu dữ liệu C# tương ứng trong class DTO:
                • **Chuỗi văn bản (`"name": "Áo sơ mi"`):** Ánh xạ vào `string` trong C#.
                • **Số lượng nguyên (`"quantity": 5`):** Ánh xạ vào `int` (hoặc `long` nếu số cực lớn).
                • **Tiền tệ / Giá bán (`"price": 299000.50`):** Bắt buộc dùng `decimal` trong C# (tuyệt đối không dùng `float`/`double` cho tiền tệ vì dễ bị sai số thập phân).
                • **Thời gian (`"createdAt": "2026-10-20T08:30:00Z"`):** Ánh xạ vào `DateTime` theo chuẩn ISO-8601.
                • **Mã định danh 36 ký tự (`"transactionId": "f47ac10b-58cc-4372-a567-0e02b2c3d479"`):** Ánh xạ vào `Guid`.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Client gửi chuỗi JSON thô", StepDetail = "Đầu vào: Người dùng nhập form ➔ Xử lý: Đóng gói thành văn bản JSON chuẩn ➔ Đẩy ra: HTTP Request Body.", CodeSnippet = "{\n  \"orderCode\": \"f47ac10b-58cc-4372-a567-0e02b2c3d479\",\n  \"customerName\": \"Hoàng Việt\",\n  \"quantity\": 2,\n  \"totalAmount\": 590000.00\n}" },
                    new() { StepNumber = 2, StepTitle = "System.Text.Json đọc từng kiểu", StepDetail = "Đầu vào: Chuỗi JSON ➔ Xử lý: Đối chiếu tên thuộc tính (không phân biệt hoa thường) ➔ Đẩy ra: Giá trị đúng kiểu C#.", CodeSnippet = "// Chuỗi UUID 36 ký tự -> Guid\n// Số tiền -> decimal\n// Số lượng -> int" },
                    new() { StepNumber = 3, StepTitle = "Đổ dữ liệu vào Class DTO C#", StepDetail = "Đầu vào: Các giá trị đã ép kiểu ➔ Xử lý: Gán vào từng Property { get; set; } ➔ Đẩy ra: Đối tượng CreateOrderDto.", CodeSnippet = "public class CreateOrderDto\n{\n    public Guid OrderCode { get; set; }\n    public string CustomerName { get; set; } = string.Empty;\n    public int Quantity { get; set; }\n    public decimal TotalAmount { get; set; }\n}" },
                    new() { StepNumber = 4, StepTitle = "Truyền DTO vào hàm Controller", StepDetail = "Đầu vào: Đối tượng CreateOrderDto ➔ Xử lý: Truyền làm tham số hàm Action ➔ Đẩy ra: Sẵn sàng chuyển xuống tầng Service.", CodeSnippet = "[HttpPost]\npublic IActionResult Create([FromBody] CreateOrderDto dto)\n{\n    return Ok(dto.TotalAmount);\n}" }
                },
                "Trong dự án thực tế tại công ty, kiểu dữ liệu C# nào luôn được dùng để lưu trữ Giá tiền / Số dư tài khoản nhằm tránh sai số làm tròn?",
                new List<string>
                {
                    "A. decimal",
                    "B. float",
                    "C. char",
                    "D. bool"
                },
                "A. decimal",
                "Chính xác! Trong C#, mọi dữ liệu tài chính, giá bán, tổng tiền hóa đơn bắt buộc dùng kiểu `decimal` (128-bit chính xác cao) để không bao giờ bị lệch xu."
            ),

            BuildSubLesson(103, topicId, "1.3",
                "Cách đọc và xử lý dấu hỏi chấm (?, ?., ??) trong code dự án để tránh lỗi Null sập API",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/null-coalescing-operator",
                """
                Lỗi phổ biến số 1 làm sập API (500 Internal Server Error) là **`NullReferenceException`**. Khi đọc code dự án của các anh Senior, bạn sẽ thấy 3 loại dấu hỏi chấm cực kỳ lợi hại:
                1. **Kiểu cho phép rỗng (`string?`, `int?`, `Order?`):** Báo hiệu biến hoặc kết quả trả về từ Database có thể bằng `null` (không tìm thấy).
                2. **Toán tử truy cập an toàn (`user?.Profile?.AvatarUrl`):** Nếu `user` hoặc `Profile` bị `null`, nó dừng lại và trả về `null` nhẹ nhàng chứ không làm văng lỗi sập chương trình.
                3. **Toán tử giá trị dự phòng (`name ?? "Khách vãng lai"`):** Nếu vế bên trái khác `null` thì lấy vế trái; nếu vế trái bị `null` thì lấy ngay giá trị mặc định bên phải!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Truy vấn DB trả về kiểu có dấu ?", StepDetail = "Đầu vào: Mã khách hàng id ➔ Xử lý: Tìm trong DB bằng FirstOrDefaultAsync ➔ Đẩy ra: Biến kiểu Customer? (có thể null).", CodeSnippet = "Customer? customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == id);" },
                    new() { StepNumber = 2, StepTitle = "Đọc thuộc tính an toàn bằng ?.", StepDetail = "Đầu vào: Biến customer ➔ Xử lý: Dùng dấu ?. để lấy tên thành phố ➔ Đẩy ra: Chuỗi tên thành phố hoặc null (không bị lỗi).", CodeSnippet = "string? city = customer?.Address?.CityName;" },
                    new() { StepNumber = 3, StepTitle = "Gán giá trị mặc định bằng ??", StepDetail = "Đầu vào: Biến city có thể null ➔ Xử lý: Kiểm tra bằng toán tử ?? ➔ Đẩy ra: Chuỗi chắc chắn không rỗng.", CodeSnippet = "string displayCity = city ?? \"Chưa cập nhật địa chỉ\";" },
                    new() { StepNumber = 4, StepTitle = "Chặn null kết hợp throw bằng ??", StepDetail = "Đầu vào: Kết quả tìm kiếm ➔ Xử lý: Nếu null thì ném ngoại lệ báo không tìm thấy ngay trên 1 dòng ➔ Đẩy ra: Đối tượng an toàn.", CodeSnippet = "var validCustomer = customer ?? throw new KeyNotFoundException(\"Không tìm thấy khách hàng!\");" }
                },
                "Đoạn code `string role = user?.RoleName ?? \"Member\";` sẽ trả về kết quả gì nếu biến `user` đang bị `null`?",
                new List<string>
                {
                    "A. Bị sập chương trình với lỗi NullReferenceException",
                    "B. Trả về chuỗi \"Member\" một cách an toàn",
                    "C. Trả về số 0",
                    "D. Xóa biến user"
                },
                "B. Trả về chuỗi \"Member\" một cách an toàn",
                "Xuất sắc! Nhờ `user?.RoleName` trả về `null` an toàn, toán tử `??` lập tức lấy giá trị dự phòng bên phải là `\"Member\"`."
            ),

            BuildSubLesson(104, topicId, "1.4",
                "Đọc luồng xử lý Chuỗi (string.IsNullOrWhiteSpace, chuẩn hóa dữ liệu request)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.string.isnullorwhitespace",
                """
                Dữ liệu do người dùng nhập vào từ giao diện luôn tiềm ẩn khoảng trắng thừa (`"  admin@gmail.com  "`) hoặc chuỗi toàn dấu cách (`"    "`).
                Khi đọc luồng đầu vào ở `Controller` hoặc `Service`, bạn sẽ luôn gặp cụm xử lý chuẩn hóa chuỗi:
                • **`string.IsNullOrWhiteSpace(input)`:** Kiểm tra cùng lúc 3 trường hợp: `null`, chuỗi rỗng `""`, hoặc chuỗi chỉ toàn khoảng trắng `"   "`.
                • **`.Trim()`:** Cắt sạch khoảng trắng thừa ở 2 đầu chuỗi.
                • **`.ToLowerInvariant()` / `.ToUpperInvariant()`:** Đưa email hoặc mã voucher về cùng chữ thường/chữ hoa trước khi so sánh trong Database.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Nhận chuỗi thô từ Request", StepDetail = "Đầu vào: Người dùng nhập Email và Mã giảm giá có lẫn khoảng trắng ➔ Đẩy ra: DTO chứa chuỗi thô.", CodeSnippet = "// Người dùng gửi lên: dto.Email = \"  VietHoang@Gmail.com  \"" },
                    new() { StepNumber = 2, StepTitle = "Kiểm tra rỗng bằng IsNullOrWhiteSpace", StepDetail = "Đầu vào: dto.Email ➔ Xử lý: Chặn ngay nếu người dùng bỏ trống hoặc chỉ gõ phím Space ➔ Đẩy ra: Luồng hợp lệ.", CodeSnippet = "if (string.IsNullOrWhiteSpace(dto.Email))\n    return BadRequest(\"Email không được để trống!\");" },
                    new() { StepNumber = 3, StepTitle = "Cắt khoảng trắng & Chuẩn hóa chữ thường", StepDetail = "Đầu vào: Chuỗi hợp lệ ➔ Xử lý: Gọi .Trim().ToLowerInvariant() ➔ Đẩy ra: Email sạch sẽ chuẩn tắc.", CodeSnippet = "string normalizedEmail = dto.Email.Trim().ToLowerInvariant();\n// Kết quả: \"viethoang@gmail.com\"" },
                    new() { StepNumber = 4, StepTitle = "Đẩy chuỗi sạch xuống truy vấn DB", StepDetail = "Đầu vào: normalizedEmail ➔ Xử lý: Tìm kiếm chính xác trong bảng Users ➔ Đẩy ra: Bản ghi người dùng.", CodeSnippet = "var user = await _userRepo.FindByEmailAsync(normalizedEmail);" }
                },
                "Tại sao trong dự án thực tế các lập trình viên luôn ưu tiên dùng `string.IsNullOrWhiteSpace(str)` thay vì chỉ kiểm tra `str == \"\"`?",
                new List<string>
                {
                    "A. Vì nó bắt được cả 3 trường hợp: chuỗi bị null, chuỗi rỗng \"\", và chuỗi chỉ toàn dấu cách \"   \"",
                    "B. Vì nó tự động dịch sang tiếng Anh",
                    "C. Vì C# không có toán tử ==",
                    "D. Vì nó tự động lưu vào SQL Server"
                },
                "A. Vì nó bắt được cả 3 trường hợp: chuỗi bị null, chuỗi rỗng \"\", và chuỗi chỉ toàn dấu cách \"   \"",
                "Chính xác! Nếu người dùng gõ 5 dấu cách `\"     \"`, `str == \"\"` sẽ bị lọt qua, còn `string.IsNullOrWhiteSpace(str)` sẽ phát hiện và chặn lại ngay."
            ),

            BuildSubLesson(105, topicId, "1.5",
                "Đọc luồng kiểm tra dữ liệu đầu vào (Validation, TryParse) tại cửa ngõ API",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/validation",
                """
                Không bao giờ được tin tưởng tuyệt đối dữ liệu gửi từ phía Client! Tại cửa ngõ API, dự án thực tế sử dụng 2 lớp kiểm tra đầu vào:
                1. **Data Annotations trên DTO (`[Required]`, `[Range]`, `[EmailAddress]`, `[MaxLength]`):** Tự động kiểm tra ngay khi JSON vừa ánh xạ vào DTO.
                2. **Ép kiểu an toàn với `TryParse` (`int.TryParse`, `DateTime.TryParse`, `Guid.TryParse`):** Khi nhận tham số dạng chuỗi từ QueryString hoặc cấu hình, luôn dùng `TryParse` (trả về `true/false`) thay vì `Parse` để không bị văng lỗi `FormatException`.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Gắn nhãn Validation trên DTO", StepDetail = "Đầu vào: Định nghĩa class DTO ➔ Xử lý: Đặt các thuộc tính [Required], [Range] ➔ Đẩy ra: Quy tắc kiểm duyệt tự động.", CodeSnippet = "public class CreateProductDto\n{\n    [Required(ErrorMessage = \"Tên SP bắt buộc nhập\")]\n    public string Name { get; set; } = string.Empty;\n\n    [Range(1000, 500000000)]\n    public decimal Price { get; set; }\n}" },
                    new() { StepNumber = 2, StepTitle = "Kiểm tra ModelState tại Controller", StepDetail = "Đầu vào: Request gửi tới Controller ➔ Xử lý: Kiểm tra ModelState.IsValid ➔ Đẩy ra: Chặn 400 BadRequest nếu vi phạm.", CodeSnippet = "if (!ModelState.IsValid)\n    return BadRequest(ModelState);" },
                    new() { StepNumber = 3, StepTitle = "Dùng TryParse cho tham số chuỗi", StepDetail = "Đầu vào: Chuỗi pageStr từ URL ➔ Xử lý: Gọi int.TryParse(pageStr, out int page) ➔ Đẩy ra: Số nguyên an toàn.", CodeSnippet = "if (!int.TryParse(rawId, out int validId))\n    return BadRequest(\"Mã sản phẩm phải là số nguyên!\");" },
                    new() { StepNumber = 4, StepTitle = "Chuyển dữ liệu sạch xuống Service", StepDetail = "Đầu vào: Dữ liệu đã qua 100% chốt chặn ➔ Xử lý: Gọi Service xử lý ➔ Đẩy ra: Đảm bảo tầng nghiệp vụ không bao giờ nhận rác.", CodeSnippet = "var result = await _productService.CreateAsync(dto);" }
                },
                "Sự khác biệt quan trọng nhất giữa `int.Parse(str)` và `int.TryParse(str, out int number)` khi đọc dữ liệu đầu vào là gì?",
                new List<string>
                {
                    "A. int.TryParse không làm sập chương trình khi chuỗi sai định dạng mà trả về false để ta xử lý êm ái",
                    "B. int.Parse chạy không cần biến str",
                    "C. int.TryParse chỉ dùng cho kiểu ngày tháng",
                    "D. Hai hàm hoàn toàn giống hệt nhau"
                },
                "A. int.TryParse không làm sập chương trình khi chuỗi sai định dạng mà trả về false để ta xử lý êm ái",
                "Chuẩn xác! Nếu người dùng truyền chữ `\"abc\"`, `int.Parse` sẽ ném `FormatException` gây lỗi 500, còn `int.TryParse` trả về `false` để ta trả về mã 400 Bad Request lịch sự."
            ),

            BuildSubLesson(106, topicId, "1.6",
                "Đọc luồng if-else theo kỹ thuật 'Guard Clauses' (Chặn lỗi trả về BadRequest/NotFound ngay đầu hàm)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/coding-style/coding-conventions",
                """
                Sinh viên mới học thường viết `if (...) { if (...) { if (...) { ... } } }` lồng nhau 4-5 tầng (gọi là *Arrow Anti-pattern* - code hình mũi tên rất khó đọc).
                Trong dự án thực tế tại công ty, các Senior luôn viết theo kỹ thuật **Guard Clauses (Các mệnh đề bảo vệ đầu hàm)**:
                • Kiểm tra các trường hợp lỗi hoặc không thỏa mãn trước, nếu dính lỗi thì `return` hoặc `throw` **ngay lập tức** để thoát hàm.
                • Luồng chạy chính (Happy Path) nằm thẳng hàng ở ngoài cùng dưới đáy hàm, không cần `else` rườm rà!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Trạm gác 1: Kiểm tra đầu vào rỗng", StepDetail = "Đầu vào: Tham số request ➔ Xử lý: Nếu số lượng <= 0 thì return BadRequest ngay dòng đầu ➔ Đẩy ra: Chỉ cho phép số lượng dương đi tiếp.", CodeSnippet = "if (dto.Quantity <= 0)\n    return BadRequest(\"Số lượng mua phải lớn hơn 0.\");" },
                    new() { StepNumber = 2, StepTitle = "Trạm gác 2: Kiểm tra tồn tại trong DB", StepDetail = "Đầu vào: ProductId ➔ Xử lý: Nếu tìm trong DB bằng null thì return NotFound ngay ➔ Đẩy ra: Đảm bảo biến product chắc chắn khác null.", CodeSnippet = "var product = await _repo.GetByIdAsync(dto.ProductId);\nif (product is null)\n    return NotFound(\"Không tìm thấy sản phẩm.\");" },
                    new() { StepNumber = 3, StepTitle = "Trạm gác 3: Kiểm tra đủ tồn kho", StepDetail = "Đầu vào: product.Stock ➔ Xử lý: Nếu tồn kho nhỏ hơn số mua thì chặn lại ➔ Đẩy ra: Đủ điều kiện tạo đơn.", CodeSnippet = "if (product.Stock < dto.Quantity)\n    return BadRequest(\"Sản phẩm đã hết hàng trong kho.\");" },
                    new() { StepNumber = 4, StepTitle = "Luồng chính (Happy Path) thẳng tắp", StepDetail = "Đầu vào: Đã vượt qua cả 3 trạm gác ➔ Xử lý: Tạo đơn hàng và trả về 200 OK mà không cần lồng bất kỳ khối else nào!", CodeSnippet = "var order = await _orderService.PlaceOrderAsync(product, dto.Quantity);\nreturn Ok(order);" }
                },
                "Lợi ích lớn nhất của kỹ thuật viết code 'Guard Clauses' (kiểm tra lỗi và return/throw ngay ở đầu hàm) là gì?",
                new List<string>
                {
                    "A. Loại bỏ các khối if-else lồng nhau sâu, giúp người đọc code nhìn thấy ngay các điều kiện chặn lỗi và luồng chính rõ ràng",
                    "B. Làm cho file code dài gấp 10 lần",
                    "C. Bắt buộc phải dùng vòng lặp while",
                    "D. Không cần kiểm tra lỗi nữa"
                },
                "A. Loại bỏ các khối if-else lồng nhau sâu, giúp người đọc code nhìn thấy ngay các điều kiện chặn lỗi và luồng chính rõ ràng",
                "Chính xác! Khi đọc một hàm viết theo Guard Clauses, bạn chỉ cần lướt qua các dòng `if (...) return/throw` ở đầu hàm là nắm được toàn bộ điều kiện ràng buộc của chức năng đó."
            ),

            BuildSubLesson(107, topicId, "1.7",
                "Đọc luồng Enum kết hợp switch trong quản lý trạng thái đơn hàng/nghiệp vụ",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/enum",
                """
                Trong cơ sở dữ liệu công ty, cột trạng thái (`Status`) thường lưu dưới dạng số nguyên (`0, 1, 2, 3...`). Nếu trong code C# viết `if (order.Status == 2)` thì người đọc sẽ không hiểu số `2` nghĩa là gì (gọi là *Magic Number*).
                Vì vậy, dự án thực tế luôn định nghĩa **`enum OrderStatus`** kết hợp với **`switch` (hoặc `switch expression`)**:
                • Giúp đọc code như đọc tiếng Anh tự nhiên: `OrderStatus.Pending`, `OrderStatus.Paid`, `OrderStatus.Shipped`, `OrderStatus.Cancelled`.
                • Kết hợp `switch expression` (`=>`) của C# hiện đại giúp phân luồng xử lý theo từng trạng thái cực kỳ ngắn gọn.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Định nghĩa Enum trạng thái rõ nghĩa", StepDetail = "Đầu vào: Quy trình nghiệp vụ đơn hàng ➔ Xử lý: Gán tên định danh cho từng con số ➔ Đẩy ra: Kiểu enum OrderStatus.", CodeSnippet = "public enum OrderStatus\n{\n    Pending = 0,   // Chờ thanh toán\n    Paid = 1,      // Đã thanh toán\n    Shipped = 2,   // Đang giao hàng\n    Cancelled = 3  // Đã hủy\n}" },
                    new() { StepNumber = 2, StepTitle = "Nhận trạng thái hiện tại của Entity", StepDetail = "Đầu vào: Bản ghi Order từ DB ➔ Xử lý: EF Core tự động ánh xạ số nguyên trong DB sang kiểu OrderStatus ➔ Đẩy ra: order.Status.", CodeSnippet = "Order order = await _orderRepo.GetByIdAsync(orderId);\nOrderStatus current = order.Status;" },
                    new() { StepNumber = 3, StepTitle = "Phân nhánh luồng bằng Switch Expression", StepDetail = "Đầu vào: Biến current ➔ Xử lý: Đối chiếu từng trạng thái bằng mũi tên => ➔ Đẩy ra: Thông báo hoặc hành động tương ứng.", CodeSnippet = "string statusMessage = current switch\n{\n    OrderStatus.Pending => \"Đơn hàng đang chờ thanh toán\",\n    OrderStatus.Paid => \"Đã thu tiền, chuẩn bị xuất kho\",\n    OrderStatus.Shipped => \"Đơn vị vận chuyển đang giao\",\n    OrderStatus.Cancelled => \"Đơn hàng đã bị hủy\",\n    _ => \"Trạng thái không xác định\"\n};" },
                    new() { StepNumber = 4, StepTitle = "Kiểm soát chuyển đổi trạng thái hợp lệ", StepDetail = "Đầu vào: Yêu cầu hủy đơn ➔ Xử lý: Chỉ cho phép hủy nếu chưa giao hàng (current != OrderStatus.Shipped) ➔ Đẩy ra: Lưu trạng thái mới.", CodeSnippet = "if (order.Status == OrderStatus.Shipped)\n    throw new InvalidOperationException(\"Hàng đã giao không thể hủy!\");\norder.Status = OrderStatus.Cancelled;" }
                },
                "Tại sao trong dự án thực tế người ta dùng `OrderStatus.Paid` (kiểu `enum`) thay vì viết trực tiếp con số `if (order.Status == 1)`?",
                new List<string>
                {
                    "A. Để tránh 'con số ma thuật' (Magic Number), giúp bất kỳ ai đọc code cũng hiểu ngay ý nghĩa trạng thái nghiệp vụ",
                    "B. Vì C# không cho phép so sánh số nguyên",
                    "C. Để ẩn code không cho đồng nghiệp đọc",
                    "D. Vì enum tự động gửi email cho khách"
                },
                "A. Để tránh 'con số ma thuật' (Magic Number), giúp bất kỳ ai đọc code cũng hiểu ngay ý nghĩa trạng thái nghiệp vụ",
                "Chính xác! Dùng `enum` giúp code tự giải thích ý nghĩa nghiệp vụ mà không cần phải mở tài liệu tra xem số 1, số 2 là trạng thái gì."
            ),

            BuildSubLesson(108, topicId, "1.8",
                "Đọc luồng vòng lặp foreach khi biến đổi danh sách dữ liệu trước khi trả về Response",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/iteration-statements#the-foreach-statement",
                """
                Khi tầng `Service` nhận về một danh sách các bản ghi từ `Repository` (ví dụ `List<Product> entities`), trước khi trả về cho `Controller`, nó thường dùng vòng lặp **`foreach`** (hoặc `.Select()` của LINQ) để duyệt qua từng phần tử:
                • Tính toán thêm thông tin hiển thị (như tính giá sau khuyến mãi, ghép họ và tên).
                • Đóng gói từng `Entity` sang `DTO` rồi thêm vào danh sách kết quả `List<ProductDto>` để trả về JSON.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Lấy danh sách Entity gốc từ Repository", StepDetail = "Đầu vào: Yêu cầu lấy danh sách SP ➔ Xử lý: Gọi Repository ➔ Đẩy ra: List<Product> chứa dữ liệu thô từ DB.", CodeSnippet = "List<Product> products = await _productRepo.GetActiveProductsAsync();" },
                    new() { StepNumber = 2, StepTitle = "Khởi tạo giỏ chứa danh sách DTO", StepDetail = "Đầu vào: Số lượng phần tử ➔ Xử lý: Tạo mới List<ProductDto> rỗng để chuẩn bị hứng kết quả ➔ Đẩy ra: Biến resultDtos.", CodeSnippet = "var resultDtos = new List<ProductDto>();" },
                    new() { StepNumber = 3, StepTitle = "Duyệt foreach qua từng phần tử", StepDetail = "Đầu vào: Từng item trong products ➔ Xử lý: Tính giá sau giảm và tạo đối tượng ProductDto ➔ Đẩy ra: Thêm vào resultDtos.", CodeSnippet = "foreach (var item in products)\n{\n    decimal finalPrice = item.Price * (1 - item.DiscountPercent / 100m);\n    resultDtos.Add(new ProductDto(item.Id, item.Name, finalPrice));\n}" },
                    new() { StepNumber = 4, StepTitle = "Trả danh sách DTO về Controller", StepDetail = "Đầu vào: Danh sách resultDtos đã hoàn thiện ➔ Xử lý: Return về cho Controller ➔ Đẩy ra: JSON Array trả về Client.", CodeSnippet = "return resultDtos;" }
                },
                "Trong tầng Service, vòng lặp `foreach (var item in entities)` thường được sử dụng vào mục đích gì trước khi trả dữ liệu về Controller?",
                new List<string>
                {
                    "A. Duyệt qua từng bản ghi Entity để tính toán nghiệp vụ và đóng gói sang danh sách DTO an toàn",
                    "B. Để khởi động lại máy chủ SQL Server",
                    "C. Để xóa sạch các file trong project",
                    "D. Để thay thế hoàn toàn file Program.cs"
                },
                "A. Duyệt qua từng bản ghi Entity để tính toán nghiệp vụ và đóng gói sang danh sách DTO an toàn",
                "Chuẩn xác! Duyệt `foreach` (hoặc `.Select()`) giúp biến đổi từng đối tượng `Entity` thô từ DB thành `DTO` đã tính toán đầy đủ cho giao diện hiển thị."
            ),

            BuildSubLesson(109, topicId, "1.9",
                "Kỹ năng đọc 'Chữ ký hàm' (Method Signature): Nhìn dòng khai báo biết ngay đầu vào, đầu ra của luồng",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/methods",
                """
                Bí quyết để đọc một file code dài 500 dòng mà không bị ngợp là **chỉ đọc dòng khai báo hàm (Method Signature)** trước khi đọc ruột hàm!
                Ví dụ nhìn dòng:
                `public async Task<OrderResponseDto?> CreateOrderAsync(int userId, CreateOrderDto input)`
                Bạn biết ngay 4 thông tin sống còn:
                1. **`public`:** Hàm này được phép gọi từ tầng khác (như Controller gọi xuống).
                2. **`async Task<...>`:** Hàm chạy bất đồng bộ (có gọi DB hoặc API ngoài), khi gọi phải có từ khóa `await`.
                3. **`OrderResponseDto?`:** Kết quả đầu ra (Output) của hàm là thùng hàng `OrderResponseDto` (có dấu `?` nghĩa là có thể trả về `null`).
                4. **`(int userId, CreateOrderDto input)`:** Đầu vào (Input) mà hàm yêu cầu phải truyền xuống.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Đọc phạm vi & Bất đồng bộ (public async)", StepDetail = "Đầu vào: Dòng khai báo hàm ➔ Xử lý: Nhìn từ khóa public async ➔ Biết ngay: Hàm mở cho tầng ngoài gọi và chạy không khóa luồng.", CodeSnippet = "public async Task<ProductDto?> GetByIdAsync(int productId)" },
                    new() { StepNumber = 2, StepTitle = "Đọc kiểu trả về nằm trong Task<T>", StepDetail = "Đầu vào: Phần nằm giữa cặp ngoặc nhọn Task<ProductDto?> ➔ Xử lý: Xác định Output ➔ Biết ngay: Hàm trả về 1 đối tượng ProductDto hoặc null.", CodeSnippet = "// Task<ProductDto?> -> Sau khi await sẽ nhận được kiểu ProductDto?" },
                    new() { StepNumber = 3, StepTitle = "Đọc tên hàm theo chuẩn Động từ + Async", StepDetail = "Đầu vào: Tên hàm GetByIdAsync ➔ Xử lý: Đọc động từ tiếng Anh ➔ Biết ngay: Nhiệm vụ là lấy thông tin theo mã ID.", CodeSnippet = "// Quy ước .NET: Các hàm bất đồng bộ luôn có đuôi ...Async" },
                    new() { StepNumber = 4, StepTitle = "Đọc danh sách tham số đầu vào (Parameters)", StepDetail = "Đầu vào: Phần trong ngoặc tròn (int productId) ➔ Xử lý: Xác định Input ➔ Biết ngay: Phải truyền vào 1 số nguyên ID.", CodeSnippet = "// Khi gọi từ Controller:\nProductDto? dto = await _productService.GetByIdAsync(105);" }
                },
                "Khi bạn nhìn thấy chữ ký hàm `public async Task<List<OrderDto>> GetOrdersByUserAsync(Guid userId)`, hàm này nhận đầu vào là gì và trả về đầu ra là gì sau khi `await`?",
                new List<string>
                {
                    "A. Đầu vào là mã `Guid userId`, đầu ra là danh sách `List<OrderDto>`",
                    "B. Đầu vào là `List<OrderDto>`, đầu ra là kiểu `bool`",
                    "C. Hàm không có đầu vào và không trả về gì",
                    "D. Đầu vào là chuỗi văn bản, đầu ra là số nguyên"
                },
                "A. Đầu vào là mã `Guid userId`, đầu ra là danh sách `List<OrderDto>`",
                "Chính xác 100%! Nhìn trong ngoặc tròn `(Guid userId)` là biết ngay Input, và nhìn bên trong `Task<List<OrderDto>>` là biết ngay Output trả về."
            ),

            BuildSubLesson(110, topicId, "1.10",
                "Kỹ năng sống còn: Đặt Breakpoint (F9) và dùng F10 / F11 trên Visual Studio để dò luồng dữ liệu chạy từng dòng",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/visualstudio/debugger/getting-started-with-the-debugger",
                """
                Khi vào công ty thực tập, bạn không cần ngồi đoán mò xem dữ liệu chạy đi đâu! Hãy dùng công cụ **Debugger của Visual Studio**:
                1. **Phím `F9` (Toggle Breakpoint):** Đặt một chấm đỏ tại dòng đầu tiên của hàm trong `Controller`. Khi gửi Request từ Postman, chương trình sẽ **tạm dừng ngay tại chấm đỏ đó**, cho phép bạn rê chuột vào biến để xem tận mắt dữ liệu bên trong!
                2. **Phím `F10` (Step Over):** Bước qua dòng hiện tại để sang dòng kế tiếp trong cùng 1 hàm.
                3. **Phím `F11` (Step Into):** Nhảy sâu vào bên trong hàm đang được gọi (ví dụ đang đứng ở `Controller` bấm `F11` sẽ nhảy thẳng vào bên trong hàm của `Service`!).
                4. **Phím `F5` (Continue):** Cho chương trình chạy tiếp bình thường đến hết luồng.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Đặt chấm đỏ Breakpoint bằng phím F9", StepDetail = "Đầu vào: Mở file OrdersController.cs ➔ Xử lý: Đưa con trỏ vào dòng đầu hàm CreateOrder và bấm F9 ➔ Đẩy ra: Chấm đỏ chờ sẵn.", CodeSnippet = "[HttpPost]\npublic async Task<IActionResult> Create(CreateOrderDto dto)\n{\n    // 🔴 [F9 Breakpoint tại đây] Rê chuột vào 'dto' để xem JSON đã vào đúng chưa!\n    var result = await _orderService.CreateAsync(dto);" },
                    new() { StepNumber = 2, StepTitle = "Bấm Send trên Postman để bắt dính luồng", StepDetail = "Đầu vào: Gửi Request từ Postman ➔ Xử lý: Visual Studio tự động bật sáng dòng màu vàng tại chấm đỏ ➔ Đẩy ra: Kiểm tra giá trị biến thực tế.", CodeSnippet = "// Lúc này dòng code tô màu vàng, bạn mở cửa sổ Locals / Watch để soi từng thuộc tính của dto." },
                    new() { StepNumber = 3, StepTitle = "Bấm F11 (Step Into) để nhảy vào Service", StepDetail = "Đầu vào: Đang đứng tại dòng gọi _orderService.CreateAsync(dto) ➔ Xử lý: Bấm F11 ➔ Đẩy ra: Màn hình tự mở file OrderService.cs!", CodeSnippet = "// Màn hình tự động nhảy vào file OrderService.cs:\npublic async Task<OrderDto> CreateAsync(CreateOrderDto dto)\n{\n    // Tiếp tục bấm F10 để đi từng dòng kiểm tra nghiệp vụ!" },
                    new() { StepNumber = 4, StepTitle = "Bấm F10 (Step Over) & F5 (Continue)", StepDetail = "Đầu vào: Đang dò từng dòng trong Service ➔ Xử lý: Bấm F10 để chạy qua từng lệnh if, bấm F5 để hoàn tất trả Response về Postman.", CodeSnippet = "    await _repo.AddAsync(order);\n    return MapToDto(order); // Bấm F5 để trả kết quả về Postman!\n}" }
                },
                "Khi đang dừng ở Breakpoint tại dòng `var res = await _orderService.CreateAsync(dto);` trong Controller, bạn bấm phím nào trên Visual Studio để **nhảy sâu vào bên trong** hàm `CreateAsync` của tầng Service?",
                new List<string>
                {
                    "A. Phím F11 (Step Into)",
                    "B. Phím Ctrl + Z",
                    "C. Phím Alt + F4",
                    "D. Phím Delete"
                },
                "A. Phím F11 (Step Into)",
                "Tuyệt vời! `F9` đặt chấm đỏ, `F10` đi ngang từng dòng, và `F11` (Step Into) giúp bạn chui thẳng vào bên trong hàm của tầng tiếp theo để theo dõi trọn vẹn luồng dữ liệu!"
            )
        };
    }

    // =========================================================================
    // MENU 1 (INTERN_TRACK) - CHỦ ĐỀ 2: 9 BÀI HỌC CON RIÊNG BIỆT (2.1 -> 2.9)
    // =========================================================================
    public static List<SubLesson> GetInternTopic2Lessons(int topicId)
    {
        return new List<SubLesson>
        {
            BuildSubLesson(201, topicId, "2.1",
                "Phân biệt 4 loại Class/Record trong dự án thực tế: Entity, DTO, ViewModel và Service/Controller",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/aspnet/core/web-api/action-return-types",
                """
                Khi mở một dự án công ty, bạn sẽ thấy hàng trăm file `.cs` đều khai báo `public class ...`. Để không bị rối, hãy chia chúng thành **4 nhóm vai trò rõ rệt**:
                1. **`Entity` (Nằm ở `Models/` hoặc `Entities/`):** Đại diện 1-1 cho bảng trong SQL Server (chứa dữ liệu lưu trữ).
                2. **`DTO` (Nằm ở `DTOs/` hoặc `Contracts/`):** Thùng hàng vận chuyển JSON qua lại giữa Client và Web API (chỉ chứa thuộc tính, không viết hàm xử lý).
                3. **`ViewModel` (Nằm ở `ViewModels/` trong dự án MVC):** Đóng gói dữ liệu tổng hợp từ nhiều nguồn để truyền ra màn hình giao diện `.cshtml`.
                4. **`Service` & `Controller` (Nằm ở `Services/`, `Controllers/`):** Các class hành động (chứa các phương thức xử lý luồng, không dùng để lưu dữ liệu).
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Nhóm 1: Entity (Ánh xạ bảng SQL)", StepDetail = "Đầu vào: Thiết kế bảng Database ➔ Vai trò: Lưu trữ toàn bộ cột trong SQL Server ➔ Đặc điểm: Có khóa chính Id và quan hệ khóa ngoại.", CodeSnippet = "public class Product // Entity trong bảng SQL\n{\n    public int Id { get; set; }\n    public string Name { get; set; } = string.Empty;\n    public decimal ImportCost { get; set; } // Giá vốn nội bộ\n}" },
                    new() { StepNumber = 2, StepTitle = "Nhóm 2: DTO (Thùng hàng Web API)", StepDetail = "Đầu vào: Yêu cầu giao tiếp JSON ➔ Vai trò: Nhận/Trả dữ liệu gọn nhẹ, giấu cột nhạy cảm ➔ Đặc điểm: Thường dùng class hoặc record.", CodeSnippet = "public record ProductDto(int Id, string Name, decimal SalePrice);" },
                    new() { StepNumber = 3, StepTitle = "Nhóm 3: ViewModel (Dữ liệu màn hình MVC)", StepDetail = "Đầu vào: Cần hiển thị trang Razor View ➔ Vai trò: Gom danh sách sản phẩm + phân trang + bộ lọc cho 1 màn hình.", CodeSnippet = "public class CatalogViewModel\n{\n    public List<ProductDto> Products { get; set; } = new();\n    public int CurrentPage { get; set; }\n}" },
                    new() { StepNumber = 4, StepTitle = "Nhóm 4: Controller & Service (Class xử lý)", StepDetail = "Đầu vào: Nhận DTO ➔ Vai trò: Chứa các hàm xử lý luồng và nghiệp vụ ➔ Đẩy ra: Trả về DTO hoặc ViewModel.", CodeSnippet = "public class ProductService : IProductService\n{\n    public async Task<List<ProductDto>> GetCatalogAsync() { ... }\n}" }
                },
                "Khi đọc source code dự án, bạn thấy một class chỉ chứa các thuộc tính `{ get; set; }` và có tên kết thúc bằng chữ `...Request` hoặc `...Dto`. Class đó thuộc nhóm vai trò nào?",
                new List<string>
                {
                    "A. Nhóm DTO (Thùng hàng vận chuyển dữ liệu JSON giữa Client và API)",
                    "B. Nhóm kết nối ổ cứng",
                    "C. Nhóm cấu hình Middleware",
                    "D. Nhóm giao diện CSS"
                },
                "A. Nhóm DTO (Thùng hàng vận chuyển dữ liệu JSON giữa Client và API)",
                "Chính xác! Các class có hậu tố `Dto`, `Request`, `Response` đều là thùng hàng vận chuyển dữ liệu tại tầng giao tiếp API."
            ),

            BuildSubLesson(202, topicId, "2.2",
                "Đọc luồng Model Binding: Cách dữ liệu JSON từ Postman tự động đổ vào các { get; set; } của C#",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/model-binding",
                """
                Nhiều bạn sinh viên thắc mắc: *"Tại sao bên Postman gửi chuỗi văn bản JSON mà vào hàm Controller lại có sẵn đối tượng `dto.Name`, `dto.Price`?"*
                Đó là cơ chế **Model Binding** tự động của ASP.NET Core:
                • **`[FromBody]`:** Đọc chuỗi JSON trong phần Body của Request và gọi bộ `JsonSerializer` gán vào các thuộc tính `public ... { get; set; }` có tên trùng khớp.
                • **`[FromRoute]`:** Lấy giá trị nằm ngay trên đường dẫn URL (ví dụ `/api/products/42` ➔ `id = 42`).
                • **`[FromQuery]`:** Lấy giá trị sau dấu hỏi chấm trên URL (ví dụ `/api/products?keyword=laptop&page=1`).
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Request gửi dữ liệu qua 3 đường", StepDetail = "Đầu vào: URL `/api/orders/15?notify=true` kèm JSON Body ➔ Xử lý: Gửi lên Kestrel Server ➔ Đẩy ra: HttpContext.", CodeSnippet = "PUT /api/orders/15?notify=true\nContent-Type: application/json\n\n{ \"shippingAddress\": \"Quận 1, TP.HCM\" }" },
                    new() { StepNumber = 2, StepTitle = "Bắt tham số trên đường dẫn [FromRoute]", StepDetail = "Đầu vào: Con số 15 trên URL ➔ Xử lý: Khớp với mẫu [HttpPut(\"{id}\")] ➔ Đẩy ra: Biến int id = 15.", CodeSnippet = "[HttpPut(\"{id:int}\")]\npublic async Task<IActionResult> Update([FromRoute] int id, ...)" },
                    new() { StepNumber = 3, StepTitle = "Bắt tham số lọc [FromQuery]", StepDetail = "Đầu vào: ?notify=true ➔ Xử lý: Đọc QueryString ➔ Đẩy ra: Biến bool notify = true.", CodeSnippet = "public async Task<IActionResult> Update([FromRoute] int id, [FromQuery] bool notify, ...)" },
                    new() { StepNumber = 4, StepTitle = "Đổ JSON vào thuộc tính { get; set; } [FromBody]", StepDetail = "Đầu vào: JSON Body ➔ Xử lý: Gọi hàm setter { set; } của thuộc tính ShippingAddress ➔ Đẩy ra: Đối tượng dto hoàn chỉnh.", CodeSnippet = "public async Task<IActionResult> Update([FromRoute] int id, [FromQuery] bool notify, [FromBody] UpdateOrderDto dto)" }
                },
                "Nếu một thuộc tính trong class DTO chỉ viết `public string Name;` (là Field, thiếu mất `{ get; set; }`), điều gì thường xảy ra khi Model Binding đọc JSON mặc định?",
                new List<string>
                {
                    "A. Bộ tuần tự hóa JSON mặc định sẽ bỏ qua không đổ dữ liệu vào vì nó cần Property có `{ get; set; }`",
                    "B. Máy tính tự động thêm { get; set; } vào file",
                    "C. Database bị xóa",
                    "D. Trình duyệt tự đóng"
                },
                "A. Bộ tuần tự hóa JSON mặc định sẽ bỏ qua không đổ dữ liệu vào vì nó cần Property có `{ get; set; }`",
                "Chuẩn xác! Trong C#, các thuộc tính trong DTO phải khai báo là Property (`{ get; set; }` hoặc `{ get; init; }`) để bộ Model Binder tự động gán dữ liệu JSON vào."
            ),

            BuildSubLesson(203, topicId, "2.3",
                "Đọc hiểu Constructor Injection & Quan hệ Has-A: Tại sao Controller luôn khai báo private readonly IService",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/dependency-injection",
                """
                Mở bất kỳ `Controller` hay `Service` nào trong dự án thực tế, bạn đều thấy một khuôn mẫu lặp đi lặp lại ở ngay đầu class:
                1. Khai báo biến `private readonly IOrderService _orderService;` (Quan hệ **Has-A**: Controller *có một* bộ xử lý đơn hàng).
                2. Hàm khởi tạo (Constructor) nhận tham số `public OrdersController(IOrderService orderService)` và gán `_orderService = orderService;`.
                Tại sao không viết `_orderService = new OrderService();`?
                • Vì nếu dùng từ khóa `new` cứng, `Controller` sẽ bị trói chặt vào class đó, không thể thay thế hay viết Unit Test. Nhờ **Constructor Injection**, ASP.NET Core tự động khởi tạo và tiêm (inject) đối tượng vào cho bạn!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Khai báo biến private readonly ở đầu Class", StepDetail = "Đầu vào: Nhu cầu gọi tầng dưới ➔ Xử lý: Khai báo biến kiểu Interface có khóa readonly (chỉ gán 1 lần trong Constructor).", CodeSnippet = "public class OrdersController : ControllerBase\n{\n    private readonly IOrderService _orderService;" },
                    new() { StepNumber = 2, StepTitle = "Yêu cầu cấp phát qua Hàm khởi tạo (Constructor)", StepDetail = "Đầu vào: Tham số IOrderService trong Constructor ➔ Xử lý: DI Container của .NET tự tìm class OrderService để truyền vào.", CodeSnippet = "    public OrdersController(IOrderService orderService)\n    {\n        _orderService = orderService;\n    }" },
                    new() { StepNumber = 3, StepTitle = "Sử dụng biến _orderService trong các hàm Action", StepDetail = "Đầu vào: HTTP Request gọi vào hàm ➔ Xử lý: Dùng _orderService chấm gọi các phương thức nghiệp vụ.", CodeSnippet = "    [HttpGet(\"{id}\")]\n    public async Task<IActionResult> Get(int id)\n    {\n        return Ok(await _orderService.GetByIdAsync(id));\n    }\n}" },
                    new() { StepNumber = 4, StepTitle = "Tự động dọn dẹp khi kết thúc Request", StepDetail = "Đầu vào: Đã trả Response xong ➔ Xử lý: DI Container tự giải phóng bộ nhớ của các dịch vụ Scoped.", CodeSnippet = "// Bạn không cần lo giải phóng bằng tay, DI Container lo trọn gói!" }
                },
                "Trong khuôn mẫu `private readonly IProductService _productService;`, từ khóa `readonly` mang ý nghĩa bảo vệ gì cho luồng chạy?",
                new List<string>
                {
                    "A. Đảm bảo biến `_productService` chỉ được gán một lần duy nhất trong Constructor, không bị hàm nào khác vô tình gán thành null giữa chừng",
                    "B. Chỉ cho phép gọi API vào buổi sáng",
                    "C. Biến class thành bảng SQL",
                    "D. Không cho phép truyền tham số"
                },
                "A. Đảm bảo biến `_productService` chỉ được gán một lần duy nhất trong Constructor, không bị hàm nào khác vô tình gán thành null giữa chừng",
                "Chính xác! `private readonly` là chuẩn mực vàng khi viết Dependency Injection trong C# để bảo vệ tham chiếu dịch vụ suốt vòng đời đối tượng."
            ),

            BuildSubLesson(204, topicId, "2.4",
                "Đọc hiểu Kế thừa trong khung .NET: Ý nghĩa thực sự của : ControllerBase, : Controller, : DbContext",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/object-oriented/inheritance",
                """
                Khi đọc dòng đầu tiên của một class trong dự án, hãy nhìn sau dấu hai chấm `:`:
                • **`public class ProductsController : ControllerBase`** ➔ Kế thừa từ `ControllerBase` (dành cho **Web API thuần**). Nhờ kế thừa này, bạn mới có sẵn các hàm `Ok()`, `BadRequest()`, `NotFound()`, `CreatedAtAction()` và thuộc tính `User`!
                • **`public class HomeController : Controller`** ➔ Kế thừa từ `Controller` (dành cho **MVC có giao diện Razor View**). Nó có thêm hàm `View()`, `PartialView()`, `TempData`, `ViewBag`.
                • **`public class AppDbContext : DbContext`** ➔ Kế thừa từ `DbContext` của EF Core, giúp có sẵn `SaveChangesAsync()`, `Set<T>()`, `Database.MigrateAsync()`.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Nhận diện dấu : (Kế thừa Is-A)", StepDetail = "Đầu vào: Dòng khai báo class ➔ Xử lý: Đọc tên lớp cha phía sau dấu hai chấm `:` ➔ Biết ngay: Vai trò của file code đó.", CodeSnippet = "public class OrdersController : ControllerBase // Đây là Web API Controller" },
                    new() { StepNumber = 2, StepTitle = "Thừa hưởng kho hàm trả kết quả từ ControllerBase", StepDetail = "Đầu vào: Cần trả mã HTTP 200, 400, 404 ➔ Xử lý: Gọi thẳng Ok(), BadRequest() có sẵn từ lớp cha ControllerBase.", CodeSnippet = "if (order == null) return NotFound(); // Hàm NotFound() thừa hưởng từ ControllerBase\nreturn Ok(order);" },
                    new() { StepNumber = 3, StepTitle = "Thừa hưởng hàm View() từ lớp Controller (MVC)", StepDetail = "Đầu vào: Cần trả về trang giao diện .cshtml ➔ Xử lý: Kế thừa : Controller để gọi return View(model).", CodeSnippet = "public class HomeController : Controller\n{\n    public IActionResult Index() => View(); // Trả về Views/Home/Index.cshtml\n}" },
                    new() { StepNumber = 4, StepTitle = "Thừa hưởng sức mạnh ORM từ : DbContext", StepDetail = "Đầu vào: Cần giao tiếp SQL Server ➔ Xử lý: Kế thừa : DbContext và truyền options lên base(options).", CodeSnippet = "public class AppDbContext : DbContext\n{\n    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }\n}" }
                },
                "Nhờ kế thừa từ lớp cha `: ControllerBase`, class Web API Controller của bạn có sẵn các phương thức tiện ích nào để trả kết quả HTTP?",
                new List<string>
                {
                    "A. Các hàm `Ok()`, `BadRequest()`, `NotFound()`, `CreatedAtAction()`",
                    "B. Hàm `Console.ReadLine()`",
                    "C. Hàm vẽ biểu đồ Chart.js",
                    "D. Hàm tắt máy tính"
                },
                "A. Các hàm `Ok()`, `BadRequest()`, `NotFound()`, `CreatedAtAction()`",
                "Chính xác! Nếu không kế thừa `: ControllerBase`, bạn sẽ không thể gọi trực tiếp `return Ok(data);` hay `return BadRequest();` trong các hàm Action."
            ),

            BuildSubLesson(205, topicId, "2.5",
                "Đọc hiểu Ghi đè (override) trong dự án: Cách đọc hàm OnModelCreating trong EF Core hoặc Middleware",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/keywords/override",
                """
                Trong các thư viện của Microsoft (như EF Core), lớp cha `DbContext` đã định nghĩa sẵn khung sườn có từ khóa `virtual` để cho phép lập trình viên tùy biến lại theo ý muốn bằng từ khóa **`override` (Ghi đè)**:
                • Ví dụ điển hình nhất bạn sẽ gặp trong mọi dự án công ty là hàm:
                  `protected override void OnModelCreating(ModelBuilder modelBuilder)`
                • Khi EF Core khởi tạo cấu trúc bảng lần đầu tiên, nó sẽ tự động gọi vào hàm `OnModelCreating` mà bạn đã `override` để đọc các cấu hình khóa chính, khóa ngoại, độ dài cột và dữ liệu khởi tạo (`HasData`).
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Lớp cha DbContext mở sẵn điểm chờ virtual", StepDetail = "Đầu vào: Khung EF Core của Microsoft ➔ Xử lý: Định nghĩa hàm virtual OnModelCreating ➔ Đẩy ra: Cho phép lớp con ghi đè.", CodeSnippet = "// Bên trong thư viện Microsoft.EntityFrameworkCore:\nprotected internal virtual void OnModelCreating(ModelBuilder modelBuilder) { }" },
                    new() { StepNumber = 2, StepTitle = "Lớp con AppDbContext dùng từ khóa override", StepDetail = "Đầu vào: File AppDbContext.cs của dự án ➔ Xử lý: Viết protected override void OnModelCreating ➔ Đẩy ra: Can thiệp cấu hình bảng.", CodeSnippet = "protected override void OnModelCreating(ModelBuilder modelBuilder)\n{\n    base.OnModelCreating(modelBuilder);" },
                    new() { StepNumber = 3, StepTitle = "Cấu hình Fluent API cho các bảng dữ liệu", StepDetail = "Đầu vào: Đối tượng modelBuilder ➔ Xử lý: Thiết lập khóa ngoại, chỉ mục (Index), kiểu tiền tệ decimal(18,2).", CodeSnippet = "    modelBuilder.Entity<Order>()\n        .Property(o => o.TotalAmount)\n        .HasColumnType(\"decimal(18,2)\");" },
                    new() { StepNumber = 4, StepTitle = "EF Core tự động kích hoạt khi chạy ứng dụng", StepDetail = "Đầu vào: Lệnh truy vấn đầu tiên hoặc Migration ➔ Xử lý: EF Core gọi hàm override của bạn để dựng sơ đồ DB chuẩn xác.", CodeSnippet = "}" }
                },
                "Khi đọc file `AppDbContext.cs`, bạn thấy hàm `protected override void OnModelCreating(ModelBuilder modelBuilder)`. Từ khóa `override` cho bạn biết điều gì?",
                new List<string>
                {
                    "A. Hàm này đang ghi đè (tùy biến lại) phương thức của lớp cha `DbContext` để cấu hình chi tiết các bảng và mối quan hệ trong Database",
                    "B. Hàm này dùng để xóa toàn bộ Controller",
                    "C. Hàm này chỉ chạy trên trình duyệt Chrome",
                    "D. Hàm này không bao giờ được gọi"
                },
                "A. Hàm này đang ghi đè (tùy biến lại) phương thức của lớp cha `DbContext` để cấu hình chi tiết các bảng và mối quan hệ trong Database",
                "Chuẩn xác! Nhìn thấy `override OnModelCreating` là biết ngay nơi chứa toàn bộ cấu hình Fluent API (khóa chính, khóa ngoại, ràng buộc cột) của Database."
            ),

            BuildSubLesson(206, topicId, "2.6",
                "Kỹ năng lần dấu từ Interface sang Class thực thi bằng phím tắt (F12 và Ctrl + F12) khi đọc code DI",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/visualstudio/ide/go-to-and-peek-definition",
                """
                Tình huống kinh điển khiến 90% sinh viên năm 4 bối rối khi mới đọc code công ty:
                • Đang ở `OrdersController`, bạn thấy dòng gọi `_orderService.CreateAsync(dto);`.
                • Bạn bấm **`F12` (Go to Definition)** vào chữ `CreateAsync`, nhưng Visual Studio lại chỉ mở ra file **`IOrderService.cs`** có đúng 1 dòng khai báo trống trơn, không hề có code xử lý bên trong!
                • **Bí quyết của Senior:** Vì dự án dùng Dependency Injection qua Interface, hãy bấm **`Ctrl + F12` (Go to Implementation)** (hoặc chuột phải chọn *Go to Implementation*). Visual Studio sẽ lập tức nhảy thẳng tới file **`OrderService.cs`** chứa toàn bộ code xử lý thật sự!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Đứng tại dòng gọi hàm trong Controller", StepDetail = "Đầu vào: Mở file OrdersController.cs ➔ Xử lý: Đặt con trỏ chuột vào tên hàm CreateOrderAsync.", CodeSnippet = "var result = await _orderService.CreateOrderAsync(dto);\n// Đặt con trỏ chuột ngay tại chữ CreateOrderAsync" },
                    new() { StepNumber = 2, StepTitle = "Nếu bấm F12: Chỉ tới bản hợp đồng Interface", StepDetail = "Đầu vào: Bấm phím F12 ➔ Xử lý: Nhảy tới IOrderService.cs ➔ Chỉ thấy chữ ký hàm, chưa thấy logic.", CodeSnippet = "public interface IOrderService\n{\n    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto); // Chỉ có dòng tiêu đề!\n}" },
                    new() { StepNumber = 3, StepTitle = "Tuyệt chiêu bấm Ctrl + F12 (Go to Implementation)", StepDetail = "Đầu vào: Bấm tổ hợp phím Ctrl + F12 ➔ Xử lý: Visual Studio tìm class thực thi Interface đó ➔ Mở ngay file OrderService.cs!", CodeSnippet = "public class OrderService : IOrderService\n{\n    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)\n    {\n        // ĐÂY RỒI! Toàn bộ code xử lý nghiệp vụ nằm ở đây!" },
                    new() { StepNumber = 4, StepTitle = "Tiếp tục bấm Ctrl + F12 để xuống Repository", StepDetail = "Đầu vào: Thấy dòng _orderRepo.AddAsync(order) trong Service ➔ Xử lý: Bấm tiếp Ctrl + F12 ➔ Nhảy thẳng xuống OrderRepository.cs!", CodeSnippet = "        await _orderRepo.AddAsync(order); // Bấm tiếp Ctrl + F12 tại AddAsync!\n    }\n}" }
                },
                "Khi đang đọc code trong Controller có sử dụng Dependency Injection (`_orderService.CreateAsync()`), phím tắt nào trên Visual Studio giúp bạn nhảy thẳng vào **Class thực thi thật sự (`OrderService.cs`)** thay vì dừng ở file Interface (`IOrderService.cs`)?",
                new List<string>
                {
                    "A. Tổ hợp phím `Ctrl + F12` (Go to Implementation)",
                    "B. Phím `Caps Lock`",
                    "C. Tổ hợp phím `Ctrl + P` (In tài liệu)",
                    "D. Phím `F1` (Mở trang trợ giúp)"
                },
                "A. Tổ hợp phím `Ctrl + F12` (Go to Implementation)",
                "Xuất sắc! Nhớ kỹ tổ hợp phím vàng `Ctrl + F12` (Go to Implementation) — nó giúp bạn xuyên qua mọi lớp Interface để lần theo luồng code từ Controller -> Service -> Repository chỉ trong 2 giây!"
            ),

            BuildSubLesson(207, topicId, "2.7",
                "Đọc hiểu các hàm tiện ích viết bằng static class và Extension Methods (Hàm mở rộng có từ khóa this)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/concepts/linq/extension-methods",
                """
                Khi đọc file `Program.cs` hoặc tầng `Service`, bạn thường thấy các hàm nhìn như có sẵn trong C#, ví dụ:
                `builder.Services.AddApplicationServices();` hoặc `entity.ToProductDto();`
                Thực chất, đó là **Extension Methods (Phương thức mở rộng)** do các anh trong công ty tự viết thêm:
                • Đặt trong một **`public static class ...Extensions`**.
                • Hàm cũng là **`public static`**, và tham số đầu tiên có từ khóa **`this`** đặt phía trước (ví dụ `public static ProductDto ToProductDto(this Product entity)`).
                • Nhờ từ khóa `this` đó, bạn có thể lấy biến `entity` chấm gọi `.ToProductDto()` cực kỳ gọn và đẹp!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Khai báo static class chứa hàm mở rộng", StepDetail = "Đầu vào: Muốn gom code chuyển đổi Entity -> DTO cho gọn ➔ Xử lý: Tạo static class ProductMappingExtensions.", CodeSnippet = "public static class ProductMappingExtensions\n{" },
                    new() { StepNumber = 2, StepTitle = "Đặt từ khóa 'this' trước tham số đầu tiên", StepDetail = "Đầu vào: Kiểu dữ liệu muốn gắn thêm hàm (Product) ➔ Xử lý: Viết (this Product entity) ➔ Đẩy ra: Hàm mở rộng ToDto().", CodeSnippet = "    public static ProductDto ToDto(this Product entity)\n    {\n        return new ProductDto(entity.Id, entity.Name, entity.SalePrice);\n    }\n}" },
                    new() { StepNumber = 3, StepTitle = "Gọi hàm bằng dấu chấm tự nhiên ở Service", StepDetail = "Đầu vào: Biến product lấy từ DB ➔ Xử lý: Gõ product.ToDto() ➔ Đẩy ra: Đối tượng ProductDto.", CodeSnippet = "var product = await _repo.GetByIdAsync(id);\nProductDto dto = product.ToDto(); // Nhìn cực gọn và sạch!" },
                    new() { StepNumber = 4, StepTitle = "Mở rộng cho cả IServiceCollection ở Program.cs", StepDetail = "Đầu vào: file Program.cs có quá nhiều dòng AddScoped ➔ Xử lý: Gom vào hàm mở rộng AddBusinessServices(this IServiceCollection services).", CodeSnippet = "// Trong Program.cs chỉ cần gọi 1 dòng:\nbuilder.Services.AddBusinessServices();" }
                },
                "Dấu hiệu nhận biết một hàm là **Extension Method (Phương thức mở rộng)** trong C# là gì?",
                new List<string>
                {
                    "A. Nằm trong `static class`, là hàm `static` và có từ khóa `this` đứng trước kiểu của tham số đầu tiên",
                    "B. Có từ khóa `virtual` ở cuối hàm",
                    "C. Phải đặt trong file HTML",
                    "D. Không có kiểu trả về"
                },
                "A. Nằm trong `static class`, là hàm `static` và có từ khóa `this` đứng trước kiểu của tham số đầu tiên",
                "Chính xác! Từ khóa `this` trước tham số đầu tiên (ví dụ `this Product entity` hoặc `this IServiceCollection services`) chính là dấu hiệu đặc trưng của Extension Method."
            ),

            BuildSubLesson(208, topicId, "2.8",
                "Đọc hiểu partial class khi làm việc với Entity Framework hoặc công cụ sinh code tự động",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods",
                """
                Đôi khi trong dự án công ty, bạn thấy một class có từ khóa **`partial`**:
                `public partial class AppDbContext : DbContext` hoặc `public partial class Order`
                • **`partial class` (Lớp từng phần)** cho phép chia nhỏ nội dung của **cùng 1 class** ra thành 2 hoặc nhiều file `.cs` khác nhau. Khi biên dịch (`dotnet build`), trình biên dịch C# sẽ tự động ghép tất cả các file đó lại thành 1 class hoàn chỉnh!
                • **Ứng dụng thực tế:** Khi dùng công cụ Database-First (Scaffold từ SQL ra C#), công cụ sẽ sinh ra file `Order.Generated.cs`. Nếu bạn muốn viết thêm hàm tính toán cho `Order` mà không sợ bị mất code khi chạy lại Scaffold, bạn tạo thêm file `Order.Custom.cs` cùng khai báo `public partial class Order`!
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "File 1: Code do EF Core tự động sinh ra", StepDetail = "Đầu vào: Chạy lệnh Scaffold-DbContext ➔ Xử lý: Sinh ra file Order.cs có từ khóa partial chứa các cột DB.", CodeSnippet = "// File 1: Entities/Order.cs (Do máy sinh tự động)\npublic partial class Order\n{\n    public int Id { get; set; }\n    public decimal SubTotal { get; set; }\n    public decimal Tax { get; set; }\n}" },
                    new() { StepNumber = 2, StepTitle = "File 2: Code do lập trình viên viết thêm", StepDetail = "Đầu vào: Muốn thêm thuộc tính tính tổng tiền ➔ Xử lý: Tạo file Order.Logic.cs cùng tên partial class Order.", CodeSnippet = "// File 2: Entities/Order.Logic.cs (Lập trình viên tự viết)\npublic partial class Order\n{\n    public decimal GrandTotal => SubTotal + Tax;\n}" },
                    new() { StepNumber = 3, StepTitle = "Trình biên dịch C# tự động hợp nhất", StepDetail = "Đầu vào: Hai file partial class Order ➔ Xử lý: Khi build, C# gộp cả hai thành một class Order duy nhất.", CodeSnippet = "// Khi sử dụng ở tầng Service, đối tượng order có đầy đủ cả Id, SubTotal, Tax lẫn GrandTotal!" },
                    new() { StepNumber = 4, StepTitle = "An toàn tuyệt đối khi cập nhật lại Database", StepDetail = "Đầu vào: Chạy lại công cụ sinh code ở File 1 ➔ Đẩy ra: Code nghiệp vụ ở File 2 vẫn nguyên vẹn 100%!", CodeSnippet = "var total = order.GrandTotal; // Dùng mượt mà!" }
                },
                "Từ khóa `partial` trong khai báo `public partial class Order` có tác dụng gì trong dự án thực tế?",
                new List<string>
                {
                    "A. Cho phép chia một class thành nhiều file `.cs` khác nhau (rất hữu ích khi tách phần code tự sinh của EF Core và phần code viết tay)",
                    "B. Xóa một nửa dữ liệu trong bảng Order",
                    "C. Biến class thành biến toàn cục",
                    "D. Chỉ cho phép chạy 50% số dòng lệnh"
                },
                "A. Cho phép chia một class thành nhiều file `.cs` khác nhau (rất hữu ích khi tách phần code tự sinh của EF Core và phần code viết tay)",
                "Chính xác! `partial class` giúp tách bạch file do công cụ tự động sinh ra và file do lập trình viên viết thêm."
            ),

            BuildSubLesson(209, topicId, "2.9",
                "Thực hành đọc trọn vẹn luồng OOP trong Web API: Từ Controller ➔ gọi IService ➔ chạy Service ➔ gọi IRepository ➔ trả về DTO/Record",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures",
                """
                Đây là bài tổng kết ghép nối toàn bộ kiến thức OOP & Dependency Injection của Chủ đề 2 vào một luồng thực tế hoàn chỉnh:
                1. **`ProductsController` (Kế thừa `: ControllerBase`):** Nhận `CreateProductDto` qua Model Binding, gọi `_productService.CreateAsync(dto)`.
                2. **`ProductService` (Thực thi `: IProductService`):** Kiểm tra quy tắc nghiệp vụ, khởi tạo `new Product` (Entity) và gọi `_productRepo.AddAsync(entity)`.
                3. **`ProductRepository` (Thực thi `: IProductRepository`):** Dùng `_context.Products.AddAsync(entity)` và `_context.SaveChangesAsync()` lưu vào SQL Server.
                4. **Đóng gói trả về:** Dùng Extension Method `entity.ToDto()` trả về `ProductDto` (Record) kèm mã `201 Created`.
                """,
                new List<InteractiveFlowStep>
                {
                    new() { StepNumber = 1, StepTitle = "Controller đón DTO & Gọi IProductService", StepDetail = "Đầu vào: POST /api/products kèm CreateProductDto ➔ Xử lý: Kiểm tra hợp lệ rồi gọi qua Interface _productService.", CodeSnippet = "[HttpPost]\npublic async Task<IActionResult> Create([FromBody] CreateProductDto dto)\n{\n    var created = await _productService.CreateAsync(dto);\n    return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);\n}" },
                    new() { StepNumber = 2, StepTitle = "ProductService xử lý nghiệp vụ & Tạo Entity", StepDetail = "Đầu vào: CreateProductDto ➔ Xử lý: Kiểm tra giá hợp lệ, tạo đối tượng Entity Product và gọi _productRepo.", CodeSnippet = "public async Task<ProductDto> CreateAsync(CreateProductDto dto)\n{\n    var entity = new Product { Name = dto.Name.Trim(), SalePrice = dto.Price };\n    await _productRepo.AddAsync(entity);\n    return entity.ToDto();\n}" },
                    new() { StepNumber = 3, StepTitle = "ProductRepository lưu xuống AppDbContext", StepDetail = "Đầu vào: Đối tượng Entity Product ➔ Xử lý: Thêm vào DbSet<Product> và gọi SaveChangesAsync() ghi xuống SQL.", CodeSnippet = "public async Task AddAsync(Product entity)\n{\n    await _context.Products.AddAsync(entity);\n    await _context.SaveChangesAsync();\n}" },
                    new() { StepNumber = 4, StepTitle = "Extension Method đóng gói Record DTO trả về", StepDetail = "Đầu vào: Entity đã có Id tự tăng từ SQL ➔ Xử lý: Chuyển sang Record ProductDto trả ngược lên Controller ➔ 201 Created.", CodeSnippet = "public record ProductDto(int Id, string Name, decimal SalePrice);" }
                },
                "Trong luồng OOP chuẩn của dự án ASP.NET Core Web API, thứ tự các lớp/giao diện tham gia xử lý một yêu cầu từ ngoài vào trong là gì?",
                new List<string>
                {
                    "A. Controller ➔ IService (Service) ➔ IRepository (Repository) ➔ DbContext (SQL Database)",
                    "B. DbContext ➔ Postman ➔ Controller ➔ HTML",
                    "C. Repository ➔ Controller ➔ Client",
                    "D. DTO ➔ SQL Server ➔ Middleware"
                },
                "A. Controller ➔ IService (Service) ➔ IRepository (Repository) ➔ DbContext (SQL Database)",
                "Xuất sắc! Bạn đã hoàn thành trọn vẹn 9 bài học của Chủ đề 2 (Thực tập) và nắm vững cách đọc luồng OOP & DI trong dự án thực tế!"
            )
        };
    }
}
