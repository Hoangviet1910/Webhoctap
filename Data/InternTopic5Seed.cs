using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class InternTopic5Seed
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
    /// Danh sách đầy đủ 10 bài học con (5.1 -> 5.10) của Chủ đề 5 (Thực tập): Thực Hành Kiểm Thử API (Swagger, Postman), Đọc Luồng Xác Thực JWT & Kỹ Năng Đi Làm Thực Tế
    /// </summary>
    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 5.1
            // =====================================================================
            Build(
                topicId,
                "5.1",
                "Đọc hiểu & Sử dụng Swagger UI (OpenAPI) để kiểm thử luồng Backend không cần Frontend",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/tutorials/getting-started-with-swashbuckle",
                """
                • Bản chất: Khi đi thực tập Backend, bạn viết hoặc sửa xong một API nhưng bên đội Frontend chưa làm xong giao diện, làm sao bạn biết code của mình chạy đúng hay sai? Câu trả lời là dùng Swagger UI (đường dẫn thường là https://localhost:xxxx/swagger).
                • Cách đọc 4 thành phần trên giao diện Swagger UI:
                1. Nhóm theo Controller: Mỗi Controller trong code C# sẽ hiển thị thành một mục lớn trên Swagger.
                2. Màu sắc các dòng API:
                - Màu Xanh dương (GET): Lấy dữ liệu.
                - Màu Xanh lá (POST): Tạo mới dữ liệu.
                - Màu Cam (PUT): Cập nhật dữ liệu.
                - Màu Đỏ (DELETE): Xóa dữ liệu.
                3. Nút Try it out ➔ Execute: Bấm Try it out để mở ô nhập tham số/JSON, sau đó bấm Execute để bắn Request thẳng vào Controller của bạn (kích hoạt ngay các chấm đỏ Breakpoint F9 đang đặt trong Visual Studio!).
                4. Mục Schemas (Ở dưới cùng trang Swagger): Liệt kê cấu trúc chi tiết của toàn bộ các class DTO trong dự án.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Cấu hình bật Swagger trong Program.cs",
                        "Swagger thường chỉ được bật ở môi trường Development (khi lập trình viên đang code).",
                        "if (app.Environment.IsDevelopment()) {\n    app.UseSwagger();\n    app.UseSwaggerUI();\n}"),
                    Step(2,
                        "Bấm Try it out và điền JSON mẫu",
                        "Swagger tự động đọc class CreateProductDto của bạn để sinh sẵn khung JSON mẫu, bạn chỉ việc điền giá trị thật vào.",
                        "{\n  \"code\": \"SP01\",\n  \"name\": \"Bàn phím cơ\",\n  \"price\": 1200000\n}"),
                    Step(3,
                        "Bấm Execute và đọc Server Response",
                        "Xem trực tiếp mã trạng thái HTTP (200, 400, 500) và dữ liệu JSON trả về ngay trên trình duyệt.",
                        "// Response Code: 200\n// Response Body: { \"isSuccess\": true, \"data\": { \"id\": 10 } }")
                },
                "Khi bạn kéo source code của công ty về máy và bấm F5 chạy dự án ở chế độ Production (hoặc Staging), trình duyệt báo lỗi 404 Not Found khi vào trang /swagger. Mở file Program.cs ra bạn nên kiểm tra câu lệnh điều kiện nào?",
                new List<string>
                {
                    "A. Kiểm tra khối lệnh if (app.Environment.IsDevelopment()) vì mặc định Swagger chỉ được cấu hình bật khi môi trường chạy là Development",
                    "B. Kiểm tra xem máy tính có cài đặt phần mềm Photoshop chưa",
                    "C. Kiểm tra bảng Users trong SQL Server",
                    "D. Xóa hết các Controller đi tạo lại"
                },
                "A",
                "Để bảo mật trên máy chủ thật (Production), các dự án thường bọc app.UseSwagger() bên trong if (app.Environment.IsDevelopment()) để người ngoài không nhìn thấy danh sách API nội bộ."
            ),

            // =====================================================================
            // BÀI 5.2
            // =====================================================================
            Build(
                topicId,
                "5.2",
                "Thực hành Postman chuyên nghiệp: Quản lý Environment, Headers, Body JSON và đọc kết quả",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/tutorials/first-web-api#test-postcreate-with-postman",
                """
                • Tại sao đi làm thực tế vẫn phải dùng Postman bên cạnh Swagger?
                - Postman cho phép lưu lại các bộ kịch bản kiểm thử (Collections) để cả nhóm dùng chung, cài đặt biến môi trường ({{baseUrl}}, {{token}}), giả lập gửi File đính kèm lớn và tự động lưu Token sau khi đăng nhập.
                • 4 Tab quan trọng nhất trên Postman bạn phải nắm lòng:
                1. Tab Params: Nơi điền các cặp Key - Value nằm sau dấu ? trên URL (tương ứng với [FromQuery] trong C#).
                2. Tab Authorization: Chọn Bearer Token và dán chuỗi JWT Token vào để gọi các API yêu cầu đăng nhập.
                3. Tab Headers: Nơi chứa thông tin phụ trợ như Content-Type: application/json, Accept-Language: vi-VN.
                4. Tab Body: Khi test POST/PUT, luôn chọn mục raw và đổi kiểu chữ ở mũi tên bên phải từ Text sang JSON (tương ứng với [FromBody] trong C#).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Chọn Method & Thiết lập biến {{baseUrl}}",
                        "Chọn POST và dùng biến môi trường {{baseUrl}}/api/orders để dễ dàng chuyển đổi giữa máy cá nhân (localhost) và server test.",
                        "POST {{baseUrl}}/api/orders"),
                    Step(2,
                        "Chọn Body -> raw -> JSON",
                        "Lỗi phổ biến nhất của Intern là chọn raw nhưng quên đổi Text sang JSON khiến API báo lỗi 415 Unsupported Media Type!",
                        "// Tab Body -> chọn chấm tròn 'raw' -> chọn 'JSON' màu xanh dương:\n{\n  \"customerId\": 5,\n  \"note\": \"Giao giờ hành chính\"\n}"),
                    Step(3,
                        "Bấm Send và soi Status + Time",
                        "Nhìn góc phải dưới của Postman: Status (200 OK), Time (thời gian chạy bao nhiêu ms - nếu > 2000ms là API đang bị chậm).",
                        "// Status: 200 OK | Time: 45 ms | Size: 1.2 KB")
                },
                "Khi dùng Postman để gọi một hàm POST có tham số [FromBody] CreateOrderDto dto, bạn đã gõ đúng cấu trúc JSON vào mục Body -> raw, nhưng khi bấm Send thì Server trả về ngay mã lỗi 415 Unsupported Media Type. Nguyên nhân là gì?",
                new List<string>
                {
                    "A. Do bạn để định dạng ở menu thả xuống cạnh nút 'raw' là 'Text' thay vì chọn 'JSON' (thiếu Header Content-Type: application/json)",
                    "B. Do SQL Server bị đầy ổ cứng",
                    "C. Do sai mật khẩu đăng nhập",
                    "D. Do máy tính mất kết nối mạng Internet"
                },
                "A",
                "Mã lỗi HTTP 415 Unsupported Media Type nghĩa là Server chỉ nhận định dạng application/json, nhưng Postman đang gửi lên dưới dạng văn bản thuần text/plain do bạn quên chuyển tùy chọn từ Text sang JSON."
            ),

            // =====================================================================
            // BÀI 5.3
            // =====================================================================
            Build(
                topicId,
                "5.3",
                "Đọc hiểu bảng Mã trạng thái HTTP (HTTP Status Codes) để bắt bệnh API trong 3 giây",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/api/microsoft.aspnetcore.http.statuscodes",
                """
                • Bí quyết chẩn đoán lỗi khi đi thực tập: Khi Tester hoặc bạn làm Frontend báo "API bị lỗi rồi!", câu đầu tiên bạn cần hỏi là: "Mã lỗi HTTP (Status Code) là số mấy?". Chỉ cần nghe con số là biết ngay lỗi do ai:
                - Đầu 2xx (Thành công):
                + 200 OK: Thành công (thường trả kèm dữ liệu JSON).
                + 201 Created: Tạo mới bản ghi thành công.
                + 204 No Content: Thành công nhưng không cần trả về dữ liệu gì (thường dùng cho DELETE hoặc PUT).
                - Đầu 4xx (Lỗi do phía Client / Postman gửi lên sai):
                + 400 Bad Request: Dữ liệu gửi lên bị thiếu trường [Required], sai định dạng, hoặc vi phạm quy tắc nghiệp vụ.
                + 401 Unauthorized: Chưa đăng nhập (Quên gửi JWT Token hoặc Token đã hết hạn).
                + 403 Forbidden: Đã đăng nhập nhưng KHÔNG ĐỦ QUYỀN (VD: Tài khoản nhân viên thường cố tình gọi vào API chỉ dành cho Admin).
                + 404 Not Found: Gõ sai đường dẫn URL hoặc không tìm thấy bản ghi có Id đó trong Database.
                + 405 Method Not Allowed: Gọi sai phương thức (VD: Hàm trong C# là [HttpPost] nhưng trên Postman lại chọn GET).
                - Đầu 5xx (Lỗi do Code C# Backend bị sập):
                + 500 Internal Server Error: Code C# của bạn bị văng Exception (như NullReferenceException, lỗi câu lệnh SQL, quên đăng ký DI trong Program.cs).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nếu gặp lỗi 401 hoặc 403 (Bảo mật)",
                        "401: Kiểm tra lại xem đã gắn Bearer Token vào Header chưa. 403: Kiểm tra Role của tài khoản trong Token có khớp với [Authorize(Roles = \"...\")] không.",
                        "[Authorize(Roles = \"Admin\")] // Nếu Token là Role \"Customer\" -> Bị chặn trả về 403!"),
                    Step(2,
                        "Nếu gặp lỗi 404 hoặc 405 (Định tuyến)",
                        "404: Soi lại chính tả URL trên Postman và [Route] trên Controller. 405: Soi lại xem đang chọn GET hay POST.",
                        "[HttpPost(\"checkout\")] // Nếu gọi bằng GET /api/orders/checkout -> Báo lỗi 405!"),
                    Step(3,
                        "Nếu gặp lỗi 500 (Sập code C#)",
                        "Mở ngay cửa sổ Output/Terminal của Visual Studio đọc dòng StackTrace để xem lỗi văng ra ở file .cs nào, dòng số mấy!",
                        "// System.NullReferenceException at OrderService.cs:line 48")
                },
                "Khi test API xóa sản phẩm (DELETE /api/products/10), bạn đã đính kèm đúng JWT Token của tài khoản vừa đăng nhập còn hạn sử dụng, nhưng API trả về mã lỗi 403 Forbidden. Nguyên nhân chính xác là gì?",
                new List<string>
                {
                    "A. Hệ thống đã nhận diện được bạn là ai (Token hợp lệ), nhưng tài khoản của bạn không có Quyền (Role/Permission) được phép thực hiện chức năng xóa này",
                    "B. Bạn chưa đăng nhập vào hệ thống",
                    "C. Sản phẩm có ID = 10 không tồn tại trong Database",
                    "D. Server SQL bị tắt"
                },
                "A",
                "Phân biệt 401 và 403 là câu hỏi phỏng vấn kinh điển: 401 Unauthorized là chưa xác thực danh tính (chưa có Token hợp lệ), còn 403 Forbidden là đã xác thực danh tính thành công nhưng không có quyền truy cập tài nguyên đó."
            ),

            // =====================================================================
            // BÀI 5.4
            // =====================================================================
            Build(
                topicId,
                "5.4",
                "Đọc luồng Đăng nhập & Cấp phát thẻ bài JWT (JSON Web Token)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authentication/",
                """
                • Bản chất của xác thực JWT trong Web API hiện đại:
                - API không lưu trạng thái phiên đăng nhập trên RAM máy chủ (Stateless). Thay vào đó, khi người dùng gửi Username + Password vào API POST /api/auth/login, nếu đúng mật khẩu, Backend sẽ in cho người dùng một "Thẻ căn cước điện tử" mã hóa thành chuỗi ký tự gọi là JWT (JSON Web Token).
                • Cấu trúc 3 phần của 1 chuỗi JWT (Ngăn cách nhau bởi 2 dấu chấm xxxxx.yyyyy.zzzzz):
                1. Header (xxxxx): Chứa thuật toán ký bảo mật (thường là HS256).
                2. Payload (yyyyy): Chứa các Claims (Thông tin đính kèm của người dùng) như UserId (ClaimTypes.NameIdentifier), Email, FullName và Role (Admin, Staff) cùng thời gian hết hạn exp. (Lưu ý: Phần này ai cũng giải mã đọc được trên trang jwt.io, nên tuyệt đối không nhét mật khẩu vào Claims!).
                3. Signature (zzzzz): Chữ ký số được tạo bằng khóa bí mật (SecretKey lưu trong appsettings.json) để chống kẻ gian tự ý sửa Role từ Member thành Admin.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Kiểm tra tài khoản & mật khẩu",
                        "Tìm User theo Email/Username và dùng hàm băm (BCrypt / PasswordHasher) kiểm tra mật khẩu.",
                        "var user = await _userRepo.GetByEmailAsync(dto.Email);\nif (user == null || !VerifyPassword(dto.Password, user.PasswordHash))\n    return Unauthorized(\"Sai tài khoản hoặc mật khẩu!\");"),
                    Step(2,
                        "Đóng gói thông tin vào danh sách Claims",
                        "Ghi các thông tin định danh (ID, Email, Quyền) vào bên trong ruột của thẻ Token.",
                        "var claims = new List<Claim> {\n    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),\n    new Claim(ClaimTypes.Email, user.Email),\n    new Claim(ClaimTypes.Role, user.RoleName)\n};"),
                    Step(3,
                        "Ký đóng dấu bằng SecretKey & Hẹn giờ hết hạn",
                        "Đọc khóa bí mật từ appsettings.json để đóng dấu chống giả mạo và đặt hạn dùng (VD: 2 tiếng).",
                        "var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[\"Jwt:SecretKey\"]));\nvar creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);\nvar token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: creds);"),
                    Step(4,
                        "Xuất chuỗi Token trả về cho Client",
                        "Biến đối tượng Token thành chuỗi văn bản eyJhbGci... để Client lưu lại dùng cho các lần gọi API sau.",
                        "string jwtString = new JwtSecurityTokenHandler().WriteToken(token);\nreturn Ok(new { accessToken = jwtString });")
                },
                "Trong luồng tạo JWT Token khi đăng nhập, danh sách List<Claim> (thuộc phần Payload của JWT) dùng để lưu trữ những thông tin gì?",
                new List<string>
                {
                    "A. Lưu mật khẩu gốc của người dùng và chuỗi kết nối SQL Server",
                    "B. Lưu các thông tin định danh không nhạy cảm của người dùng (như UserId, Email, Username, Role) để ở các Request tiếp theo Backend đọc ra biết ngay ai đang gọi API mà không cần truy vấn lại Database",
                    "C. Lưu toàn bộ hình ảnh sản phẩm của trang web",
                    "D. Lưu mã nguồn giao diện HTML"
                },
                "B",
                "Các Claim trong JWT giúp đính kèm định danh (UserId) và quyền hạn (Role) vào ngay trong thẻ Token. Khi người dùng gửi kèm Token lên ở các API sau, Server chỉ cần giải mã Token là lấy được ngay UserId đang thao tác."
            ),

            // =====================================================================
            // BÀI 5.5
            // =====================================================================
            Build(
                topicId,
                "5.5",
                "Đọc luồng Bảo vệ API với [Authorize], Phân quyền Roles và Lấy UserId từ Token hiện tại",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authorization/roles",
                """
                • Sau khi Client đã có JWT Token, luồng kiểm tra và lấy thông tin người dùng trong Controller diễn ra như thế nào?
                1. Gắn ổ khóa [Authorize]:
                - Đặt [Authorize] trên đầu Class Controller (khóa toàn bộ các hàm trong file đó) hoặc trên đầu từng hàm lẻ.
                - Nếu có 1 hàm trong Controller đã khóa lại muốn mở tự do cho khách vãng lai xem (như hàm Xem danh sách sản phẩm hoặc hàm Login), chỉ cần gắn thẻ [AllowAnonymous] lên hàm đó.
                2. Phân quyền theo Vai trò ([Authorize(Roles = "Admin,Manager")]):
                - Chỉ những ai có ClaimTypes.Role trong Token khớp với chữ "Admin" hoặc "Manager" mới được đi qua, ngược lại nhận lỗi 403 Forbidden.
                3. Cách đọc code lấy UserId của người đang đăng nhập:
                - Khi làm chức năng "Xem lịch sử đơn hàng của tôi" hoặc "Tạo bài viết mới", không bao giờ bắt Client truyền UserId trong Body JSON (vì người ta có thể truyền giả ID của người khác!).
                - Thay vào đó, Backend tự móc UserId từ trong chiếc thẻ JWT Token của người đang gọi thông qua thuộc tính User.FindFirstValue(ClaimTypes.NameIdentifier) (hoặc thông qua dịch vụ ICurrentUserService).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Client gửi Header Authorization",
                        "Postman/Frontend đính kèm chuỗi Token vào Header với từ khóa 'Bearer ' đứng đằng trước.",
                        "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."),
                    Step(2,
                        "Kiểm tra thẻ [Authorize] trên Controller",
                        "Middleware xác thực chữ ký Token. Nếu hợp lệ và đúng Role mới cho phép chạy vào hàm.",
                        "[Authorize(Roles = \"Customer,Admin\")]\n[HttpPost(\"my-orders\")]\npublic async Task<IActionResult> CreateMyOrder(CreateOrderDto dto)"),
                    Step(3,
                        "Trích xuất UserId an toàn từ HttpContext.User",
                        "Đọc trực tiếp mã UserId từ Claim bên trong Token đã giải mã để gán làm chủ sở hữu đơn hàng.",
                        "{\n    string? userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);\n    int currentUserId = int.Parse(userIdStr!);\n    await _orderService.CreateAsync(currentUserId, dto);\n    return Ok();\n}")
                },
                "Trên đầu class ProductsController có gắn thẻ [Authorize] (yêu cầu đăng nhập cho toàn bộ Controller). Tuy nhiên, sếp yêu cầu riêng hàm GetAllProducts() phải cho phép khách chưa đăng nhập cũng xem được hàng hóa. Bạn cần gắn thêm thẻ gì lên trên hàm GetAllProducts()?",
                new List<string>
                {
                    "A. Gắn thẻ [AllowAnonymous]",
                    "B. Gắn thẻ [HttpGet]",
                    "C. Gắn thẻ [Required]",
                    "D. Gắn thẻ [FromBody]"
                },
                "A",
                "Thuộc tính [AllowAnonymous] cho phép bỏ qua rào chắn xác thực [Authorize] đối với riêng Action cụ thể đó, giúp khách vãng lai truy cập tự do."
            ),

            // =====================================================================
            // BÀI 5.6
            // =====================================================================
            Build(
                topicId,
                "5.6",
                "Đọc hiểu Cấu hình môi trường (appsettings.json, IConfiguration & Options Pattern)",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/configuration/",
                """
                • Quy tắc vàng trong doanh nghiệp: Tuyệt đối KHÔNG viết cứng (Hard-code) các thông số nhạy cảm hoặc dễ thay đổi (như Chuỗi kết nối Database, Khóa bí mật JWT, Mật khẩu gửi Email, Đường dẫn lưu file) vào trong file code .cs. Tất cả phải được cất vào file appsettings.json (và appsettings.Development.json).
                • 2 cách đọc dữ liệu từ appsettings.json vào code C# bạn sẽ gặp:
                1. Cách 1 - Dùng IConfiguration trực tiếp: Tiêm IConfiguration _config vào Constructor và đọc bằng dấu hai chấm ::
                string secret = _config["JwtSettings:SecretKey"]; (Dấu : dùng để đi sâu vào từng cấp ngoặc nhọn {} trong file JSON).
                2. Cách 2 - Dùng IOptions<T> (Options Pattern - Rất phổ biến ở công ty): Tạo một class JwtSettings.cs có các thuộc tính trùng tên với mục "JwtSettings" trong JSON, ánh xạ trong Program.cs bằng builder.Services.Configure<JwtSettings>(...), rồi tiêm IOptions<JwtSettings> options vào Service để chấm gọi cực kỳ an toàn, không sợ gõ sai chính tả chuỗi!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khai báo cấu hình phân cấp trong appsettings.json",
                        "Tổ chức các thông số cài đặt thành từng cụm đối tượng JSON rõ ràng.",
                        "{\n  \"MailSettings\": {\n    \"Host\": \"smtp.gmail.com\",\n    \"Port\": 587\n  }\n}"),
                    Step(2,
                        "Ánh xạ vào Class C# trong Program.cs",
                        "Kết nối mục \"MailSettings\" trong JSON vào class MailSettings của C#.",
                        "builder.Services.Configure<MailSettings>(\n    builder.Configuration.GetSection(\"MailSettings\"));"),
                    Step(3,
                        "Đọc giá trị qua IOptions<T>.Value trong Service",
                        "Lấy thông số ra sử dụng thông qua thuộc tính .Value hoàn toàn tự động.",
                        "public class EmailService {\n    private readonly MailSettings _mailSettings;\n    public EmailService(IOptions<MailSettings> options) {\n        _mailSettings = options.Value; // .Host = \"smtp.gmail.com\", .Port = 587\n    }\n}")
                },
                "Trong file appsettings.json có cấu trúc: \"PaymentConfig\": { \"VnPay\": { \"TmnCode\": \"ABC12345\" } }. Nếu dùng biến IConfiguration _config để lấy giá trị \"ABC12345\" ra, bạn phải viết chuỗi khóa truy cập như thế nào?",
                new List<string>
                {
                    "A. _config[\"PaymentConfig:VnPay:TmnCode\"] (Dùng dấu hai chấm : để ngăn cách các cấp)",
                    "B. _config[\"PaymentConfig/VnPay/TmnCode\"]",
                    "C. _config[\"TmnCode\"]",
                    "D. _config[\"PaymentConfig-VnPay-TmnCode\"]"
                },
                "A",
                "Trong hệ thống IConfiguration của .NET, dấu hai chấm (:) là ký hiệu chuẩn để truy cập sâu vào các đối tượng JSON lồng nhau trong appsettings.json."
            ),

            // =====================================================================
            // BÀI 5.7
            // =====================================================================
            Build(
                topicId,
                "5.7",
                "Đọc luồng Ghi nhận Nhật ký hệ thống (Logging với ILogger & Serilog) để tìm lỗi thực tế",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/logging/",
                """
                • Làm sao tìm lỗi khi dự án đã chạy trên Server (không thể bật Visual Studio đặt Breakpoint F9)?
                Khi ứng dụng đã đưa lên Server thử nghiệm của công ty, nếu khách hàng thao tác bị lỗi, lập trình viên sẽ mở File Nhật ký (Log file) ra đọc. Đó là lý do mọi file Service hay Controller ở công ty đều tiêm công cụ ILogger<Tên_Class> _logger.
                • Cách đọc 4 cấp độ Log trong code dự án:
                1. _logger.LogInformation(...): Ghi lại các cột mốc hoạt động bình thường (VD: "Đơn hàng #102 vừa được tạo bởi User 5").
                2. _logger.LogWarning(...): Cảnh báo điều bất thường nhưng chưa làm sập hàm (VD: "User 5 nhập sai mật khẩu lần thứ 3").
                3. _logger.LogError(ex, ...): Quan trọng nhất! Ghi lại toàn bộ chi tiết lỗi Exception và dòng code bị sập khi rơi vào khối catch.
                4. _logger.LogCritical(...): Lỗi nghiêm trọng cấp hệ thống (mất kết nối Database, hết dung lượng ổ cứng).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Tiêm ILogger<T> vào Constructor",
                        "Hệ thống .NET cung cấp sẵn ILogger mà không cần cài thêm thư viện ngoài.",
                        "private readonly ILogger<OrderService> _logger;\npublic OrderService(ILogger<OrderService> logger) {\n    _logger = logger;\n}"),
                    Step(2,
                        "Ghi Log Information theo dõi luồng chạy",
                        "Dùng cú pháp giữ chỗ {OrderId} (Structured Logging) giúp tìm kiếm log cực nhanh.",
                        "_logger.LogInformation(\"Bắt đầu xử lý thanh toán cho đơn hàng {OrderId}\", order.Id);"),
                    Step(3,
                        "Ghi Log Error kèm đối tượng Exception khi lỗi",
                        "Truyền biến 'ex' vào tham số đầu tiên để in ra chính xác dòng code gây lỗi.",
                        "catch (Exception ex) {\n    _logger.LogError(ex, \"Lỗi thanh toán đơn hàng {OrderId}\", order.Id);\n    throw;\n}")
                },
                "Khi ghi log lỗi trong khối catch (Exception ex), tại sao các anh Senior luôn viết _logger.LogError(ex, \"Lỗi khi tạo đơn {OrderId}\", dto.Id); (truyền biến ex vào tham số đầu tiên) thay vì chỉ ghi mỗi dòng chữ thông báo?",
                new List<string>
                {
                    "A. Để hệ thống in ra đầy đủ StackTrace (tên file .cs và số thứ tự dòng code cụ thể gây ra lỗi) cùng thông điệp gốc của Exception vào file Log, giúp tìm ra nguyên nhân lỗi ngay lập tức",
                    "B. Để tự động sửa lỗi đó mà không cần lập trình viên",
                    "C. Để gửi email cho khách hàng",
                    "D. Vì nếu không truyền biến ex thì code không biên dịch được"
                },
                "A",
                "Truyền đối tượng Exception ex vào LogError giúp lưu lại toàn bộ dấu vết ngăn xếp (StackTrace), chỉ rõ lỗi phát sinh ở file nào và dòng số mấy."
            ),

            // =====================================================================
            // BÀI 5.8
            // =====================================================================
            Build(
                topicId,
                "5.8",
                "Đọc luồng Upload File / Hình ảnh (IFormFile) và Lưu đường dẫn vào Database",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/models/file-uploads",
                """
                • Một tính năng cực kỳ phổ biến trong cả Bài tập lớn trên trường lẫn khi đi thực tập: Upload ảnh đại diện (Avatar) hoặc ảnh sản phẩm!
                • Nguyên tắc vàng khi xử lý File/Ảnh trong Web API & MVC:
                - Tuyệt đối KHÔNG lưu thẳng cục file ảnh nặng hàng Megabyte vào cột trong SQL Server (vì làm phình to và chậm Database khủng khiếp).
                - Luồng chuẩn 3 bước của mọi công ty:
                1. Nhận file từ Client gửi lên qua kiểu dữ liệu IFormFile (với nhãn [FromForm]).
                2. Lưu file ảnh vật lý đó vào thư mục wwwroot/uploads/ trên ổ cứng Server (hoặc đẩy lên đám mây Cloudinary / AWS S3) và đổi tên file kèm Guid.NewGuid() để không bị trùng tên.
                3. Chỉ lấy Chuỗi đường dẫn (URL/Path) của bức ảnh đó (VD: "/uploads/a8f9-laptop.jpg") lưu vào cột ImageUrl (kiểu string) trong bảng SQL Server!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nhận file qua IFormFile với [FromForm]",
                        "Kiểm tra dung lượng file (> 0) và đuôi file hợp lệ (.jpg, .png) trước khi lưu.",
                        "[HttpPost(\"upload-avatar\")]\npublic async Task<IActionResult> Upload([FromForm] IFormFile file) {\n    if (file == null || file.Length == 0) return BadRequest(\"Chưa chọn file!\");"),
                    Step(2,
                        "Tạo tên file duy nhất & Copy vào thư mục wwwroot",
                        "Ghép Guid vào tên file để 2 người cùng up ảnh tên 'avatar.jpg' không bị đè mất ảnh của nhau.",
                        "    string fileName = $\"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}\";\n    string savePath = Path.Combine(_env.WebRootPath, \"uploads\", fileName);\n    using (var stream = new FileStream(savePath, FileMode.Create)) {\n        await file.CopyToAsync(stream);\n    }"),
                    Step(3,
                        "Lưu chuỗi đường dẫn vào cột kiểu string trong DB",
                        "Chỉ lưu chuỗi đường dẫn nhẹ nhàng vào Database để Frontend gắn vào thẻ <img src=\"...\" />.",
                        "    user.AvatarUrl = $\"/uploads/{fileName}\";\n    await _db.SaveChangesAsync();\n    return Ok(new { avatarUrl = user.AvatarUrl });\n}")
                },
                "Trong các dự án thực tế, khi làm chức năng thêm mới sản phẩm có kèm hình ảnh, dữ liệu thực sự được lưu vào cột ImageUrl của bảng Products trong SQL Server là gì?",
                new List<string>
                {
                    "A. Chỉ lưu chuỗi đường dẫn (Path / URL kiểu string, ví dụ '/uploads/sp01.jpg'), còn file ảnh gốc đã được lưu ở thư mục lưu trữ trên Server hoặc dịch vụ Cloud",
                    "B. Lưu toàn bộ điểm ảnh màu sắc của bức ảnh vào cột kiểu int",
                    "C. Không lưu gì vào Database cả",
                    "D. Lưu tên phần mềm chụp bức ảnh đó"
                },
                "A",
                "Lưu file vật lý ở ổ cứng/Cloud Storage và chỉ lưu đường dẫn (string) vào Database giúp cơ sở dữ liệu luôn gọn nhẹ, truy vấn nhanh và hiển thị ảnh lên thẻ <img> dễ dàng."
            ),

            // =====================================================================
            // BÀI 5.9
            // =====================================================================
            Build(
                topicId,
                "5.9",
                "Quy trình Git thực chiến khi đi thực tập: Clone -> Branch -> Pull -> Commit -> Push -> Pull Request",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/gitworkflow",
                """
                • Đi thực tập ngày đầu tiên, anh Leader giao cho bạn 1 tính năng nhỏ, bạn phải thao tác Git theo đúng luồng nào để không làm hỏng code của cả công ty?
                • Quy trình 6 bước chuẩn mực tại doanh nghiệp (Git Flow cơ bản):
                1. Cấm kỵ số 1: Tuyệt đối KHÔNG BAO GIỜ code trực tiếp hoặc git push thẳng lên nhánh main / master / develop của công ty!
                2. Bước 1 (git checkout develop & git pull): Kéo code mới nhất của cả nhóm về máy mình trước.
                3. Bước 2 (git checkout -b feature/ten-tinh-nang): Tự tách ra một nhánh riêng (Branch) của bạn (VD: feature/create-category-api) để thoải mái code trên đó.
                4. Bước 3 (Code & Test kỹ bằng Swagger/Postman): Đảm bảo code chạy ngon, không lỗi biên dịch (dotnet build).
                5. Bước 4 (git add . ➔ git commit -m "..." ➔ git push origin feature/...): Đẩy nhánh riêng của bạn lên GitHub/GitLab.
                6. Bước 5 (Tạo Pull Request - PR / Merge Request - MR): Lên giao diện web GitHub/GitLab bấm tạo Pull Request xin gộp từ nhánh của bạn vào nhánh develop để anh Leader kiểm tra (Review Code) và bấm duyệt!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Cập nhật code mới nhất & Tách nhánh riêng",
                        "Luôn lấy code mới nhất từ nhánh chung về rồi mới tạo nhánh tính năng riêng cho mình.",
                        "git checkout develop\ngit pull origin develop\ngit checkout -b feature/add-product-search"),
                    Step(2,
                        "Viết code C#, kiểm tra thay đổi (git status)",
                        "Sau khi code và test xong API trên máy, kiểm tra xem mình đã sửa những file nào.",
                        "git status\ngit add .\ngit commit -m \"feat: add search product by keyword API\""),
                    Step(3,
                        "Đẩy nhánh cá nhân lên kho chứa từ xa (Push)",
                        "Đẩy đúng nhánh feature của mình lên GitHub/GitLab.",
                        "git push -u origin feature/add-product-search"),
                    Step(4,
                        "Tạo Pull Request (PR) chờ Mentor Review",
                        "Mở GitHub tạo PR vào nhánh develop. Nếu Mentor comment yêu cầu sửa, bạn sửa trên máy rồi commit + push tiếp là PR tự cập nhật!",
                        "// Tạo Pull Request: feature/add-product-search ➔ develop")
                },
                "Khi đi thực tập và được giao làm tính năng \"Thêm API lấy danh sách danh mục\", hành động nào dưới đây là ĐÚNG CHUẨN quy trình làm việc nhóm tại công ty?",
                new List<string>
                {
                    "A. Code thẳng vào nhánh main rồi gõ git push --force",
                    "B. Kéo code mới nhất về, tách một nhánh mới riêng (ví dụ feature/get-categories), code và test xong thì push nhánh đó lên và tạo Pull Request để anh Leader review trước khi gộp vào nhánh chung",
                    "C. Copy file code gửi qua Zalo cho sếp",
                    "D. Xóa nhánh của người khác đi để đẩy code của mình lên"
                },
                "B",
                "Làm việc trên nhánh tính năng riêng (feature branch) và hợp nhất qua Pull Request giúp bảo vệ nhánh chính luôn ổn định và tạo cơ hội để Senior review, góp ý code cho bạn."
            ),

            // =====================================================================
            // BÀI 5.10
            // =====================================================================
            Build(
                topicId,
                "5.10",
                "Tổng kết Lộ trình Thực tập: Bí kíp trả lời phỏng vấn & Cách tự làm 1 Task mới từ con số 0 khi vào công ty",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/web-api/",
                """
                • Chúc mừng Hoàng Việt đã đi đến bài cuối cùng của Lộ trình Thực tập!
                • Khi vào công ty thực tập và được giao 1 Task mới (VD: "Hãy viết API Thêm mới và Lấy danh sách Khuyến mãi - Voucher"), bạn hãy mở đúng 1 chức năng tương tự có sẵn trong dự án (như Product hoặc Category) làm mẫu và làm theo đúng 6 bước tuần tự từ dưới lên trên (Bottom-Up):
                1. Bước 1 (Tầng Entity & DB): Xem hoặc tạo class Voucher.cs trong thư mục Entities, khai báo DbSet<Voucher> trong DbContext (nếu cần tạo bảng mới thì chạy Migration).
                2. Bước 2 (Tầng DTO): Tạo VoucherDto.cs và CreateVoucherDto.cs (gắn [Required]).
                3. Bước 3 (Tầng Repository): Tạo IVoucherRepository và VoucherRepository kế thừa từ BaseRepository<Voucher>.
                4. Bước 4 (Tầng Service): Tạo IVoucherService và VoucherService (tiêm IVoucherRepository vào Constructor, viết hàm kiểm tra trùng mã và lưu dữ liệu).
                5. Bước 5 (Đăng ký DI trong Program.cs): Thêm builder.Services.AddScoped<IVoucherService, VoucherService>(); (và Repository).
                6. Bước 6 (Tầng Controller & Test Swagger): Tạo VouchersController : ControllerBase, tiêm IVoucherService vào, viết hàm [HttpGet], [HttpPost] rồi bật Swagger lên bấm Execute kiểm tra thành quả!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Mở 1 luồng mẫu có sẵn trong dự án",
                        "Không bao giờ ngồi tự nghĩ từ trang giấy trắng! Hãy mở 1 cặp Controller -> Service -> Repository có sẵn của công ty để xem chuẩn đặt tên và cấu trúc.",
                        "// Xem mẫu: CategoriesController -> CategoryService -> CategoryRepository"),
                    Step(2,
                        "2. Xây từ trong ra ngoài (Entity -> DTO -> Repo -> Service)",
                        "Chuẩn bị dữ liệu và tầng xử lý nghiệp vụ trước.",
                        "// 1. Tạo Entity & DTO\n// 2. Viết hàm trong Repository & Service"),
                    Step(3,
                        "3. Đăng ký DI tại Program.cs & Mở cổng Controller",
                        "Khai báo AddScoped trong Program.cs và tạo API Endpoint tại Controller.",
                        "builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();\nbuilder.Services.AddScoped<IVoucherService, VoucherService>();"),
                    Step(4,
                        "4. Đặt Breakpoint (F9) & Bắn thử trên Swagger",
                        "Chạy Debug qua từng bước một để chắc chắn dữ liệu chảy từ Swagger xuống tới bảng SQL Server trơn tru 100% trước khi tạo Pull Request!",
                        "// Bấm Execute trên Swagger -> Kiểm tra HTTP 200 OK và dữ liệu trong SQL Server")
                },
                "Khi vào công ty thực tập, bạn được giao viết một API mới hoàn toàn. Bạn đã viết xong VoucherController, IVoucherService, VoucherService, IVoucherRepository, VoucherRepository rất đầy đủ, không có lỗi đỏ nào. Nhưng vừa bấm gửi Request trên Swagger thì bị lỗi 500: \"InvalidOperationException: Unable to resolve service for type 'IVoucherService' while attempting to activate 'VoucherController'\". Bạn đã quên bước nào trong 6 bước?",
                new List<string>
                {
                    "A. Quên bước 5: Chưa đăng ký cặp IVoucherService và VoucherService bằng lệnh builder.Services.AddScoped<IVoucherService, VoucherService>(); trong file Program.cs",
                    "B. Quên cài đặt Postman",
                    "C. Quên đặt tên biến bằng tiếng Việt",
                    "D. Quên xóa bảng trong Database"
                },
                "A",
                "Đây là lỗi kinh điển nhất mà 100% lập trình viên .NET đều từng gặp khi tạo Service/Repository mới: viết xong class và interface nhưng quên khai báo vào DI Container trong Program.cs."
            )
        };
    }

    /// <summary>
    /// Thay thế các bài mẫu tạm thời và nạp/cập nhật đầy đủ 10 bài học con (5.1 -> 5.10) cho Chủ đề 5 (Menu 1: INTERN_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var internTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "INTERN_TRACK");

        if (internTrack == null)
            return;

        var topic5 = internTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 5);
        if (topic5 == null)
            return;

        topic5.Title = "Chủ đề 5 (Thực tập): Thực Hành Kiểm Thử API (Swagger, Postman), Đọc Luồng Xác Thực JWT & Kỹ Năng Đi Làm Thực Tế";
        topic5.Description = "10 Bài học con thực chiến về Swagger UI, Postman, HTTP Status Codes, JWT Authentication, [Authorize] & Roles, appsettings.json, ILogger, IFormFile Upload, Git Flow & 6 bước hoàn thành Task mới.";

        var targetLessons = GetLessons(topic5.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic5.Id)
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
