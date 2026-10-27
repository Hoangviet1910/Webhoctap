using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic1Seed
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
            // BÀI 1.1
            // =====================================================================
            Build(
                topicId,
                "1.1",
                "Kiến trúc .NET (CLR, BCL) & Cấu trúc một chương trình C#",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/tour-of-csharp/overview",
                """
                • 1. Hiểu đơn giản .NET và C# là gì?
                  - C# (C-Sharp): Là ngôn ngữ lập trình (giống như tiếng Việt, tiếng Anh) mà bạn dùng để viết ra các dòng lệnh chỉ dẫn cho máy tính.
                  - .NET (Đọc là Dot-Net): Là cả một "nhà máy sản xuất" của Microsoft. Nhà máy này cung cấp sẵn máy móc để dịch code C# ra ngôn ngữ máy tính hiểu được, và cung cấp sẵn hàng nghìn công cụ (đọc file, kết nối Database, làm web) để bạn lấy ra dùng ngay mà không phải tự chế tạo từ đầu.
                • 2. Hai bộ phận cốt lõi bên trong nhà máy .NET:
                  - BCL (Base Class Library - Kho thư viện có sẵn): Chứa các đoạn code do kỹ sư Microsoft viết sẵn (ví dụ: System.Console để in chữ ra màn hình, System.Math để tính toán, System.IO để đọc ghi file). Khi cần dùng kho nào, bạn khai báo từ khóa using ở đầu file.
                  - CLR (Common Language Runtime - Máy ảo chạy chương trình): Khi bạn bấm chạy (Run), code C# chưa biến thẳng thành mã máy ngay mà được dịch sang mã trung gian (IL). Lúc này CLR sẽ đứng ra dịch mã đó sang mã máy (0 và 1) để CPU chạy, đồng thời tự động quản lý bộ nhớ và dọn rác (Garbage Collector) giúp chương trình không bị tràn RAM.
                • 3. Cấu trúc 3 tầng của một file code C#:
                  - Namespace (Khu phố): Gom nhóm các Class có liên quan vào một tên chung để không bị trùng tên.
                  - Class (Ngôi nhà): Mọi đoạn code trong C# đều phải nằm bên trong một class.
                  - Main() (Cánh cửa chính): Là hàm đầu tiên được máy tính tìm đến và chạy khi bạn bấm nút khởi động chương trình.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo thư viện (using)",
                        "Mượn các hộp công cụ có sẵn trong kho BCL của Microsoft để sử dụng trong file hiện tại.",
                        "using System;\nusing System.Collections.Generic;"),
                    Step(2,
                        "2. Định nghĩa Namespace & Class",
                        "Đặt tên khu vực quản lý (Namespace) và tạo ngôi nhà chứa code (Class Program).",
                        "namespace HoangVietApp\n{\n    internal class Program\n    {\n        // Code nằm ở trong này\n    }\n}"),
                    Step(3,
                        "3. Điểm bắt đầu hàm Main()",
                        "Khi bấm F5, CLR sẽ tìm đúng hàm static void Main() để bắt đầu chạy từ dòng đầu tiên.",
                        "static void Main(string[] args)\n{\n    Console.WriteLine(\"Xin chào Hoàng Việt!\");\n}"),
                    Step(4,
                        "4. Biên dịch & Thực thi qua CLR",
                        "Trình biên dịch Roslyn kiểm tra lỗi cú pháp -> Dịch ra file .dll -> CLR nạp vào RAM và chạy ra kết quả trên màn hình.",
                        "// Kết quả in ra màn hình Console:\n// Xin chào Hoàng Việt!")
                },
                "Trong kiến trúc của nền tảng .NET, thành phần CLR (Common Language Runtime) đóng vai trò gì khi bạn bấm chạy chương trình C#?",
                new List<string>
                {
                    "A. Là phần mềm dùng để thiết kế hình ảnh logo cho trang web",
                    "B. Là môi trường thực thi cốt lõi giúp dịch mã trung gian (IL) sang mã máy để CPU chạy, đồng thời quản lý bộ nhớ và tự động thu gom rác (Garbage Collection)",
                    "C. Là hệ quản trị cơ sở dữ liệu thay thế cho SQL Server",
                    "D. Là trình duyệt web dùng để mở trang HTML"
                },
                "B",
                "CLR là trái tim của .NET Runtime: chịu trách nhiệm biên dịch thực thi (JIT Compilation), cấp phát bộ nhớ, bắt lỗi ngoại lệ và tự động dọn dẹp vùng nhớ không còn sử dụng."
            ),

            // =====================================================================
            // BÀI 1.2
            // =====================================================================
            Build(
                topicId,
                "1.2",
                "Biến, Hằng số (const, readonly), từ khóa var và các Kiểu dữ liệu cơ bản",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/built-in-types",
                """
                • 1. Biến (Variable) là gì?
                  - Hãy tưởng tượng bộ nhớ RAM của máy tính như một tủ chứa đồ có hàng triệu ngăn kéo. Biến chính là một cái ngăn kéo đã được dán nhãn tên để bạn cất dữ liệu vào đó và lấy ra dùng khi cần.
                  - Cú pháp khai báo: <Kiểu_dữ_liệu> <Tên_biến> = <Giá_trị>; (Ví dụ: int age = 21;).
                • 2. Bảng các kiểu dữ liệu cơ bản hay dùng nhất trong C#:
                  - Số nguyên (Không có phần lẻ): int (lưu số từ -2 tỷ đến +2 tỷ, dùng nhiều nhất), long (lưu số cực lớn).
                  - Số thực (Có phần thập phân):
                    + double: Dùng cho tính toán khoa học, tọa độ (VD: double score = 8.5;).
                    + decimal: Bắt buộc dùng cho Tiền tệ, Tài chính vì độ chính xác tuyệt đối, khi gán phải có chữ m ở cuối (VD: decimal price = 150000.5m;).
                  - Ký tự & Chuỗi văn bản: char (1 ký tự đơn nằm trong nháy đơn 'A'), string (chuỗi văn bản nằm trong nháy kép "Hoàng Việt").
                  - Đúng / Sai (Logic): bool (chỉ nhận 2 giá trị true hoặc false).
                • 3. Từ khóa var và Hằng số (const):
                  - var (Tự suy luận kiểu): Thay vì viết rõ string name = "Việt";, bạn viết var name = "Việt";. Trình biên dịch nhìn vế phải là chuỗi nên tự hiểu name có kiểu string. (Lưu ý: Đã là string thì dòng dưới không thể gán số vào được).
                  - const (Hằng số cố định): Giá trị không bao giờ được phép thay đổi trong suốt chương trình (VD: const double PI = 3.14;).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khai báo Biến tường minh",
                        "Chỉ định rõ kiểu dữ liệu ở đầu, đặt tên biến theo chuẩn camelCase (chữ đầu viết thường, các chữ sau viết hoa chữ cái đầu).",
                        "int studentAge = 21;\ndecimal tuitionFee = 12500000m;\nstring fullName = \"Nguyễn Hoàng Việt\";\nbool isPassed = true;"),
                    Step(2,
                        "Khai báo nhanh với từ khóa var",
                        "Trình biên dịch tự động nhìn giá trị bên phải dấu bằng (=) để quyết định kiểu dữ liệu lúc biên dịch.",
                        "var totalOrders = 100; // Tự hiểu là kiểu int\nvar message = \"Thành công\"; // Tự hiểu là kiểu string"),
                    Step(3,
                        "Khóa cứng giá trị bằng Hằng số (const)",
                        "Dùng cho các con số quy định cố định. Nếu cố tình gán lại giá trị mới ở dòng dưới sẽ báo lỗi đỏ ngay lập tức.",
                        "const int MAX_LOGIN_ATTEMPTS = 5;\n// MAX_LOGIN_ATTEMPTS = 10; -> LỖI BIÊN DỊCH!")
                },
                "Khi khai báo biến lưu trữ giá tiền sản phẩm bằng kiểu decimal trong C#, cú pháp nào dưới đây là ĐÚNG CHUẨN?",
                new List<string>
                {
                    "A. decimal price = 99000.50m; (Có hậu tố m hoặc M ở cuối con số)",
                    "B. decimal price = \"99000.50\";",
                    "C. decimal price = true;",
                    "D. Không cần hậu tố, viết số thập phân bất kỳ C# đều tự hiểu là decimal"
                },
                "A",
                "Trong C#, một số thập phân viết bình thường (như 99.5) mặc định được hiểu là kiểu double. Để gán vào biến decimal, bạn phải thêm hậu tố m (hoặc M - viết tắt của Monetary/Tiền tệ) ở cuối số đó."
            ),

            // =====================================================================
            // BÀI 1.3
            // =====================================================================
            Build(
                topicId,
                "1.3",
                "Bản chất bộ nhớ Stack và Heap (Value Types vs Reference Types)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/value-types",
                """
                • 1. Tại sao phải hiểu Stack và Heap?
                  - Đây là kiến thức phân loại sinh viên nắm vững bản chất và người chỉ học vẹt. Bộ nhớ RAM khi chạy chương trình C# được chia làm 2 khu vực chính: Stack (Ngăn xếp) và Heap (Đống).
                • 2. Nhóm 1 - Kiểu Tham Trị (Value Types) -> Nằm trên vùng nhớ STACK:
                  - Bao gồm: Các kiểu số (int, double, decimal), bool, char, struct, enum.
                  - Cách hoạt động: Ngăn kéo biến trên Stack chứa trực tiếp giá trị thật bên trong nó (VD: biến int a = 10; thì ngăn kéo a chứa thẳng con số 10).
                  - Khi copy (int b = a;): Máy tính photo con số 10 sang một ngăn kéo b độc lập hoàn toàn. Lúc này sửa b = 99 thì a vẫn bằng 10 (không ảnh hưởng gì tới nhau).
                • 3. Nhóm 2 - Kiểu Tham Chiếu (Reference Types) -> Nằm trên vùng nhớ HEAP:
                  - Bao gồm: class, object, interface, array (mảng), string.
                  - Cách hoạt động: Vì đối tượng Class rất to lớn, dữ liệu thật được cất ở kho rộng lớn tên là Heap. Còn cái tên biến trên Stack chỉ lưu tờ giấy ghi địa chỉ (tọa độ) trỏ tới kho Heap đó!
                  - Khi copy (Student sv2 = sv1;): Nó chỉ photo tờ giấy địa chỉ đưa cho sv2. Nghĩa là cả sv1 và sv2 đang cùng nhìn vào 1 ngôi nhà duy nhất trên Heap. Nếu bạn đổi tên sv2.Name = "Bình", thì khi in sv1.Name ra nó cũng bị đổi thành "Bình" luôn!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Value Type (int, bool, struct) trên Stack",
                        "Mỗi biến giữ một bản sao giá trị riêng biệt. Sửa biến này KHÔNG làm thay đổi biến kia.",
                        "int a = 10;\nint b = a; // Copy giá trị 10 sang b\nb = 50;\n// Kết quả: a vẫn bằng 10, b bằng 50"),
                    Step(2,
                        "Reference Type (Class, Array) trên Heap",
                        "Từ khóa 'new' tạo đối tượng trên Heap và trả về địa chỉ bộ nhớ cho biến trên Stack nắm giữ.",
                        "var user1 = new User { Name = \"Việt\" }; // Đối tượng nằm ở địa chỉ 0x99 trên Heap"),
                    Step(3,
                        "Hiệu ứng dùng chung địa chỉ (Reference Copy)",
                        "Khi gán user2 = user1, cả hai biến cùng trỏ vào địa chỉ 0x99. Sửa qua user2 sẽ làm user1 thay đổi theo!",
                        "var user2 = user1; // Cùng trỏ vào 0x99\nuser2.Name = \"Hoàng\";\n// Kết quả: user1.Name cũng bị đổi thành \"Hoàng\"!")
                },
                "Cho class Product có thuộc tính Price. Chạy đoạn code sau: var p1 = new Product { Price = 100 }; var p2 = p1; p2.Price = 500;. Hỏi lúc này giá trị p1.Price bằng bao nhiêu và vì sao?",
                new List<string>
                {
                    "A. Bằng 100 vì p1 và p2 là hai biến khác nhau",
                    "B. Bằng 500 vì Class là kiểu tham chiếu (Reference Type), khi gán p2 = p1 thì cả hai biến cùng trỏ tới một vùng nhớ đối tượng duy nhất trên Heap",
                    "C. Bằng 600 vì cộng dồn hai giá trị",
                    "D. Báo lỗi biên dịch"
                },
                "B",
                "Với kiểu tham chiếu (class), phép gán p2 = p1 chỉ sao chép địa chỉ tham chiếu chứ không tạo ra đối tượng mới trên Heap. Do đó mọi thay đổi thông qua p2 đều tác động trực tiếp lên đối tượng mà p1 đang trỏ tới."
            ),

            // =====================================================================
            // BÀI 1.4
            // =====================================================================
            Build(
                topicId,
                "1.4",
                "Kiểu dữ liệu Nullable (?) và cách phòng chống lỗi NullReferenceException",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/nullable-value-types",
                """
                • 1. Giá trị null nghĩa là gì?
                  - null nghĩa là "Trống rỗng / Chưa có dữ liệu / Không trỏ tới đâu cả". (Lưu ý: null hoàn toàn khác với số 0 và khác với chuỗi rỗng "").
                • 2. Tại sao cần dấu hỏi chấm ? (Nullable Value Types)?
                  - Mặc định, các kiểu Tham trị (int, double, bool, DateTime) bắt buộc phải có giá trị, không bao giờ được gán bằng null (viết int age = null; sẽ bị báo lỗi đỏ ngay).
                  - Nhưng trong thực tế (và trong bảng SQL Server), có những thông tin người dùng chưa điền như Ngày sinh, Điểm thi, Ngày xuất hàng. Để cho phép các kiểu này nhận giá trị null, bạn chỉ cần thêm dấu ? ngay sau tên kiểu: int?, double?, DateTime?, bool?.
                • 3. Các công cụ làm việc an toàn với biến Nullable:
                  - .HasValue: Trả về true nếu biến có dữ liệu thật, trả về false nếu đang bị null.
                  - .Value: Lấy giá trị thật ra (nhưng nếu biến đang null mà cố gọi .Value sẽ sập lỗi).
                  - Toán tử ?? (Null-coalescing): int finalScore = examScore ?? 0; (Nếu examScore bị null thì lấy số 0 thay thế).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khai báo biến cho phép rỗng với dấu ?",
                        "Thêm dấu ? sau kiểu dữ liệu để biến có thể lưu được cả giá trị bình thường lẫn giá trị null.",
                        "int? discountPercent = null;\nDateTime? GraduationDate = null;"),
                    Step(2,
                        "Kiểm tra trước khi dùng bằng .HasValue",
                        "Đảm bảo biến có dữ liệu thật rồi mới lấy ra tính toán.",
                        "if (discountPercent.HasValue) {\n    Console.WriteLine($\"Giảm giá: {discountPercent.Value}%\");\n} else {\n    Console.WriteLine(\"Không có giảm giá\");\n}"),
                    Step(3,
                        "Rút gọn siêu tốc bằng toán tử ??",
                        "Lấy giá trị nếu có, hoặc tự động thay bằng số 0 nếu đang bị null chỉ trong 1 dòng lệnh.",
                        "int actualDiscount = discountPercent ?? 0;")
                },
                "Trong bảng Orders (Đơn hàng), cột DeliveredDate (Ngày giao hàng thành công) lúc khách mới đặt đơn thì chưa thể có ngày giao (phải để NULL trong Database). Vậy trong class C#, thuộc tính DeliveredDate phải được khai báo kiểu gì?",
                new List<string>
                {
                    "A. public DateTime DeliveredDate { get; set; } (Không có dấu hỏi chấm)",
                    "B. public DateTime? DeliveredDate { get; set; } (Có dấu hỏi chấm ? để cho phép nhận giá trị null)",
                    "C. public int DeliveredDate { get; set; }",
                    "D. public bool DeliveredDate { get; set; }"
                },
                "B",
                "DateTime là một struct (Value Type) nên mặc định không nhận null. Phải khai báo là DateTime? (Nullable<DateTime>) để ánh xạ chính xác với cột cho phép NULL dưới cơ sở dữ liệu."
            ),

            // =====================================================================
            // BÀI 1.5
            // =====================================================================
            Build(
                topicId,
                "1.5",
                "Ép kiểu dữ liệu (Implicit, Explicit, Parse, TryParse và Convert)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/types/casting-and-type-conversions",
                """
                • 1. Khi nào cần Ép kiểu (Type Conversion)?
                  - Khi bạn muốn đổi dữ liệu từ kiểu này sang kiểu khác: ví dụ đổi từ số nhỏ (int) sang số lớn (long), hoặc đổi từ chuỗi người dùng nhập trên bàn phím ("2026") sang con số (int 2026) để cộng trừ nhân chia.
                • 2. Bốn phương pháp chuyển đổi kiểu trong C# từ dễ đến nâng cao:
                  1. Ép kiểu Ngầm định (Implicit Casting - Tự động 100%): Đổ nước từ cốc nhỏ (int) sang bình to (long hoặc double). Vì không bao giờ sợ tràn mất dữ liệu nên C# tự động làm luôn: int a = 10; double b = a;.
                  2. Ép kiểu Tường minh (Explicit Casting - Phải viết (kiểu) ở trước): Đổ nước từ bình to (double) về cốc nhỏ (int). Vì sẽ bị cắt mất phần thập phân, bạn phải viết rõ (int) để xác nhận: double d = 9.8; int i = (int)d; (Kết quả i chỉ còn 9).
                  3. Parse vs TryParse (Đổi từ Chuỗi string sang Số/Ngày tháng):
                     - int.Parse("123"): Nếu chuỗi là "123a" (có chữ cái), chương trình sẽ văng lỗi sập ngay lập tức (FormatException).
                     - int.TryParse("123a", out int result): Cách an toàn nhất! Nó thử đổi, nếu đổi được thì trả về true và nhét số vào biến result; nếu chuỗi sai thì trả về false nhẹ nhàng chứ không bao giờ làm sập chương trình.
                  4. Lớp Convert (Convert.ToInt32, Convert.ToDouble, Convert.ToString): Hỗ trợ đổi qua lại giữa mọi kiểu cơ bản. Điểm đặc biệt: Convert.ToInt32(null) trả về 0 chứ không báo lỗi.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Ép kiểu giữa các kiểu số (Implicit & Explicit)",
                        "Từ nhỏ sang lớn thì tự động; từ lớn về nhỏ hoặc có phần lẻ về số nguyên thì phải mở ngoặc (int).",
                        "int smallNum = 100;\nlong bigNum = smallNum; // Tự động (Implicit)\n\ndouble score = 8.95;\nint rounded = (int)score; // Tường minh (Explicit) -> rounded = 8"),
                    Step(2,
                        "Sự nguy hiểm của hàm Parse()",
                        "Chỉ dùng Parse khi bạn chắc chắn 100% chuỗi đầu vào toàn là con số hợp lệ.",
                        "int valid = int.Parse(\"2026\"); // OK = 2026\n// int crash = int.Parse(\"2026xyz\"); -> SẬP CHƯƠNG TRÌNH!"),
                    Step(3,
                        "Chuẩn mực an toàn với TryParse()",
                        "Kết hợp với if để bắt trường hợp người dùng nhập sai định dạng một cách êm ái.",
                        "string input = \"abc\";\nif (int.TryParse(input, out int age)) {\n    Console.WriteLine($\"Tuổi của bạn: {age}\");\n} else {\n    Console.WriteLine(\"Vui lòng chỉ nhập chữ số!\");\n}")
                },
                "Khi thực hiện câu lệnh ép kiểu tường minh: double gpa = 3.99; int result = (int)gpa;, giá trị của biến result sẽ bằng bao nhiêu?",
                new List<string>
                {
                    "A. Bằng 4 (Tự động làm tròn lên)",
                    "B. Bằng 3 (Cắt bỏ hoàn toàn phần thập phân .99 chứ không làm tròn)",
                    "C. Bằng 3.99",
                    "D. Báo lỗi không chạy được"
                },
                "B",
                "Phép ép kiểu tường minh (int)doubleValue thực hiện cắt cụt phần thập phân (Truncation) chứ không làm tròn toán học. Nếu muốn làm tròn thành 4, bạn phải dùng Math.Round() hoặc Convert.ToInt32(gpa)."
            ),

            // =====================================================================
            // BÀI 1.6
            // =====================================================================
            Build(
                topicId,
                "1.6",
                "Toàn bộ các loại Toán tử trong C# (Số học, So sánh, Logic, Gán và Toán tử 3 ngôi)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/",
                """
                • 1. Toán tử Số học (Arithmetic):
                  - + (Cộng), - (Trừ), * (Nhân), / (Chia), và % (Chia lấy phần dư - Modulo).
                  - Lưu ý cực quan trọng về phép chia / và %:
                    + Nếu lấy 2 số nguyên chia cho nhau (int chia int), kết quả luôn bị cắt bỏ phần lẻ: 7 / 2 ra 3 (chứ không phải 3.5!). Muốn ra 3.5 phải viết 7.0 / 2 hoặc (double)7 / 2.
                    + Phép chia lấy dư %: 7 % 2 dư 1 (Dùng để kiểm tra số chẵn/lẻ: nếu n % 2 == 0 là số chẵn).
                  - Tăng/Giảm 1 đơn vị: x++ (tăng thêm 1), x-- (giảm đi 1).
                • 2. Toán tử So sánh (Luôn trả về bool: true hoặc false):
                  - == (Bằng nhau — chú ý viết 2 dấu bằng), != (Khác nhau), > (Lớn hơn), < (Nhỏ hơn), >= (Lớn hơn hoặc bằng), <= (Nhỏ hơn hoặc bằng).
                • 3. Toán tử Logic (Kết hợp nhiều điều kiện):
                  - && (VÀ - AND): Chỉ đúng khi TẤT CẢ các vế cùng đúng (VD: Đủ 18 tuổi VÀ có bằng lái).
                  - || (HOẶC - OR): Chỉ cần 1 trong các vế đúng là đúng (VD: Thanh toán bằng Tiền mặt HOẶC Chuyển khoản).
                  - ! (PHỦ ĐỊNH - NOT): Đảo ngược đúng thành sai, sai thành đúng (!true -> false).
                • 4. Toán tử 3 ngôi (Điều_kiện ? Giá_trị_nếu_Đúng : Giá_trị_nếu_Sai):
                  - Cách viết rút gọn siêu đẹp của if-else trong 1 dòng duy nhất!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Phép chia nguyên (/) và Chia lấy dư (%)",
                        "Cẩn thận bẫy chia 2 số nguyên trong C# và ứng dụng phép % để kiểm tra chẵn lẻ.",
                        "int thuong = 10 / 3; // Kết quả = 3\nint soDu = 10 % 3;   // Kết quả = 1\nbool isEven = (8 % 2 == 0); // true (số chẵn)"),
                    Step(2,
                        "Kết hợp điều kiện bằng && (Và) / || (Hoặc)",
                        "Kiểm tra đồng thời nhiều điều kiện nghiệp vụ.",
                        "bool canBuy = (age >= 18) && (balance >= 50000);\nbool getsDiscount = isVip || (orderTotal > 1000000);"),
                    Step(3,
                        "Viết gọn với Toán tử 3 ngôi (? :)",
                        "Thay thế 4 dòng if-else bằng 1 dòng gán trực tiếp rõ ràng.",
                        "double diem = 7.5;\nstring ketQua = (diem >= 5.0) ? \"Qua môn\" : \"Học lại\";\n// ketQua = \"Qua môn\"")
                },
                "Trong C#, kết quả của biểu thức int result = 9 / 2; và int mod = 9 % 2; lần lượt là bao nhiêu?",
                new List<string>
                {
                    "A. result = 4.5 và mod = 0",
                    "B. result = 4 (vì phép chia 2 số nguyên int sẽ bỏ phần thập phân) và mod = 1 (vì 9 chia 2 bằng 4 dư 1)",
                    "C. result = 5 và mod = 1",
                    "D. Báo lỗi vì 9 không chia hết cho 2"
                },
                "B",
                "Khi cả tử số và mẫu số đều là kiểu int, phép / thực hiện chia lấy phần nguyên (9 / 2 = 4), còn phép % trả về phần dư của phép chia đó (9 % 2 = 1)."
            ),

            // =====================================================================
            // BÀI 1.7
            // =====================================================================
            Build(
                topicId,
                "1.7",
                "Làm việc chuyên sâu với Chuỗi (string, các hàm xử lý chuỗi, String Interpolation và StringBuilder)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/strings/",
                """
                • 1. Đặc tính "Bất biến" (Immutable) của string trong C#:
                  - Trong C#, một khi chuỗi string đã được tạo ra trên vùng nhớ Heap thì không bao giờ sửa trực tiếp được nội dung tại ô nhớ đó. Mỗi khi bạn cộng chuỗi (str = str + "a"), máy tính thực chất phải tạo ra một chuỗi hoàn toàn mới trên RAM và vứt chuỗi cũ đi!
                • 2. Cú pháp ghép chuỗi hiện đại (String Interpolation - $"..."):
                  - Thay vì dùng dấu cộng rườm rà "Xin chào " + name + ", bạn " + age + " tuổi", hãy đặt dấu $ ở ngay trước dấu nháy kép và nhét thẳng biến vào trong cặp ngoặc nhọn {}:
                    $"Xin chào {name}, bạn {age} tuổi".
                • 3. Các hàm xử lý chuỗi bắt buộc phải thuộc:
                  - .Length: Lấy độ dài (số ký tự) của chuỗi.
                  - .Trim(): Cắt bỏ khoảng trắng thừa ở đầu và cuối chuỗi.
                  - .ToUpper() / .ToLower(): Đổi toàn bộ thành chữ IN HOA / chữ thường.
                  - .Contains("từ_khóa"): Kiểm tra xem trong chuỗi có chứa từ khóa đó không (true/false).
                  - .Replace("cũ", "mới"): Thay thế từ cũ bằng từ mới.
                  - .Split(','): Cắt một chuỗi dài thành mảng các chuỗi con dựa theo dấu phân cách.
                  - string.IsNullOrWhiteSpace(s): Kiểm tra chuỗi có bị null, rỗng "" hoặc toàn dấu cách "   " hay không.
                • 4. Khi nào phải dùng StringBuilder thay cho string?
                  - Khi bạn cần nối chuỗi liên tục hàng nghìn lần bên trong vòng lặp (for/foreach). Dùng StringBuilder (sb.Append(...)) sẽ sửa trực tiếp trên 1 vùng nhớ duy nhất, nhanh gấp hàng trăm lần và không làm tràn RAM!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Chuẩn hóa và Kiểm tra chuỗi",
                        "Xóa khoảng trắng thừa 2 đầu, đổi chữ thường và kiểm tra sự tồn tại của từ khóa.",
                        "string rawEmail = \"  HoangViet@Gmail.com  \";\nstring cleanEmail = rawEmail.Trim().ToLower(); // \"hoangviet@gmail.com\"\nbool isGmail = cleanEmail.Contains(\"@gmail.com\"); // true"),
                    Step(2,
                        "Ghép chuỗi thanh lịch với $\"...\"",
                        "Nhúng trực tiếp biến hoặc biểu thức tính toán vào bên trong cặp ngoặc nhọn {}.",
                        "string product = \"Laptop\";\ndecimal price = 20000000;\nstring info = $\"Sản phẩm: {product} - Giá: {price:N0} VNĐ\";\n// Kết quả: \"Sản phẩm: Laptop - Giá: 20,000,000 VNĐ\""),
                    Step(3,
                        "Tối ưu nối chuỗi trong vòng lặp với StringBuilder",
                        "Sử dụng lớp StringBuilder (trong System.Text) khi phải ghép chuỗi lặp đi lặp lại nhiều lần.",
                        "var sb = new System.Text.StringBuilder();\nfor (int i = 1; i <= 3; i++) {\n    sb.AppendLine($\"Dòng thứ {i}\");\n}\nstring finalReport = sb.ToString();")
                },
                "Tại sao khi cần nối chuỗi bên trong một vòng lặp chạy 10.000 lần, lập trình viên C# luôn dùng StringBuilder thay vì dùng toán tử cộng chuỗi str += \"...\"?",
                new List<string>
                {
                    "A. Vì kiểu string có tính bất biến (Immutable), mỗi lần dùng dấu += sẽ tạo ra một đối tượng chuỗi mới trên Heap gây tốn RAM và giảm hiệu năng nghiêm trọng, còn StringBuilder nối trực tiếp trên một bộ đệm bộ nhớ duy nhất",
                    "B. Vì toán tử += không dùng được trong vòng lặp for",
                    "C. Vì StringBuilder tự động lưu chuỗi vào SQL Server",
                    "D. Vì string chỉ chứa được tối đa 100 ký tự"
                },
                "A",
                "Do tính bất biến (Immutability) của string, việc cộng chuỗi 10.000 lần trong vòng lặp sẽ cấp phát và xả rác 10.000 vùng nhớ trên Heap. StringBuilder giải quyết triệt để vấn đề này bằng cách thay đổi nội dung ngay trên bộ nhớ đệm động."
            ),

            // =====================================================================
            // BÀI 1.8
            // =====================================================================
            Build(
                topicId,
                "1.8",
                "Làm việc với Ngày tháng (DateTime, DateOnly, TimeSpan) và mã định danh duy nhất Guid",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.datetime",
                """
                • 1. Kiểu DateTime (Quản lý Ngày và Giờ):
                  - DateTime.Now: Lấy ngày giờ hiện tại theo múi giờ máy tính đang chạy (VD: giờ Việt Nam GMT+7).
                  - DateTime.UtcNow: Lấy ngày giờ chuẩn quốc tế (GMT+0) — rất hay dùng trong các hệ thống Server toàn cầu.
                  - Cộng/trừ thêm thời gian: Dùng các hàm .AddDays(7) (cộng thêm 7 ngày), .AddHours(2) (cộng 2 tiếng), .AddMonths(1). Muốn lùi về quá khứ thì truyền số âm: .AddDays(-5) (lùi 5 ngày trước).
                  - Định dạng hiển thị ra màn hình (.ToString("format")):
                    + date.ToString("dd/MM/yyyy") -> "19/10/2026" (Chú ý: chữ MM viết hoa là Tháng, còn mm viết thường là Phút!).
                    + date.ToString("dd/MM/yyyy HH:mm:ss") -> "19/10/2026 14:30:05".
                • 2. Kiểu TimeSpan (Khoảng thời gian chênh lệch):
                  - Khi lấy 2 mốc DateTime trừ cho nhau (DateTime end - DateTime start), kết quả trả về là một đối tượng TimeSpan cho biết khoảng cách giữa 2 mốc đó là bao nhiêu ngày (.TotalDays), bao nhiêu giờ (.TotalHours), bao nhiêu phút (.TotalMinutes).
                • 3. Kiểu Guid (Globally Unique Identifier - Mã định danh duy nhất):
                  - Là một chuỗi 32 ký tự thập lục phân (dạng "f81d4fae-7dec-11d0-a765-00a0c91e6bf6").
                  - Dùng lệnh Guid.NewGuid() để sinh ra một mã mới đảm bảo không bao giờ trùng lặp trên toàn thế giới (dùng làm ID đơn hàng, mã giao dịch, đổi tên file ảnh upload).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Lấy thời gian hiện tại & Tính hạn sử dụng",
                        "Dùng DateTime.Now kết hợp .AddDays() để tính ngày hết hạn khóa học hoặc voucher.",
                        "DateTime createdAt = DateTime.Now;\nDateTime expiredAt = createdAt.AddDays(30); // Hết hạn sau 30 ngày\nstring display = expiredAt.ToString(\"dd/MM/yyyy\");"),
                    Step(2,
                        "Tính khoảng cách thời gian với TimeSpan",
                        "Trừ 2 mốc DateTime để biết còn bao nhiêu ngày nữa đến kỳ thực tập.",
                        "DateTime internDate = new DateTime(2026, 11, 25);\nTimeSpan remaining = internDate - DateTime.Now;\nint daysLeft = remaining.Days;"),
                    Step(3,
                        "Sinh mã định danh không trùng lặp với Guid",
                        "Mỗi lần gọi Guid.NewGuid() sẽ tạo ra một mã duy nhất toàn cầu.",
                        "Guid transactionId = Guid.NewGuid();\n// Ví dụ: 9b1deb4d-3b7d-4bad-9bdd-2b0d7b3dcb6d")
                },
                "Khi định dạng ngày tháng bằng hàm dateTime.ToString(\"...\") trong C#, vì sao định dạng Tháng phải viết hoa là \"MM\" (ví dụ \"dd/MM/yyyy\") chứ không được viết thường là \"mm\"?",
                new List<string>
                {
                    "A. Vì trong chuẩn định dạng của .NET, 'MM' (viết hoa) đại diện cho Tháng (Month), còn 'mm' (viết thường) đại diện cho Phút (minute)",
                    "B. Vì viết thường chương trình sẽ bị sập",
                    "C. Vì 'mm' là ký hiệu của đơn vị mili-mét",
                    "D. Hai cách viết cho kết quả giống hệt nhau"
                },
                "A",
                "Đây là lỗi rất nhiều người mới mắc phải: \"MM\" là Month (Tháng từ 01-12), còn \"mm\" là Minute (Phút từ 00-59); tương tự \"HH\" là giờ 24h, còn \"hh\" là giờ 12h."
            ),

            // =====================================================================
            // BÀI 1.9
            // =====================================================================
            Build(
                topicId,
                "1.9",
                "Cấu trúc điều khiển rẽ nhánh (if - else, switch - case và switch expression hiện đại)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/selection-statements",
                """
                • 1. Bản chất của Cấu trúc rẽ nhánh:
                  - Bình thường code chạy tuần tự từng dòng từ trên xuống dưới. Cấu trúc rẽ nhánh giúp chương trình biết suy nghĩ và ra quyết định: "Nếu điều kiện A đúng thì rẽ trái, ngược lại thì rẽ phải".
                • 2. Câu lệnh if - else if - else:
                  - Dùng khi kiểm tra các khoảng giá trị (VD: diem >= 8.0, tuoi < 18) hoặc kết hợp nhiều điều kiện logic phức tạp.
                  - Ngay khi gặp nhánh if hoặc else if đầu tiên thỏa mãn (true), nó sẽ chạy khối lệnh đó và bỏ qua toàn bộ các nhánh bên dưới.
                • 3. Câu lệnh switch - case và switch expression (C# mới):
                  - Dùng khi bạn cần so sánh 1 biến duy nhất với nhiều giá trị cố định cụ thể (VD: Kiểm tra thứ trong tuần, kiểm tra mã trạng thái 1, 2, 3 hoặc Enum).
                  - Cú pháp switch expression hiện đại (Dùng mũi tên =>): Ngắn gọn hơn gấp 3 lần so với switch-case cổ điển, không cần gõ từ khóa case và break lặp đi lặp lại, dùng dấu gạch dưới _ để đại diện cho default.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Phân loại theo khoảng với if - else if - else",
                        "Kiểm tra từ điều kiện cao nhất xuống thấp nhất. Nhánh else cuối cùng bắt mọi trường hợp còn lại.",
                        "double score = 8.2;\nif (score >= 8.5) {\n    Console.WriteLine(\"Giỏi\");\n} else if (score >= 7.0) {\n    Console.WriteLine(\"Khá\"); // Chạy vào nhánh này\n} else {\n    Console.WriteLine(\"Trung bình\");\n}"),
                    Step(2,
                        "Rẽ nhánh cổ điển với switch - case",
                        "Mỗi nhánh case phải kết thúc bằng lệnh break (hoặc return) để không bị trôi xuống nhánh dưới.",
                        "int roleId = 1;\nswitch (roleId) {\n    case 1:\n        Console.WriteLine(\"Quản trị viên\");\n        break;\n    default:\n        Console.WriteLine(\"Thành viên\");\n        break;\n}"),
                    Step(3,
                        "Cú pháp Switch Expression hiện đại (Khuyên dùng)",
                        "Trả về trực tiếp giá trị qua dấu mũi tên => cực kỳ sạch đẹp và chuyên nghiệp.",
                        "string roleName = roleId switch {\n    1 => \"Quản trị viên\",\n    2 => \"Nhân viên\",\n    _ => \"Khách vãng lai\" // Dấu _ thay cho default\n};")
                },
                "Khi nào bạn nên ưu tiên sử dụng switch (hoặc switch expression) thay vì dùng nhiều câu lệnh if - else if nối tiếp nhau?",
                new List<string>
                {
                    "A. Khi cần so sánh một biến (như mã trạng thái, lựa chọn menu, kiểu Enum) với một danh sách các giá trị rời rạc cố định, giúp code gọn gàng và dễ đọc hơn",
                    "B. Khi chỉ có duy nhất 1 điều kiện đúng/sai",
                    "C. Khi cần lặp đi lặp lại một công việc 100 lần",
                    "D. Khi cần kết nối cơ sở dữ liệu"
                },
                "A",
                "switch được thiết kế tối ưu về cả độ rõ ràng lẫn tốc độ nhảy nhánh (jump table) khi kiểm tra một biến với nhiều hằng số hoặc giá trị Enum cụ thể."
            ),

            // =====================================================================
            // BÀI 1.10
            // =====================================================================
            Build(
                topicId,
                "1.10",
                "Các cấu trúc Vòng lặp (for, while, do-while, foreach và từ khóa break, continue)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/iteration-statements",
                """
                • 1. Khi nào dùng loại vòng lặp nào?
                  - for (int i = 0; i < n; i++): Dùng khi bạn biết trước số lần lặp hoặc cần dùng đến vị trí chỉ số i (Index: thứ 0, thứ 1, thứ 2...).
                  - foreach (var item in danh_sách): Dùng nhiều nhất trong C#! Dùng để duyệt qua từng phần tử từ đầu đến cuối của một Mảng hoặc Danh sách (List) mà không cần quan tâm đến biến đếm i.
                  - while (điều_kiện): Kiểm tra điều kiện trước, nếu đúng mới lặp. Dùng khi chưa biết trước sẽ lặp bao nhiêu lần (VD: Đọc từng dòng trong file cho đến khi hết file).
                  - do { ... } while (điều_kiện);: Chạy khối lệnh ít nhất 1 lần trước, sau đó mới kiểm tra điều kiện ở cuối để xem có lặp tiếp hay không (VD: Bắt người dùng nhập mật khẩu, nếu sai thì bắt nhập lại).
                • 2. Hai chiếc phanh điều khiển vòng lặp (break vs continue):
                  - break (Đạp phanh dừng hẳn): Thoát khỏi vòng lặp ngay lập tức, không chạy thêm bất kỳ vòng nào nữa (VD: Tìm thấy sản phẩm cần tìm rồi thì break nghỉ luôn).
                  - continue (Bỏ qua lượt hiện tại): Bỏ qua các dòng lệnh bên dưới của lượt đang chạy để nhảy sang vòng lặp kế tiếp luôn (VD: Duyệt danh sách học sinh, nếu gặp học sinh được miễn thi thì continue bỏ qua để chấm điểm bạn tiếp theo).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Vòng lặp for với biến đếm i",
                        "Gồm 3 phần: Khởi tạo i = 1; Điều kiện tiếp tục i <= 5; Bước nhảy i++ sau mỗi vòng.",
                        "int sum = 0;\nfor (int i = 1; i <= 5; i++) {\n    sum += i; // Cộng dồn 1 + 2 + 3 + 4 + 5 = 15\n}"),
                    Step(2,
                        "Vòng lặp foreach duyệt danh sách",
                        "Tự động lấy lần lượt từng phần tử trong mảng/danh sách gán vào biến 'name'.",
                        "string[] names = { \"An\", \"Bình\", \"Việt\" };\nforeach (string name in names) {\n    Console.WriteLine($\"Xin chào {name}\");\n}"),
                    Step(3,
                        "Điều khiển luồng lặp bằng continue và break",
                        "continue bỏ qua số âm; break dừng hẳn vòng lặp nếu gặp số 99.",
                        "foreach (int n in numbers) {\n    if (n < 0) continue; // Bỏ qua số âm, nhảy sang số kế tiếp\n    if (n == 99) break;   // Gặp 99 thì thoát khỏi vòng lặp ngay\n    Console.WriteLine(n);\n}")
                },
                "Sự khác biệt cốt lõi giữa từ khóa break và continue khi đặt bên trong một vòng lặp là gì?",
                new List<string>
                {
                    "A. break chấm dứt và thoát hoàn toàn khỏi vòng lặp ngay lập tức; còn continue chỉ bỏ qua phần còn lại của lần lặp hiện tại để nhảy sang lần lặp kế tiếp",
                    "B. continue làm dừng hẳn chương trình, còn break chỉ tạm dừng 5 giây",
                    "C. Hai từ khóa hoạt động giống hệt nhau",
                    "D. break chỉ dùng trong vòng lặp while, continue chỉ dùng trong vòng lặp for"
                },
                "A",
                "break phá vỡ vòng lặp để đi ra ngoài, trong khi continue chỉ bỏ qua các câu lệnh phía dưới nó trong lượt hiện tại và tiếp tục vòng lặp ở lượt tiếp theo."
            ),

            // =====================================================================
            // BÀI 1.11
            // =====================================================================
            Build(
                topicId,
                "1.11",
                "Làm việc với Mảng (Array 1 chiều, đa chiều) và Kiểu liệt kê (Enum)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/arrays/",
                """
                • 1. Mảng (Array) là gì?
                  - Thay vì khai báo 50 biến rời rạc diem1, diem2... diem50, Mảng giúp bạn gom các phần tử có cùng kiểu dữ liệu vào chung một dãy ngăn kéo nằm liền kề nhau với kích thước cố định.
                  - Quy tắc đánh số thứ tự (Index): Trong C#, mảng luôn đếm bắt đầu từ vị trí số 0. Nghĩa là mảng có 5 phần tử thì phần tử đầu tiên là arr[0] và phần tử cuối cùng là arr[4] (Length - 1). Nếu cố truy cập arr[5] sẽ văng lỗi IndexOutOfRangeException!
                • 2. Kiểu liệt kê (Enum - Enumeration):
                  - Là một kiểu dữ liệu đặc biệt do bạn tự định nghĩa để đặt tên có ý nghĩa cho một tập hợp các con số nguyên cố định.
                  - Ví dụ: Thay vì quy ước miệng "Số 0 là Mới tạo, số 1 là Đang giao, số 2 là Hoàn thành", bạn tạo enum OrderStatus { New = 0, Shipping = 1, Completed = 2 }. Khi viết code chỉ cần gõ OrderStatus.Shipping vừa tự gợi ý code, vừa không bao giờ sợ nhớ nhầm số!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Khởi tạo và Truy xuất Mảng 1 chiều",
                        "Khởi tạo mảng kèm giá trị và lấy phần tử thông qua chỉ mục [index] bắt đầu từ 0.",
                        "int[] scores = { 8, 9, 10, 7 };\nint first = scores[0]; // Lấy phần tử đầu tiên = 8\nint length = scores.Length; // Độ dài mảng = 4\nscores[3] = 10; // Sửa phần tử cuối cùng từ 7 thành 10"),
                    Step(2,
                        "Khai báo Kiểu liệt kê (Enum)",
                        "Định nghĩa danh sách các hằng số có tên rõ ràng.",
                        "public enum UserRole {\n    Member = 1,\n    Moderator = 2,\n    Admin = 3\n}"),
                    Step(3,
                        "Chuyển đổi qua lại giữa Enum và int / string",
                        "Có thể ép kiểu dễ dàng từ Enum ra số nguyên (để lưu DB) hoặc lấy tên chữ của Enum.",
                        "UserRole myRole = UserRole.Admin;\nint roleCode = (int)myRole; // Kết quả = 3\nstring roleText = myRole.ToString(); // Kết quả = \"Admin\"")
                },
                "Cho mảng string[] fruits = { \"Táo\", \"Cam\", \"Xoài\", \"Đào\" };. Nếu bạn viết lệnh Console.WriteLine(fruits[4]); thì điều gì sẽ xảy ra?",
                new List<string>
                {
                    "A. In ra chữ Đào",
                    "B. Chương trình văng lỗi IndexOutOfRangeException vì mảng có 4 phần tử thì chỉ mục (Index) chỉ đánh số từ 0 đến 3",
                    "C. In ra giá trị null",
                    "D. Tự động thêm phần tử thứ 5 vào mảng"
                },
                "B",
                "Do chỉ mục mảng bắt đầu từ 0 (fruits[0] là \"Táo\", ..., fruits[3] là \"Đào\"), việc truy cập fruits[4] vượt quá giới hạn mảng và gây lỗi IndexOutOfRangeException."
            ),

            // =====================================================================
            // BÀI 1.12
            // =====================================================================
            Build(
                topicId,
                "1.12",
                "Khai báo và sử dụng Phương thức (Method): Tham số, Giá trị trả về, ref, out và params",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/methods",
                """
                • 1. Tại sao phải chia code thành các Phương thức (Method / Hàm)?
                  - Nếu nhồi nhét hàng trăm dòng code vào chung hàm Main(), code sẽ rối như tơ vò và khi cần tính toán lại phải copy-paste nhiều lần. Phương thức (Method) giúp đóng gói một công việc cụ thể thành 1 khối có tên riêng để gọi đi gọi lại nhiều lần (Tái sử dụng code).
                • 2. Cấu trúc của 1 Phương thức:
                  <Phạm_vi> <Kiểu_trả_về> <Tên_Hàm>(<Danh_sách_tham_số>) { ... }
                  - Kiểu trả về (Return Type):
                    + Nếu hàm chỉ thực hiện hành động (như in ra màn hình) mà không trả về kết quả: Dùng từ khóa void.
                    + Nếu hàm tính toán và trả về kết quả: Ghi rõ kiểu dữ liệu (int, double, string, bool...) và bên trong thân hàm bắt buộc phải có lệnh return <kết_quả>;.
                • 3. Ba từ khóa truyền tham số đặc biệt (ref, out, params):
                  - Truyền bình thường (Pass by Value): Hàm chỉ nhận bản photo của biến, sửa bên trong hàm thì biến gốc bên ngoài không đổi.
                  - ref và out (Pass by Reference - Truyền tham chiếu): Cho phép hàm sửa trực tiếp biến gốc ở bên ngoài! Đặc biệt từ khóa out giúp một hàm có thể trả về cùng lúc nhiều kết quả (như hàm int.TryParse(str, out int number) vừa trả về bool, vừa đẩy kết quả số ra qua cổng out).
                  - params: Cho phép truyền vào hàm một số lượng tham số tùy ý (2 số, 5 số hay 10 số đều nhận hết).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Viết hàm có giá trị trả về (return)",
                        "Hàm nhận vào 2 tham số (giá gốc và phần trăm giảm), tính toán và return kết quả.",
                        "public static decimal TinhGiaSauGiam(decimal giaGoc, int phanTram) {\n    decimal soTienGiam = giaGoc * phanTram / 100;\n    return giaGoc - soTienGiam;\n}"),
                    Step(2,
                        "Dùng từ khóa out để trả về nhiều giá trị cùng lúc",
                        "Hàm vừa tính Chu vi vừa tính Diện tích hình chữ nhật và đẩy cả 2 kết quả ra ngoài qua tham số out.",
                        "public static void TinhHinhChuNhat(double dai, double rong, out double chuVi, out double dienTich) {\n    chuVi = (dai + rong) * 2;\n    dienTich = dai * rong;\n}"),
                    Step(3,
                        "Gọi hàm và hứng kết quả trả về",
                        "Truyền đối số vào hàm để lấy kết quả sử dụng.",
                        "decimal giaCuoi = TinhGiaSauGiam(200000m, 15); // 170000\nTinhHinhChuNhat(5, 3, out double cv, out double dt); // cv = 16, dt = 15")
                },
                "Khi khai báo một phương thức có kiểu trả về là public int CalculateSum(int a, int b), từ khóa nào BẮT BUỘC phải có ở bên trong thân phương thức để trả kết quả về cho nơi gọi?",
                new List<string>
                {
                    "A. Từ khóa break",
                    "B. Từ khóa return kèm theo một giá trị kiểu int (ví dụ: return a + b;)",
                    "C. Từ khóa void",
                    "D. Từ khóa continue"
                },
                "B",
                "Mọi phương thức có kiểu trả về khác void đều bắt buộc phải sử dụng câu lệnh return trả về đúng kiểu dữ liệu đã cam kết ở chữ ký hàm."
            ),

            // =====================================================================
            // BÀI 1.13
            // =====================================================================
            Build(
                topicId,
                "1.13",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 1 (Nền tảng C# & .NET)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/tour-of-csharp/",
                """
                • Tổng kết toàn bộ kiến thức cốt lõi của Chủ đề 1:
                  1. Kiến trúc & Cú pháp: Nắm vững vai trò của CLR (máy ảo thực thi, dọn rác GC), BCL (thư viện cơ sở) và cấu trúc Namespace -> Class -> Main().
                  2. Bộ nhớ & Kiểu dữ liệu: Phân biệt rạch ròi Value Types (lưu giá trị trực tiếp trên Stack: int, double, decimal, bool, struct, enum) và Reference Types (lưu địa chỉ trên Stack trỏ tới đối tượng trên Heap: class, string, array).
                  3. An toàn dữ liệu: Luôn dùng decimal cho tiền tệ, dùng ? (Nullable) cho biến có thể rỗng, dùng TryParse khi đổi chuỗi sang số và dùng string.IsNullOrWhiteSpace khi kiểm tra chuỗi.
                  4. Điều khiển luồng & Hàm: Thành thạo if-else, switch expression, for/foreach, break/continue và cách tách hàm (Method) với tham số đầu vào/đầu ra rõ ràng.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Nhận & Làm sạch dữ liệu đầu vào",
                        "Kiểm tra chuỗi rỗng và dùng TryParse ép kiểu an toàn.",
                        "string rawPrice = \" 250000 \";\nif (!decimal.TryParse(rawPrice.Trim(), out decimal price)) return;"),
                    Step(2,
                        "Xử lý logic với Toán tử & Rẽ nhánh",
                        "Áp dụng switch expression và toán tử để tính mức chiết khấu.",
                        "int memberLevel = 2;\ndecimal discountRate = memberLevel switch {\n    1 => 0.05m,\n    2 => 0.10m,\n    _ => 0m\n};"),
                    Step(3,
                        "Tách hàm tính toán độc lập (Method)",
                        "Gọi phương thức tính tổng tiền sau chiết khấu.",
                        "decimal finalAmount = CalculateFinalPrice(price, discountRate);"),
                    Step(4,
                        "Định dạng chuỗi kết quả đầu ra",
                        "Sử dụng String Interpolation kết hợp định dạng ngày tháng và số tiền.",
                        "Console.WriteLine($\"Ngày {DateTime.Now:dd/MM/yyyy} - Thanh toán: {finalAmount:N0} đ\");")
                },
                "(Câu hỏi tổng hợp Chủ đề 1) Đoạn code nào dưới đây tuân thủ TẤT CẢ các quy tắc thực hành tốt nhất (Best Practices) của C# khi nhận chuỗi giá tiền từ người dùng và tính thuế VAT 10%?",
                new List<string>
                {
                    "A. double price = double.Parse(input); double vat = price * 0.1;",
                    "B. if (!string.IsNullOrWhiteSpace(input) && decimal.TryParse(input.Trim(), out decimal price)) { decimal vat = price * 0.1m; }",
                    "C. int price = (int)input; int vat = price / 10;",
                    "D. string vat = input + \"10%\";"
                },
                "B",
                "Phương án B chuẩn xác ở cả 3 điểm: (1) Kiểm tra chuỗi không rỗng bằng IsNullOrWhiteSpace, (2) Dùng TryParse để không bị crash nếu người dùng nhập chữ, và (3) Dùng kiểu decimal (với hậu tố m) để tính tiền chính xác tuyệt đối."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 13 bài học con (1.1 -> 1.13) cho Chủ đề 1 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic1 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 1);
        if (topic1 == null)
            return;

        topic1.Title = "Chủ đề 1: Giới thiệu về C# và lập trình .NET";
        topic1.Description = "13 Bài học con chi tiết từ Kiến trúc .NET (CLR, BCL), Biến & Kiểu dữ liệu, Bộ nhớ Stack/Heap, Nullable, Ép kiểu, Toán tử, Chuỗi, Ngày tháng, Rẽ nhánh, Vòng lặp, Mảng/Enum đến Phương thức (Method).";

        var targetLessons = GetLessons(topic1.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic1.Id)
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
