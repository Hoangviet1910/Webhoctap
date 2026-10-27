using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic8Seed
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
            // BÀI 8.1
            // =====================================================================
            Build(
                topicId,
                "8.1",
                "Bản chất ORM (Object-Relational Mapping) và Kiến trúc của Entity Framework Core (DbContext & DbSet)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/",
                """
                • **💡 1. Hiểu bản chất của `ORM` và `EF Core` trong 30 giây:**
                  - Thế giới của **C#** quản lý dữ liệu bằng **Đối tượng (`Class / Object`)**, còn thế giới của **SQL Server** quản lý dữ liệu bằng **Bảng, Hàng và Cột (`Tables, Rows, Columns`)**. Hai thế giới này nói 2 ngôn ngữ khác nhau!
                  - Ngày xưa (thời `ADO.NET` thuần), lập trình viên phải tự mở kết nối, tự viết chuỗi `"SELECT * FROM..."`, rồi tự đọc từng cột gán bằng tay vào từng biến C# cực kỳ vất vả và dễ gõ sai tên cột.
                  - **ORM (`Object-Relational Mapping` — Ánh xạ Quan hệ - Đối tượng)** mà đại diện chính thức của Microsoft là **Entity Framework Core (EF Core)** đóng vai trò là **"Người phiên dịch tự động 2 chiều"**: Bạn chỉ cần làm việc 100% với các đối tượng C# (`_db.Books.Where(...)`), EF Core sẽ tự động dịch ra câu lệnh SQL gửi xuống Database, rồi tự động đóng gói kết quả SQL trả về thành `List<Book>` cho bạn!
                • **🖼️ 2. Sơ đồ Ánh Xạ 1 - 1 Giữa Thế Giới C# (`EF Core`) và Thế Giới `SQL Server`:**
                ```
                Thế giới C# (.NET Code)                     Bộ máy dịch                Thế giới SQL Server (Database)
                ┌────────────────────────────────────┐      ╔═══════════╗      ┌────────────────────────────────────┐
                │ Class kế thừa : DbContext          │ ◄══► ║           ║ ◄══► │ Nguyên 1 Database (HoangVietDb)    │
                ├────────────────────────────────────┤      ║           ║      ├────────────────────────────────────┤
                │ Thuộc tính: DbSet<Book> Books      │ ◄══► ║  EF CORE  ║ ◄══► │ Bảng: [dbo].[Books]                │
                ├────────────────────────────────────┤      ║   (ORM)   ║      ├────────────────────────────────────┤
                │ Class Entity: public class Book    │ ◄══► ║           ║ ◄══► │ Cấu trúc các Cột của bảng Books    │
                ├────────────────────────────────────┤      ║           ║      ├────────────────────────────────────┤
                │ 1 Đối tượng: new Book { Id = 1 }   │ ◄══► ╚═══════════╝ ◄══► │ 1 Dòng bản ghi (Row) trong bảng    │
                └────────────────────────────────────┘                         └────────────────────────────────────┘
                ```
                • **📊 3. Hai trường phái tiếp cận trong EF Core: `Code-First` vs `Database-First`:**
                | Tiêu chí | `Code-First` (Viết code C# trước $\rightarrow$ Sinh ra DB) | `Database-First` (Có sẵn DB SQL $\rightarrow$ Sinh ra code C#) |
                | :--- | :--- | :--- |
                | **Quy trình làm** | Viết các class `Entity` và `DbContext` bằng C# trước $\rightarrow$ Gõ lệnh `Add-Migration` & `Update-Database` để EF Core tự tạo bảng trong SQL Server! | Mở SSMS tạo bảng SQL trước $\rightarrow$ Gõ lệnh `Scaffold-DbContext` để EF Core tự đọc DB và đẻ ra các file `.cs`! |
                | **Ưu điểm & Xu hướng**| ⭐ **Chiếm 90% các dự án .NET hiện đại!** Dễ dàng quản lý lịch sử thay đổi bảng qua Git cùng với code C#. | Dùng khi công ty đã có sẵn một Database cũ khổng lồ từ nhiều năm trước. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Class Entity đại diện cho Bảng (Book.cs)",
                        "📥 Đầu vào: Các thông tin của một cuốn sách. ⚙️ Cơ chế ngầm: Mặc định EF Core nhìn thấy thuộc tính tên là 'Id' (hoặc 'BookId') sẽ tự động hiểu ngay đây là Khóa chính (Primary Key) của bảng! 📤 Đầu ra: Bản thiết kế bảng bằng C#.",
                        "public class Book {\n    public int Id { get; set; } // Tự động nhận diện là Primary Key (IDENTITY)\n    public string Title { get; set; } = string.Empty;\n    public decimal Price { get; set; }\n}"),
                    Step(2,
                        "2. Tạo Class AppDbContext kế thừa từ DbContext",
                        "📥 Đầu vào: Class Book vừa tạo. ⚙️ Cơ chế ngầm: Khai báo thuộc tính DbSet<Book> Books để đăng ký bảng Books với EF Core, và truyền DbContextOptions lên lớp cha qua : base(options). 📤 Đầu ra: Trạm quản lý kết nối Database.",
                        "public class AppDbContext : DbContext {\n    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }\n\n    public DbSet<Book> Books { get; set; } // Đại diện cho bảng Books trong SQL\n}"),
                    Step(3,
                        "3. Đăng ký AppDbContext vào Program.cs (Dependency Injection)",
                        "📥 Đầu vào: Chuỗi kết nối DefaultConnection trong appsettings.json. ⚙️ Cơ chế ngầm: Gọi AddDbContext với UseSqlServer để tiêm AppDbContext (vòng đời Scoped) vào mọi Repository/Service cần dùng. 📤 Đầu ra: Sẵn sàng truy vấn DB.",
                        "builder.Services.AddDbContext<AppDbContext>(options =>\n    options.UseSqlServer(builder.Configuration.GetConnectionString(\"DefaultConnection\")));")
                },
                "Trong Entity Framework Core, theo quy ước mặc định (Convention), khi bạn tạo một class Entity tên là Student, thuộc tính có tên như thế nào sẽ được EF Core tự động nhận diện là Khóa chính (Primary Key) ngay cả khi bạn không gắn thẻ [Key]?",
                new List<string>
                {
                    "A. Thuộc tính đặt tên là 'Id' hoặc '<Tên_Class>Id' (ví dụ: 'StudentId')",
                    "B. Thuộc tính đặt tên là 'PrimaryKey'",
                    "C. Thuộc tính đầu tiên bất kỳ trong class",
                    "D. Thuộc tính có kiểu dữ liệu là string"
                },
                "A",
                "Theo quy ước của EF Core (Convention over Configuration), bất kỳ thuộc tính nào mang tên Id hoặc <ClassName>Id đều tự động được thiết lập làm Primary Key (và nếu có kiểu int/long sẽ tự động bật IDENTITY(1,1) trong SQL Server)."
            ),

            // =====================================================================
            // BÀI 8.2
            // =====================================================================
            Build(
                topicId,
                "8.2",
                "Cấu hình Bảng chuyên sâu: Kết hợp Data Annotations và Fluent API (OnModelCreating & IEntityTypeConfiguration)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/modeling/",
                """
                • **💡 1. Tại sao đã có `Data Annotations` (`[Required]`, `[MaxLength]`) rồi mà vẫn phải học `Fluent API`?**
                  - **Data Annotations:** Viết nhanh gọn ngay trên đầu thuộc tính của class Entity, nhưng làm class Entity bị rối và **không thể cấu hình được những thứ phức tạp** (như Khóa chính kép *Composite Key*, đánh *Index* nhiều cột, cấu hình hành vi xóa *Cascade/Restrict*).
                  - **Fluent API (Viết trong hàm `OnModelCreating`):** Có quyền lực **cao nhất (ghi đè lên cả Data Annotations)**, làm được 100% mọi tính năng của SQL Server và giữ cho các class Entity sạch bóng!
                • **🖼️ 2. Tổ chức Fluent API chuẩn Doanh nghiệp với `IEntityTypeConfiguration<T>`:**
                ```
                Nếu dự án có 50 bảng mà bạn viết cấu hình Fluent API của cả 50 bảng dồn hết vào trong hàm
                OnModelCreating của file AppDbContext.cs thì file đó sẽ dài 2.000 dòng!
                ──► Giải pháp chuẩn: Tách cấu hình của mỗi bảng ra 1 file riêng (VD: BookConfiguration.cs)
                    thực thi IEntityTypeConfiguration<Book>, rồi trong OnModelCreating chỉ cần gọi ĐÚNG 1 DÒNG:
                    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
                ```
                • **📊 3. Bảng Đối Chiếu Cú Pháp Giữa `Data Annotations` và `Fluent API`:**
                | Mục đích cấu hình | Dùng `Data Annotations` (Trên thuộc tính) | Dùng `Fluent API` (Trong `OnModelCreating`) |
                | :--- | :--- | :--- |
                | **Đặt tên bảng SQL** | `[Table("tb_Books")]` | `builder.ToTable("tb_Books");` |
                | **Khóa chính (`PK`)** | `[Key]` | `builder.HasKey(b => b.Id);` (Hoặc khóa kép: `b => new { b.OrderId, b.BookId }`) |
                | **Bắt buộc & Độ dài (`NVARCHAR`)**| `[Required]`, `[MaxLength(150)]` | `builder.Property(b => b.Title).IsRequired().HasMaxLength(150);` |
                | **Chuỗi không dấu (`VARCHAR`)** | `[Column(TypeName = "varchar(20)")]` | `builder.Property(b => b.Code).HasMaxLength(20).IsUnicode(false);` |
                | **Kiểu Tiền tệ (`DECIMAL`)** | `[Column(TypeName = "decimal(18,2)")]`| `builder.Property(b => b.Price).HasPrecision(18, 2);` |
                | **Đánh Index chống trùng** | `[Index(nameof(Code), IsUnique = true)]`| `builder.HasIndex(b => b.Code).IsUnique();` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo file cấu hình riêng cho bảng Book (BookConfiguration.cs)",
                        "📥 Đầu vào: Class Entity Book sạch sẽ. ⚙️ Cơ chế ngầm: Thực thi giao diện IEntityTypeConfiguration<Book> để gom toàn bộ quy tắc cột, kiểu dữ liệu, Index của bảng Books vào 1 file riêng. 📤 Đầu ra: Cấu hình chuẩn mực, dễ bảo trì.",
                        "public class BookConfiguration : IEntityTypeConfiguration<Book> {\n    public void Configure(EntityTypeBuilder<Book> builder) {\n        builder.ToTable(\"Books\");\n        builder.HasKey(b => b.Id);\n        builder.Property(b => b.BookCode).IsRequired().HasMaxLength(20).IsUnicode(false); // VARCHAR(20)\n        builder.Property(b => b.Title).IsRequired().HasMaxLength(200); // NVARCHAR(200)\n        builder.Property(b => b.Price).HasPrecision(18, 2);\n        builder.HasIndex(b => b.BookCode).IsUnique();\n    }\n}"),
                    Step(2,
                        "2. Tự động nạp toàn bộ file cấu hình trong AppDbContext",
                        "📥 Đầu vào: Hàm OnModelCreating trong AppDbContext.cs. ⚙️ Cơ chế ngầm: Lệnh ApplyConfigurationsFromAssembly tự động quét khắp project, tìm tất cả các class thực thi IEntityTypeConfiguration và áp dụng chúng trong 1 dòng lệnh! 📤 Đầu ra: File AppDbContext luôn ngắn gọn.",
                        "protected override void OnModelCreating(ModelBuilder modelBuilder) {\n    base.OnModelCreating(modelBuilder);\n    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);\n}"),
                    Step(3,
                        "3. Nạp dữ liệu mẫu ban đầu (Data Seeding) bằng HasData",
                        "📥 Đầu vào: Muốn khi tạo bảng xong có sẵn ngay 2 Thể loại mặc định. ⚙️ Cơ chế ngầm: Dùng hàm builder.HasData(...) trong Fluent API (chú ý bắt buộc phải điền rõ giá trị Id). 📤 Đầu ra: Dữ liệu mẫu được nhúng thẳng vào file Migration.",
                        "builder.HasData(\n    new Category { Id = 1, Name = \"Công nghệ thông tin\" },\n    new Category { Id = 2, Name = \"Kinh tế - Quản trị\" }\n);")
                },
                "Trong Fluent API của Entity Framework Core, khi cấu hình một thuộc tính kiểu string bằng lệnh builder.Property(x => x.Code).HasMaxLength(20).IsUnicode(false);, lệnh .IsUnicode(false) sẽ tạo ra cột có kiểu dữ liệu gì trong SQL Server?",
                new List<string>
                {
                    "A. Tạo ra cột kiểu VARCHAR(20) (chuỗi không dấu, tiết kiệm một nửa dung lượng lưu trữ so với NVARCHAR)",
                    "B. Tạo ra cột kiểu NVARCHAR(20)",
                    "C. Tạo ra cột kiểu INT",
                    "D. Báo lỗi vì chuỗi luôn phải là Unicode"
                },
                "A",
                "Mặc định mọi thuộc tính string trong C# đều được EF Core dịch thành NVARCHAR (IsUnicode(true)). Khi gọi .IsUnicode(false), EF Core sẽ dịch cột đó thành VARCHAR trong SQL Server — rất tối ưu cho các cột Mã code, Số điện thoại, Email."
            ),

            // =====================================================================
            // BÀI 8.3
            // =====================================================================
            Build(
                topicId,
                "8.3",
                "Thiết lập Mối quan hệ giữa các Bảng trong EF Core (1-1, 1-N, N-N, Navigation Properties & DeleteBehavior)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/modeling/relationships",
                """
                • **💡 1. Hiểu bản chất của `Navigation Property` (Thuộc tính điều hướng) trong 30 giây:**
                  - Trong bảng SQL Server chỉ có cột số nguyên `CategoryId` (Khóa ngoại). Nhưng trong class C# của EF Core, ngoài biến `public int CategoryId { get; set; }`, chúng ta luôn khai báo thêm biến đối tượng **`public Category Category { get; set; }`** (ở bảng Con) và danh sách **`public ICollection<Book> Books { get; set; }`** (ở bảng Cha).
                  - Hai thuộc tính đối tượng đó gọi là **Navigation Properties (Thuộc tính điều hướng)**: Chúng giống như chiếc cầu nối giúp bạn đứng từ đối tượng Sách chấm thẳng sang tên Thể loại (`book.Category.Name`) mà không cần tự viết câu lệnh `JOIN`!
                • **🖼️ 2. Sơ đồ Cấu Hình Quan Hệ `1 - Nhiều` và Ý Nghĩa Của `OnDelete(DeleteBehavior...)`:**
                ```csharp
                // Bảng Cha (Category - Một):                 // Bảng Con (Book - Nhiều):
                public class Category {                       public class Book {
                    public int Id { get; set; }                   public int Id { get; set; }
                    public string Name { get; set; }              public int CategoryId { get; set; } // Khóa ngoại (FK)
                    // 1 Danh mục có NHIỀU Sách:                  // 1 Sách thuộc về 1 Danh mục:
                    public ICollection<Book> Books {get;set;}     public Category Category { get; set; }
                }                                             }

                // Cấu hình Fluent API:
                builder.HasOne(b => b.Category)           // 1 Cuốn sách thuộc về 1 Thể loại
                       .WithMany(c => c.Books)            // 1 Thể loại có nhiều Cuốn sách
                       .HasForeignKey(b => b.CategoryId)  // Nối qua khóa ngoại CategoryId
                       .OnDelete(DeleteBehavior.Restrict);// ⚠️ CHẶN không cho xóa Thể loại nếu đang có Sách bên trong!
                ```
                • **📊 3. Bảng Phân Biệt 3 Chế Độ Xóa (`DeleteBehavior`) Cực Quan Trọng Trong Thực Tế:**
                | Chế độ `DeleteBehavior` | Chuyện gì xảy ra khi xóa bản ghi Cha (`Category`)? | Khi nào nên dùng? |
                | :--- | :--- | :--- |
                | **`Cascade` (Mặc định của EF)**| Xóa Cha phát là **XÓA SẠCH LUÔN tất cả các bản ghi Con** đi kèm! | Chỉ dùng cho quan hệ phụ thuộc chặt (như xóa *Phiếu mượn* thì xóa luôn *Chi tiết phiếu mượn*). |
                | **`Restrict`** / `NoAction` | **CHẶN LẠI BÁO LỖI**, không cho phép xóa Cha nếu bên dưới vẫn đang còn bản ghi Con! | ⭐ **Khuyên dùng cho Danh mục, Khách hàng, Nhân viên** (Tránh lỡ tay xóa 1 Danh mục làm bay màu 500 cuốn sách!). |
                | **`SetNull`** | Cho phép xóa Cha, và tự động đổi cột khóa ngoại `CategoryId` của các con thành `NULL`. | Khi khóa ngoại của bảng con cho phép rỗng (`int? CategoryId`). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết lập quan hệ 1 - Nhiều (Category - Books)",
                        "📥 Đầu vào: Class Category và Class Book. ⚙️ Cơ chế ngầm: Khai báo cặp Navigation Properties ở 2 đầu và cấu hình HasOne - WithMany với DeleteBehavior.Restrict để bảo vệ dữ liệu sách. 📤 Đầu ra: Quan hệ 1-N chuẩn doanh nghiệp.",
                        "builder.HasOne(b => b.Category)\n       .WithMany(c => c.Books)\n       .HasForeignKey(b => b.CategoryId)\n       .OnDelete(DeleteBehavior.Restrict);"),
                    Step(2,
                        "2. Thiết lập quan hệ Nhiều - Nhiều có bảng trung gian tường minh",
                        "📥 Đầu vào: Bảng PhieuMuon (Borrow) và bảng Sach (Book) có thêm cột Số lượng mượn và Tình trạng sách lúc trả. ⚙️ Cơ chế ngầm: Tạo class thực thể trung gian BorrowDetail có Khóa chính kép (BorrowId, BookId) và nối 2 quan hệ 1-Nhiều vào bảng trung gian này. 📤 Đầu ra: Quản lý được các cột phụ trên bảng trung gian.",
                        "// Tại BorrowDetailConfiguration:\nbuilder.HasKey(bd => new { bd.BorrowId, bd.BookId }); // Khóa chính kép\n\nbuilder.HasOne(bd => bd.Borrow)\n       .WithMany(b => b.BorrowDetails)\n       .HasForeignKey(bd => bd.BorrowId);\n\nbuilder.HasOne(bd => bd.Book)\n       .WithMany(bk => bk.BorrowDetails)\n       .HasForeignKey(bd => bd.BookId);"),
                    Step(3,
                        "3. Thiết lập quan hệ 1 - 1 (User - UserProfile)",
                        "📥 Đầu vào: Class User và Class UserProfile. ⚙️ Cơ chế ngầm: Dùng HasOne - WithOne và bắt buộc chỉ định rõ bảng nào giữ khóa ngoại thông qua HasForeignKey<UserProfile>(p => p.UserId). 📤 Đầu ra: Quan hệ 1-1 chuẩn xác.",
                        "builder.HasOne(u => u.Profile)\n       .WithOne(p => p.User)\n       .HasForeignKey<UserProfile>(p => p.UserId);")
                },
                "Mặc định trong Entity Framework Core, đối với khóa ngoại bắt buộc (int CategoryId không có dấu ?), hành vi xóa được đặt là DeleteBehavior.Cascade (nếu xóa 1 Danh mục sẽ tự động xóa sạch toàn bộ Sản phẩm thuộc danh mục đó). Để ngăn chặn việc lỡ tay xóa Danh mục làm mất Sản phẩm, bạn nên đổi cấu hình trong Fluent API thành gì?",
                new List<string>
                {
                    "A. Đổi thành .OnDelete(DeleteBehavior.Restrict) (hoặc DeleteBehavior.NoAction) để hệ thống chặn lại nếu danh mục đó vẫn đang chứa sản phẩm",
                    "B. Đổi thành DeleteBehavior.Cascade",
                    "C. Xóa khóa ngoại đi",
                    "D. Không có cách nào chặn được"
                },
                "A",
                "Thiết lập DeleteBehavior.Restrict tạo ràng buộc khóa ngoại không cho phép xóa bản ghi cha khi vẫn còn bản ghi con đang tham chiếu tới nó, bảo vệ an toàn dữ liệu tuyệt đối."
            ),

            // =====================================================================
            // BÀI 8.4
            // =====================================================================
            Build(
                topicId,
                "8.4",
                "Quản lý Vòng đời Migrations chuyên nghiệp (Add-Migration, Update-Database, Rollback, Script & Xử lý lỗi)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/managing-schemas/migrations/",
                """
                • **💡 1. Hiểu bản chất của `Migrations` trong 30 giây:**
                  - Khi bạn sửa class C# (ví dụ: thêm thuộc tính `public string Publisher { get; set; }` vào `Book.cs`), bảng `Books` trong SQL Server **chưa hề biết gì về sự thay đổi này**!
                  - **Migration** chính là **"Bản kế hoạch nâng cấp Database"**: Khi bạn gõ lệnh tạo Migration, EF Core sẽ so sánh code C# hiện tại với ảnh chụp cũ (`ModelSnapshot`) để sinh ra một file C# chứa 2 hàm:
                    + **Hàm `Up(MigrationBuilder)`:** Chứa lệnh nâng cấp (VD: `AddColumn("Publisher")`).
                    + **Hàm `Down(MigrationBuilder)`:** Chứa lệnh quay lui khôi phục lại như cũ nếu bạn đổi ý (VD: `DropColumn("Publisher")`).
                • **🖼️ 2. Sơ đồ Quy Trình 3 Bước Của `EF Core Migrations`:**
                ```
                [1. Sửa code Entity / Fluent API]
                        │
                        ▼  Gõ lệnh: dotnet ef migrations add Ten_Thay_Doi
                [2. Sinh ra file Migration (Up / Down) & Cập nhật ModelSnapshot]
                        │  (Mở file Migration ra kiểm tra xem lệnh tạo bảng/cột đã đúng ý chưa!)
                        ▼  Gõ lệnh: dotnet ef database update
                [3. Áp dụng xuống SQL Server & Ghi tên vào bảng lịch sử __EFMigrationsHistory]
                ```
                • **📊 3. Bảng Đối Chiếu Toàn Bộ Lệnh `Migrations` Giữa `Terminal (.NET CLI)` và `Package Manager Console (Visual Studio)`:**
                | Mục đích thao tác | Gõ trên `Terminal` (VS Code / CMD) | Gõ trên `Package Manager Console` (Visual Studio) |
                | :--- | :--- | :--- |
                | **1. Tạo Migration mới** | `dotnet ef migrations add <Tên>` | `Add-Migration <Tên>` |
                | **2. Cập nhật xuống SQL Server**| `dotnet ef database update` | `Update-Database` |
                | **3. Xóa Migration vừa tạo (khi chưa update DB)**| `dotnet ef migrations remove` | `Remove-Migration` |
                | **4. Lùi DB về lại một mốc Migration cũ (Rollback)**| `dotnet ef database update <Tên_Mốc_Cũ>`| `Update-Database <Tên_Mốc_Cũ>` |
                | **5. Xuất ra file `.sql` để nộp bài / chạy trên Server**| `dotnet ef migrations script` | `Script-Migration` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Migration và kiểm tra file sinh ra",
                        "📥 Đầu vào: Vừa thêm cột ISBN vào class Book.cs. ⚙️ Cơ chế ngầm: EF Core so sánh với AppDbContextModelSnapshot và tạo file 20261025_AddBookIsbn.cs chứa hàm Up và Down. 📤 Đầu ra: File kế hoạch nâng cấp DB.",
                        "dotnet ef migrations add AddBookIsbn\n// Hoặc trên PMC của Visual Studio:\nAdd-Migration AddBookIsbn"),
                    Step(2,
                        "2. Thực thi cập nhật lên SQL Server (Update-Database)",
                        "📥 Đầu vào: File Migration AddBookIsbn. ⚙️ Cơ chế ngầm: EF Core kiểm tra bảng __EFMigrationsHistory trong SQL Server, thấy bản AddBookIsbn chưa chạy nên dịch hàm Up() thành lệnh ALTER TABLE Books ADD ISBN... và thực thi. 📤 Đầu ra: Bảng SQL có thêm cột mới mà không mất dữ liệu cũ!",
                        "dotnet ef database update\n// Hoặc trên PMC:\nUpdate-Database"),
                    Step(3,
                        "3. Cách xử lý đúng khi lỡ tạo Migration sai hoặc muốn quay lui",
                        "📥 Đầu vào: Phát hiện mình đặt sai kiểu dữ liệu của cột ISBN sau khi đã chạy Update-Database. ⚙️ Cơ chế ngầm: Tuyệt đối KHÔNG bấm chuột phải xóa tay file Migration! Phải dùng lệnh lùi DB về mốc trước đó (kích hoạt hàm Down), rồi dùng lệnh remove để gỡ bỏ an toàn! 📤 Đầu ra: Sửa lại code và tạo Migration mới sạch sẽ.",
                        "// Bước 3.1: Lùi Database về lại mốc Migration liền trước:\ndotnet ef database update InitialCreate\n\n// Bước 3.2: Gỡ bỏ file Migration bị sai (đồng thời trả lại Snapshot cũ):\ndotnet ef migrations remove")
                },
                "Trong SQL Server của một dự án dùng EF Core Code-First, bạn luôn nhìn thấy một bảng hệ thống tự động sinh ra có tên là __EFMigrationsHistory. Bảng này có nhiệm vụ gì?",
                new List<string>
                {
                    "A. Lưu danh sách tên các bản Migration đã được chạy trên Database đó; nhờ bảng này mà khi bạn gõ Update-Database, EF Core biết chính xác những Migration nào đã chạy rồi để bỏ qua và chỉ chạy những Migration mới",
                    "B. Lưu mật khẩu đăng nhập của người dùng",
                    "C. Lưu lịch sử chat của AI",
                    "D. Lưu các bản ghi đã bị xóa"
                },
                "A",
                "Bảng __EFMigrationsHistory chính là cuốn sổ nhật ký đồng bộ giữa code C# và SQL Server. Mỗi khi một Migration chạy thành công, tên của nó (MigrationId) được ghi vào bảng này để không bao giờ bị chạy trùng lặp lần 2."
            ),

            // =====================================================================
            // BÀI 8.5
            // =====================================================================
            Build(
                topicId,
                "8.5",
                "So sánh 3 Cơ chế Tải dữ liệu liên kết: Eager Loading (Include), Explicit Loading và Lazy Loading (Cùng lỗi kinh điển N+1 Queries)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/related-data/",
                """
                • **💡 1. Hiểu bản chất 3 cơ chế tải dữ liệu bảng liên kết trong 30 giây (Câu hỏi phỏng vấn 99% gặp):**
                  Khi lấy một cuốn sách `Book` từ DB lên, mặc định EF Core chỉ lấy các cột của bảng `Books`, còn thuộc tính đối tượng `book.Category` sẽ bị để là **`null`** (để tiết kiệm băng thông). Muốn lấy cả thông tin `Category`, có 3 cách:
                  1. **`Eager Loading` (Tải háo hức — Dùng `.Include()` / `.ThenInclude()`):** Lấy luôn cả Sách lẫn Thể loại trong **1 câu lệnh `JOIN` duy nhất ngay từ đầu**. *(Đây là cách khuyên dùng số 1 trong Web API & MVC!)*.
                  2. **`Explicit Loading` (Tải tường minh — Dùng `_db.Entry(book).Reference(...).LoadAsync()`):** Lúc đầu chỉ lấy Sách; lát nữa ở dòng code bên dưới nếu cần dùng đến Thể loại mới chủ động gọi lệnh tải riêng Thể loại của cuốn sách đó lên.
                  3. **`Lazy Loading` (Tải lười biếng — Gắn từ khóa `virtual` lên Navigation Property):** Lúc đầu chỉ lấy Sách; nhưng hễ trong code hoặc ngoài View `.cshtml` bạn chấm tới `book.Category.Name`, EF Core sẽ **âm thầm bắn thêm 1 câu lệnh SQL phụ** xuống Database để lấy Thể loại!
                • **🖼️ 2. Tại sao `Lazy Loading` lại là thủ phạm gây ra thảm họa hiệu năng `N + 1 Queries Problem`?**
                ```
                Giả sử bạn bật Lazy Loading và hiển thị danh sách 50 cuốn sách ra bảng HTML:
                1. Câu lệnh SQL đầu tiên (1): SELECT TOP(50) * FROM Books; (Lấy 50 cuốn sách)
                2. Ra ngoài vòng lặp @foreach (50 vòng), mỗi dòng bạn in @item.Category.Name:
                   ├── Vòng 1: Bắn câu SQL thứ 2 hỏi Category của sách 1
                   ├── Vòng 2: Bắn câu SQL thứ 3 hỏi Category của sách 2
                   │   ...
                   └── Vòng 50: Bắn câu SQL thứ 51 hỏi Category của sách 50!
                ──► Tổng cộng tốn 1 + 50 = 51 CÂU LỆNH SQL chỉ để hiện 1 trang web! (Đó gọi là lỗi N + 1 Queries!)
                ──► Nếu dùng Eager Loading (.Include(b => b.Category)): Chỉ tốn ĐÚNG 1 CÂU LỆNH SQL JOIN duy nhất!
                ```
                • **📊 3. Kỹ thuật `AsSplitQuery()` khi phải `.Include()` nhiều danh sách con (`1 - N`) cùng lúc:**
                  - Nếu 1 cuốn sách bạn vừa `.Include(b => b.Authors)` vừa `.Include(b => b.Reviews)` (nối nhiều bảng 1-N cùng lúc), câu lệnh `JOIN` sẽ bị nhân bản dữ liệu chéo (*Cartesian Explosion*). Thêm **`.AsSplitQuery()`** giúp EF Core tách thành 2 câu `SELECT` gọn nhẹ, chạy nhanh hơn rất nhiều!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Eager Loading chuẩn mực với Include và ThenInclude",
                        "📥 Đầu vào: Cần lấy thông tin Phiếu mượn kèm thông tin Độc giả và danh sách Chi tiết sách mượn. ⚙️ Cơ chế ngầm: EF Core dịch thành câu lệnh SQL JOIN lấy toàn bộ dữ liệu liên kết chỉ trong 1 lần đi về Database. 📤 Đầu ra: Không bao giờ bị lỗi N+1.",
                        "var borrow = await _db.Borrows\n    .Include(b => b.Reader)              // Nối bảng Readers\n    .Include(b => b.BorrowDetails)       // Nối bảng BorrowDetails\n        .ThenInclude(bd => bd.Book)      // Từ BorrowDetails nối tiếp sang bảng Books!\n    .FirstOrDefaultAsync(b => b.Id == id);"),
                    Step(2,
                        "2. Chống bùng nổ dữ liệu chéo bằng .AsSplitQuery()",
                        "📥 Đầu vào: Include nhiều danh sách Collection cùng lúc. ⚙️ Cơ chế ngầm: Thay vì ghép thành 1 câu JOIN khổng lồ bị lặp dòng, AsSplitQuery tách thành các câu SELECT riêng biệt cho từng bảng con. 📤 Đầu ra: Tối ưu băng thông truyền tải từ SQL.",
                        "var category = await _db.Categories\n    .Include(c => c.Books)\n    .Include(c => c.Promotions)\n    .AsSplitQuery()\n    .FirstOrDefaultAsync(c => c.Id == id);"),
                    Step(3,
                        "3. Tối ưu tối thượng bằng Projection (.Select sang DTO/ViewModel)",
                        "📥 Đầu vào: Chỉ cần lấy vài cột của bảng cha và bảng con để hiển thị lên bảng. ⚙️ Cơ chế ngầm: Dùng .Select() nhặt đúng cột cần dùng. EF Core tự động JOIN ngầm và chỉ lấy đúng các cột đó mà không cần viết .Include()! 📤 Đầu ra: Nhanh nhất trong mọi phương pháp.",
                        "var bookList = await _db.Books\n    .AsNoTracking()\n    .Select(b => new BookViewModel {\n        Id = b.Id,\n        Title = b.Title,\n        CategoryName = b.Category.Name // Tự động JOIN ngầm cực nhẹ!\n    })\n    .ToListAsync();")
                },
                "Lỗi hiệu năng kinh điển \"N + 1 Queries Problem\" trong ORM / Entity Framework Core xảy ra khi nào và cách khắc phục chuẩn nhất là gì?",
                new List<string>
                {
                    "A. Xảy ra khi lấy danh sách N bản ghi cha (1 câu SQL), sau đó trong vòng lặp duyệt qua từng bản ghi cha lại gọi truy vấn lấy dữ liệu bảng con (tốn thêm N câu SQL nữa); khắc phục bằng cách dùng Eager Loading (.Include) hoặc Projection (.Select) để lấy hết trong 1 lần truy vấn",
                    "B. Xảy ra khi viết câu lệnh SQL dài quá 10 dòng",
                    "C. Xảy ra khi bảng có nhiều hơn 1 cột khóa ngoại",
                    "D. Xảy ra khi đặt tên biến có chữ N"
                },
                "A",
                "Hiểu và phòng tránh lỗi N + 1 Queries (bằng cách tắt Lazy Loading và chủ động dùng .Include() hoặc .Select()) là kỹ năng quan trọng hàng đầu mà các công ty kiểm tra khi phỏng vấn Backend Developer."
            ),

            // =====================================================================
            // BÀI 8.6
            // =====================================================================
            Build(
                topicId,
                "8.6",
                "Vòng đời Entity State trong Change Tracker & Thao tác Hàng loạt siêu tốc (ExecuteUpdateAsync & ExecuteDeleteAsync)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/saving/execute-insert-update-delete",
                """
                • **💡 1. Hiểu bản chất 5 trạng thái (`EntityState`) trong bộ theo dõi `Change Tracker` của EF Core:**
                  Mỗi một đối tượng Entity khi làm việc với `DbContext` luôn mang 1 trong 5 trạng thái sau:
                  1. **`Detached` (Đứng ngoài cuộc):** Đối tượng vừa mới `new` ra trên RAM hoặc lấy lên bằng `.AsNoTracking()` — `DbContext` không hề theo dõi nó.
                  2. **`Unchanged` (Nguyên vẹn):** Đối tượng vừa được đọc từ SQL Server lên, chưa bị sửa thuộc tính nào.
                  3. **`Added` (Chờ thêm mới):** Vừa gọi `_db.Books.Add(book)` $\rightarrow$ Khi `SaveChangesAsync()` sẽ sinh lệnh **`INSERT`**.
                  4. **`Modified` (Đã bị chỉnh sửa):** Đã sửa giá trị thuộc tính (VD: `book.Price = 200;`) $\rightarrow$ Khi `SaveChangesAsync()` sẽ sinh lệnh **`UPDATE`**.
                  5. **`Deleted` (Chờ xóa):** Vừa gọi `_db.Books.Remove(book)` $\rightarrow$ Khi `SaveChangesAsync()` sẽ sinh lệnh **`DELETE`**.
                • **🖼️ 2. Vũ khí mới cực mạnh của EF Core 7 / 8 / 9 / 10: `ExecuteUpdateAsync` & `ExecuteDeleteAsync` (Bulk Operations):**
                ```
                Bài toán: "Tăng giá toàn bộ 10.000 cuốn sách thuộc thể loại CNTT thêm 10%".

                [Cách cũ truyền thống (Rất chậm)]:
                1. Phải kéo cả 10.000 cuốn sách từ SQL lên RAM (ToListAsync)
                2. Chạy vòng lặp foreach 10.000 vòng để cộng giá
                3. Gọi SaveChangesAsync() -> Bắn 10.000 câu lệnh UPDATE đơn lẻ xuống SQL Server!

                [Cách mới hiện đại với ExecuteUpdateAsync (Nhanh gấp 100 lần!)]:
                Chỉ bắn ĐÚNG 1 CÂU LỆNH SQL UPDATE trực tiếp xuống SQL Server mà KHÔNG CẦN kéo dữ liệu lên RAM:
                await _db.Books
                    .Where(b => b.CategoryId == 1)
                    .ExecuteUpdateAsync(s => s.SetProperty(b => b.Price, b => b.Price * 1.1m));
                ```
                • **📊 3. Bảng So Sánh `SaveChangesAsync()` Truyền Thống vs `ExecuteUpdateAsync / ExecuteDeleteAsync`:**
                | Tiêu chí | Dùng `FindAsync` ➔ Sửa/Xóa ➔ `SaveChangesAsync()` | Dùng `ExecuteUpdateAsync()` / `ExecuteDeleteAsync()` |
                | :--- | :--- | :--- |
                | **Số lần gọi SQL** | 2 lần (1 câu `SELECT` lấy lên + $N$ câu `UPDATE/DELETE`). | **ĐÚNG 1 LẦN DUY NHẤT** (Chạy thẳng lệnh `UPDATE/DELETE WHERE...` dưới DB!). |
                | **Có cần gọi `SaveChangesAsync` không?**| **Bắt buộc phải gọi.** | **KHÔNG CẦN!** Lệnh chạy trực tiếp ngay lập tức xuống SQL Server! |
                | **Khi nào sử dụng?** | Khi thêm/sửa/xóa **1 bản ghi đơn lẻ** có kiểm tra nghiệp vụ phức tạp. | ⭐ Khi cần **Cập nhật hoặc Xóa hàng loạt nhiều bản ghi cùng lúc** theo điều kiện! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cập nhật 1 bản ghi đơn lẻ qua Change Tracker",
                        "📥 Đầu vào: Sửa thông tin cuốn sách có Id = 5. ⚙️ Cơ chế ngầm: Lấy sách lên (Trạng thái Unchanged) -> Gán giá trị mới (Tự chuyển sang Modified) -> Gọi SaveChangesAsync() để lưu. 📤 Đầu ra: Cập nhật chuẩn xác 1 đối tượng.",
                        "var book = await _db.Books.FindAsync(5);\nif (book != null) {\n    book.Title = \"Tên mới\"; // EntityState tự chuyển thành Modified\n    await _db.SaveChangesAsync();\n}"),
                    Step(2,
                        "2. Cập nhật hàng loạt siêu tốc với ExecuteUpdateAsync",
                        "📥 Đầu vào: Giảm giá 20% cho toàn bộ sách xuất bản trước năm 2024. ⚙️ Cơ chế ngầm: EF Core dịch thẳng thành 1 câu lệnh UPDATE Books SET Price = Price * 0.8 WHERE PublishYear < 2024 gửi xuống SQL và chạy ngay lập tức! 📤 Đầu ra: Cập nhật hàng nghìn dòng chỉ trong 5ms.",
                        "int updatedRows = await _db.Books\n    .Where(b => b.PublishYear < 2024)\n    .ExecuteUpdateAsync(setters => setters\n        .SetProperty(b => b.Price, b => b.Price * 0.8m));"),
                    Step(3,
                        "3. Xóa hàng loạt siêu tốc với ExecuteDeleteAsync",
                        "📥 Đầu vào: Dọn dẹp toàn bộ nhật ký hệ thống cũ hơn 1 năm trước. ⚙️ Cơ chế ngầm: Sinh trực tiếp lệnh DELETE FROM SystemLogs WHERE CreatedAt < ... mà không tốn 1 byte RAM nào để tải dữ liệu lên. 📤 Đầu ra: Xóa sạch trong chớp mắt.",
                        "int deletedRows = await _db.SystemLogs\n    .Where(log => log.CreatedAt < DateTime.Now.AddYears(-1))\n    .ExecuteDeleteAsync(); // Chạy ngay lập tức, không cần SaveChangesAsync!")
                },
                "Khi sử dụng phương thức mới await _db.Products.Where(p => p.Stock == 0).ExecuteDeleteAsync(); trong EF Core hiện đại, nhận định nào dưới đây là ĐÚNG?",
                new List<string>
                {
                    "A. EF Core sẽ dịch và thực thi trực tiếp một câu lệnh SQL 'DELETE FROM Products WHERE Stock = 0' ngay lập tức dưới Database mà không cần tải các bản ghi lên RAM và không cần gọi thêm SaveChangesAsync()",
                    "B. Bắt buộc phải gọi thêm lệnh await _db.SaveChangesAsync() ở dòng dưới thì dữ liệu mới bị xóa",
                    "C. Phương thức này chỉ xóa tạm trên RAM chứ không xóa trong SQL Server",
                    "D. Phương thức này chỉ xóa được tối đa 1 dòng"
                },
                "A",
                "ExecuteUpdateAsync và ExecuteDeleteAsync là các thao tác thực thi trực tiếp (Immediate Bulk Execution) ngay tại tầng Database, bỏ qua Change Tracker nên chạy cực nhanh và có hiệu lực tức thì mà không cần SaveChangesAsync()."
            ),

            // =====================================================================
            // BÀI 8.7
            // =====================================================================
            Build(
                topicId,
                "8.7",
                "Kỹ thuật Xóa mềm (Soft Delete) kết hợp Bộ lọc Toàn cục (Global Query Filters - HasQueryFilter)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/filters",
                """
                • **💡 1. Tại sao các hệ thống thực tế hầu như KHÔNG BAO GIỜ xóa thật (`DELETE`) dữ liệu khỏi Database?**
                  - Hãy tưởng tượng một cuốn sách trong thư viện đã từng có 100 sinh viên mượn trong quá khứ. Nếu hôm nay cuốn sách đó bị rách và thủ thư bấm nút *"Xóa sách"*, nếu bạn dùng lệnh `DELETE` xóa mất dòng sách đó khỏi bảng `Books`, thì toàn bộ 100 tờ phiếu mượn lịch sử trong quá khứ sẽ bị mất thông tin sách (hoặc bị lỗi khóa ngoại không cho xóa)!
                  - **Giải pháp chuẩn của mọi công ty — `Soft Delete` (Xóa mềm / Xóa logic):**
                    + Thêm một cột **`public bool IsDeleted { get; set; } = false;`** (hoặc `DateTime? DeletedAt`) vào bảng.
                    + Khi người dùng bấm nút *"Xóa"*, ta **KHÔNG xóa dòng đó**, mà chỉ cập nhật **`book.IsDeleted = true;`**! Dữ liệu vẫn nằm an toàn trong Database để tra cứu lịch sử, nhưng được ẩn đi trên giao diện!
                • **🖼️ 2. Vấn đề phát sinh và Giải pháp tự động 100% bằng `Global Query Filter` (`HasQueryFilter`):**
                ```
                Nếu dùng Soft Delete (IsDeleted = true), chẳng lẽ ở cả 200 hàm truy vấn trong dự án, hàm nào bạn
                cũng phải nhớ gõ tay .Where(x => !x.IsDeleted)? Nếu chỉ cần 1 hàm quên gõ là sách đã xóa lại hiện ra!

                ──► Giải pháp thần thánh của EF Core: Khai báo ĐÚNG 1 DÒNG trong OnModelCreating:
                    modelBuilder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);

                ──► Kể từ giây phút đó: Mọi câu lệnh _db.Books.ToListAsync() ở khắp mọi nơi trong dự án đều được
                    EF Core TỰ ĐỘNG gắn thêm đuôi "WHERE [IsDeleted] = 0" hoàn toàn tự động!
                ```
                • **📊 3. Khi muốn xem lại "Thùng rác" (Lấy cả những bản ghi đã bị xóa mềm) thì làm thế nào?**
                  - Rất đơn giản! Chỉ cần chèn thêm hàm **`.IgnoreQueryFilters()`** vào câu truy vấn:
                    `_db.Books.IgnoreQueryFilters().Where(b => b.IsDeleted).ToListAsync();`!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thêm cờ IsDeleted vào lớp cơ sở BaseEntity",
                        "📥 Đầu vào: Mọi bảng trong dự án đều cần tính năng Xóa mềm. ⚙️ Cơ chế ngầm: Đặt thuộc tính bool IsDeleted = false và DateTime? DeletedAt vào class cha BaseEntity. 📤 Đầu ra: Các bảng con tự động có cột IsDeleted.",
                        "public abstract class BaseEntity {\n    public int Id { get; set; }\n    public bool IsDeleted { get; set; } = false;\n    public DateTime? DeletedAt { get; set; }\n}"),
                    Step(2,
                        "2. Bật bộ lọc tự động toàn cục bằng HasQueryFilter",
                        "📥 Đầu vào: Hàm OnModelCreating trong AppDbContext. ⚙️ Cơ chế ngầm: Đăng ký HasQueryFilter(b => !b.IsDeleted). Từ nay mọi câu lệnh SELECT trên bảng Books đều tự động lọc bỏ các dòng đã xóa mềm. 📤 Đầu ra: Không bao giờ sợ quên viết Where(!b.IsDeleted).",
                        "modelBuilder.Entity<Book>()\n    .HasQueryFilter(b => !b.IsDeleted);"),
                    Step(3,
                        "3. Viết hàm Xóa mềm và hàm Xem thùng rác (IgnoreQueryFilters)",
                        "📥 Đầu vào: Yêu cầu xóa sách hoặc khôi phục sách từ thùng rác. ⚙️ Cơ chế ngầm: Khi xóa chỉ gán IsDeleted = true; khi admin mở trang Thùng rác thì dùng IgnoreQueryFilters() để tạm tắt bộ lọc tự động. 📤 Đầu ra: Quản lý xóa và khôi phục hoàn hảo.",
                        "// 1. Khi bấm nút Xóa (Soft Delete):\nbook.IsDeleted = true;\nbook.DeletedAt = DateTime.Now;\nawait _db.SaveChangesAsync();\n\n// 2. Khi Admin xem danh sách Thùng rác:\nvar trashBooks = await _db.Books\n    .IgnoreQueryFilters()\n    .Where(b => b.IsDeleted)\n    .ToListAsync();")
                },
                "Sau khi bạn đã cấu hình Bộ lọc toàn cục modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted); trong OnModelCreating, nếu ở trang Quản trị viên (Admin) bạn muốn viết một câu truy vấn lấy ra cả những sản phẩm đã bị xóa mềm (IsDeleted == true), bạn phải thêm phương thức nào vào chuỗi truy vấn LINQ?",
                new List<string>
                {
                    "A. Thêm phương thức .IgnoreQueryFilters() (ví dụ: _db.Products.IgnoreQueryFilters().Where(p => p.IsDeleted).ToListAsync())",
                    "B. Thêm phương thức .AsNoTracking()",
                    "C. Phải xóa dòng HasQueryFilter trong DbContext đi rồi biên dịch lại",
                    "D. Không thể lấy lại được các dòng đã bị HasQueryFilter ẩn đi"
                },
                "A",
                ".IgnoreQueryFilters() cho phép vô hiệu hóa tạm thời các bộ lọc HasQueryFilter đối với riêng câu truy vấn hiện tại, rất hữu ích khi làm tính năng Thùng rác (Trash) hoặc Kiểm toán dữ liệu cho Admin."
            ),

            // =====================================================================
            // BÀI 8.8
            // =====================================================================
            Build(
                topicId,
                "8.8",
                "Kết hợp SQL Thuần trong EF Core (FromSqlRaw, FromSqlInterpolated) & Xử lý Xung đột Đồng thời (Concurrency Control)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/sql-queries",
                """
                • **💡 1. Khi nào đang dùng EF Core mà vẫn cần gọi SQL thuần (`Raw SQL`)?**
                  - LINQ giải quyết được 95% các câu truy vấn. Nhưng với 5% các báo cáo thống kê siêu phức tạp (dùng `CTE đệ quy`, `Window Functions` nhiều tầng, hoặc gọi `Stored Procedure` có sẵn), EF Core cho phép bạn chạy thẳng câu lệnh SQL thuần và tự động hứng kết quả vào class C#!
                  - **Lưu ý bảo mật sống còn:** Luôn ưu tiên dùng **`FromSqlInterpolated($"...")`** (hoặc `FromSql($"...")` trong EF Core 7+) thay vì cộng chuỗi trong `FromSqlRaw`, vì `FromSqlInterpolated` tự động biến các biến `{bien}` thành tham số `@p0` chống `SQL Injection` 100%!
                • **🖼️ 2. Bài toán "2 Người Cùng Mua 1 Cuốn Sách Cuối Cùng" (`Optimistic Concurrency Control`):**
                ```
                Trong kho chỉ còn ĐÚNG 1 cuốn sách (Stock = 1). Cùng lúc 9h00:00, cả Việt và An đều bấm nút "Mượn sách":
                - Cả 2 cùng đọc lên thấy Stock = 1.
                - Việt trừ 1 -> Lưu xuống DB Stock = 0 (Thành công ở 9h00:01).
                - 1 mili-giây sau, lệnh của An cũng lấy số 1 ban đầu trừ 1 -> Ghi đè xuống DB Stock = 0!
                  ──► Kết quả: Kho chỉ có 1 cuốn mà hệ thống cho cả 2 người mượn thành công!

                [Giải pháp: Gắn thẻ [Timestamp] / [ConcurrencyCheck] (Optimistic Concurrency)]:
                Khi An bấm lưu ở giây sau, EF Core phát hiện dòng dữ liệu đó đã bị Việt sửa mất rồi (Version đã đổi!),
                lập tức ném ra lỗi DbUpdateConcurrencyException để chặn lệnh của An lại và báo:
                "Rất tiếc, cuốn sách vừa được người khác mượn cách đây 1 giây!"
                ```
                • **📊 3. Bảng Các Hàm Chạy SQL Thuần & Kiểm Soát Đồng Thời Trong EF Core:**
                | Tính năng | Cú pháp sử dụng trong EF Core | Công dụng |
                | :--- | :--- | :--- |
                | **Truy vấn trả về Entity** | `_db.Books.FromSqlInterpolated($"EXEC sp_Get {id}")` | Chạy câu lệnh `SELECT` hoặc `Stored Procedure` trả về các cột của bảng `Books`. |
                | **Truy vấn trả về DTO bất kỳ**| `_db.Database.SqlQuery<ReportDto>($"SELECT ...")` | ⭐ (EF Core 8/9/10): Chạy câu `SELECT` bất kỳ và đổ thẳng vào class `ReportDto` không cần khai báo `DbSet`! |
                | **Thực thi lệnh `UPDATE/DELETE` SQL**| `_db.Database.ExecuteSqlInterpolatedAsync($"...")`| Chạy các câu lệnh SQL không trả về bảng. |
                | **Chống ghi đè đồng thời** | Gắn **`[Timestamp]`** lên thuộc tính `public byte[] RowVersion { get; set; }` | SQL Server tự động đổi mã `RowVersion` mỗi khi dòng đó được `UPDATE`, giúp EF Core phát hiện xung đột tức thì! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Chạy câu lệnh SQL thống kê đổ thẳng ra DTO bằng SqlQuery<T>",
                        "📥 Đầu vào: Câu lệnh SQL thống kê phức tạp. ⚙️ Cơ chế ngầm: Sử dụng _db.Database.SqlQuery<T> (tính năng cực mạnh của EF Core mới), tự động tham số hóa biến year thành @p0 và ánh xạ kết quả vào class MonthlyReportDto. 📤 Đầu ra: List<MonthlyReportDto> an toàn và nhanh chóng.",
                        "int year = 2026;\nvar report = await _db.Database\n    .SqlQuery<MonthlyReportDto>($@\"\n        SELECT MONTH(BorrowDate) AS Month, COUNT(Id) AS TotalBorrows\n        FROM Borrows\n        WHERE YEAR(BorrowDate) = {year}\n        GROUP BY MONTH(BorrowDate)\")\n    .ToListAsync();"),
                    Step(2,
                        "2. Khai báo cột RowVersion ([Timestamp]) chống xung đột ghi đè",
                        "📥 Đầu vào: Bảng Books cần bảo vệ số lượng tồn kho Stock khi nhiều người mượn cùng lúc. ⚙️ Cơ chế ngầm: Thêm thuộc tính byte[] RowVersion có gắn thẻ [Timestamp]. 📤 Đầu ra: Mỗi lần UPDATE, EF Core tự động kiểm tra WHERE Id = @p0 AND RowVersion = @p1.",
                        "public class Book {\n    public int Id { get; set; }\n    public int Stock { get; set; }\n\n    [Timestamp]\n    public byte[] RowVersion { get; set; } = null!;\n}"),
                    Step(3,
                        "3. Bắt lỗi DbUpdateConcurrencyException khi có 2 người sửa cùng lúc",
                        "📥 Đầu vào: Hai người cùng cập nhật 1 cuốn sách tại cùng 1 thời điểm. ⚙️ Cơ chế ngầm: Người thứ hai lưu sau sẽ bị văng lỗi DbUpdateConcurrencyException vì RowVersion trong DB đã thay đổi. 📤 Đầu ra: Thông báo lịch sự cho người dùng tải lại dữ liệu mới.",
                        "try {\n    await _db.SaveChangesAsync();\n} catch (DbUpdateConcurrencyException) {\n    return Conflict(\"Dữ liệu vừa được người khác cập nhật, vui lòng thử lại!\");\n}")
                },
                "Trong Entity Framework Core, khi hai người dùng cùng mở trang chỉnh sửa của cùng một sản phẩm tại một thời điểm, để ngăn chặn việc người bấm Lưu sau vô tình ghi đè làm mất thay đổi của người bấm Lưu trước (Optimistic Concurrency), bạn nên sử dụng kỹ thuật nào?",
                new List<string>
                {
                    "A. Thêm một thuộc tính kiểu byte[] RowVersion có gắn thẻ [Timestamp] (hoặc dùng [ConcurrencyCheck]) vào Entity và bắt ngoại lệ DbUpdateConcurrencyException khi gọi SaveChangesAsync()",
                    "B. Chỉ cho phép 1 người truy cập trang web mỗi ngày",
                    "C. Dùng hàm AsNoTracking()",
                    "D. Xóa bảng đó đi tạo lại"
                },
                "A",
                "Khi có cột [Timestamp] (rowversion trong SQL Server), mọi câu lệnh UPDATE do EF Core sinh ra đều kèm điều kiện WHERE Id = @id AND RowVersion = @oldVersion. Nếu người khác đã sửa dòng đó trước, RowVersion dưới DB đã đổi nên UPDATE trả về 0 dòng và EF Core lập tức ném DbUpdateConcurrencyException."
            ),

            // =====================================================================
            // BÀI 8.9
            // =====================================================================
            Build(
                topicId,
                "8.9",
                "Kiến trúc Repository Pattern & Unit of Work hoàn chỉnh với Entity Framework Core (Ứng dụng cho cả Đồ án MVC & Thực tập)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design",
                """
                • **💡 1. Tại sao phải bọc `AppDbContext` vào trong `Repository` và `Unit of Work`?**
                  - Nếu bạn tiêm thẳng `AppDbContext _db` vào khắp các `Controller` và viết câu lệnh LINQ rải rác ở trong `Controller`:
                    1. Khi một câu truy vấn *"Lấy sách còn tồn kho và chưa xóa"* cần dùng ở 5 Controller khác nhau, bạn phải copy-paste đoạn LINQ đó 5 lần!
                    2. `Controller` bị phình to, trộn lẫn cả điều hướng HTTP lẫn logic Database, rất khó bảo trì và không thể viết Unit Test giả lập (Mock) được.
                  - **Mô hình `Generic Repository` + `Unit of Work`** giúp gom toàn bộ các thao tác Database về một mối chuyên nghiệp, sạch sẽ!
                • **🖼️ 2. Sơ đồ Kiến trúc 3 Tầng Chuẩn Mực Khi Kết Hợp `Controller / Service` ➔ `UnitOfWork` ➔ `Repositories`:**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ Tầng 1: Controller / Service (Xử lý luồng nghiệp vụ)                       │
                └─────────────────────────────────────┬──────────────────────────────────────┘
                                                      │ Gọi thông qua Interface
                                                      ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ Tầng 2: IUnitOfWork (Quản lý phiên giao dịch chung & hàm SaveChangesAsync) │
                │   ├── IBookRepository Books { get; }                                       │
                │   ├── IReaderRepository Readers { get; }                                   │
                │   └── Task<int> SaveChangesAsync(); (Chốt lưu tất cả chỉ trong 1 lần!)     │
                └─────────────────────────────────────┬──────────────────────────────────────┘
                                                      │ Điều phối xuống
                                                      ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ Tầng 3: GenericRepository<T> & AppDbContext (Giao tiếp với SQL Server)     │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Trách nhiệm rõ ràng của từng thành phần trong kiến trúc này:**
                  - **`IGenericRepository<T>`:** Chứa các hàm dùng chung cho mọi bảng (`GetByIdAsync`, `GetAllAsync`, `AddAsync`, `Update`, `Delete`).
                  - **`IBookRepository : IGenericRepository<Book>`:** Kế thừa toàn bộ hàm chung và chỉ khai báo thêm các hàm đặc thù riêng của bảng Sách (VD: `GetBooksWithCategoryAsync()`).
                  - **`IUnitOfWork`:** Giữ duy nhất 1 `AppDbContext` dùng chung cho các Repository và cung cấp hàm `SaveChangesAsync()` để lưu đồng bộ mọi thay đổi!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Xây dựng lớp cha dùng chung GenericRepository<T>",
                        "📥 Đầu vào: Kiểu thực thể T bất kỳ (class). ⚙️ Cơ chế ngầm: Dùng _context.Set<T>() để đại diện cho bảng tương ứng với kiểu T, viết sẵn các hàm CRUD cơ bản một lần duy nhất cho toàn dự án. 📤 Đầu ra: Tái sử dụng cho 100 bảng không cần viết lại.",
                        "public class GenericRepository<T> : IGenericRepository<T> where T : class {\n    protected readonly AppDbContext _context;\n    protected readonly DbSet<T> _dbSet;\n\n    public GenericRepository(AppDbContext context) {\n        _context = context;\n        _dbSet = context.Set<T>();\n    }\n    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);\n    public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);\n}"),
                    Step(2,
                        "2. Tạo BookRepository kế thừa GenericRepository<Book>",
                        "📥 Đầu vào: Bảng Books cần thêm hàm lấy sách kèm tên Thể loại. ⚙️ Cơ chế ngầm: Kế thừa GenericRepository<Book> là có sẵn ngay GetById, Add, Update, Delete; chỉ cần viết thêm hàm riêng GetBooksWithCategoryAsync(). 📤 Đầu ra: Code cực kỳ ngắn gọn.",
                        "public class BookRepository : GenericRepository<Book>, IBookRepository {\n    public BookRepository(AppDbContext context) : base(context) { }\n\n    public async Task<List<Book>> GetBooksWithCategoryAsync() {\n        return await _dbSet.AsNoTracking().Include(b => b.Category).ToListAsync();\n    }\n}"),
                    Step(3,
                        "3. Gói gọn trong UnitOfWork và gọi tại Service/Controller",
                        "📥 Đầu vào: Thực hiện nghiệp vụ mượn sách (vừa thêm Borrow, vừa sửa Book). ⚙️ Cơ chế ngầm: Cả hai Repository cùng dùng chung 1 _context bên trong UnitOfWork và chốt lưu bằng _unitOfWork.SaveChangesAsync(). 📤 Đầu ra: Đảm bảo tính toàn vẹn giao dịch 100%.",
                        "public async Task BorrowBookAsync(Borrow borrow, Book book) {\n    await _unitOfWork.Borrows.AddAsync(borrow);\n    book.Stock -= 1;\n    // Chốt lưu cả 2 bảng trong 1 lần gọi duy nhất:\n    await _unitOfWork.SaveChangesAsync();\n}")
                },
                "Trong lớp GenericRepository<T> của Entity Framework Core, câu lệnh nào của DbContext cho phép bạn lấy ra đối tượng DbSet<T> tương ứng với kiểu T bất kỳ được truyền vào?",
                new List<string>
                {
                    "A. Sử dụng phương thức _context.Set<T>()",
                    "B. Sử dụng phương thức _context.GetTable(T)",
                    "C. Sử dụng phương thức _context.SaveChanges()",
                    "D. Sử dụng phương thức _context.Database"
                },
                "A",
                "Phương thức generic _context.Set<T>() trả về một đối tượng DbSet<T> cho phép thực hiện các truy vấn và thao tác CRUD trên bảng tương ứng với thực thể T, là hạt nhân để xây dựng GenericRepository<T>."
            ),

            // =====================================================================
            // BÀI 8.10
            // =====================================================================
            Build(
                topicId,
                "8.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 8 (Checklist 8 Quy tắc Tối ưu EF Core Chuẩn Senior)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/performance/efficient-querying",
                """
                • **💡 1. Bản Checklist 8 Quy Tắc Vàng Khi Làm Việc Với Entity Framework Core (Dùng cho cả Đồ án & Phỏng vấn):**
                ```
                ✅ 1. Luôn dùng .AsNoTracking() cho mọi câu truy vấn chỉ đọc (HTTP GET danh sách, xem chi tiết, báo cáo).
                ✅ 2. Luôn dùng Async/Await (.ToListAsync, .FirstOrDefaultAsync, .SaveChangesAsync) kèm CancellationToken.
                ✅ 3. Không bao giờ kéo cả bảng về RAM rồi mới lọc: Phải đặt .Where(), .Skip(), .Take() TRƯỚC .ToListAsync()!
                ✅ 4. Dùng Projection (.Select(x => new Dto { ... })) để chỉ nhặt đúng những cột cần thiết thay vì lấy cả 30 cột của Entity.
                ✅ 5. Kiểm tra tồn tại hoặc kiểm tra trùng lặp luôn dùng .AnyAsync() thay vì .CountAsync() > 0.
                ✅ 6. Phòng chống lỗi N+1 Queries bằng .Include() / .ThenInclude() (hoặc .Select()), không lạm dụng Lazy Loading.
                ✅ 7. Khi cần cập nhật hoặc xóa hàng loạt nhiều dòng theo điều kiện, dùng .ExecuteUpdateAsync() / .ExecuteDeleteAsync().
                ✅ 8. Sử dụng Soft Delete (IsDeleted) kết hợp Global Query Filter (HasQueryFilter) và DeleteBehavior.Restrict để bảo vệ dữ liệu.
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo truy vấn chỉ đọc với AsNoTracking",
                        "📥 Đầu vào: API lấy danh sách sách có tìm kiếm và phân trang. ⚙️ Cơ chế ngầm: Tắt bộ theo dõi Change Tracker ngay từ đầu bằng AsNoTracking(). 📤 Đầu ra: Giảm 40% tiêu thụ RAM.",
                        "IQueryable<Book> query = _db.Books.AsNoTracking();"),
                    Step(2,
                        "2. Ghép điều kiện lọc động trên IQueryable (Chưa gọi DB)",
                        "📥 Đầu vào: Từ khóa keyword và mã thể loại categoryId. ⚙️ Cơ chế ngầm: Nối thêm các mệnh đề .Where() vào cây biểu thức IQueryable. 📤 Đầu ra: Câu truy vấn SQL được chuẩn bị tối ưu.",
                        "if (!string.IsNullOrWhiteSpace(keyword))\n    query = query.Where(b => b.Title.Contains(keyword.Trim()));\n\nif (categoryId.HasValue)\n    query = query.Where(b => b.CategoryId == categoryId.Value);"),
                    Step(3,
                        "3. Đếm tổng số dòng và Chiếu sang DTO bằng Select",
                        "📥 Đầu vào: Biến query đã gắn đủ điều kiện lọc. ⚙️ Cơ chế ngầm: Gọi CountAsync() để lấy tổng số dòng, sau đó dùng .Select() chỉ nhặt đúng 4 cột cần hiển thị ra màn hình (tự động JOIN lấy Category.Name). 📤 Đầu ra: Không lấy thừa bất kỳ cột nào.",
                        "int total = await query.CountAsync(ct);\nvar items = await query\n    .OrderByDescending(b => b.Id)\n    .Skip((page - 1) * pageSize)\n    .Take(pageSize)\n    .Select(b => new BookListItemDto(b.Id, b.BookCode, b.Title, b.Category.Name, b.Price))"),
                    Step(4,
                        "4. Chốt thực thi bất đồng bộ bằng ToListAsync(ct)",
                        "📥 Đầu vào: Toàn bộ chuỗi truy vấn từ Bước 1 đến Bước 3. ⚙️ Cơ chế ngầm: Bắn câu lệnh SQL tối ưu nhất xuống SQL Server kèm CancellationToken và đóng gói kết quả trả về. 📤 Đầu ra: Phản hồi API dưới 15ms!",
                        "    .ToListAsync(ct);\nreturn new PagedResult<BookListItemDto>(items, total, page, pageSize);")
                },
                "(Câu hỏi tổng hợp Chủ đề 8) Trong các đoạn code truy vấn Entity Framework Core dưới đây, đoạn code nào tuân thủ ĐẦY ĐỦ NHẤT các nguyên tắc tối ưu hiệu năng khi lấy danh sách 10 sản phẩm hiển thị lên trang chủ?",
                new List<string>
                {
                    "A. var list = _db.Products.ToList().Where(p => p.IsActive).Take(10).ToList();",
                    "B. var list = await _db.Products.AsNoTracking().Where(p => p.IsActive).OrderByDescending(p => p.Id).Take(10).Select(p => new ProductDto(p.Id, p.Name, p.Price)).ToListAsync(ct);",
                    "C. var list = _db.Products.Include(p => p.Category).ToListAsync().Result;",
                    "D. foreach (var id in ids) { list.Add(await _db.Products.FindAsync(id)); }"
                },
                "B",
                "Phương án B hội tụ đủ mọi kỹ thuật chuẩn Senior: (1) Dùng .AsNoTracking(), (2) Lọc .Where() và .Take(10) trên IQueryable trước khi gọi DB, (3) Dùng .Select() chỉ lấy đúng 3 cột cần thiết sang DTO, và (4) Dùng await ... ToListAsync(ct) bất đồng bộ hoàn toàn."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (8.1 -> 8.10) cho Chủ đề 8 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic8 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 8);
        if (topic8 == null)
            return;

        topic8.Title = "Chủ đề 8: Entity Framework Core và ORM";
        topic8.Description = "10 Bài học con siêu chi tiết từ Bản chất ORM (DbContext/DbSet), Data Annotations & Fluent API (IEntityTypeConfiguration), Quan hệ 1-1/1-N/N-N & DeleteBehavior, Migrations, Eager/Explicit/Lazy Loading & N+1, Change Tracker & ExecuteUpdate/Delete, Soft Delete & HasQueryFilter, Raw SQL & Concurrency đến Generic Repository & Unit of Work.";

        var targetLessons = GetLessons(topic8.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic8.Id)
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
