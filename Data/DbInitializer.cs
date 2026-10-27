using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        // 1. Seed 6 nhóm kỹ năng mặc định 0%
        if (!await context.SkillMetrics.AnyAsync())
        {
            var skills = new List<SkillMetric>
            {
                new() { SkillName = "Tư duy đọc luồng", CurrentScore = 0, MaxScore = 100 },
                new() { SkillName = "C# toàn diện", CurrentScore = 0, MaxScore = 100 },
                new() { SkillName = "OOP thực tế", CurrentScore = 0, MaxScore = 100 },
                new() { SkillName = "SQL & EF Core", CurrentScore = 0, MaxScore = 100 },
                new() { SkillName = "Web API", CurrentScore = 0, MaxScore = 100 },
                new() { SkillName = "Kiểm thử & Debug", CurrentScore = 0, MaxScore = 100 }
            };
            context.SkillMetrics.AddRange(skills);
            await context.SaveChangesAsync();
        }

        // 2. Seed 2 Lộ trình (Tracks), 21 Chủ đề (5 Thực tập + 16 Chi tiết) và các Bài học con (SubLessons)
        if (!await context.Tracks.AnyAsync())
        {
            var internTrack = new Track
            {
                Code = "INTERN_TRACK",
                Name = "Lộ trình Đi Thực Tập (5 Chủ đề cốt lõi)",
                Description = "Chuyên sâu Đọc Code & Đọc Luồng BE/Web API thực chiến tại doanh nghiệp"
            };

            var detailedTrack = new Track
            {
                Code = "DETAILED_TRACK",
                Name = "Lộ trình Học Chi Tiết (Đầy đủ 16 Chủ đề)",
                Description = "Học bài bản toàn diện Fullstack .NET 10 từ gốc C# đến Kiến trúc, Docker & AI"
            };

            context.Tracks.AddRange(internTrack, detailedTrack);
            await context.SaveChangesAsync();

            // --- 5 CHỦ ĐỀ CỦA MENU 1: LỘ TRÌNH ĐI THỰC TẬP ---
            var internTopics = new List<Topic>
            {
                new()
                {
                    TrackId = internTrack.Id,
                    TopicNumber = 1,
                    Title = "Chủ đề 1 (Thực tập): Giải phẫu Luồng Chạy Web API & Đọc Hiểu Kiến Trúc Dự Án",
                    Description = "10 Bài học con giúp đọc hiểu luồng đi từ HTTP Request qua Controller, DTO, Service, Repository đến SQL Server."
                },
                new()
                {
                    TrackId = internTrack.Id,
                    TopicNumber = 2,
                    Title = "Chủ đề 2 (Thực tập): C#, OOP, LINQ & Bất Đồng Bộ (Async/Await) Khi Đọc Code Công Ty",
                    Description = "9 Bài học con giải mã Interface, Dependency Injection, LINQ, IQueryable, Async/Await và Middleware."
                },
                new()
                {
                    TrackId = internTrack.Id,
                    TopicNumber = 3,
                    Title = "Chủ đề 3 (Thực tập): Làm Chủ Entity Framework Core, SQL Server & Tối Ưu Truy Vấn",
                    Description = "Đọc hiểu Fluent API, Migration, N+1 Query, AsNoTracking, Transaction và tối ưu truy vấn thực tế."
                },
                new()
                {
                    TrackId = internTrack.Id,
                    TopicNumber = 4,
                    Title = "Chủ đề 4 (Thực tập): Bóc Tách Từng Mắt Xích Luồng ASP.NET Core Web API (Client ➔ Controller ➔ Service ➔ Repository ➔ DB)",
                    Description = "10 Bài học con chuyên sâu bóc tách Request Lifecycle, Routing, Middleware Pipeline, Thin Controller, ApiResponse Wrapper, AutoMapper, Repository & UnitOfWork, Global Exception Handling & Service Lifetimes."
                },
                new()
                {
                    TrackId = internTrack.Id,
                    TopicNumber = 5,
                    Title = "Chủ đề 5 (Thực tập): Thực Hành Kiểm Thử API (Swagger, Postman), Đọc Luồng Xác Thực JWT & Kỹ Năng Đi Làm Thực Tế",
                    Description = "10 Bài học con thực hành kiểm thử API với Swagger & Postman, giải mã JWT Bearer Token, [Authorize], Claims, CORS, Breakpoint Debugging, đọc Log/StackTrace, Git Flow và Checklist hoàn thành Task thực tập."
                }
            };

            // --- 16 CHỦ ĐỀ CỦA MENU 2: LỘ TRÌNH HỌC CHI TIẾT ---
            var detailedTopics = new List<Topic>
            {
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 1,
                    Title = "Chủ đề 1: Giới thiệu về C# và lập trình .NET",
                    Description = "13 Bài học con chi tiết từ Kiến trúc .NET (CLR, BCL), Biến & Kiểu dữ liệu, Bộ nhớ Stack/Heap, Nullable, Ép kiểu, Toán tử, Chuỗi, Ngày tháng, Rẽ nhánh, Vòng lặp, Mảng/Enum đến Phương thức (Method)."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 2,
                    Title = "Chủ đề 2: Lập trình hướng đối tượng trong C# (OOP)",
                    Description = "11 Bài học con chi tiết từ Tư duy OOP (Class/Object), Đóng gói (Field/Property/init), Phạm vi truy cập, Constructor & GC, Kế thừa, Đa hình, Trừu tượng (Abstract Class vs Interface), Has-A vs Is-A, static & Extension Methods, Class vs Struct vs Record đến Bài tập tổng kết."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 3,
                    Title = "Chủ đề 3: Làm việc với Collection và LINQ",
                    Description = "12 Bài học con siêu chi tiết từ Non-Generic vs Generic (Boxing/Unboxing), List<T> (Count vs Capacity), Dictionary<TKey,TValue> O(1), HashSet/Queue/Stack, Delegate/Func/Action, Lambda Expression đến trọn bộ LINQ thực chiến."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 4,
                    Title = "Chủ đề 4: Xử lý ngoại lệ và lập trình bất đồng bộ",
                    Description = "10 Bài học con siêu chi tiết từ Cây phân cấp Exception & StackTrace, Try-Catch-Finally, throw vs throw ex & Custom Exception, IDisposable & using, Synchronous vs Asynchronous, Task/Task<T>/async/await, Task.WhenAll/WhenAny, CancellationToken đến phòng chống Deadlock (.Result/.Wait)."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 5,
                    Title = "Chủ đề 5: Quản lý source code với Git",
                    Description = "10 Bài học con siêu chi tiết từ 3 Vùng trạng thái Git (Working Directory, Staging, Repository), .gitignore chuẩn .NET, Remote GitHub (clone, push, pull, fetch), Branching (switch/checkout), Merge vs Rebase, xử lý Merge Conflict, Git Flow & Pull Request, Git Stash đến git restore/reset/revert."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 6,
                    Title = "Chủ đề 6: Xây dựng giao diện với Blazor và Bootstrap",
                    Description = "10 Bài học con siêu chi tiết từ HTML5 Semantic & DOM, CSS3 Box Model/Flexbox/Grid, Bootstrap 5 Grid 12 cột & Utility Classes, Kiến trúc _Layout.cshtml/@RenderBody, Razor (@model/ViewBag/TempData), Tag Helpers (asp-for), Blazor Component (.razor) đến Fetch API (AJAX)."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 7,
                    Title = "Chủ đề 7: Làm việc với cơ sở dữ liệu SQL Server",
                    Description = "10 Bài học con siêu chi tiết từ Thiết kế RDBMS & ERD (PK, FK, 1-N, N-N), Chuẩn hóa 1NF-2NF-3NF & Kiểu dữ liệu SQL-C#, Thứ tự thực thi SELECT, INNER/LEFT/SELF JOIN, GROUP BY & WHERE vs HAVING, Subquery/CTE/Window Functions, Clustered vs Non-Clustered Index, View/Stored Procedure/Function đến Transaction ACID, Trigger & chống SQL Injection."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 8,
                    Title = "Chủ đề 8: Entity Framework Core và ORM",
                    Description = "10 Bài học con siêu chi tiết từ Bản chất ORM (DbContext/DbSet), Data Annotations & Fluent API (IEntityTypeConfiguration), Quan hệ 1-1/1-N/N-N & DeleteBehavior, Migrations, Eager/Explicit/Lazy Loading & N+1, Change Tracker & ExecuteUpdate/Delete, Soft Delete & HasQueryFilter, Raw SQL & Concurrency đến Generic Repository & Unit of Work."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 9,
                    Title = "Chủ đề 9: Xây dựng ứng dụng với ASP.NET Core Web API",
                    Description = "10 Bài học con siêu chi tiết từ RESTful API & Idempotency, [ApiController] & Content Negotiation, Route Constraints, IValidatableObject & FluentValidation, IExceptionHandler & ProblemDetails (RFC 7807), Action Filters, CORS Policy, API Versioning & OpenAPI/Swagger đến Output Caching & Rate Limiting."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 10,
                    Title = "Chủ đề 10: Bảo mật ứng dụng với Authentication và Authorization",
                    Description = "10 Bài học con siêu chi tiết từ Authentication vs Authorization (Claims, ClaimsIdentity, ClaimsPrincipal), Password Hashing & Salt (PasswordHasher/BCrypt), Cookie Authentication, ASP.NET Core Identity, JWT Bearer chuyên sâu, Access Token & Refresh Token Rotation, RBAC vs Policy-Based Authorization, OAuth2/OIDC (Google Login) đến phòng chống OWASP Top 10."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 11,
                    Title = "Chủ đề 11: Kiểm thử ứng dụng và tối ưu hóa hiệu suất",
                    Description = "10 Bài học con siêu chi tiết từ Kim tự tháp Kiểm thử & Quy tắc 3A, xUnit ([Fact]/[Theory]/[InlineData]), Mocking với Moq, Integration Test (InMemory DB & WebApplicationFactory), Tự động hóa Postman, Profiling (SQL Logging/Stopwatch/Health Checks), Compiled Queries & IAsyncEnumerable, Brotli/Gzip & IHttpClientFactory đến phòng chống Memory Leak & Captive Dependency."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 12,
                    Title = "Chủ đề 12: Phát triển ứng dụng Web Full Stack",
                    Description = "10 Bài học con siêu chi tiết từ Kiến trúc Monolithic MVC vs Decoupled Frontend-Backend, Typed HttpClient & Fetch API, DelegatingHandler tự gắn JWT, State Management (Loading/Toast/Cart), Debounce Search & Phân trang, Upload File (Multipart), SignalR Real-time, Chart.js Dashboard, Xuất Excel/PDF (ClosedXML/QuestPDF) đến Checklist ghép nối Full Stack từ A-Z."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 13,
                    Title = "Chủ đề 13: Triển khai ứng dụng với CI/CD và Docker",
                    Description = "10 Bài học con siêu chi tiết từ dotnet publish & Kestrel/Reverse Proxy, Cấu hình đa môi trường & Environment Variables, Bản chất Docker, Multi-Stage Dockerfile & Layer Caching, Docker Compose (Web + SQL Server + Volumes), Bản chất CI/CD, GitHub Actions Workflow, Database Migrations trên Production (Idempotent Script), Nginx + HTTPS đến Checklist DevOps trọn gói."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 14,
                    Title = "Chủ đề 14: Tích hợp dịch vụ bên thứ ba (Email, Payment Gateway, Cloud Storage, Redis, Hangfire & Resilience)",
                    Description = "10 Bài học con siêu chi tiết từ Adapter Pattern & Options Pattern, Gửi Email HTML (MailKit), Cổng thanh toán (VNPAY/HMAC-SHA512), Cloud Storage (Cloudinary/S3), Redis (IDistributedCache), Background Jobs (Hangfire), Resilience (Polly), Webhooks Idempotency, Refit REST Client đến Checklist tích hợp đa dịch vụ."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 15,
                    Title = "Chủ đề 15: Tích hợp Trí tuệ Nhân tạo (AI & LLM) trong ứng dụng .NET",
                    Description = "10 Bài học con siêu chi tiết từ Microsoft.Extensions.AI & Semantic Kernel, Gọi trực tiếp Google Gemini API (Typed HttpClient), Prompt Engineering & Context Injection, Multi-turn Chat History (Sliding Window), Structured Outputs (JSON Schema), Streaming (IAsyncEnumerable), RAG với SQL Server, Vector Embeddings & CosineSimilarity, Function Calling đến Kiến trúc ViệtBot 2D thực chiến."
                },
                new()
                {
                    TrackId = detailedTrack.Id,
                    TopicNumber = 16,
                    Title = "Chủ đề 16: Xây dựng dự án thực tế cuối khóa (Clean Architecture, Capstone Project, Portfolio & Phỏng vấn)",
                    Description = "10 Bài học con siêu chi tiết từ Phân tích SRS/Use Case/ERD, Clean Architecture 4 tầng, 5 nguyên lý SOLID & CQRS, Base Infrastructure (AuditableEntity & Soft Delete Interceptor), Luồng nghiệp vụ đa bảng (Transaction), Admin Dashboard, Clean Code & Refactoring, GitHub README.md chuẩn quốc tế, Chiến thuật CV .NET Intern (STAR/XYZ) đến Tổng kết 20 câu hỏi phỏng vấn trọng tâm."
                }
            };

            context.Topics.AddRange(internTopics);
            context.Topics.AddRange(detailedTopics);
            await context.SaveChangesAsync();

            // --- SEED ĐẦY ĐỦ BÀI HỌC CON TOÀN KHÓA ---
            var allSubLessons = new List<SubLesson>();

            // Menu 1 - Chủ đề 1 (10 bài), Chủ đề 2 (9 bài), Chủ đề 3 (10 bài), Chủ đề 4 (10 bài) & Chủ đề 5 (10 bài)
            allSubLessons.AddRange(InternTopic1Seed.GetLessons(internTopics[0].Id));
            allSubLessons.AddRange(InternTopic2Seed.GetLessons(internTopics[1].Id));
            allSubLessons.AddRange(InternTopic3Seed.GetLessons(internTopics[2].Id));
            allSubLessons.AddRange(InternTopic4Seed.GetLessons(internTopics[3].Id));
            allSubLessons.AddRange(InternTopic5Seed.GetLessons(internTopics[4].Id));

            // Menu 2 - Trọn bộ 16 Chủ đề (166 bài học chi tiết)
            allSubLessons.AddRange(DetailedTopic1Seed.GetLessons(detailedTopics[0].Id));
            allSubLessons.AddRange(DetailedTopic2Seed.GetLessons(detailedTopics[1].Id));
            allSubLessons.AddRange(DetailedTopic3Seed.GetLessons(detailedTopics[2].Id));
            allSubLessons.AddRange(DetailedTopic4Seed.GetLessons(detailedTopics[3].Id));
            allSubLessons.AddRange(DetailedTopic5Seed.GetLessons(detailedTopics[4].Id));
            allSubLessons.AddRange(DetailedTopic6Seed.GetLessons(detailedTopics[5].Id));
            allSubLessons.AddRange(DetailedTopic7Seed.GetLessons(detailedTopics[6].Id));
            allSubLessons.AddRange(DetailedTopic8Seed.GetLessons(detailedTopics[7].Id));
            allSubLessons.AddRange(DetailedTopic9Seed.GetLessons(detailedTopics[8].Id));
            allSubLessons.AddRange(DetailedTopic10Seed.GetLessons(detailedTopics[9].Id));
            allSubLessons.AddRange(DetailedTopic11Seed.GetLessons(detailedTopics[10].Id));
            allSubLessons.AddRange(DetailedTopic12Seed.GetLessons(detailedTopics[11].Id));
            allSubLessons.AddRange(DetailedTopic13Seed.GetLessons(detailedTopics[12].Id));
            allSubLessons.AddRange(DetailedTopic14Seed.GetLessons(detailedTopics[13].Id));
            allSubLessons.AddRange(DetailedTopic15Seed.GetLessons(detailedTopics[14].Id));
            allSubLessons.AddRange(DetailedTopic16Seed.GetLessons(detailedTopics[15].Id));

            context.SubLessons.AddRange(allSubLessons);
            await context.SaveChangesAsync();
        }
        else
        {
            // Đảm bảo cập nhật chính xác Chủ đề 1..5 của Menu 1 và Chủ đề 1..16 của Menu 2 trên Database hiện có
            await InternTopic1Seed.EnsureSeededAsync(context);
            await InternTopic2Seed.EnsureSeededAsync(context);
            await InternTopic3Seed.EnsureSeededAsync(context);
            await InternTopic4Seed.EnsureSeededAsync(context);
            await InternTopic5Seed.EnsureSeededAsync(context);
            await DetailedTopic1Seed.EnsureSeededAsync(context);
            await DetailedTopic2Seed.EnsureSeededAsync(context);
            await DetailedTopic3Seed.EnsureSeededAsync(context);
            await DetailedTopic4Seed.EnsureSeededAsync(context);
            await DetailedTopic5Seed.EnsureSeededAsync(context);
            await DetailedTopic6Seed.EnsureSeededAsync(context);
            await DetailedTopic7Seed.EnsureSeededAsync(context);
            await DetailedTopic8Seed.EnsureSeededAsync(context);
            await DetailedTopic9Seed.EnsureSeededAsync(context);
            await DetailedTopic10Seed.EnsureSeededAsync(context);
            await DetailedTopic11Seed.EnsureSeededAsync(context);
            await DetailedTopic12Seed.EnsureSeededAsync(context);
            await DetailedTopic13Seed.EnsureSeededAsync(context);
            await DetailedTopic14Seed.EnsureSeededAsync(context);
            await DetailedTopic15Seed.EnsureSeededAsync(context);
            await DetailedTopic16Seed.EnsureSeededAsync(context);
        }
    }
}
