using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class InternTopic2Seed
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
    /// Danh sách đầy đủ 9 bài học con (2.1 -> 2.9) của Chủ đề 2 (Menu 1: Luồng Đi Thực Tập - Đọc luồng Hướng đối tượng OOP & DI trong dự án Web API)
    /// </summary>
    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 2.1
            // =====================================================================
            Build(
                topicId,
                "2.1",
                "Phân biệt 4 loại Class/Record trong dự án thực tế: Entity, DTO, ViewModel và Service/Controller",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-5",
                """
                • Bản chất: Khi mở một dự án thực tế ở công ty, bạn sẽ thấy hàng trăm file .cs đều khai báo là public class ... hoặc public record .... Để không bị rối, bạn chỉ cần chia chúng vào đúng 4 nhóm vai trò sau:
                1. Entity Class (Nằm ở thư mục Entities hoặc Models): Đại diện trực tiếp cho Bảng trong SQL Server. Mỗi thuộc tính là 1 cột trong bảng (thường chứa cả các cột nhạy cảm như PasswordHash, IsDeleted, CreatedAt).
                2. DTO - Data Transfer Object (Nằm ở thư mục DTOs hoặc Contracts): Là "thùng hàng trung gian" dùng để nhận dữ liệu từ Postman/Frontend gửi lên (CreateProductDto) hoặc trả dữ liệu về cho Client (ProductResponseDto). DTO giúp giấu đi cấu trúc bảng thật và các cột nhạy cảm của Entity. Trong .NET mới, DTO rất hay được viết bằng từ khóa record.
                3. ViewModel (Nằm ở dự án MVC): Chuyên dùng để gom dữ liệu đổ ra giao diện trang web Razor View (.cshtml).
                4. Controller / Service / Repository Class: Các class này không dùng để chứa dữ liệu, mà chứa các Hàm (Methods) để điều phối và xử lý luồng nghiệp vụ.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Client gửi Request DTO",
                        "Người dùng chỉ gửi lên đúng các thông tin cần thiết qua DTO (không có Id hay PasswordHash).",
                        "public record CreateUserDto(string Email, string FullName);"),
                    Step(2,
                        "Service chuyển DTO sang Entity",
                        "Tầng Service tạo đối tượng Entity hoàn chỉnh để chuẩn bị lưu xuống Database.",
                        "var userEntity = new User {\n    Id = Guid.NewGuid(),\n    Email = dto.Email,\n    PasswordHash = \"...\",\n    CreatedAt = DateTime.UtcNow\n};"),
                    Step(3,
                        "Repository lưu Entity vào SQL",
                        "EF Core chỉ làm việc với Entity để ánh xạ vào bảng SQL Server.",
                        "_dbContext.Users.Add(userEntity);\nawait _dbContext.SaveChangesAsync();"),
                    Step(4,
                        "Chuyển Entity sang Response DTO",
                        "Lọc bỏ mật khẩu, chỉ gói các trường an toàn vào DTO để trả về cho Client.",
                        "return new UserResponseDto(userEntity.Id, userEntity.Email, userEntity.FullName);")
                },
                "Khi đi thực tập, vì sao các anh Senior cấm tuyệt đối việc trả thẳng class User (Entity trong Database) ra ngoài API Controller mà bắt buộc phải map sang UserDto?",
                new List<string>
                {
                    "A. Vì Entity không thể chuyển thành định dạng JSON được",
                    "B. Để bảo mật (tránh lộ các cột nhạy cảm như PasswordHash) và tránh lỗi vòng lặp tham chiếu (Circular Reference) khi các bảng nối nhau",
                    "C. Vì dùng DTO giúp SQL Server không cần tạo bảng",
                    "D. Vì Controller chỉ nhận diện được class có chữ Dto ở cuối tên"
                },
                "B",
                "Trả trực tiếp Entity ra API sẽ làm lộ cấu trúc Database, lộ thông tin mật (như mật khẩu mã hóa) và rất dễ bị lỗi vòng lặp JSON khi bảng A liên kết bảng B, bảng B lại trỏ ngược về bảng A."
            ),

            // =====================================================================
            // BÀI 2.2
            // =====================================================================
            Build(
                topicId,
                "2.2",
                "Đọc luồng Model Binding: Cách dữ liệu từ Postman/Client tự động đổ vào các { get; set; } của C#",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/model-binding",
                """
                • Bản chất: Khi bạn viết một hàm trong Controller, làm sao dữ liệu từ URL, Query String hay Body JSON của Postman lại tự động chui vào đúng các thuộc tính { get; set; } của đối tượng C#? Cơ chế tự động đó của ASP.NET Core gọi là Model Binding.
                • Cách đọc 4 nhãn đánh dấu nguồn dữ liệu (Binding Attributes) trong hàm Controller:
                1. [FromBody]: Đọc dữ liệu từ cục JSON nằm trong phần Body của Request (thường dùng cho POST, PUT).
                2. [FromRoute]: Đọc dữ liệu nằm ngay trên đường dẫn URL (VD: GET /api/products/15 -> số 15 chui vào biến int id).
                3. [FromQuery]: Đọc dữ liệu nằm sau dấu hỏi chấm ? trên URL (VD: GET /api/products?keyword=asus&page=1 -> dùng cho tìm kiếm, phân trang).
                4. [FromForm]: Đọc dữ liệu gửi từ Form HTML hoặc khi có Upload File ảnh/tài liệu (IFormFile).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Client gửi Request kết hợp",
                        "Client gọi URL: PUT /api/products/99?notify=true kèm theo Body JSON.",
                        "// URL: /api/products/99?notify=true\n// Body: { \"name\": \"Chuột Gaming\", \"price\": 500000 }"),
                    Step(2,
                        "Bộ máy Model Binder phân tách",
                        "ASP.NET Core đọc từng phần của HTTP Request và khớp với các nhãn trong tham số hàm.",
                        "public async Task<IActionResult> Update(\n    [FromRoute] int id,\n    [FromQuery] bool notify,\n    [FromBody] UpdateProductDto dto)"),
                    Step(3,
                        "Gọi hàm set; của Property",
                        "Model Binder tự động gọi hàm set; trên từng thuộc tính của UpdateProductDto để gán giá trị.",
                        "// Lúc này trong code C#:\n// id = 99, notify = true, dto.Name = \"Chuột Gaming\"")
                },
                "Bạn đọc một hàm API tìm kiếm có khai báo: public async Task<IActionResult> Search([FromQuery] string keyword, [FromQuery] int page = 1). Khi test bằng Postman, bạn phải truyền keyword và page ở đâu?",
                new List<string>
                {
                    "A. Truyền trong tab Body dưới dạng raw JSON",
                    "B. Truyền ngay trên đường dẫn URL sau dấu chấm hỏi (VD: /api/items/search?keyword=laptop&page=2) hoặc tab Params của Postman",
                    "C. Truyền trong tab Headers",
                    "D. Truyền dưới dạng file đính kèm"
                },
                "B",
                "Thuộc tính [FromQuery] chỉ định rằng Model Binder sẽ lấy dữ liệu từ chuỗi truy vấn (Query String) nằm sau dấu ? trên thanh địa chỉ URL."
            ),

            // =====================================================================
            // BÀI 2.3
            // =====================================================================
            Build(
                topicId,
                "2.3",
                "Đọc hiểu Constructor Injection & Quan hệ Has-A: Tại sao Controller luôn khai báo private readonly IService",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/dependency-injection",
                """
                • Câu hỏi mọi thực tập sinh đều thắc mắc khi mở code công ty: "Tại sao ở đầu mọi file Controller hay Service đều có biến private readonly IProductService _productService; rồi lại viết một hàm khởi tạo (Constructor) gán _productService = productService; mà không dùng từ khóa new ProductService()?"
                • Giải thích bản chất:
                - Đây là sự kết hợp giữa quan hệ Has-A (Composition - Controller "có chứa" một Service để sai vặt) và cơ chế Dependency Injection (DI - Tiêm phụ thuộc qua Constructor).
                - Thay vì Controller phải tự đi khởi tạo new ProductService() (gây phụ thuộc cứng), Controller chỉ cần viết vào hàm Constructor: "Tôi cần một đối tượng tuân thủ hợp đồng IProductService". Khi có Request gọi đến, hệ thống .NET (đã cấu hình trong Program.cs) sẽ tự động new ngầm ở hậu trường và truyền (tiêm) vào Constructor cho bạn dùng ngay!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khai báo biến private readonly",
                        "Tạo một biến giữ chỗ ở đầu class. Từ khóa readonly đảm bảo biến này chỉ được gán 1 lần duy nhất trong Constructor, không sợ bị gán nhầm bằng null ở chỗ khác.",
                        "public class OrderController : ControllerBase {\n    private readonly IOrderService _orderService;"),
                    Step(2,
                        "Nhận đối tượng qua Constructor",
                        ".NET DI Container tự động khởi tạo OrderService và truyền vào tham số của Constructor khi có Request.",
                        "    public OrderController(IOrderService orderService) {\n        _orderService = orderService;\n    }"),
                    Step(3,
                        "Sử dụng trong các hàm API",
                        "Ở bất kỳ hàm nào bên dưới, bạn chỉ cần lấy biến _orderService ra chấm gọi hàm xử lý.",
                        "    [HttpPost]\n    public async Task<IActionResult> Create(OrderDto dto) {\n        await _orderService.CreateOrderAsync(dto);\n        return Ok();\n    }\n}")
                },
                "Ý nghĩa của từ khóa readonly trong khai báo private readonly IUserService _userService; ở đầu các class Controller/Service là gì?",
                new List<string>
                {
                    "A. Biến dữ liệu trong Database thành chỉ đọc, không cho phép xóa",
                    "B. Đảm bảo biến _userService chỉ được phép gán giá trị lúc khởi tạo trong Constructor, ngăn ngừa lập trình viên vô tình gán đè hoặc làm mất kết nối dịch vụ ở các hàm bên dưới",
                    "C. Giúp ẩn toàn bộ các hàm của IUserService",
                    "D. Bắt buộc phải có thì mới dùng được async/await"
                },
                "B",
                "readonly khóa chặt tham chiếu của biến sau khi Constructor chạy xong, giúp luồng dịch vụ an toàn tuyệt đối trong suốt vòng đời xử lý Request."
            ),

            // =====================================================================
            // BÀI 2.4
            // =====================================================================
            Build(
                topicId,
                "2.4",
                "Đọc hiểu Access Modifiers (private readonly vs public) để phân biệt hàm nội bộ và hàm mở cho tầng khác",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers",
                """
                • Mẹo đọc nhanh một file class dài hàng trăm dòng: Khi mở một file Service trong dự án, đừng đọc hết mọi hàm! Hãy nhìn vào từ khóa phạm vi truy cập (Access Modifier) ở đầu mỗi hàm để phân loại ngay:
                1. public: Hàm mở ra cho tầng khác gọi (chắc chắn nằm trong Interface). Đây là các luồng nghiệp vụ chính (VD: CreateOrderAsync, LoginAsync). Hãy đọc các hàm public trước!
                2. private: Hàm phụ trợ nội bộ (Helper method) chỉ dùng bên trong chính file .cs đó để chia nhỏ code cho đỡ dài (VD: CalculateTax(), ValidatePhoneFormat()). Chỉ đọc hàm private khi bạn đang dò luồng từ hàm public nhảy vào.
                3. protected: Chỉ dành cho các class con kế thừa (Inheritance) gọi (rất hay gặp trong BaseController hoặc BaseRepository).
                4. internal: Chỉ gọi được trong phạm vi cùng 1 Project (.csproj).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Controller gọi hàm public của Service",
                        "Controller ở tầng ngoài chỉ nhìn thấy và gọi được các hàm khai báo là public.",
                        "// Tại Controller:\nvar bill = await _paymentService.ProcessCheckoutAsync(cartDto);"),
                    Step(2,
                        "Hàm public điều phối luồng chính",
                        "Hàm public trong Service chạy luồng chính và gọi các hàm private nhỏ bên dưới để tính toán phụ.",
                        "public async Task<BillDto> ProcessCheckoutAsync(CartDto cart) {\n    decimal discount = CalculateDiscount(cart.Total); // Gọi hàm private\n    ...\n}"),
                    Step(3,
                        "Hàm private xử lý chi tiết ẩn",
                        "Được giấu kín bên trong Service, tầng ngoài không thể gọi bừa bãi.",
                        "private decimal CalculateDiscount(decimal total) {\n    return total > 1000000 ? total * 0.1m : 0;\n}")
                },
                "Khi đang ở ProductController, bạn muốn gọi hàm CheckInventory() nằm trong ProductService, nhưng gõ _productService.CheckInventory() lại báo lỗi đỏ không tìm thấy. Mở ProductService.cs ra xem thì thấy viết là private bool CheckInventory(). Lý do là gì?",
                new List<string>
                {
                    "A. Vì hàm đó đang để là private (chỉ nội bộ trong file ProductService mới gọi được) và chưa được khai báo trong IProductService",
                    "B. Vì hàm trả về kiểu bool nên Controller không gọi được",
                    "C. Vì Visual Studio bị treo bộ nhớ đệm",
                    "D. Vì tên hàm không có chữ Async ở cuối"
                },
                "A",
                "Phạm vi private giới hạn hàm đó chỉ được sử dụng bên trong chính class chứa nó. Muốn Controller gọi được qua DI, hàm phải được khai báo trong IProductService và để public ở ProductService."
            ),

            // =====================================================================
            // BÀI 2.5
            // =====================================================================
            Build(
                topicId,
                "2.5",
                "Đọc hiểu Kế thừa (Inheritance) trong khung .NET: Ý nghĩa thực sự của : ControllerBase, : Controller, : DbContext",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/object-oriented/inheritance",
                """
                • Bản chất: Khi đi làm, bạn sẽ thấy hầu hết các class trong dự án đều có dấu hai chấm : ở sau tên class (Kế thừa - Inheritance). Nhờ kế thừa lớp cha của Microsoft hoặc của công ty viết sẵn, class của bạn tự nhiên có sẵn hàng chục hàm quyền năng mà bạn không cần tự viết một dòng nào.
                • 4 kiểu kế thừa luôn gặp trong dự án thực tế:
                1. public class ProductsController : ControllerBase -> Dành cho Web API. Nhờ kế thừa ControllerBase, bạn mới gọi được các hàm Ok(), NotFound(), BadRequest(), CreatedAtAction() và biến User, HttpContext.
                2. public class HomeController : Controller -> Dành cho Web MVC. Lớp Controller kế thừa từ ControllerBase và bổ sung thêm hàm View(), ViewBag, ViewData, TempData để trả về giao diện .cshtml.
                3. public class AppDbContext : DbContext -> Nhờ kế thừa DbContext của EF Core, bạn mới có sẵn hàm SaveChangesAsync(), Set<T>(), Database.Migrate().
                4. public class ProductRepository : GenericRepository<Product> -> Kế thừa lớp dùng chung do các anh Senior trong công ty tự viết để có sẵn các hàm GetById, GetAll, Add, Delete mà không phải viết lại cho từng bảng.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nhìn dấu hai chấm (:) trên tên Class",
                        "Xác định ngay class này đang thừa hưởng sức mạnh từ lớp cha nào.",
                        "public class OrdersController : BaseApiController // Kế thừa lớp cha chung của công ty"),
                    Step(2,
                        "Kiểm tra hàm gọi từ lớp cha",
                        "Khi thấy một hàm được gọi (VD: GetCurrentUserId()) mà tìm khắp file hiện tại không thấy định nghĩa, chắc chắn nó nằm ở lớp cha!",
                        "Guid userId = GetCurrentUserId(); // Hàm này nằm trong BaseApiController"),
                    Step(3,
                        "Truyền tham số lên lớp cha bằng : base(...)",
                        "Đọc hiểu cách Constructor của lớp con chuyển tiếp dữ liệu cấu hình lên cho Constructor của lớp cha xử lý.",
                        "public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }")
                },
                "Khi đọc code file OrderRepository.cs, bạn thấy bên trong class hoàn toàn trống trơn (không có hàm GetByIdAsync nào được viết), nhưng bên OrderService vẫn gọi _orderRepo.GetByIdAsync(id) chạy mượt mà. Khi nhìn lên đầu file bạn thấy public class OrderRepository : BaseRepository<Order>, IOrderRepository. Hàm GetByIdAsync thực chất nằm ở đâu?",
                new List<string>
                {
                    "A. Nằm ẩn trong SQL Server",
                    "B. Được viết sẵn trong lớp cha BaseRepository<T> và OrderRepository kế thừa lại toàn bộ các hàm đó",
                    "C. Do AI tự động sinh ra lúc chạy",
                    "D. Nằm trong file appsettings.json"
                },
                "B",
                "Trong các dự án doanh nghiệp, các thao tác CRUD cơ bản giống nhau giữa các bảng đều được đưa lên lớp cha BaseRepository<T> (Generic Repository) để tái sử dụng, lớp con kế thừa là có sẵn ngay."
            ),

            // =====================================================================
            // BÀI 2.6
            // =====================================================================
            Build(
                topicId,
                "2.6",
                "Đọc hiểu Ghi đè (virtual / override) trong dự án: Cách đọc hàm OnModelCreating và SaveChangesAsync",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/knowing-when-to-use-override-and-new-keywords",
                """
                • Bản chất của Đa hình (Polymorphism - Overriding) khi đi làm: Microsoft viết sẵn các hàm chạy theo luồng mặc định trong lớp cha (được đánh dấu là virtual), nhưng cho phép lập trình viên viết đè (override) lại hàm đó ở lớp con để chèn thêm quy tắc riêng của công ty vào.
                • 2 chỗ dùng override nhiều nhất trong mọi dự án .NET:
                1. protected override void OnModelCreating(ModelBuilder modelBuilder) (Trong DbContext): Ghi đè lên quá trình tạo bảng để cấu hình khóa chính, khóa ngoại, đánh Index hoặc nạp dữ liệu mẫu (Fluent API).
                2. public override Task<int> SaveChangesAsync(...) (Trong DbContext): Các công ty rất hay override lại hàm lưu Database này để tự động gán ngày tạo (CreatedAt = DateTime.Now) hoặc biến lệnh Xóa thật (Delete) thành Xóa mềm (IsDeleted = true) trước khi gọi base.SaveChangesAsync().
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Service gọi SaveChangesAsync như bình thường",
                        "Lập trình viên gọi lệnh lưu xuống DB.",
                        "await _dbContext.SaveChangesAsync();"),
                    Step(2,
                        "Chạy vào hàm override trong AppDbContext",
                        "Trước khi lưu thật, hàm override chặn lại để tự động điền ngày giờ tạo/cập nhật cho mọi bản ghi mới.",
                        "public override Task<int> SaveChangesAsync(CancellationToken ct = default) {\n    foreach (var entry in ChangeTracker.Entries<BaseEntity>()) {\n        if (entry.State == EntityState.Added)\n            entry.Entity.CreatedAt = DateTime.UtcNow;\n    }"),
                    Step(3,
                        "Gọi base.SaveChangesAsync() để lưu thật",
                        "Sau khi đã gán xong dữ liệu tự động, gọi từ khóa base để kích hoạt hàm lưu SQL gốc của Microsoft.",
                        "    return base.SaveChangesAsync(ct);\n}")
                },
                "Khi đi thực tập, bạn thấy trong code ProductService lúc tạo mới sản phẩm không hề có dòng nào gán product.CreatedAt = DateTime.Now, nhưng khi lưu vào SQL Server cột CreatedAt vẫn luôn có đầy đủ ngày giờ hiện tại. Bạn nên kiểm tra hàm override nào trong AppDbContext để thấy đoạn code tự động gán đó?",
                new List<string>
                {
                    "A. Hàm override SaveChangesAsync()",
                    "B. Hàm Main() trong Program.cs",
                    "C. Hàm ToString()",
                    "D. Hàm Dispose()"
                },
                "A",
                "Ghi đè (override) hàm SaveChangesAsync trong DbContext là kỹ thuật chuẩn trong doanh nghiệp để tự động xử lý Audit Fields (CreatedAt, CreatedBy, UpdatedAt, Soft Delete) tập trung tại một nơi duy nhất."
            ),

            // =====================================================================
            // BÀI 2.7
            // =====================================================================
            Build(
                topicId,
                "2.7",
                "Kỹ năng cốt lõi đi thực tập: Cách lần dấu từ Interface sang Class thực thi bằng phím tắt (F12 và Ctrl + F12)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/visualstudio/ide/go-to-and-peek-definition",
                """
                • Nỗi ám ảnh lớn nhất của Intern khi đọc code dự án:
                Bạn mở OrderController.cs, thấy dòng await _orderService.CreateAsync(dto);. Bạn muốn xem hàm CreateAsync viết code xử lý gì bên trong nên bấm chuột phải chọn Go to Definition (F12). Nhưng Visual Studio lại mở ra file IOrderService.cs chỉ có đúng 1 dòng chữ ký Task CreateAsync(OrderDto dto); mà không hề có một dòng code xử lý nào!
                • Lý do & Bí quyết giải quyết trong 1 giây:
                - IOrderService chỉ là bản hợp đồng (Interface). Code xử lý thật nằm ở class thực thi (OrderService.cs).
                - Thay vì bấm F12 (Nhảy tới định nghĩa Interface), bạn hãy đặt con trỏ chuột vào tên hàm CreateAsync và bấm tổ hợp phím Ctrl + F12 (Go to Implementation - Đi tới nơi viết code thực thi). Visual Studio sẽ nhảy thẳng một phát vào đúng dòng code xử lý thật bên trong file OrderService.cs!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đứng tại lời gọi hàm ở Controller",
                        "Đặt con trỏ chuột vào tên hàm CreateOrderAsync mà bạn muốn đọc luồng.",
                        "var result = await _orderService.CreateOrderAsync(request);"),
                    Step(2,
                        "Nếu bấm F12 (Go to Definition)",
                        "Chỉ nhảy vào file Interface (IOrderService.cs) - Nơi chỉ chứa danh sách tên hàm, không có code logic.",
                        "public interface IOrderService {\n    Task<OrderDto> CreateOrderAsync(CreateOrderRequest request);\n}"),
                    Step(3,
                        "Bấm Ctrl + F12 (Go to Implementation)",
                        "Nhảy thẳng vào thân hàm thật sự trong file OrderService.cs để đọc toàn bộ luồng xử lý nghiệp vụ!",
                        "public class OrderService : IOrderService {\n    public async Task<OrderDto> CreateOrderAsync(CreateOrderRequest request) {\n        // Toàn bộ code xử lý thật nằm ở đây!\n    }\n}")
                },
                "Trong Visual Studio, khi đang đọc code ở Controller và muốn nhảy trực tiếp vào xem code xử lý bên trong class Service (bỏ qua bước dừng ở file Interface), bạn dùng phím tắt nào?",
                new List<string>
                {
                    "A. Phím F5",
                    "B. Tổ hợp phím Ctrl + F12 (Go to Implementation)",
                    "C. Tổ hợp phím Ctrl + Z",
                    "D. Phím F9"
                },
                "B",
                "F12 đưa bạn đến nơi khai báo kiểu (Interface), còn Ctrl + F12 (Go to Implementation) tìm đúng class đang thực thi Interface đó và mở thẳng hàm chứa code logic."
            ),

            // =====================================================================
            // BÀI 2.8
            // =====================================================================
            Build(
                topicId,
                "2.8",
                "Đọc hiểu các hàm tiện ích viết bằng static class, Extension Methods (từ khóa this) và partial class",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/extension-methods",
                """
                • 1. static class & Extension Methods (Phương thức mở rộng):
                - Khi đọc code Program.cs hoặc Service, bạn sẽ thấy những hàm được chấm trực tiếp như: services.AddApplicationServices(), hay product.ToProductDto(), hoặc str.ToSlug().
                - Làm sao kiểu string hay đối tượng product lại có sẵn hàm ToProductDto()? Đó là nhờ Extension Method: Một hàm static nằm trong một static class, có tham số đầu tiên bắt đầu bằng từ khóa this (VD: public static ProductDto ToProductDto(this Product entity)). Từ khóa this này gắn thêm hàm mới vào kiểu dữ liệu đứng sau nó!
                • 2. partial class (Lớp chia nhỏ nhiều file):
                - Khi một class có từ khóa public partial class User, nghĩa là class User này được tách ra nằm ở 2 hoặc nhiều file .cs khác nhau, khi biên dịch C# sẽ tự gộp chung lại thành 1 class. Rất hay gặp khi dùng công cụ Database-First của EF Core.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Định nghĩa Extension Method với từ khóa this",
                        "Tạo một static class chứa hàm biến đổi từ Entity sang DTO. Chú ý từ khóa 'this' đứng trước Product.",
                        "public static class ProductMappingExtensions {\n    public static ProductDto ToDto(this Product entity) {\n        return new ProductDto(entity.Id, entity.Name, entity.Price);\n    }\n}"),
                    Step(2,
                        "Gọi hàm mượt mà như hàm có sẵn",
                        "Ở tầng Service, bạn chỉ cần lấy đối tượng entity chấm trực tiếp .ToDto() cực kỳ gọn gàng.",
                        "Product entity = await _repo.GetByIdAsync(id);\nProductDto dto = entity.ToDto();"),
                    Step(3,
                        "Đọc gom nhóm dịch vụ trong Program.cs",
                        "Các công ty thường gom hàng chục lệnh AddScoped vào 1 hàm Extension Method để file Program.cs sạch đẹp.",
                        "// Định nghĩa: public static void AddMyServices(this IServiceCollection services)\n// Gọi tại Program.cs:\nbuilder.Services.AddMyServices();")
                },
                "Khi đọc một hàm được khai báo là: public static string FormatVnd(this decimal amount) nằm trong một static class, bạn sẽ sử dụng hàm này ở trong code như thế nào?",
                new List<string>
                {
                    "A. Lấy bất kỳ biến nào có kiểu decimal rồi chấm trực tiếp: decimal price = 50000; string text = price.FormatVnd();",
                    "B. Phải khởi tạo bằng từ khóa new trước khi gọi",
                    "C. Chỉ gọi được ở trong file SQL",
                    "D. Chỉ dùng được cho kiểu int"
                },
                "A",
                "Nhờ từ khóa this decimal amount ở tham số đầu tiên, FormatVnd trở thành một Extension Method gắn trực tiếp vào mọi biến có kiểu decimal."
            ),

            // =====================================================================
            // BÀI 2.9
            // =====================================================================
            Build(
                topicId,
                "2.9",
                "Thực hành đọc trọn vẹn luồng OOP trong Web API: Controller ➔ IService ➔ Service ➔ IRepository ➔ Repository ➔ DTO",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures",
                """
                • Tổng kết ghép nối toàn bộ kiến thức Chủ đề 1 & 2 vào một luồng dự án thật:
                Khi bạn đi thực tập và được giao nhiệm vụ: "Hãy đọc hiểu chức năng Lấy thông tin chi tiết nhân viên theo ID", mắt và tay của bạn sẽ đi theo đúng quy trình 5 bước chuẩn sau:
                1. Tại EmployeesController.cs: Tìm hàm có gắn [HttpGet("{id}")]. Xem nó nhận [FromRoute] int id và gọi _employeeService.GetByIdAsync(id).
                2. Bấm Ctrl + F12 vào GetByIdAsync: Nhảy thẳng từ IEmployeeService sang file EmployeeService.cs.
                3. Tại EmployeeService.cs: Đọc dòng gọi xuống kho dữ liệu _employeeRepo.GetByIdAsync(id). Thấy câu lệnh Guard Clause: if (emp == null) return null;.
                4. Bấm Ctrl + F12 vào _employeeRepo.GetByIdAsync: Nhảy sang file EmployeeRepository.cs để xem nó gọi _dbContext.Employees.FindAsync(id) lấy ra Employee (Entity).
                5. Quay lại EmployeeService.cs: Xem nó chuyển từ Employee (Entity) sang EmployeeDto (bằng hàm .ToDto() hoặc AutoMapper) rồi trả ngược về Controller để đóng gói vào Ok(dto) (HTTP 200).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Controller (Cửa ngõ tiếp nhận)",
                        "Nhận ID từ URL, ủy quyền xử lý cho IEmployeeService (tiêm qua Constructor).",
                        "[HttpGet(\"{id}\")]\npublic async Task<IActionResult> Get(int id) {\n    var dto = await _empService.GetByIdAsync(id);\n    if (dto == null) return NotFound();\n    return Ok(dto);\n}"),
                    Step(2,
                        "2. IEmployeeService (Hợp đồng dịch vụ)",
                        "Đóng vai trò cầu nối trung gian giữa Controller và Service. Dùng Ctrl+F12 để nhảy qua bước này.",
                        "public interface IEmployeeService {\n    Task<EmployeeDto?> GetByIdAsync(int id);\n}"),
                    Step(3,
                        "3. EmployeeService (Xử lý nghiệp vụ)",
                        "Gọi IEmployeeRepository lấy Entity, kiểm tra null và chuyển đổi sang DTO an toàn.",
                        "public async Task<EmployeeDto?> GetByIdAsync(int id) {\n    var entity = await _empRepo.GetByIdAsync(id);\n    if (entity == null) return null;\n    return entity.ToDto(); // Extension method\n}"),
                    Step(4,
                        "4. EmployeeRepository (Truy xuất DB)",
                        "Kế thừa hoặc sử dụng AppDbContext để lấy bản ghi thực tế từ bảng SQL Server.",
                        "public async Task<Employee?> GetByIdAsync(int id) {\n    return await _db.Employees.FindAsync(id);\n}"),
                    Step(5,
                        "5. Trả kết quả DTO về Client",
                        "Dữ liệu chảy ngược từ Repository (Entity) -> Service (DTO) -> Controller (200 OK JSON) -> Postman.",
                        "// Kết quả JSON trên Postman:\n// { \"id\": 1, \"fullName\": \"Nguyễn Hoàng Việt\", \"role\": \"Backend Intern\" }")
                },
                "Khi đi phỏng vấn thực tập, người phỏng vấn hỏi: \"Tại sao dự án phải tách ra Controller -> Service -> Repository thông qua các Interface mà không viết thẳng câu lệnh gọi Database (_db.Employees.FindAsync) ngay trong Controller cho nhanh?\". Bạn sẽ trả lời như thế nào để ghi điểm tuyệt đối?",
                new List<string>
                {
                    "A. Để dự án có nhiều file hơn cho giống dự án lớn",
                    "B. Để tách biệt trách nhiệm (Separation of Concerns): Controller chỉ lo HTTP Request/Response, Service lo tính toán nghiệp vụ, Repository lo Database; giúp code dễ đọc, dễ bảo trì, tái sử dụng được logic và dễ viết Unit Test",
                    "C. Vì Microsoft cấm gọi DbContext ở trong Controller",
                    "D. Vì viết trong Controller sẽ làm SQL Server chạy chậm hơn"
                },
                "B",
                "Nguyên lý Tách biệt trách nhiệm (Separation of Concerns) và Dependency Inversion (SOLID) là nền tảng của kiến trúc phần mềm doanh nghiệp, giúp thay đổi Database hay sửa logic nghiệp vụ mà không làm hỏng tầng giao tiếp API."
            )
        };
    }

    /// <summary>
    /// Đảm bảo 9 bài học con (2.1 -> 2.9) của Chủ đề 2 (Menu 1: INTERN_TRACK) luôn được nạp / cập nhật chính xác 100% vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var internTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "INTERN_TRACK");

        if (internTrack == null)
            return;

        var topic2 = internTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 2);
        if (topic2 == null)
            return;

        topic2.Title = "Chủ đề 2 (Thực tập): Đọc luồng Hướng đối tượng OOP & DI trong dự án Web API (9 Bài học con)";

        var targetLessons = GetLessons(topic2.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic2.Id)
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
