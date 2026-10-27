using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class InternTopic3Seed
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
    /// Danh sách đầy đủ 10 bài học con (3.1 -> 3.10) của Chủ đề 3 (Thực tập): Đọc luồng Truy vấn Dữ liệu (LINQ, SQL & Entity Framework Core)
    /// </summary>
    public static List<SubLesson> GetLessons(int topicId)
    {
        return new List<SubLesson>
        {
            // =====================================================================
            // BÀI 3.1
            // =====================================================================
            Build(
                topicId,
                "3.1",
                "Đọc hiểu Biểu thức Lambda (x => x.Property) - Chìa khóa mở mọi câu lệnh LINQ",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/lambda-expressions",
                """
                • Bản chất: Khi mở bất kỳ file Repository hay Service nào ở công ty, bạn sẽ thấy ký hiệu mũi tên => xuất hiện dày đặc (VD: p => p.Price > 100, u => u.Email == email). Đó gọi là Lambda Expression (Biểu thức Lambda) — một hàm ẩn danh siêu ngắn gọn dùng để truyền điều kiện vào LINQ.
                • Cách đọc hiểu bằng tiếng Việt chỉ trong 3 giây:
                - Chữ cái đứng trước dấu => (ví dụ p, x, u, item): Đại diện cho 1 dòng bản ghi (1 đối tượng) đang được xét tới trong bảng. Bạn đặt tên là gì cũng được (thường lấy chữ cái đầu của bảng: p cho Product, u cho User).
                - Dấu =>: Đọc là "sao cho" hoặc "lấy ra".
                - Vế sau dấu =>: Là điều kiện kiểm tra (trả về true/false) hoặc cột muốn lấy. Ví dụ: p => p.IsActive == true đọc là: "Với mỗi sản phẩm p, sao cho thuộc tính IsActive của p bằng true".
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Xác định biến đại diện (Bên trái dấu =>)",
                        "Nhìn vào danh sách đang chấm (VD: _db.Users), chữ 'u' đứng trước => chính là 1 đối tượng User trong bảng đó.",
                        "_db.Users.Where(u => ...)"),
                    Step(2,
                        "Đọc điều kiện lọc (Sau dấu => trả về bool)",
                        "Khi đi kèm hàm Where hoặc FirstOrDefault, vế sau => là biểu thức so sánh trả về đúng/sai.",
                        "_db.Users.Where(u => u.Age >= 18 && !u.IsDeleted)"),
                    Step(3,
                        "Đọc phép chọn cột (Sau dấu => trả về dữ liệu)",
                        "Khi đi kèm hàm Select hoặc OrderBy, vế sau => chỉ định cột dữ liệu muốn bóc tách hoặc sắp xếp.",
                        "_db.Users.Select(u => u.FullName)")
                },
                "Khi đọc dòng code _db.Orders.Where(o => o.TotalAmount >= 500000 && o.Status == OrderStatus.Paid), chữ o đứng trước dấu => có ý nghĩa là gì?",
                new List<string>
                {
                    "A. Là từ khóa bắt buộc của ngôn ngữ C#, đổi thành chữ khác sẽ báo lỗi",
                    "B. Là biến đại diện cho từng bản ghi Order trong bảng Orders đang được đem ra kiểm tra điều kiện",
                    "C. Là tên của Database trong SQL Server",
                    "D. Là biến đếm số lượng đơn hàng"
                },
                "B",
                "Trong biểu thức Lambda o => ..., o là tham số đầu vào đại diện cho một phần tử Order trong tập hợp Orders. Bạn có thể đặt là o, x, hay order đều hoàn toàn giống nhau."
            ),

            // =====================================================================
            // BÀI 3.2
            // =====================================================================
            Build(
                topicId,
                "3.2",
                "Đọc luồng lấy 1 bản ghi: Phân biệt FindAsync, FirstOrDefaultAsync và SingleOrDefaultAsync",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/",
                """
                • Bản chất: Trong các hàm xem chi tiết (GetById), đăng nhập (Login), hoặc cập nhật (Update), bạn luôn cần tìm đúng 1 bản ghi từ Database.
                • Sự khác biệt sống còn giữa 3 hàm hay gặp nhất trong dự án:
                1. .FindAsync(id): Chỉ tìm theo Khóa chính (Primary Key). Đặc biệt: Nó kiểm tra trong bộ nhớ tạm (RAM/ChangeTracker) trước, nếu chưa có mới hỏi SQL Server -> Rất nhanh khi tìm theo ID.
                2. .FirstOrDefaultAsync(x => ...): Tìm bản ghi đầu tiên khớp điều kiện (tương đương SELECT TOP(1) trong SQL). Nếu không tìm thấy, trả về null rất an toàn. Đây là hàm được dùng nhiều nhất (90%) trong dự án thực tế!
                3. .SingleOrDefaultAsync(x => ...): Giống FirstOrDefault, nhưng sinh ra lệnh SELECT TOP(2) để kiểm tra khắt khe: nếu trong bảng có từ 2 bản ghi trở lên trùng điều kiện, nó sẽ văng lỗi sập hàm (Exception) ngay lập tức. Chỉ dùng khi cột đó bắt buộc duy nhất (như CCCD, Username).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Gọi FirstOrDefaultAsync trong Repository",
                        "Truyền biểu thức Lambda để tìm người dùng theo Email.",
                        "var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);"),
                    Step(2,
                        "EF Core dịch sang câu lệnh SQL",
                        "Tự động dịch thành lệnh SELECT TOP(1) tối ưu gửi xuống SQL Server.",
                        "SELECT TOP(1) [u].[Id], [u].[Email], [u].[FullName]\nFROM [Users] AS [u]\nWHERE [u].[Email] = @__email_0"),
                    Step(3,
                        "Kiểm tra null tại Service/Controller",
                        "Vì hàm có chữ 'OrDefault', nếu không có dữ liệu nó sẽ trả về null để bạn chặn bằng Guard Clause.",
                        "if (user == null) return NotFound(\"Email không tồn tại!\");")
                },
                "Tại sao trong thực tế, các lập trình viên luôn ưu tiên dùng FirstOrDefaultAsync() thay vì dùng FirstAsync() (không có chữ OrDefault)?",
                new List<string>
                {
                    "A. Vì FirstAsync() không chạy được với SQL Server",
                    "B. Vì nếu không tìm thấy bản ghi nào trong DB, FirstAsync() sẽ văng lỗi InvalidOperationException làm sập API (500 Error), còn FirstOrDefaultAsync() trả về null nhẹ nhàng để ta tự trả mã 404 NotFound",
                    "C. Vì FirstOrDefaultAsync() tự động tạo bản ghi mới nếu không tìm thấy",
                    "D. Vì FirstAsync() chỉ dùng cho kiểu số nguyên"
                },
                "B",
                "FirstAsync() giả định dữ liệu bắt buộc phải tồn tại và sẽ ném Exception nếu kết quả rỗng. FirstOrDefaultAsync() trả về giá trị mặc định (null đối với class), giúp luồng kiểm tra if (item == null) an toàn tuyệt đối."
            ),

            // =====================================================================
            // BÀI 3.3
            // =====================================================================
            Build(
                topicId,
                "3.3",
                "Đọc luồng lọc danh sách, sắp xếp & kiểm tra tồn tại (Where, OrderBy, AnyAsync, CountAsync)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/standard-query-operators/",
                """
                • Các mắt xích truy vấn danh sách luôn nối đuôi nhau (Method Chaining):
                - .Where(x => ...): Lọc các bản ghi thỏa mãn điều kiện (tương đương mệnh đề WHERE trong SQL). Có thể nối nhiều .Where() liên tiếp khi lọc theo nhiều tiêu chí tìm kiếm.
                - .OrderBy(x => ...) / .OrderByDescending(x => ...): Sắp xếp tăng dần / giảm dần (VD: sắp xếp bài viết mới nhất lên đầu bằng .OrderByDescending(x => x.CreatedAt)).
                - .AnyAsync(x => ...): Kiểm tra xem có tồn tại ít nhất 1 bản ghi nào không (trả về true/false). Lưu ý phỏng vấn: Khi kiểm tra trùng Email hay trùng Mã sản phẩm, bắt buộc dùng .AnyAsync() chứ tuyệt đối không dùng .CountAsync() > 0 (vì Any thấy 1 dòng là dừng ngay, còn Count phải đếm hết cả bảng triệu dòng rất chậm).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Kiểm tra trùng lặp siêu tốc với AnyAsync",
                        "Trước khi thêm mới, kiểm tra xem mã sản phẩm đã tồn tại trong bảng chưa.",
                        "bool isExist = await _db.Products.AnyAsync(p => p.Code == dto.Code);\nif (isExist) return BadRequest(\"Mã SP đã tồn tại!\");"),
                    Step(2,
                        "Nối chuỗi lọc Where và sắp xếp OrderBy",
                        "Lọc các sản phẩm đang mở bán và sắp xếp theo ngày tạo mới nhất.",
                        "var query = _db.Products\n    .Where(p => p.IsActive && p.Price >= minPrice)\n    .OrderByDescending(p => p.CreatedAt);"),
                    Step(3,
                        "Chốt hạ thực thi bằng ToListAsync()",
                        "Câu lệnh SQL thực sự được gửi đi và trả về danh sách List<Product>.",
                        "List<Product> result = await query.ToListAsync();")
                },
                "Khi đi thực tập, bạn cần viết code kiểm tra xem Username người dùng đăng ký đã có ai dùng trong bảng Users chưa. Cách viết nào dưới đây tối ưu hiệu năng nhất?",
                new List<string>
                {
                    "A. if ((await _db.Users.ToListAsync()).Where(u => u.Username == name).Count() > 0)",
                    "B. if (await _db.Users.AnyAsync(u => u.Username == name))",
                    "C. if (await _db.Users.CountAsync(u => u.Username == name) >= 1)",
                    "D. Dùng vòng lặp foreach duyệt toàn bộ bảng Users"
                },
                "B",
                "AnyAsync sinh ra câu lệnh IF EXISTS (SELECT 1 FROM Users WHERE ...) trong SQL. Ngay khi tìm thấy 1 bản ghi khớp, SQL Server dừng quét bảng ngay lập tức, nhanh gấp nhiều lần so với CountAsync hay ToListAsync."
            ),

            // =====================================================================
            // BÀI 3.4
            // =====================================================================
            Build(
                topicId,
                "3.4",
                "Bản chất IQueryable vs IEnumerable (Cơ chế Deferred Execution - Trì hoãn thực thi)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/how-query-works",
                """
                • Câu hỏi phỏng vấn số 1 mọi công ty .NET đều hỏi Intern/Fresher: "Sự khác nhau giữa IQueryable và IEnumerable là gì? Khi nào câu lệnh SQL thực sự chạy?"
                • Hiểu bản chất cực dễ:
                1. Trì hoãn thực thi (Deferred Execution): Khi bạn viết var query = _db.Products.Where(p => p.Price > 100);, lúc này CHƯA HỀ có câu lệnh SQL nào được gửi tới Database cả! Biến query mới chỉ là một "bản thiết kế câu hỏi" thuộc kiểu IQueryable<Product>.
                2. Khi nào SQL mới chạy thật? Chỉ khi bạn chấm các hàm "chốt đơn" như: await query.ToListAsync(), FirstOrDefaultAsync(), CountAsync(), AnyAsync(), thì EF Core mới gom toàn bộ các điều kiện .Where(), .OrderBy() trước đó dịch thành 1 câu SQL duy nhất gửi xuống SQL Server.
                3. Sự khác biệt:
                - IQueryable: Lọc dữ liệu ngay tại SQL Server (chỉ lấy những dòng thỏa mãn đem về RAM web) -> Luôn dùng khi ghép nối điều kiện truy vấn DB.
                - IEnumerable (hoặc khi đã lỡ gọi .ToList() sớm): Kéo toàn bộ bảng từ SQL Server lên RAM của máy chủ Web rồi mới dùng C# để lọc -> Làm tràn RAM nếu bảng có hàng trăm nghìn dòng!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khởi tạo IQueryable (Chưa gọi DB)",
                        "Mới chỉ tạo cây biểu thức (Expression Tree) trong bộ nhớ, chưa tốn 1 mili-giây kết nối SQL nào.",
                        "IQueryable<Product> query = _db.Products.Where(p => !p.IsDeleted);"),
                    Step(2,
                        "Cộng dồn điều kiện lọc động",
                        "Nếu người dùng có gửi từ khóa tìm kiếm thì nối thêm .Where() vào bản thiết kế.",
                        "if (!string.IsNullOrWhiteSpace(keyword)) {\n    query = query.Where(p => p.Name.Contains(keyword));\n}"),
                    Step(3,
                        "Thực thi tại SQL Server với ToListAsync()",
                        "Lúc này EF Core mới dịch toàn bộ Bước 1 + Bước 2 thành câu lệnh SQL WHERE ... AND ... gửi xuống DB.",
                        "var finalData = await query.ToListAsync(); // SQL chạy tại dòng này!")
                },
                "Hãy chỉ ra điểm sai nghiêm trọng về hiệu năng trong đoạn code sau: var list = await _db.Orders.ToListAsync(); var result = list.Where(x => x.UserId == 5).ToList();",
                new List<string>
                {
                    "A. Gọi ToListAsync() quá sớm ở dòng đầu khiến EF Core kéo TOÀN BỘ đơn hàng của cả hệ thống từ SQL Server lên RAM, sau đó mới lọc lấy UserId = 5 trên RAM",
                    "B. Không được phép gọi hàm Where sau khi đã có List",
                    "C. Sai cú pháp C# không biên dịch được",
                    "D. Làm xóa mất đơn hàng của UserId = 5"
                },
                "A",
                "Phải đặt .Where(x => x.UserId == 5) TRƯỚC .ToListAsync() (await _db.Orders.Where(x => x.UserId == 5).ToListAsync()) để SQL Server lọc sẵn dưới Database, chỉ trả về vài đơn hàng của User số 5."
            ),

            // =====================================================================
            // BÀI 3.5
            // =====================================================================
            Build(
                topicId,
                "3.5",
                "Đọc luồng Kết nối nhiều bảng (Relationships): Include, ThenInclude và Projection (.Select sang DTO)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/related-data/eager",
                """
                • Bản chất: Trong SQL Server, bảng Products chỉ lưu cột khóa ngoại CategoryId = 2, còn tên danh mục "Điện thoại" lại nằm ở bảng Categories. Khi lấy sản phẩm lên, nếu bạn không nối bảng (JOIN), thuộc tính product.Category sẽ bị null!
                • 2 cách lấy dữ liệu từ bảng liên kết trong EF Core:
                1. Dùng .Include() và .ThenInclude() (Eager Loading): Bảo EF Core hãy LEFT JOIN hoặc INNER JOIN bảng liên kết vào luôn.
                - .Include(p => p.Category): Lấy Sản phẩm kèm luôn đối tượng Danh mục của nó.
                - .ThenInclude(...): Nối tiếp sang bảng thứ 3 (VD: Lấy Đơn hàng -> .Include(o => o.OrderDetails) -> .ThenInclude(od => od.Product)).
                2. Dùng .Select() chiếu thẳng sang DTO (Projection - Khuyên dùng nhất): Dùng .Select(p => new ProductDto { Id = p.Id, Name = p.Name, CategoryName = p.Category.Name }). EF Core sẽ tự động sinh câu lệnh JOIN và chỉ lấy đúng 3 cột cần thiết thay vì lấy toàn bộ các cột thừa của cả 2 bảng!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nối bảng trực tiếp bằng Include",
                        "Dùng khi bạn cần lấy cả đối tượng Entity con lên để xử lý nghiệp vụ hoặc sửa đổi.",
                        "var order = await _db.Orders\n    .Include(o => o.Customer)\n    .Include(o => o.OrderItems)\n    .FirstOrDefaultAsync(o => o.Id == id);"),
                    Step(2,
                        "Hoặc dùng Select chiếu thẳng ra DTO (Tối ưu nhất)",
                        "Khi chỉ cần lấy dữ liệu hiển thị ra API, dùng Select để nhặt đúng tên cột từ bảng liên kết mà không cần viết Include.",
                        "var dtos = await _db.Products\n    .Select(p => new ProductItemDto {\n        Id = p.Id,\n        ProductName = p.Name,\n        CategoryName = p.Category.Name // Tự động JOIN bảng Category\n    }).ToListAsync();"),
                    Step(3,
                        "SQL sinh ra gọn nhẹ tuyệt đối",
                        "SQL Server chỉ SELECT đúng 3 cột được yêu cầu, tốc độ truy vấn cực nhanh.",
                        "SELECT [p].[Id], [p].[Name], [c].[Name] AS [CategoryName]\nFROM [Products] AS [p]\nINNER JOIN [Categories] AS [c] ON [p].[CategoryId] = [c].[Id]")
                },
                "Khi đọc code lấy danh sách bài viết (Posts), bạn thấy tác giả không dùng .Include(p => p.Author) nhưng trong câu lệnh .Select(p => new PostDto { Title = p.Title, AuthorName = p.Author.FullName }) vẫn lấy được AuthorName bình thường mà không bị lỗi Null. Tại sao?",
                new List<string>
                {
                    "A. Vì khi truy cập thuộc tính điều hướng (p.Author.FullName) bên trong hàm .Select() của IQueryable, EF Core tự động dịch thành câu lệnh SQL JOIN hai bảng mà không cần gọi .Include()",
                    "B. Vì dữ liệu tên tác giả đã được lưu sẵn trong bộ nhớ trình duyệt",
                    "C. Do may mắn chưa bị lỗi",
                    "D. Vì C# tự đoán được tên tác giả"
                },
                "A",
                "Khi dùng Projection (.Select() trên IQueryable), EF Core phân tích biểu thức và tự động tạo câu lệnh JOIN trong SQL để lấy đúng cột FullName từ bảng Authors."
            ),

            // =====================================================================
            // BÀI 3.6
            // =====================================================================
            Build(
                topicId,
                "3.6",
                "Đọc luồng Phân trang dữ liệu (Pagination) với Skip và Take trong mọi dự án thực tế",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/pagination",
                """
                • Tại sao đi làm mọi API lấy danh sách đều bắt buộc phải có Phân trang? Nếu bảng Products có 100.000 sản phẩm mà API trả về hết trong 1 lần gọi, cả SQL Server, Backend lẫn trình duyệt của người dùng sẽ bị treo cứng.
                • Công thức Phân trang bất biến trong mọi dự án .NET:
                Luôn nhận vào 2 tham số từ Client: pageIndex (Trang số mấy, bắt đầu từ 1) và pageSize (Số bản ghi trên 1 trang, VD: 10 hoặc 20):
                1. Bước 1: Tính tổng số bản ghi thỏa mãn điều kiện lọc bằng int totalRecords = await query.CountAsync(); (để Frontend biết có tổng cộng bao nhiêu trang).
                2. Bước 2: Dùng công thức vàng:
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                - Giải thích: Nếu mỗi trang có 10 món (pageSize = 10):
                + Trang 1 (pageIndex = 1): .Skip((1 - 1) * 10) = Bỏ qua 0 món -> .Take(10) = Lấy 10 món đầu tiên (từ 1 đến 10).
                + Trang 2 (pageIndex = 2): .Skip((2 - 1) * 10) = Bỏ qua 10 món đầu -> .Take(10) = Lấy 10 món tiếp theo (từ 11 đến 20).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đếm tổng số bản ghi sau khi lọc (CountAsync)",
                        "Phải đếm tổng số dòng TRƯỚC khi cắt trang để trả về tổng số trang cho Frontend vẽ nút phân trang.",
                        "var query = _db.Products.Where(p => p.IsActive);\nint totalCount = await query.CountAsync();"),
                    Step(2,
                        "Sắp xếp và Cắt trang bằng Skip & Take",
                        "Luôn phải có OrderBy trước khi gọi Skip/Take để thứ tự dữ liệu giữa các trang không bị nhảy lộn xộn.",
                        "var items = await query\n    .OrderByDescending(p => p.CreatedAt)\n    .Skip((pageIndex - 1) * pageSize)\n    .Take(pageSize)\n    .ToListAsync();"),
                    Step(3,
                        "Đóng gói vào PagedResult trả về Client",
                        "Trả về cả danh sách của trang hiện tại lẫn tổng số bản ghi.",
                        "return new PagedResult<ProductDto>(items, totalCount, pageIndex, pageSize);")
                },
                "Với pageSize = 15 (mỗi trang hiển thị 15 sản phẩm), nếu người dùng bấm xem Trang số 3 (pageIndex = 3), công thức .Skip((pageIndex - 1) * pageSize).Take(pageSize) sẽ bỏ qua bao nhiêu bản ghi đầu và lấy ra bao nhiêu bản ghi?",
                new List<string>
                {
                    "A. Bỏ qua 30 bản ghi đầu (của trang 1 và trang 2) và lấy ra 15 bản ghi tiếp theo (từ thứ 31 đến 45)",
                    "B. Bỏ qua 15 bản ghi và lấy 3 bản ghi",
                    "C. Bỏ qua 45 bản ghi và lấy 15 bản ghi",
                    "D. Lấy toàn bộ 45 bản ghi từ đầu"
                },
                "A",
                "(3 - 1) * 15 = 30. Hệ thống sẽ Skip(30) bản ghi của 2 trang đầu tiên và Take(15) bản ghi của trang thứ 3."
            ),

            // =====================================================================
            // BÀI 3.7
            // =====================================================================
            Build(
                topicId,
                "3.7",
                "Đọc luồng Thêm, Sửa, Xóa (CRUD) và cơ chế Change Tracker (Theo dõi thay đổi) của EF Core",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/change-tracking/",
                """
                • Điều kỳ diệu khi đọc code Cập nhật (Update) trong EF Core:
                Khi đọc hàm sửa thông tin sản phẩm ở công ty, bạn sẽ thấy người ta chỉ viết:
                var product = await _db.Products.FindAsync(id);
                product.Name = dto.NewName;
                await _db.SaveChangesAsync();
                (Ủa, tại sao không hề gọi lệnh _db.Products.Update(product) mà khi SaveChangesAsync() nó vẫn tự động cập nhật vào SQL Server?)
                • Bản chất của Change Tracker (Bộ theo dõi thay đổi):
                - Mặc định, khi bạn lấy một Entity từ _db lên, EF Core sẽ gắn một "camera giám sát" (Change Tracker) đi theo đối tượng đó với trạng thái Unchanged.
                - Ngay khi bạn gán product.Name = dto.NewName;, camera phát hiện thuộc tính Name đã bị thay đổi và tự động chuyển trạng thái sang Modified.
                - Khi gọi await _db.SaveChangesAsync(), EF Core quét tất cả các đối tượng đang có trạng thái:
                + Added (do gọi .Add()) -> Sinh lệnh INSERT INTO.
                + Modified (do sửa thuộc tính) -> Sinh lệnh UPDATE ... SET Name = ....
                + Deleted (do gọi .Remove()) -> Sinh lệnh DELETE FROM.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Truy vấn đối tượng (Bắt đầu Tracking)",
                        "Lấy bản ghi từ DB lên. Lúc này EF Core âm thầm chụp lại ảnh gốc (Snapshot) của đối tượng.",
                        "var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);"),
                    Step(2,
                        "Thay đổi giá trị thuộc tính",
                        "Gán giá trị mới từ DTO sang Entity. Change Tracker lập tức đánh dấu cột Price và Name là 'Modified'.",
                        "product.Name = dto.Name;\nproduct.Price = dto.Price;"),
                    Step(3,
                        "Gọi SaveChangesAsync() (Chốt lưu vào DB)",
                        "Nếu quên dòng này, mọi thay đổi ở Bước 2 chỉ nằm trên RAM và biến mất khi kết thúc hàm!",
                        "int affectedRows = await _db.SaveChangesAsync();"),
                    Step(4,
                        "SQL UPDATE tự động thực thi",
                        "EF Core chỉ sinh lệnh UPDATE cho đúng những cột bị thay đổi trong một Transaction an toàn.",
                        "UPDATE [Products] SET [Name] = @p0, [Price] = @p1\nWHERE [Id] = @p2;")
                },
                "Một bạn thực tập sinh viết hàm đổi mật khẩu như sau: lấy user từ _db.Users.FindAsync(id) lên, gán user.PasswordHash = newHash; rồi lập tức return Ok(\"Đổi thành công\"); nhưng khi kiểm tra trong SQL Server thì mật khẩu vẫn y nguyên như cũ. Bạn ấy đã quên mắt xích quan trọng nào?",
                new List<string>
                {
                    "A. Quên gọi lệnh await _db.SaveChangesAsync() trước khi return",
                    "B. Quên khởi động lại máy tính",
                    "C. Quên viết câu lệnh SELECT",
                    "D. Quên xóa user cũ đi"
                },
                "A",
                "Mọi thao tác thêm (.Add), sửa thuộc tính, hay xóa (.Remove) chỉ làm thay đổi trạng thái trên bộ nhớ RAM (Change Tracker). Bắt buộc phải gọi await _db.SaveChangesAsync() thì EF Core mới phát sinh lệnh SQL gửi xuống Database."
            ),

            // =====================================================================
            // BÀI 3.8
            // =====================================================================
            Build(
                topicId,
                "3.8",
                "Tối ưu tốc độ đọc với .AsNoTracking() - Vũ khí tăng tốc API GET mà mọi Senior đều dùng",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/querying/tracking#no-tracking-queries",
                """
                • Khi nào "Camera giám sát" (Change Tracker) trở thành gánh nặng?
                Ở Bài 3.7, bạn biết EF Core luôn gắn bộ theo dõi vào từng bản ghi lấy lên để phục vụ việc Update. Nhưng với các API chỉ lấy dữ liệu ra để xem (HTTP GET danh sách sản phẩm, xem báo cáo) — bạn lấy 1.000 dòng lên chỉ để trả về JSON cho Client rồi vứt đi chứ không hề sửa đổi gì cả! Việc bắt EF Core tạo 1.000 cái camera giám sát cho 1.000 dòng đó rất tốn RAM và làm chậm API.
                • Giải pháp .AsNoTracking():
                - Khi viết câu truy vấn chỉ để đọc (Read-only), các lập trình viên luôn chèn thêm .AsNoTracking() ngay sau tên bảng:
                _db.Products.AsNoTracking().Where(...).ToListAsync();
                - Nó bảo EF Core: "Chỉ lấy dữ liệu lên thôi, tắt bộ theo dõi thay đổi đi!" -> Giúp câu truy vấn chạy nhanh hơn rõ rệt và tiết kiệm bộ nhớ máy chủ.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Bật chế độ Không theo dõi (.AsNoTracking)",
                        "Gắn ngay đầu chuỗi truy vấn trong các hàm GET danh sách hoặc GET chi tiết.",
                        "var products = await _db.Products\n    .AsNoTracking()\n    .Where(p => p.CategoryId == catId)\n    .ToListAsync();"),
                    Step(2,
                        "EF Core bỏ qua bước tạo Snapshot",
                        "Dữ liệu từ SQL Server đọc thẳng ra đối tượng C# mà không lưu bản sao vào bộ nhớ ChangeTracker.",
                        "// Tiết kiệm 30% - 40% bộ nhớ RAM và thời gian CPU xử lý"),
                    Step(3,
                        "Lưu ý khi KHÔNG ĐƯỢC dùng AsNoTracking",
                        "Nếu hàm đó lấy bản ghi lên để Sửa (Update) rồi gọi SaveChangesAsync(), thì KHÔNG được viết AsNoTracking!",
                        "// CẢNH BÁO: Nếu có AsNoTracking(), bạn sửa product.Price = 100 rồi gọi SaveChangesAsync() sẽ KHÔNG có gì được lưu vào DB!")
                },
                "Trong trường hợp nào dưới đây bạn KHÔNG NÊN sử dụng .AsNoTracking()?",
                new List<string>
                {
                    "A. Khi viết API lấy danh sách sản phẩm hiển thị lên trang chủ",
                    "B. Khi viết API xuất báo cáo thống kê doanh thu",
                    "C. Khi lấy một bản ghi từ Database lên với mục đích thay đổi giá trị thuộc tính của nó rồi gọi SaveChangesAsync() để lưu lại",
                    "D. Khi viết API tìm kiếm bài viết theo từ khóa"
                },
                "C",
                "Vì .AsNoTracking() tắt bộ theo dõi thay đổi (Change Tracker), nên nếu bạn sửa thuộc tính của đối tượng đó rồi gọi SaveChangesAsync(), EF Core sẽ không biết đối tượng đã bị sửa và không sinh lệnh UPDATE."
            ),

            // =====================================================================
            // BÀI 3.9
            // =====================================================================
            Build(
                topicId,
                "3.9",
                "Đọc hiểu Cấu hình Bảng & Quan hệ (Fluent API trong OnModelCreating vs Data Annotations)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/modeling/",
                """
                • Làm sao biết bảng này nối với bảng kia bằng khóa ngoại nào, cột nào là bắt buộc, cột nào đánh Index?
                Khi mở một dự án dùng EF Core, có 2 nơi quy định cấu trúc bảng SQL mà bạn cần biết cách đọc:
                1. Cách 1 - Data Annotations (Viết ngay trên đầu thuộc tính của Entity): Dùng các thẻ vuông như [Key], [Required], [MaxLength(200)], [Column(TypeName = "decimal(18,2)")], [ForeignKey("CategoryId")].
                2. Cách 2 - Fluent API (Viết trong hàm OnModelCreating của DbContext hoặc thư mục Configurations): Đây là cách các dự án lớn ưa chuộng nhất vì giữ cho class Entity sạch sẽ.
                • Cách đọc 4 dòng Fluent API phổ biến nhất:
                - entity.HasKey(e => e.Id); -> Đặt cột Id làm Khóa chính (Primary Key).
                - entity.Property(e => e.Name).IsRequired().HasMaxLength(150); -> Cột Name là NVARCHAR(150) NOT NULL.
                - entity.HasIndex(e => e.Email).IsUnique(); -> Đánh chỉ mục tăng tốc tìm kiếm và cấm trùng lặp Email dưới mức Database.
                - entity.HasOne(p => p.Category).WithMany(c => c.Products).HasForeignKey(p => p.CategoryId); -> Thiết lập quan hệ 1 - Nhiều: 1 Sản phẩm thuộc về 1 Danh mục (HasOne), 1 Danh mục có nhiều Sản phẩm (WithMany), nối qua khóa ngoại CategoryId.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Đọc thuộc tính điều hướng trong Entity",
                        "Trong class Product có biến CategoryId (khóa ngoại) và biến Category (đối tượng liên kết).",
                        "public class Product {\n    public int Id { get; set; }\n    public int CategoryId { get; set; }\n    public Category Category { get; set; } // Navigation Property\n}"),
                    Step(2,
                        "Đọc cấu hình Fluent API trong OnModelCreating",
                        "Xác định quy tắc ràng buộc của các cột và mối quan hệ giữa các bảng.",
                        "modelBuilder.Entity<Product>(entity => {\n    entity.Property(p => p.Price).HasColumnType(\"decimal(18,2)\");\n    entity.HasOne(p => p.Category)\n          .WithMany(c => c.Products)\n          .HasForeignKey(p => p.CategoryId)\n          .OnDelete(DeleteBehavior.Restrict);\n});"),
                    Step(3,
                        "Hiểu tác động khi chạy Migration",
                        "Các dòng cấu hình trên chính là bản thiết kế để lệnh Add-Migration sinh ra file tạo bảng SQL chuẩn xác.",
                        "// DeleteBehavior.Restrict: Ngăn không cho xóa Danh mục nếu bên trong vẫn đang có Sản phẩm!")
                },
                "Khi đọc file cấu hình Fluent API của bảng Orders, bạn thấy dòng: builder.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);. Dòng này thể hiện mối quan hệ gì trong Cơ sở dữ liệu?",
                new List<string>
                {
                    "A. Quan hệ 1 - 1 (Mỗi User chỉ được mua duy nhất 1 Đơn hàng)",
                    "B. Quan hệ 1 - Nhiều (Một Đơn hàng thuộc về 1 User, và một User có thể có nhiều Đơn hàng, liên kết qua khóa ngoại UserId)",
                    "C. Xóa bảng Users khỏi Database",
                    "D. Tự động gộp 2 bảng làm một"
                },
                "B",
                "HasOne(o => o.User) kết hợp với WithMany(u => u.Orders) là cú pháp chuẩn của Fluent API để định nghĩa mối quan hệ Một - Nhiều (One-to-Many) giữa bảng Users và bảng Orders."
            ),

            // =====================================================================
            // BÀI 3.10
            // =====================================================================
            Build(
                topicId,
                "3.10",
                "Đọc luồng Giao dịch an toàn (Database Transaction) & Quản lý Migration trong dự án nhóm",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/saving/transactions",
                """
                • 1. Khi nào cần đọc/dùng Database Transaction (BeginTransactionAsync)?
                - Giả sử trong hàm Đặt hàng (Checkout) phải làm 3 việc: (1) Lưu bảng Orders, (2) Lưu chi tiết vào bảng OrderDetails, (3) Trừ số lượng tồn kho trong bảng Products.
                - Nếu bước (1) và (2) đã lưu xong, nhưng đến bước (3) bị lỗi mất điện hoặc sập code -> Khách bị tạo đơn mà kho không bị trừ!
                - Giải pháp Transaction: Bọc cả 3 bước vào using var transaction = await _db.Database.BeginTransactionAsync();. Nếu cả 3 bước trót lọt mới gọi await transaction.CommitAsync() (Lưu chính thức). Nếu bất kỳ bước nào lỗi, nhảy vào catch gọi await transaction.RollbackAsync() (Hoàn tác sạch sẽ như chưa từng có chuyện gì xảy ra!).
                • 2. Quy tắc sống còn với Migrations khi đi thực tập:
                - Thư mục Migrations/ chứa lịch sử thay đổi cấu trúc bảng của cả nhóm.
                - Khi kéo code mới của đồng đội từ Git về (git pull), nếu thấy có file migration mới, bạn chỉ cần gõ dotnet ef database update để cập nhật bảng vào máy mình. Tuyệt đối không tự ý xóa file Migration cũ đã đẩy lên Git của nhóm!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Mở phiên Giao dịch (BeginTransaction)",
                        "Bắt đầu gom các thao tác thay đổi dữ liệu vào một rọ an toàn.",
                        "using var transaction = await _db.Database.BeginTransactionAsync();\ntry {"),
                    Step(2,
                        "Thực thi chuỗi thao tác liên hoàn",
                        "Tạo đơn hàng và trừ tồn kho sản phẩm.",
                        "    _db.Orders.Add(newOrder);\n    await _db.SaveChangesAsync();\n\n    product.Stock -= quantity;\n    await _db.SaveChangesAsync();"),
                    Step(3,
                        "Chốt giao dịch (Commit) nếu thành công",
                        "Tất cả các bước đều không lỗi -> Chính thức ghi nhận dữ liệu vào ổ cứng SQL Server.",
                        "    await transaction.CommitAsync();\n}"),
                    Step(4,
                        "Hoàn tác (Rollback) nếu có bất kỳ lỗi nào",
                        "Nếu có lỗi xảy ra ở bất kỳ dòng nào trong khối try, hủy bỏ toàn bộ thay đổi trước đó để bảo toàn dữ liệu.",
                        "catch (Exception) {\n    await transaction.RollbackAsync();\n    throw;\n}")
                },
                "Trong một hàm chuyển tiền ngân hàng (Trừ tiền tài khoản A và Cộng tiền tài khoản B), nếu sau khi đã trừ tiền tài khoản A (SaveChangesAsync lần 1) mà đến bước cộng tiền tài khoản B bị văng lỗi Exception, lệnh await transaction.RollbackAsync() trong khối catch sẽ làm gì?",
                new List<string>
                {
                    "A. Khôi phục lại số tiền vừa bị trừ của tài khoản A về nguyên trạng ban đầu, đảm bảo không bị mất tiền oan",
                    "B. Vẫn trừ tiền của tài khoản A nhưng khóa tài khoản B",
                    "C. Xóa sạch toàn bộ bảng tài khoản trong Database",
                    "D. Chỉ gửi thông báo lỗi chứ không khôi phục được dữ liệu đã SaveChanges"
                },
                "A",
                "Đó chính là tính chất Atomicity (Tính nguyên tử: Tất cả cùng thành công hoặc không gì cả) của Database Transaction. Mọi lệnh SaveChangesAsync() nằm trong phạm vi một Transaction đều sẽ được hoàn tác sạch sẽ khi gọi RollbackAsync()."
            )
        };
    }

    /// <summary>
    /// Thay thế 2 bài tạm bợ và nạp/cập nhật đầy đủ 10 bài học con (3.1 -> 3.10) cho Chủ đề 3 (Menu 1: INTERN_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var internTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "INTERN_TRACK");

        if (internTrack == null)
            return;

        var topic3 = internTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 3);
        if (topic3 == null)
            return;

        topic3.Title = "Chủ đề 3 (Thực tập): Đọc luồng Truy vấn Dữ liệu (LINQ, SQL & Entity Framework Core)";
        topic3.Description = "10 Bài học con chuyên sâu đọc hiểu Lambda, FirstOrDefaultAsync, IQueryable vs IEnumerable, Include/Select, Pagination, Change Tracker, AsNoTracking, Fluent API & Transaction.";

        var targetLessons = GetLessons(topic3.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic3.Id)
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
