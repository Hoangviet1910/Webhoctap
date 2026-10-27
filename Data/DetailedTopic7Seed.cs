using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic7Seed
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
            // BÀI 7.1
            // =====================================================================
            Build(
                topicId,
                "7.1",
                "Bản chất Hệ quản trị CSDL Quan hệ (RDBMS), Thiết kế ERD, Khóa chính (PK) và Khóa ngoại (FK)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/relational-databases/tables/primary-and-foreign-key-constraints",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Nếu bạn lưu toàn bộ dữ liệu vào 1 bảng Excel khổng lồ: Mỗi khi độc giả `"Nguyễn Hoàng Việt"` mượn 10 cuốn sách, bạn phải gõ lặp lại Họ tên, Số điện thoại, Địa chỉ của Việt 10 lần! Nếu Việt đổi số điện thoại, bạn phải đi sửa cả 10 dòng (nếu quên sửa 1 dòng sẽ gây sai lệch dữ liệu — *Data Anomaly*).
                  - **RDBMS (SQL Server)** giải quyết triệt để bằng cách **tách dữ liệu thành các Bảng (Tables) độc lập** và nối chúng lại với nhau bằng **Sợi dây liên kết Khóa chính (`Primary Key`) — Khóa ngoại (`Foreign Key`)**!
                • **🖼️ 2. Sơ đồ Liên kết Khóa Chính (PK) - Khóa Ngoại (FK) & 3 Mối Quan Hệ ERD Cốt Lõi:**
                ```
                [Bảng Categories (Danh mục)]                [Bảng Books (Sách)]
                ┌──────────────────────────┐                ┌────────────────────────────────────────┐
                │ 🔑 Id (PK - Khóa chính)  │ 1 ────────── ∞ │ 🔑 Id (PK - Khóa chính của Sách)       │
                │    Name (NVARCHAR)       │ (Một - Nhiều)  │    Title (NVARCHAR)                    │
                └──────────────────────────┘                │ 🔗 CategoryId (FK - Trỏ sang Id ở bên!)│
                                                            └────────────────────────────────────────┘

                * 3 Mối quan hệ thực thể (ERD) bắt buộc nắm vững khi thiết kế Database:
                  1. Quan hệ 1 - Nhiều (1 - N): Phổ biến nhất (1 Khoa có Nhiều Sinh viên; 1 Thể loại có Nhiều Sách).
                     -> Quy tắc đặt khóa: Luôn đặt cột Khóa ngoại (FK) nằm ở bảng bên "NHIỀU"!
                  2. Quan hệ Nhiều - Nhiều (N - N): (1 Sinh viên học Nhiều Môn; 1 Môn có Nhiều Sinh viên).
                     -> Quy tắc bắt buộc: Phải tách ra một "BẢNG TRUNG GIAN" ở giữa chứa 2 Khóa ngoại!
                  3. Quan hệ 1 - 1 (1 - 1): (1 Tài khoản User có 1 Hồ sơ chi tiết UserProfile).
                ```
                • **📊 3. Bảng Phân Biệt Các Ràng Buộc Toàn Vẹn Dữ Liệu (`Constraints`) Trong SQL Server:**
                | Tên ràng buộc | Ký hiệu SQL | Tác dụng bảo vệ dữ liệu |
                | :--- | :--- | :--- |
                | **Khóa chính (`Primary Key`)**| `PRIMARY KEY IDENTITY(1,1)`| Định danh duy nhất cho mỗi dòng. **Cấm trùng lặp** và **Cấm NULL**. `IDENTITY(1,1)` giúp tự tăng `1, 2, 3...`. |
                | **Khóa ngoại (`Foreign Key`)**| `FOREIGN KEY REFERENCES` | Đảm bảo giá trị nhập vào bắt buộc phải tồn tại ở bảng cha (Chống dữ liệu mồ côi!). |
                | **Duy nhất (`Unique`)** | `UNIQUE` | Cấm trùng lặp dữ liệu giữa các dòng (dùng cho cột `Email`, `CCCD`, `MaSinhVien`). |
                | **Kiểm tra điều kiện (`Check`)**| `CHECK (Price >= 0)` | Chặn không cho phép lưu dữ liệu vô lý (như giá âm, điểm `< 0` hoặc `> 10`). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo bảng Cha (Categories) với Khóa chính tự tăng",
                        "📥 Đầu vào: Thiết kế bảng Thể loại sách. ⚙️ Cơ chế ngầm: IDENTITY(1,1) tự động đánh số từ 1 và tăng thêm 1 cho mỗi dòng mới. 📤 Đầu ra: Bảng Categories chuẩn mực.",
                        "CREATE TABLE Categories (\n    Id INT PRIMARY KEY IDENTITY(1,1),\n    Name NVARCHAR(100) NOT NULL UNIQUE\n);"),
                    Step(2,
                        "2. Tạo bảng Con (Books) có Khóa ngoại nối sang bảng Cha",
                        "📥 Đầu vào: Thiết kế bảng Sách thuộc về Thể loại. ⚙️ Cơ chế ngầm: Cột CategoryId đóng vai trò Foreign Key tham chiếu tới Categories(Id). Nếu cố tình điền CategoryId = 999 (chưa có bên bảng Categories), SQL Server sẽ chặn lại ngay! 📤 Đầu ra: Liên kết 1-Nhiều an toàn.",
                        "CREATE TABLE Books (\n    Id INT PRIMARY KEY IDENTITY(1,1),\n    Title NVARCHAR(200) NOT NULL,\n    Price DECIMAL(18,2) NOT NULL CHECK (Price >= 0),\n    CategoryId INT NOT NULL,\n    CONSTRAINT FK_Books_Categories FOREIGN KEY (CategoryId) REFERENCES Categories(Id)\n);"),
                    Step(3,
                        "3. Tách bảng trung gian cho quan hệ Nhiều - Nhiều (N - N)",
                        "📥 Đầu vào: 1 Phiếu mượn có nhiều Sách, 1 Sách nằm trong nhiều Phiếu mượn. ⚙️ Cơ chế ngầm: Tạo bảng trung gian BorrowDetails kết hợp cả BorrowId và BookId làm Khóa chính kép (Composite Key). 📤 Đầu ra: Giải quyết triệt để quan hệ N-N.",
                        "CREATE TABLE BorrowDetails (\n    BorrowId INT NOT NULL REFERENCES Borrows(Id),\n    BookId INT NOT NULL REFERENCES Books(Id),\n    Quantity INT DEFAULT 1,\n    PRIMARY KEY (BorrowId, BookId) -- Khóa chính kép\n);")
                },
                "Khi thiết kế cơ sở dữ liệu cho bài toán có mối quan hệ Một - Nhiều (1 - N) giữa bảng Departments (1 Khoa) và bảng Students (Nhiều Sinh viên), cột Khóa ngoại (Foreign Key) bắt buộc phải được đặt ở bảng nào?",
                new List<string>
                {
                    "A. Đặt cột DepartmentId (Khóa ngoại) ở bảng bên NHIỀU (bảng Students)",
                    "B. Đặt cột StudentId ở bảng bên MỘT (bảng Departments)",
                    "C. Phải tạo thêm 3 bảng phụ nữa mới nối được",
                    "D. Không cần dùng khóa ngoại"
                },
                "A",
                "Quy tắc bất di bất dịch của thiết kế CSDL quan hệ (ERD): trong mối quan hệ 1 - Nhiều, khóa chính của bảng \"Một\" (Departments.Id) luôn được đem sang làm khóa ngoại (DepartmentId) nằm ở bảng \"Nhiều\" (Students)."
            ),

            // =====================================================================
            // BÀI 7.2
            // =====================================================================
            Build(
                topicId,
                "7.2",
                "Chuẩn hóa Cơ sở dữ liệu (1NF, 2NF, 3NF) & Cách chọn Kiểu dữ liệu SQL Server khớp 100% với C#",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/framework/data/adonet/sql-server-data-type-mappings",
                """
                • **💡 1. Hiểu bản chất của 3 Dạng Chuẩn (`1NF`, `2NF`, `3NF`) trong 30 giây (Câu hỏi thi & bảo vệ môn CSDL):**
                  - **Chuẩn 1 (`1NF` - Nguyên tử):** Mỗi ô trong bảng **chỉ được chứa ĐÚNG 1 giá trị đơn lẻ** (Cấm nhét `"0912345, 0988888"` gồm 2 số điện thoại chung vào 1 ô!).
                  - **Chuẩn 2 (`2NF` - Phụ thuộc đầy đủ vào Khóa chính):** Đã đạt `1NF`, và mọi cột thông tin trong bảng phải phụ thuộc vào **toàn bộ Khóa chính** (Ví dụ trong bảng `ChiTietHoaDon(MaHD, MaSP, SoLuong, TenSP)`, cột `TenSP` chỉ phụ thuộc vào `MaSP` chứ không phụ thuộc vào `MaHD` $\rightarrow$ Phải vứt `TenSP` về bảng `SanPham`!).
                  - **Chuẩn 3 (`3NF` - Không phụ thuộc bắc cầu):** Đã đạt `2NF`, và các cột thường **không được phụ thuộc lẫn nhau** (Ví dụ trong bảng `SinhVien` có cột `MaLop` và `TenKhoa`: vì `SinhVien` $\rightarrow$ `MaLop` $\rightarrow$ `TenKhoa` là bắc cầu, nên phải tách `TenKhoa` sang bảng `Lop`!).
                • **🖼️ 2. Bảng Đối Chiếu Kiểu Dữ Liệu Giữa `SQL Server` và `C# (.NET)`:**
                ```
                SQL Server Type                  C# (.NET) Type        Lưu ý sống còn khi thiết kế bảng
                ──────────────────────────────────────────────────────────────────────────────────────────
                INT                          ──► int                   Số nguyên chuẩn (-2 tỷ đến +2 tỷ)
                BIGINT                       ──► long                  Số nguyên cực lớn
                BIT                          ──► bool                  Chỉ lưu 0 (false) hoặc 1 (true)
                DECIMAL(18, 2)               ──► decimal               BẮT BUỘC dùng cho Tiền tệ (18 số, 2 số lẻ)
                VARCHAR(n)                   ──► string                Chuỗi KHÔNG DẤU (Mã code, Email, Số ĐT)
                NVARCHAR(n)                  ──► string                Chuỗi CÓ DẤU TIẾNG VIỆT (Họ tên, Địa chỉ)
                DATETIME2 / DATETIME         ──► DateTime              Ngày và giờ
                UNIQUEIDENTIFIER             ──► Guid                  Mã định danh duy nhất toàn cầu (NEWID())
                ```
                • **⚠️ 3. Bẫy "Lỗi Font Chữ Tiếng Việt (`???`)" Kinh Điển Trong SQL Server:**
                  - 100% sinh viên đều từng gặp lỗi: Lưu tên `"Nguyễn Hoàng Việt"` vào SQL Server xong mở ra biến thành `"Nguy?n Ho?ng Vi?t"`.
                  - **2 nguyên nhân & cách diệt tận gốc:**
                    1. Cột đó khai báo là `VARCHAR` thay vì **`NVARCHAR`** (Chữ **`N`** viết tắt của *National / Unicode* hỗ trợ tiếng Việt).
                    2. Khi viết câu lệnh `INSERT` hoặc `WHERE` bằng tay trong SSMS, quên đặt chữ **`N` viết hoa** ở trước dấu nháy đơn: Phải viết **`N'Nguyễn Hoàng Việt'`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Chọn đúng VARCHAR (Không dấu) và NVARCHAR (Tiếng Việt)",
                        "📥 Đầu vào: Tạo bảng Độc giả (Readers). ⚙️ Cơ chế ngầm: Mã thẻ và Email không bao giờ có dấu tiếng Việt -> Dùng VARCHAR cho nhẹ bộ nhớ (1 byte/ký tự). Họ tên và Địa chỉ có dấu -> Bắt buộc dùng NVARCHAR (2 bytes/ký tự). 📤 Đầu ra: Bảng tối ưu dung lượng và không lỗi font.",
                        "CREATE TABLE Readers (\n    ReaderCode VARCHAR(20) PRIMARY KEY,\n    FullName NVARCHAR(100) NOT NULL,\n    Email VARCHAR(100) NOT NULL,\n    IsActive BIT DEFAULT 1\n);"),
                    Step(2,
                        "2. Thêm dữ liệu Tiếng Việt với tiền tố N'...'",
                        "📥 Đầu vào: Câu lệnh INSERT bằng SQL thuần. ⚙️ Cơ chế ngầm: Chữ N đứng trước chuỗi báo cho SQL Server biết đây là chuỗi Unicode cần giữ nguyên dấu tiếng Việt. 📤 Đầu ra: Lưu chính xác 100% tiếng Việt.",
                        "INSERT INTO Readers (ReaderCode, FullName, Email)\nVALUES ('DG001', N'Nguyễn Hoàng Việt', 'viet@uneti.edu.vn');"),
                    Step(3,
                        "3. Chuẩn hóa bảng đạt 3NF (Loại bỏ cột tính toán thừa)",
                        "📥 Đầu vào: Bảng Chi tiết đơn hàng có Số lượng và Đơn giá. ⚙️ Cơ chế ngầm: Không lưu cứng cột ThanhTien, hoặc dùng Computed Column tự động nhân (Quantity * UnitPrice) để tránh lệch số liệu khi sửa giá. 📤 Đầu ra: Đạt chuẩn 3NF.",
                        "CREATE TABLE OrderItems (\n    Id INT PRIMARY KEY IDENTITY(1,1),\n    Quantity INT NOT NULL,\n    UnitPrice DECIMAL(18,2) NOT NULL,\n    LineTotal AS (Quantity * UnitPrice) -- Cột tự động tính toán\n);")
                },
                "Trong SQL Server, khi bạn khai báo cột FullName có kiểu là NVARCHAR(100), nhưng khi chạy câu lệnh INSERT INTO Students(FullName) VALUES ('Nguyễn Hoàng Việt') trên SSMS thì dữ liệu lưu vào bảng vẫn bị lỗi thành 'Nguy?n Hoàng Vi?t'. Nguyên nhân là vì sao?",
                new List<string>
                {
                    "A. Vì thiếu chữ N viết hoa đứng trước chuỗi nháy đơn: phải viết là N'Nguyễn Hoàng Việt' để SQL Server hiểu chuỗi hằng đó là Unicode",
                    "B. Vì độ dài 100 là quá ngắn",
                    "C. Vì SQL Server không hỗ trợ tiếng Việt",
                    "D. Vì phải dùng dấu nháy kép thay cho dấu nháy đơn"
                },
                "A",
                "Dù cột đã là NVARCHAR, nhưng trong câu lệnh T-SQL thuần, một chuỗi đặt trong '...' mặc định được hiểu là mã ASCII (VARCHAR). Bạn bắt buộc phải thêm tiền tố N (N'...') trước các chuỗi có dấu tiếng Việt."
            ),

            // =====================================================================
            // BÀI 7.3
            // =====================================================================
            Build(
                topicId,
                "7.3",
                "Ngôn ngữ Thao tác Dữ liệu (DML: INSERT, UPDATE, DELETE) & Thứ tự chạy thực tế của câu lệnh SELECT",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/t-sql/queries/select-transact-sql",
                """
                • **💡 1. Bẫy chết người với `UPDATE` và `DELETE` khi đi làm thực tế:**
                  - Nếu bạn gõ `DELETE FROM Books` hoặc `UPDATE Books SET Price = 0` mà **QUÊN viết mệnh đề `WHERE Id = ...`** ở cuối, SQL Server sẽ **xóa sạch hoặc sửa sạch toàn bộ 100.000 dòng trong bảng ngay lập tức**!
                  - *Mẹo sống còn của Senior:* Trước khi gõ `DELETE FROM Books WHERE ...`, hãy gõ `SELECT * FROM Books WHERE ...` trước để xem có đúng những dòng mình định xóa hiện ra không, rồi mới đổi chữ `SELECT *` thành `DELETE`!
                • **🖼️ 2. Bí mật ít người biết: Thứ tự bạn VIẾT câu lệnh `SELECT` hoàn toàn KHÁC với thứ tự SQL Server CHẠY ngầm!**
                ```
                [Thứ tự bạn gõ chữ trên bàn phím]:
                SELECT TOP(10) ... ➔ FROM ... ➔ JOIN ... ➔ WHERE ... ➔ GROUP BY ... ➔ HAVING ... ➔ ORDER BY ...

                [Thứ tự SQL Server thực sự chạy ở bên dưới (Lý do vì sao WHERE không dùng được tên Alias của SELECT!)]:
                Bước 1: FROM & JOIN   (Tìm đến các bảng và nối chúng lại với nhau trước tiên)
                  │
                Bước 2: WHERE         (Lọc bỏ các dòng không thỏa mãn điều kiện thô)
                  │
                Bước 3: GROUP BY      (Gom các dòng còn lại vào từng nhóm)
                  │
                Bước 4: HAVING        (Lọc các nhóm sau khi đã gom)
                  │
                Bước 5: SELECT        (Lúc này mới nhặt các cột và đặt tên giả Alias AS ...)
                  │
                Bước 6: DISTINCT & ORDER BY & TOP / OFFSET-FETCH (Khử trùng, sắp xếp và cắt trang cuối cùng!)
                ```
                • **📊 3. Bảng Các Toán Tử Lọc Dữ Liệu Thực Chiến Trong Mệnh Đề `WHERE`:**
                | Toán tử SQL | Cách sử dụng | Lưu ý thực tế |
                | :--- | :--- | :--- |
                | **`BETWEEN a AND b`** | `WHERE Price BETWEEN 100 AND 500` | Tương đương `Price >= 100 AND Price <= 500` (Lấy cả 2 đầu mút). |
                | **`IN (ds_giá_trị)`** | `WHERE Status IN (1, 2, 5)` | Gọn hơn nhiều so với viết `Status = 1 OR Status = 2 OR Status = 5`. |
                | **`LIKE N'%từ_khóa%'`**| `WHERE Title LIKE N'%Lập trình%'` | Dấu `%` đại diện cho chuỗi ký tự bất kỳ (tương đương `.Contains()` trong C#). |
                | **`IS NULL` / `IS NOT NULL`**| `WHERE DeletedAt IS NULL` | ⚠️ **CẤM viết `= NULL`** (Trong SQL, viết `WHERE x = NULL` luôn không trả về dòng nào, phải viết **`IS NULL`**!). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thêm một hoặc nhiều dòng dữ liệu cùng lúc (INSERT)",
                        "📥 Đầu vào: Thông tin 2 cuốn sách mới. ⚙️ Cơ chế ngầm: Không truyền cột Id (vì đã tự tăng IDENTITY), truyền các cột còn lại. 📤 Đầu ra: 2 bản ghi mới được ghi vào bảng.",
                        "INSERT INTO Books (Title, Price, CategoryId)\nVALUES \n    (N'Cấu trúc dữ liệu và giải thuật', 120000, 1),\n    (N'Phân tích thiết kế hệ thống', 150000, 1);"),
                    Step(2,
                        "2. Cập nhật và Xóa an toàn luôn kèm khóa chính (WHERE Id = ...)",
                        "📥 Đầu vào: Mã sách Id = 5 cần giảm giá. ⚙️ Cơ chế ngầm: Tìm đúng dòng có Id = 5 thông qua Clustered Index và cập nhật cột Price. 📤 Đầu ra: Chỉ đúng 1 dòng bị tác động.",
                        "UPDATE Books\nSET Price = Price * 0.9\nWHERE Id = 5; -- BẮT BUỘC phải có WHERE!"),
                    Step(3,
                        "3. Truy vấn lọc đa điều kiện kết hợp phân trang (OFFSET - FETCH)",
                        "📥 Đầu vào: Tìm sách có chữ 'C#', chưa xóa, lấy trang 2 (bỏ qua 10 dòng đầu, lấy 10 dòng tiếp). ⚙️ Cơ chế ngầm: SQL chạy FROM -> WHERE -> ORDER BY -> OFFSET FETCH -> SELECT. 📤 Đầu ra: 10 cuốn sách của trang 2.",
                        "SELECT Id, Title, Price\nFROM Books\nWHERE Title LIKE N'%C#%' AND DeletedAt IS NULL\nORDER BY Price DESC\nOFFSET 10 ROWS FETCH NEXT 10 ROWS ONLY;")
                },
                "Trong SQL Server, tại sao câu lệnh SELECT Title, (Price * 0.9) AS DiscountPrice FROM Books WHERE DiscountPrice > 100000 lại bị báo lỗi đỏ không chạy được (Invalid column name 'DiscountPrice')?",
                new List<string>
                {
                    "A. Vì theo thứ tự thực thi của SQL Server, mệnh đề WHERE chạy ở Bước 2 (TRƯỚC khi mệnh đề SELECT chạy ở Bước 5), nên tại thời điểm chạy WHERE thì tên cột giả (Alias) 'DiscountPrice' vẫn chưa hề tồn tại!",
                    "B. Vì từ khóa AS không được dùng cho phép nhân",
                    "C. Vì SQL Server không cho phép nhân với số thập phân",
                    "D. Vì tên cột quá dài"
                },
                "A",
                "Hiểu thứ tự thực thi (FROM -> WHERE -> GROUP BY -> HAVING -> SELECT -> ORDER BY) giúp bạn giải thích ngay vì sao WHERE không nhận diện được Alias của SELECT, nhưng ORDER BY (chạy sau SELECT) thì lại dùng được ORDER BY DiscountPrice bình thường!"
            ),

            // =====================================================================
            // BÀI 7.4
            // =====================================================================
            Build(
                topicId,
                "7.4",
                "Làm chủ toàn bộ các phép nối bảng (INNER JOIN, LEFT JOIN, RIGHT JOIN, FULL JOIN & SELF JOIN)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/relational-databases/performance/joins",
                """
                • **💡 1. Hiểu bản chất của `JOIN` trong 30 giây:**
                  - Dữ liệu đã được tách ra nhiều bảng (`Books`, `Categories`, `Authors`). Khi hiển thị lên giao diện web, người dùng không muốn nhìn thấy con số vô hồn `CategoryId = 3`, mà họ muốn nhìn thấy chữ `"Công nghệ thông tin"`.
                  - Phép **`JOIN`** chính là chiếc cầu ghép các bảng lại với nhau dựa trên điều kiện khớp khóa: **`ON Bảng_Con.Khóa_Ngoại = Bảng_Cha.Khóa_Chính`**.
                • **🖼️ 2. Sơ đồ Trực quan Phân Biệt `INNER JOIN` vs `LEFT JOIN` (Câu hỏi phỏng vấn 100% gặp):**
                ```
                Giả sử bảng [Categories] có 3 thể loại: (1: CNTT), (2: Kinh tế), (3: Ngoại ngữ - Chưa có cuốn sách nào!)

                [1. INNER JOIN (Phần giao nhau ở giữa - Chỉ lấy những dòng KHỚP Ở CẢ 2 BẢNG)]:
                SELECT c.Name, b.Title FROM Categories c INNER JOIN Books b ON c.Id = b.CategoryId
                ──► Thể loại (3: Ngoại ngữ) vì chưa có sách nào nên BỊ LOẠI BỎ BIẾN MẤT khỏi kết quả!

                [2. LEFT JOIN (Lấy TẤT CẢ bảng bên Trái + Phần khớp ở bảng bên Phải)]:
                SELECT c.Name, b.Title FROM Categories c LEFT JOIN Books b ON c.Id = b.CategoryId
                ──► Thể loại (3: Ngoại ngữ) VẪN HIỆN RA ĐẦY ĐỦ, và cột b.Title của nó mang giá trị NULL!
                    (Rất quan trọng khi làm báo cáo: "Thống kê số lượng sách của TẤT CẢ các thể loại, kể cả thể loại có 0 cuốn!")
                ```
                • **📊 3. Bảng Tóm Tắt 4 Loại `JOIN` Quan Trọng Nhất:**
                | Loại JOIN | Quy tắc giữ dòng | Khi nào sử dụng trong thực tế? |
                | :--- | :--- | :--- |
                | **`INNER JOIN`** (Hoặc viết tắt là `JOIN`) | Chỉ giữ lại những dòng có mặt ở **cả 2 bảng**. | Khi mối quan hệ là bắt buộc (`NOT NULL`), ví dụ lấy Sách kèm Tên thể loại của nó. |
                | **`LEFT JOIN`** (`LEFT OUTER JOIN`) | Giữ lại **100% các dòng của bảng đứng trước (Bên Trái)**; nếu bảng phải không có dữ liệu khớp thì điền `NULL`. | Khi khóa ngoại cho phép rỗng (`Nullable`), hoặc khi thống kê danh mục cha kể cả khi chưa có con. |
                | **`RIGHT JOIN`** | Ngược lại với `LEFT JOIN` (Giữ 100% bảng bên Phải). | Ít dùng (vì người ta thường đảo thứ tự bảng để dùng `LEFT JOIN` cho thuận mắt). |
                | **`SELF JOIN`** (Tự nối chính mình) | Nối 1 bảng với chính nó (bắt buộc đặt 2 tên bí danh khác nhau `e` và `m`). | Bảng Nhân viên có cột `ManagerId` trỏ về chính cột `Id` của bảng Nhân viên đó! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Nối 3 bảng liên tiếp bằng INNER JOIN",
                        "📥 Đầu vào: Bảng Phiếu mượn (Borrows), Độc giả (Readers) và Nhân viên (Staffs). ⚙️ Cơ chế ngầm: Ghép nối lần lượt qua các khóa ngoại ReaderId và StaffId để lấy đầy đủ họ tên người mượn và người cho mượn. 📤 Đầu ra: Bảng thông tin đầy đủ.",
                        "SELECT \n    br.Id AS BorrowCode,\n    r.FullName AS ReaderName,\n    s.FullName AS StaffName,\n    br.BorrowDate\nFROM Borrows br\nINNER JOIN Readers r ON br.ReaderId = r.Id\nINNER JOIN Staffs s ON br.StaffId = s.Id;"),
                    Step(2,
                        "2. Dùng LEFT JOIN để không bị mất các dòng chưa có dữ liệu con",
                        "📥 Đầu vào: Thống kê tất cả Thể loại và số sách bên trong. ⚙️ Cơ chế ngầm: Dùng LEFT JOIN từ Categories sang Books kết hợp COUNT(b.Id) để thể loại chưa có sách vẫn hiện ra với số lượng = 0! 📤 Đầu ra: Báo cáo chính xác 100%.",
                        "SELECT \n    c.Name AS CategoryName,\n    COUNT(b.Id) AS TotalBooks -- Chú ý: Đếm theo b.Id để dòng NULL tính là 0!\nFROM Categories c\nLEFT JOIN Books b ON c.Id = b.CategoryId\nGROUP BY c.Id, c.Name;"),
                    Step(3,
                        "3. Tìm các bản ghi \"Chưa từng phát sinh giao dịch\" bằng LEFT JOIN + IS NULL",
                        "📥 Đầu vào: Tìm những cuốn sách trong kho CHƯA TỪNG có ai mượn. ⚙️ Cơ chế ngầm: LEFT JOIN bảng Books với BorrowDetails, những cuốn sách chưa ai mượn sẽ có cột bd.BookId bị NULL. 📤 Đầu ra: Danh sách sách tồn đọng.",
                        "SELECT b.Id, b.Title\nFROM Books b\nLEFT JOIN BorrowDetails bd ON b.Id = bd.BookId\nWHERE bd.BookId IS NULL; -- Tuyệt chiêu tìm sách chưa từng được mượn!")
                },
                "Trong báo cáo thống kê số lượng sách của từng Thể loại: SELECT c.Name, COUNT(b.Id) FROM Categories c LEFT JOIN Books b ON c.Id = b.CategoryId GROUP BY c.Id, c.Name. Tại sao trong hàm COUNT(...) người ta lại viết COUNT(b.Id) (đếm cột của bảng bên phải) mà không viết COUNT(*)?",
                new List<string>
                {
                    "A. Vì với thể loại chưa có cuốn sách nào, LEFT JOIN vẫn sinh ra 1 dòng có b.Id = NULL; nếu dùng COUNT(*) nó sẽ đếm dòng đó thành 1 cuốn (bị sai), còn COUNT(b.Id) sẽ bỏ qua giá trị NULL và trả về đúng kết quả là 0 cuốn!",
                    "B. Vì COUNT(*) bị cấm dùng cùng với LEFT JOIN",
                    "C. Vì COUNT(b.Id) chạy chậm hơn",
                    "D. Hai cách đều ra kết quả bằng 0 giống hệt nhau"
                },
                "A",
                "Đây là câu hỏi bẫy kinh điển khi phỏng vấn SQL: COUNT(*) đếm tổng số dòng (kể cả dòng chứa toàn NULL ở vế phải của LEFT JOIN), còn COUNT(tên_cột) chỉ đếm những ô khác NULL!"
            ),

            // =====================================================================
            // BÀI 7.5
            // =====================================================================
            Build(
                topicId,
                "7.5",
                "Thống kê và Gom nhóm dữ liệu chuyên sâu (GROUP BY, Các hàm tổng hợp và Phân biệt WHERE vs HAVING)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/t-sql/queries/select-group-by-transact-sql",
                """
                • **💡 1. Quy tắc Sống Còn của `GROUP BY` mà ai cũng từng bị báo lỗi đỏ:**
                  - Khi câu lệnh đã có **`GROUP BY`**, mọi cột xuất hiện trên mệnh đề `SELECT` bắt buộc phải thuộc 1 trong 2 loại sau:
                    1. Hoặc là cột đó **nằm bên trong các hàm tổng hợp (`COUNT`, `SUM`, `AVG`, `MIN`, `MAX`)**.
                    2. Hoặc là cột đó **phải được liệt kê đầy đủ ở sau từ khóa `GROUP BY`**!
                  - *(Nếu bạn `SELECT CategoryId, Title, COUNT(*)` mà ở dưới chỉ viết `GROUP BY CategoryId` thiếu mất `Title`, SQL Server sẽ báo lỗi đỏ ngay lập tức!)*
                • **🖼️ 2. Sơ đồ Phân Biệt Rạch Ròi Giữa `WHERE` và `HAVING` (Câu hỏi thi & phỏng vấn số 1):**
                ```
                Bảng Dữ Liệu Gốc (1.000 dòng)
                     │
                     ▼
                [1. Mệnh đề WHERE]   ──► Lọc TỪNG DÒNG LẺ TRƯỚC KHI gom nhóm!
                     │                   (KHÔNG ĐƯỢC dùng các hàm COUNT, SUM, AVG ở trong WHERE!)
                     ▼
                [2. Mệnh đề GROUP BY]──► Gom các dòng đã lọc vào thành từng Nhóm (Giỏ)
                     │
                     ▼
                [3. Mệnh đề HAVING]  ──► Lọc CÁC NHÓM SAU KHI đã gom xong!
                                         (Chuyên dùng để đặt điều kiện trên hàm tổng hợp: HAVING COUNT(*) > 5, HAVING SUM(...) >= 1000)
                ```
                • **📊 3. Bảng So Sánh `WHERE` vs `HAVING`:**
                | Tiêu chí | Mệnh đề `WHERE` | Mệnh đề `HAVING` |
                | :--- | :--- | :--- |
                | **Thời điểm chạy** | Chạy **TRƯỚC** `GROUP BY`. | Chạy **SAU** `GROUP BY`. |
                | **Đối tượng bị lọc** | Lọc từng dòng bản ghi thô trong bảng. | Lọc các nhóm đã được gộp lại. |
                | **Dùng hàm `SUM`, `COUNT`?**| ❌ **CẤM TUYỆT ĐỐI** (`WHERE COUNT(*) > 2` sẽ báo lỗi!). | ✅ **BẮT BUỘC dùng ở đây** (`HAVING COUNT(*) > 2`). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Dùng WHERE lọc dòng trước khi gom nhóm",
                        "📥 Đầu vào: Bảng Đơn hàng (Orders) trong năm 2026. ⚙️ Cơ chế ngầm: Dùng WHERE để chỉ giữ lại các đơn hàng đã thanh toán thành công (Status = 2) trước khi đem đi cộng tổng. 📤 Đầu ra: Dữ liệu sạch sẵn sàng gom nhóm.",
                        "SELECT CustomerId, COUNT(Id) AS TotalOrders, SUM(TotalAmount) AS TotalSpent\nFROM Orders\nWHERE Status = 2 AND YEAR(OrderDate) = 2026"),
                    Step(2,
                        "2. Gom nhóm theo Khách hàng bằng GROUP BY",
                        "📥 Đầu vào: Các đơn hàng đã thanh toán. ⚙️ Cơ chế ngầm: Gom tất cả đơn hàng của cùng một CustomerId lại để tính tổng số đơn và tổng số tiền đã chi tiêu của từng người. 📤 Đầu ra: Thống kê theo từng khách.",
                        "GROUP BY CustomerId"),
                    Step(3,
                        "3. Dùng HAVING lọc ra các Khách hàng VIP (Chi tiêu trên 10 triệu)",
                        "📥 Đầu vào: Kết quả tổng tiền của từng nhóm khách hàng. ⚙️ Cơ chế ngầm: Vì điều kiện lọc nằm trên hàm tổng hợp SUM(TotalAmount), ta bắt buộc dùng HAVING để chỉ giữ lại khách hàng có tổng chi >= 10.000.000đ. 📤 Đầu ra: Danh sách khách VIP.",
                        "HAVING SUM(TotalAmount) >= 10000000\nORDER BY TotalSpent DESC;")
                },
                "Bạn muốn lấy ra danh sách các Khoa (DepartmentId) có số lượng sinh viên lớn hơn 100 người. Câu lệnh SQL nào dưới đây là ĐÚNG CÚ PHÁP?",
                new List<string>
                {
                    "A. SELECT DepartmentId, COUNT(Id) FROM Students WHERE COUNT(Id) > 100 GROUP BY DepartmentId",
                    "B. SELECT DepartmentId, COUNT(Id) FROM Students GROUP BY DepartmentId HAVING COUNT(Id) > 100",
                    "C. SELECT DepartmentId, COUNT(Id) FROM Students HAVING COUNT(Id) > 100",
                    "D. SELECT * FROM Students GROUP BY DepartmentId"
                },
                "B",
                "Không bao giờ được đặt hàm tổng hợp COUNT(Id) > 100 bên trong mệnh đề WHERE. Mọi điều kiện lọc dựa trên kết quả của hàm tổng hợp (COUNT, SUM, AVG) đều phải đặt trong mệnh đề HAVING đứng sau GROUP BY."
            ),

            // =====================================================================
            // BÀI 7.6
            // =====================================================================
            Build(
                topicId,
                "7.6",
                "Truy vấn con (Subquery), Bảng tạm CTE (Common Table Expressions - WITH) & Window Functions (ROW_NUMBER, RANK)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/t-sql/queries/with-common-table-expression-transact-sql",
                """
                • **💡 1. Hiểu bản chất của `CTE (WITH ... AS)` trong 30 giây:**
                  - Khi viết một câu truy vấn phức tạp phải lồng 2-3 câu `SELECT` ở bên trong nhau (**Subquery**), câu lệnh sẽ trông như một đám rừng ngoặc tròn `((SELECT ... (SELECT ...)))` cực kỳ khó đọc!
                  - **`CTE` (Common Table Expression — Khai báo bằng từ khóa `WITH TenBangTam AS (...)` ở đầu câu)** giúp bạn tách câu truy vấn con ra thành một **"Bảng tạm ảo có tên rõ ràng"** ở phía trên, sau đó ở bên dưới bạn chỉ việc `SELECT` hoặc `JOIN` với cái tên bảng tạm đó cực kỳ thoáng mắt và sạch sẽ!
                • **🖼️ 2. Vũ khí nâng cao `Window Functions` (`ROW_NUMBER()`, `RANK()`, `DENSE_RANK()`):**
                ```
                Giả sử điểm thi của 4 bạn là: Việt (10đ), An (9đ), Bình (9đ), Nam (8đ).
                Khi dùng hàm xếp hạng kèm OVER (ORDER BY Score DESC), sự khác nhau giữa 3 hàm là:

                Họ tên   │ Điểm │ ROW_NUMBER() (Đánh số thứ tự) │ RANK() (Đồng hạng thì nhảy cóc) │ DENSE_RANK() (Đồng hạng liên tiếp)
                ─────────┼──────┼───────────────────────────────┼─────────────────────────────────┼───────────────────────────────────
                Việt     │  10  │               1               │                1                │                 1
                An       │   9  │               2               │                2 (Đồng hạng 2)  │                 2 (Đồng hạng 2)
                Bình     │   9  │               3 (Không trùng) │                2 (Đồng hạng 2)  │                 2 (Đồng hạng 2)
                Nam      │   8  │               4               │                4 (Nhảy bỏ số 3!)│                 3 (Không bỏ số 3!)
                ```
                • **📊 3. Khi nào dùng `PARTITION BY` bên trong `OVER(...)`?**
                  - Nếu đề bài yêu cầu: *"Tìm ra **Top 2 cuốn sách đắt nhất CỦA TỪNG THỂ LOẠI**"* (chứ không phải của toàn bộ thư viện!), bạn chỉ cần dùng `ROW_NUMBER() OVER (PARTITION BY CategoryId ORDER BY Price DESC)` kết hợp với `CTE`! Từ khóa `PARTITION BY CategoryId` sẽ tự động reset bộ đếm về `1` mỗi khi sang một Thể loại mới!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Truy vấn con (Subquery) tìm sách có giá cao hơn giá trung bình",
                        "📥 Đầu vào: Bảng Books. ⚙️ Cơ chế ngầm: Câu lệnh con trong ngoặc (SELECT AVG(Price) FROM Books) chạy trước để tính ra mức giá trung bình, sau đó câu lệnh ngoài lấy các sách lớn hơn con số đó. 📤 Đầu ra: Danh sách sách giá cao.",
                        "SELECT Id, Title, Price\nFROM Books\nWHERE Price > (SELECT AVG(Price) FROM Books);"),
                    Step(2,
                        "2. Khai báo CTE (WITH) kết hợp ROW_NUMBER() OVER (PARTITION BY ...)",
                        "📥 Đầu vào: Bài toán lấy mỗi Thể loại 2 cuốn sách đắt nhất. ⚙️ Cơ chế ngầm: Tạo bảng tạm RankedBooks đánh số thứ tự giá giảm dần theo từng CategoryId. 📤 Đầu ra: Bảng tạm đã có cột RowNum.",
                        "WITH RankedBooks AS (\n    SELECT \n        Id, Title, CategoryId, Price,\n        ROW_NUMBER() OVER (PARTITION BY CategoryId ORDER BY Price DESC) AS RowNum\n    FROM Books\n)"),
                    Step(3,
                        "3. Truy vấn từ bảng tạm CTE để lấy Top 2 mỗi nhóm",
                        "📥 Đầu vào: Bảng tạm RankedBooks ở Bước 2. ⚙️ Cơ chế ngầm: Chỉ cần lọc WHERE RowNum <= 2 từ bảng tạm CTE là lấy được ngay 2 cuốn đắt nhất của mỗi thể loại! 📤 Đầu ra: Kết quả chuẩn xác, code cực kỳ dễ đọc.",
                        "SELECT Id, Title, CategoryId, Price\nFROM RankedBooks\nWHERE RowNum <= 2;")
                },
                "Trong SQL Server, khi xếp hạng điểm số bằng các hàm Window Function: nếu có 2 sinh viên cùng đạt 9.0 điểm (đồng hạng 2), hàm nào dưới đây sẽ xếp người đạt 8.5 điểm tiếp theo ở hạng 3 (không bị nhảy cóc bỏ qua số 3)?",
                new List<string>
                {
                    "A. Hàm DENSE_RANK()",
                    "B. Hàm RANK()",
                    "C. Hàm ROW_NUMBER()",
                    "D. Hàm SUM()"
                },
                "A",
                "RANK() sẽ xếp là 1, 2, 2, 4 (nhảy cóc qua hạng 3), trong khi DENSE_RANK() (Xếp hạng dày đặc) sẽ xếp là 1, 2, 2, 3 (không bỏ trống thứ hạng nào)."
            ),

            // =====================================================================
            // BÀI 7.7
            // =====================================================================
            Build(
                topicId,
                "7.7",
                "Tối ưu Hiệu năng SQL Server với Chỉ mục (Index: Clustered vs Non-Clustered Index)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/relational-databases/indexes/clustered-and-nonclustered-indexes-described",
                """
                • **💡 1. Hiểu bản chất của `Index` (Chỉ mục) qua hình ảnh "Mục lục cuốn từ điển" trong 30 giây:**
                  - Giả sử bạn có một cuốn sách dày 2.000 trang **không có trang Mục lục**. Nếu muốn tìm bài *"Lập trình bất đồng bộ"*, bạn phải lật tìm từng trang một từ trang 1 đến trang 2.000 (**Table Scan** — Quét toàn bộ bảng, cực kỳ chậm!).
                  - **`Index` (Chỉ mục)** chính là **Trang Mục Lục được sắp xếp theo cấu trúc Cây Nhị Phân (`B-Tree`)**: Bạn mở mục lục tra 3 bước là biết ngay nội dung nằm ở trang 850 và lật thẳng tới trang đó chỉ trong `0.001 giây` (**Index Seek**)!
                • **🖼️ 2. Sơ đồ Phân Biệt `Clustered Index` vs `Non-Clustered Index` (Câu hỏi phỏng vấn kinh điển):**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 1. CLUSTERED INDEX (Chỉ mục Cụm - Giống như Số Trang Sách vật lý):         │
                │    - Sắp xếp trực tiếp thứ tự vật lý của các dòng dữ liệu thật trong bảng. │
                │    - Mỗi bảng chỉ có TỐI ĐA 1 Clustered Index duy nhất!                    │
                │    - Mặc định khi bạn tạo PRIMARY KEY (Id), SQL tự tạo Clustered Index!    │
                ├────────────────────────────────────────────────────────────────────────────┤
                │ 2. NON-CLUSTERED INDEX (Chỉ mục Phi cụm - Giống bảng Tra cứu cuối sách):   │
                │    - Là một bảng phụ lưu cột cần tìm (VD: Email, Phone) kèm "Con trỏ"      │
                │      chỉ về khóa chính Id của dòng thật.                                   │
                │    - Một bảng có thể tạo NHIỀU Non-Clustered Index (tối đa 999 cái)!       │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Khi nào NÊN đánh Index và khi nào KHÔNG NÊN lạm dụng Index?**
                | Trường hợp NÊN tạo Non-Clustered Index ✅ | Tại sao KHÔNG NÊN đánh Index lên tất cả mọi cột? ⚠️ |
                | :--- | :--- |
                | 1. Các cột **Khóa ngoại (`Foreign Key`)** (như `CategoryId`, `UserId`) để tăng tốc độ `JOIN`. | Mỗi khi bạn chạy lệnh **`INSERT`, `UPDATE`, `DELETE`**, SQL Server không chỉ lưu vào bảng chính mà còn phải **cập nhật và sắp xếp lại toàn bộ các cây Index**! |
                | 2. Các cột thường xuyên nằm trong mệnh đề **`WHERE`**, **`ORDER BY`** (như `Email`, `PhoneNumber`, `BookCode`). | Đánh quá nhiều Index thừa sẽ làm **chậm tốc độ Thêm/Sửa/Xóa** và tốn dung lượng ổ cứng! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Clustered Index tự động tạo trên Khóa chính (Primary Key)",
                        "📥 Đầu vào: Tạo bảng Users với cột Id là PRIMARY KEY. ⚙️ Cơ chế ngầm: SQL Server tự động tạo 1 Clustered Index sắp xếp toàn bộ các dòng trong bảng theo thứ tự tăng dần của Id. 📤 Đầu ra: Tìm theo WHERE Id = ... nhanh tức thì.",
                        "-- Khi khai báo PRIMARY KEY, Clustered Index tự động được tạo:\nCREATE TABLE Users (\n    Id INT PRIMARY KEY IDENTITY(1,1),\n    Email VARCHAR(100) NOT NULL,\n    Phone VARCHAR(20) NOT NULL\n);"),
                    Step(2,
                        "2. Tạo Non-Clustered Index cho cột thường xuyên tìm kiếm (WHERE)",
                        "📥 Đầu vào: Chức năng Đăng nhập và Tìm kiếm luôn lọc theo cột Email và Phone. ⚙️ Cơ chế ngầm: Tạo Non-Clustered Index giúp SQL chuyển từ Table Scan (quét cả triệu dòng) sang Index Seek (nhảy thẳng tới đích). 📤 Đầu ra: Tốc độ tìm kiếm tăng gấp hàng trăm lần.",
                        "CREATE UNIQUE NONCLUSTERED INDEX IX_Users_Email ON Users(Email);\nCREATE NONCLUSTERED INDEX IX_Users_Phone ON Users(Phone);"),
                    Step(3,
                        "3. Tối ưu đỉnh cao với Covering Index (INCLUDE)",
                        "📥 Đầu vào: Câu lệnh SELECT Id, Email, FullName FROM Users WHERE Email = '...'. ⚙️ Cơ chế ngầm: Đính kèm thêm cột FullName vào lá của Index bằng từ khóa INCLUDE để SQL lấy luôn đủ dữ liệu ngay trên Index mà không cần quay lại bảng gốc (Key Lookup)! 📤 Đầu ra: Truy vấn siêu tốc.",
                        "CREATE NONCLUSTERED INDEX IX_Users_Email_Include\nON Users(Email)\nINCLUDE (FullName);")
                },
                "Trong một bảng của SQL Server, có thể tồn tại tối đa bao nhiêu Clustered Index và bao nhiêu Non-Clustered Index?",
                new List<string>
                {
                    "A. Chỉ có tối đa 1 Clustered Index duy nhất (vì dữ liệu vật lý của bảng chỉ có thể sắp xếp theo 1 trật tự duy nhất), nhưng có thể có nhiều Non-Clustered Index",
                    "B. Có thể tạo không giới hạn số lượng Clustered Index",
                    "C. Chỉ có tối đa 1 Non-Clustered Index và nhiều Clustered Index",
                    "D. Mỗi bảng chỉ được phép có tổng cộng 1 Index"
                },
                "A",
                "Vì Clustered Index quy định trực tiếp thứ tự sắp xếp vật lý của các bản ghi trên đĩa cứng, nên mỗi bảng chỉ có thể có duy nhất 1 Clustered Index (thường nằm ở cột Primary Key)."
            ),

            // =====================================================================
            // BÀI 7.8
            // =====================================================================
            Build(
                topicId,
                "7.8",
                "View (Bảng ảo), Stored Procedure (Thủ tục lưu trữ) và User-Defined Function (Hàm tự định nghĩa)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/relational-databases/stored-procedures/stored-procedures-database-engine",
                """
                • **💡 1. Hiểu bản chất của bộ ba `View`, `Stored Procedure` và `Function` trong 30 giây:**
                  - **`View` (Khung nhìn / Bảng ảo):** Là một câu lệnh `SELECT` dài (nối 4-5 bảng) được **đặt tên và lưu lại như một bảng ảo**. Lần sau thay vì viết lại 30 dòng `JOIN`, bạn chỉ cần gọi `SELECT * FROM vw_BookDetails`!
                  - **`Stored Procedure` (Thủ tục lưu trữ - `USP / SP`):** Là một **hàm chương trình viết bằng T-SQL lưu sẵn trong SQL Server** (có nhận tham số `@Param`, có biến, có `IF-ELSE`, `TRANSACTION` và thực hiện được cả `SELECT, INSERT, UPDATE, DELETE`). Đã được biên dịch sẵn kế hoạch chạy (*Execution Plan*) nên tốc độ cực nhanh!
                  - **`Function` (Hàm tự định nghĩa - `UDF`):** Chỉ dùng để tính toán và trả về 1 giá trị (`Scalar Function`) hoặc 1 bảng (`Table-Valued Function`), có thể nhúng trực tiếp vào trong câu lệnh `SELECT`!
                • **🖼️ 2. Bảng So Sánh `Stored Procedure` vs `Function (UDF)` (Câu hỏi bảo vệ môn Hệ Quản Trị CSDL):**
                | Tiêu chí | `Stored Procedure` (`CREATE PROCEDURE`) | `Function` (`CREATE FUNCTION`) |
                | :--- | :--- | :--- |
                | **Cách gọi thực thi** | Phải gọi bằng lệnh **`EXEC tên_proc ...`** độc lập. | Gọi trực tiếp ngay bên trong câu lệnh **`SELECT` / `WHERE`**! |
                | **Sửa dữ liệu (`INSERT/UPDATE/DELETE`)?**| ✅ **ĐƯỢC PHÉP** làm mọi thao tác thêm, sửa, xóa và dùng `TRY-CATCH`, `TRANSACTION`. | ❌ **CẤM TUYỆT ĐỐI** không được làm thay đổi dữ liệu của các bảng! |
                | **Giá trị trả về** | Không bắt buộc trả về, hoặc trả về nhiều bảng kết quả / tham số `OUTPUT`. | **BẮT BUỘC** phải có lệnh `RETURNS` trả về giá trị hoặc bảng. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo View đóng gói câu lệnh JOIN phức tạp",
                        "📥 Đầu vào: Câu lệnh nối bảng Sách và Thể loại thường xuyên dùng đi dùng lại. ⚙️ Cơ chế ngầm: Lưu câu truy vấn dưới tên vw_ActiveBooks. View không tốn dung lượng lưu dữ liệu trùng lặp mà luôn lấy dữ liệu tươi mới từ bảng gốc. 📤 Đầu ra: Bảng ảo tiện lợi.",
                        "CREATE VIEW vw_ActiveBooks AS\nSELECT b.Id, b.Title, b.Price, c.Name AS CategoryName\nFROM Books b\nINNER JOIN Categories c ON b.CategoryId = c.Id\nWHERE b.IsDeleted = 0;\nGO\n-- Cách dùng: SELECT * FROM vw_ActiveBooks;"),
                    Step(2,
                        "2. Tạo Stored Procedure có tham số đầu vào (@Param)",
                        "📥 Đầu vào: Tham số @CategoryId và @MinPrice. ⚙️ Cơ chế ngầm: SQL Server kiểm tra cú pháp và lưu sẵn kế hoạch thực thi tối ưu. C# có thể gọi trực tiếp Procedure này qua EF Core (FromSqlRaw). 📤 Đầu ra: Thủ tục xử lý nghiệp vụ nhanh gọn.",
                        "CREATE PROCEDURE sp_GetBooksByFilter\n    @CategoryId INT,\n    @MinPrice DECIMAL(18,2)\nAS\nBEGIN\n    SET NOCOUNT ON;\n    SELECT Id, Title, Price FROM Books\n    WHERE CategoryId = @CategoryId AND Price >= @MinPrice;\nEND;\nGO\n-- Cách gọi: EXEC sp_GetBooksByFilter @CategoryId = 1, @MinPrice = 50000;"),
                    Step(3,
                        "3. Gọi Stored Procedure trực tiếp từ C# (EF Core)",
                        "📥 Đầu vào: Tên Stored Procedure đã tạo trong SQL Server. ⚙️ Cơ chế ngầm: Dùng hàm FromSqlInterpolated của EF Core để truyền tham số an toàn chống lỗi bảo mật SQL Injection. 📤 Đầu ra: Danh sách Entity trong C#.",
                        "// Trong C# Repository:\nvar books = await _db.Books\n    .FromSqlInterpolated($\"EXEC sp_GetBooksByFilter {catId}, {minPrice}\")\n    .ToListAsync();")
                },
                "Điểm khác biệt quan trọng nhất giữa Stored Procedure và User-Defined Function (Hàm) trong SQL Server là gì?",
                new List<string>
                {
                    "A. Stored Procedure có thể thực hiện các câu lệnh làm thay đổi dữ liệu trong bảng (INSERT, UPDATE, DELETE) và dùng Transaction, trong khi Function chỉ được phép đọc/tính toán trả về kết quả và có thể nhúng trực tiếp vào câu lệnh SELECT",
                    "B. Function có thể xóa bảng còn Stored Procedure thì không",
                    "C. Stored Procedure không nhận được tham số đầu vào",
                    "D. Hai khái niệm hoàn toàn giống hệt nhau"
                },
                "A",
                "Function bắt buộc phải trả về một giá trị và không được phép gây tác dụng phụ (không được INSERT/UPDATE/DELETE bảng thật), trong khi Stored Procedure được phép thực thi toàn bộ các thao tác thay đổi dữ liệu và quản lý giao dịch."
            ),

            // =====================================================================
            // BÀI 7.9
            // =====================================================================
            Build(
                topicId,
                "7.9",
                "Giao dịch (Transaction - Tính chất ACID), Trigger (Bảng ảo Inserted/Deleted) & Phòng chống SQL Injection",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/t-sql/language-elements/transactions-transact-sql",
                """
                • **💡 1. Hiểu bản chất của `ACID` trong Giao dịch Database (`Transaction`) trong 30 giây:**
                  Khi đi phỏng vấn hoặc thi môn CSDL, câu hỏi *"4 tính chất ACID của Transaction là gì?"* luôn xuất hiện:
                  1. **`A - Atomicity` (Tính Nguyên tử):** *Tất cả hoặc Không gì cả* — Một giao dịch gồm 5 bước thì cả 5 bước phải thành công hết mới lưu (`COMMIT`); nếu 1 bước lỗi thì hoàn tác sạch cả 5 bước (`ROLLBACK`).
                  2. **`C - Consistency` (Tính Nhất quán):** Dữ liệu trước và sau giao dịch luôn tuân thủ đúng mọi ràng buộc Khóa ngoại, Check (tổng tiền không tự nhiên sinh ra hay mất đi).
                  3. **`I - Isolation` (Tính Cô lập):** Hai người cùng đặt mua 1 cuốn sách cuối cùng trong kho tại cùng 1 giây sẽ được xếp hàng xử lý tách biệt, không làm loạn dữ liệu của nhau.
                  4. **`D - Durability` (Tính Bền vững):** Một khi đã `COMMIT` thành công, dữ liệu được ghi hẳn xuống ổ cứng — dù ngay giây sau bị rút phích cắm mất điện thì bật máy lên dữ liệu vẫn còn nguyên!
                • **🖼️ 2. Cơ chế hoạt động của `Trigger` và 2 bảng ảo thần thánh `inserted` / `deleted`:**
                ```
                Trigger là đoạn code tự động kích hoạt ngầm khi có sự kiện INSERT, UPDATE hoặc DELETE trên bảng.
                Bên trong Trigger, SQL Server cung cấp sẵn 2 bảng tạm trong bộ nhớ:
                ├── Bảng [inserted] : Chứa các dòng dữ liệu MỚI vừa được thêm vào (khi INSERT) hoặc giá trị MỚI sau khi sửa (khi UPDATE).
                └── Bảng [deleted]  : Chứa các dòng dữ liệu CŨ vừa bị xóa đi (khi DELETE) hoặc giá trị CŨ trước khi sửa (khi UPDATE).
                ```
                • **📊 3. Lỗ hổng bảo mật `SQL Injection` và Cách phòng chống tuyệt đối:**
                  - Nếu bạn nối chuỗi trực tiếp: `"SELECT * FROM Users WHERE Username = '" + input + "'"` và kẻ gian nhập vào ô Username chuỗi: **`admin' OR '1'='1`**, câu lệnh biến thành `WHERE Username = 'admin' OR '1'='1'` (luôn luôn đúng!) $\rightarrow$ Kẻ gian đăng nhập thẳng vào quyền Admin không cần mật khẩu!
                  - **Cách phòng chống 100%:** Luôn dùng **Tham số hóa (`@Parameter`)** hoặc dùng **LINQ / Entity Framework Core** (vì EF Core tự động biến mọi biến C# thành `@p0, @p1` an toàn tuyệt đối!).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Viết Transaction chuẩn trong T-SQL với TRY...CATCH",
                        "📥 Đầu vào: Nghiệp vụ tạo phiếu mượn sách và trừ số lượng sách trong kho. ⚙️ Cơ chế ngầm: Bọc trong BEGIN TRAN ... COMMIT TRAN. Nếu bất kỳ lệnh nào lỗi, nhảy ngay xuống khối CATCH để ROLLBACK TRAN hoàn tác toàn bộ. 📤 Đầu ra: Đảm bảo tính ACID tuyệt đối.",
                        "BEGIN TRY\n    BEGIN TRANSACTION;\n        INSERT INTO Borrows (ReaderId, BorrowDate) VALUES (1, GETDATE());\n        UPDATE Books SET Stock = Stock - 1 WHERE Id = 10; -- Nếu Stock < 0 vi phạm CHECK -> Nổ lỗi!\n    COMMIT TRANSACTION;\nEND TRY\nBEGIN CATCH\n    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;\n    THROW;\nEND CATCH;"),
                    Step(2,
                        "2. Viết Trigger tự động ghi lịch sử khi thay đổi giá sách",
                        "📥 Đầu vào: Sự kiện AFTER UPDATE trên bảng Books. ⚙️ Cơ chế ngầm: Lấy giá cũ từ bảng ảo [deleted] và giá mới từ bảng ảo [inserted] để tự động lưu vết vào bảng PriceHistory. 📤 Đầu ra: Tự động lưu lịch sử kiểm toán.",
                        "CREATE TRIGGER trg_BookPriceAudit\nON Books AFTER UPDATE AS\nBEGIN\n    IF UPDATE(Price)\n    BEGIN\n        INSERT INTO PriceHistory (BookId, OldPrice, NewPrice, ChangedAt)\n        SELECT d.Id, d.Price, i.Price, GETDATE()\n        FROM deleted d INNER JOIN inserted i ON d.Id = i.Id;\n    END\nEND;"),
                    Step(3,
                        "3. Chặn đứng SQL Injection bằng Parameterized Query",
                        "📥 Đầu vào: Từ khóa tìm kiếm từ người dùng. ⚙️ Cơ chế ngầm: Truyền qua biến tham số @Email thay vì cộng chuỗi trực tiếp. 📤 Đầu ra: Kẻ gian nhập ký tự ' OR 1=1 cũng chỉ bị coi là một chuỗi văn bản vô hại.",
                        "-- Luôn truyền qua tham số @Email:\nSELECT Id, FullName FROM Users WHERE Email = @Email;")
                },
                "Khi viết một Trigger bắt sự kiện AFTER UPDATE trên bảng Products trong SQL Server, làm thế nào để bạn lấy được giá trị cũ trước khi sửa và giá trị mới sau khi sửa của bản ghi vừa bị cập nhật?",
                new List<string>
                {
                    "A. Truy vấn từ 2 bảng ảo đặc biệt có sẵn trong Trigger: bảng [deleted] (chứa dữ liệu cũ trước khi sửa) và bảng [inserted] (chứa dữ liệu mới sau khi sửa)",
                    "B. Không thể lấy được giá trị cũ vì nó đã bị ghi đè mất",
                    "C. Phải mở file Log của Windows để tìm",
                    "D. Hỏi người dùng xem trước đó họ nhập số mấy"
                },
                "A",
                "Trong SQL Server Trigger, thao tác UPDATE được xử lý như một bước xóa dòng cũ (lưu vào bảng ảo deleted) và thêm dòng mới (lưu vào bảng ảo inserted). Nối (JOIN) hai bảng ảo này theo Id giúp bạn so sánh chính xác giá trị cũ và mới."
            ),

            // =====================================================================
            // BÀI 7.10
            // =====================================================================
            Build(
                topicId,
                "7.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 7 (Thiết kế & Truy vấn CSDL Quản lý Thư viện / Bán hàng thực tế)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/sql/sql-server/",
                """
                • **💡 1. Quy trình 5 Bước Chuẩn Mực Khi Thiết Kế & Tối Ưu Cơ Sở Dữ Liệu Cho Dự Án:**
                  1. **Bước 1 (Phân tích thực thể & ERD):** Xác định các bảng chính, tách bảng trung gian cho các quan hệ Nhiều - Nhiều (`N - N`), đặt khóa ngoại ở bảng bên Nhiều (`1 - N`).
                  2. **Bước 2 (Chọn kiểu dữ liệu chuẩn):** Tiền tệ dùng `DECIMAL(18,2)`, chuỗi tiếng Việt dùng `NVARCHAR`, chuỗi mã/email không dấu dùng `VARCHAR`, trạng thái dùng `INT` (ánh xạ với `Enum` trong C#).
                  3. **Bước 3 (Đặt ràng buộc toàn vẹn):** Khóa chính `PRIMARY KEY`, khóa ngoại `FOREIGN KEY`, chống trùng `UNIQUE` và kiểm tra giá trị âm `CHECK`.
                  4. **Bước 4 (Đánh chỉ mục Index):** Tạo `NONCLUSTERED INDEX` trên tất cả các cột Khóa ngoại (`Foreign Key`) và các cột thường xuyên tìm kiếm trong `WHERE`.
                  5. **Bước 5 (Viết truy vấn tối ưu):** Chỉ `SELECT` đúng những cột cần dùng (hạn chế `SELECT *`), dùng `LEFT JOIN` khi cần giữ cả các danh mục chưa có dữ liệu con, và luôn phân trang bằng `OFFSET ... FETCH NEXT`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo cấu trúc bảng chuẩn 3NF kèm Ràng buộc",
                        "📥 Đầu vào: Bài toán Quản lý Mượn trả sách thư viện. ⚙️ Cơ chế ngầm: Thiết lập bảng Books và Borrows có đầy đủ PK, FK, CHECK tồn kho >= 0. 📤 Đầu ra: CSDL vững chắc ngay từ tầng vật lý.",
                        "CREATE TABLE Books (\n    Id INT PRIMARY KEY IDENTITY(1,1),\n    BookCode VARCHAR(20) NOT NULL UNIQUE,\n    Title NVARCHAR(200) NOT NULL,\n    Stock INT NOT NULL CHECK (Stock >= 0)\n);"),
                    Step(2,
                        "2. Đánh Index tăng tốc tìm kiếm theo Mã sách và Tên sách",
                        "📥 Đầu vào: Thư viện có 50.000 đầu sách. ⚙️ Cơ chế ngầm: Tạo Non-Clustered Index trên cột Title giúp tìm kiếm sách nhanh tức thì. 📤 Đầu ra: Tối ưu hiệu năng đọc.",
                        "CREATE NONCLUSTERED INDEX IX_Books_Title ON Books(Title) INCLUDE (BookCode, Stock);"),
                    Step(3,
                        "3. Viết câu truy vấn Báo cáo Top 5 cuốn sách được mượn nhiều nhất",
                        "📥 Đầu vào: Bảng Books nối với bảng BorrowDetails. ⚙️ Cơ chế ngầm: Kết hợp INNER JOIN, GROUP BY, hàm SUM(Quantity) và ORDER BY DESC để xếp hạng sách hot nhất. 📤 Đầu ra: Dữ liệu biểu đồ Dashboard.",
                        "SELECT TOP(5)\n    b.BookCode,\n    b.Title,\n    SUM(bd.Quantity) AS TotalBorrowed\nFROM Books b\nINNER JOIN BorrowDetails bd ON b.Id = bd.BookId\nGROUP BY b.Id, b.BookCode, b.Title\nORDER BY TotalBorrowed DESC;"),
                    Step(4,
                        "4. Lọc các cuốn sách có tổng lượt mượn >= 10 bằng HAVING",
                        "📥 Đầu vào: Kết quả gom nhóm ở Bước 3. ⚙️ Cơ chế ngầm: Thêm mệnh đề HAVING SUM(bd.Quantity) >= 10 ngay sau GROUP BY để lọc nhóm. 📤 Đầu ra: Hoàn tất truy vấn báo cáo chuyên sâu.",
                        "HAVING SUM(bd.Quantity) >= 10\nORDER BY TotalBorrowed DESC;")
                },
                "(Câu hỏi tổng hợp Chủ đề 7) Khi thiết kế một bảng lớn trong SQL Server và viết câu truy vấn tìm kiếm phân trang cho Web API, tập hợp các kỹ thuật nào dưới đây giúp câu truy vấn đạt tốc độ nhanh nhất và an toàn nhất?",
                new List<string>
                {
                    "A. Đánh Non-Clustered Index trên cột tìm kiếm và cột Khóa ngoại, chỉ SELECT đúng các cột cần thiết thay vì SELECT *, sử dụng tham số hóa (@Param) chống SQL Injection và phân trang bằng OFFSET...FETCH",
                    "B. Dùng kiểu NVARCHAR(MAX) cho tất cả mọi cột termasuk cả cột tuổi và giá tiền, không tạo Khóa chính và luôn dùng SELECT *",
                    "C. Lấy toàn bộ 1 triệu dòng về C# rồi dùng vòng lặp for để lọc",
                    "D. Xóa hết Index trước khi truy vấn"
                },
                "A",
                "Kết hợp Index hợp lý, giới hạn cột SELECT, phân trang ngay tại tầng SQL Server (OFFSET-FETCH / .Skip().Take()) và truyền tham số an toàn là tiêu chuẩn vàng trong thiết kế và truy vấn CSDL doanh nghiệp."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (7.1 -> 7.10) cho Chủ đề 7 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic7 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 7);
        if (topic7 == null)
            return;

        topic7.Title = "Chủ đề 7: Làm việc với cơ sở dữ liệu SQL Server";
        topic7.Description = "10 Bài học con siêu chi tiết từ Thiết kế RDBMS & ERD (PK, FK, 1-N, N-N), Chuẩn hóa 1NF-2NF-3NF & Kiểu dữ liệu SQL-C#, Thứ tự thực thi SELECT, INNER/LEFT/SELF JOIN, GROUP BY & WHERE vs HAVING, Subquery/CTE/Window Functions, Clustered vs Non-Clustered Index, View/Stored Procedure/Function đến Transaction ACID, Trigger & chống SQL Injection.";

        var targetLessons = GetLessons(topic7.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic7.Id)
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
