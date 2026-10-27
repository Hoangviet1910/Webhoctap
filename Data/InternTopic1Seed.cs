using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class InternTopic1Seed
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

    /// <summary>
    /// Danh sách đầy đủ 10 bài học con (1.1 -> 1.10) của Chủ đề 1 (Menu 1: Luồng Đi Thực Tập - Đọc hiểu code C# & Cấu trúc dự án thực tế)
    /// </summary>
    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 1.1
            // =====================================================================
            Build(
                topicId,
                "1.1",
                "Cách đọc cấu trúc Solution dự án công ty & Luồng chạy của file Program.cs",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/",
                """
                • Bản chất: Khi mở một dự án C# (.sln) ở công ty, bạn sẽ thấy nhiều Project con bên trong (ví dụ: .API, .Core/Domain, .Infrastructure/Data). File Program.cs trong project .API luôn là cửa ngõ đầu tiên chạy lên khi khởi động web.
                • Cách đọc file Program.cs (Chia làm 2 nửa rõ rệt):
                1. Nửa trên (var builder = WebApplication.CreateBuilder(args); đến trước builder.Build()): Nơi đăng ký các dịch vụ vào hệ thống (Dependency Injection Container) như kết nối Database (AddDbContext), đăng ký Controller (AddControllers), đăng ký Swagger và các cặp Interface - Service (AddScoped).
                2. Nửa dưới (Sau var app = builder.Build(); đến app.Run();): Nơi cấu hình Middleware Pipeline (luồng ống dẫn xử lý Request) như UseHttpsRedirection, UseAuthentication (xác thực), UseAuthorization (phân quyền) và MapControllers (nối URL vào Controller).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khởi tạo Builder",
                        "Đọc file cấu hình appsettings.json (chuỗi kết nối DB, API key) và chuẩn bị môi trường.",
                        "var builder = WebApplication.CreateBuilder(args);"),
                    Step(2,
                        "Đăng ký Services (DI)",
                        "Khai báo cho .NET biết project dùng DbContext nào, Interface nào đi với Class thực thi nào.",
                        "builder.Services.AddDbContext<AppDbContext>(...);\nbuilder.Services.AddScoped<IProductService, ProductService>();"),
                    Step(3,
                        "Thiết lập Middleware",
                        "Quy định thứ tự các trạm kiểm soát mà mọi HTTP Request phải đi qua (Thứ tự cực kỳ quan trọng: Xác thực trước -> Phân quyền sau).",
                        "var app = builder.Build();\napp.UseAuthentication();\napp.UseAuthorization();"),
                    Step(4,
                        "Định tuyến & Chạy Server",
                        "Ánh xạ Request vào đúng Controller và mở cổng lắng nghe kết nối.",
                        "app.MapControllers();\napp.Run();")
                },
                "Khi đi thực tập, sếp giao bạn tạo thêm OrderService và IOrderService, nhưng khi chạy API lại báo lỗi: \"Unable to resolve service for type IOrderService\". Bạn phải mở file nào và thêm dòng gì để sửa?",
                new List<string>
                {
                    "A. Mở OrderController.cs và thêm từ khóa new OrderService()",
                    "B. Mở Program.cs (phần builder.Services) và thêm builder.Services.AddScoped<IOrderService, OrderService>();",
                    "C. Mở appsettings.json và khai báo tên OrderService",
                    "D. Mở DbContext và thêm DbSet<OrderService>"
                },
                "B",
                "Trong ASP.NET Core, mọi Service dùng qua Dependency Injection đều bắt buộc phải được đăng ký vào builder.Services bên trong file Program.cs trước khi builder.Build() được gọi."
            ),

            // =====================================================================
            // BÀI 1.2
            // =====================================================================
            Build(
                topicId,
                "1.2",
                "Đọc hiểu dữ liệu JSON từ Client ánh xạ vào các kiểu C# (int, string, decimal, DateTime, Guid)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/built-in-types",
                """
                • Bản chất: Client (Web/Mobile/Postman) gửi dữ liệu lên Backend dưới dạng văn bản JSON. ASP.NET Core tự động dịch (Deserialize) JSON đó thành các kiểu dữ liệu C# tương ứng trong class DTO.
                • Quy tắc chọn kiểu dữ liệu thực tế ở công ty:
                - int / long: Dùng cho số lượng, thứ tự, hoặc ID tự tăng (1, 2, 3).
                - Guid: Chuỗi định danh duy nhất toàn cầu (VD: "3fa85f64-5717-4562-b3fc-2c963f66afa6"), cực kỳ phổ biến làm khóa chính Id trong dự án doanh nghiệp để tránh bị đoán ID.
                - decimal: Bắt buộc dùng cho Tiền tệ / Giá tiền (VD: Price, TotalAmount). Tuyệt đối không dùng float/double cho tiền vì bị sai số thập phân.
                - DateTime: Ngày giờ chuẩn ISO 8601 (VD: "2026-11-25T14:30:00Z").
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Client gửi JSON",
                        "Postman gửi gói tin JSON chứa thông tin tạo đơn hàng.",
                        "{\n  \"productId\": \"d290f1ee-6c54-4b01-90e6-d701748f0851\",\n  \"price\": 150000.50,\n  \"orderDate\": \"2026-10-20T08:00:00\"\n}"),
                    Step(2,
                        "JSON Deserializer",
                        ".NET tự động khớp tên trường JSON (không phân biệt hoa thường) với tên Property trong C#.",
                        "// Hệ thống tự động parse chuỗi UUID sang kiểu Guid, số sang decimal"),
                    Step(3,
                        "Ánh xạ vào C# DTO",
                        "Dữ liệu nằm gọn trong đối tượng C# để bạn chấm (.) lấy ra dùng.",
                        "public class CreateOrderDto {\n    public Guid ProductId { get; set; }\n    public decimal Price { get; set; }\n    public DateTime OrderDate { get; set; }\n}")
                },
                "Khi đọc class ProductDto hoặc Order trong dự án thương mại điện tử của công ty, vì sao cột giá tiền (Price) các anh Senior luôn khai báo là decimal chứ không bao giờ dùng double?",
                new List<string>
                {
                    "A. Vì decimal tốn ít bộ nhớ RAM hơn double",
                    "B. Vì decimal tính toán hệ cơ số 10 chính xác tuyệt đối phần thập phân, không bị lỗi làm tròn lẻ tiền như double",
                    "C. Vì SQL Server không lưu được kiểu double",
                    "D. Vì JSON chỉ hỗ trợ kiểu decimal"
                },
                "B",
                "double biểu diễn số theo hệ nhị phân nên phép tính 0.1 + 0.2 có thể ra 0.30000000000000004, gây lệch sổ sách kế toán. Do đó tài chính luôn dùng decimal."
            ),

            // =====================================================================
            // BÀI 1.3
            // =====================================================================
            Build(
                topicId,
                "1.3",
                "Cách đọc và xử lý dấu hỏi chấm (?, ?., ??) trong code dự án để tránh lỗi Null sập API",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/member-access-operators#null-conditional-operators--and-",
                """
                • Lỗi kinh điển nhất khi đi thực tập: System.NullReferenceException: Object reference not set to an instance of an object (Cố tình chấm .Name từ một biến đang bị null).
                • Cách đọc 3 loại dấu hỏi chấm trong code C#:
                1. int?, DateTime?, string? (Nullable Type): Cho biết biến hoặc cột Database này được phép bỏ trống (nhận giá trị null). Ví dụ: UpdatedAt (DateTime?) vì lúc mới tạo bản ghi thì chưa có ngày cập nhật.
                2. user?.Profile?.AvatarUrl (Null-conditional operator ?.): Nếu user hoặc Profile bị null, nó dừng lại và trả về null nhẹ nhàng chứ không làm sập chương trình.
                3. string name = inputName ?? "Khách vãng lai"; (Null-coalescing operator ??): Nếu vế trái khác null thì lấy vế trái, nếu vế trái bị null thì lấy giá trị mặc định ở vế phải.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Truy vấn DB có thể Null",
                        "Tìm kiếm khách hàng theo ID, nếu ID không tồn tại trong bảng thì biến customer sẽ mang giá trị null.",
                        "var customer = await _db.Customers.FindAsync(id);"),
                    Step(2,
                        "Truy cập an toàn với ?.",
                        "Dùng dấu ?. để lấy tên thành phố mà không sợ văng lỗi 500 nếu khách hàng chưa nhập địa chỉ.",
                        "string? city = customer?.Address?.CityName;"),
                    Step(3,
                        "Gán mặc định với ??",
                        "Nếu city bị null thì trả về chữ 'Chưa cập nhật' cho giao diện hiển thị đẹp mắt.",
                        "string displayCity = city ?? \"Chưa cập nhật\";")
                },
                "Đọc đoạn code sau trong dự án: int pageSize = request.PageSize ?? 10;. Đoạn code này hoạt động như thế nào?",
                new List<string>
                {
                    "A. Nếu request.PageSize có gửi số lên thì lấy số đó, nếu bị null (không gửi) thì mặc định lấy 10",
                    "B. Luôn luôn cộng thêm 10 vào request.PageSize",
                    "C. Báo lỗi nếu request.PageSize khác 10",
                    "D. Chia request.PageSize cho 10"
                },
                "A",
                "Toán tử ?? kiểm tra nếu phần tử bên trái khác null sẽ lấy giá trị của nó, ngược lại nếu là null sẽ trả về giá trị dự phòng bên phải (ở đây là 10)."
            ),

            // =====================================================================
            // BÀI 1.4
            // =====================================================================
            Build(
                topicId,
                "1.4",
                "Đọc luồng xử lý Chuỗi (string.IsNullOrWhiteSpace, chuẩn hóa dữ liệu request)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/strings/",
                """
                • Bản chất: Người dùng cuối rất hay nhập thừa dấu cách ở đầu/cuối (VD: "  admin@gmail.com  "), gõ chữ hoa/thường lộn xộn hoặc gửi toàn dấu cách ("   ").
                • Các hàm xử lý chuỗi luôn đi theo cụm ở đầu hàm Service/Controller:
                - string.IsNullOrWhiteSpace(str): Kiểm tra cùng lúc 3 trường hợp: null, chuỗi rỗng "", hoặc chuỗi chỉ toàn dấu cách "   ". Luôn dùng hàm này thay vì chỉ kiểm tra str == "".
                - .Trim(): Cắt bỏ khoảng trắng thừa ở 2 đầu.
                - .ToLower() / .ToUpper(): Đưa về chữ thường/hoa để so sánh hoặc lưu mã Code, Email chuẩn.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Kiểm tra chuỗi rỗng/khoảng trắng",
                        "Chặn ngay nếu người dùng gửi từ khóa tìm kiếm rỗng hoặc toàn dấu cách.",
                        "if (string.IsNullOrWhiteSpace(keyword)) return BadRequest(\"Vui lòng nhập từ khóa\");"),
                    Step(2,
                        "Làm sạch chuỗi (Normalize)",
                        "Cắt khoảng trắng thừa 2 đầu và chuyển về chữ thường trước khi đem đi tìm trong Database.",
                        "var cleanKey = keyword.Trim().ToLower();"),
                    Step(3,
                        "Tạo thông báo với String Interpolation",
                        "Ghép biến trực tiếp vào chuỗi bằng ký hiệu $\"...\" rất dễ đọc.",
                        "string logMsg = $\"User tìm kiếm từ khóa: {cleanKey} lúc {DateTime.Now}\";")
                },
                "Tại sao trong code dự án thực tế, để kiểm tra người dùng có nhập Email hay không, lập trình viên luôn dùng string.IsNullOrWhiteSpace(dto.Email) thay vì string.IsNullOrEmpty(dto.Email)?",
                new List<string>
                {
                    "A. Vì IsNullOrWhiteSpace bắt được cả trường hợp người dùng cố tình bấm phím Space (dấu cách) nhiều lần mà không nhập chữ",
                    "B. Vì IsNullOrEmpty đã bị Microsoft xóa bỏ trong .NET 10",
                    "C. Vì IsNullOrWhiteSpace tự động gửi email xác nhận",
                    "D. Hai hàm hoàn toàn giống hệt nhau"
                },
                "A",
                "Nếu người dùng nhập \"   \", hàm IsNullOrEmpty sẽ cho qua (vì độ dài chuỗi bằng 3), còn IsNullOrWhiteSpace phát hiện đây chỉ là khoảng trắng vô nghĩa và chặn lại."
            ),

            // =====================================================================
            // BÀI 1.5
            // =====================================================================
            Build(
                topicId,
                "1.5",
                "Đọc luồng kiểm tra dữ liệu đầu vào (Data Annotations, Validation & TryParse) tại cửa ngõ API",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/validation",
                """
                • Bản chất: Không bao giờ được tin tưởng dữ liệu từ Client gửi lên. Trước khi cho phép dữ liệu chạy vào tầng Service và lưu xuống Database, nó phải qua bước Validation (Kiểm tra hợp lệ).
                • 2 cách kiểm tra bạn sẽ gặp khi đọc code công ty:
                1. Khai báo thuộc tính (Attributes) ngay trên DTO: Dùng [Required], [MaxLength(100)], [Range(1, 1000)], [EmailAddress]. Với [ApiController], nếu vi phạm các nhãn này, .NET tự động trả về lỗi 400 Bad Request ngay lập tức.
                2. Ép kiểu an toàn với TryParse: Khi nhận chuỗi từ URL/Query (VD: "123a") cần đổi sang số hoặc ngày tháng, dùng int.TryParse(str, out int number) để kiểm tra; nếu đổi thành công trả về true, thất bại trả về false chứ không văng Exception.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đặt rào chắn tại DTO",
                        "Gắn các thẻ Data Annotations lên các trường dữ liệu bắt buộc trong class DTO.",
                        "public class CreateUserDto {\n    [Required(ErrorMessage = \"Tên không được trống\")]\n    [MaxLength(50)]\n    public string FullName { get; set; }\n}"),
                    Step(2,
                        "ApiController tự động kiểm tra",
                        "Trước khi vào hàm Controller, ASP.NET kiểm tra ModelState.IsValid. Nếu sai, trả thẳng mã 400 về Client.",
                        "// Không cần viết code thủ công nếu class có gắn [ApiController]"),
                    Step(3,
                        "Kiểm tra định dạng đặc thù bằng TryParse",
                        "Kiểm tra chuỗi tham số có phải số nguyên hợp lệ không trước khi xử lý tiếp.",
                        "if (!int.TryParse(rawId, out int validId)) {\n    return BadRequest(\"ID phải là một con số!\");\n}")
                },
                "Khi test API tạo sản phẩm bằng Postman, bạn thấy code trong hàm CreateProduct của Controller chưa hề chạy dòng nào (đặt Breakpoint không dừng) nhưng Postman đã nhận về lỗi 400 Bad Request. Nguyên nhân là gì?",
                new List<string>
                {
                    "A. Server bị mất kết nối SQL Server",
                    "B. Do dữ liệu JSON gửi lên vi phạm các thẻ [Required], [Range] trong DTO và bị thuộc tính [ApiController] tự động chặn lại trước khi vào hàm",
                    "C. Do viết sai tên hàm trong Service",
                    "D. Do Postman bị hỏng"
                },
                "B",
                "Thuộc tính [ApiController] tự động kích hoạt bộ lọc kiểm tra ModelState. Nếu DTO không hợp lệ, nó lập tức trả về HTTP 400 mà không cần chạy vào thân hàm Controller."
            ),

            // =====================================================================
            // BÀI 1.6
            // =====================================================================
            Build(
                topicId,
                "1.6",
                "Đọc luồng if-else theo kỹ thuật \"Guard Clauses\" (Chặn lỗi sớm ngay đầu hàm)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/selection-statements",
                """
                • Sự khác biệt giữa code sinh viên và code ở công ty:
                - Code sinh viên: Hay viết if (...) { if (...) { if (...) { ... } } else { ... } } lồng nhau 4-5 tầng (Arrow Anti-pattern), rất khó đọc.
                - Code công ty (Guard Clauses - Câu lệnh bảo vệ): Kiểm tra các điều kiện lỗi/không hợp lệ ở ngay những dòng đầu tiên của hàm và lập tức return (hoặc throw) thoát ra luôn. Phần code chạy thành công chính thức sẽ nằm thẳng hàng ở cuối hàm mà không cần bọc trong else.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Trạm gác 1: Kiểm tra tồn tại",
                        "Nếu không tìm thấy sản phẩm trong DB -> Thoát hàm ngay, trả về 404.",
                        "var product = await _repo.GetByIdAsync(id);\nif (product == null) return NotFound(\"Không tìm thấy SP\");"),
                    Step(2,
                        "Trạm gác 2: Kiểm tra nghiệp vụ",
                        "Nếu sản phẩm đã ngừng kinh doanh hoặc hết hàng -> Thoát hàm ngay, trả về 400.",
                        "if (product.Stock <= 0) return BadRequest(\"Sản phẩm đã hết hàng\");"),
                    Step(3,
                        "Luồng chính (Happy Path)",
                        "Khi đã vượt qua mọi trạm gác trên, yên tâm thực thi nghiệp vụ chính ở cuối hàm mà không cần từ khóa else.",
                        "await _orderService.CreateAsync(product);\nreturn Ok(\"Đặt hàng thành công!\");")
                },
                "Lợi ích lớn nhất của việc viết if (...) return ...; (Guard Clauses) ở đầu hàm thay vì dùng nhiều cặp if - else lồng nhau là gì?",
                new List<string>
                {
                    "A. Giúp người đọc code loại bỏ ngay các trường hợp lỗi ở đầu hàm, code phẳng không bị thụt lề sâu và cực kỳ dễ theo dõi luồng chính",
                    "B. Giúp cơ sở dữ liệu lưu nhanh hơn",
                    "C. Bắt buộc phải viết vậy thì C# mới biên dịch được",
                    "D. Giúp ẩn code không cho Frontend nhìn thấy"
                },
                "A",
                "Guard Clauses làm phẳng cấu trúc code, giúp lập trình viên nhìn lướt từ trên xuống dưới biết ngay hàm này chặn những điều kiện biên nào trước khi chạy logic chính."
            ),

            // =====================================================================
            // BÀI 1.7
            // =====================================================================
            Build(
                topicId,
                "1.7",
                "Đọc luồng Enum kết hợp switch trong quản lý trạng thái đơn hàng/nghiệp vụ",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/enum",
                """
                • Tại sao công ty không dùng số 1, 2, 3 trần trụi trong code? Nếu viết if (order.Status == 2), người đọc sau sẽ không thể biết số 2 nghĩa là "Đã thanh toán" hay "Đã hủy".
                • Giải pháp chuẩn: Tạo một enum OrderStatus { Pending = 0, Paid = 1, Shipped = 2, Cancelled = 3 }. Dưới Database vẫn lưu số nguyên 0, 1, 2, 3 cho nhẹ, nhưng trên code C# hiển thị chữ OrderStatus.Paid cực kỳ rõ nghĩa và kết hợp với switch để rẽ nhánh xử lý.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Định nghĩa Enum trạng thái",
                        "Khai báo danh sách các trạng thái cố định kèm giá trị số lưu trong DB.",
                        "public enum OrderStatus {\n    Pending = 1,\n    Confirmed = 2,\n    Cancelled = 3\n}"),
                    Step(2,
                        "Phân luồng bằng Switch Expression",
                        "Dựa vào trạng thái hiện tại để quyết định hành động hoặc thông báo tương ứng.",
                        "string message = order.Status switch {\n    OrderStatus.Pending => \"Đang chờ duyệt\",\n    OrderStatus.Confirmed => \"Đã xác nhận đơn\",\n    OrderStatus.Cancelled => \"Đơn đã bị hủy\",\n    _ => \"Không xác định\"\n};"),
                    Step(3,
                        "Cập nhật trạng thái mới",
                        "Gán trực tiếp qua tên Enum rõ ràng, tránh gán nhầm số.",
                        "order.Status = OrderStatus.Confirmed;\nawait _db.SaveChangesAsync();")
                },
                "Trong cú pháp switch expression mới của C# (dùng dấu =>), ký hiệu dấu gạch dưới _ => ... ở dòng cuối cùng đóng vai trò tương đương với từ khóa nào trong switch-case truyền thống?",
                new List<string>
                {
                    "A. Tương đương với break",
                    "B. Tương đương với default (xử lý tất cả các trường hợp còn lại chưa được liệt kê)",
                    "C. Tương đương với continue",
                    "D. Tương đương với return null"
                },
                "B",
                "Ký hiệu _ (Discard pattern) trong switch expression đại diện cho nhánh default, bắt mọi giá trị không khớp với các nhánh phía trên."
            ),

            // =====================================================================
            // BÀI 1.8
            // =====================================================================
            Build(
                topicId,
                "1.8",
                "Đọc luồng vòng lặp foreach khi biến đổi danh sách dữ liệu trước khi trả về Response",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/iteration-statements#the-foreach-statement",
                """
                • Bản chất: Trong Backend, 95% trường hợp duyệt danh sách dùng foreach (hoặc hàm .Select() của LINQ) chứ ít khi dùng for (int i = 0; ...) vì dữ liệu lấy từ DB là một tập hợp các đối tượng (List<Product>).
                • Quy tắc quan trọng khi đọc luồng foreach:
                - Dùng để tính tổng tiền giỏ hàng, kiểm tra tồn kho từng món, hoặc chuyển đổi danh sách List<ProductEntity> sang List<ProductDto>.
                - Cấm kỵ: Không được gọi lệnh xóa/thêm phần tử (list.Remove(...)) trực tiếp trên chính danh sách đang chạy foreach vì sẽ văng lỗi InvalidOperationException: Collection was modified.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Lấy danh sách gốc từ DB",
                        "Repository trả về danh sách các Entity từ bảng SQL.",
                        "List<Product> entities = await _repo.GetAllAsync();"),
                    Step(2,
                        "Duyệt qua từng phần tử bằng foreach",
                        "Khởi tạo danh sách DTO rỗng và duyệt từng món hàng để tính giá sau khuyến mãi.",
                        "var dtoList = new List<ProductDto>();\nforeach (var item in entities) {\n    dtoList.Add(new ProductDto {\n        Name = item.Name,\n        FinalPrice = item.Price * 0.9m\n    });\n}"),
                    Step(3,
                        "Trả danh sách kết quả",
                        "Gửi danh sách DTO đã chế biến xong về cho Controller.",
                        "return dtoList;")
                },
                "Khi đi thực tập, bạn thấy một hàm chạy vòng lặp foreach (var item in cartItems) và bên trong mỗi vòng lặp lại gọi await _db.Products.FindAsync(item.ProductId) (gọi Database liên tục trong vòng lặp). Vấn đề của đoạn code này là gì?",
                new List<string>
                {
                    "A. Gây ra lỗi hiệu năng N+1 Queries (nếu giỏ hàng có 50 món sẽ phải mở kết nối hỏi Database 50 lần liên tiếp làm chậm API)",
                    "B. C# không cho phép dùng await bên trong foreach",
                    "C. Làm mất dữ liệu trong SQL Server",
                    "D. Không có vấn đề gì, đây là cách viết tối ưu nhất"
                },
                "A",
                "Gọi truy vấn DB bên trong vòng lặp foreach là lỗi hiệu năng phổ biến. Cách chuẩn là lấy toàn bộ sản phẩm cần thiết trong 1 câu truy vấn (WHERE Id IN (...)) trước khi vào vòng lặp."
            ),

            // =====================================================================
            // BÀI 1.9
            // =====================================================================
            Build(
                topicId,
                "1.9",
                "Kỹ năng đọc \"Chữ ký hàm\" (Method Signature): Nhìn dòng khai báo biết ngay đầu vào, đầu ra của luồng",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/methods",
                """
                • Mẹo đọc code không cần xem ruột hàm: Khi đọc một file Service dài 500 dòng, bạn đừng đọc từng dòng code bên trong ngay. Hãy nhìn vào Chữ ký hàm (Method Signature) gồm 3 thành phần:
                1. Kiểu trả về (Return Type): Ví dụ Task<PagedResult<UserDto>> -> Hàm bất đồng bộ, trả về danh sách UserDto có phân trang. Nếu là Task<bool> -> Trả về đúng/sai. Nếu là Task (không có <>) -> Chỉ thực thi (tương đương void), không trả về dữ liệu.
                2. Tên hàm (Method Name): Cho biết hành động (VD: CreateOrderAsync, ValidateToken).
                3. Tham số đầu vào (Parameters): Dữ liệu hàm này đòi hỏi tầng trên phải truyền xuống (VD: (Guid userId, UpdateProfileDto request)).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Soi Tham số đầu vào (Bên phải tên hàm)",
                        "Biết ngay hàm này cần nhận những nguyên liệu gì từ Controller truyền xuống.",
                        "(int categoryId, string? searchKeyword)"),
                    Step(2,
                        "Soi Tên hàm (Ở giữa)",
                        "Nắm bắt mục đích nghiệp vụ chính mà không cần đọc code chi tiết bên trong.",
                        "GetProductsByCategoryAsync"),
                    Step(3,
                        "Soi Kiểu trả về (Bên trái tên hàm)",
                        "Biết chính xác sau khi chạy xong, hàm này nhả ra cục dữ liệu gì cho tầng gọi nó.",
                        "public async Task<IEnumerable<ProductDto>>")
                },
                "Nhìn vào chữ ký hàm sau trong IOrderService: Task<Guid?> CancelOrderAsync(int orderId, string reason);. Bạn rút ra được thông tin gì mà chưa cần mở code bên trong?",
                new List<string>
                {
                    "A. Hàm nhận vào mã đơn (int) và lý do hủy (string), xử lý bất đồng bộ và trả về một mã Guid (hoặc trả về null nếu không hủy được)",
                    "B. Hàm này không trả về bất kỳ dữ liệu gì",
                    "C. Hàm này chỉ chạy đồng bộ và trả về chuỗi lý do hủy",
                    "D. Hàm này dùng để tạo mới đơn hàng"
                },
                "A",
                "Task<Guid?> cho biết đây là hàm bất đồng bộ có kết quả trả về thuộc kiểu Guid? (có thể nhận giá trị Guid hoặc null), và nhận 2 tham số đầu vào là orderId cùng reason."
            ),

            // =====================================================================
            // BÀI 1.10
            // =====================================================================
            Build(
                topicId,
                "1.10",
                "Kỹ năng sống còn: Đặt Breakpoint (F9) và dùng F10 / F11 trên Visual Studio để dò luồng dữ liệu chạy từng dòng",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/visualstudio/debugger/debugger-feature-tour",
                """
                • Vũ khí mạnh nhất khi đi thực tập: Khi vào công ty nhận một project lớn hàng trăm file, đừng ngồi đọc chay bằng mắt! Hãy dùng Debugger của Visual Studio để bắt chương trình dừng lại và nhìn tận mắt dữ liệu chảy từ file này sang file khác:
                - Phím F9 (Toggle Breakpoint): Đặt một chấm đỏ ở dòng đầu tiên của hàm trong Controller. Khi bạn bấm gửi Request từ Postman/Swagger, Visual Studio sẽ tự động phanh gấp và dừng ngay tại chấm đỏ đó.
                - Di chuột vào biến (Hover): Xem ngay biến request đang chứa dữ liệu gì, biến nào bị null.
                - Phím F10 (Step Over): Chạy qua dòng hiện tại để sang dòng tiếp theo trong cùng 1 file.
                - Phím F11 (Step Into): Nhảy chui vào bên trong hàm đang được gọi (ví dụ đang ở Controller bấm F11 sẽ nhảy thẳng vào bên trong file Service tương ứng để xem nó chạy tiếp thế nào).
                - Phím F5 (Continue): Cho phép chương trình chạy tiếp bình thường đến hết luồng.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đặt bẫy tại cửa ngõ (Phím F9)",
                        "Mở file Controller, bấm F9 tạo chấm đỏ ở dòng gọi Service, sau đó bấm F5 chạy dự án ở chế độ Debug.",
                        "// [Chấm đỏ F9] tại đây:\nvar result = await _productService.CreateAsync(dto);"),
                    Step(2,
                        "Bắn Request từ Postman / Swagger",
                        "Gửi thử 1 request. Lập tức màn hình Visual Studio hiện vạch màu vàng dừng ngay tại chấm đỏ. Bạn di chuột vào biến 'dto' để soi giá trị.",
                        "// Vạch vàng dừng lại -> Soi thấy dto.Name = \"Laptop ASUS\", dto.Price = 20000000"),
                    Step(3,
                        "Chui sâu vào tầng dưới (Phím F11)",
                        "Bấm F11, màn hình tự động mở file ProductService.cs và nhảy con trỏ vào đúng hàm CreateAsync.",
                        "public async Task<ProductDto> CreateAsync(CreateProductDto dto) {\n    // Con trỏ nhảy vào đây!\n}"),
                    Step(4,
                        "Đi từng dòng kiểm tra (Phím F10)",
                        "Bấm F10 từng nhịp để xem dữ liệu đi qua bước kiểm tra, gọi Repository và lưu xuống Database rồi trả kết quả về.",
                        "await _repo.AddAsync(entity); // Bấm F10 qua từng dòng")
                },
                "Khi đang dừng ở chế độ Debug tại dòng var user = await _userService.GetByIdAsync(id); trong Controller, bạn muốn nhảy thẳng vào bên trong hàm GetByIdAsync của file UserService.cs để xem nó viết gì bên trong thì bấm phím nào?",
                new List<string>
                {
                    "A. Bấm phím F10 (Step Over)",
                    "B. Bấm phím F11 (Step Into)",
                    "C. Bấm phím F9",
                    "D. Bấm tổ hợp Ctrl + C"
                },
                "B",
                "F10 sẽ chạy lướt qua hàm đó để xuống dòng kế tiếp trong Controller, còn F11 (Step Into) sẽ đưa bạn đi sâu vào bên trong thân hàm GetByIdAsync ở tầng Service."
            )
        };
    }

    /// <summary>
    /// Đảm bảo 10 bài học con (1.1 -> 1.10) của Chủ đề 1 (Menu 1: INTERN_TRACK) luôn được nạp / cập nhật chính xác 100% vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var internTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "INTERN_TRACK");

        if (internTrack == null)
            return;

        var topic1 = internTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 1);
        if (topic1 == null)
            return;

        topic1.Title = "Chủ đề 1 (Thực tập): Đọc hiểu code C# & Cấu trúc dự án thực tế (10 Bài học con)";

        var targetLessons = GetLessons(topic1.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic1.Id)
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
