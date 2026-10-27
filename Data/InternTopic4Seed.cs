using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class InternTopic4Seed
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
    /// Danh sách đầy đủ 10 bài học con (4.1 -> 4.10) của Chủ đề 4 (Thực tập): Bóc Tách Từng Mắt Xích Luồng ASP.NET Core Web API (Client ➔ Controller ➔ Service ➔ Repository ➔ DB)
    /// </summary>
    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 4.1
            // =====================================================================
            Build(
                topicId,
                "4.1",
                "Toàn cảnh Vòng đời một HTTP Request (Request Lifecycle) trong ASP.NET Core Web API",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/middleware/",
                """
                • Bản chất: Khi người dùng bấm một nút trên Web/App (hoặc bạn bấm nút Send trên Postman), một gói tin gọi là HTTP Request sẽ bay tới máy chủ ASP.NET Core. Để không bị lạc lối giữa hàng trăm file code của công ty, bạn phải thuộc lòng Bản đồ 6 trạm dừng (6 mắt xích) mà gói tin này đi qua theo chiều đi và chiều về:
                1. Trạm 1 - Client (Postman / Frontend): Đóng gói URL, HTTP Method (GET, POST, PUT, DELETE), Header (chứa Token) và Body (chứa JSON).
                2. Trạm 2 - Middleware Pipeline & Routing (Program.cs): Trạm kiểm soát vé (kiểm tra lỗi toàn cục, kiểm tra đăng nhập Authentication, kiểm tra quyền Authorization) và dò đường dẫn URL để tìm đúng Controller.
                3. Trạm 3 - Controller (Controllers/): Tiếp nhận dữ liệu đã được Model Binding sang DTO, kiểm tra tính hợp lệ cơ bản và gọi tầng Service.
                4. Trạm 4 - Service (Services/): Bộ não xử lý các quy tắc nghiệp vụ (kiểm tra tồn kho, tính tiền, kiểm tra trùng lặp) rồi gọi Repository.
                5. Trạm 5 - Repository & DbContext (Repositories/, Data/): Giao tiếp với SQL Server thông qua Entity Framework Core để lấy hoặc lưu Entity.
                6. Trạm 6 - HTTP Response: Kết quả chảy ngược từ Repository ➔ Service (chuyển sang DTO) ➔ Controller (đóng gói kèm Status Code 200, 201, 400...) trả về cho Client.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Client ➔ Middleware ➔ Routing",
                        "Request đi qua các trạm gác bảo mật trong Program.cs trước khi được định tuyến vào đúng hàm Controller.",
                        "// Request: POST /api/orders\n// Đi qua: ExceptionHandler -> Https -> Cors -> Authentication -> Authorization"),
                    Step(2,
                        "2. Controller ➔ Service (Truyền DTO)",
                        "Controller không tự xử lý nghiệp vụ mà chuyển DTO xuống cho Service thông qua Interface.",
                        "[HttpPost]\npublic async Task<IActionResult> Create(CreateOrderDto dto) {\n    var result = await _orderService.CreateAsync(dto);\n    return Ok(result);\n}"),
                    Step(3,
                        "3. Service ➔ Repository ➔ SQL Server",
                        "Service kiểm tra logic, chuyển DTO thành Entity rồi nhờ Repository lưu xuống Database.",
                        "var entity = new Order { CustomerId = dto.CustomerId, Total = dto.Total };\nawait _orderRepo.AddAsync(entity);\nawait _unitOfWork.SaveChangesAsync();"),
                    Step(4,
                        "4. Chiều trả về: DB ➔ DTO ➔ HTTP Response",
                        "Dữ liệu hoàn tất được bọc trong mã trạng thái HTTP (200 OK / 201 Created) trả ra JSON cho Client.",
                        "// HTTP/1.1 201 Created\n// { \"orderId\": 105, \"status\": \"Success\" }")
                },
                "Khi một HTTP Request gửi từ Postman lên Server ASP.NET Core, nó sẽ đi qua thành phần nào ĐẦU TIÊN trước khi chạm được vào hàm trong Controller?",
                new List<string>
                {
                    "A. Chạy thẳng vào SQL Server trước rồi mới quay lại Controller",
                    "B. Đi qua chuỗi Middleware Pipeline và bộ định tuyến Routing được cấu hình trong Program.cs",
                    "C. Chạy vào Repository trước rồi mới lên Controller",
                    "D. Chạy vào file View .cshtml trước"
                },
                "B",
                "Mọi HTTP Request khi vào ứng dụng ASP.NET Core đều phải đi xuyên qua ống dẫn Middleware (như CORS, Authentication, Authorization, Routing) trong Program.cs trước khi được kích hoạt tại Controller."
            ),

            // =====================================================================
            // BÀI 4.2
            // =====================================================================
            Build(
                topicId,
                "4.2",
                "Mắt xích Routing & HTTP Methods: Cách đọc URL biết ngay hàm nào trong Controller sẽ chạy",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/controllers/routing",
                """
                • Kỹ năng thực chiến số 1: Khi đi thực tập, anh Leader đưa cho bạn 1 đường link API bị lỗi: PUT https://localhost:7001/api/v1/products/42/activate. Làm sao trong 5 giây bạn tìm ra đúng hàm C# đang xử lý đường link này trong số 50 file Controller của dự án?
                • Quy tắc ghép nối Route (Đường dẫn):
                1. Nhìn trên đầu class Controller: Thấy [Route("api/v1/[controller]")]. Chữ [controller] sẽ tự động được thay thế bằng tên class bỏ đi chữ Controller (ví dụ ProductsController ➔ api/v1/products).
                2. Nhìn trên đầu từng Hàm (Action): Kết hợp HTTP Verb ([HttpGet], [HttpPost], [HttpPut], [HttpDelete], [HttpPatch]) và Đường dẫn con.
                - Ví dụ trên hàm viết: [HttpPut("{id}/activate")]
                - Ghép với Route của class trên đầu ta được: PUT api/v1/products/{id}/activate (với {id} nhận giá trị 42).
                • Ý nghĩa chuẩn của 5 HTTP Methods trong RESTful API:
                - GET: Chỉ lấy dữ liệu (Read) — Không có Body.
                - POST: Tạo mới bản ghi (Create) — Dữ liệu gửi trong Body JSON.
                - PUT: Cập nhật toàn bộ bản ghi (Update) — Kèm {id} trên URL và Body JSON.
                - PATCH: Cập nhật 1 phần nhỏ (VD: chỉ đổi trạng thái Kích hoạt/Khóa).
                - DELETE: Xóa bản ghi — Kèm {id} trên URL.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đọc Route gốc trên đầu Class Controller",
                        "Xác định tiền tố đường dẫn chung cho toàn bộ các hàm bên trong file Controller đó.",
                        "[ApiController]\n[Route(\"api/[controller]\")] // Tương ứng với: /api/categories\npublic class CategoriesController : ControllerBase"),
                    Step(2,
                        "Ghép nối với Attribute trên đầu Hàm",
                        "Mỗi hàm gắn 1 HTTP Method và tham số ngoặc nhọn {id} nếu cần truyền biến trên URL.",
                        "[HttpGet(\"{id}/products\")] // Ghép lại thành: GET /api/categories/{id}/products\npublic async Task<IActionResult> GetProductsByCategory(int id)"),
                    Step(3,
                        "Khớp tham số {id} vào biến C#",
                        "Tên trong ngoặc nhọn {id} trên Route phải trùng khớp với tên tham số 'int id' của hàm.",
                        "// Khi gọi GET /api/categories/8/products -> biến id tự động nhận giá trị bằng 8")
                },
                "Trên đầu class OrdersController có gắn [Route(\"api/[controller]\")]. Bên trong có một hàm gắn [HttpPost(\"{orderId}/cancel\")]. Vậy từ Postman bạn phải gọi Method gì và URL nào để chạy vào đúng hàm đó?",
                new List<string>
                {
                    "A. Gọi GET /api/orders/cancel",
                    "B. Gọi POST /api/orders/123/cancel (với 123 là mã orderId)",
                    "C. Gọi PUT /api/OrdersController/123/cancel",
                    "D. Gọi DELETE /api/cancel/123"
                },
                "B",
                "[controller] lấy tên orders, ghép với [HttpPost(\"{orderId}/cancel\")] sẽ yêu cầu phương thức POST và đường dẫn /api/orders/{orderId}/cancel."
            ),

            // =====================================================================
            // BÀI 4.3
            // =====================================================================
            Build(
                topicId,
                "4.3",
                "Mắt xích Middleware Pipeline: Các trạm gác vô hình trước và sau Controller",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/middleware/#middleware-order",
                """
                • Bản chất: Hãy tưởng tượng Middleware giống như các cổng kiểm soát an ninh ở sân bay. Mọi Request từ ngoài đi vào Controller đều phải đi qua lần lượt từng cổng Middleware theo thứ tự khai báo trong Program.cs. Khi Controller xử lý xong, Response đi ra cũng phải đi ngược lại qua các cổng đó!
                • Thứ tự Middleware chuẩn trong mọi dự án .NET thực tế:
                1. UseExceptionHandler / CustomExceptionMiddleware (Đặt ở ngoài cùng để tóm gọn mọi lỗi xảy ra ở các tầng bên trong).
                2. UseHttpsRedirection (Ép chuyển sang HTTPS bảo mật).
                3. UseCors (Cực kỳ hay gặp lỗi khi đi thực tập: Cho phép Frontend React/Angular/Blazor ở cổng khác gọi vào API mà không bị trình duyệt chặn lỗi CORS policy).
                4. UseAuthentication() (Xác thực: Kiểm tra xem "Bạn là ai?" thông qua JWT Token).
                5. UseAuthorization() (Phân quyền: Kiểm tra xem "Bạn có quyền vào hàm này không?"). Lưu ý: Authentication bắt buộc phải đứng TRƯỚC Authorization!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Trạm 1: CORS & Bắt lỗi toàn cục",
                        "Kiểm tra nguồn gọi API có hợp lệ không và mở lưới hứng lỗi Exception cho toàn bộ luồng bên trong.",
                        "app.UseMiddleware<GlobalExceptionMiddleware>();\napp.UseCors(\"AllowFrontend\");"),
                    Step(2,
                        "Trạm 2: UseAuthentication (Bạn là ai?)",
                        "Đọc chuỗi Bearer Token trên Header, giải mã lấy thông tin UserId, Role gắn vào biến HttpContext.User.",
                        "app.UseAuthentication();"),
                    Step(3,
                        "Trạm 3: UseAuthorization (Có quyền không?)",
                        "Kiểm tra xem hàm Controller sắp tới có gắn thẻ [Authorize] không. Nếu chưa đăng nhập -> Trả thẳng 401 Unauthorized, chặn không cho vào Controller!",
                        "app.UseAuthorization();"),
                    Step(4,
                        "Hàm next() chuyển giao giữa các trạm",
                        "Trong code của mỗi Middleware, lệnh await _next(context) chính là chiếc cầu cho phép Request đi tiếp sang trạm kế tiếp.",
                        "public async Task InvokeAsync(HttpContext context) {\n    // Code chạy TRƯỚC khi vào Controller\n    await _next(context);\n    // Code chạy SAU khi Controller đã xử lý xong\n}")
                },
                "Khi ghép nối Frontend với Backend API hoặc khi cấu hình Program.cs, nếu bạn đặt nhầm dòng app.UseAuthorization(); nằm TRƯỚC dòng app.UseAuthentication(); thì hiện tượng gì sẽ xảy ra khi gọi vào các API có bảo mật [Authorize]?",
                new List<string>
                {
                    "A. Hệ thống vẫn chạy bình thường không ảnh hưởng gì",
                    "B. Dù bạn đã gửi kèm Token đăng nhập đúng, API vẫn luôn báo lỗi 401 Unauthorized vì hệ thống kiểm tra quyền trước khi kịp đọc và giải mã Token xem bạn là ai",
                    "C. Máy tính tự động khởi động lại",
                    "D. Database bị xóa trắng"
                },
                "B",
                "Thứ tự trong Middleware Pipeline mang tính quyết định: Phải xác thực danh tính (UseAuthentication) trước thì bước kiểm tra quyền hạn (UseAuthorization) mới có thông tin User để xét duyệt."
            ),

            // =====================================================================
            // BÀI 4.4
            // =====================================================================
            Build(
                topicId,
                "4.4",
                "Mắt xích Controller chuẩn Doanh nghiệp: \"Thin Controller\" & Các kiểu trả về IActionResult, ActionResult<T>",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/web-api/action-return-types",
                """
                • Triết lý "Thin Controller, Fat Service" (Controller gầy, Service béo):
                Khi đi thực tập, nếu bạn viết một hàm trong Controller dài 60 dòng (vừa kiểm tra điều kiện, vừa tính toán khuyến mãi, vừa gọi _db.Products...), khi Review Code chắc chắn anh Mentor sẽ bắt bạn viết lại!
                - Một hàm Controller chuẩn ở công ty chỉ dài từ 3 đến 10 dòng, làm đúng 3 việc: (1) Nhận Request DTO, (2) Gọi _service thực thi, (3) Trả về HTTP Response tương ứng.
                • Đọc hiểu 3 kiểu trả về của hàm Controller:
                1. Task<IActionResult>: Phổ biến nhất, cho phép trả về bất kỳ mã trạng thái nào như Ok(data) (200), CreatedAtAction(...) (201), NoContent() (204), BadRequest(error) (400), NotFound() (404).
                2. Task<ActionResult<ProductDto>>: Kiểu hiện đại giúp Swagger tự động nhận diện và hiển thị mẫu cấu trúc JSON của ProductDto trên tài liệu API mà không cần khai báo thêm thẻ [ProducesResponseType].
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nhận Request với chữ ký ActionResult<T>",
                        "Khai báo rõ kiểu dữ liệu thành công sẽ trả về nằm trong cặp thẻ <...> để Swagger đọc hiểu.",
                        "[HttpGet(\"{id}\")]\npublic async Task<ActionResult<ProductDto>> GetById(int id)"),
                    Step(2,
                        "Ủy quyền hoàn toàn cho tầng Service",
                        "Controller chỉ đóng vai trò người điều phối, đẩy việc nặng xuống Service.",
                        "{\n    var product = await _productService.GetByIdAsync(id);"),
                    Step(3,
                        "Đóng gói HTTP Status Code chuẩn xác",
                        "Trả về mã lỗi hoặc dữ liệu thành công tương ứng với kết quả của Service.",
                        "    if (product == null) return NotFound(new { message = \"Không tồn tại SP\" });\n    return Ok(product);\n}")
                },
                "Tại sao trong các dự án thực tế, nhiều lập trình viên thích dùng kiểu trả về Task<ActionResult<UserDto>> hơn là Task<IActionResult>?",
                new List<string>
                {
                    "A. Vì nó giúp định nghĩa rõ ràng kiểu dữ liệu trả về là UserDto (giúp Swagger tự hiển thị cấu trúc JSON mẫu) nhưng đồng thời vẫn cho phép return NotFound() hoặc BadRequest() khi có lỗi",
                    "B. Vì IActionResult không trả về được JSON",
                    "C. Vì ActionResult<T> không cần dùng từ khóa async/await",
                    "D. Vì nó tự động lưu dữ liệu vào SQL Server"
                },
                "A",
                "ActionResult<T> kết hợp được cả 2 ưu điểm: vừa linh hoạt trả về các mã lỗi HTTP (NotFound, BadRequest), vừa tường minh về kiểu dữ liệu trả về (T) cho công cụ tạo tài liệu Swagger."
            ),

            // =====================================================================
            // BÀI 4.5
            // =====================================================================
            Build(
                topicId,
                "4.5",
                "Mắt xích Service Layer & Mẫu bọc kết quả (ServiceResult / ApiResponse Wrapper Pattern)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api",
                """
                • Bản chất của tầng Service: Đây là nơi chứa 100% quy tắc nghiệp vụ (Business Logic). Ví dụ khi Đặt hàng: kiểm tra khách có bị khóa tài khoản không, kiểm tra mã giảm giá còn hạn không, kiểm tra số lượng tồn kho, tính tổng tiền... tất cả đều nằm ở OrderService.cs.
                • Cách đọc cấu trúc ApiResponse<T> / ServiceResult<T> trong dự án công ty:
                - Ở các dự án thực tế, Service thường không trả về mỗi biến ProductDto lẻ loi, mà bọc trong một hộp chuẩn gọi là ApiResponse<T> hoặc Result<T> gồm 3 thuộc tính cố định:
                + bool IsSuccess: Cho biết thành công (true) hay thất bại (false).
                + string Message: Thông báo gửi cho người dùng (VD: "Tạo sản phẩm thành công" hoặc "Số dư không đủ").
                + T? Data: Dữ liệu chính trả về.
                - Nhờ cái hộp chuẩn này, đội làm Frontend (React/Mobile) lúc nào cũng đọc đúng cấu trúc { isSuccess, message, data } đồng nhất trên toàn bộ 100 API của dự án!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Định nghĩa khung bọc ApiResponse<T>",
                        "Class dùng chung cho mọi API trong dự án để chuẩn hóa định dạng JSON trả về.",
                        "public class ApiResponse<T> {\n    public bool IsSuccess { get; set; }\n    public string Message { get; set; }\n    public T? Data { get; set; }\n}"),
                    Step(2,
                        "Service kiểm tra nghiệp vụ và trả về Result",
                        "Nếu vi phạm nghiệp vụ, trả về IsSuccess = false kèm lý do rõ ràng thay vì làm sập chương trình.",
                        "if (product.Stock < request.Quantity)\n    return ApiResponse<OrderDto>.Fail(\"Số lượng tồn kho không đủ!\");\n\n// Nếu hợp lệ -> Lưu DB và trả về thành công:\nreturn ApiResponse<OrderDto>.Success(orderDto, \"Đặt hàng thành công\");"),
                    Step(3,
                        "Controller đọc IsSuccess để trả mã HTTP",
                        "Controller kiểm tra cờ IsSuccess để quyết định trả về mã 200 OK hay 400 BadRequest.",
                        "var result = await _orderService.CreateAsync(dto);\nif (!result.IsSuccess) return BadRequest(result);\nreturn Ok(result);")
                },
                "Mục đích chính của việc các công ty luôn dùng một class bọc kết quả như ApiResponse<T> (chứa IsSuccess, Message, Data) cho mọi API là gì?",
                new List<string>
                {
                    "A. Để chuẩn hóa cấu trúc JSON trả về đồng nhất trên toàn hệ thống, giúp Frontend dễ dàng xử lý thông báo lỗi/thành công và giúp Service truyền thông điệp lỗi nghiệp vụ lên Controller rõ ràng",
                    "B. Để tăng dung lượng file JSON cho nặng hơn",
                    "C. Vì C# bắt buộc phải có class tên là ApiResponse",
                    "D. Để thay thế hoàn toàn cho cơ sở dữ liệu"
                },
                "A",
                "Chuẩn hóa Response Wrapper (ApiResponse<T> / Result<T>) giúp giao tiếp giữa Backend và Frontend nhất quán, sạch sẽ và dễ bắt lỗi nghiệp vụ."
            ),

            // =====================================================================
            // BÀI 4.6
            // =====================================================================
            Build(
                topicId,
                "4.6",
                "Mắt xích AutoMapper vs Manual Mapping: Đọc luồng chuyển đổi dữ liệu giữa DTO và Entity",
                "OOP thực tế",
                "https://docs.automapper.org/en/stable/Getting-started.html",
                """
                • Tại sao phải có bước Mapping (Chuyển đổi dữ liệu)?
                - Khi nhận dữ liệu vào (POST/PUT): Bạn phải chép dữ liệu từ CreateProductDto sang Product (Entity) để lưu vào Database.
                - Khi trả dữ liệu ra (GET): Bạn phải chép dữ liệu từ Product (Entity) sang ProductDto để trả về cho Client.
                • 2 trường phái Mapping bạn sẽ gặp khi đọc code công ty:
                1. Manual Mapping (Viết hàm chuyển đổi tay qua Extension Method): Gán rõ ràng từng dòng Name = entity.Name, Price = entity.Price. Ưu điểm: Chạy cực nhanh, dễ đọc, bấm F12 là thấy ngay cột nào gán vào cột nào, không sợ lỗi ngầm.
                2. Thư viện AutoMapper (_mapper.Map<TDestination>(source)): Tự động chép các thuộc tính có cùng tên và cùng kiểu dữ liệu từ đối tượng này sang đối tượng kia chỉ bằng 1 dòng lệnh _mapper.Map<ProductDto>(entity). Các quy tắc gán đặc biệt được khai báo tập trung trong file kế thừa : Profile (VD: MappingProfile.cs).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Cấu hình bản đồ trong MappingProfile.cs",
                        "Khai báo cho AutoMapper biết cặp class nào được phép chuyển đổi qua lại với nhau.",
                        "public class MappingProfile : Profile {\n    public MappingProfile() {\n        CreateMap<Product, ProductDto>();\n        CreateMap<CreateProductDto, Product>();\n    }\n}"),
                    Step(2,
                        "Tiêm IMapper vào Constructor của Service",
                        "Sử dụng Dependency Injection để lấy công cụ _mapper vào trong Service.",
                        "private readonly IMapper _mapper;\npublic ProductService(IMapper mapper) { _mapper = mapper; }"),
                    Step(3,
                        "Gọi _mapper.Map<Đích>(Nguồn) trong luồng xử lý",
                        "Biến đổi từ DTO sang Entity (khi thêm mới) hoặc từ Entity sang DTO (khi lấy dữ liệu) chỉ bằng 1 dòng.",
                        "// Chuyển từ CreateProductDto (nguồn) sang Product Entity (đích):\nProduct entity = _mapper.Map<Product>(createDto);")
                },
                "Khi đọc code trong UserService.cs, bạn thấy dòng lệnh: var userDto = _mapper.Map<UserDto>(userEntity);, nhưng khi chạy API thì phát hiện một trường đặc biệt là FullName trong userDto bị rỗng, hoặc báo lỗi AutoMapperMappingException. Bạn cần mở file nào trong dự án để kiểm tra quy tắc nối trường của AutoMapper?",
                new List<string>
                {
                    "A. Mở file kế thừa từ class Profile của AutoMapper (thường tên là MappingProfile.cs hoặc AutoMapperProfile.cs) nơi chứa các lệnh CreateMap<User, UserDto>()",
                    "B. Mở file launchSettings.json",
                    "C. Mở bảng trong SQL Server",
                    "D. Mở file HTML giao diện"
                },
                "A",
                "Mọi cấu hình ánh xạ của AutoMapper (CreateMap<Source, Destination>().ForMember(...)) đều được định nghĩa tập trung trong các class kế thừa từ AutoMapper.Profile."
            ),

            // =====================================================================
            // BÀI 4.7
            // =====================================================================
            Build(
                topicId,
                "4.7",
                "Mắt xích Repository & Unit of Work Pattern: Cách dự án lớn tổ chức tầng truy cập Database",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application",
                """
                • Tại sao dự án công ty lại bọc DbContext bên trong Repository và UnitOfWork?
                1. GenericRepository<T> (Kho dùng chung): Chứa sẵn các hàm cơ bản mà bảng nào cũng cần: GetByIdAsync, GetAllAsync, AddAsync, Update, Delete. Nhờ vậy khi tạo bảng mới, bạn không phải viết lại các hàm CRUD này nữa.
                2. ProductRepository (Kho riêng của từng bảng): Kế thừa từ GenericRepository<Product> và viết thêm các câu truy vấn phức tạp chỉ riêng bảng Sản phẩm mới có (VD: GetTopSellingProductsAsync()).
                3. UnitOfWork (Đơn vị công việc chung): Khi một hàm trong OrderService vừa gọi _orderRepo.Add(order) vừa gọi _productRepo.Update(product), nếu mỗi Repository tự gọi SaveChangesAsync() riêng lẻ thì sẽ không đảm bảo tính toàn vẹn giao dịch. Do đó, các Repository chỉ làm nhiệm vụ .Add() / .Update() trên bộ nhớ, và cuối cùng gọi await _unitOfWork.SaveChangesAsync() một lần duy nhất để lưu tất cả xuống DB cùng lúc!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Repository thực hiện thao tác trên DbSet",
                        "Các hàm trong Repository chỉ thêm/sửa/xóa trạng thái trên bộ nhớ DbContext chứ chưa gọi SaveChanges.",
                        "public async Task AddAsync(Order order) {\n    await _dbContext.Orders.AddAsync(order);\n}"),
                    Step(2,
                        "Service phối hợp nhiều Repository",
                        "Trong 1 nghiệp vụ đặt hàng, Service gọi cả OrderRepository và ProductRepository.",
                        "await _orderRepo.AddAsync(newOrder);\n_productRepo.UpdateStock(productId, quantity);"),
                    Step(3,
                        "UnitOfWork chốt lưu toàn bộ xuống SQL",
                        "Gọi _unitOfWork.SaveChangesAsync() (hoặc CompleteAsync) ở dòng cuối cùng của Service để lưu đồng bộ tất cả thay đổi.",
                        "await _unitOfWork.SaveChangesAsync(); // Lưu cả Order và Product trong 1 lần!")
                },
                "Trong các dự án áp dụng mẫu thiết kế Repository + Unit of Work, vì sao bên trong hàm AddAsync của Repository người ta thường KHÔNG gọi lệnh _dbContext.SaveChangesAsync() ngay mà để tầng Service gọi _unitOfWork.SaveChangesAsync() ở cuối hàm?",
                new List<string>
                {
                    "A. Vì bên trong Repository không có quyền gọi SaveChangesAsync",
                    "B. Để gom nhiều thao tác thay đổi trên nhiều bảng (nhiều Repository khác nhau) vào chung một giao dịch (Transaction) duy nhất; chỉ khi tất cả các bước trong Service hoàn tất mới lưu xuống Database một lần",
                    "C. Vì quên không viết",
                    "D. Để bắt người dùng phải bấm nút lưu 2 lần"
                },
                "B",
                "Mẫu Unit of Work giúp quản lý một phiên làm việc với Database: gom tất cả thay đổi từ nhiều Repository lại và thực thi SaveChangesAsync() một lần duy nhất, đảm bảo tính toàn vẹn dữ liệu."
            ),

            // =====================================================================
            // BÀI 4.8
            // =====================================================================
            Build(
                topicId,
                "4.8",
                "Đọc luồng Xử lý lỗi Tập trung (Global Exception Handling Middleware) & Custom Exceptions",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/error-handling",
                """
                • Cách dự án chuyên nghiệp xử lý try - catch:
                - Ở các dự án nhỏ trên trường, sinh viên thường viết try { ... } catch (Exception ex) { return StatusCode(500, ex.Message); } lặp đi lặp lại ở khắp mọi hàm trong Controller.
                - Ở công ty: Trong Controller và Service hầu như KHÔNG thấy viết try - catch! Vậy khi có lỗi xảy ra thì ai bắt?
                - Câu trả lời: Toàn bộ dự án chỉ dùng 1 cái lưới bắt lỗi khổng lồ duy nhất đặt ở đầu Program.cs gọi là GlobalExceptionMiddleware (hoặc IExceptionHandler).
                • Cách luồng lỗi hoạt động:
                - Ở tầng Service, nếu không tìm thấy bản ghi, lập trình viên chỉ cần ném lỗi: throw new NotFoundException("Không tìm thấy đơn hàng"); hoặc nếu sai quy tắc thì throw new BadRequestException("Mã giảm giá đã hết hạn");.
                - Lỗi đó sẽ bay thẳng ra trạm GlobalExceptionMiddleware. Trạm này tự động ghi Log lỗi (_logger.LogError) và nhìn kiểu lỗi để tự động đổi thành mã HTTP chuẩn (NotFoundException ➔ 404, BadRequestException ➔ 400, lỗi hệ thống khác ➔ 500) trả về JSON đẹp mắt cho Client!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Service chủ động ném lỗi (throw Exception)",
                        "Khi phát hiện dữ liệu vi phạm nghiệp vụ, Service ném ra Custom Exception để ngắt luồng ngay lập tức.",
                        "var user = await _repo.GetByIdAsync(id);\nif (user == null)\n    throw new NotFoundException($\"Không tìm thấy User có ID = {id}\");"),
                    Step(2,
                        "GlobalExceptionMiddleware hứng trọn lỗi",
                        "Khối catch ở Middleware ngoài cùng tóm được lỗi bay ra từ bất kỳ tầng nào (Repository, Service, Controller).",
                        "try {\n    await _next(context);\n} catch (Exception ex) {\n    _logger.LogError(ex, \"Có lỗi xảy ra!\");\n    await HandleExceptionAsync(context, ex);\n}"),
                    Step(3,
                        "Phân loại Exception thành HTTP Status Code",
                        "Dựa vào loại Exception để trả về đúng mã 400, 404 hay 500 kèm JSON thông báo chuẩn.",
                        "context.Response.StatusCode = ex switch {\n    NotFoundException => StatusCodes.Status404NotFound,\n    BadRequestException => StatusCodes.Status400BadRequest,\n    _ => StatusCodes.Status500InternalServerError\n};")
                },
                "Khi đọc code dự án công ty, bạn thấy trong hàm Service có dòng throw new KeyNotFoundException(\"Sản phẩm không tồn tại\"); nhưng cả hàm Service lẫn hàm Controller gọi nó đều không hề có khối try - catch nào, thế nhưng khi test trên Postman vẫn nhận về thông báo lỗi JSON mã 404 rất gọn gàng. Đoạn code bắt lỗi đó nằm ở đâu?",
                new List<string>
                {
                    "A. Nằm trong Global Exception Handling Middleware (được đăng ký ở Program.cs để bắt và xử lý tập trung mọi Exception của toàn dự án)",
                    "B. Do Postman tự động sửa lỗi",
                    "C. Nằm trong SQL Server",
                    "D. Do trình duyệt tự dịch lỗi"
                },
                "A",
                "Xử lý ngoại lệ toàn cục (Global Exception Handling) giúp giữ cho code ở Controller và Service sạch sẽ, không bị rác bởi hàng trăm khối try-catch lặp lại."
            ),

            // =====================================================================
            // BÀI 4.9
            // =====================================================================
            Build(
                topicId,
                "4.9",
                "Đọc hiểu Vòng đời Dịch vụ (Service Lifetimes): Phân biệt AddTransient, AddScoped và AddSingleton",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/core/extensions/dependency-injection#service-lifetimes",
                """
                • Câu hỏi phỏng vấn bắt buộc phải thuộc lòng 100% khi đi xin thực tập .NET:
                "Khi đăng ký Dependency Injection trong Program.cs, sự khác nhau giữa AddTransient, AddScoped và AddSingleton là gì? Khi nào dùng cái nào?"
                • Giải thích cực kỳ dễ nhớ theo vòng đời của 1 HTTP Request:
                1. AddScoped<IService, Service>() (Dùng nhiều nhất - 85% trong Web API):
                - Mỗi một HTTP Request gửi lên sẽ được cấp ĐÚNG 1 đối tượng (Instance) duy nhất và dùng chung xuyên suốt từ đầu đến cuối Request đó. Khi Request trả về kết quả xong, đối tượng đó bị hủy giải phóng RAM.
                - Ứng dụng: Luôn dùng cho DbContext, tất cả các Repository, UnitOfWork và hầu hết các Service nghiệp vụ.
                2. AddTransient<IService, Service>() (Tạo mới liên tục):
                - Cứ mỗi một nơi nào yêu cầu (Inject) trong cùng 1 Request là nó lại new ra một đối tượng hoàn toàn mới.
                - Ứng dụng: Dùng cho các dịch vụ nhỏ, nhẹ, không lưu trạng thái (Stateless) như EmailSender, Validator.
                3. AddSingleton<IService, Service>() (Duy nhất toàn server):
                - Chỉ khởi tạo 1 lần duy nhất khi bật Server và tất cả mọi người dùng, mọi Request đều xài chung đối tượng đó cho đến khi tắt Server.
                - Ứng dụng: Dùng cho MemoryCache, Configuration, Logger. (Cấm kỵ: Không bao giờ đăng ký DbContext là Singleton!).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "AddScoped (1 Instance / 1 HTTP Request)",
                        "Chuẩn mực cho DbContext, Repository và Business Service. Đảm bảo trong 1 lần gọi API, các tầng dùng chung 1 kết nối DB an toàn.",
                        "builder.Services.AddDbContext<AppDbContext>(); // Mặc định là Scoped\nbuilder.Services.AddScoped<IOrderRepository, OrderRepository>();\nbuilder.Services.AddScoped<IOrderService, OrderService>();"),
                    Step(2,
                        "AddTransient (Luôn tạo mới mỗi lần gọi)",
                        "Dành cho các công cụ tính toán độc lập, nhẹ nhàng, không giữ dữ liệu.",
                        "builder.Services.AddTransient<IEmailNotification, EmailNotification>();"),
                    Step(3,
                        "AddSingleton (1 Instance duy nhất cho cả Server)",
                        "Dùng chung cho toàn bộ ứng dụng từ lúc bật web đến lúc tắt web.",
                        "builder.Services.AddMemoryCache(); // Singleton lưu đệm trên RAM")
                },
                "Khi đi phỏng vấn C# Intern, nhà tuyển dụng hỏi: \"Vòng đời mặc định của AddDbContext là gì? Và tại sao các Repository hay Service làm việc với Database lại luôn được đăng ký là AddScoped?\". Đáp án chuẩn xác nhất là:",
                new List<string>
                {
                    "A. Là Scoped. Vì DbContext không an toàn khi chạy đa luồng cùng lúc (Not Thread-Safe), nên mỗi HTTP Request cần có một phiên DbContext và Repository riêng biệt, dùng xong trong Request đó là giải phóng ngay",
                    "B. Là Singleton để cả nghìn người dùng chung 1 kết nối Database cho tiết kiệm",
                    "C. Là Transient để mỗi dòng code lại mở một kết nối SQL mới",
                    "D. Cả 3 cách đều giống hệt nhau"
                },
                "A",
                "DbContext được thiết kế theo mẫu Unit of Work cho từng yêu cầu đơn lẻ (Scoped). Nếu để Singleton, nhiều người dùng gọi API cùng lúc trên cùng 1 DbContext sẽ gây xung đột dữ liệu và văng lỗi ngay lập tức."
            ),

            // =====================================================================
            // BÀI 4.10
            // =====================================================================
            Build(
                topicId,
                "4.10",
                "Tổng thực hành Chủ đề 4: Đọc thông suốt 2 luồng API hoàn chỉnh (GET Danh sách có lọc & POST Tạo mới)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/tutorials/first-web-api",
                """
                • Bài tổng duyệt trước khi bước sang phần Kiểm thử & Bảo mật:
                Khi bạn mở bất kỳ dự án ASP.NET Core Web API nào ở công ty, 90% công việc hàng ngày của bạn xoay quanh 2 luồng chuẩn mực này:
                - Luồng 1 - Luồng Đọc dữ liệu (HTTP GET /api/products?keyword=asus&page=1):
                Client ➔ Controller nhận [FromQuery] ➔ gọi _service.GetPagedAsync() ➔ gọi _repo khởi tạo IQueryable với .AsNoTracking() ➔ nối .Where() lọc từ khóa ➔ đếm CountAsync() ➔ cắt trang .Skip().Take() ➔ chiếu .Select() sang ProductDto ➔ chốt ToListAsync() ➔ trả về 200 OK kèm JSON.
                - Luồng 2 - Luồng Ghi dữ liệu (HTTP POST /api/products kèm JSON Body):
                Client ➔ [ApiController] tự kiểm tra [Required] trên CreateProductDto ➔ Controller nhận [FromBody] ➔ gọi _service.CreateAsync(dto) ➔ Service kiểm tra trùng mã bằng AnyAsync() (nếu trùng trả lỗi 400) ➔ Map từ CreateProductDto sang Product (Entity) ➔ gọi _repo.AddAsync(entity) ➔ gọi SaveChangesAsync() lưu vào SQL Server ➔ trả về 201 Created (hoặc 200 OK).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Cửa ngõ Controller tiếp nhận Request",
                        "Nhận DTO từ Body, ủy quyền cho tầng Service xử lý.",
                        "[HttpPost]\npublic async Task<IActionResult> Create([FromBody] CreateProductDto dto) {\n    var result = await _productService.CreateAsync(dto);\n    return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);\n}"),
                    Step(2,
                        "Service kiểm tra quy tắc nghiệp vụ",
                        "Dùng Guard Clause kết hợp AnyAsync để chặn dữ liệu trùng lặp trước khi cho phép tạo mới.",
                        "public async Task<ProductDto> CreateAsync(CreateProductDto dto) {\n    if (await _repo.ExistsByCodeAsync(dto.Code))\n        throw new BadRequestException(\"Mã sản phẩm đã tồn tại!\");"),
                    Step(3,
                        "Chuyển đổi sang Entity & Lưu xuống DB",
                        "Ánh xạ từ DTO sang Entity, thêm vào DbContext và gọi SaveChangesAsync để sinh lệnh INSERT INTO.",
                        "    var entity = new Product {\n        Code = dto.Code.Trim().ToUpper(),\n        Name = dto.Name.Trim(),\n        Price = dto.Price\n    };\n    await _repo.AddAsync(entity);\n    await _unitOfWork.SaveChangesAsync();"),
                    Step(4,
                        "Trả Entity đã có ID mới về thành DTO",
                        "Sau khi SaveChangesAsync(), SQL Server tự sinh Id cho entity. Ta gói vào DTO trả ngược lên Controller.",
                        "    return new ProductDto(entity.Id, entity.Code, entity.Name, entity.Price);\n}")
                },
                "Trong luồng POST tạo mới một bản ghi (với khóa chính Id kiểu int tự tăng trong SQL Server), ở thời điểm nào trong code C# thì thuộc tính entity.Id chính thức nhận được con số ID thật do SQL Server cấp (ví dụ entity.Id = 15)?",
                new List<string>
                {
                    "A. Ngay lúc vừa viết dòng var entity = new Product();",
                    "B. Ngay lúc gọi dòng _dbContext.Products.Add(entity);",
                    "C. Ngay sau khi dòng lệnh await _dbContext.SaveChangesAsync(); thực thi thành công",
                    "D. Phải khởi động lại Visual Studio mới có Id"
                },
                "C",
                "Lúc gọi .Add(entity), đối tượng mới chỉ nằm trên RAM. Chỉ sau khi await _dbContext.SaveChangesAsync() chạy xong, EF Core thực thi lệnh INSERT xuống SQL Server và tự động lấy mã IDENTITY vừa sinh ra gán ngược lại vào thuộc tính entity.Id cho bạn dùng ngay ở dòng tiếp theo."
            )
        };
    }

    /// <summary>
    /// Thay thế các bài mẫu tạm thời và nạp/cập nhật đầy đủ 10 bài học con (4.1 -> 4.10) cho Chủ đề 4 (Menu 1: INTERN_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var internTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "INTERN_TRACK");

        if (internTrack == null)
            return;

        var topic4 = internTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 4);
        if (topic4 == null)
            return;

        topic4.Title = "Chủ đề 4 (Thực tập): Bóc Tách Từng Mắt Xích Luồng ASP.NET Core Web API (Client ➔ Controller ➔ Service ➔ Repository ➔ DB)";
        topic4.Description = "10 Bài học con chuyên sâu bóc tách Request Lifecycle, Routing, Middleware Pipeline, Thin Controller, ApiResponse Wrapper, AutoMapper, Repository & UnitOfWork, Global Exception Handling & Service Lifetimes.";

        var targetLessons = GetLessons(topic4.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic4.Id)
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
