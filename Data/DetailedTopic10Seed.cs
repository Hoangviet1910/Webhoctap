using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic10Seed
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
            // BÀI 10.1
            // =====================================================================
            Build(
                topicId,
                "10.1",
                "Phân biệt Authentication vs Authorization & Cấu trúc Identity (Claims, ClaimsIdentity, ClaimsPrincipal)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/",
                """
                • **💡 1. Hiểu bản chất của `Authentication` và `Authorization` trong 30 giây:**
                  - Hãy tưởng tượng bạn đi vào một tòa nhà văn phòng cao cấp:
                    1. **`Authentication` (Xác thực danh tính — Trả lời câu hỏi *"Bạn là ai?"*):** Bạn quẹt thẻ nhân viên ở cổng lễ tân để chứng minh *"Tôi là Nguyễn Hoàng Việt, nhân viên phòng IT"*. Nếu thẻ giả hoặc quên mang thẻ ➔ Bị chặn ngay ở cửa kèm mã lỗi **`401 Unauthorized`**!
                    2. **`Authorization` (Phân quyền truy cập — Trả lời câu hỏi *"Bạn được phép làm gì?"*):** Sau khi qua cổng lễ tân, bạn đi lên phòng Két sắt Tài chính của Giám đốc. Dù hệ thống biết rõ bạn là Hoàng Việt rồi, nhưng thẻ của bạn không có quyền mở phòng Giám đốc ➔ Bị chặn lại kèm mã lỗi **`403 Forbidden`**!
                • **🖼️ 2. Sơ đồ Giải Phẫu Đối Tượng `HttpContext.User` (`ClaimsPrincipal`) Trong ASP.NET Core:**
                ```
                Khi người dùng đăng nhập thành công, ASP.NET Core tạo ra hồ sơ danh tính gắn vào biến HttpContext.User:
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 👤 ClaimsPrincipal (HttpContext.User - Con người thực tế đang gọi API)     │
                │   └── 🪪 ClaimsIdentity (Ví giấy tờ: Đăng nhập bằng JWT hay Cookie?)       │
                │         ├── 📄 Claim 1: Type = "NameIdentifier" │ Value = "105" (UserId)   │
                │         ├── 📄 Claim 2: Type = "Email"          │ Value = "viet@uneti.vn"  │
                │         ├── 📄 Claim 3: Type = "Role"           │ Value = "Admin"          │
                │         └── 📄 Claim 4: Type = "Department"     │ Value = "IT_Software"    │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng So Sánh 3 Khái Niệm Cốt Lõi Của Hệ Thống Danh Tính .NET:**
                | Thành phần | Hình ảnh ẩn dụ | Vai trò kỹ thuật trong C# |
                | :--- | :--- | :--- |
                | **`Claim` (Thông tin đơn lẻ)** | 1 dòng thông tin in trên thẻ (Họ tên, Ngày sinh, Chức vụ). | Là cặp `Key - Value` (`ClaimType` và `ClaimValue`), ví dụ: `new Claim(ClaimTypes.Role, "Admin")`. |
                | **`ClaimsIdentity` (Thẻ căn cước)**| Chiếc thẻ nhựa chứa tập hợp các dòng thông tin (`Claims`). | Chứa danh sách `List<Claim>` và trạng thái `IsAuthenticated` (`true/false`). |
                | **`ClaimsPrincipal` (Chủ thẻ)** | Chính là người dùng đang cầm chiếc ví chứa thẻ căn cước đó. | Đại diện bởi biến **`User`** trong mọi Controller (`User.Identity.IsAuthenticated`). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo danh sách các Claims mô tả thông tin người dùng",
                        "📥 Đầu vào: Đối tượng User vừa kiểm tra đúng mật khẩu từ Database. ⚙️ Cơ chế ngầm: Đóng gói Id, Email, FullName và Role của User thành các đối tượng Claim. 📤 Đầu ra: Danh sách List<Claim>.",
                        "var claims = new List<Claim> {\n    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),\n    new Claim(ClaimTypes.Name, user.FullName),\n    new Claim(ClaimTypes.Email, user.Email),\n    new Claim(ClaimTypes.Role, \"Librarian\") // Quyền Thủ thư\n};"),
                    Step(2,
                        "2. Đóng gói vào ClaimsIdentity và ClaimsPrincipal",
                        "📥 Đầu vào: Danh sách claims ở Bước 1. ⚙️ Cơ chế ngầm: Tạo chiếc thẻ ClaimsIdentity (kèm tên phương thức xác thực) rồi đưa vào ví ClaimsPrincipal. 📤 Đầu ra: Đối tượng danh tính hoàn chỉnh.",
                        "var identity = new ClaimsIdentity(claims, \"AuthenticationType_CookieOrJwt\");\nvar principal = new ClaimsPrincipal(identity);"),
                    Step(3,
                        "3. Đọc thông tin người dùng đang đăng nhập tại Controller",
                        "📥 Đầu vào: Biến User (kiểu ClaimsPrincipal) có sẵn trong ControllerBase. ⚙️ Cơ chế ngầm: Dùng hàm FindFirstValue để rút trích chính xác mã UserId hoặc Email của người đang gọi Request. 📤 Đầu ra: Lấy được UserId an toàn tuyệt đối.",
                        "string? currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);\nstring? currentEmail = User.FindFirstValue(ClaimTypes.Email);\nbool isAdmin = User.IsInRole(\"Admin\");")
                },
                "Trong ASP.NET Core, sự khác biệt chính xác giữa mã trạng thái HTTP 401 Unauthorized và 403 Forbidden là gì?",
                new List<string>
                {
                    "A. 401 là lỗi Xác thực (Authentication - người dùng chưa đăng nhập hoặc Token không hợp lệ); còn 403 là lỗi Phân quyền (Authorization - người dùng đã đăng nhập hợp lệ nhưng không đủ quyền Role/Policy để thực hiện chức năng đó)",
                    "B. 401 là lỗi không tìm thấy trang, còn 403 là lỗi sập SQL Server",
                    "C. 403 là chưa đăng nhập, còn 401 là đã đăng nhập",
                    "D. Hai mã lỗi hoàn toàn giống nhau"
                },
                "A",
                "Nhớ quy tắc: Chưa biết bạn là ai (thiếu/sai Token) ➔ 401; Đã biết bạn là ai nhưng chức vụ của bạn không được phép vào ➔ 403."
            ),

            // =====================================================================
            // BÀI 10.2
            // =====================================================================
            Build(
                topicId,
                "10.2",
                "Bảo mật Mật khẩu: Tại sao CẤM lưu mật khẩu thô? Bản chất Hashing, Salting và BCrypt / PasswordHasher",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/data-protection/consumer-apis/password-hashing",
                """
                • **💡 1. Tội ác lớn nhất trong lập trình: Lưu mật khẩu dạng chữ thuần (`Plain Text`) vào Database!**
                  - Nếu bạn lưu thẳng mật khẩu `"123456"` của người dùng vào cột `Password` trong bảng `Users`: Khi Hacker (hoặc nhân viên nội bộ) xem được bảng `Users`, toàn bộ tài khoản của khách hàng bị lộ sạch 100%!
                  - **Giải pháp bắt buộc: Băm mật khẩu một chiều (`One-way Hashing`) kèm Muối ngẫu nhiên (`Salt`)!**
                • **🖼️ 2. Sơ đồ Phân Biệt Mã Hóa 2 Chiều (`Encryption`) vs Băm 1 Chiều (`Hashing + Salt`):**
                ```
                [1. Encryption (Mã hóa 2 chiều - Dùng cho dữ liệu cần dịch ngược lại):]
                "Xin chào" ──(Khóa K)──► "X9#a8@k" ──(Giải mã bằng Khóa K)──► "Xin chào"

                [2. Password Hashing + Salt (Băm 1 chiều - BẮT BUỘC dùng cho Mật khẩu!):]
                Mật khẩu: "MatKhau123" + Salt ngẫu nhiên ("$2a$11$xyz...")
                     │
                     ▼  (Qua thuật toán băm BCrypt / PBKDF2 -> KHÔNG BAO GIỜ dịch ngược lại được!)
                Lưu vào DB chuỗi PasswordHash: "$2a$11$xyz...u8K9pL2mN5vQ1rT4wX7yZ0"

                👉 Vậy lúc Đăng nhập kiểm tra kiểu gì?
                   Lấy mật khẩu khách vừa nhập ──► Băm lại với cùng hạt muối Salt đó ──► So sánh 2 chuỗi Hash!
                ```
                • **📊 3. Tại sao không dùng `MD5` hay `SHA256` đơn thuần mà phải dùng `PasswordHasher<T>` hoặc `BCrypt`?**
                | Thuật toán | Có tự động sinh Muối (`Salt`) riêng cho từng người không? | Mức độ an toàn hiện nay |
                | :--- | :--- | :--- |
                | **`MD5` / `SHA1` trần** | ❌ **KHÔNG!** (2 người cùng đặt mật khẩu `"123456"` sẽ ra 2 chuỗi MD5 giống hệt nhau ➔ Bị tra từ điển *Rainbow Table* trong 1 giây!). | ❌ **Tuyệt đối không dùng cho mật khẩu!** |
                | **`PasswordHasher<TUser>`** (Có sẵn trong ASP.NET Core) | ✅ **CÓ!** Tự động trộn hạt muối `Salt` ngẫu nhiên 128-bit (dùng thuật toán `PBKDF2`). 2 người cùng mật khẩu `"123456"` sẽ ra 2 chuỗi Hash hoàn toàn khác nhau! | ⭐ **An toàn chuẩn Microsoft (Không cần cài thêm gói ngoài).** |
                | **`BCrypt.Net-Next`** | ✅ **CÓ!** Tự động gộp cả `Salt` và độ khó (`Work Factor`) vào trong chuỗi băm 60 ký tự. | ⭐ **Rất phổ biến trong các dự án doanh nghiệp.** |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Băm mật khẩu khi Đăng ký tài khoản (HashPassword)",
                        "📥 Đầu vào: Mật khẩu gốc người dùng nhập (dto.Password). ⚙️ Cơ chế ngầm: PasswordHasher tự động sinh chuỗi Salt ngẫu nhiên, trộn với mật khẩu và băm qua 100.000 vòng lặp PBKDF2. 📤 Đầu ra: Chuỗi PasswordHash an toàn để lưu vào SQL Server.",
                        "var hasher = new PasswordHasher<User>();\nvar newUser = new User { Email = dto.Email };\n\n// Băm mật khẩu kèm Salt tự động:\nnewUser.PasswordHash = hasher.HashPassword(newUser, dto.Password);\nawait _db.Users.AddAsync(newUser);"),
                    Step(2,
                        "2. Kiểm tra mật khẩu khi Đăng nhập (VerifyHashedPassword)",
                        "📥 Đầu vào: Chuỗi PasswordHash đang lưu trong DB và mật khẩu người dùng vừa gõ (dto.Password). ⚙️ Cơ chế ngầm: Hàm VerifyHashedPassword tự tách lấy hạt Salt từ chuỗi trong DB, băm thử mật khẩu vừa gõ rồi so sánh xem có khớp 100% không. 📤 Đầu ra: PasswordVerificationResult.Success hoặc Failed.",
                        "var result = hasher.VerifyHashedPassword(userInDb, userInDb.PasswordHash, dto.Password);\nif (result == PasswordVerificationResult.Failed) {\n    return Unauthorized(\"Email hoặc mật khẩu không chính xác!\");\n}"),
                    Step(3,
                        "3. Cách dùng tương đương với thư viện BCrypt (Nếu công ty dùng BCrypt)",
                        "📥 Đầu vào: Mật khẩu người dùng. ⚙️ Cơ chế ngầm: BCrypt.HashPassword(pass) lúc tạo tài khoản và BCrypt.Verify(pass, hash) lúc đăng nhập. 📤 Đầu ra: Cực kỳ gọn gàng trong 1 dòng lệnh.",
                        "// Lúc Đăng ký: user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);\n// Lúc Đăng nhập: bool isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);")
                },
                "Tại sao khi sử dụng PasswordHasher<User> của ASP.NET Core (hoặc BCrypt), nếu có 2 người dùng khác nhau cùng đặt mật khẩu giống hệt nhau là \"123456\", thì 2 chuỗi PasswordHash lưu trong SQL Server của họ lại khác nhau hoàn toàn?",
                new List<string>
                {
                    "A. Vì hệ thống tự động sinh ra một chuỗi ngẫu nhiên duy nhất gọi là 'Salt' (Muối) cho mỗi lần băm và trộn chung vào mật khẩu trước khi băm, giúp chống lại kiểu tấn công tra bảng dò sẵn (Rainbow Table Attack)",
                    "B. Vì máy tính bị lỗi tính toán",
                    "C. Vì SQL Server tự động đổi chữ",
                    "D. Vì hàm HashPassword lấy ngày sinh của người dùng làm mật khẩu"
                },
                "A",
                "Kỹ thuật Salting đảm bảo mỗi mật khẩu khi băm đều được gắn kèm một chuỗi ngẫu nhiên độc lập. Nhờ vậy kẻ tấn công không thể dùng bảng băm tính sẵn (Rainbow Tables) để dò ngược mật khẩu hàng loạt."
            ),

            // =====================================================================
            // BÀI 10.3
            // =====================================================================
            Build(
                topicId,
                "10.3",
                "Xác thực bằng Cookie & Session trong ASP.NET Core MVC (Cookie Authentication cho Đồ án Web MVC)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authentication/cookie",
                """
                • **💡 1. Khi làm Đồ án Web ASP.NET Core MVC (`.cshtml`), bạn nên dùng `Cookie Authentication` hay `JWT`?**
                  - Với **Web API** (cho Mobile App / React / Angular gọi): Dùng **JWT Token**.
                  - Với **Web MVC truyền thống** (như bài tập lớn *Quản lý Thư viện* chuyển trang bằng thẻ `<a asp-action="...">` trên trình duyệt): Bạn phải dùng **`Cookie Authentication`**! Vì trình duyệt sẽ **tự động lưu chiếc bánh Cookie đã mã hóa và tự động gửi kèm lên Server trong mọi cú click chuột chuyển trang** mà bạn không cần viết JavaScript gắn Header!
                • **🖼️ 2. Sơ đồ Luồng Đăng Nhập Bằng `Cookie Authentication` Trong ASP.NET Core MVC:**
                ```
                1. Người dùng nhập Tài khoản + Mật khẩu vào Form Login.cshtml ──► Bấm Submit
                                                                                      │
                2. AuthController kiểm tra DB đúng mật khẩu ◄─────────────────────────┘
                     │
                     ▼  Gọi lệnh: await HttpContext.SignInAsync(principal);
                3. ASP.NET Core mã hóa ClaimsPrincipal thành 1 chuỗi mật mã và gắn vào Header "Set-Cookie"
                     │
                     ▼
                4. Trình duyệt tự động cất Cookie (HttpOnly) và TỰ ĐỘNG GỬI KÈM trong mọi lần bấm sang trang khác!
                     │
                     ▼  Khi bấm nút Đăng xuất ──► Gọi: await HttpContext.SignOutAsync(); (Xóa sạch Cookie!)
                ```
                • **📊 3. Ba Thuộc Tính Bảo Mật Bắt Buộc Khi Cấu Hình Cookie:**
                | Thuộc tính Cookie | Thiết lập chuẩn | Tác dụng bảo vệ |
                | :--- | :--- | :--- |
                | **`HttpOnly = true`** | Bắt buộc bật `true` | **Chống tấn công XSS:** Ngăn chặn không cho bất kỳ đoạn mã JavaScript độc hại nào (`document.cookie`) đọc trộm được Cookie đăng nhập! |
                | **`SecurePolicy`** | `CookieSecurePolicy.Always`| Chỉ cho phép truyền Cookie qua đường truyền mã hóa `HTTPS`. |
                | **`LoginPath` & `AccessDeniedPath`**| `"/Auth/Login"` & `"/Auth/Forbidden"` | Khi khách chưa đăng nhập bấm vào trang có gắn `[Authorize]`, hệ thống **tự động đá chuyển hướng về trang `/Auth/Login`**! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cấu hình AddAuthentication(Cookie) trong Program.cs",
                        "📥 Đầu vào: Dự án ASP.NET Core MVC cần chức năng Đăng nhập/Phân quyền. ⚙️ Cơ chế ngầm: Đăng ký CookieAuthenticationDefaults, chỉ định đường dẫn trang Login và thời gian sống của phiên đăng nhập. 📤 Đầu ra: Hệ thống sẵn sàng quản lý phiên bằng Cookie.",
                        "builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)\n    .AddCookie(options => {\n        options.LoginPath = \"/Auth/Login\";               // Chưa đăng nhập tự chuyển về đây\n        options.AccessDeniedPath = \"/Auth/AccessDenied\"; // Không đủ quyền tự chuyển về đây\n        options.ExpireTimeSpan = TimeSpan.FromHours(8);\n        options.Cookie.HttpOnly = true;\n    });"),
                    Step(2,
                        "2. Thực hiện Đăng nhập bằng HttpContext.SignInAsync()",
                        "📥 Đầu vào: Đã kiểm tra đúng Email và Mật khẩu trong AuthController. ⚙️ Cơ chế ngầm: Tạo ClaimsPrincipal chứa Id, Tên, Quyền rồi gọi HttpContext.SignInAsync để phát hành Cookie bảo mật về trình duyệt. 📤 Đầu ra: Đăng nhập thành công.",
                        "var claims = new List<Claim> {\n    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),\n    new Claim(ClaimTypes.Name, user.FullName),\n    new Claim(ClaimTypes.Role, user.Role)\n};\nvar principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));\nawait HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);\nreturn RedirectToAction(\"Index\", \"Home\");"),
                    Step(3,
                        "3. Thực hiện Đăng xuất bằng HttpContext.SignOutAsync()",
                        "📥 Đầu vào: Người dùng bấm nút 'Đăng xuất' trên thanh Header. ⚙️ Cơ chế ngầm: Lệnh SignOutAsync ra lệnh cho trình duyệt xóa bỏ ngay lập tức chiếc Cookie xác thực hiện tại. 📤 Đầu ra: Quay về trang Login.",
                        "[HttpPost]\npublic async Task<IActionResult> Logout() {\n    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);\n    return RedirectToAction(\"Login\", \"Auth\");\n}")
                },
                "Khi cấu hình Cookie xác thực trong ASP.NET Core, tại sao việc thiết lập thuộc tính HttpOnly = true lại cực kỳ quan trọng đối với bảo mật?",
                new List<string>
                {
                    "A. Vì HttpOnly = true ngăn chặn hoàn toàn việc dùng JavaScript (document.cookie) trên trình duyệt để đọc nội dung Cookie, giúp bảo vệ phiên đăng nhập của người dùng khỏi lỗ hổng tấn công XSS (Cross-Site Scripting)",
                    "B. Vì nó giúp trang web không cần dùng mạng Internet vẫn chạy được",
                    "C. Vì nó tự động đổi mật khẩu của người dùng mỗi ngày",
                    "D. Vì nó giúp SQL Server chạy nhanh hơn"
                },
                "A",
                "Nếu không bật HttpOnly, kẻ tấn công chèn được một đoạn script nhỏ vào bình luận (XSS) có thể lấy cắp toàn bộ document.cookie gửi về server của chúng để chiếm quyền tài khoản. Khi HttpOnly = true, chỉ có trình duyệt mới được đính kèm Cookie vào HTTP Request, JavaScript hoàn toàn không nhìn thấy nó."
            ),

            // =====================================================================
            // BÀI 10.4
            // =====================================================================
            Build(
                topicId,
                "10.4",
                "Làm chủ Hệ sinh thái ASP.NET Core Identity (IdentityDbContext, UserManager, SignInManager & RoleManager)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authentication/identity",
                """
                • **💡 1. Hiểu bản chất của `ASP.NET Core Identity` trong 30 giây:**
                  - Nếu tự viết hệ thống tài khoản từ đầu, bạn phải tự tạo bảng `Users`, `Roles`, tự viết hàm băm mật khẩu, khóa tài khoản khi nhập sai 5 lần, tạo mã xác nhận quên mật khẩu qua Email, xác thực 2 lớp (2FA)...
                  - **`ASP.NET Core Identity`** là **Bộ khung quản lý Tài khoản & Phân quyền toàn diện do chính Microsoft xây dựng sẵn**: Chỉ cần kế thừa **`IdentityDbContext`**, nó sẽ tự động tạo sẵn cho bạn 7 bảng SQL chuẩn quốc tế (`AspNetUsers`, `AspNetRoles`, `AspNetUserRoles`...) cùng bộ 3 quản gia quyền lực: **`UserManager`**, **`SignInManager`** và **`RoleManager`**!
                • **🖼️ 2. Sơ đồ Cấu Trúc Các Bảng Của `ASP.NET Core Identity` & Bộ 3 Quản Gia:**
                ```
                [1. Các bảng tự động sinh ra khi kế thừa IdentityDbContext<ApplicationUser>]:
                ┌─────────────────┐       ┌─────────────────────┐       ┌─────────────────┐
                │   AspNetUsers   │ 1 ─── │   AspNetUserRoles   │ ─── 1 │   AspNetRoles   │
                │ (Id, UserName,  │       │ (UserId, RoleId)    │       │ (Id, Name)      │
                │  Email, Hash...)│       │ [Bảng trung gian]   │       │ (Admin, Staff)  │
                └─────────────────┘       └─────────────────────┘       └─────────────────┘

                [2. Bộ 3 Quản gia (Services) tiêm qua Dependency Injection]:
                ├── 👨‍💼 UserManager<ApplicationUser>   ──► Tạo user, Đổi mật khẩu, Gán Role cho user, Tìm user.
                ├── 🔐 SignInManager<ApplicationUser> ──► Kiểm tra đăng nhập, Khóa tài khoản khi nhập sai nhiều lần.
                └── 🛡️ RoleManager<IdentityRole>      ──► Tạo quyền mới ("Admin", "Reader"), Kiểm tra quyền tồn tại.
                ```
                • **📊 3. Cách mở rộng thêm cột riêng (`FullName`, `StudentCode`) vào bảng `AspNetUsers`:**
                  - Mặc định lớp `IdentityUser` của Microsoft chỉ có sẵn `Id`, `UserName`, `Email`, `PhoneNumber`, `PasswordHash`.
                  - Muốn thêm cột `HoTen`, `NgaySinh`, `MaDocGia`, bạn chỉ cần tạo một class kế thừa từ `IdentityUser`:
                    `public class ApplicationUser : IdentityUser { public string FullName { get; set; } }`!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kế thừa IdentityUser và IdentityDbContext",
                        "📥 Đầu vào: Cần thêm cột FullName và AvatarUrl vào bảng người dùng của Identity. ⚙️ Cơ chế ngầm: Tạo ApplicationUser : IdentityUser và cho AppDbContext kế thừa từ IdentityDbContext<ApplicationUser>. 📤 Đầu ra: Giữ nguyên toàn bộ bảng riêng của bạn đồng thời có thêm 7 bảng chuẩn của Identity.",
                        "public class ApplicationUser : IdentityUser {\n    public string FullName { get; set; } = string.Empty;\n}\n\npublic class AppDbContext : IdentityDbContext<ApplicationUser> {\n    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }\n    public DbSet<Book> Books { get; set; } // Các bảng nghiệp vụ của bạn vẫn khai báo bình thường!\n}"),
                    Step(2,
                        "2. Cấu hình quy tắc mật khẩu trong Program.cs (AddIdentity)",
                        "📥 Đầu vào: Tuỳ chỉnh độ khó mật khẩu và tính năng khóa tạm thời khi dò mật khẩu (Lockout). ⚙️ Cơ chế ngầm: Đăng ký AddIdentity<ApplicationUser, IdentityRole>() nối với EntityFrameworkStores. 📤 Đầu ra: Kích hoạt UserManager và RoleManager.",
                        "builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {\n    options.Password.RequiredLength = 6;\n    options.Password.RequireNonAlphanumeric = false;\n    options.Lockout.MaxFailedAccessAttempts = 5; // Nhập sai 5 lần tự động khóa 15 phút!\n})\n.AddEntityFrameworkStores<AppDbContext>()\n.AddDefaultTokenProviders();"),
                    Step(3,
                        "3. Dùng UserManager để Tạo tài khoản & Gán quyền tự động",
                        "📥 Đầu vào: Thông tin đăng ký của người dùng mới. ⚙️ Cơ chế ngầm: Gọi _userManager.CreateAsync(user, password) — hàm này tự động kiểm tra trùng Email, tự băm mật khẩu kèm Salt và lưu xuống DB, sau đó gọi AddToRoleAsync! 📤 Đầu ra: Tạo User kèm Role trong 3 dòng lệnh.",
                        "var user = new ApplicationUser { UserName = dto.Email, Email = dto.Email, FullName = dto.FullName };\nIdentityResult result = await _userManager.CreateAsync(user, dto.Password);\n\nif (result.Succeeded) {\n    await _userManager.AddToRoleAsync(user, \"Member\");\n}")
                },
                "Khi sử dụng UserManager<ApplicationUser> của ASP.NET Core Identity để tạo mới một người dùng bằng câu lệnh await _userManager.CreateAsync(user, dto.Password);, bạn có cần phải tự viết code băm mật khẩu (dto.Password) trước khi truyền vào hàm này không?",
                new List<string>
                {
                    "A. KHÔNG CẦN! Vì bên trong hàm CreateAsync(user, password), UserManager đã tự động kiểm tra độ mạnh của mật khẩu, tự động sinh Salt, tự động băm mật khẩu bằng PasswordHasher và lưu vào cột PasswordHash giúp bạn",
                    "B. Bắt buộc phải tự băm bằng MD5 trước khi truyền vào",
                    "C. Hàm CreateAsync sẽ lưu mật khẩu dạng chữ thô không mã hóa",
                    "D. Hàm CreateAsync không lưu mật khẩu vào Database"
                },
                "A",
                "UserManager.CreateAsync(user, plainPassword) nhận mật khẩu gốc để thực hiện đầy đủ các bước: chạy PasswordValidator kiểm tra quy tắc mật khẩu, sau đó tự động gọi IPasswordHasher để băm kèm Salt rồi mới lưu vào AspNetUsers."
            ),

            // =====================================================================
            // BÀI 10.5
            // =====================================================================
            Build(
                topicId,
                "10.5",
                "Kiến trúc Xác thực JWT (JSON Web Token) Chuyên sâu: Cấu hình JwtBearer, SigningKey và cơ chế Giải mã Token",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authentication/configure-jwt-bearer-authentication",
                """
                • **💡 1. Tại sao Web API hiện đại luôn chọn `JWT (JSON Web Token)` thay vì Session/Cookie?**
                  - **Session truyền thống (Stateful):** Mỗi khi có người đăng nhập, Server phải tốn một vùng RAM để nhớ phiên của người đó. Nếu có 100.000 người dùng trên Mobile App cùng mở ứng dụng, RAM Server bị quá tải và khi nâng cấp lên 2-3 máy chủ chạy song song (*Load Balancing*), máy chủ số 2 sẽ không biết phiên đăng nhập đang lưu ở RAM máy chủ số 1!
                  - **JWT (Stateless — Phi trạng thái):** Server **không cần lưu bất kỳ phiên nào trên RAM**! Mọi thông tin định danh (`UserId`, `Role`, `Hạn dùng`) đã được gói gọn bên trong chuỗi Token và **đóng dấu mật mã (`HMAC-SHA256`)** giao cho Client tự giữ. Bất kỳ máy chủ nào chỉ cần dùng chung `SecretKey` kiểm tra con dấu đúng là cho qua ngay trong `0.01ms`!
                • **🖼️ 2. Giải phẫu 3 Phần Của Chuỗi `JWT` (`Header.Payload.Signature`) & Cơ Chế Chống Giả Mạo:**
                ```
                Chuỗi JWT:  eyJhbGciOiJIUzI1NiJ9 . eyJzdWIiOiIxMDUiLCJyb2xlIjoiQWRtaW4ifQ . SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
                            ├─── 1. HEADER ────┤   ├──────────── 2. PAYLOAD ────────────┤   ├───────────── 3. SIGNATURE ────────────┤
                            │ Thuật toán: HS256│   │ Chứa Claims: Id=105, Role="Admin", │   │ Chữ ký = HMACSHA256(                  │
                            │ Loại: JWT        │   │ exp = 1735689600 (Giờ hết hạn)     │   │   Base64(Header) + "." + Base64(Payload),│
                            └──────────────────┘   └────────────────────────────────────┘   │   Khóa_Bí_Mật_SecretKey_Trên_Server)  │
                                                                                            └───────────────────────────────────────┘
                🛡️ ĐIỀU GÌ XẢY RA NẾU HACKER MỞ PAYLOAD SỬA "Role": "Member" THÀNH "Role": "Admin"?
                   ──► Vì Hacker không có "Khóa_Bí_Mật_SecretKey" nằm trên Server của bạn, nên khi sửa Payload,
                       phần Chữ ký (Signature) ở cuối sẽ BỊ LỆCH NGAY LẬP TỨC! Middleware JwtBearer phát hiện chữ ký
                       không khớp sẽ lập tức từ chối và trả về 401 Unauthorized!
                ```
                • **📊 3. Bảng 5 Tham Số Kiểm Tra Sống Còn Trong `TokenValidationParameters` (`Program.cs`):**
                | Tham số cấu hình | Giá trị chuẩn | Ý nghĩa kiểm tra tại cổng `UseAuthentication()` |
                | :--- | :--- | :--- |
                | **`ValidateIssuerSigningKey`**| **`true`** | Kiểm tra chữ ký số bằng `SecretKey` (Chống Token giả mạo/bị chỉnh sửa). |
                | **`ValidateLifetime`** | **`true`** | Kiểm tra xem Token đã quá giờ hết hạn (`exp`) chưa. |
                | **`ClockSkew`** | **`TimeSpan.Zero`**| ⚠️ Mặc định .NET cho phép lố giờ thêm 5 phút! Đặt bằng `TimeSpan.Zero` để Token hết hạn đúng từng giây! |
                | **`ValidateIssuer`** | `true` | Kiểm tra xem Token có đúng là do Server của mình (`Issuer`) phát hành không. |
                | **`ValidateAudience`** | `true` | Kiểm tra xem Token có đúng là cấp cho Ứng dụng (`Audience`) này dùng không. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo cấu hình JwtSettings trong appsettings.json",
                        "📥 Đầu vào: Khóa bí mật (ít nhất 32 ký tự), tên nơi phát hành (Issuer) và thời gian sống. ⚙️ Cơ chế ngầm: Tách biệt thông số bảo mật ra khỏi code C#. 📤 Đầu ra: Cấu hình chuẩn mực.",
                        "\"JwtSettings\": {\n  \"SecretKey\": \"ChuoiBiMatSieuDaiVaAnToanCuaDuAnHoangViet2026!\",\n  \"Issuer\": \"HoangVietApi\",\n  \"Audience\": \"HoangVietClient\",\n  \"ExpiryMinutes\": 30\n}"),
                    Step(2,
                        "2. Đăng ký dịch vụ AddJwtBearer trong Program.cs",
                        "📥 Đầu vào: Các thông số từ JwtSettings. ⚙️ Cơ chế ngầm: Cấu hình Middleware tự động bắt mọi Request có Header 'Authorization: Bearer ...', kiểm tra chữ ký và hạn dùng với ClockSkew = TimeSpan.Zero. 📤 Đầu ra: Cổng kiểm soát JWT tự động.",
                        "builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)\n    .AddJwtBearer(options => {\n        options.TokenValidationParameters = new TokenValidationParameters {\n            ValidateIssuer = true,\n            ValidIssuer = builder.Configuration[\"JwtSettings:Issuer\"],\n            ValidateAudience = true,\n            ValidAudience = builder.Configuration[\"JwtSettings:Audience\"],\n            ValidateIssuerSigningKey = true,\n            IssuerSigningKey = new SymmetricSecurityKey(\n                Encoding.UTF8.GetBytes(builder.Configuration[\"JwtSettings:SecretKey\"]!)),\n            ValidateLifetime = true,\n            ClockSkew = TimeSpan.Zero // Hết hạn chính xác tuyệt đối, không cộng bù 5 phút\n        };\n    });"),
                    Step(3,
                        "3. Viết hàm Sinh chuỗi JWT Token (GenerateAccessToken)",
                        "📥 Đầu vào: Đối tượng User đã đăng nhập thành công. ⚙️ Cơ chế ngầm: Đóng gói Claims, ký bằng thuật toán HmacSha256 với SecretKey và xuất ra chuỗi Token. 📤 Đầu ra: Chuỗi AccessToken gửi về cho Client.",
                        "public string GenerateAccessToken(User user) {\n    var claims = new[] {\n        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),\n        new Claim(ClaimTypes.Email, user.Email),\n        new Claim(ClaimTypes.Role, user.Role)\n    };\n    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));\n    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);\n    var token = new JwtSecurityToken(\n        issuer: _jwtSettings.Issuer,\n        audience: _jwtSettings.Audience,\n        claims: claims,\n        expires: DateTime.UtcNow.AddMinutes(30),\n        signingCredentials: creds);\n    return new JwtSecurityTokenHandler().WriteToken(token);\n}")
                },
                "Khi cấu hình TokenValidationParameters cho JWT trong ASP.NET Core, bạn đặt thời gian hết hạn của Token là 15 phút (DateTime.UtcNow.AddMinutes(15)), nhưng khi test thực tế bạn thấy đến phút thứ 18 Token vẫn dùng được mà chưa báo hết hạn. Nguyên nhân là vì sao và cần thêm dòng cấu hình nào để Token hết hạn đúng phút thứ 15?",
                new List<string>
                {
                    "A. Vì mặc định thuộc tính ClockSkew của .NET có giá trị là 5 phút (để bù trừ lệch đồng hồ giữa các máy chủ); cần đặt ClockSkew = TimeSpan.Zero trong TokenValidationParameters để Token hết hạn chính xác từng giây",
                    "B. Vì đồng hồ của SQL Server chạy chậm hơn 1 tiếng",
                    "C. Vì JWT không hỗ trợ thời gian dưới 60 phút",
                    "D. Vì Postman tự động gia hạn Token"
                },
                "A",
                "Mặc định TokenValidationParameters.ClockSkew bằng TimeSpan.FromMinutes(5). Khi test Token ngắn hạn (hoặc cần độ chính xác tuyệt đối), các lập trình viên luôn gán ClockSkew = TimeSpan.Zero."
            ),

            // =====================================================================
            // BÀI 10.6
            // =====================================================================
            Build(
                topicId,
                "10.6",
                "Cơ chế Cặp bài trùng Access Token & Refresh Token (Gia hạn phiên đăng nhập tự động & Thu hồi Token khi bị lộ)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/entra/identity-platform/refresh-tokens",
                """
                • **💡 1. Bài toán nan giải của `Access Token` (JWT) đơn lẻ và Sự ra đời của `Refresh Token`:**
                  - Nếu bạn đặt thời gian sống của `Access Token (JWT)` là **30 ngày**: Lỡ kẻ gian đánh cắp được chuỗi JWT đó, hắn sẽ ung dung dùng suốt 30 ngày mà Server không dễ gì thu hồi được (vì JWT là Stateless)!
                  - Nhưng nếu bạn đặt thời gian sống của `Access Token` chỉ **15 phút** cho an toàn: Cứ 15 phút người dùng đang đọc bài lại bị văng ra bắt gõ lại Tài khoản & Mật khẩu thì khách hàng sẽ xóa App ngay lập tức!
                  - **Giải pháp Chuẩn Doanh Nghiệp — Cặp đôi `Access Token` + `Refresh Token`:**
                    1. **`Access Token` (JWT ngắn hạn — Sống `15 đến 30 phút`):** Dùng để gọi các API hàng ngày.
                    2. **`Refresh Token` (Chuỗi ngẫu nhiên dài hạn — Sống `7 đến 30 ngày`, được lưu trong bảng SQL Server):** Khi `Access Token` hết hạn sau 15 phút, Frontend âm thầm đem `Refresh Token` gửi lên API `/api/auth/refresh` để **đổi lấy một cặp `Access Token` + `Refresh Token` mới tinh** mà người dùng không hề hay biết!
                • **🖼️ 2. Sơ đồ Luồng Hoạt Động Của `Access Token` & `Refresh Token Rotation`:**
                ```
                [1. Lúc Login thành công] ──► Server trả về cả 2 thẻ:
                    ├── AccessToken (JWT - Hạn 15 phút)
                    └── RefreshToken (Chuỗi ngẫu nhiên 64 bytes - Hạn 7 ngày, LƯU VÀO DATABASE)

                [2. Sau 15 phút, AccessToken hết hạn (API trả về 401)]
                    │
                    ▼  Frontend tự động gọi ngầm: POST /api/auth/refresh-token (Gửi kèm RefreshToken)
                [3. Server kiểm tra bảng RefreshTokens trong DB]:
                    ├── Nếu hợp lệ & chưa bị thu hồi (IsRevoked == false):
                    │     ──► Thu hồi RefreshToken cũ + Cấp cặp (AccessToken mới + RefreshToken mới)!
                    └── Nếu Admin đã bấm "Đăng xuất khỏi mọi thiết bị" (IsRevoked = true):
                          ──► Từ chối cấp mới, bắt người dùng phải đăng nhập lại!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo chuỗi Refresh Token ngẫu nhiên an toàn mật mã học",
                        "📥 Đầu vào: Yêu cầu sinh Refresh Token khi đăng nhập. ⚙️ Cơ chế ngầm: Dùng RandomNumberGenerator.GetBytes(64) để sinh 64 bytes ngẫu nhiên không thể đoán trước và lưu xuống bảng Users / RefreshTokens trong SQL Server. 📤 Đầu ra: Chuỗi RefreshToken kèm hạn dùng 7 ngày.",
                        "public string GenerateRefreshToken() {\n    byte[] randomBytes = RandomNumberGenerator.GetBytes(64);\n    return Convert.ToBase64String(randomBytes);\n}"),
                    Step(2,
                        "2. Lưu Refresh Token vào Database khi Login",
                        "📥 Đầu vào: User vừa đăng nhập. ⚙️ Cơ chế ngầm: Cập nhật RefreshToken và RefreshTokenExpiryTime vào bản ghi User trong DB, trả về cả AccessToken và RefreshToken cho Client. 📤 Đầu ra: TokenResponseDto.",
                        "user.RefreshToken = GenerateRefreshToken();\nuser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);\nawait _db.SaveChangesAsync();\n\nreturn new TokenResponseDto(accessToken, user.RefreshToken);"),
                    Step(3,
                        "3. Viết API Cấp mới Token (/api/auth/refresh) & Thu hồi khi Logout",
                        "📥 Đầu vào: RefreshToken gửi từ Client lên khi AccessToken hết hạn. ⚙️ Cơ chế ngầm: Tìm User có RefreshToken khớp và hạn dùng > UtcNow. Nếu đúng, cấp cặp Token mới (Token Rotation); khi Logout chỉ cần xóa RefreshToken = null trong DB! 📤 Đầu ra: Duy trì đăng nhập liền mạch và bảo mật 100%.",
                        "var user = await _db.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);\nif (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)\n    return Unauthorized(\"Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại!\");\n\n// Cấp mới cả hai (Refresh Token Rotation):\nstring newAccess = GenerateAccessToken(user);\nuser.RefreshToken = GenerateRefreshToken();\nawait _db.SaveChangesAsync();\nreturn Ok(new TokenResponseDto(newAccess, user.RefreshToken));")
                },
                "Tại sao trong các hệ thống thực tế, người ta luôn kết hợp Access Token có thời gian sống ngắn (15-30 phút) đi kèm với Refresh Token có thời gian sống dài (lưu trong Database) thay vì chỉ dùng một Access Token duy nhất có hạn 30 ngày?",
                new List<string>
                {
                    "A. Vì Access Token ngắn hạn giúp giảm thiểu rủi ro nếu chẳng may bị lộ trên đường truyền; đồng thời việc lưu Refresh Token trong Database giúp Server có quyền chủ động thu hồi (Revoke) phiên đăng nhập của người dùng bất cứ lúc nào (khi đổi mật khẩu hoặc phát hiện nghi vấn) mà người dùng bình thường vẫn không phải đăng nhập lại nhiều lần",
                    "B. Vì JWT không thể đặt thời gian lớn hơn 30 phút",
                    "C. Để làm cho code dài hơn",
                    "D. Vì Refresh Token dùng để lưu hình ảnh đại diện"
                },
                "A",
                "Sự kết hợp này mang lại ưu điểm của cả hai thế giới: tốc độ kiểm tra siêu nhanh không cần gọi DB của Stateless JWT Access Token trong 15 phút, và khả năng kiểm soát/thu hồi phiên chặt chẽ dưới DB của Stateful Refresh Token."
            ),

            // =====================================================================
            // BÀI 10.7
            // =====================================================================
            Build(
                topicId,
                "10.7",
                "Phân quyền Chuyên sâu: Role-Based (RBAC) vs Policy-Based & Claim-Based Authorization",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authorization/policies",
                """
                • **💡 1. Khi nào Phân quyền theo Vai trò (`Role-Based`) là chưa đủ và phải dùng `Policy-Based`?**
                  - **Cấp độ 1 - `Role-Based Authorization` (`[Authorize(Roles = "Admin,Librarian")]`):**
                    + Đơn giản, kiểm tra xem người dùng có thuộc nhóm chức danh `"Admin"` hay `"Librarian"` không.
                  - **Cấp độ 2 - `Claim-Based & Policy-Based Authorization` (`[Authorize(Policy = "...")]`):**
                    + *Bài toán thực tế:* Công ty quy định: *"Chỉ những nhân viên thuộc phòng `IT`, có thâm niên trên `2 năm` VÀ đã xác thực số điện thoại mới được phép bấm nút Xóa Database"*. Nếu chỉ dùng `Role` sẽ phải đẻ ra hàng chục cái tên Role ghép nối kỳ dị!
                    + **Giải pháp `Policy` (Chính sách phân quyền):** Bạn định nghĩa một Chính sách tên là `"CanDeleteDatabase"` trong `Program.cs` kết hợp nhiều điều kiện (`RequireRole`, `RequireClaim`, hoặc hàm kiểm tra `RequireAssertion`), sau đó trên Controller chỉ cần gắn gọn gàng: **`[Authorize(Policy = "CanDeleteDatabase")]`**!
                • **🖼️ 2. Sơ đồ So Sánh 3 Cấp Độ Phân Quyền Trong ASP.NET Core:**
                ```
                [1. Role-Based (Theo chức danh cố định)]:
                    [Authorize(Roles = "Admin,Manager")] ──► Kiểm tra ClaimTypes.Role

                [2. Policy + Claim-Based (Theo thuộc tính chi tiết gắn trong thẻ Token)]:
                    [Authorize(Policy = "VipLibraryMember")] ──► Kiểm tra: Role == "Reader" VÀ Claim "Membership" == "Gold"

                [3. Resource-Based (Kiểm tra chủ sở hữu bản ghi - Ai tạo người đó mới được sửa!)]:
                    Kiểm tra trong Service: if (post.AuthorId != currentUserId && !User.IsInRole("Admin")) return Forbid();
                ```
                • **📊 3. Bảng Các Phương Thức Xây Dựng Chính Sách (`AddAuthorizationBuilder`) Trong `Program.cs`:**
                | Phương thức cấu hình Policy | Ý nghĩa điều kiện bắt buộc |
                | :--- | :--- |
                | **`.RequireRole("Admin", "Staff")`** | Người dùng phải có ít nhất 1 trong các Role được liệt kê. |
                | **`.RequireClaim("Department", "IT")`** | Trong Token của người dùng bắt buộc phải có Claim `"Department"` mang giá trị `"IT"`. |
                | **`.RequireAssertion(ctx => ...)`** | Viết biểu thức Lambda kiểm tra logic kết hợp (`AND` / `OR`) phức tạp tùy ý! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Định nghĩa các Policy phân quyền tập trung tại Program.cs",
                        "📥 Đầu vào: Quy tắc phân quyền của thư viện. ⚙️ Cơ chế ngầm: Sử dụng AddAuthorization để tạo chính sách 'ManageBooksPolicy' yêu cầu quyền Admin hoặc Thủ thư có Claim 'CanEditBook'. 📤 Đầu ra: Chính sách phân quyền rõ ràng, dễ sửa đổi.",
                        "builder.Services.AddAuthorization(options => {\n    options.AddPolicy(\"ManageBooksPolicy\", policy =>\n        policy.RequireAssertion(context =>\n            context.User.IsInRole(\"Admin\") ||\n            (context.User.IsInRole(\"Librarian\") && context.User.HasClaim(\"Permission\", \"Book.Write\"))\n        ));\n});"),
                    Step(2,
                        "2. Áp dụng Policy lên Controller hoặc Action bằng [Authorize(Policy = \"...\")]",
                        "📥 Đầu vào: Hàm xóa hoặc cập nhật sách. ⚙️ Cơ chế ngầm: Gắn [Authorize(Policy = \"ManageBooksPolicy\")]. Nếu người gọi không thỏa mãn Policy, Middleware tự động chặn lại và trả về 403 Forbidden. 📤 Đầu ra: Bảo vệ chặt chẽ API.",
                        "[Authorize(Policy = \"ManageBooksPolicy\")]\n[HttpDelete(\"{id:int}\")]\npublic async Task<IActionResult> DeleteBook(int id) {\n    await _bookService.DeleteAsync(id);\n    return NoContent();\n}"),
                    Step(3,
                        "3. Kiểm tra Quyền sở hữu tài nguyên (Chống lỗi IDOR - Sửa bài của người khác)",
                        "📥 Đầu vào: Người dùng A muốn sửa đánh giá sách (Review) có id = 10. ⚙️ Cơ chế ngầm: Phải kiểm tra xem review.UserId trong DB có đúng bằng currentUserId trong Token của người đang gọi không! Nếu khác thì trả về Forbid() (403). 📤 Đầu ra: Chặn đứng việc người này sửa/xóa dữ liệu của người kia.",
                        "int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);\nvar review = await _db.Reviews.FindAsync(reviewId);\n\nif (review.UserId != currentUserId && !User.IsInRole(\"Admin\")) {\n    return Forbid(); // Trả về 403 Forbidden: Không được sửa bài của người khác!\n}")
                },
                "Một lỗ hổng bảo mật rất phổ biến gọi là IDOR (Insecure Direct Object References) xảy ra khi API GET /api/orders/{id} chỉ gắn mỗi thẻ [Authorize] (yêu cầu đăng nhập), dẫn đến việc Khách hàng A (UserId = 5) sau khi đăng nhập có thể gõ đại /api/orders/99 để xem trộm đơn hàng của Khách hàng B (UserId = 8). Bạn phải viết thêm kiểm tra gì trong code để vá lỗ hổng này?",
                new List<string>
                {
                    "A. Lấy UserId của người đang đăng nhập từ Token (User.FindFirstValue) và kiểm tra đối chiếu với cột order.UserId của đơn hàng trong Database: nếu không khớp (và không phải là Admin) thì lập tức chặn lại trả về 403 Forbid() hoặc 404 NotFound()",
                    "B. Bỏ thẻ [Authorize] đi",
                    "C. Đổi phương thức GET thành POST",
                    "D. Không cần kiểm tra vì khách hàng không biết số 99"
                },
                "A",
                "Thẻ [Authorize] chỉ kiểm tra người dùng đã đăng nhập hay chưa, chứ không biết bản ghi số 99 trong Database thuộc về ai. Kiểm tra quyền sở hữu (order.UserId == currentUserId) là bước bắt buộc đối với mọi API xem/sửa/xóa dữ liệu cá nhân."
            ),

            // =====================================================================
            // BÀI 10.8
            // =====================================================================
            Build(
                topicId,
                "10.8",
                "Xác thực qua bên thứ ba với OAuth 2.0 & OpenID Connect (Đăng nhập bằng Google / GitHub trong .NET)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/authentication/social/google-logins",
                """
                • **💡 1. Hiểu bản chất của `OAuth 2.0` và `OpenID Connect (OIDC)` trong 30 giây:**
                  - Khi bạn vào các trang web hiện nay và bấm nút **"Đăng nhập bằng Google"** hoặc **"Đăng nhập bằng GitHub"**: Trang web đó **KHÔNG HỀ biết mật khẩu Gmail của bạn**, mà nó ủy quyền cho Google xác thực bạn rồi nhận về một **"Giấy xác nhận danh tính"** chứa Email, Họ tên và Ảnh đại diện của bạn!
                  - **Sự khác nhau giữa `OAuth 2.0` và `OpenID Connect`:**
                    + **`OAuth 2.0`:** Là giao thức **Ủy quyền truy cập tài nguyên (`Authorization`)** — cấp `Access Token` để ứng dụng được phép đọc lịch Google Calendar hoặc kho code GitHub của bạn.
                    + **`OpenID Connect (OIDC)`:** Là lớp mở rộng xây chồng lên trên `OAuth 2.0` chuyên dùng để **Xác thực danh tính (`Authentication`)** — cấp thêm thẻ **`ID Token` (dạng JWT)** chứa thông tin hồ sơ người dùng (`Email`, `Name`, `Picture`) để thực hiện chức năng *"Đăng nhập bằng Google"*!
                • **🖼️ 2. Sơ đồ 4 Bước Của Luồng Đăng Nhập Google (`OAuth2 / OIDC Authorization Code Flow`):**
                ```
                [1. Người dùng bấm "Login with Google"]
                      │
                      ▼  (Chuyển hướng sang màn hình chọn tài khoản của accounts.google.com)
                [2. Người dùng đồng ý] ──► Google trả về 1 mã tạm "Authorization Code" vào URL Callback (/signin-google)
                      │
                      ▼  (Backend ASP.NET Core cầm Code + ClientSecret gọi ngầm lên Server Google)
                [3. Google xác nhận và trả về ID Token + thông tin Email, Họ tên, Avatar]
                      │
                      ▼
                [4. Backend C# kiểm tra Email trong bảng Users]:
                      ├── Nếu chưa có: Tự động tạo tài khoản mới trong DB!
                      └── Cấp JWT Token (hoặc Cookie) của hệ thống mình cho người dùng vào trang chủ!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo ClientId và ClientSecret trong cấu hình",
                        "📥 Đầu vào: Tạo ứng dụng trên Google Cloud Console để lấy cặp khóa ClientId và ClientSecret. ⚙️ Cơ chế ngầm: Lưu vào User Secrets hoặc appsettings.json và đăng ký dịch vụ AddGoogle() trong Program.cs. 📤 Đầu ra: Kết nối với máy chủ xác thực Google.",
                        "builder.Services.AddAuthentication()\n    .AddGoogle(googleOptions => {\n        googleOptions.ClientId = builder.Configuration[\"Authentication:Google:ClientId\"]!;\n        googleOptions.ClientSecret = builder.Configuration[\"Authentication:Google:ClientSecret\"]!;\n    });"),
                    Step(2,
                        "2. Kích hoạt chuyển hướng sang Google (Challenge)",
                        "📥 Đầu vào: Người dùng bấm nút 'Đăng nhập bằng Google'. ⚙️ Cơ chế ngầm: Hàm Challenge() chuyển hướng trình duyệt sang trang đăng nhập chính thức của Google. 📤 Đầu ra: Màn hình chọn tài khoản Google.",
                        "[HttpGet(\"google-login\")]\npublic IActionResult GoogleLogin() {\n    var properties = new AuthenticationProperties { RedirectUri = Url.Action(\"GoogleCallback\") };\n    return Challenge(properties, GoogleDefaults.AuthenticationScheme);\n}"),
                    Step(3,
                        "3. Hứng thông tin Email & Họ tên trả về tại hàm Callback",
                        "📥 Đầu vào: Google xác thực xong và gọi ngược về hàm GoogleCallback. ⚙️ Cơ chế ngầm: Đọc Email và Name từ Claims do Google trả về, tự động tạo User trong DB nếu lần đầu đăng nhập và cấp Token. 📤 Đầu ra: Đăng nhập một chạm hoàn tất.",
                        "[HttpGet(\"google-callback\")]\npublic async Task<IActionResult> GoogleCallback() {\n    var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);\n    if (!result.Succeeded) return Unauthorized();\n\n    string email = result.Principal.FindFirstValue(ClaimTypes.Email)!;\n    string fullName = result.Principal.FindFirstValue(ClaimTypes.Name)!;\n    // Kiểm tra/Tạo user trong DB theo email và trả về JWT của hệ thống!\n    return Ok(new { email, fullName });\n}")
                },
                "Trong luồng \"Đăng nhập bằng Google\" sử dụng chuẩn OAuth 2.0 & OpenID Connect, mật khẩu tài khoản Gmail của người dùng được nhập và kiểm tra ở đâu?",
                new List<string>
                {
                    "A. Người dùng nhập trực tiếp trên trang của Google (accounts.google.com); ứng dụng web C# của chúng ta hoàn toàn KHÔNG bao giờ nhìn thấy hay lưu trữ mật khẩu Gmail của người dùng mà chỉ nhận về Token xác nhận danh tính (Email, Họ tên) từ Google",
                    "B. Người dùng gửi mật khẩu Gmail vào Database SQL Server của chúng ta lưu trữ",
                    "C. Ứng dụng C# tự đoán mật khẩu của người dùng",
                    "D. Không cần xác thực gì cả"
                },
                "A",
                "Điểm ưu việt nhất của OAuth 2.0 / OpenID Connect là người dùng chỉ cung cấp mật khẩu cho nhà cung cấp danh tính gốc (Google/Microsoft/GitHub), còn ứng dụng bên thứ ba chỉ nhận được mã thông báo (Token) ủy quyền an toàn."
            ),

            // =====================================================================
            // BÀI 10.9
            // =====================================================================
            Build(
                topicId,
                "10.9",
                "Phòng chống 5 Lỗ hổng Bảo mật Web phổ biến nhất (OWASP Top 10: SQLi, XSS, CSRF, Secret Exposure, Mass Assignment)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/app-secrets",
                """
                • **💡 1. Năm Lỗ Hổng Bảo Mật Kinh Điển & Cơ Chế Phòng Thủ Sẵn Có Của ASP.NET Core:**
                  1. **`SQL Injection (SQLi)`:** Kẻ gian chèn mã lệnh SQL vào ô tìm kiếm/đăng nhập.
                     - *Phòng thủ:* Dùng **LINQ / EF Core** hoặc `FromSqlInterpolated` (tự động tham số hóa `@p0`).
                  2. **`XSS (Cross-Site Scripting)`:** Kẻ gian nhập đoạn mã `<script>alert('hack')</script>` vào ô Bình luận/Tên sách để khi người khác mở trang lên sẽ bị chạy mã độc đánh cắp dữ liệu.
                     - *Phòng thủ:* Cú pháp **`@Model.Content`** của Razor tự động mã hóa HTML (`<` biến thành `&lt;`) làm mã độc bị vô hiệu hóa 100%! *(Cảnh báo: Rất cẩn thận khi dùng `@Html.Raw(...)` vì hàm này tắt chế độ bảo vệ của Razor!)*.
                  3. **`CSRF (Cross-Site Request Forgery - Giả mạo yêu cầu từ trang web lạ)`:**
                     - *Phòng thủ (Trong MVC):* Mọi `<form method="post">` dùng Tag Helper trong MVC đều tự động sinh một thẻ ẩn `__RequestVerificationToken` và kiểm tra bằng **`[ValidateAntiForgeryToken]`**.
                  4. **`Mass Assignment / Over-Posting`:** Kẻ gian tự ý nhét thêm `"Role": "Admin"` hoặc `"IsVip": true` vào cục JSON gửi lên khi cập nhật hồ sơ cá nhân.
                     - *Phòng thủ:* **Luôn nhận dữ liệu đầu vào qua `DTO`** (chỉ chứa đúng `FullName`, `Phone`), tuyệt đối không nhận thẳng class `Entity` trong tham số của hàm Controller!
                  5. **`Lộ Khóa Bí Mật (Secret Exposure trên GitHub)`:** Đẩy nhầm API Key của Gemini/OpenAI hoặc mật khẩu SQL Server trong `appsettings.json` lên kho GitHub công khai!
                     - *Phòng thủ:* Dùng công cụ **`Secret Manager` (`dotnet user-secrets`)** khi code ở máy cá nhân và dùng **Biến môi trường (`Environment Variables`)** trên Server!
                • **🖼️ 2. Sơ đồ Cách Giấu API Key An Toàn Bằng `User Secrets` (Không bao giờ sợ lộ lên GitHub!):**
                ```
                Trong file appsettings.json (Đẩy lên GitHub):
                  "Gemini": { "ApiKey": "" }  <-- Để trống rỗng!

                Trên máy tính cá nhân của bạn (Chạy lệnh dotnet user-secrets set):
                  Lưu API Key thật ở thư mục bảo mật riêng trên ổ C của Windows (Nằm ngoài thư mục Git!)
                  ──► Khi bấm F5 chạy web, ASP.NET Core tự động nạp API Key thật vào chương trình!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Giấu API Key / Chuỗi kết nối nhạy cảm bằng dotnet user-secrets",
                        "📥 Đầu vào: API Key thật của Gemini hoặc JWT SecretKey. ⚙️ Cơ chế ngầm: Khởi tạo user-secrets cho project và lưu khóa bí mật vào kho lưu trữ cá nhân của máy tính, tuyệt đối không ghi vào file appsettings.json đẩy lên GitHub. 📤 Đầu ra: Bảo mật khóa 100% khi đẩy code lên GitHub.",
                        "dotnet user-secrets init\ndotnet user-secrets set \"GeminiSettings:ApiKey\" \"AIzaSyD-KhoaBiMatThatCuaBan\"\n// Trong code C# vẫn đọc bình thường qua: builder.Configuration[\"GeminiSettings:ApiKey\"]!"),
                    Step(2,
                        "2. Chống tấn công Mass Assignment bằng cách tách biệt DTO",
                        "📥 Đầu vào: API Cập nhật thông tin cá nhân (PUT /api/users/profile). ⚙️ Cơ chế ngầm: Tạo UpdateProfileDto chỉ có 2 trường FullName và PhoneNumber (không có trường Role hay Balance). Dù kẻ gian có cố gửi kèm \"role\": \"Admin\" trong JSON cũng bị bỏ qua hoàn toàn! 📤 Đầu ra: An toàn dữ liệu nghiệp vụ.",
                        "// ĐÚNG CHUẨN: Chỉ cho phép sửa đúng những trường có trong DTO\npublic record UpdateProfileDto(string FullName, string PhoneNumber);"),
                    Step(3,
                        "3. Chống CSRF trong MVC và Chống XSS trên giao diện",
                        "📥 Đầu vào: Form gửi dữ liệu và hiển thị bình luận của người dùng. ⚙️ Cơ chế ngầm: Dùng [ValidateAntiForgeryToken] trên hàm POST của MVC Controller và luôn in dữ liệu bằng @item.Comment (tự động HTML Encode). 📤 Đầu ra: Miễn nhiễm với CSRF và XSS.",
                        "[HttpPost]\n[ValidateAntiForgeryToken] // Chống giả mạo Request từ trang web độc hại khác\npublic async Task<IActionResult> Create(CreateBookViewModel model) { ... }")
                },
                "Khi làm dự án có tích hợp Gemini API Key (hoặc mật khẩu SQL Server đám mây) và cần đẩy source code lên kho chứa GitHub Public để nộp bài hoặc đưa vào CV, cách làm nào dưới đây giúp ứng dụng trên máy bạn vẫn đọc được API Key bình thường nhưng người ngoài trên GitHub không thể nhìn thấy API Key của bạn?",
                new List<string>
                {
                    "A. Sử dụng công cụ Secret Manager của .NET (lệnh dotnet user-secrets set ...) hoặc biến môi trường (Environment Variables), còn trong file appsettings.json đẩy lên GitHub chỉ để giá trị rỗng",
                    "B. Viết thẳng API Key vào trong file Controller.cs",
                    "C. Đổi tên biến thành tiếng Việt trong appsettings.json rồi đẩy lên GitHub",
                    "D. Ghi API Key vào phần mô tả README.md trên GitHub"
                },
                "A",
                "Các công cụ quét tự động (Bots) trên GitHub chỉ mất 3 giây để phát hiện và khóa ngay lập tức mọi API Key bị đẩy lên trong appsettings.json hoặc file .cs. Dùng dotnet user-secrets lưu khóa ở ngoài thư mục dự án là kỹ năng bảo mật bắt buộc của mọi lập trình viên .NET."
            ),

            // =====================================================================
            // BÀI 10.10
            // =====================================================================
            Build(
                topicId,
                "10.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 10 (Xây dựng Luồng AuthService Hoàn chỉnh Chuẩn Doanh Nghiệp)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/security/",
                """
                • **💡 1. Sơ đồ Tổng Hợp Kiến Trúc Một Module Bảo Mật (`AuthModule`) Hoàn Chỉnh Từ A-Z:**
                ```
                ┌──────────────────────────────────────────────────────────────────────────────┐
                │ 1. ĐĂNG KÝ (POST /api/auth/register):                                        │
                │    Kiểm tra trùng Email (.AnyAsync) ──► Băm mật khẩu kèm Salt (PasswordHasher│
                │    hoặc BCrypt) ──► Gán Role mặc định "Member" ──► Lưu vào bảng Users!       │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 2. ĐĂNG NHẬP (POST /api/auth/login):                                         │
                │    Tìm User theo Email ──► VerifyHashedPassword ──► Đóng gói Claims (Id,     │
                │    Email, Role) ──► Cấp cặp [Access Token (30p) + Refresh Token (7 ngày)]!   │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 3. GỌI API BẢO MẬT (Kèm Header: Authorization: Bearer <AccessToken>):        │
                │    Middleware giải mã Token ──► Kiểm tra [Authorize(Roles/Policy)] ──►       │
                │    Trích xuất currentUserId từ User.FindFirstValue(ClaimTypes.NameIdentifier)│
                └──────────────────────────────────────────────────────────────────────────────┘
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Xử lý Đăng ký tài khoản an toàn (RegisterAsync)",
                        "📥 Đầu vào: RegisterDto (Email, Password, FullName). ⚙️ Cơ chế ngầm: Chuẩn hóa Email chữ thường, kiểm tra trùng lặp bằng AnyAsync, băm mật khẩu bằng PasswordHasher và lưu DB. 📤 Đầu ra: Tài khoản mới được bảo mật mật khẩu 100%.",
                        "string cleanEmail = dto.Email.Trim().ToLower();\nif (await _db.Users.AnyAsync(u => u.Email == cleanEmail))\n    throw new BadRequestException(\"Email này đã được sử dụng!\");\n\nvar user = new User { Email = cleanEmail, FullName = dto.FullName, Role = \"Reader\" };\nuser.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);\nawait _db.Users.AddAsync(user);\nawait _db.SaveChangesAsync();"),
                    Step(2,
                        "2. Xử lý Đăng nhập & Phát hành cặp Token (LoginAsync)",
                        "📥 Đầu vào: LoginDto (Email, Password). ⚙️ Cơ chế ngầm: Kiểm tra mật khẩu băm, sinh AccessToken (JWT) và RefreshToken, lưu RefreshToken vào DB. 📤 Đầu ra: AuthResponseDto.",
                        "var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email.Trim().ToLower());\nif (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password) == PasswordVerificationResult.Failed)\n    throw new UnauthorizedAccessException(\"Tài khoản hoặc mật khẩu không đúng!\");\n\nstring accessToken = _tokenService.GenerateAccessToken(user);\nuser.RefreshToken = _tokenService.GenerateRefreshToken();\nuser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);\nawait _db.SaveChangesAsync();\nreturn new AuthResponseDto(accessToken, user.RefreshToken);"),
                    Step(3,
                        "3. Tạo dịch vụ ICurrentUserService để lấy UserId ở mọi tầng",
                        "📥 Đầu vào: IHttpContextAccessor được tiêm vào CurrentUserService. ⚙️ Cơ chế ngầm: Giúp tầng Service hoặc DbContext (khi tự động lưu cột CreatedBy) lấy được ngay UserId của người đang đăng nhập mà không cần truyền thủ công từ Controller xuống! 📤 Đầu ra: Kiến trúc Clean Architecture chuẩn mực.",
                        "public class CurrentUserService : ICurrentUserService {\n    private readonly IHttpContextAccessor _httpContextAccessor;\n    public CurrentUserService(IHttpContextAccessor accessor) => _httpContextAccessor = accessor;\n\n    public int? UserId => int.TryParse(\n        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),\n        out int id) ? id : null;\n}"),
                    Step(4,
                        "4. Bảo vệ Endpoint bằng [Authorize] và kiểm tra quyền sở hữu",
                        "📥 Đầu vào: API thao tác dữ liệu nhạy cảm. ⚙️ Cơ chế ngầm: Kết hợp kiểm tra Role/Policy trên Attribute và đối chiếu UserId thực tế bên trong Service. 📤 Đầu ra: Hệ thống bảo mật đa lớp vững chắc.",
                        "[Authorize(Roles = \"Admin,Librarian\")]\n[HttpPut(\"{id:int}/approve\")]\npublic async Task<IActionResult> ApproveBorrow(int id) {\n    await _borrowService.ApproveAsync(id, _currentUser.UserId!.Value);\n    return Ok(new { message = \"Đã duyệt phiếu mượn thành công!\" });\n}")
                },
                "(Câu hỏi tổng hợp Chủ đề 10) Trong kiến trúc dự án nhiều tầng (Controller ➔ Service ➔ DbContext), nếu ở bên trong tầng Service hoặc AppDbContext (nơi không kế thừa từ ControllerBase nên không có sẵn biến User), bạn muốn lấy thông tin UserId từ JWT Token của người đang gọi Request hiện tại thì phải tiêm (Inject) giao diện chuẩn nào của ASP.NET Core?",
                new List<string>
                {
                    "A. Đăng ký builder.Services.AddHttpContextAccessor(); trong Program.cs và tiêm giao diện IHttpContextAccessor (truy cập qua _httpContextAccessor.HttpContext?.User)",
                    "B. Tiêm giao diện ILogger",
                    "C. Tiêm giao diện IMemoryCache",
                    "D. Không thể lấy được thông tin người dùng ở ngoài Controller"
                },
                "A",
                "IHttpContextAccessor là công cụ chuẩn trong ASP.NET Core giúp truy cập vào HttpContext.User hiện tại từ bất kỳ tầng nào (như trong CurrentUserService hoặc trong SaveChangesAsync của DbContext để tự động điền cột CreatedBy / UpdatedBy)."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (10.1 -> 10.10) cho Chủ đề 10 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic10 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 10);
        if (topic10 == null)
            return;

        topic10.Title = "Chủ đề 10: Bảo mật ứng dụng với Authentication và Authorization";
        topic10.Description = "10 Bài học con siêu chi tiết từ Authentication vs Authorization (Claims, ClaimsIdentity, ClaimsPrincipal), Password Hashing & Salt (PasswordHasher/BCrypt), Cookie Authentication, ASP.NET Core Identity, JWT Bearer chuyên sâu, Access Token & Refresh Token Rotation, RBAC vs Policy-Based Authorization, OAuth2/OIDC (Google Login) đến phòng chống OWASP Top 10.";

        var targetLessons = GetLessons(topic10.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic10.Id)
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
