using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic14Seed
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
            // BÀI 14.1
            // =====================================================================
            Build(
                topicId,
                "14.1",
                "Kiến trúc Tích hợp Dịch vụ Bên thứ ba: Nguyên tắc Adapter Pattern (Bọc qua Interface) & Options Pattern",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/configuration/options",
                """
                • **💡 1. Hiểu bản chất của `Adapter / Wrapper Pattern` khi tích hợp dịch vụ ngoài trong 30 giây:**
                  - Giả sử hôm nay sếp bảo bạn dùng dịch vụ **Gmail SMTP** để gửi mail thông báo mượn sách. Nếu bạn viết code gọi trực tiếp thư viện Gmail rải rác ở trong `BorrowController`, `UserController`, `OrderController`... thì tháng sau công ty chuyển sang dùng **Amazon SES** hoặc **SendGrid**, bạn sẽ phải mở cả 20 file Controller ra đập đi viết lại!
                  - **Nguyên tắc Vàng khi tích hợp bất kỳ dịch vụ bên thứ ba nào (Email, Thanh toán, Lưu ảnh, SMS):**
                Luôn tạo một **`Interface` chung của riêng mình** (ví dụ: `IEmailService`, `IPaymentService`, `IFileStorageService`) rồi viết class thực thi bọc thư viện bên thứ ba ở bên trong! Khi muốn đổi nhà cung cấp, chỉ cần thay đổi đúng 1 dòng đăng ký `AddScoped` trong `Program.cs`, toàn bộ 20 Controller không phải sửa một chữ nào!
                • **🖼️ 2. Sơ đồ Kiến trúc "Cổng Chuyển Đổi" (`Adapter Pattern` + `Dependency Injection`):**
                ```
                    [Tầng Nghiệp vụ của bạn (BorrowService / OrderService)]
                                          │
                                          ▼  (Chỉ gọi qua hợp đồng chung, không quan tâm bên dưới dùng hãng nào!)
                             ╔═════════════════════════╗
                             ║   IEmailSender /        ║
                             ║   IPaymentGateway       ║
                             ╚════════════╤════════════╝
                                          │ (Đổi nhà cung cấp trong 5 giây tại Program.cs)
                           ┌──────────────┼──────────────┐
                           ▼              ▼              ▼
                    [GmailSmtpService] [SendGridService] [AwsSesService]
                ```
                • **📊 3. Quản lý Thông số Kết nối Bên thứ ba An toàn với `IOptions<T>` (`Options Pattern`):**
                | Thành phần cấu hình | Cách làm SAI ❌ | Cách làm ĐÚNG CHUẨN DOANH NGHIỆP ✅ |
                | :--- | :--- | :--- |
                | **Lưu API Key / Secret** | Viết cứng `"sk_live_123..."` trong file `.cs`. | Cất vào `appsettings.json` (hoặc `User Secrets` / Biến môi trường). |
                | **Đọc cấu hình vào C#** | Gọi `_config["Mail:Host"]` lặp lại nhiều nơi dễ gõ sai chữ. | Tạo class `MailSettings`, đăng ký `Configure<MailSettings>()` và tiêm **`IOptions<MailSettings>`** kèm kiểm tra `ValidateOnStart()`! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Class chứa thông số cấu hình (MailSettings.cs)",
                        "📥 Đầu vào: Mục \"MailSettings\" trong appsettings.json. ⚙️ Cơ chế ngầm: Tạo class C# có các thuộc tính trùng tên 100% với các khóa trong JSON, gắn [Required] để kiểm tra ngay lúc bật Server. 📤 Đầu ra: Đối tượng cấu hình định kiểu mạnh (Strongly-typed).",
                        "public class MailSettings {\n    [Required] public string Host { get; set; } = string.Empty;\n    [Range(1, 65535)] public int Port { get; set; }\n    [Required] public string SenderEmail { get; set; } = string.Empty;\n    [Required] public string AppPassword { get; set; } = string.Empty;\n}"),
                    Step(2,
                        "2. Đăng ký Options kèm ValidateOnStart() tại Program.cs",
                        "📥 Đầu vào: Class MailSettings. ⚙️ Cơ chế ngầm: Lệnh ValidateDataAnnotations().ValidateOnStart() giúp kiểm tra ngay khi vừa bấm chạy web: nếu quên điền AppPassword trong cấu hình, web sẽ báo lỗi rõ ràng ngay lập tức chứ không đợi đến lúc khách bấm gửi mail mới sập! 📤 Đầu ra: Cấu hình an toàn tuyệt đối.",
                        "builder.Services.AddOptions<MailSettings>()\n    .BindConfiguration(\"MailSettings\")\n    .ValidateDataAnnotations()\n    .ValidateOnStart(); // Kiểm tra đủ cấu hình ngay lúc khởi động Server!"),
                    Step(3,
                        "3. Định nghĩa Interface chung và tiêm IOptions<MailSettings>",
                        "📥 Đầu vào: Tiêm IOptions<MailSettings> vào dịch vụ gửi mail. ⚙️ Cơ chế ngầm: Lấy đối tượng cấu hình thông qua thuộc tính options.Value. 📤 Đầu ra: Tách biệt hoàn toàn logic nghiệp vụ và nhà cung cấp bên thứ ba.",
                        "public interface IEmailService {\n    Task SendEmailAsync(string toEmail, string subject, string htmlBody);\n}\n\npublic class SmtpEmailService : IEmailService {\n    private readonly MailSettings _settings;\n    public SmtpEmailService(IOptions<MailSettings> options) {\n        _settings = options.Value;\n    }\n    // Viết code gửi mail ở đây...\n}")
                },
                "Khi tích hợp một dịch vụ bên thứ ba (như cổng thanh toán hoặc dịch vụ gửi Email) vào dự án ASP.NET Core, tại sao việc gắn thêm .ValidateDataAnnotations().ValidateOnStart() khi đăng ký AddOptions<T>() trong Program.cs lại được các Senior đặc biệt khuyên dùng?",
                new List<string>
                {
                    "A. Để hệ thống tự động kiểm tra xem các thông số bắt buộc (như API Key, Host, SecretKey) đã được cấu hình đầy đủ chưa ngay tại thời điểm vừa khởi động Server (Fail-fast), tránh việc chạy lên bình thường nhưng đến lúc khách hàng thao tác mới bị nổ lỗi thiếu cấu hình",
                    "B. Để tự động tạo tài khoản Gmail mới",
                    "C. Để xóa file appsettings.json",
                    "D. Để bỏ qua bước kiểm tra mật khẩu"
                },
                "A",
                "Nguyên tắc Fail-Fast (Phát hiện lỗi sớm nhất có thể) giúp lập trình viên hoặc hệ thống CI/CD biết ngay lập tức nếu quên cài đặt biến môi trường trên Server mới ngay lúc khởi động ứng dụng."
            ),

            // =====================================================================
            // BÀI 14.2
            // =====================================================================
            Build(
                topicId,
                "14.2",
                "Tích hợp Dịch vụ Gửi Email Tự động (SMTP, Gmail App Password & Thư viện MailKit) với Giao diện HTML Template",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.net.mail.smtpclient",
                """
                • **💡 1. Tại sao trong .NET hiện đại phải dùng thư viện `MailKit` thay vì `System.Net.Mail.SmtpClient` cũ?**
                  - Lớp `SmtpClient` cũ có sẵn từ thời .NET Framework đã bị chính Microsoft đánh dấu **`[Obsolete]` (Khuyên không nên dùng nữa)** vì không hỗ trợ tốt các chuẩn bảo mật hiện đại và hiệu năng bất đồng bộ kém.
                  - Trong tài liệu chính thức của Microsoft Learn, Microsoft chỉ định dùng thư viện mã nguồn mở chuẩn quốc tế là **`MailKit` (cùng `MimeKit`)** để gửi Email trong mọi dự án ASP.NET Core!
                • **🖼️ 2. Lưu ý Sống Còn Khi Dùng Tài Khoản Gmail Cá Nhân Để Gửi Mail Từ Code C# (`App Password`):**
                ```
                    ⚠️ BẪY 100% SINH VIÊN GẶP PHẢI:
                    Khi cấu hình gửi mail qua máy chủ Gmail (Host: "smtp.gmail.com", Port: 587):
                    - Nếu bạn điền "Mật khẩu đăng nhập Gmail bình thường" của bạn vào code C# ──► Google sẽ CHẶN NGAY LẬP TỨC!
                    - Cách làm đúng chuẩn:
                      1. Vào quản lý Tài khoản Google ──► Bật "Xác minh 2 bước (2-Step Verification)".
                      2. Tìm mục "Mật khẩu ứng dụng (App Passwords)" ──► Tạo 1 mã gồm 16 chữ cái (VD: "abcd efgh ijkl mnop").
                      3. Dùng chuỗi 16 ký tự đó làm mật khẩu trong code C# ──► Gửi mail thành công 100%!
                ```
                • **📊 3. Cấu trúc một bức thư điện tử (`MimeMessage`) trong `MailKit`:**
                | Thành phần trong `MimeKit` | Vai trò thực tế |
                | :--- | :--- |
                | **`MimeMessage`** | Phong bì thư chứa Địa chỉ người gửi (`From`), Người nhận (`To`, `Cc`) và Tiêu đề (`Subject`). |
                | **`BodyBuilder`** | Soạn nội dung thư bằng **HTML (`HtmlBody`)** có màu sắc, bảng biểu, nút bấm và hỗ trợ đính kèm file (`Attachments` như file PDF hóa đơn/phiếu mượn)! |
                | **`MailKit.Net.Smtp.SmtpClient`**| Kết nối tới `smtp.gmail.com` cổng `587` (`StartTls`), xác thực `AuthenticateAsync` và bắn thư đi (`SendAsync`)! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo bức thư MimeMessage và thiết kế nội dung HTML đẹp mắt",
                        "📥 Đầu vào: Email người nhận, Tiêu đề và Nội dung HTML. ⚙️ Cơ chế ngầm: Dùng MimeMessage và BodyBuilder (từ gói NuGet MailKit) để đóng gói bức thư hỗ trợ đầy đủ tiếng Việt UTF-8 và định dạng HTML/CSS. 📤 Đầu ra: Đối tượng emailMessage hoàn chỉnh.",
                        "var emailMessage = new MimeMessage();\nemailMessage.From.Add(new MailboxAddress(\"Hệ thống Hoàng Việt\", _settings.SenderEmail));\nemailMessage.To.Add(MailboxAddress.Parse(toEmail));\nemailMessage.Subject = subject;\n\nvar builder = new BodyBuilder {\n    HtmlBody = $@\"<div style='font-family:sans-serif; padding:20px; border:1px solid #ddd; border-radius:8px;'>\n                    <h2 style='color:#2563EB;'>{subject}</h2>\n                    <p>{htmlContent}</p>\n                  </div>\"\n};\nemailMessage.Body = builder.ToMessageBody();"),
                    Step(2,
                        "2. Mở kết nối bảo mật TLS tới máy chủ SMTP và gửi thư bất đồng bộ",
                        "📥 Đầu vào: Đối tượng emailMessage ở Bước 1. ⚙️ Cơ chế ngầm: Dùng SmtpClient của MailKit kết nối tới smtp.gmail.com qua cổng 587 với tùy chọn SecureSocketOptions.StartTls, đăng nhập bằng App Password 16 ký tự và gửi thư. 📤 Đầu ra: Email bay vào hộp thư người nhận trong 1 giây.",
                        "using var smtp = new MailKit.Net.Smtp.SmtpClient();\ntry {\n    await smtp.ConnectAsync(_settings.Host, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);\n    await smtp.AuthenticateAsync(_settings.SenderEmail, _settings.AppPassword);\n    await smtp.SendAsync(emailMessage);\n} finally {\n    await smtp.DisconnectAsync(true);\n}"),
                    Step(3,
                        "3. Đính kèm File PDF Hóa đơn / Phiếu mượn vào Email",
                        "📥 Đầu vào: Mảng byte[] của file PDF vừa tạo bằng QuestPDF (ở Chủ đề 12). ⚙️ Cơ chế ngầm: Chỉ cần gọi thêm 1 dòng builder.Attachments.Add(\"PhieuMuon.pdf\", pdfBytes) trước khi gửi! 📤 Đầu ra: Khách hàng nhận được Email có đính kèm sẵn file PDF.",
                        "// Đính kèm trực tiếp file PDF từ RAM vào Email:\nbuilder.Attachments.Add(\"PhieuMuonSach.pdf\", pdfBytes, new ContentType(\"application\", \"pdf\"));")
                },
                "Khi cấu hình gửi Email tự động qua máy chủ SMTP của Gmail (smtp.gmail.com, cổng 587) trong dự án C#, tại sao bạn không thể dùng mật khẩu đăng nhập Gmail thông thường mà phải vào cài đặt tài khoản Google để tạo \"Mật khẩu ứng dụng\" (App Password gồm 16 ký tự)?",
                new List<string>
                {
                    "A. Vì chính sách bảo mật của Google chặn các ứng dụng bên thứ ba đăng nhập trực tiếp bằng mật khẩu chính; tạo App Password (sau khi bật xác thực 2 bước) cấp một mã truy cập riêng biệt chỉ dùng cho giao thức SMTP mà không làm lộ mật khẩu chính của bạn",
                    "B. Vì C# chỉ nhận chuỗi mật khẩu dài đúng 16 ký tự",
                    "C. Vì mật khẩu thường không gõ được vào file JSON",
                    "D. Để gửi email không cần kết nối mạng"
                },
                "A",
                "Sử dụng App Password là yêu cầu bắt buộc của Google SMTP từ năm 2022 trở đi nhằm bảo vệ tài khoản chính khỏi bị đánh cắp và cho phép bạn thu hồi quyền gửi mail của ứng dụng bất cứ lúc nào."
            ),

            // =====================================================================
            // BÀI 14.3
            // =====================================================================
            Build(
                topicId,
                "14.3",
                "Tích hợp Cổng Thanh toán Trực tuyến (VNPAY / MoMo / VietQR): Luồng Redirect, Chữ ký băm HMAC-SHA512 & Webhook IPN",
                "Web API",
                "https://sandbox.vnpayment.vn/apis/docs/thanh-toan-pay/pay.html",
                """
                • **💡 1. Hiểu bản chất Luồng Thanh Toán Trực Tuyến (`Payment Gateway` như VNPAY / MoMo) trong 30 giây:**
                  - Rất nhiều sinh viên thắc mắc: *"Khi khách bấm Thanh toán 100.000đ trên web của mình, làm sao web mình biết khách đã quét mã QR chuyển tiền thật bên trang VNPAY hay chưa? Và nếu khách tự gõ lên thanh địa chỉ trình duyệt `ReturnUrl?vnp_Amount=100000&vnp_ResponseCode=00` để lừa web mình là đã trả tiền rồi thì sao?"*.
                  - Để chống giả mạo 100%, mọi cổng thanh toán (VNPAY, MoMo, Stripe) đều hoạt động dựa trên **Chữ ký số mật mã (`HMAC-SHA512` với khóa bí mật `HashSecret`)** và **2 luồng xác nhận song song (`ReturnUrl` & `IPN Webhook`)**!
                • **🖼️ 2. Sơ đồ Luồng Thanh Toán Chuẩn Quốc Tế Của VNPAY / MoMo:**
                ```
                    [Bước 1]: Khách bấm "Thanh toán đơn #105 (100.000đ)" trên Web của bạn
                         │
                         ▼  Backend C# sắp xếp các tham số (vnp_Amount, vnp_TxnRef...) theo thứ tự A-Z
                            ──► Ký đóng dấu bằng khóa bí mật HashSecret (HMAC-SHA512) tạo ra đuôi "&vnp_SecureHash=..."
                         │
                         ▼
                    [Bước 2]: Chuyển hướng (Redirect) trình duyệt của khách sang trang quét QR/Thẻ của VNPAY
                         │  (Khách quét mã thanh toán thành công trên VNPAY)
                         ├───────────────────────────────────────────────────────────┐
                         ▼ (Luồng 1: Trình duyệt)                                    ▼ (Luồng 2: Server-to-Server ngầm)
                    [Bước 3A - ReturnUrl (Hiển thị giao diện)]:                [Bước 3B - IPN Webhook (Cập nhật Database!)]:
                    Đưa khách quay về trang "Cảm ơn!" trên web bạn.            Server VNPAY gọi ngầm vào API IPN của bạn để
                    *(Kiểm tra lại chữ ký vnp_SecureHash trước khi tin!)*      chốt đổi trạng thái đơn hàng thành "Đã thanh toán"!
                ```
                • **📊 3. Ba Quy Tắc Sống Còn Khi Lập Trình Tích Hợp Cổng Thanh Toán:**
                    1. **Nhân `100` số tiền khi gửi sang VNPAY:** Quy định của VNPAY là triệt tiêu phần thập phân nên đơn hàng `150.000đ` phải truyền `vnp_Amount = 150000 * 100` (`15000000`)!
                    2. **Luôn sắp xếp tham số theo bảng chữ cái A-Z (`SortedList`)** trước khi băm `HMAC-SHA512` tạo chữ ký `vnp_SecureHash`.
                    3. **Luôn kiểm tra lại chữ ký (`ValidateSignature`) và kiểm tra trạng thái đơn hàng chưa thanh toán** trước khi cộng tiền/duyệt đơn để chống lỗi cộng tiền 2 lần!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Sắp xếp tham số theo thứ tự A-Z bằng SortedList và nhân 100 số tiền",
                        "📥 Đầu vào: Đơn hàng Order (Id = 105, Amount = 150.000đ). ⚙️ Cơ chế ngầm: Dùng SortedList<string, string> để các khóa vnp_... tự động xếp theo thứ tự bảng chữ cái A-Z đúng chuẩn yêu cầu ghép chuỗi của VNPAY. 📤 Đầu ra: Danh sách tham số chuẩn.",
                        "var vnpParams = new SortedList<string, string>();\nvnpParams.Add(\"vnp_Version\", \"2.1.0\");\nvnpParams.Add(\"vnp_Command\", \"pay\");\nvnpParams.Add(\"vnp_TmnCode\", _vnpaySettings.TmnCode);\nvnpParams.Add(\"vnp_Amount\", ((long)(order.Amount * 100)).ToString()); // Nhân 100!\nvnpParams.Add(\"vnp_TxnRef\", order.Id.ToString());\nvnpParams.Add(\"vnp_ReturnUrl\", _vnpaySettings.ReturnUrl);"),
                    Step(2,
                        "2. Tạo chữ ký bảo mật vnp_SecureHash bằng thuật toán HMACSHA512",
                        "📥 Đầu vào: Chuỗi query nối từ vnpParams và khóa bí mật HashSecret. ⚙️ Cơ chế ngầm: Dùng lớp HMACSHA512 của System.Security.Cryptography để băm chuỗi dữ liệu thành chữ ký chống chỉnh sửa và gắn vào cuối URL thanh toán. 📤 Đầu ra: Đường link thanh toán VNPAY hoàn chỉnh.",
                        "public static string HmacSHA512(string key, string inputData) {\n    byte[] keyBytes = Encoding.UTF8.GetBytes(key);\n    byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);\n    using var hmac = new HMACSHA512(keyBytes);\n    byte[] hashValue = hmac.ComputeHash(inputBytes);\n    return Convert.ToHexString(hashValue).ToLower();\n}"),
                    Step(3,
                        "3. Xác thực chữ ký khi VNPAY gọi trả kết quả về (ReturnUrl & IPN)",
                        "📥 Đầu vào: Các tham số vnp_... trả về trên QueryString kèm vnp_SecureHash. ⚙️ Cơ chế ngầm: Backend tự băm lại các tham số trả về với HashSecret của mình: nếu chữ ký tự tính ra KHỚP 100% với vnp_SecureHash gửi về và vnp_ResponseCode == \"00\" thì mới cập nhật đơn hàng thành Đã thanh toán! 📤 Đầu ra: Chống giả mạo giao dịch tuyệt đối.",
                        "bool isValidSignature = ValidateSignature(Request.Query, _vnpaySettings.HashSecret);\nif (!isValidSignature) return BadRequest(\"Chữ ký giao dịch không hợp lệ (Nghi vấn giả mạo)!\");\n\nif (Request.Query[\"vnp_ResponseCode\"] == \"00\") {\n    await _orderService.MarkAsPaidAsync(orderId);\n    return Ok(\"Thanh toán thành công!\");\n}")
                },
                "Trong luồng tích hợp cổng thanh toán trực tuyến (như VNPAY hoặc MoMo), khi người dùng hoàn tất thanh toán và được chuyển hướng quay về đường dẫn ReturnUrl trên website của bạn kèm tham số vnp_ResponseCode=00, tại sao Backend của bạn BẮT BUỘC phải kiểm tra lại chữ ký băm (vnp_SecureHash) bằng khóa bí mật HashSecret trước khi cập nhật trạng thái đơn hàng thành \"Đã thanh toán\"?",
                new List<string>
                {
                    "A. Vì các tham số trên thanh địa chỉ URL hoàn toàn có thể bị người dùng tự gõ sửa trên trình duyệt (sửa mã lỗi thất bại thành 00 hoặc sửa số tiền); việc băm lại và đối chiếu vnp_SecureHash bằng khóa bí mật trên Server giúp phát hiện ngay lập tức nếu có bất kỳ ký tự nào trên URL bị chỉnh sửa giả mạo",
                    "B. Để đổi màu giao diện trang thanh toán",
                    "C. Vì SQL Server bắt buộc phải lưu chuỗi vnp_SecureHash",
                    "D. Để giảm phí giao dịch ngân hàng"
                },
                "A",
                "Không bao giờ tin tưởng tham số trên URL nếu chưa kiểm tra chữ ký số (HMAC-SHA512). Chỉ có Server của bạn và Server của VNPAY giữ khóa HashSecret, nên kẻ gian sửa bất kỳ tham số nào trên URL đều sẽ làm sai lệch vnp_SecureHash và bị chặn đứng ngay lập tức."
            ),

            // =====================================================================
            // BÀI 14.4
            // =====================================================================
            Build(
                topicId,
                "14.4",
                "Lưu trữ File & Hình ảnh trên Đám mây (Cloud Storage: Cloudinary / AWS S3 / Azure Blob Storage)",
                "Web API",
                "https://cloudinary.com/documentation/dotnet_integration",
                """
                • **💡 1. Tại sao khi đưa Web lên Server thật (Docker / Cloud), người ta không lưu ảnh người dùng upload vào thư mục `wwwroot` nữa?**
                  - Khi làm ở máy cá nhân, lưu ảnh vào `wwwroot/uploads` rất tiện. **NHƯNG** khi đưa lên Docker hoặc Cloud (như Render, Azure, AWS):
                    1. Mỗi lần bạn cập nhật phiên bản code mới (CI/CD tạo lại Container mới), các file ảnh lưu tạm bên trong Container cũ sẽ bị **xóa sạch bay màu**!
                    2. Khi công ty chạy 2 máy chủ Web song song, ảnh lưu ở ổ cứng máy chủ 1 thì người dùng vào máy chủ 2 sẽ bị lỗi không thấy ảnh (`404 Not Found`)!
                  - **Giải pháp Chuẩn Công Nghiệp — Dùng `Cloud Object Storage` (`Cloudinary`, `AWS S3` hoặc `Azure Blob Storage`):**
                Backend nhận `IFormFile` từ người dùng ➔ Bắn thẳng luồng ảnh đó lên đám mây **Cloudinary** (có gói miễn phí 25GB cực xịn cho sinh viên và dự án thực tế!) ➔ Nhận về đường link `https://res.cloudinary.com/.../book-cover.webp` (đã được tự động nén nhẹ và phân phối qua CDN toàn cầu) để lưu vào SQL Server!
                • **🖼️ 2. Sơ đồ Luồng Upload Ảnh Lên Đám Mây `Cloudinary / S3`:**
                ```
                    Người dùng chọn ảnh (3 MB) ──► Gửi IFormFile lên ASP.NET Core API
                                                           │
                                                           ▼  (Truyền thẳng luồng Stream trên RAM, không lưu rác ra ổ cứng!)
                                                  Gọi SDK Cloudinary.UploadAsync(stream)
                                                           │
                                                           ▼  (Tự động cắt khung chuẩn, đổi sang chuẩn ảnh nhẹ .webp)
                    Lưu chuỗi URL "https://res.cloudinary.com/..." vào cột ImageUrl trong SQL Server! ◄──┘
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo giao diện ICloudStorageService và cấu hình tài khoản Cloudinary",
                        "📥 Đầu vào: CloudName, ApiKey và ApiSecret lấy từ trang chủ Cloudinary. ⚙️ Cơ chế ngầm: Khởi tạo đối tượng Cloudinary thông qua IOptions<CloudinarySettings> và bọc trong interface ICloudStorageService. 📤 Đầu ra: Dịch vụ lưu trữ đám mây độc lập.",
                        "public interface ICloudStorageService {\n    Task<string> UploadImageAsync(IFormFile file, string folderName);\n}\n\npublic class CloudinaryStorageService : ICloudStorageService {\n    private readonly Cloudinary _cloudinary;\n    public CloudinaryStorageService(IOptions<CloudinarySettings> config) {\n        var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);\n        _cloudinary = new Cloudinary(account);\n    }"),
                    Step(2,
                        "2. Đẩy luồng ảnh trực tiếp từ RAM (OpenReadStream) lên Đám mây",
                        "📥 Đầu vào: Đối tượng IFormFile file người dùng gửi lên. ⚙️ Cơ chế ngầm: Dùng file.OpenReadStream() truyền thẳng lên Cloudinary kèm Transformation tự động giới hạn kích thước tối đa 800x800 giúp ảnh nhẹ và nét. 📤 Đầu ra: Nhận về đường dẫn HTTPS vĩnh viễn (SecureUrl).",
                        "    public async Task<string> UploadImageAsync(IFormFile file, string folderName) {\n        using var stream = file.OpenReadStream();\n        var uploadParams = new ImageUploadParams {\n            File = new FileDescription(file.FileName, stream),\n            Folder = folderName,\n            Transformation = new Transformation().Width(800).Crop(\"limit\").Quality(\"auto\")\n        };\n        var uploadResult = await _cloudinary.UploadAsync(uploadParams);\n        return uploadResult.SecureUrl.ToString(); // Trả về link https://res.cloudinary.com/...\n    }\n}"),
                    Step(3,
                        "3. Lưu chuỗi URL đám mây vào Database tại tầng Service",
                        "📥 Đầu vào: Đường link HTTPS trả về từ Bước 2. ⚙️ Cơ chế ngầm: Gán thẳng chuỗi URL đó vào thuộc tính book.CoverImageUrl và lưu vào SQL Server. Dù bạn xóa hay tạo mới bao nhiêu Docker Container thì ảnh trên đám mây vẫn còn nguyên vẹn mãi mãi! 📤 Đầu ra: Hiển thị ảnh siêu tốc qua CDN.",
                        "string imageUrl = await _cloudStorage.UploadImageAsync(dto.CoverFile, \"hoangviet_books\");\nbookEntity.CoverImageUrl = imageUrl;\nawait _unitOfWork.SaveChangesAsync();")
                },
                "Khi triển khai ứng dụng web dưới dạng Docker Container hoặc chạy nhiều máy chủ song song trên Cloud, tại sao việc lưu trữ hình ảnh người dùng tải lên vào các dịch vụ đám mây chuyên dụng (như Cloudinary, AWS S3, Azure Blob Storage) lại ưu việt hơn hẳn việc lưu trực tiếp vào thư mục wwwroot bên trong Container?",
                new List<string>
                {
                    "A. Vì ảnh lưu trên Cloud Storage được bảo toàn độc lập (không bị mất khi cập nhật hay xóa Container), mọi máy chủ Web đều truy cập chung được một nguồn ảnh thống nhất, đồng thời tận dụng được mạng lưới CDN giúp người dùng tải ảnh nhanh hơn và giảm tải băng thông cho máy chủ C#",
                    "B. Vì thư mục wwwroot không chứa được file đuôi .png",
                    "C. Vì SQL Server không lưu được chuỗi bắt đầu bằng chữ https",
                    "D. Để không cần dùng thẻ <img> trong HTML nữa"
                },
                "A",
                "Tách biệt phần lưu trữ file tĩnh (Stateless File Storage) ra dịch vụ Cloud Storage (Cloudinary/S3/Blob) là kiến trúc bắt buộc để ứng dụng Web có thể triển khai trên Docker/Cloud và mở rộng quy mô (Scale-out) dễ dàng."
            ),

            // =====================================================================
            // BÀI 14.5
            // =====================================================================
            Build(
                topicId,
                "14.5",
                "Bộ nhớ đệm Phân tán với Redis (IDistributedCache) - Tăng tốc Hệ thống Lớn & Chia sẻ Cache giữa nhiều Server",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/aspnet/core/performance/caching/distributed",
                """
                • **💡 1. Khi nào `IMemoryCache` (Chủ đề 9) là chưa đủ và phải nâng cấp lên `Redis (IDistributedCache)`?**
                  - **`IMemoryCache` (Bộ nhớ đệm cục bộ):** Lưu trực tiếp trên thanh RAM của chính máy chủ đang chạy code C#.
                + *Nhược điểm:* Nếu bạn tắt bật lại web (Restart/Deploy), toàn bộ Cache trên RAM bị xóa sạch! Và nếu công ty chạy 3 máy chủ Web (`Server A, B, C`), dữ liệu lưu trên RAM của `Server A` thì `Server B` không nhìn thấy được!
                  - **`Redis` (`IDistributedCache` - Bộ nhớ đệm Phân tán độc lập):**
                • **Redis** là một máy chủ cơ sở dữ liệu dạng `Key - Value` chạy **100% trên RAM** nằm tách biệt bên ngoài (chạy qua Docker chỉ mất 5 giây!).
                + Tất cả các máy chủ Web C# đều kết nối chung vào 1 trạm **Redis** duy nhất: Lưu 1 lần, mọi Server cùng đọc được với tốc độ **dưới 1 mili-giây**, và dù bạn có khởi động lại ứng dụng C# thì dữ liệu trong Redis vẫn còn nguyên!
                • **🖼️ 2. Sơ đồ So Sánh `In-Memory Cache` vs `Redis Distributed Cache`:**
                ```
                    [1. IMemoryCache (Cục bộ trên từng máy)]:
                    Web Server 1 (RAM riêng)   │   Web Server 2 (RAM riêng) ──► Dữ liệu 2 bên lệch nhau! Restart web là mất!
                
                    [2. Redis Distributed Cache (Tập trung, siêu tốc, bền bỉ)]:
                    Web Server 1 ──┐
                                   ├──(Đọc/Ghi tốc độ 0.5ms)──► ⚡ [ REDIS SERVER (Cổng 6379) ]
                    Web Server 2 ──┘                            (Lưu Cache JSON, Giỏ hàng, Mã OTP, Blacklist Token!)
                ```
                • **📊 3. Bốn bài toán thực tế luôn dùng `Redis` trong các công ty lớn:**
                    1. **Lưu đệm danh sách dữ liệu nóng (Hot Data Caching):** Trang chủ, Danh mục, Bảng xếp hạng.
                    2. **Lưu mã xác thực OTP có hạn 3 phút:** Gán `TTL (Time-To-Live) = 180 giây` — hết 3 phút Redis **tự động xóa biến mất mã OTP** mà bạn không cần viết code dọn dẹp!
                    3. **Lưu Giỏ hàng tạm thời (Shopping Cart)** của khách hàng.
                    4. **Danh sách thu hồi Token (`JWT Blacklist`)** khi người dùng bấm Đăng xuất trước hạn.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Đăng ký kết nối Redis (AddStackExchangeRedisCache) trong Program.cs",
                        "📥 Đầu vào: Gói NuGet Microsoft.Extensions.Caching.StackExchangeRedis. ⚙️ Cơ chế ngầm: Cấu hình chuỗi kết nối tới máy chủ Redis (localhost:6379) và đặt tiền tố InstanceName để phân biệt khóa của dự án. 📤 Đầu ra: Tiêm được giao diện chuẩn IDistributedCache.",
                        "builder.Services.AddStackExchangeRedisCache(options => {\n    options.Configuration = builder.Configuration.GetConnectionString(\"RedisConnection\"); // \"localhost:6379\"\n    options.InstanceName = \"HoangViet_\";\n});"),
                    Step(2,
                        "2. Viết hàm đọc/ghi dữ liệu vào Redis bằng IDistributedCache",
                        "📥 Đầu vào: Giao diện IDistributedCache _redisCache. ⚙️ Cơ chế ngầm: Kiểm tra khóa bằng GetStringAsync(key): nếu có sẵn trong Redis thì dịch từ JSON ra trả về ngay; nếu chưa có mới gọi SQL Server rồi dùng SetStringAsync lưu vào Redis 15 phút! 📤 Đầu ra: Tăng tốc API gấp 50 lần.",
                        "string cacheKey = $\"top_books_v1\";\nstring? cachedJson = await _redisCache.GetStringAsync(cacheKey, ct);\n\nif (!string.IsNullOrEmpty(cachedJson)) {\n    return JsonSerializer.Deserialize<List<BookDto>>(cachedJson)!; // Lấy thẳng từ Redis!\n}\n\n// Nếu chưa có trong Redis -> Hỏi SQL Server:\nvar books = await _bookRepo.GetTop10BooksAsync(ct);\nvar options = new DistributedCacheEntryOptions {\n    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)\n};\nawait _redisCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(books), options, ct);\nreturn books;"),
                    Step(3,
                        "3. Ứng dụng Redis lưu mã OTP xác thực tự hủy sau 3 phút",
                        "📥 Đầu vào: Người dùng yêu cầu gửi mã OTP quên mật khẩu. ⚙️ Cơ chế ngầm: Lưu mã OTP 6 số vào Redis với khóa 'OTP_{email}' và đặt hạn sống đúng 3 phút. Sau 180 giây, khóa này tự bốc hơi khỏi Redis! 📤 Đầu ra: Không cần tạo bảng rác lưu OTP trong SQL Server.",
                        "string otpCode = Random.Shared.Next(100000, 999999).ToString();\nawait _redisCache.SetStringAsync(\n    $\"OTP_{email}\",\n    otpCode,\n    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3) });")
                },
                "Khi xây dựng tính năng gửi Mã xác thực OTP (có hiệu lực trong đúng 3 phút) qua Email hoặc SMS, tại sao các kiến trúc sư phần mềm luôn chọn lưu mã OTP này vào Redis (IDistributedCache) thay vì lưu vào một bảng trong SQL Server?",
                new List<string>
                {
                    "A. Vì Redis hoạt động trên bộ nhớ RAM có tốc độ truy xuất tức thì và hỗ trợ cơ chế TTL (Time-To-Live - AbsoluteExpirationRelativeToNow): đúng sau 3 phút Redis sẽ tự động xóa sạch mã OTP đó khỏi bộ nhớ mà không làm bẩn hay tốn công chạy lệnh DELETE trong SQL Server",
                    "B. Vì SQL Server không lưu được con số có 6 chữ số",
                    "C. Vì Redis tự động gửi SMS miễn phí",
                    "D. Vì dùng Redis không cần viết code C#"
                },
                "A",
                "Cơ chế tự động hết hạn và tự hủy dữ liệu (TTL / Expiration) chính xác đến từng mili-giây của Redis khiến nó trở thành công cụ hoàn hảo số 1 để lưu mã OTP, Phiên đăng nhập tạm thời và Rate Limiting."
            ),

            // =====================================================================
            // BÀI 14.6
            // =====================================================================
            Build(
                topicId,
                "14.6",
                "Xử lý Tác vụ Chạy ngầm & Đặt lịch tự động (Background Jobs) với BackgroundService và Thư viện Hangfire",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/host/hosted-services",
                """
                • **💡 1. Ba loại công việc "Chạy ngầm ở hậu trường" (`Background Jobs`) mà mọi dự án thực tế đều cần:**
                    1. **`Fire-and-Forget Jobs` (Bắn đi rồi chạy ngầm ngay 1 lần):** Khi người dùng vừa bấm nút *"Đăng ký tài khoản"* hoặc *"Đặt mượn sách"*, bạn đẩy việc **Gửi Email xác nhận** cho hệ thống chạy ngầm làm, còn API lập tức trả kết quả thành công cho người dùng trong `0.05 giây` không bắt họ đứng chờ!
                    2. **`Delayed Jobs` (Hẹn giờ chạy sau một khoảng thời gian):** *"Sau khi người dùng tạo đơn hàng đúng **15 phút**, hãy tự động kiểm tra xem họ đã thanh toán chưa; nếu chưa thanh toán thì tự động hủy đơn hoàn lại sách vào kho!"*.
                    3. **`Recurring Jobs` (Lặp lại định kỳ hàng ngày/hàng tuần - `Cron Job`):** *"Cứ **đúng 07:00 sáng mỗi ngày**, hãy tự động quét bảng Phiếu mượn xem ai sắp hết hạn trả sách thì tự động gửi Email nhắc nhở!"*.
                • **🖼️ 2. Tại sao thư viện `Hangfire` lại được ưa chuộng số 1 trong các dự án .NET doanh nghiệp?**
                ```
                    So sánh giữa BackgroundService thuần của .NET và Thư viện Hangfire:
                    ├── BackgroundService thuần: Nếu đang chờ gửi mail mà Server bị mất điện/restart ──► Công việc trong RAM mất trắng!
                    └── Thư viện HANGFIRE:
                        1. Lưu danh sách các công việc cần làm vào thẳng SQL Server (Không bao giờ sợ mất việc khi tắt máy!).
                        2. Nếu gửi Email lần 1 bị lỗi mạng ──► Hangfire TỰ ĐỘNG THỬ LẠI (Auto-Retry) tối đa 10 lần!
                        3. Tặng kèm sẵn một trang Giao diện Quản trị tuyệt đẹp tại đường dẫn "/hangfire" (Biểu đồ xem việc nào
                           đang chạy, việc nào lỗi, bấm nút chạy lại chỉ bằng 1 cú click chuột!).
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cấu hình Hangfire lưu trữ công việc vào SQL Server và mở Dashboard",
                        "📥 Đầu vào: Cài gói NuGet Hangfire.AspNetCore và Hangfire.SqlServer. ⚙️ Cơ chế ngầm: Đăng ký AddHangfire nối vào chuỗi kết nối SQL Server, bật AddHangfireServer() và mở giao diện theo dõi tại '/hangfire'. 📤 Đầu ra: Hệ thống quản lý tác vụ ngầm kèm giao diện giám sát.",
                        "builder.Services.AddHangfire(config => config\n    .UseSimpleAssemblyNameTypeSerializer()\n    .UseRecommendedSerializerSettings()\n    .UseSqlServerStorage(builder.Configuration.GetConnectionString(\"DefaultConnection\")));\nbuilder.Services.AddHangfireServer();\n// ...\napp.UseHangfireDashboard(\"/hangfire\"); // Mở trang quản lý tiến trình chạy ngầm!"),
                    Step(2,
                        "2. Dùng Fire-and-Forget (Enqueue) và Delayed Job (Schedule)",
                        "📥 Đầu vào: Tiêm IBackgroundJobClient _jobClient vào BorrowService. ⚙️ Cơ chế ngầm: Dùng .Enqueue() để đẩy việc gửi mail chạy ngầm tức thì, và dùng .Schedule() để hẹn giờ sau 24 tiếng tự động kiểm tra phiếu mượn! 📤 Đầu ra: API phản hồi siêu tốc.",
                        "// 1. Chạy ngầm ngay lập tức (Không bắt người dùng chờ gửi mail):\n_jobClient.Enqueue<IEmailService>(x => x.SendEmailAsync(user.Email, \"Xác nhận mượn sách\", \"Thành công!\"));\n\n// 2. Hẹn giờ chạy sau 24 giờ nữa:\n_jobClient.Schedule<IBorrowService>(x => x.CheckUnpickedBorrowAsync(borrow.Id), TimeSpan.FromHours(24));"),
                    Step(3,
                        "3. Đặt lịch chạy tự động lúc 7h00 sáng hàng ngày bằng RecurringJob (Cron)",
                        "📥 Đầu vào: Hàm quét và gửi thông báo nhắc trả sách quá hạn. ⚙️ Cơ chế ngầm: Dùng RecurringJob.AddOrUpdate với biểu thức Cron.Daily(7, 0) để hệ thống tự động thức dậy chạy đúng 7h00 sáng mỗi ngày! 📤 Đầu ra: Tự động hóa vận hành 100%.",
                        "RecurringJob.AddOrUpdate<IBorrowService>(\n    \"daily-overdue-reminder\",\n    service => service.SendOverdueRemindersAsync(),\n    Cron.Daily(7, 0) // Tự động chạy vào 07:00 sáng mỗi ngày!\n);")
                },
                "Ưu điểm vượt trội lớn nhất của việc sử dụng thư viện Hangfire (so với việc chỉ chạy Task.Run trên bộ nhớ RAM) khi xử lý các tác vụ chạy ngầm quan trọng như gửi Email hóa đơn hoặc hẹn giờ tự động hủy đơn hàng là gì?",
                new List<string>
                {
                    "A. Hangfire lưu trữ bền vững thông tin các tác vụ vào Cơ sở dữ liệu (SQL Server/Redis), tự động thử lại (Auto-Retry) khi gặp lỗi mạng tạm thời, không bị mất tác vụ khi Server khởi động lại và cung cấp sẵn giao diện Dashboard trực quan tại /hangfire để theo dõi",
                    "B. Hangfire không cần dùng CPU để chạy",
                    "C. Hangfire giúp viết code HTML không cần thẻ đóng",
                    "D. Hangfire thay thế hoàn toàn cho Entity Framework Core"
                },
                "A",
                "Nếu dùng Task.Run thông thường trên RAM (Fire-and-forget không lưu trữ), khi ứng dụng web bị khởi động lại hoặc gặp lỗi mạng chập chờn trong 2 giây, công việc gửi mail đó sẽ biến mất vĩnh viễn mà không ai biết. Hangfire đảm bảo độ tin cậy 100% nhờ lưu trạng thái vào Database và cơ chế Auto-Retry."
            ),

            // =====================================================================
            // BÀI 14.7
            // =====================================================================
            Build(
                topicId,
                "14.7",
                "Xây dựng Kết nối Bền bỉ với Dịch vụ Ngoài (Resilience, Retry, Circuit Breaker & Timeout) bằng Microsoft.Extensions.Http.Resilience (Polly)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/core/resilience/http-resilience",
                """
                • **💡 1. Thực tế phũ phàng khi gọi API bên thứ ba: "Mạng Internet luôn có lúc chập chờn 1-2 giây!"**
                  - Khi Backend của bạn gọi sang API của Google Gemini, VNPAY, hay GiaoHangNhanh, sẽ có những lúc mạng bị nghẽn nhẹ trong `0.5 giây` (**Transient Fault** - Lỗi thoáng qua).
                  - Nếu bạn không có cơ chế bảo vệ: Chỉ vì mạng chập chờn nửa giây mà ứng dụng của bạn báo lỗi sập đơn hàng của khách luôn! Nhưng nếu chỉ cần **tự động chờ 1 giây rồi gọi thử lại lần 2 (`Retry`)** là lại thành công mỹ mãn!
                • **🖼️ 2. Bộ 3 Cơ Chế Tự Phục Hồi (`Resilience Pipeline` / Thư viện `Polly`) Tích Hợp Sẵn Trong .NET 8/9/10:**
                ```
                    Khi HttpClient của bạn gọi sang Dịch vụ bên thứ ba:
                    ┌────────────────────────────────────────────────────────────────────────────┐
                    │ 1. ⏱️ Timeout (Giới hạn chờ): Quá 10 giây không trả lời ──► Ngắt ngay!     │
                    ├────────────────────────────────────────────────────────────────────────────┤
                    │ 2. 🔄 Retry (Thử lại thông minh - Exponential Backoff):                    │
                    │       Lỗi lần 1 ──► Chờ 1 giây thử lại                                     │
                    │       Lỗi lần 2 ──► Chờ 2 giây thử lại                                     │
                    │       Lỗi lần 3 ──► Chờ 4 giây thử lại (Tránh dồn dập làm sập server họ!)  │
                    ├────────────────────────────────────────────────────────────────────────────┤
                    │ 3. 🔌 Circuit Breaker (Cầu dao tự ngắt an toàn):                           │
                    │       Nếu gọi 10 lần liên tiếp đều sập (Server bên kia đã chết hẳn rồi!)   │
                    │       ──► Tự động "Dập cầu dao" ngưng gọi trong 30 giây để bảo vệ hệ thống!│
                    └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Tin vui trong .NET mới (8/9/10): Kích hoạt cả 3 cơ chế trên chỉ bằng ĐÚNG 1 DÒNG LỆNH!**
                  - Chỉ cần cài gói chính chủ `Microsoft.Extensions.Http.Resilience` và gắn đuôi **`.AddStandardResilienceHandler()`** vào sau `AddHttpClient()`!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Gắn bộ bảo vệ tiêu chuẩn AddStandardResilienceHandler cho HttpClient",
                        "📥 Đầu vào: Đăng ký AddHttpClient gọi sang dịch vụ AI hoặc Giao hàng bên ngoài. ⚙️ Cơ chế ngầm: Chỉ với 1 lệnh .AddStandardResilienceHandler(), .NET tự động trang bị sẵn cả 3 lớp giáp: Timeout, Retry (3 lần theo cấp số nhân) và Circuit Breaker! 📤 Đầu ra: Kết nối mạng siêu bền bỉ.",
                        "// Trong Program.cs (Cài gói Microsoft.Extensions.Http.Resilience):\nbuilder.Services.AddHttpClient<IGeminiAiService, GeminiAiService>()\n    .AddStandardResilienceHandler(); // Tự động bật Retry + Circuit Breaker + Timeout!"),
                    Step(2,
                        "2. Tùy chỉnh chi tiết số lần Retry và thời gian chờ theo ý muốn",
                        "📥 Đầu vào: Muốn tùy chỉnh tối đa 3 lần thử lại, mỗi lần cách nhau theo cấp số nhân (2s, 4s, 8s). ⚙️ Cơ chế ngầm: Cấu hình thuộc tính Retry.MaxRetryAttempts và BackoffType = DelayBackoffType.Exponential. 📤 Đầu ra: Kiểm soát hoàn toàn chiến lược thử lại.",
                        "builder.Services.AddHttpClient<IShippingClient, ShippingClient>()\n    .AddStandardResilienceHandler(options => {\n        options.Retry.MaxRetryAttempts = 3;\n        options.Retry.Delay = TimeSpan.FromSeconds(2);\n        options.Retry.BackoffType = Polly.DelayBackoffType.Exponential;\n    });"),
                    Step(3,
                        "3. Cơ chế Dự phòng (Fallback) khi dịch vụ bên ngoài sập hoàn toàn",
                        "📥 Đầu vào: Cả 3 lần Retry đều thất bại do máy chủ bên thứ ba đang bảo trì. ⚙️ Cơ chế ngầm: Bắt ngoại lệ ở tầng Service và trả về dữ liệu dự phòng từ bộ nhớ Cache (hoặc câu trả lời lịch sự mặc định) để ứng dụng của bạn vẫn hoạt động êm ái! 📤 Đầu ra: Không bao giờ bị sập dây chuyền.",
                        "try {\n    return await _externalApi.GetExchangeRateAsync();\n} catch (Exception ex) {\n    _logger.LogWarning(ex, \"API tỷ giá bên ngoài đang bảo trì, sử dụng tỷ giá dự phòng từ Cache!\");\n    return _cache.Get<decimal>(\"FALLBACK_RATE\");\n}")
                },
                "Trong kiến trúc kết nối với các API bên thứ ba sử dụng thư viện Polly (ResilienceHandler), cơ chế \"Circuit Breaker\" (Cầu dao ngắt mạch) hoạt động như thế nào và nhằm mục đích gì?",
                new List<string>
                {
                    "A. Khi phát hiện dịch vụ bên thứ ba liên tục báo lỗi vượt ngưỡng cho phép (ví dụ sập máy chủ), Circuit Breaker sẽ tự động 'ngắt mạch' ngừng gửi Request sang đó trong một khoảng thời gian (ví dụ 30 giây) và trả lỗi nhanh ngay lập tức, giúp ứng dụng của ta không bị treo hàng nghìn luồng vô ích và giúp server bên kia có thời gian hồi phục",
                    "B. Tự động tắt nguồn máy tính khi có lỗi biên dịch",
                    "C. Xóa sạch dữ liệu trong Database khi mất mạng",
                    "D. Gửi hàng triệu Request liên tục mỗi giây để ép server bên kia phải trả lời"
                },
                "A",
                "Giống như chiếc aptomat (cầu dao điện) trong nhà: khi thiết bị bên ngoài bị chập cháy, Circuit Breaker tự động ngắt kết nối tạm thời để ngăn đám cháy lan sang làm treo cứng toàn bộ hệ thống Server của bạn."
            ),

            // =====================================================================
            // BÀI 14.8
            // =====================================================================
            Build(
                topicId,
                "14.8",
                "Đọc và Xử lý Webhook từ Bên thứ ba (GitHub, Stripe, PayOS, Cổng thanh toán) & Đảm bảo tính Idempotency",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/webhooks/",
                """
                • **💡 1. Hiểu bản chất của `Webhook` qua hình ảnh đời thường trong 30 giây:**
                  - Giả sử bạn gửi quần áo ra tiệm giặt là:
                • **Cách 1 - `Polling` (Hỏi thăm liên tục):** Cứ 5 phút bạn lại phải gọi điện hỏi chủ tiệm: *"Quần áo của tôi giặt xong chưa?"* ➔ Rất phiền và tốn công cả hai bên!
                • **Cách 2 - `Webhook` (Để lại số điện thoại để tiệm chủ động gọi báo khi xong):** Bạn để lại một đường link API của bạn (ví dụ: `POST https://hoangviet.vn/api/webhooks/payment`) cho đối tác (Ngân hàng / VietQR / PayOS / Stripe / GitHub). Ngay khi khách hàng vừa chuyển khoản xong, **máy chủ của Ngân hàng sẽ tự động gọi ngược vào đường link Webhook đó của bạn** để báo tin tức thì!
                • **🖼️ 2. Hai Nguyên Tắc Sống Còn Khi Viết Một API Nhận `Webhook` Trong ASP.NET Core:**
                ```
                    Đối tác bên ngoài (Ngân hàng / Cổng thanh toán) bắn HTTP POST vào Webhook của bạn:
                          │
                          ▼
                    🛡️ Nguyên tắc 1 - Xác thực Chữ ký (Signature Verification):
                       Bắt buộc kiểm tra Header chữ ký mật mã (X-Hub-Signature / Checksum) để chắc chắn 100%
                       gói tin này là do Ngân hàng thật gửi tới chứ không phải do Hacker giả mạo bắn vào!
                          │
                          ▼
                    🛡️ Nguyên tắc 2 - Chống xử lý trùng lặp (Idempotency Check - CỰC KỲ QUAN TRỌNG!):
                       Do mạng chập chờn, máy chủ Ngân hàng có thể gửi lặp lại cùng 1 gói tin Webhook 2-3 lần!
                       ──► Phải kiểm tra mã giao dịch (TransactionId / EventId) trong DB:
                           Nếu đơn hàng đã ở trạng thái "Đã thanh toán" rồi ──► Trả về 200 OK luôn, KHÔNG cộng tiền lần 2!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Mở API Endpoint nhận Webhook ([AllowAnonymous] + Kiểm tra Chữ ký)",
                        "📥 Đầu vào: Máy chủ bên thứ ba gọi POST vào '/api/webhooks/payment'. ⚙️ Cơ chế ngầm: Vì máy chủ đối tác không đăng nhập bằng tài khoản User của web bạn, nên hàm Webhook phải để [AllowAnonymous] và thay bằng bước kiểm tra chữ ký HMAC trên Header! 📤 Đầu ra: Xác thực đúng nguồn đối tác.",
                        "[AllowAnonymous]\n[HttpPost(\"api/webhooks/payment\")]\npublic async Task<IActionResult> ReceivePaymentWebhook(\n    [FromBody] PaymentWebhookPayload payload,\n    [FromHeader(Name = \"X-Webhook-Signature\")] string signature) {\n\n    if (!_webhookVerifier.IsValid(payload, signature))\n        return Unauthorized(\"Chữ ký Webhook không hợp lệ!\");"),
                    Step(2,
                        "2. Kiểm tra chống trùng lặp giao dịch (Idempotency Check)",
                        "📥 Đầu vào: Mã giao dịch duy nhất payload.TransactionId. ⚙️ Cơ chế ngầm: Kiểm tra trong bảng PaymentTransactions xem mã giao dịch này đã từng được xử lý trước đó chưa. Nếu đã xử lý rồi thì trả về Ok() ngay lập tức! 📤 Đầu ra: Không bao giờ bị cộng tiền 2 lần cho 1 giao dịch.",
                        "    bool alreadyProcessed = await _db.PaymentTransactions\n        .AnyAsync(t => t.ExternalTransactionId == payload.TransactionId);\n\n    if (alreadyProcessed) {\n        _logger.LogInformation(\"Giao dịch {TxId} đã được xử lý trước đó, bỏ qua!\", payload.TransactionId);\n        return Ok(); // Vẫn trả 200 OK để đối tác ngừng gửi lại!\n    }"),
                    Step(3,
                        "3. Lưu giao dịch trong Transaction và phản hồi 200 OK nhanh chóng",
                        "📥 Đầu vào: Giao dịch mới hợp lệ. ⚙️ Cơ chế ngầm: Ghi nhận giao dịch vào DB, cập nhật trạng thái đơn hàng và trả về mã 200 OK ngay để máy chủ đối tác biết bạn đã nhận tin thành công. 📤 Đầu ra: Hoàn tất chu trình thanh toán tự động.",
                        "    await _orderService.ConfirmPaymentWebhookAsync(payload);\n    return Ok(new { received = true });\n}")
                },
                "Khi xây dựng một API nhận Webhook báo kết quả chuyển khoản thành công từ ngân hàng/cổng thanh toán, nếu máy chủ đối tác gửi lặp lại gói tin của cùng một giao dịch TransactionId = \"TX999\" lần thứ hai (do lần đầu phản hồi mạng hơi chậm), hàm Webhook của bạn nên xử lý như thế nào?",
                new List<string>
                {
                    "A. Kiểm tra thấy mã 'TX999' đã được xử lý thành công trong Database rồi thì bỏ qua bước cộng tiền/duyệt đơn lặp lại, và trả về ngay mã 200 OK để báo cho máy chủ đối tác biết đã nhận được thông tin (Đảm bảo tính Idempotency)",
                    "B. Cộng thêm tiền cho khách hàng một lần nữa",
                    "C. Xóa đơn hàng của khách đi",
                    "D. Cho sập Server để đối tác gửi tiếp lần thứ 3"
                },
                "A",
                "Trong kiến trúc Webhook, đối tác áp dụng cơ chế At-least-once delivery (Gửi ít nhất 1 lần cho đến khi nhận được mã 200 OK). Do đó hàm nhận Webhook bắt buộc phải kiểm tra trùng TransactionId (Idempotency) và trả về 200 OK nếu đã xử lý rồi."
            ),

            // =====================================================================
            // BÀI 14.9
            // =====================================================================
            Build(
                topicId,
                "14.9",
                "Đăng nhập Mạng xã hội (OAuth2 Google / GitHub) & Gọi REST API Đối tác bằng Refit (Declarative Type-Safe REST Client)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/core/extensions/httpclient-factory#generated-clients",
                """
                • **💡 1. Vũ khí bí mật giúp gọi API bên thứ ba nhanh gấp 5 lần: Thư viện `Refit`!**
                  - Bình thường khi cần gọi 10 API của đối tác bên ngoài (như API Tỉnh/Thành phố, API GiaoHangNhanh), bạn phải viết một class `ShippingClient` dài hàng trăm dòng chứa các lệnh `_http.GetFromJsonAsync(...)`, `_http.PostAsJsonAsync(...)` lặp đi lặp lại.
                  - Với thư viện **`Refit`** (thư viện chuẩn được cộng đồng .NET và Microsoft cực kỳ yêu thích): **Bạn chỉ cần viết duy nhất một `Interface` có gắn `[Get]`, `[Post]`**, Refit sẽ **tự động sinh toàn bộ code thực thi `HttpClient` ngầm ở bên dưới lúc biên dịch**!
                • **🖼️ 2. Sơ đồ Sự Kỳ Diệu Của `Refit` (Biến `Interface` thành `API Client` chạy được ngay!):**
                ```csharp
                    // 1. Bạn CHỈ CẦN viết đúng 1 Interface như thế này (KHÔNG CẦN viết Class thực thi!):
                    public interface IGitHubApi
                    {
                        [Get("/users/{username}")]
                        Task<GitHubUserDto> GetUserAsync(string username);
                    }
                
                    // 2. Đăng ký 1 dòng trong Program.cs:
                    builder.Services.AddRefitClient<IGitHubApi>()
                           .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.github.com"));
                
                    // 3. Tiêm thẳng IGitHubApi vào Service/Controller và chấm gọi luôn!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Định nghĩa hợp đồng gọi API bên ngoài bằng Interface của Refit",
                        "📥 Đầu vào: Cần gọi API lấy thông tin hành chính hoặc thông tin tài khoản GitHub/Google. ⚙️ Cơ chế ngầm: Dùng các Attribute [Get], [Post], [Body], [Header] của gói NuGet Refit.HttpClientFactory ngay trên chữ ký hàm của Interface. 📤 Đầu ra: Không cần viết 1 dòng code HttpClient thủ công nào.",
                        "using Refit;\n\npublic interface IProvinceExternalApi {\n    [Get(\"/api/p/\")]\n    Task<List<ProvinceDto>> GetAllProvincesAsync();\n\n    [Get(\"/api/p/{code}?depth=2\")]\n    Task<ProvinceDetailDto> GetDistrictsByProvinceAsync(int code);\n}"),
                    Step(2,
                        "2. Kết hợp Refit + Polly Resilience trong Program.cs",
                        "📥 Đầu vào: Giao diện IProvinceExternalApi. ⚙️ Cơ chế ngầm: Đăng ký AddRefitClient<IProvinceExternalApi>() nối tiếp với .AddStandardResilienceHandler() để vừa tự sinh code gọi API vừa có sẵn cơ chế tự động thử lại khi lỗi mạng! 📤 Đầu ra: Cặp đôi hoàn hảo nhất trong .NET.",
                        "builder.Services.AddRefitClient<IProvinceExternalApi>()\n    .ConfigureHttpClient(c => c.BaseAddress = new Uri(\"https://provinces.open-api.vn\"))\n    .AddStandardResilienceHandler(); // Kết hợp cả Refit và Polly Resilience!"),
                    Step(3,
                        "3. Tiêm Interface vào Service/Controller và sử dụng trực tiếp",
                        "📥 Đầu vào: Tiêm IProvinceExternalApi _provinceApi vào Constructor. ⚙️ Cơ chế ngầm: Gọi hàm như gọi một hàm C# nội bộ bình thường. 📤 Đầu ra: Code sạch sẽ, ngắn gọn tối đa.",
                        "public class AddressService {\n    private readonly IProvinceExternalApi _provinceApi;\n    public AddressService(IProvinceExternalApi provinceApi) => _provinceApi = provinceApi;\n\n    public async Task<List<ProvinceDto>> GetProvincesAsync() {\n        return await _provinceApi.GetAllProvincesAsync();\n    }\n}")
                },
                "Khi sử dụng thư viện Refit (Refit.HttpClientFactory) để tích hợp gọi các REST API bên thứ ba trong dự án ASP.NET Core, lập trình viên cần phải viết những gì?",
                new List<string>
                {
                    "A. Chỉ cần khai báo một Interface định nghĩa các chữ ký hàm kèm Attribute đường dẫn ([Get], [Post]...) và các DTO; Refit sẽ tự động sinh toàn bộ class thực thi gọi HttpClient và giải mã JSON ở hậu trường",
                    "B. Phải tự viết câu lệnh mở kết nối Socket TCP bằng tay",
                    "C. Phải viết code bằng ngôn ngữ Assembly",
                    "D. Phải tự cắt chuỗi JSON bằng vòng lặp for"
                },
                "A",
                "Refit được chính Microsoft giới thiệu trong tài liệu IHttpClientFactory (Generated Clients) giúp biến việc gọi REST API bên ngoài thành các lời gọi phương thức C# định kiểu mạnh (Type-safe) mà không tốn công viết code lặp lại (Boilerplate code)."
            ),

            // =====================================================================
            // BÀI 14.10
            // =====================================================================
            Build(
                topicId,
                "14.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 14 (Ghép nối Đa dịch vụ trong Luồng Đặt hàng / Mượn sách Thực tế)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/",
                """
                • **💡 1. Bản Đồ Tổng Hợp Cách Một Dự Án Doanh Nghiệp Phối Hợp Các Dịch Vụ Bên Thứ Ba:**
                Hãy nhìn vào một luồng nghiệp vụ hoàn chỉnh khi tích hợp toàn bộ vũ khí của Chủ đề 14:
                ```
                    Khách hàng tạo sản phẩm / đặt đơn hàng:
                      ├── 1. Ảnh đính kèm        ──► Đẩy lên đám mây [Cloudinary / AWS S3] qua ICloudStorageService
                      ├── 2. Dữ liệu danh mục    ──► Đọc siêu tốc từ [Redis Distributed Cache] (0.5ms)
                      ├── 3. Thanh toán          ──► Sinh link chữ ký HMAC-SHA512 chuyển sang [VNPAY / VietQR]
                      ├── 4. Nhận kết quả ngầm   ──► Hứng tại cổng [Webhook IPN] (có kiểm tra chữ ký & Idempotency)
                      └── 5. Gửi Email & PDF     ──► Đẩy vào hàng đợi ngầm [Hangfire] ──► Gửi qua [MailKit SMTP] (có Polly Retry)!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tải ảnh lên Cloudinary & Lưu thông tin vào SQL Server",
                        "📥 Đầu vào: Yêu cầu tạo sách mới kèm file ảnh bìa. ⚙️ Cơ chế ngầm: Gọi ICloudStorageService đẩy ảnh lên đám mây lấy URL, lưu bản ghi vào SQL Server và xóa khóa Cache cũ trong Redis. 📤 Đầu ra: Dữ liệu đồng bộ giữa Cloud Storage, SQL và Redis.",
                        "string coverUrl = await _cloudStorage.UploadImageAsync(dto.CoverImage, \"books\");\nvar book = new Book { Title = dto.Title, Price = dto.Price, CoverUrl = coverUrl };\nawait _unitOfWork.Books.AddAsync(book);\nawait _unitOfWork.SaveChangesAsync();\nawait _redisCache.RemoveAsync(\"TOP_BOOKS_CACHE\");"),
                    Step(2,
                        "2. Tạo URL Thanh toán VNPAY có chữ ký số HMAC-SHA512",
                        "📥 Đầu vào: Đơn hàng cần thanh toán phí cọc hoặc mua sách. ⚙️ Cơ chế ngầm: Sắp xếp tham số vào SortedList, nhân 100 số tiền, ký bằng HashSecret và trả về URL chuyển hướng. 📤 Đầu ra: Link thanh toán an toàn.",
                        "string paymentUrl = _vnpayService.CreatePaymentUrl(order.Id, order.TotalAmount, clientIp);\nreturn Ok(new { redirectUrl = paymentUrl });"),
                    Step(3,
                        "3. Xử lý Webhook IPN xác nhận thanh toán chống trùng lặp",
                        "📥 Đầu vào: Tín hiệu gọi về từ cổng thanh toán. ⚙️ Cơ chế ngầm: Kiểm tra chữ ký vnp_SecureHash, kiểm tra Idempotency (đơn chưa thanh toán mới xử lý) và chốt trạng thái đơn hàng. 📤 Đầu ra: Cập nhật trạng thái chính xác 100%.",
                        "if (order.Status == OrderStatus.Paid) return Ok(); // Idempotency: Đã xử lý rồi thì bỏ qua!\norder.Status = OrderStatus.Paid;\nawait _unitOfWork.SaveChangesAsync();"),
                    Step(4,
                        "4. Đẩy tác vụ gửi Email kèm Hóa đơn PDF vào Hangfire Background Job",
                        "📥 Đầu vào: Đơn hàng vừa thanh toán xong ở Bước 3. ⚙️ Cơ chế ngầm: Dùng _jobClient.Enqueue đẩy việc sinh PDF và gửi MailKit xuống chạy ngầm ở hậu trường để trả kết quả ngay lập tức! 📤 Đầu ra: Hệ thống phản hồi tức thì, Email tự động gửi tới khách hàng.",
                        "_backgroundJobClient.Enqueue<IOrderNotificationJob>(\n    job => job.SendInvoiceEmailWithPdfAsync(order.Id));")
                },
                "(Câu hỏi tổng hợp Chủ đề 14) Trong một luồng Đặt hàng thương mại điện tử, sau khi khách hàng thanh toán thành công, hệ thống cần Tạo file PDF hóa đơn và Gửi Email qua SMTP Gmail (mất khoảng 3 đến 4 giây). Cách thiết kế kiến trúc nào dưới đây giúp người dùng nhận được phản hồi trên màn hình nhanh nhất (dưới 50ms) và không bị mất Email nếu mạng SMTP chập chờn tạm thời?",
                new List<string>
                {
                    "A. Lưu trạng thái đơn hàng vào Database xong là trả kết quả thành công về cho người dùng ngay lập tức; còn tác vụ Tạo PDF + Gửi Email qua MailKit được đẩy vào hàng đợi chạy ngầm của Hangfire (_jobClient.Enqueue) để xử lý ở hậu trường kèm cơ chế tự động thử lại (Auto-Retry)",
                    "B. Bắt người dùng đứng nhìn màn hình trắng chờ gửi xong 10 cái Email rồi mới chuyển trang",
                    "C. Không gửi Email nữa",
                    "D. Bắt người dùng tự viết câu lệnh SQL để lấy hóa đơn"
                },
                "A",
                "Tách các tác vụ gọi bên thứ ba chậm chạp (như gửi SMTP Email, gửi SMS, đồng bộ dữ liệu) ra khỏi luồng HTTP Request chính và đưa vào Background Job (Hangfire) là bí quyết cốt lõi giúp các hệ thống lớn luôn phản hồi mượt mà tức thì.\n</USER_REQUEST>\n<ADDITIONAL_METADATA>\nThe current local time is: 2026-10-26T08:50:44+07:00.\n</ADDITIONAL_METADATA>\n<USER_SETTINGS_CHANGE>\nThe user changed setting Model Selection from Claude Sonnet 5.5 (Medium) to Gemini 3.8 Flash (High). No need to comment on this change if the user doesn't ask about it. If reporting what model you are, please use a human readable name instead of the exact string.\n</USER_SETTINGS_CHANGE>"
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

        var topic14 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 14);
        if (topic14 == null)
            return;

        topic14.Title = "Chủ đề 14: Tích hợp dịch vụ bên thứ ba (Email, Payment Gateway, Cloud Storage, Redis, Hangfire & Resilience)";
        topic14.Description = "10 Bài học con siêu chi tiết từ Adapter Pattern & Options Pattern, Gửi Email HTML (MailKit), Cổng thanh toán (VNPAY/HMAC-SHA512), Cloud Storage (Cloudinary/S3), Redis (IDistributedCache), Background Jobs (Hangfire), Resilience (Polly), Webhooks Idempotency, Refit REST Client đến Checklist tích hợp đa dịch vụ.";

        var targetLessons = GetLessons(topic14.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic14.Id)
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
