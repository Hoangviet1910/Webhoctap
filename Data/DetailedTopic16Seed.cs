using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic16Seed
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
            // BÀI 16.1
            // =====================================================================
            Build(
                topicId,
                "16.1",
                "Phân tích Nghiệp vụ (BRD/SRS), Thiết kế Sơ đồ Use Case, DFD, ERD & Lựa chọn Kiến trúc Dự án",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/",
                """
                • **💡 1. Sai lầm lớn nhất khi bắt đầu làm Đồ án / Dự án thực tế: "Mở Visual Studio lên gõ code ngay khi chưa có bản vẽ!"**
                  - Giống như xây một tòa nhà 10 tầng: nếu không có bản vẽ móng và sơ đồ phòng ốc mà cứ xếp gạch xây bừa, đến tầng 3 phát hiện thiếu cầu thang sẽ phải đập đi xây lại từ đầu!
                  - Trong kỹ nghệ phần mềm (**Software Engineering**), trước khi viết dòng code C# đầu tiên, bạn luôn phải đi qua **3 bước thiết kế cốt lõi**:
                    1. **Xác định Tác nhân (`Actors`) & Ca sử dụng (`Use Cases`):** Ai sẽ dùng hệ thống này (Độc giả, Thủ thư, Quản trị viên)? Mỗi người vào hệ thống để làm những chức năng gì?
                    2. **Vẽ Sơ đồ Luồng dữ liệu (`DFD`) & Sơ đồ Hoạt động (`Activity Diagram`):** Dữ liệu đi từ màn hình nào, qua bước kiểm tra nào rồi lưu vào kho nào?
                    3. **Thiết kế Thực thể liên kết (`ERD` - Entity Relationship Diagram):** Chốt danh sách các bảng, các cột, kiểu dữ liệu (`NVARCHAR`, `DECIMAL`), khóa chính (`PK`) và khóa ngoại (`FK`).
                • **🖼️ 2. Sơ đồ Quy Trình 4 Bước Từ Ý Tưởng Đề Tài Đến Khi Khởi Tạo Solution .NET:**
                ```
                [Bước 1: Đặc tả Yêu cầu (SRS)] ──► Liệt kê Yêu cầu Chức năng (Functional) & Phi chức năng (Bảo mật, Tốc độ)
                             │
                             ▼
                [Bước 2: Thiết kế UML / DFD]   ──► Vẽ Sơ đồ Use Case (Phân quyền Actor) + Activity / Sequence Diagram
                             │
                             ▼
                [Bước 3: Thiết kế CSDL (ERD)]  ──► Chốt các Bảng đạt chuẩn 3NF (1-N đặt FK ở bảng Nhiều; N-N tách bảng trung gian)
                             │
                             ▼
                [Bước 4: Khởi tạo Kiến trúc]   ──► Dựng khung Solution .NET + Tạo Entities + Chạy Migration đầu tiên!
                ```
                • **📊 3. Bảng Phân Loại Yêu Cầu Trong Tài Liệu Báo Cáo Đồ Án & Dự Án Công Ty:**
                | Loại yêu cầu | Giải thích bản chất | Ví dụ cụ thể trong hệ thống Quản lý Thư viện / Bán hàng |
                | :--- | :--- | :--- |
                | **1. Yêu cầu Chức năng (`Functional Requirements`)** | Hệ thống **làm được những tính năng nghiệp vụ gì** cho người dùng? | Đăng nhập, Tìm kiếm sách, Lập phiếu mượn, Tính tiền phạt trả trễ, Xuất báo cáo Excel/PDF, Tư vấn bằng AI. |
                | **2. Yêu cầu Phi chức năng (`Non-Functional Requirements`)**| Hệ thống vận hành **nhanh, an toàn và ổn định như thế nào**? | Mật khẩu phải băm `BCrypt/PBKDF2`, phân quyền `JWT/Cookie`, API phản hồi `< 300ms`, chống `SQL Injection`. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Phân tách Actor (Tác nhân) và Ma trận Phân quyền (RBAC Matrix)",
                        "📥 Đầu vào: Đề bài xây dựng hệ thống Quản lý Thư viện hoặc Thương mại điện tử. ⚙️ Cơ chế ngầm: Xác định rõ các nhóm người dùng (Admin, Librarian/Staff, Reader/Customer) để làm cơ sở thiết kế bảng Roles và gắn thẻ [Authorize(Roles = \"...\")] sau này. 📤 Đầu ra: Bảng ma trận quyền hạn rõ ràng.",
                        "// Ma trận Phân quyền (Role-Based Access Control):\n// 1. Reader (Độc giả): Xem sách, Tìm kiếm, Đặt mượn online, Xem lịch sử cá nhân.\n// 2. Librarian (Thủ thư): Duyệt phiếu mượn, Ghi nhận trả sách, Thu tiền phạt.\n// 3. Admin (Quản trị): Quản lý danh mục sách, Quản lý nhân sự, Xem Dashboard doanh thu."),
                    Step(2,
                        "2. Chuyển hóa bản vẽ ERD thành danh sách các Class Entity cốt lõi",
                        "📥 Đầu vào: Bản vẽ ERD gồm các thực thể và mối quan hệ. ⚙️ Cơ chế ngầm: Tạo lớp cơ sở BaseEntity (Id, CreatedAt, IsDeleted) và các lớp thực thể chính đảm bảo mọi mối quan hệ Nhiều-Nhiều đều đã có bảng trung gian. 📤 Đầu ra: Bộ Entities chuẩn bị cho Code-First.",
                        "// Quan hệ 1 - Nhiều: Category (1) ---> (N) Book\n// Quan hệ 1 - Nhiều: User (1)     ---> (N) Borrow\n// Quan hệ N - Nhiều: Borrow (1)   ---> (N) BorrowDetail (N) <--- (1) Book"),
                    Step(3,
                        "3. Lựa chọn Kiến trúc phù hợp với quy mô dự án",
                        "📥 Đầu vào: Quy mô nhóm và mục tiêu dự án. ⚙️ Cơ chế ngầm: Với bài tập lớn MVC dùng Kiến trúc 3 tầng (Controller - Service - Repository); với dự án doanh nghiệp lớn hoặc Đồ án tốt nghiệp dùng Clean Architecture (4 Projects tách biệt). 📤 Đầu ra: Khung dự án vững chắc ngay từ ngày đầu.",
                        "// Lựa chọn kiến trúc thông minh:\n// - Dự án vừa / MVC: 1 Project chia thư mục (Controllers, Services, Repositories, Models)\n// - Dự án lớn / API: Clean Architecture (Domain, Application, Infrastructure, WebAPI)")
                },
                "Khi viết báo cáo phân tích thiết kế hệ thống (hoặc khi trả lời phỏng vấn về quy trình làm dự án), các tiêu chí như: \"Tốc độ phản hồi trang web dưới 500ms, mật khẩu người dùng phải được băm kèm Salt, hệ thống chịu tải được 1.000 người dùng cùng lúc\" được xếp vào nhóm yêu cầu nào?",
                new List<string>
                {
                    "A. Yêu cầu Phi chức năng (Non-Functional Requirements - thuộc về chất lượng, hiệu năng và bảo mật của hệ thống)",
                    "B. Yêu cầu Chức năng (Functional Requirements)",
                    "C. Yêu cầu xóa cơ sở dữ liệu",
                    "D. Yêu cầu thiết kế màu sắc logo"
                },
                "A",
                "Phân biệt rõ Functional Requirements (hệ thống có tính năng gì) và Non-Functional Requirements (hệ thống chạy tốt, nhanh và bảo mật ra sao) là câu hỏi kinh điển trong cả môn Phân tích thiết kế hệ thống lẫn phỏng vấn thực tập."
            ),

            // =====================================================================
            // BÀI 16.2
            // =====================================================================
            Build(
                topicId,
                "16.2",
                "Thiết kế Kiến trúc Clean Architecture / Onion Architecture chuẩn Doanh nghiệp (4 Tầng: Domain, Application, Infrastructure, Presentation)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture",
                """
                • **💡 1. Hiểu bản chất của `Clean Architecture` trong 30 giây (Kiến trúc mà 80% công ty .NET hiện nay sử dụng!):**
                  - Trong kiến trúc truyền thống cũ, tầng Nghiệp vụ (`Business Logic`) phụ thuộc chặt vào tầng Database (`SQL Server`). Nếu sau này công ty muốn đổi từ SQL Server sang PostgreSQL hoặc MongoDB, phải sửa lại cả code nghiệp vụ!
                  - **`Clean Architecture` (Kiến trúc Sạch / Kiến trúc Củ hành)** đảo ngược hoàn toàn điều đó bằng **Quy tắc Phụ thuộc hướng vào tâm (`Dependency Rule`)**:
                    + **Tầng Lõi ở trong cùng (`Domain` & `Application`):** Chứa các thực thể `Entities`, các `Interfaces` (`IRepository`, `IEmailService`) và `Services` nghiệp vụ. **Tầng lõi này hoàn toàn độc lập, KHÔNG phụ thuộc vào bất kỳ thư viện Database hay giao diện nào!**
                    + **Tầng Vỏ ở bên ngoài (`Infrastructure` & `WebAPI/MVC`):** Phụ thuộc ngược vào Tầng Lõi để thực thi việc lưu SQL Server (`EF Core`), gửi Mail (`MailKit`) và mở cổng HTTP (`Controllers`)!
                • **🖼️ 2. Sơ đồ 4 Tầng (4 Projects) Của `Clean Architecture` & Chiều Mũi Tên Tham Chiếu (`Add Reference`):**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🌐 4. Presentation Layer (Project: HoangViet.WebAPI / HoangViet.MVC)       │
                │    - Chứa: Controllers, Views/Razor, Middlewares, Program.cs (DI Container)│
                └───────────────────┬────────────────────────────────────┬───────────────────┘
                                    │ Tham chiếu                         │ Tham chiếu
                                    ▼                                    ▼
                ┌───────────────────────────────────────┐    ┌───────────────────────────────┐
                │ ⚙️ 2. Application Layer (Core Logic)  │◄───│ 🗄️ 3. Infrastructure Layer    │
                │ - Chứa: DTOs, Interfaces (IRepo,      │    │ - Chứa: AppDbContext (EF Core)│
                │   IService), Services, Validators.    │    │   Repositories, MailKit, AI...│
                └───────────────────┬───────────────────┘    └───────────────────────────────┘
                                    │ Tham chiếu (Mọi mũi tên đều hướng về tâm Domain!)
                                    ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 💎 1. Domain Layer (Trái tim ở trong cùng - KHÔNG tham chiếu tới ai cả!)   │
                │    - Chứa: Entities (Book, User, Order), Enums, Custom Domain Exceptions   │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng Quy Tắc Tham Chiếu Giữa 4 Project Trong Solution Clean Architecture:**
                | Tên Project | Chứa những gì bên trong? | Được phép `Add Reference` tới Project nào? |
                | :--- | :--- | :--- |
                | **`1. Domain`** (`Class Library`) | `Entities`, `Enums`, `ValueObjects`, `Exceptions`. | ❌ **KHÔNG tham chiếu tới bất kỳ Project nào!** (Sạch 100%). |
                | **`2. Application`** (`Class Library`)| `DTOs`, `Interfaces` (`IBookRepository`), `Services`, `AutoMapper`, `Validators`. | Chỉ tham chiếu tới **`Domain`**. |
                | **`3. Infrastructure`** (`Class Library`)| `AppDbContext`, `Migrations`, `GenericRepository`, `SmtpEmailService`, `GeminiService`.| Tham chiếu tới **`Application`** (và gián tiếp thấy `Domain`). |
                | **`4. WebAPI / WebMVC`** (`Web App`) | `Controllers`, `Views`, `Middlewares`, `Program.cs`, `appsettings.json`. | Tham chiếu tới **`Application`** và **`Infrastructure`** (để đăng ký DI). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo Solution gồm 4 Projects theo chuẩn Clean Architecture",
                        "📥 Đầu vào: Cửa sổ Terminal. ⚙️ Cơ chế ngầm: Dùng lệnh dotnet new sln tạo Solution, tạo 3 Class Library (Domain, Application, Infrastructure) và 1 Web Project (WebAPI/MVC). 📤 Đầu ra: Cấu trúc thư mục chuẩn doanh nghiệp.",
                        "dotnet new sln -n HoangVietEnterprise\ndotnet new classlib -n HoangViet.Domain\ndotnet new classlib -n HoangViet.Application\ndotnet new classlib -n HoangViet.Infrastructure\ndotnet new webapi -n HoangViet.WebAPI\ndotnet sln add **/*.csproj"),
                    Step(2,
                        "2. Nối dây tham chiếu (Project References) tuân thủ Dependency Rule",
                        "📥 Đầu vào: 4 Projects vừa tạo. ⚙️ Cơ chế ngầm: Application trỏ vào Domain; Infrastructure trỏ vào Application; WebAPI trỏ vào cả Application và Infrastructure. Domain đứng độc lập ở tâm! 📤 Đầu ra: Không bao giờ bị lỗi tham chiếu vòng (Circular Dependency).",
                        "dotnet add HoangViet.Application reference HoangViet.Domain\ndotnet add HoangViet.Infrastructure reference HoangViet.Application\ndotnet add HoangViet.WebAPI reference HoangViet.Application HoangViet.Infrastructure"),
                    Step(3,
                        "3. Đặt Interface ở Application nhưng viết Class thực thi ở Infrastructure",
                        "📥 Đầu vào: Cần truy vấn bảng Books. ⚙️ Cơ chế ngầm: Giao diện IBookRepository nằm ở project Application, còn class BookRepository (dùng EF Core DbContext) nằm ở project Infrastructure, sau đó nối lại với nhau tại Program.cs của WebAPI! 📤 Đầu ra: Đảo ngược sự phụ thuộc (Dependency Inversion) hoàn hảo.",
                        "// 1. Tại HoangViet.Application/Interfaces/IBookRepository.cs:\npublic interface IBookRepository { Task<Book?> GetByIdAsync(int id); }\n\n// 2. Tại HoangViet.Infrastructure/Repositories/BookRepository.cs:\npublic class BookRepository : IBookRepository { ... }\n\n// 3. Tại HoangViet.WebAPI/Program.cs:\nbuilder.Services.AddScoped<IBookRepository, BookRepository>();")
                },
                "Trong kiến trúc Clean Architecture gồm 4 tầng (Domain, Application, Infrastructure, Presentation/WebAPI), tại sao giao diện IBookRepository lại được đặt ở tầng Application, trong khi class thực thi BookRepository (chứa code AppDbContext của EF Core) lại được đặt ở tầng Infrastructure?",
                new List<string>
                {
                    "A. Để tuân thủ nguyên lý Đảo ngược sự phụ thuộc (Dependency Inversion Principle): Tầng nghiệp vụ Application chỉ làm việc với bản hợp đồng trừu tượng (IBookRepository) mà không bị phụ thuộc cứng vào công nghệ cơ sở dữ liệu cụ thể (EF Core/SQL Server) nằm ở Infrastructure",
                    "B. Vì project Application không cho phép tạo file class",
                    "C. Vì Visual Studio bắt buộc phải chia đôi 2 file vào 2 thư mục khác ổ đĩa",
                    "D. Để làm cho chương trình chạy chậm hơn"
                },
                "A",
                "Đây là câu hỏi phỏng vấn kiến trúc kinh điển nhất: Đặt Interface ở tầng lõi (Application) và đặt Implementation ở tầng vỏ (Infrastructure) giúp tầng nghiệp vụ hoàn toàn độc lập với Database và dễ dàng viết Unit Test bằng Moq."
            ),

            // =====================================================================
            // BÀI 16.3
            // =====================================================================
            Build(
                topicId,
                "16.3",
                "Áp dụng 5 Nguyên lý SOLID & Các Design Patterns thực chiến trong .NET (Dependency Injection, Factory, Mediator/CQRS)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/modern-web-apps-azure/architectural-principles",
                """
                • **💡 1. Hiểu trọn vẹn 5 Nguyên lý `SOLID` bằng ví dụ C# thực tế trong 60 giây (Câu hỏi phỏng vấn 100% gặp!):**
                  Chữ **`SOLID`** là viết tắt của 5 nguyên tắc thiết kế hướng đối tượng giúp code sạch, dễ mở rộng và không bị hỏng dây chuyền khi sửa đổi:
                  1. **`S` - Single Responsibility Principle (Đơn trách nhiệm):** *Mỗi class chỉ nên có ĐÚNG 1 lý do để thay đổi.*
                     - *Ví dụ thực tế:* `BookController` chỉ lo nhận/trả HTTP; `BookService` chỉ lo tính toán nghiệp vụ; `BookRepository` chỉ lo truy vấn SQL; `EmailService` chỉ lo gửi mail (Không nhồi cả 4 việc vào chung 1 class!).
                  2. **`O` - Open/Closed Principle (Đóng để sửa, Mở để thêm):** *Khi thêm tính năng mới, hãy viết thêm class mới thực thi Interface chứ hạn chế sửa nát hàm cũ đang chạy tốt.*
                     - *Ví dụ:* Thêm cổng thanh toán `ZaloPayService : IPaymentGateway` mà không cần sửa code của `VnPayService`!
                  3. **`L` - Liskov Substitution Principle (Thay thế Liskov):** *Các class con kế thừa phải thay thế được hoàn toàn vị trí của class cha/Interface mà không làm nổ lỗi bất ngờ.*
                  4. **`I` - Interface Segregation Principle (Phân tách Interface nhỏ gọn):** *Thay vì tạo 1 Interface khổng lồ có 50 hàm bắt mọi class phải thực thi thừa, hãy chia nhỏ thành các Interface chuyên biệt* (như `IReadRepository<T>` và `IWriteRepository<T>`).
                  5. **`D` - Dependency Inversion Principle (Đảo ngược phụ thuộc):** *Các tầng không phụ thuộc trực tiếp vào Class cụ thể của nhau, mà cả hai cùng phụ thuộc vào **`Interface`** (chính là cơ chế Dependency Injection trong `Program.cs`!).*
                • **🖼️ 2. Giới thiệu Mẫu kiến trúc nâng cao: `CQRS` (Command Query Responsibility Segregation) & `MediatR`:**
                ```
                Trong các hệ thống lớn, người ta tách bạch hoàn toàn thao tác ĐỌC (Query) và thao tác GHI (Command):
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 📖 1. QUERY (Chỉ Đọc dữ liệu - GET):                                       │
                │       Dùng AsNoTracking() / Dapper / Redis Cache ──► Chiếu thẳng ra DTO!   │
                ├────────────────────────────────────────────────────────────────────────────┤
                │ ✍️ 2. COMMAND (Thay đổi dữ liệu - POST / PUT / DELETE):                    │
                │       Kiểm tra FluentValidation ──► Xử lý Nghiệp vụ ──► Lưu qua EF Core DB!│
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Áp dụng chữ S (Single Responsibility) và chữ D (Dependency Inversion)",
                        "📥 Đầu vào: Một hàm đăng ký tài khoản đang ôm đồm cả lưu DB lẫn gửi SMTP Email. ⚙️ Cơ chế ngầm: Tách phần gửi mail sang IEmailService và phần lưu DB sang IUserRepository, sau đó tiêm cả 2 Interface vào AuthService. 📤 Đầu ra: Mỗi class chỉ làm đúng 1 nhiệm vụ chuyên môn.",
                        "public class AuthService : IAuthService {\n    private readonly IUserRepository _userRepo;\n    private readonly IEmailService _emailService;\n\n    public AuthService(IUserRepository userRepo, IEmailService emailService) {\n        _userRepo = userRepo;\n        _emailService = emailService;\n    }\n}"),
                    Step(2,
                        "2. Áp dụng chữ O (Open/Closed) kết hợp Factory / Strategy Pattern",
                        "📥 Đầu vào: Hệ thống cần hỗ trợ nhiều hình thức thanh toán (Tiền mặt, VNPAY, MoMo). ⚙️ Cơ chế ngầm: Dùng giao diện chung IPaymentStrategy. Khi công ty muốn thêm cổng VietQR, chỉ cần tạo thêm class VietQrStrategy : IPaymentStrategy mà không cần sửa code của các cổng cũ! 📤 Đầu ra: Dễ dàng mở rộng không giới hạn.",
                        "public interface IPaymentStrategy {\n    string ProviderName { get; }\n    Task<string> PayAsync(Order order);\n}\n\npublic class VnPayStrategy : IPaymentStrategy {\n    public string ProviderName => \"VNPAY\";\n    public async Task<string> PayAsync(Order order) => \"https://sandbox.vnpayment.vn/...\";\n}"),
                    Step(3,
                        "3. Áp dụng chữ I (Interface Segregation - Tách nhỏ Interface)",
                        "📥 Đầu vào: Một bảng Danh mục Tỉnh/Thành phố chỉ cho phép Đọc chứ không bao giờ cho phép người dùng Xóa hay Sửa. ⚙️ Cơ chế ngầm: Tách thành IReadOnlyRepository<T> (chỉ có GetById, GetAll) và IWriteRepository<T> (có Add, Update, Delete). 📤 Đầu ra: An toàn và chặt chẽ.",
                        "public interface IReadOnlyRepository<T> where T : class {\n    Task<T?> GetByIdAsync(int id);\n    Task<List<T>> GetAllAsync();\n}\n\npublic interface IGenericRepository<T> : IReadOnlyRepository<T> where T : class {\n    Task AddAsync(T entity);\n    void Remove(T entity);\n}")
                },
                "Khi đi phỏng vấn thực tập hoặc Fresher .NET, nhà tuyển dụng hỏi: \"Em đã áp dụng nguyên lý S và nguyên lý D trong bộ 5 nguyên lý SOLID vào dự án của mình như thế nào?\". Câu trả lời nào dưới đây ghi điểm tuyệt đối?",
                new List<string>
                {
                    "A. Nguyên lý S (Single Responsibility) được em áp dụng bằng cách tách rõ Controller chỉ điều phối HTTP, Service xử lý nghiệp vụ và Repository truy cập DB; còn nguyên lý D (Dependency Inversion) được áp dụng bằng cách cho Controller và Service giao tiếp thông qua các Interface (IService, IRepository) và tiêm qua Dependency Injection trong Program.cs",
                    "B. Em viết toàn bộ code của dự án vào chung 1 file Program.cs dài 10.000 dòng",
                    "C. Em dùng từ khóa static cho tất cả các class",
                    "D. SOLID là tên của một hệ quản trị cơ sở dữ liệu"
                },
                "A",
                "Trả lời các nguyên lý SOLID bằng chính ví dụ thực tế trong dự án của bạn (Controller ➔ IService ➔ IRepository) cho thấy bạn thực sự hiểu và vận dụng được lý thuyết vào thực tiễn chứ không học thuộc lòng sáo rỗng."
            ),

            // =====================================================================
            // BÀI 16.4
            // =====================================================================
            Build(
                topicId,
                "16.4",
                "Xây dựng Khung Cơ sở (Base Infrastructure) Dùng Chung cho Toàn Dự Án: BaseEntity, AuditableEntity, PagedResult & ApiResponse",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/logging-events-diagnostics/interceptors",
                """
                • **💡 1. Bí quyết giúp các Senior dựng mới 1 dự án trong 30 phút: "Bộ Tứ Khuôn Mẫu Dùng Chung (`Common / Shared Kernel`)"!**
                  - Trong một dự án thực tế có 30 bảng Database và 100 API, nếu bảng nào bạn cũng gõ lại `Id`, `CreatedAt`, `UpdatedAt`, `IsDeleted` và API nào cũng tự nghĩ ra một kiểu trả về khác nhau thì code sẽ cực kỳ lộn xộn.
                  - Trước khi code bất kỳ tính năng nào, người trưởng nhóm (Tech Lead) luôn tạo sẵn **4 file khung nền tảng dùng chung cho toàn bộ dự án**:
                    1. **`BaseEntity` / `AuditableEntity`:** Lớp cha của mọi bảng SQL (chứa `Id`, `CreatedAt`, `CreatedBy`, `UpdatedAt`, `IsDeleted`).
                    2. **Tự động điền ngày tạo/sửa trong `SaveChangesAsync()`:** Ghi đè trong `AppDbContext` để tự động gán `CreatedAt = DateTime.UtcNow` khi thêm mới và `UpdatedAt = DateTime.UtcNow` khi cập nhật!
                    3. **`ApiResponse<T>` (`Result<T>`):** Hộp đóng gói kết quả chuẩn (`IsSuccess`, `Message`, `Data`, `Errors`).
                    4. **`PagedResult<T>` & `PaginationFilter`:** Khung phân trang dùng chung cho mọi API lấy danh sách.
                • **🖼️ 2. Sơ đồ Tự Động Hóa Kiểm Toán Dữ Liệu (`Audit Interceptor`) Trong `AppDbContext`:**
                ```
                Bất kỳ Service nào gọi: await _unitOfWork.SaveChangesAsync()
                      │
                      ▼  Chạy qua hàm override SaveChangesAsync() trong AppDbContext:
                Quét toàn bộ các đối tượng trong ChangeTracker.Entries<AuditableEntity>():
                  ├── Nếu trạng thái là Added (Thêm mới):
                  │     ──► Tự động gán: entity.CreatedAt = DateTime.UtcNow; entity.IsDeleted = false;
                  ├── Nếu trạng thái là Modified (Cập nhật):
                  │     ──► Tự động gán: entity.UpdatedAt = DateTime.UtcNow;
                  └── Nếu trạng thái là Deleted (Xóa):
                        ──► Tự động chuyển thành Soft Delete: entry.State = Modified; entity.IsDeleted = true!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo lớp cha AuditableEntity cho toàn bộ các bảng trong Database",
                        "📥 Đầu vào: Các cột quản lý lịch sử bản ghi. ⚙️ Cơ chế ngầm: Mọi Entity trong dự án (Book, Reader, Borrow, Lesson) chỉ cần kế thừa ': AuditableEntity' là tự động có đủ khóa chính và các cột theo dõi thời gian, xóa mềm. 📤 Đầu ra: Chuẩn hóa 100% các bảng.",
                        "public abstract class AuditableEntity {\n    public int Id { get; set; }\n    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;\n    public string? CreatedBy { get; set; }\n    public DateTime? UpdatedAt { get; set; }\n    public bool IsDeleted { get; set; } = false;\n}"),
                    Step(2,
                        "2. Tự động hóa gán CreatedAt, UpdatedAt và Soft Delete trong AppDbContext",
                        "📥 Đầu vào: Ghi đè hàm SaveChangesAsync trong AppDbContext.cs. ⚙️ Cơ chế ngầm: Duyệt qua ChangeTracker.Entries<AuditableEntity>() để tự động điền ngày giờ và biến lệnh Remove() thành xóa mềm IsDeleted = true hoàn toàn tự động! 📤 Đầu ra: Không bao giờ phải gõ tay CreatedAt = DateTime.Now ở từng Service nữa.",
                        "public override Task<int> SaveChangesAsync(CancellationToken ct = default) {\n    foreach (var entry in ChangeTracker.Entries<AuditableEntity>()) {\n        switch (entry.State) {\n            case EntityState.Added:\n                entry.Entity.CreatedAt = DateTime.UtcNow;\n                break;\n            case EntityState.Modified:\n                entry.Entity.UpdatedAt = DateTime.UtcNow;\n                break;\n            case EntityState.Deleted:\n                entry.State = EntityState.Modified; // Chuyển Xóa cứng thành Cập nhật\n                entry.Entity.IsDeleted = true;      // Đánh dấu Xóa mềm tự động!\n                break;\n        }\n    }\n    return base.SaveChangesAsync(ct);\n}"),
                    Step(3,
                        "3. Tạo lớp bọc kết quả chuẩn ApiResponse<T> dùng chung toàn hệ thống",
                        "📥 Đầu vào: Kết quả trả về của mọi API. ⚙️ Cơ chế ngầm: Cung cấp 2 hàm tĩnh tiện lợi ApiResponse<T>.Success(data, msg) và ApiResponse<T>.Failure(msg) để mọi Controller trả về JSON đồng nhất 100%. 📤 Đầu ra: Giao tiếp chuẩn mực với Frontend.",
                        "public class ApiResponse<T> {\n    public bool IsSuccess { get; set; }\n    public string Message { get; set; } = string.Empty;\n    public T? Data { get; set; }\n\n    public static ApiResponse<T> Success(T data, string msg = \"Thành công\") =>\n        new() { IsSuccess = true, Data = data, Message = msg };\n\n    public static ApiResponse<T> Failure(string msg) =>\n        new() { IsSuccess = false, Message = msg };\n}")
                },
                "Trong đoạn code ghi đè SaveChangesAsync() của AppDbContext ở trên, khi một lập trình viên gọi lệnh _db.Books.Remove(book); (khiến entry.State mang giá trị EntityState.Deleted), đoạn code trong nhánh case EntityState.Deleted: đã làm điều kỳ diệu gì?",
                new List<string>
                {
                    "A. Tự động chặn lệnh xóa vật lý khỏi Database bằng cách đổi trạng thái từ EntityState.Deleted sang EntityState.Modified và gán thuộc tính IsDeleted = true (biến mọi lệnh xóa thông thường thành Xóa mềm - Soft Delete hoàn toàn tự động)",
                    "B. Xóa luôn cả bảng Books khỏi SQL Server",
                    "C. Nhân đôi bản ghi sách đó lên",
                    "D. Báo lỗi không cho phép lưu"
                },
                "A",
                "Đây là kỹ thuật cực kỳ đẳng cấp trong EF Core: bằng cách can thiệp vào ChangeTracker ngay trước khi lưu, bạn biến lệnh .Remove() thành lệnh UPDATE Books SET IsDeleted = 1, giúp toàn bộ dự án thực hiện Soft Delete đồng bộ mà không sợ ai đó gọi nhầm .Remove() làm mất dữ liệu thật!"
            ),

            // =====================================================================
            // BÀI 16.5
            // =====================================================================
            Build(
                topicId,
                "16.5",
                "Thực chiến Module Nghiệp vụ Phức tạp (Multi-step Business Workflow): Luồng Đặt hàng / Mượn trả sách có Kiểm tra Tồn kho & Tính phí",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/saving/transactions",
                """
                • **💡 1. Phân biệt rõ "Chức năng CRUD đơn giản" và "Chức năng Nghiệp vụ cốt lõi (Core Business Workflow)" khi đi bảo vệ Đồ án & Phỏng vấn:**
                  - Khi chấm đồ án hoặc phỏng vấn CV, thầy cô và nhà tuyển dụng **rất ít khi hỏi các bảng CRUD đơn giản** (như Thêm/Sửa/Xóa Thể loại), mà họ sẽ **xoáy sâu 100% vào các luồng Nghiệp vụ Phức tạp tác động lên nhiều bảng cùng lúc** (như *Luồng Lập Phiếu Mượn & Trả Sách Tính Tiền Phạt*, hoặc *Luồng Đặt Hàng Áp Mã Giảm Giá & Thanh Toán*)!
                • **🖼️ 2. Sơ đồ Luồng Nghiệp Vụ Khép Kín: "Mượn Sách & Trả Sách Tính Tiền Phạt Quá Hạn":**
                ```
                [LUỒNG A - ĐẶT MƯỢN SÁCH (CreateBorrowAsync)]:
                1. Kiểm tra Thẻ độc giả còn hạn hoạt động không? Độc giả có đang nợ sách quá hạn chưa trả không?
                2. Kiểm tra từng cuốn sách yêu cầu: Còn tồn kho (Stock >= Quantity) không?
                3. Mở Database Transaction:
                   ├── Tạo bản ghi Phiếu mượn (Borrows) + Các dòng Chi tiết (BorrowDetails)
                   ├── Trừ số lượng tồn kho (book.Stock -= quantity) của từng cuốn sách
                   └── Lưu đồng bộ xuống SQL Server (CommitAsync) + Gửi Email xác nhận ngầm!

                [LUỒNG B - TRẢ SÁCH & TÍNH PHẠT (ReturnBookAsync)]:
                1. Cập nhật Ngày trả thực tế (ActualReturnDate = DateTime.Now)
                2. So sánh với Ngày hẹn trả (DueDate):
                   ├── Nếu ActualReturnDate > DueDate: Tính số ngày trễ = (ActualReturnDate - DueDate).Days
                   │                                   Tiền phạt = Số ngày trễ * 5.000đ/ngày * Số cuốn!
                   └── Cộng trả lại số lượng sách vào kho (book.Stock += quantity) ──► Lưu DB!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm tra các điều kiện tiên quyết của Nghiệp vụ (Business Rules Validation)",
                        "📥 Đầu vào: Yêu cầu mượn sách gồm ReaderId và danh sách BookIds. ⚙️ Cơ chế ngầm: Dùng Guard Clauses kiểm tra thẻ độc giả hợp lệ, kiểm tra không mượn quá số lượng tối đa (VD: tối đa 5 cuốn) và kiểm tra tồn kho của các sách. 📤 Đầu ra: Đảm bảo 100% quy tắc thư viện trước khi mở giao dịch.",
                        "if (dto.Items.Count == 0 || dto.Items.Count > 5)\n    throw new BadRequestException(\"Mỗi phiếu mượn chỉ được mượn từ 1 đến 5 cuốn sách!\");\n\nbool hasOverdue = await _borrowRepo.HasUnreturnedOverdueAsync(dto.ReaderId, ct);\nif (hasOverdue)\n    throw new BadRequestException(\"Độc giả đang có sách quá hạn chưa trả, không thể mượn thêm!\");"),
                    Step(2,
                        "2. Thực thi Tạo phiếu mượn và Trừ tồn kho trong Database Transaction",
                        "📥 Đầu vào: Dữ liệu đã qua kiểm tra hợp lệ. ⚙️ Cơ chế ngầm: Mở BeginTransactionAsync, duyệt qua từng món để trừ Stock, tạo Borrow kèm danh sách BorrowDetails và gọi SaveChangesAsync + CommitAsync. 📤 Đầu ra: Dữ liệu 3 bảng (Borrows, BorrowDetails, Books) nhất quán tuyệt đối.",
                        "using var transaction = await _db.Database.BeginTransactionAsync(ct);\ntry {\n    var borrow = new Borrow {\n        ReaderId = dto.ReaderId,\n        BorrowDate = DateTime.Now,\n        DueDate = DateTime.Now.AddDays(14),\n        Status = BorrowStatus.Borrowing\n    };\n    foreach (var item in dto.Items) {\n        var book = await _db.Books.FindAsync(new object[] { item.BookId }, ct);\n        if (book == null || book.Stock < item.Quantity)\n            throw new BadRequestException($\"Sách '{book?.Title}' không đủ số lượng trong kho!\");\n\n        book.Stock -= item.Quantity; // Trừ tồn kho\n        borrow.BorrowDetails.Add(new BorrowDetail { BookId = book.Id, Quantity = item.Quantity });\n    }\n    _db.Borrows.Add(borrow);\n    await _db.SaveChangesAsync(ct);\n    await transaction.CommitAsync(ct);\n} catch {\n    await transaction.RollbackAsync(ct);\n    throw;\n}"),
                    Step(3,
                        "3. Xử lý nghiệp vụ Trả sách, Hoàn kho và Tự động tính Tiền phạt quá hạn",
                        "📥 Đầu vào: Mã phiếu mượn borrowId khi độc giả mang sách đến trả. ⚙️ Cơ chế ngầm: Lấy phiếu mượn kèm BorrowDetails (.Include), cộng hoàn lại Stock cho từng cuốn sách, tính số ngày trễ hạn (nếu có) nhân với đơn giá phạt 5.000đ/ngày. 📤 Đầu ra: Cập nhật trạng thái Đã trả và số tiền phạt chính xác.",
                        "var borrow = await _db.Borrows\n    .Include(b => b.BorrowDetails)\n    .FirstOrDefaultAsync(b => b.Id == borrowId, ct)\n    ?? throw new KeyNotFoundException(\"Không tìm thấy phiếu mượn!\");\n\nborrow.ActualReturnDate = DateTime.Now;\nborrow.Status = BorrowStatus.Returned;\n\n// Tính số ngày quá hạn (nếu trả trễ hơn DueDate):\nint overdueDays = (borrow.ActualReturnDate.Value.Date - borrow.DueDate.Date).Days;\nborrow.FineAmount = overdueDays > 0 ? overdueDays * 5000m : 0m;"),
                    Step(4,
                        "4. Hoàn trả số lượng sách vào kho và lưu hoàn tất",
                        "📥 Đầu vào: Danh sách BorrowDetails của phiếu vừa trả. ⚙️ Cơ chế ngầm: Cộng trả lại số lượng vào cột Stock của bảng Books và gọi SaveChangesAsync. 📤 Đầu ra: Khép kín vòng đời mượn - trả hoàn hảo.",
                        "foreach (var detail in borrow.BorrowDetails) {\n    var book = await _db.Books.FindAsync(new object[] { detail.BookId }, ct);\n    if (book != null) book.Stock += detail.Quantity; // Cộng trả lại vào kho!\n}\nawait _db.SaveChangesAsync(ct);")
                },
                "Trong luồng nghiệp vụ Trả sách thư viện (hoặc Hủy đơn hàng thương mại điện tử), ngoài việc đổi trạng thái của phiếu từ Đang mượn / Đang xử lý sang Đã trả / Đã hủy, thao tác dữ liệu bắt buộc nào trên bảng Books / Products phải được thực hiện trong cùng một Transaction để không bị sai lệch số liệu kho?",
                new List<string>
                {
                    "A. Phải cộng hoàn trả lại số lượng sách/sản phẩm của phiếu đó vào cột tồn kho (book.Stock += detail.Quantity) trong bảng Books/Products",
                    "B. Phải xóa cuốn sách đó khỏi bảng Books",
                    "C. Phải đổi tên cuốn sách thành Đã trả",
                    "D. Không cần cập nhật gì ở bảng Books"
                },
                "A",
                "Lúc mượn (hoặc đặt hàng) đã trừ tồn kho (Stock -= Quantity), nên khi trả sách (hoặc khi hủy đơn hàng) bắt buộc phải hoàn trả lại số lượng vào kho (Stock += Quantity). Đây là điểm các thầy cô chấm đồ án và người phỏng vấn luôn kiểm tra xem sinh viên có tư duy nghiệp vụ chặt chẽ hay không."
            ),

            // =====================================================================
            // BÀI 16.6
            // =====================================================================
            Build(
                topicId,
                "16.6",
                "Xây dựng Trang Quản trị Tổng hợp (Admin Dashboard): Truy vấn Thống kê Đa chiều, Top Ranking & Xuất Báo cáo",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/",
                """
                • **💡 1. Tại sao màn hình `Dashboard Tổng Quan` luôn là "Bộ mặt ăn tiền nhất" của mọi dự án?**
                  - Khi thầy cô chấm đồ án hoặc nhà tuyển dụng bấm vào xem sản phẩm của bạn, màn hình đầu tiên đập vào mắt họ là **Trang Chủ / Dashboard Thống Kê**.
                  - Một Dashboard đẳng cấp không bao giờ hiển thị mấy chữ tĩnh nhàm chán, mà luôn gồm **3 khu vực dữ liệu sống**:
                    1. **Hàng 4 Thẻ Chỉ Số KPI (`KPI Summary Cards` ở trên cùng):** Tổng số đầu sách / Tổng số bài học, Số lượt đang mượn / Tiến độ hoàn thành (`%`), Số phiếu quá hạn cần xử lý (màu đỏ cảnh báo), Tổng doanh thu / Điểm năng lực.
                    2. **Khu vực Biểu đồ Trực quan (`Chart.js` ở giữa):** Biểu đồ đường/cột theo thời gian + Biểu đồ tròn/Radar phân bổ theo danh mục.
                    3. **Bảng Xếp hạng & Cảnh báo nhanh (`Top 5 & Recent Alerts` ở dưới):** Top 5 cuốn sách được mượn nhiều nhất tháng + Danh sách các phiếu mượn sắp đến hạn trả hôm nay!
                • **🖼️ 2. Sơ đồ Bố Cục Màn Hình `Dashboard` Chuẩn UI/UX Hiện Đại:**
                ```
                ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
                │ 📚 Tổng số Sách │ │ 📖 Đang cho mượn│ │ ⚠️ Quá hạn trả  │ │ 💰 Tiền phạt thu│
                │     1,250       │ │       84        │ │    5 (Cần xử lý)│ │   450,000 VNĐ   │
                └─────────────────┘ └─────────────────┘ └─────────────────┘ └─────────────────┘
                ┌───────────────────────────────────────────┬─────────────────────────────────┐
                │ 📈 Biểu đồ Lượt mượn 6 tháng gần nhất     │ 🎯 Tỷ lệ Sách theo Thể loại     │
                │    (Line / Bar Chart - Chart.js)          │    (Doughnut / Radar Chart)     │
                ├───────────────────────────────────────────┴─────────────────────────────────┤
                │ 🏆 Bảng Top 5 Sách mượn nhiều nhất & Danh sách Phiếu mượn mới nhất hôm nay  │
                └─────────────────────────────────────────────────────────────────────────────┘
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo DashboardViewModel / Dto gom toàn bộ chỉ số của màn hình",
                        "📥 Đầu vào: Các con số thống kê và danh sách cần hiển thị trên trang chủ. ⚙️ Cơ chế ngầm: Gom tất cả vào 1 class DashboardDto duy nhất để giao diện chỉ cần nhận 1 Model là vẽ được toàn bộ trang! 📤 Đầu ra: Cấu trúc dữ liệu tổng hợp.",
                        "public class DashboardSummaryDto {\n    public int TotalBooks { get; set; }\n    public int ActiveBorrowsCount { get; set; }\n    public int OverdueBorrowsCount { get; set; }\n    public decimal TotalFineCollected { get; set; }\n    public List<TopBookDto> TopBorrowedBooks { get; set; } = new();\n    public ChartDataDto MonthlyBorrowChart { get; set; } = new(new(), new());\n}"),
                    Step(2,
                        "2. Viết các câu lệnh LINQ Thống kê tối ưu với AsNoTracking",
                        "📥 Đầu vào: Truy vấn từ AppDbContext. ⚙️ Cơ chế ngầm: Sử dụng CountAsync, SumAsync và GroupBy kết hợp OrderByDescending().Take(5) để lấy dữ liệu chính xác từ SQL Server. 📤 Đầu ra: Dữ liệu thống kê thời gian thực.",
                        "var summary = new DashboardSummaryDto {\n    TotalBooks = await _db.Books.AsNoTracking().CountAsync(ct),\n    ActiveBorrowsCount = await _db.Borrows.AsNoTracking()\n        .CountAsync(b => b.Status == BorrowStatus.Borrowing, ct),\n    OverdueBorrowsCount = await _db.Borrows.AsNoTracking()\n        .CountAsync(b => b.Status == BorrowStatus.Borrowing && b.DueDate < DateTime.Now, ct),\n    TotalFineCollected = await _db.Borrows.AsNoTracking()\n        .SumAsync(b => b.FineAmount, ct)\n};"),
                    Step(3,
                        "3. Truy vấn Top 5 Đối tượng nổi bật bằng Projection (.Select)",
                        "📥 Đầu vào: Bảng Books và BorrowDetails. ⚙️ Cơ chế ngầm: Tính tổng số lượng đã được mượn của từng cuốn sách, sắp xếp giảm dần và lấy 5 cuốn đứng đầu. 📤 Đầu ra: Bảng xếp hạng Top 5 trên Dashboard.",
                        "summary.TopBorrowedBooks = await _db.Books.AsNoTracking()\n    .Select(b => new TopBookDto {\n        BookId = b.Id,\n        Title = b.Title,\n        TotalBorrowCount = b.BorrowDetails.Sum(bd => bd.Quantity)\n    })\n    .OrderByDescending(x => x.TotalBorrowCount)\n    .Take(5)\n    .ToListAsync(ct);")
                },
                "Trên màn hình Dashboard quản trị có hiển thị 4 thẻ số liệu tổng hợp, 2 biểu đồ và 1 bảng Top 5 (dữ liệu này được rất nhiều người dùng mở xem liên tục nhưng chỉ cần cập nhật mỗi 5 phút một lần). Bạn nên kết hợp kỹ thuật nào đã học ở Chủ đề 9 / Chủ đề 14 vào hàm GetDashboardSummaryAsync() để trang chủ luôn tải trong 1ms mà không bắt SQL Server phải chạy đi chạy lại 7 câu lệnh thống kê mỗi giây?",
                new List<string>
                {
                    "A. Lưu đệm đối tượng kết quả DashboardSummaryDto vào IMemoryCache (hoặc Redis) với thời gian sống 5 phút (AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5))",
                    "B. Xóa bớt các biểu đồ đi",
                    "C. Dùng Thread.Sleep(5000)",
                    "D. Bắt người dùng tự bấm máy tính bỏ túi"
                },
                "A",
                "Các câu truy vấn COUNT, SUM, GROUP BY trên trang Dashboard là ứng cử viên hoàn hảo nhất cho Caching: lưu đệm kết quả DashboardSummaryDto trong 3-5 phút giúp giảm 99% tải cho SQL Server và làm trang chủ mở lên tức thì!"
            ),

            // =====================================================================
            // BÀI 16.7
            // =====================================================================
            Build(
                topicId,
                "16.7",
                "Tiêu chuẩn Clean Code, Refactoring & Quy tắc Review Code trước khi Nộp Đồ án / Gửi Pull Request ở Công ty",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/coding-style/coding-conventions",
                """
                • **💡 1. Câu nói nổi tiếng của tác giả cuốn sách *Clean Code*: "Bất kỳ ai cũng viết được đoạn code mà máy tính hiểu được. Nhưng chỉ lập trình viên giỏi mới viết được đoạn code mà CON NGƯỜI đọc hiểu được!"**
                  Khi đi thực tập hoặc khi nhà tuyển dụng mở GitHub của bạn ra xem code, họ đánh giá trình độ của bạn chỉ trong 30 giây thông qua **5 tiêu chuẩn Clean Code trong C#**:
                • **🖼️ 2. Bảng Đối Chiếu: Code Sinh Viên Mới Học ❌ vs Code Chuẩn Senior .NET (Clean Code) ✅:**
                ```
                ┌───────────────────────────┬───────────────────────────────────┬────────────────────────────────────────┐
                │ Tiêu chí đánh giá         │ ❌ Cách viết lỗi cần tránh        │ ✅ Chuẩn Clean Code chuyên nghiệp      │
                ├───────────────────────────┼───────────────────────────────────┼────────────────────────────────────────┤
                │ 1. Đặt tên (Naming)       │ int d; var list1; void Xuly();    │ int overdueDays; var activeBooks;      │
                │    - Class/Method/Property│ (Viết tắt vô nghĩa, lộn xộn)      │ public async Task ApproveBorrowAsync() │
                │    - Biến cục bộ/Tham số  │                                   │ (PascalCase cho Hàm, camelCase cho Biến│
                ├───────────────────────────┼───────────────────────────────────┼────────────────────────────────────────┤
                │ 2. Con số ma thuật        │ if (user.Status == 3)             │ if (user.Status == UserStatus.Locked)  │
                │    (Magic Numbers/Strings)│ fine = days * 5000;               │ fine = days * DEFAULT_FINE_PER_DAY;    │
                ├───────────────────────────┼───────────────────────────────────┼────────────────────────────────────────┤
                │ 3. Độ dài của 1 Hàm       │ 1 hàm dài 200 dòng, if-else lồng  │ Mỗi hàm dài 10-30 dòng, dùng Guard     │
                │    & Độ sâu thụt lề       │ nhau 5 tầng hình mũi tên.         │ Clauses (if lỗi -> return sớm ở đầu!). │
                ├───────────────────────────┼───────────────────────────────────┼────────────────────────────────────────┤
                │ 4. Code chết (Dead Code)  │ Để lại hàng trăm dòng code cũ     │ Xóa sạch code thừa (Vì nếu cần xem lại │
                │    & Comment rác          │ bị bôi xanh // comment khắp file! │ đã có lịch sử Git lưu giữ 100% rồi!).  │
                └───────────────────────────┴───────────────────────────────────┴────────────────────────────────────────┘
                ```
                • **📊 3. Nguyên tắc `DRY` (Don't Repeat Yourself - Đừng lặp lại chính mình):**
                  - Hễ bạn thấy mình đang **Copy - Paste cùng một đoạn code từ chỗ thứ 2 trở đi**, hãy dừng tay lại ngay! Tách đoạn code đó ra thành một **Hàm dùng chung (`Private Helper Method`, `Extension Method` hoặc `BaseClass`)** để khi cần sửa chỉ phải sửa ở đúng 1 nơi duy nhất!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khử \"Magic Numbers\" bằng Enum và Hằng số (const)",
                        "📥 Đầu vào: Đoạn code có các con số 1, 2, 5000, 14 nằm rải rác không rõ ý nghĩa. ⚙️ Cơ chế ngầm: Thay các mã trạng thái bằng Enum và thay các ngưỡng quy định bằng hằng số const viết hoa rõ nghĩa. 📤 Đầu ra: Đọc code êm như đọc văn xuôi.",
                        "private const int MAX_BORROW_DAYS = 14;\nprivate const decimal FINE_PER_DAY_VND = 5000m;\n\nif (borrow.Status == BorrowStatus.Overdue) {\n    decimal fine = lateDays * FINE_PER_DAY_VND;\n}"),
                    Step(2,
                        "2. Làm phẳng hàm dài bằng Guard Clauses & Tách hàm nhỏ (Extract Method)",
                        "📥 Đầu vào: Một hàm dài có nhiều tầng if-else lồng nhau. ⚙️ Cơ chế ngầm: Đưa các điều kiện kiểm tra lỗi lên đầu hàm để return/throw sớm, và tách phần tính toán phụ ra một hàm private có tên rõ nghĩa. 📤 Đầu ra: Hàm chính ngắn gọn chỉ còn 10 dòng.",
                        "public async Task<BorrowDto> ProcessReturnAsync(int borrowId, CancellationToken ct) {\n    var borrow = await GetValidBorrowOrThrowAsync(borrowId, ct);\n    \n    borrow.MarkAsReturned(DateTime.Now, FINE_PER_DAY_VND);\n    await RestoreBookStockAsync(borrow.BorrowDetails, ct);\n    await _unitOfWork.SaveChangesAsync(ct);\n    \n    return borrow.ToDto();\n}"),
                    Step(3,
                        "3. Tự kiểm tra bằng Checklist 5 điểm trước khi tạo Pull Request",
                        "📥 Đầu vào: Code vừa viết xong chuẩn bị git push. ⚙️ Cơ chế ngầm: Rà soát: (1) Đã xóa các dòng using thừa và code comment rác chưa? (2) Đã có async/await đầy đủ chưa? (3) Đã truyền CancellationToken chưa? (4) Có cảnh báo vàng (Warning) nào khi dotnet build không? 📤 Đầu ra: Code đạt chuẩn Senior Review.",
                        "dotnet format // Lệnh tự động căn chỉnh lề và định dạng code chuẩn Microsoft!\ndotnet build --configuration Release")
                },
                "Khái niệm \"Magic Numbers\" (Con số ma thuật) trong lập trình là gì, tại sao nó bị coi là một thói quen xấu (Code Smell) và cách khắc phục chuẩn nhất trong C# là gì?",
                new List<string>
                {
                    "A. Là những con số nguyên/số thực được viết trần trụi trực tiếp trong các câu lệnh điều kiện hoặc tính toán (như if (status == 2) hay price * 0.08m) khiến người đọc không hiểu con số đó mang ý nghĩa gì; khắc phục bằng cách thay thế chúng bằng kiểu liệt kê (Enum) hoặc Hằng số (const/readonly) có tên rõ nghĩa",
                    "B. Là những con số ngẫu nhiên dùng trong mật mã học",
                    "C. Là khóa chính tự tăng trong SQL Server",
                    "D. Là số lượng dòng code trong một file"
                },
                "A",
                "Loại bỏ Magic Numbers / Magic Strings bằng Enum và const vừa giúp code tự giải thích ý nghĩa của nó (Self-documenting code), vừa giúp khi thay đổi quy định (ví dụ đổi thuế từ 8% lên 10%) bạn chỉ cần sửa đúng 1 dòng khai báo hằng số."
            ),

            // =====================================================================
            // BÀI 16.8
            // =====================================================================
            Build(
                topicId,
                "16.8",
                "Đóng gói Hồ sơ Dự án lên GitHub Chuyên nghiệp (Cách viết README.md chuẩn Quốc tế, Sơ đồ Kiến trúc & Demo)",
                "Kiểm thử & Debug",
                "https://docs.github.com/en/repositories/managing-your-repositorys-settings-and-features/customizing-your-repository/about-readmes",
                """
                • **💡 1. Sự thật khi đi ứng tuyển Intern / Fresher .NET: "Nhà tuyển dụng nhìn vào file `README.md` trên GitHub của bạn đầu tiên!"**
                  - Hàng nghìn sinh viên đưa link GitHub vào CV, nhưng khi nhà tuyển dụng bấm vào thì **kho GitHub trống trơn không có nổi một dòng giới thiệu `README.md`** (hoặc chỉ có dòng chữ mặc định), không có ảnh chụp màn hình, không có hướng dẫn chạy! Nhà tuyển dụng sẽ đóng tab lại sau 3 giây vì họ không có thời gian tải code của bạn về cài SQL Server để chạy thử!
                  - Ngược lại, một kho GitHub có file **`README.md` được trình bày chuyên nghiệp** (có Ảnh chụp giao diện thực tế, Sơ đồ kiến trúc, Danh sách công nghệ sử dụng, Điểm nhấn kỹ thuật và Hướng dẫn chạy 1 chạm) sẽ khiến HR và Tech Lead ấn tượng mạnh ngay lập tức!
                • **🖼️ 2. Cấu trúc 6 Phần Bắt Buộc Của Một File `README.md` Điểm 10 Trên GitHub:**
                ```markdown
                # 🚀 Tên Dự Án (VD: Hoàng Việt - Nền Tảng Học Lập Trình .NET Tích Hợp AI Mentor 2D)
                > Mô tả ngắn 2 dòng về mục tiêu của dự án + Các nhãn Badges (.NET 10, EF Core, SQL Server, Gemini AI)

                ## 📸 1. Hình ảnh Giao diện Thực tế (Screenshots / GIF Demo)
                (Đặt 3-4 bức ảnh chụp màn hình đẹp nhất của trang web ngay ở đầu để người xem thấy thành quả luôn!)

                ## 🏗️ 2. Kiến trúc Hệ thống & Công nghệ sử dụng (Tech Stack & Architecture)
                - Backend: C# ASP.NET Core 10.0 MVC / Web API, Entity Framework Core (Code-First).
                - Database & Caching: SQL Server, IMemoryCache (24h Smart Hashing).
                - AI Integration: Google Gemini 2.5 Flash API (RAG Context Injection, Token Optimization).

                ## ✨ 3. Các Tính năng & Điểm nhấn Kỹ thuật Nổi bật (Key Technical Highlights)
                - Phân tách lộ trình kép, Hộp bước tương tác, Phòng Mentor AI 2D, Biểu đồ năng lực Chart.js...

                ## 🗄️ 4. Sơ đồ Cơ sở dữ liệu (Database Schema / ERD)

                ## ⚡ 5. Hướng dẫn Cài đặt & Chạy dự án trong 1 phút (Getting Started)
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo file README.md ở thư mục gốc với Badges và Điểm nhấn Kỹ thuật",
                        "📥 Đầu vào: Các công nghệ bạn đã dùng trong dự án (như dự án Hoàng Việt, Quản lý Thư viện, hoặc LAVITA). ⚙️ Cơ chế ngầm: Dùng cú pháp Markdown kết hợp thẻ Badge của Shields.io để làm nổi bật Tech Stack (.NET, C#, EF Core, SQL Server, Gemini AI). 📤 Đầu ra: Phần mở đầu README chuyên nghiệp như các dự án mã nguồn mở lớn.",
                        "# 🎓 Hoàng Việt - Nền Tảng Học Lập Trình Fullstack .NET & AI Mentor\n\n![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)\n![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp)\n![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?style=for-the-badge&logo=microsoft-sql-server)\n![Gemini AI](https://img.shields.io/badge/Google%20Gemini-8E75B2?style=for-the-badge&logo=googlebard)"),
                    Step(2,
                        "2. Đính kèm Ảnh chụp màn hình (Screenshots) trực quan vào README",
                        "📥 Đầu vào: Các ảnh chụp màn hình giao diện chạy thật của dự án. ⚙️ Cơ chế ngầm: Tạo thư mục docs/screenshots/ trong dự án (hoặc kéo thả ảnh trực tiếp vào khung soạn thảo README trên web GitHub) để nhà tuyển dụng nhìn thấy ngay giao diện mà không cần tải code về chạy! 📤 Đầu ra: Thu hút thị giác tức thì.",
                        "## 📸 Giao diện Hệ thống\n\n| Lộ trình & Hộp Bước Tương Tác | Phòng Mentor AI 2D & Biểu đồ Năng lực |\n| :---: | :---: |\n| ![Learning View](docs/images/screen-learning.png) | ![AI Mentor](docs/images/screen-ai-dashboard.png) |"),
                    Step(3,
                        "3. Viết Hướng dẫn chạy dự án rõ ràng từng câu lệnh (Quick Start)",
                        "📥 Đầu vào: Các bước để người khác kéo code về chạy được ngay. ⚙️ Cơ chế ngầm: Hướng dẫn rõ lệnh git clone, cách điền API Key vào user-secrets và lệnh dotnet ef database update. 📤 Đầu ra: Bất kỳ ai cũng chạy thử được dự án trong 60 giây.",
                        "## ⚡ Hướng dẫn Chạy Dự án (Quick Start)\n\n```bash\n# 1. Clone dự án về máy:\ngit clone https://github.com/hoangviet/HoangViet-LearningPlatform.git\ncd HoangViet-LearningPlatform\n\n# 2. Khởi tạo Database SQL Server LocalDB & Nạp sẵn 160 bài học:\ndotnet ef database update\n\n# 3. Chạy ứng dụng:\ndotnet run\n```")
                },
                "Khi đưa đường dẫn kho chứa GitHub của dự án cá nhân hoặc đồ án vào trong CV xin thực tập, yếu tố nào trong kho GitHub giúp nhà tuyển dụng (Tech Lead / HR) đánh giá cao năng lực của bạn ngay trong 30 giây đầu tiên mà chưa cần phải tải toàn bộ source code về máy họ để chạy?",
                new List<string>
                {
                    "A. Một file README.md được trình bày rõ ràng, có hình ảnh chụp màn hình (Screenshots/GIF) kết quả chạy thực tế, sơ đồ kiến trúc, liệt kê rõ các kỹ thuật đã áp dụng (EF Core, DI, Caching, AI...) cùng lịch sử Commit sạch sẽ và có file .gitignore chuẩn",
                    "B. Đẩy cả thư mục bin/ và obj/ nặng 500MB lên GitHub",
                    "C. Nén toàn bộ code thành 1 file .rar duy nhất rồi tải lên GitHub",
                    "D. Để kho GitHub ở chế độ Private (Riêng tư) khóa kín không cho nhà tuyển dụng xem"
                },
                "A",
                "Nhà tuyển dụng phải duyệt hàng trăm hồ sơ mỗi đợt, họ sẽ mở link GitHub để xem README.md, ảnh chụp giao diện và bấm vào xem thử 2-3 file Controller/Service tiêu biểu. Một README.md chỉnh chu kèm ảnh minh họa chứng tỏ tư duy làm sản phẩm nghiêm túc của bạn."
            ),

            // =====================================================================
            // BÀI 16.9
            // =====================================================================
            Build(
                topicId,
                "16.9",
                "Chiến thuật Viết CV .NET Developer Intern/Fresher & Cách mô tả Dự án theo Công thức STAR / XYZ",
                "Tư duy đọc luồng",
                "https://learn.microsoft.com/vi-vn/shows/career-development/",
                """
                • **💡 1. Tại sao cùng một dự án làm trên trường, có bạn viết vào CV nhìn rất tầm thường, có bạn viết vào lại được gọi đi phỏng vấn ngay?**
                  - Hãy xem sự khác biệt một trời một vực giữa 2 cách mô tả dự án trong CV:
                    + ❌ **Cách viết sơ sài (Kiểu liệt kê tên môn học):**
                      *"Dự án Quản lý Thư viện — Làm bằng C# MVC và SQL Server. Chức năng: Đăng nhập, thêm sửa xóa sách, mượn trả sách."* ➔ Nhà tuyển dụng thấy giống hàng nghìn sinh viên khác, không thấy được chiều sâu kỹ thuật!
                    + ✅ **Cách viết chuẩn Công thức `XYZ` của Google (*Làm được gì [X] - Đo lường thế nào [Y] - Bằng kỹ thuật chuyên sâu nào [Z]*):**
                      *"**Hệ thống Quản lý Thư viện (ASP.NET Core MVC & EF Core):** Dẫn dắt nhóm kỹ thuật (Git Flow, phân nhánh module), thiết kế CSDL chuẩn 3NF với **Entity Framework Core (Code-First Migrations)**; áp dụng **Generic Repository, Dependency Injection, Soft Delete (`HasQueryFilter`)**, tối ưu truy vấn bằng **`.AsNoTracking()`, Pagination** và xử lý giao dịch mượn/trả sách an toàn với **Database Transaction**."*
                • **🖼️ 2. Cấu trúc 4 Dự Án Đắt Giá Trong CV & Portfolio Của Nguyễn Hoàng Việt:**
                ```
                ┌──────────────────────────────────────────────────────────────────────────────┐
                │ 1. Dự án Web Học Tập Cá Nhân Hóa "Hoàng Việt" (ASP.NET Core 10.0 MVC & AI):  │
                │    - Điểm nhấn: Thiết kế 2 lộ trình (Thực tập & Chi tiết 16 chủ đề - 160 bài)│
                │      Hộp bước tương tác, Biểu đồ năng lực Chart.js, tích hợp Gemini Flash AI │
                │      với RAG Context Injection và tối ưu 90% Token bằng IMemoryCache SHA256. │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 2. Hệ thống Quản lý Thư viện Nhóm (ASP.NET Core MVC & SQL Server):           │
                │    - Điểm nhấn: Vai trò điều phối kỹ thuật nhóm (Git Branching/Merge),       │
                │      xây dựng Module nghiệp vụ, EF Core Code-First, phân quyền & Transaction.│
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 3. Ứng dụng Thương mại Điện tử Desktop "LAVITA" (C# .NET 6 WinForms & EF):   │
                │    - Điểm nhấn: Thiết kế DFD/ERD, Guna UI2, xuất hóa đơn PDF bằng QuestPDF,  │
                │      kiểm thử giao diện tự động (Automated UI Testing) với thư viện FlaUI.   │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 4. Ứng dụng Web E-Commerce Đa nền tảng (Java JSP/Servlet MVC, MySQL & AI):   │
                │    - Điểm nhấn: Chứng minh nền tảng MVC vững chắc, làm chủ cả SQL Server     │
                │      lẫn MySQL và kinh nghiệm tích hợp Gemini 2.5 Flash API đa ngôn ngữ.     │
                └──────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Cách phân nhóm mục `Technical Skills` (Kỹ năng kỹ thuật) trong CV sao cho khoa học nhất:**
                  - **Ngôn ngữ lập trình:** `C#`, `Java`, `C++`, `SQL`, `JavaScript`, `HTML5/CSS3`.
                  - **Frameworks & Nền tảng:** `.NET 6 / .NET 10`, `ASP.NET Core MVC`, `ASP.NET Core Web API`, `Entity Framework Core (Code-First)`, `LINQ`, `JSP/Servlet`, `Node.js`, `Bootstrap 5`.
                  - **Cơ sở dữ liệu:** `Microsoft SQL Server (SSMS, LocalDB)`, `MySQL`.
                  - **Công cụ & Kiểm thử:** `Git / GitHub (Git Flow)`, `Visual Studio`, `VS Code`, `Postman`, `Swagger UI (OpenAPI)`, `QuestPDF`, `FlaUI`, `Docker`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Trình bày mục Technical Skills phân nhóm rõ ràng, ưu tiên C# & .NET lên đầu",
                        "📥 Đầu vào: Danh sách các công nghệ bạn đã nắm vững. ⚙️ Cơ chế ngầm: Khi ứng tuyển vị trí .NET Developer Intern, luôn đặt C#, .NET, ASP.NET Core MVC / Web API, EF Core và SQL Server ở dòng đầu tiên của mỗi nhóm để hệ thống lọc CV (ATS) và mắt người đọc bắt trúng từ khóa trong 3 giây! 📤 Đầu ra: Mục Skills sắc bén.",
                        "TECHNICAL SKILLS:\n- Languages: C#, SQL, Java, C++, JavaScript, HTML5/CSS3.\n- Backend & Frameworks: .NET (6/8/10), ASP.NET Core MVC, ASP.NET Core Web API, Entity Framework Core (Code-First), LINQ, RESTful API, Dependency Injection.\n- Databases: SQL Server (SSMS), MySQL.\n- Tools & Testing: Git/GitHub, Postman, Swagger UI, Visual Studio, VS Code, FlaUI, QuestPDF."),
                    Step(2,
                        "2. Mô tả từng Dự án theo cấu trúc: Vai trò - Công nghệ - Điểm nhấn Kỹ thuật",
                        "📥 Đầu vào: Các dự án thực tế bạn đã làm. ⚙️ Cơ chế ngầm: Dùng các động từ hành động mạnh (Thiết kế, Xây dựng, Tối ưu, Tích hợp) và in đậm các từ khóa kỹ thuật đắt giá (EF Core, LINQ, Transaction, MemoryCache, Gemini API, QuestPDF). 📤 Đầu ra: Phần Projects thuyết phục tuyệt đối.",
                        "HOÀNG VIỆT - NỀN TẢNG HỌC LẬP TRÌNH .NET TÍCH HỢP AI MENTOR (10/2026)\n- Vai trò: Fullstack .NET Developer (Dự án cá nhân).\n- Công nghệ: C#, ASP.NET Core MVC, EF Core, SQL Server, Chart.js, Google Gemini API, IMemoryCache.\n- Điểm nhấn kỹ thuật:\n  + Thiết kế CSDL quan hệ quản lý 2 lộ trình học tập với hơn 160 bài học phân cấp và bài tập kiểm tra.\n  + Xây dựng kiến trúc Dependency Injection, tối ưu truy vấn đọc với AsNoTracking() và phân trang.\n  + Tích hợp trợ lý AI Mentor 2D sử dụng kỹ thuật Context Injection kết hợp bộ nhớ đệm IMemoryCache (SHA256 Hashing 24h) giúp giảm 90% chi phí Token và phản hồi tức thì."),
                    Step(3,
                        "3. Chuẩn bị kịch bản Demo 2 phút khi mang Laptop đi Phỏng vấn",
                        "📥 Đầu vào: Buổi phỏng vấn trực tiếp hoặc phỏng vấn Online chia sẻ màn hình. ⚙️ Cơ chế ngầm: Mở sẵn dự án Hoàng Việt và các dự án chính trên máy tính; khi người phỏng vấn hỏi về dự án, chủ động xin phép mở màn hình chạy thử luồng và bật code giải thích trực tiếp! 📤 Đầu ra: Tạo ấn tượng vượt trội so với mọi ứng viên chỉ nói miệng.",
                        "// Kịch bản Demo 2 phút chinh phục người phỏng vấn:\n// 1. Mở giao diện Web (30s): Cho xem luồng Hộp bước tương tác, Biểu đồ năng lực và AI Chatbot.\n// 2. Mở Visual Studio (60s): Chỉ rõ cấu trúc Program.cs (DI), Controller -> Service -> EF Core.\n// 3. Mở GitHub / Swagger (30s): Cho xem lịch sử quản lý nhánh Git của nhóm và cách test API.")
                },
                "Khi viết phần mô tả dự án (Project Description) trong CV ứng tuyển vị trí .NET Developer Intern, cách viết nào dưới đây gây ấn tượng mạnh nhất với người phỏng vấn kỹ thuật (Tech Lead / Senior Developer)?",
                new List<string>
                {
                    "A. Nêu rõ kiến trúc và các kỹ thuật chuyên sâu đã trực tiếp triển khai trong dự án (ví dụ: áp dụng Dependency Injection, EF Core Code-First Migrations, tối ưu LINQ với AsNoTracking & Pagination, xử lý Database Transaction, tích hợp Caching và xuất PDF bằng QuestPDF)",
                    "B. Chỉ ghi đúng tên dự án và dòng chữ 'Làm web bán hàng có đăng nhập'",
                    "C. Copy nguyên 500 dòng code C# dán vào trong tờ CV",
                    "D. Chỉ liệt kê màu sắc của các nút bấm trên trang web"
                },
                "A",
                "Người phỏng vấn kỹ thuật đọc CV để tìm kiếm các \"từ khóa năng lực\" (DI, EF Core, LINQ, AsNoTracking, Transaction, RESTful, Git Flow). Khi bạn nêu rõ các kỹ thuật này trong phần mô tả dự án, họ sẽ hỏi xoáy vào chính những phần bạn đã nắm chắc nhất!"
            ),

            // =====================================================================
            // BÀI 16.10
            // =====================================================================
            Build(
                topicId,
                "16.10",
                "Tổng kết Toàn bộ 16 Chủ đề & Bộ 20 Câu hỏi Phỏng vấn Thực tập .NET Trọng tâm Nhất (Sẵn sàng Chinh phục Kỳ Thực Tập!)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/",
                """
                • **💡 1. Chúc mừng Nguyễn Hoàng Việt đã hoàn thành trọn vẹn 100% Lộ trình 16 Chủ đề Fullstack .NET!**
                  - Bạn hiện đã sở hữu trong tay một hệ thống kiến thức bài bản, xuyên suốt từ nền tảng ngôn ngữ C# đến kiến trúc doanh nghiệp hiện đại.
                  - Để giúp bạn tự tin tuyệt đối khi bước vào phòng phỏng vấn thực tập tháng 11, dưới đây là **Bản Tóm Tắt Vàng 10 Câu Hỏi Phỏng Vấn C# .NET Xuất Hiện Nhiều Nhất (99% công ty đều hỏi ít nhất 5 câu trong bảng này!)**:
                • **📊 2. Bảng Tóm Tắt "10 Câu Hỏi Phỏng Vấn C# .NET Bất Bại" (Đọc Lướt 5 Phút Trước Giờ Phỏng Vấn):**
                | Câu hỏi phỏng vấn thường gặp | Từ khóa trả lời cốt lõi ghi điểm tuyệt đối (Chuẩn Senior) |
                | :--- | :--- |
                | **1. `Value Type` vs `Reference Type`?** | `Value Type` (`int, decimal, bool, struct, enum`) lưu trực tiếp giá trị trên **Stack**; `Reference Type` (`class, string, array, interface`) lưu địa chỉ trên Stack trỏ tới đối tượng thật trên **Heap**. |
                | **2. Bốn tính chất `OOP` áp dụng thực tế thế nào?**| **Đóng gói** (`private` field + `public` Property); **Kế thừa** (`: ControllerBase`, `: DbContext`); **Đa hình** (`virtual/override` `OnModelCreating`); **Trừu tượng** (`Interface` tiêm qua DI). |
                | **3. `Abstract Class` khác gì `Interface`?** | Một class chỉ kế thừa được **1 `Abstract Class`** (quan hệ *Is-A*, chứa được cả field và hàm có sẵn code), nhưng thực thi được **nhiều `Interface`** (bản hợp đồng dùng cho Dependency Injection). |
                | **4. `IEnumerable` khác gì `IQueryable`?** | Cả hai đều có *Deferred Execution* (chỉ chạy khi gọi `.ToList()`). Nhưng `IEnumerable` lọc dữ liệu **trên RAM máy chủ Web**, còn `IQueryable` dịch biểu thức thành **câu lệnh SQL lọc ngay tại SQL Server**! |
                | **5. `FirstOrDefault` khác gì `SingleOrDefault`?**| Cả hai đều trả về `null` nếu không tìm thấy. Nhưng nếu có **từ 2 bản ghi trở lên trùng điều kiện**, `FirstOrDefault` lấy dòng đầu tiên (`TOP 1`), còn `SingleOrDefault` **văng lỗi Exception** (`TOP 2`)! |
                | **6. Tại sao phải dùng `async / await`?** | Giúp **giải phóng luồng xử lý (`Thread`) về lại `ThreadPool`** trong thời gian chờ đợi I/O (chờ SQL Server, chờ gọi API mạng), giúp Server phục vụ hàng nghìn Request đồng thời không bị nghẽn. |
                | **7. Phân biệt `Transient`, `Scoped`, `Singleton`?**| **`Scoped`:** 1 Instance cho mỗi 1 HTTP Request (dùng cho `DbContext`, `Repository`, `Service`). **`Transient`:** Tạo mới mỗi lần gọi. **`Singleton`:** 1 Instance duy nhất từ lúc bật đến lúc tắt Server (`MemoryCache`). |
                | **8. Làm sao tối ưu tốc độ `EF Core`?** | Dùng **`.AsNoTracking()`** cho truy vấn đọc, dùng **`.Select()` (Projection)** sang DTO chỉ lấy đúng cột cần dùng, tránh lỗi **N+1 Queries** bằng `.Include()`, phân trang `.Skip().Take()` và đánh **Index** cột tìm kiếm/khóa ngoại. |
                | **9. Phân biệt `401 Unauthorized` và `403 Forbidden`?**| **`401`:** Chưa xác thực danh tính (*Authentication* - chưa đăng nhập hoặc Token hết hạn/sai). **`403`:** Đã đăng nhập hợp lệ nhưng không đủ quyền (*Authorization* - không đúng Role/Policy). |
                | **10. Tại sao dùng `DTO` mà không trả thẳng `Entity`?**| Để **bảo mật** (ẩn cột nhạy cảm như `PasswordHash`), tránh lỗi **Mass Assignment**, tránh lỗi **vòng lặp JSON (`Object Cycle`)** và tách biệt cấu trúc bảng DB với dữ liệu giao tiếp API. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Ôn tập Nền tảng C#, OOP, LINQ & Async/Await (Chủ đề 1 -> 4)",
                        "📥 Đầu vào: Các câu hỏi lý thuyết cốt lõi về ngôn ngữ C#. ⚙️ Cơ chế ngầm: Nắm chắc Stack vs Heap, 4 tính chất OOP, Interface vs Abstract Class, IQueryable vs IEnumerable và quy tắc Async/Await. 📤 Đầu ra: Vượt qua vòng hỏi lý thuyết dễ dàng.",
                        "// Tư duy cốt lõi:\n// 1. Tiền tệ luôn dùng decimal; Kiểm tra chuỗi dùng string.IsNullOrWhiteSpace.\n// 2. Luôn lọc .Where() trên IQueryable trước khi gọi await .ToListAsync(ct)."),
                    Step(2,
                        "2. Ôn tập SQL Server, EF Core & Kiến trúc Web API (Chủ đề 7 -> 9)",
                        "📥 Đầu vào: Các câu hỏi về Database và thiết kế Backend. ⚙️ Cơ chế ngầm: Nắm chắc PK/FK, 3NF, INNER JOIN vs LEFT JOIN, Index, AsNoTracking, Soft Delete, RESTful API và Vòng đời DI (Scoped, Transient, Singleton). 📤 Đầu ra: Tự tin giải thích mọi dòng code trong dự án.",
                        "// Luồng chuẩn mực:\n// Client ➔ Middleware ➔ Controller (DTO) ➔ IService ➔ IRepository (EF Core) ➔ SQL Server"),
                    Step(3,
                        "3. Ôn tập Bảo mật, Kiểm thử, Full Stack, Docker & AI (Chủ đề 10 -> 15)",
                        "📥 Đầu vào: Các câu hỏi nâng cao tạo sự khác biệt. ⚙️ Cơ chế ngầm: Hiểu rõ Password Hashing + Salt, Cookie vs JWT + Refresh Token, Unit Test (xUnit/Moq), Docker Compose và tích hợp Gemini AI kèm MemoryCache. 📤 Đầu ra: Điểm cộng vượt trội so với các ứng viên khác.",
                        "// Điểm nhấn nâng cao:\n// - Bảo mật: PasswordHasher/BCrypt, JWT + Refresh Token, User Secrets.\n// - Tối ưu & Hiện đại: IMemoryCache/Redis, QuestPDF, RAG AI Mentor."),
                    Step(4,
                        "4. Thực hành Phỏng vấn thử cùng Phòng Mentor AI 2D ngay trên Web Hoàng Việt!",
                        "📥 Đầu vào: Toàn bộ kiến thức bạn đã tích lũy trên hệ thống. ⚙️ Cơ chế ngầm: Mở '🤖 Phòng Mentor AI 2D' trên thanh Header, chọn chế độ 'Phỏng vấn thử C# Intern' để luyện phản xạ trả lời câu hỏi và đánh dấu hoàn thành 100% lộ trình! 📤 Đầu ra: Sẵn sàng bước vào kỳ thực tập và đạt kết quả xuất sắc!",
                        "Console.WriteLine(\"Chúc mừng Nguyễn Hoàng Việt đã hoàn thành 100% Lộ trình Fullstack .NET!\");")
                },
                "(Câu hỏi Tổng kết Cuối khóa) Khi đi phỏng vấn vị trí .NET Developer Intern, nhà tuyển dụng đưa ra một tình huống tổng hợp: \"Giả sử anh giao cho em viết một API lấy danh sách Lịch sử mượn sách của độc giả đang đăng nhập (có lọc theo ngày và phân trang). Em hãy trình bày ngắn gọn cách em thiết kế API đó từ đầu đến cuối sao cho vừa bảo mật, vừa tối ưu hiệu năng?\". Câu trả lời nào dưới đây thể hiện đẳng cấp của một thực tập sinh xuất sắc nhất?",
                new List<string>
                {
                    "A. Em sẽ gắn [Authorize] lên hàm GET trong Controller và lấy ReaderId trực tiếp từ Claims trong Token của người đang đăng nhập (chống lỗi xem trộm IDOR); sau đó truyền xuống Service và Repository truy vấn trên IQueryable với .AsNoTracking(), lọc .Where() theo ReaderId và khoảng ngày, đếm CountAsync(), cắt trang bằng .Skip().Take(), dùng .Select() chiếu thẳng sang BorrowHistoryDto chỉ lấy các cột cần thiết rồi mới gọi await .ToListAsync(ct) trả về PagedResult chuẩn!",
                    "B. Em bỏ thẻ [Authorize], dùng SELECT * kéo toàn bộ bảng phiếu mượn của tất cả mọi người trong thư viện lên RAM bằng .ToList() rồi dùng vòng lặp for tìm",
                    "C. Em viết toàn bộ câu lệnh kết nối SQL trực tiếp vào trong file giao diện HTML",
                    "D. Em bảo người dùng tự vào SQL Server mà xem"
                },
                "A",
                "Câu trả lời A kết hợp hoàn hảo toàn bộ kiến thức của cả khóa học trong 1 luồng thực tế: Bảo mật ([Authorize] + lấy UserId từ Claim chống IDOR) + Kiến trúc phân tầng (Controller ➔ Service ➔ Repository) + Tối ưu EF Core (IQueryable, .AsNoTracking(), .Skip().Take(), Projection .Select() sang DTO) + Bất đồng bộ (await ... ToListAsync(ct))."
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

        var topic16 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 16);
        if (topic16 == null)
            return;

        topic16.Title = "Chủ đề 16: Xây dựng dự án thực tế cuối khóa (Clean Architecture, Capstone Project, Portfolio & Phỏng vấn)";
        topic16.Description = "10 Bài học con siêu chi tiết từ Phân tích SRS/Use Case/ERD, Clean Architecture 4 tầng, 5 nguyên lý SOLID & CQRS, Base Infrastructure (AuditableEntity & Soft Delete Interceptor), Luồng nghiệp vụ đa bảng (Transaction), Admin Dashboard, Clean Code & Refactoring, GitHub README.md chuẩn quốc tế, Chiến thuật CV .NET Intern (STAR/XYZ) đến Tổng kết 20 câu hỏi phỏng vấn trọng tâm.";

        var targetLessons = GetLessons(topic16.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic16.Id)
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
