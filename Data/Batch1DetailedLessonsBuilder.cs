using System.Text.Json;
using HoangViet.Models;

namespace HoangViet.Data;

public static class Batch1DetailedLessonsBuilder
{
    private static string ToJson<T>(T obj) => JsonSerializer.Serialize(obj);

    private static SubLesson BuildSubLesson(
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

    private static InteractiveFlowStep S(int num, string title, string detail, string code) => new()
    {
        StepNumber = num,
        StepTitle = title,
        StepDetail = detail,
        CodeSnippet = code
    };

    // =========================================================================
    // MENU 2 (DETAILED_TRACK) - CHỦ ĐỀ 1 (13 BÀI HỌC CON: 1.1 -> 1.13)
    // =========================================================================
    public static List<SubLesson> GetDetailedTopic1Lessons(int topicId)
    {
        return new List<SubLesson>
        {
            BuildSubLesson(topicId, "1.1",
                "Kiến trúc .NET Runtime (CLR, JIT Compiler, IL Code) & Cấu trúc Solution",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/clr",
                """
                Để hiểu sâu C#, bạn cần biết mã nguồn `.cs` được biên dịch và chạy trên máy tính như thế nào:
                • **Roslyn Compiler (`csc`):** Dịch mã nguồn C# (`.cs`) sang mã trung gian **IL (Intermediate Language)** lưu trong file `.dll` hoặc `.exe`.
                • **CLR (Common Language Runtime):** Môi trường thực thi của .NET quản lý bộ nhớ, luồng chạy (Thread), và an toàn kiểu dữ liệu.
                • **JIT Compiler (Just-In-Time):** Khi hàm được gọi lần đầu, JIT dịch mã IL sang **Native Machine Code** phù hợp với CPU (x64/ARM64) ngay tại thời điểm chạy.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "C# Source Code (.cs)",
                        "Đầu vào: File mã nguồn C# do lập trình viên viết -> Xử lý: Kiểm tra cú pháp và kiểu tĩnh -> Đẩy kết quả: Chuyển cho trình biên dịch Roslyn.",
                        "namespace HoangViet;\npublic class Calculator {\n    public static int Add(int a, int b) => a + b;\n}"),
                    S(2, "Roslyn Compiler -> IL (.dll)",
                        "Đầu vào: File .cs -> Xử lý: Dịch sang mã trung gian CIL/MSIL và đóng gói Metadata -> Đẩy kết quả: Sinh file Assembly (.dll).",
                        "// Lệnh CLI biên dịch dự án:\n// dotnet build --configuration Release\n// Kết quả: bin/Release/net10.0/HoangViet.dll"),
                    S(3, "CLR & JIT Compiler",
                        "Đầu vào: Mã IL trong file .dll -> Xử lý: JIT dịch IL sang mã máy Native CPU ngay khi phương thức được gọi -> Đẩy kết quả: Lệnh CPU thực thi.",
                        "// Khi hàm Add(5, 10) được gọi lần đầu tiên:\nint sum = Calculator.Add(5, 10);\n// JIT dịch IL của hàm Add sang lệnh Assembly x64"),
                    S(4, "Native Execution & GC",
                        "Đầu vào: Mã máy Native -> Xử lý: CPU chạy trực tiếp với tốc độ cao, CLR giám sát vùng nhớ -> Đẩy kết quả: Trả về kết quả chương trình.",
                        "Console.WriteLine($\"Tổng = {sum}\");")
                },
                "Trình biên dịch JIT (Just-In-Time) trong .NET CLR có nhiệm vụ chính là gì?",
                new List<string>
                {
                    "Dịch mã nguồn C# (.cs) trực tiếp sang file HTML",
                    "Dịch mã trung gian IL (Intermediate Language) sang mã máy (Native Machine Code) tại thời điểm chạy",
                    "Xóa các bảng trong SQL Server",
                    "Chỉ kiểm tra chính tả tên biến trong Visual Studio"
                },
                "Dịch mã trung gian IL (Intermediate Language) sang mã máy (Native Machine Code) tại thời điểm chạy",
                "Trình biên dịch C# (Roslyn) dịch `.cs` sang IL (`.dll`), còn JIT Compiler trong CLR dịch IL sang mã máy Native khi chương trình chạy."
            ),

            BuildSubLesson(topicId, "1.2",
                "Biến, Hằng số (const vs readonly) và Hệ thống kiểu dữ liệu cơ sở (Primitive Types)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/built-in-types",
                """
                Trong C#, mọi biến đều có kiểu dữ liệu xác định:
                • **Kiểu số nguyên:** `byte` (1B), `short` (2B), `int` (4B), `long` (8B).
                • **Kiểu số thực:** `float` (4B), `double` (8B - khoa học), `decimal` (16B - **bắt buộc dùng cho tiền tệ** để tránh sai số dấu phẩy động).
                • **`const` vs `readonly`:**
                  - `const`: Hằng số biên dịch (Compile-time), phải gán giá trị ngay lúc khai báo.
                  - `readonly`: Hằng số thời gian chạy (Runtime), có thể gán trong Constructor (rất hay dùng để inject Service trong Controller).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khai báo kiểu dữ liệu cơ sở",
                        "Đầu vào: Yêu cầu lưu trữ tuổi, giá tiền, trạng thái -> Xử lý: Chọn đúng kiểu int, decimal, bool -> Đẩy kết quả: Cấp phát đúng kích thước byte.",
                        "int studentAge = 22;\ndecimal tuitionFee = 15_500_000.50m; // Hậu tố m cho decimal\nbool isInternReady = true;"),
                    S(2, "Khai báo const (Compile-time)",
                        "Đầu vào: Giá trị cố định tuyệt đối -> Xử lý: Nhúng thẳng giá trị vào IL lúc build -> Đẩy kết quả: Không thể thay đổi ở bất kỳ đâu.",
                        "public const double VatRate = 0.08;"),
                    S(3, "Khai báo readonly (Runtime)",
                        "Đầu vào: Biến chỉ gán 1 lần lúc khởi tạo đối tượng -> Xử lý: Gán giá trị trong Constructor -> Đẩy kết quả: Bảo vệ tham chiếu suốt vòng đời object.",
                        "public class OrderService {\n    private readonly string _environmentName;\n    public OrderService(string env) => _environmentName = env;\n}"),
                    S(4, "Sử dụng var (Implicit Typing)",
                        "Đầu vào: Biểu thức vế phải đã rõ kiểu -> Xử lý: Trình biên dịch tự suy luận kiểu tĩnh lúc compile -> Đẩy kết quả: Code gọn mà vẫn 100% Strongly-Typed.",
                        "var totalAmount = tuitionFee * 1.08m; // Compiler tự hiểu totalAmount là decimal")
                },
                "Khi lập trình tính toán tiền tệ, hóa đơn hoặc số dư ngân hàng trong C#, bạn PHẢI chọn kiểu dữ liệu nào để không bị sai số làm tròn?",
                new List<string>
                {
                    "float",
                    "double",
                    "decimal",
                    "int"
                },
                "decimal",
                "Kiểu `decimal` (128-bit) lưu trữ theo hệ cơ số 10 với độ chính xác cao, chuyên dùng cho tài chính - tiền tệ để tránh sai số nhị phân của `float`/`double`."
            ),

            BuildSubLesson(topicId, "1.3",
                "Bản chất bộ nhớ: Value Types (Stack) vs Reference Types (Heap) & Boxing/Unboxing",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/value-types",
                """
                Hiểu rõ Stack và Heap giúp bạn tránh lỗi thay đổi dữ liệu ngoài ý muốn và tối ưu hiệu năng:
                • **Value Types (`int`, `double`, `bool`, `struct`, `enum`):** Lưu trực tiếp giá trị trên **Stack**. Khi gán biến này cho biến khác, C# tạo bản sao độc lập.
                • **Reference Types (`class`, `interface`, `delegate`, `string`, `array`):** Đối tượng thực nằm trên **Heap**, còn biến trên **Stack** chỉ giữ địa chỉ tham chiếu (con trỏ).
                • **Boxing & Unboxing:** Chuyển từ Value Type sang `object` (Boxing - cấp phát trên Heap tốn chi phí) và ngược lại (Unboxing).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Value Type trên vùng nhớ Stack",
                        "Đầu vào: Khai báo struct/int -> Xử lý: Lưu trực tiếp giá trị trên Stack; phép gán copy toàn bộ giá trị -> Đẩy kết quả: 2 biến độc lập nhau.",
                        "int a = 10;\nint b = a; // Copy giá trị 10 sang b\nb = 99;    // a vẫn bằng 10"),
                    S(2, "Reference Type trên vùng nhớ Heap",
                        "Đầu vào: Khởi tạo đối tượng class bằng từ khóa new -> Xử lý: Cấp phát object trên Heap, trả địa chỉ về Stack -> Đẩy kết quả: Hai biến cùng trỏ 1 vùng nhớ.",
                        "var user1 = new UserProfile { Name = \"Việt\" };\nvar user2 = user1; // Copy địa chỉ tham chiếu\nuser2.Name = \"Hoàng Việt\"; // user1.Name cũng đổi thành \"Hoàng Việt\"!"),
                    S(3, "Hiện tượng Boxing (Stack -> Heap)",
                        "Đầu vào: Gán biến int vào kiểu object -> Xử lý: CLR phải bọc giá trị int vào một khung đối tượng trên Heap -> Đẩy kết quả: Chậm hơn và tốn bộ nhớ GC.",
                        "int score = 100;\nobject boxedScore = score; // Boxing: Tạo object mới trên Heap"),
                    S(4, "Giải pháp Generic tránh Boxing",
                        "Đầu vào: Cần danh sách chứa số nguyên -> Xử lý: Dùng List<int> thay vì ArrayList -> Đẩy kết quả: Không xảy ra Boxing/Unboxing.",
                        "List<int> scores = new() { 90, 95, 100 }; // An toàn kiểu, không tốn chi phí Boxing")
                },
                "Điều gì xảy ra khi bạn gán `user2 = user1` (với `User` là một `class`) rồi thay đổi `user2.Name = \"An\"`?",
                new List<string>
                {
                    "`user1.Name` không thay đổi vì `class` là Value Type",
                    "`user1.Name` cũng đổi thành \"An\" vì `class` là Reference Type, cả hai biến đang cùng trỏ tới một đối tượng trên Heap",
                    "Trình biên dịch báo lỗi cú pháp",
                    "`user1` tự động bị xóa khỏi bộ nhớ"
                },
                "`user1.Name` cũng đổi thành \"An\" vì `class` là Reference Type, cả hai biến đang cùng trỏ tới một đối tượng trên Heap",
                "Với Reference Type (`class`), phép gán `user2 = user1` chỉ sao chép địa chỉ tham chiếu, nên cả hai biến cùng trỏ vào một đối tượng duy nhất trên Heap."
            ),

            BuildSubLesson(topicId, "1.4",
                "Ép kiểu an toàn (Implicit, Explicit, TryParse, Convert) tránh lỗi sập chương trình",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/types/casting-and-type-conversions",
                """
                Dữ liệu từ người dùng hoặc file cấu hình luôn ở dạng chuỗi (`string`), nên ép kiểu an toàn là kỹ năng sống còn:
                • **Implicit Cast (Ngầm định):** Từ kiểu nhỏ sang kiểu lớn (`int` -> `long` -> `double`), không mất dữ liệu.
                • **Explicit Cast (Tường minh):** `(int)3.99` -> cắt phần thập phân còn `3` (có nguy cơ tràn số).
                • **`int.Parse` vs `int.TryParse`:** `Parse` sẽ ném `FormatException` nếu chuỗi không hợp lệ; **`TryParse`** trả về `bool` an toàn tuyệt đối mà không ném ngoại lệ.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Implicit & Explicit Casting",
                        "Đầu vào: Các kiểu số khác kích thước -> Xử lý: Chuyển đổi tự động hoặc dùng toán tử (type) -> Đẩy kết quả: Giá trị ở kiểu mới.",
                        "int count = 42;\ndouble precise = count; // Implicit: an toàn\nint rounded = (int)9.85; // Explicit: kết quả = 9"),
                    S(2, "Nguy cơ lỗi với int.Parse()",
                        "Đầu vào: Chuỗi người dùng nhập \"12a\" -> Xử lý: Gọi int.Parse(\"12a\") -> Đẩy kết quả: Ném FormatException làm dừng luồng nếu không bắt lỗi.",
                        "// NGUY HIỂM nếu input không phải số nguyên:\n// int page = int.Parse(rawInput); // Ném FormatException!"),
                    S(3, "Chuẩn thực tế: Dùng TryParse()",
                        "Đầu vào: Chuỗi bất kỳ từ Query/Form -> Xử lý: Gọi int.TryParse(input, out int value) -> Đẩy kết quả: Trả về true/false an toàn, biến out nhận kết quả.",
                        "string rawPage = \"25\";\nif (int.TryParse(rawPage, out int pageNumber)) {\n    Console.WriteLine($\"Trang hợp lệ: {pageNumber}\");\n}"),
                    S(4, "Pattern Matching với is và as",
                        "Đầu vào: Đối tượng kiểu cha/object -> Xử lý: Kiểm tra kiểu và gán biến ngay trong if -> Đẩy kết quả: Ép kiểu đối tượng an toàn không sợ InvalidCastException.",
                        "object payload = \"Xin chào Hoàng Việt\";\nif (payload is string message) {\n    Console.WriteLine(message.ToUpper());\n}")
                },
                "Phương thức nào dưới đây giúp chuyển chuỗi `\"abc\"` sang số nguyên `int` một cách an toàn mà KHÔNG ném ra ngoại lệ (Exception)?",
                new List<string>
                {
                    "int.Parse(\"abc\")",
                    "Convert.ToInt32(\"abc\")",
                    "int.TryParse(\"abc\", out int result)",
                    "(int)\"abc\""
                },
                "int.TryParse(\"abc\", out int result)",
                "`int.TryParse` kiểm tra và trả về `false` nếu chuỗi không đúng định dạng số, gán `result = 0` thay vì ném `FormatException`."
            ),

            BuildSubLesson(topicId, "1.5",
                "Nullable Types (?, ??, ??=, ?.) và Cách phòng chống triệt để lỗi NullReferenceException",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/nullable-value-types",
                """
                `NullReferenceException` được mệnh danh là "sai lầm tỷ đô" trong lập trình. C# hiện đại cung cấp bộ toán tử chống Null cực mạnh:
                • **Nullable (`int?`, `string?`):** Cho phép biến nhận giá trị `null`.
                • **Null-conditional (`?.`):** Nếu đối tượng bên trái khác `null` thì mới truy cập thuộc tính bên phải, ngược lại trả về `null` êm ái.
                • **Null-coalescing (`??`):** Lấy giá trị bên trái nếu khác `null`, nếu `null` thì lấy giá trị dự phòng bên phải.
                • **Null-coalescing assignment (`??=`):** Chỉ gán giá trị mới nếu biến hiện tại đang là `null`.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khai báo kiểu Nullable (?)",
                        "Đầu vào: Cột ngày hoàn thành trong DB có thể trống (NULL) -> Xử lý: Dùng DateTime? -> Đẩy kết quả: Ánh xạ chính xác cột NULL của SQL Server.",
                        "DateTime? completedAt = null;\nint? bonusPoints = 15;\nbool hasBonus = bonusPoints.HasValue;"),
                    S(2, "Truy cập an toàn bằng toán tử ?.",
                        "Đầu vào: Đối tượng Order hoặc Customer có thể null -> Xử lý: Dùng ?. để nối chuỗi truy cập -> Đẩy kết quả: Không bao giờ bị nổ NullReferenceException.",
                        "string? city = order?.Customer?.Address?.City;\n// Nếu order hoặc Customer hoặc Address là null -> city = null an toàn"),
                    S(3, "Giá trị mặc định với toán tử ??",
                        "Đầu vào: Biến có thể null -> Xử lý: Kết hợp ?. và ?? để đặt giá trị mặc định -> Đẩy kết quả: Luôn có chuỗi hợp lệ hiển thị lên UI.",
                        "string displayCity = order?.Customer?.Address?.City ?? \"Chưa cập nhật địa chỉ\";"),
                    S(4, "Gán mặc định gọn gàng với ??=",
                        "Đầu vào: Danh sách hoặc cấu hình có thể chưa khởi tạo -> Xử lý: Dùng ??= -> Đẩy kết quả: Khởi tạo List rỗng nếu biến đang null.",
                        "List<string>? tags = null;\ntags ??= new List<string>(); // Khởi tạo nếu đang null\ntags.Add(\"CSharp10\");")
                },
                "Biểu thức `string name = user?.FullName ?? \"Khách\";` sẽ trả về kết quả gì nếu biến `user` đang có giá trị là `null`?",
                new List<string>
                {
                    "Ném ra lỗi NullReferenceException",
                    "Trả về chuỗi \"Khách\"",
                    "Trả về chuỗi rỗng \"\"",
                    "Trả về giá trị null"
                },
                "Trả về chuỗi \"Khách\"",
                "Toán tử `user?.FullName` trả về `null` khi `user` là `null`, sau đó toán tử `??` lấy giá trị mặc định bên phải là `\"Khách\"`."
            ),

            BuildSubLesson(topicId, "1.6",
                "Các loại Toán tử trong C# và Quy tắc độ ưu tiên toán tử",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/",
                """
                Nắm vững toán tử giúp bạn đọc biểu thức điều kiện trong tầng Service nhanh và chính xác:
                • **Toán tử số học:** `+`, `-`, `*`, `/`, `%` (chia lấy dư - rất hay dùng kiểm tra chẵn lẻ hoặc phân trang).
                • **Toán tử logic ngắn mạch (Short-circuit):** `&&` (AND) và `||` (OR). Với `A && B`, nếu `A` sai thì C# **bỏ qua không chạy `B`**.
                • **Toán tử 3 ngôi (`condition ? exprIfTrue : exprIfFalse`):** Viết điều kiện gán giá trị ngắn gọn trên 1 dòng.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Toán tử số học & Chia lấy dư (%)",
                        "Đầu vào: Tổng số bản ghi và kích thước trang -> Xử lý: Tính số trang bằng phép chia và % -> Đẩy kết quả: Tổng số trang chính xác.",
                        "int totalItems = 25, pageSize = 10;\nint totalPages = (totalItems / pageSize) + (totalItems % pageSize > 0 ? 1 : 0); // = 3"),
                    S(2, "Short-circuit Evaluation (&& và ||)",
                        "Đầu vào: Kiểm tra mảng khác null và có phần tử -> Xử lý: Vế trái false thì dừng ngay không chạy vế phải -> Đẩy kết quả: Bảo vệ vế phải khỏi lỗi.",
                        "if (items != null && items.Count > 0) {\n    // Nếu items == null, C# dừng ngay ở vế đầu nên items.Count không bị lỗi\n}"),
                    S(3, "Toán tử tăng/giảm (++i vs i++)",
                        "Đầu vào: Biến đếm -> Xử lý: Tiền tố (++i) tăng trước khi lấy giá trị; Hậu tố (i++) lấy giá trị trước rồi mới tăng -> Đẩy kết quả: Giá trị biểu thức.",
                        "int x = 5;\nint a = ++x; // x lên 6, a = 6\nint b = x++; // b = 6, sau đó x mới lên 7"),
                    S(4, "Toán tử 3 ngôi (Ternary Operator)",
                        "Đầu vào: Điểm số học viên -> Xử lý: Kiểm tra >= 50 -> Đẩy kết quả: Chuỗi trạng thái Đạt / Chưa đạt.",
                        "int score = 85;\nstring status = score >= 50 ? \"Đạt yêu cầu\" : \"Cần ôn lại\";")
                },
                "Nhờ cơ chế Short-circuit (ngắn mạch) của toán tử `&&`, điều gì xảy ra trong câu lệnh `if (user != null && user.IsActive)` khi `user` bằng `null`?",
                new List<string>
                {
                    "Chương trình vẫn kiểm tra `user.IsActive` và ném lỗi NullReferenceException",
                    "Thấy `user != null` là `false`, C# lập tức kết luận cả biểu thức `false` và KHÔNG chạy vế `user.IsActive`",
                    "Biểu thức trả về `true`",
                    "Chương trình tự khởi tạo `user` mới"
                },
                "Thấy `user != null` là `false`, C# lập tức kết luận cả biểu thức `false` và KHÔNG chạy vế `user.IsActive`",
                "Toán tử `&&` kiểm tra từ trái sang phải; nếu vế trái đã `false` thì kết quả chắc chắn `false` nên vế phải được bỏ qua hoàn toàn."
            ),

            BuildSubLesson(topicId, "1.7",
                "Cấu trúc rẽ nhánh truyền thống (if-else, switch-case) và Kỹ thuật Guard Clauses",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/selection-statements",
                """
                Trong các dự án thực tế, lập trình viên Senior **không viết `if-else` lồng nhau 4-5 tầng (Arrow Anti-pattern)** mà sử dụng kỹ thuật **Guard Clauses (Return sớm)**:
                • Kiểm tra các trường hợp lỗi/không hợp lệ ở ngay đầu hàm và `return` (hoặc `throw`) ngay lập tức.
                • Luồng chính (Happy Path) ở cuối hàm sẽ thẳng hàng, phẳng và cực kỳ dễ đọc.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Vấn đề: If-Else lồng nhau (Arrow Code)",
                        "Đầu vào: Nhiều điều kiện kiểm tra đơn hàng -> Xử lý: Lồng if trong if 4 cấp -> Đẩy kết quả: Code thụt lề sâu, rất khó đọc và bảo trì.",
                        "// Tránh viết kiểu mũi tên lồng nhau:\n// if (order != null) { if (order.Items.Count > 0) { if (user.IsActive) { ... } } }"),
                    S(2, "Giải pháp: Guard Clause 1 (Kiểm tra Null)",
                        "Đầu vào: Tham số order -> Xử lý: Nếu null thì trả lỗi ngay ở dòng đầu tiên -> Đẩy kết quả: Loại bỏ 1 cấp thụt lề.",
                        "public string ProcessOrder(Order? order) {\n    if (order is null) return \"Đơn hàng không tồn tại\";"),
                    S(3, "Guard Clause 2 (Kiểm tra trạng thái)",
                        "Đầu vào: order đã chắc chắn khác null -> Xử lý: Kiểm tra giỏ hàng rỗng hoặc đã thanh toán -> Đẩy kết quả: Thoát sớm nếu vi phạm.",
                        "    if (order.Items.Count == 0) return \"Giỏ hàng trống\";\n    if (order.IsPaid) return \"Đơn hàng đã được thanh toán trước đó\";"),
                    S(4, "Happy Path phẳng và sạch",
                        "Đầu vào: Đơn hàng đã vượt qua mọi cổng kiểm tra -> Xử lý: Thực thi nghiệp vụ chính không cần bọc trong else -> Đẩy kết quả: Code chuẩn Clean Code.",
                        "    // Luồng chính nằm thẳng hàng ngoài cùng:\n    order.IsPaid = true;\n    return \"Thanh toán thành công!\";\n}")
                },
                "Mục đích chính của kỹ thuật **Guard Clauses (Kiểm tra điều kiện biên và return sớm)** là gì?",
                new List<string>
                {
                    "Làm cho file code dài hơn",
                    "Loại bỏ tình trạng `if-else` lồng nhau nhiều tầng, giúp luồng xử lý chính (Happy Path) phẳng và dễ đọc",
                    "Thay thế hoàn toàn vòng lặp `for`",
                    "Tự động kết nối cơ sở dữ liệu"
                },
                "Loại bỏ tình trạng `if-else` lồng nhau nhiều tầng, giúp luồng xử lý chính (Happy Path) phẳng và dễ đọc",
                "Guard Clauses xử lý dứt điểm các trường hợp không hợp lệ ngay đầu hàm, giữ cho phần code nghiệp vụ chính không bị lồng trong nhiều khối `else`."
            ),

            BuildSubLesson(topicId, "1.8",
                "Cấu trúc rẽ nhánh hiện đại: Switch Expressions & Pattern Matching",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/switch-expression",
                """
                Từ C# 8 đến C# 12, **Switch Expression** kết hợp **Pattern Matching** đã thay thế hoàn toàn `switch-case-break` dài dòng:
                • Sử dụng mũi tên `=>` để trả về trực tiếp giá trị từ mỗi nhánh.
                • Hỗ trợ **Relational Pattern** (`>=`, `<`), **Logical Pattern** (`and`, `or`, `not`) và **Property Pattern** (`{ Role: "Admin", IsActive: true }`).
                • Dấu gạch dưới `_` đóng vai trò là nhánh mặc định (`default`).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Switch Expression cơ bản",
                        "Đầu vào: Mã trạng thái đơn hàng -> Xử lý: Ánh xạ từng giá trị bằng dấu => -> Đẩy kết quả: Trả trực tiếp chuỗi mô tả vào biến.",
                        "string statusText = statusCode switch {\n    1 => \"Chờ xác nhận\",\n    2 => \"Đang giao hàng\",\n    3 => \"Hoàn thành\",\n    _ => \"Không xác định\" // _ tương đương default\n};"),
                    S(2, "Relational & Logical Pattern",
                        "Đầu vào: Điểm số từ 0-100 -> Xử lý: So sánh khoảng giá trị bằng >= và and -> Đẩy kết quả: Xếp loại học lực cực gọn.",
                        "string rank = score switch {\n    >= 90 and <= 100 => \"Xuất sắc\",\n    >= 75 and < 90   => \"Giỏi\",\n    >= 50 and < 75   => \"Đạt\",\n    _                => \"Cần cố gắng\"\n};"),
                    S(3, "Property Pattern trên Object",
                        "Đầu vào: Đối tượng Order -> Xử lý: Kiểm tra trực tiếp các thuộc tính bên trong object -> Đẩy kết quả: Tính phí vận chuyển theo quy tắc.",
                        "decimal shippingFee = order switch {\n    { IsVipMember: true } => 0m,\n    { TotalAmount: >= 500_000m } => 0m,\n    { City: \"HCM\" or \"HaNoi\" } => 20_000m,\n    _ => 35_000m\n};"),
                    S(4, "Kiểm tra đầy đủ (Exhaustive Check)",
                        "Đầu vào: Toàn bộ các nhánh switch -> Xử lý: Trình biên dịch cảnh báo nếu thiếu trường hợp -> Đẩy kết quả: Đảm bảo mọi đầu vào đều có kết quả trả về.",
                        "Console.WriteLine($\"Hạng: {rank} - Phí ship: {shippingFee:N0}đ\");")
                },
                "Trong cú pháp `switch expression` của C# hiện đại, ký hiệu `_` (dấu gạch dưới) ở nhánh cuối cùng có ý nghĩa là gì?",
                new List<string>
                {
                    "Báo lỗi biên dịch",
                    "Nhánh mặc định (Discard pattern - tương đương với `default:` trong switch truyền thống), khớp với mọi giá trị còn lại",
                    "Chỉ khớp với số âm",
                    "Dừng chương trình ngay lập tức"
                },
                "Nhánh mặc định (Discard pattern - tương đương với `default:` trong switch truyền thống), khớp với mọi giá trị còn lại",
                "Ký hiệu `_` (discard pattern) trong `switch expression` đóng vai trò nhánh mặc định bắt tất cả các trường hợp chưa khớp ở phía trên."
            ),

            BuildSubLesson(topicId, "1.9",
                "Vòng lặp cơ bản (for, while, do-while) và Bài toán duyệt mảng 1 chiều, 2 chiều",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/iteration-statements",
                """
                Ba vòng lặp truyền thống phục vụ các mục đích khác nhau:
                • **`for (int i = 0; i < n; i++)`:** Dùng khi biết trước số lần lặp hoặc cần **chỉ số `i` (index)** để truy cập/sửa phần tử tại vị trí `arr[i]`.
                • **`while (condition)`:** Kiểm tra điều kiện trước, dùng khi chưa biết trước số lần lặp (VD: đọc từng dòng file, chờ kết nối).
                • **`do { ... } while (condition)`:** Luôn chạy khối lệnh **ít nhất 1 lần** rồi mới kiểm tra điều kiện.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Vòng lặp for với chỉ số Index",
                        "Đầu vào: Mảng điểm số -> Xử lý: Duyệt từ i = 0 đến Length - 1 -> Đẩy kết quả: Truy cập cả vị trí i lẫn giá trị scores[i].",
                        "int[] scores = { 70, 85, 90, 100 };\nfor (int i = 0; i < scores.Length; i++) {\n    Console.WriteLine($\"Bài #{i + 1}: {scores[i]} điểm\");\n}"),
                    S(2, "Duyệt mảng 2 chiều (Ma trận)",
                        "Đầu vào: Bảng dữ liệu hàng và cột int[,] -> Xử lý: 2 vòng for lồng nhau (hàng r, cột c) -> Đẩy kết quả: Đọc từng ô trong bảng.",
                        "int[,] grid = { { 1, 2 }, { 3, 4 } };\nfor (int r = 0; r < grid.GetLength(0); r++)\n    for (int c = 0; c < grid.GetLength(1); c++)\n        Console.Write($\"{grid[r, c]} \");"),
                    S(3, "Vòng lặp while (Chưa biết số lần lặp)",
                        "Đầu vào: Số lần thử kết nối tối đa -> Xử lý: Lặp chừng nào chưa kết nối được và còn lượt thử -> Đẩy kết quả: Dừng ngay khi thành công.",
                        "int retries = 0;\nwhile (!isConnected && retries < 3) {\n    retries++;\n    isConnected = TryConnectDatabase();\n}"),
                    S(4, "Vòng lặp do-while (Chạy ít nhất 1 lần)",
                        "Đầu vào: Yêu cầu thực hiện tác vụ tối thiểu 1 lần -> Xử lý: Chạy thân vòng lặp trước rồi mới xét while -> Đẩy kết quả: Đảm bảo lượt chạy đầu tiên.",
                        "int attempt = 0;\ndo {\n    attempt++;\n} while (attempt < 1);")
                },
                "Điểm khác biệt cốt lõi giữa vòng lặp `while` và `do-while` trong C# là gì?",
                new List<string>
                {
                    "`while` chạy nhanh gấp đôi `do-while`",
                    "`do-while` luôn thực thi khối lệnh ít nhất 1 lần trước khi kiểm tra điều kiện, còn `while` kiểm tra điều kiện trước",
                    "`do-while` không thể dùng với kiểu số nguyên",
                    "`while` bắt buộc phải chạy vô hạn"
                },
                "`do-while` luôn thực thi khối lệnh ít nhất 1 lần trước khi kiểm tra điều kiện, còn `while` kiểm tra điều kiện trước",
                "Trong `do-while`, điều kiện nằm ở cuối nên thân vòng lặp luôn được chạy tối thiểu 1 lần ngay cả khi điều kiện ban đầu là `false`."
            ),

            BuildSubLesson(topicId, "1.10",
                "Vòng lặp foreach, Cơ chế IEnumerable và Từ khóa điều khiển (break, continue, return, yield)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/yield",
                """
                Khi duyệt danh sách đối tượng trong Backend, **`foreach`** là vòng lặp được dùng nhiều nhất (hơn 90%):
                • **`foreach`** hoạt động dựa trên bộ lặp `IEnumerable<T>` / `GetEnumerator()`, an toàn tuyệt đối vì không bao giờ bị lỗi tràn chỉ số (`IndexOutOfRangeException`).
                • **`continue`:** Bỏ qua phần còn lại của lượt lặp hiện tại để sang phần tử tiếp theo.
                • **`break`:** Thoát khỏi vòng lặp ngay lập tức.
                • **`yield return`:** Trả về từng phần tử một theo kiểu **Lazy Evaluation** (cần đến đâu sinh đến đó, không tốn RAM tạo `List` trung gian).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Duyệt danh sách an toàn với foreach",
                        "Đầu vào: Danh sách bài học List<SubLesson> -> Xử lý: Duyệt từng phần tử dạng chỉ đọc (read-only iteration) -> Đẩy kết quả: Code ngắn gọn, không lo sai index.",
                        "foreach (var lesson in lessons) {\n    Console.WriteLine($\"{lesson.LessonCode}: {lesson.Title}\");\n}"),
                    S(2, "Lọc nhanh với continue & Dừng sớm với break",
                        "Đầu vào: Danh sách đơn hàng -> Xử lý: Bỏ qua đơn đã hủy (continue), dừng khi gặp đơn lỗi nặng (break) -> Đẩy kết quả: Kiểm soát luồng lặp chính xác.",
                        "foreach (var order in orders) {\n    if (order.IsCancelled) continue; // Bỏ qua lượt này, sang đơn kế tiếp\n    if (order.HasCriticalError) break; // Thoát hẳn vòng lặp\n    Process(order);\n}"),
                    S(3, "Lazy Streaming với yield return",
                        "Đầu vào: Tập dữ liệu lớn -> Xử lý: Dùng yield return trả từng phần tử khi nơi gọi duyệt tới -> Đẩy kết quả: Tiết kiệm bộ nhớ RAM.",
                        "public static IEnumerable<int> GetEvenNumbers(int max) {\n    for (int i = 2; i <= max; i += 2)\n        yield return i; // Trả từng số ngay khi foreach bên ngoài cần\n}"),
                    S(4, "Lưu ý: Không xóa phần tử trực tiếp trong foreach",
                        "Đầu vào: Muốn xóa phần tử thỏa điều kiện -> Xử lý: Không gọi list.Remove() bên trong foreach (sẽ lỗi InvalidOperationException) -> Đẩy kết quả: Dùng RemoveAll().",
                        "// ĐÚNG CHUẨN: Dùng RemoveAll thay vì xóa trong foreach\norders.RemoveAll(o => o.IsCancelled);")
                },
                "Từ khóa `continue` bên trong vòng lặp `foreach` hoặc `for` có tác dụng gì?",
                new List<string>
                {
                    "Thoát hoàn toàn khỏi vòng lặp",
                    "Dừng toàn bộ chương trình",
                    "Bỏ qua các câu lệnh phía dưới của lượt lặp hiện tại và chuyển ngay sang lượt lặp kế tiếp",
                    "Khởi động lại vòng lặp từ phần tử đầu tiên"
                },
                "Bỏ qua các câu lệnh phía dưới của lượt lặp hiện tại và chuyển ngay sang lượt lặp kế tiếp",
                "`continue` chỉ bỏ qua phần còn lại của vòng lặp hiện hành để nhảy sang phần tử tiếp theo, còn `break` mới thoát hẳn khỏi vòng lặp."
            ),

            BuildSubLesson(topicId, "1.11",
                "Cơ chế xử lý ngoại lệ chuyên sâu (try - catch - finally, throw vs throw ex, Custom Exceptions)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/exceptions/",
                """
                Xử lý ngoại lệ đúng chuẩn giúp bạn biết chính xác dòng code nào gây lỗi trên môi trường thực tế:
                • **Thứ tự bắt lỗi:** Luôn đặt `catch` của Exception **cụ thể** (như `SqlException`, `ArgumentNullException`) ở trên, và `catch (Exception ex)` tổng quát ở dưới cùng.
                • **Quy tắc vàng `throw;` vs `throw ex;`:**
                  - `throw;`: Ném lại ngoại lệ và **GIỮ NGUYÊN StackTrace gốc** (biết chính xác lỗi phát sinh ở dòng nào).
                  - `throw ex;`: Làm **MẤT StackTrace gốc** (chỉ thấy dòng chứa chữ `throw ex`), tuyệt đối tránh dùng!
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khối try & Nhiều khối catch theo độ ưu tiên",
                        "Đầu vào: Đoạn code gọi DB/File có rủi ro -> Xử lý: Bắt lỗi cụ thể trước, lỗi chung sau -> Đẩy kết quả: Xử lý đúng từng tình huống lỗi.",
                        "try {\n    await SaveDataAsync(input);\n} catch (ArgumentNullException ex) {\n    LogWarning(ex.Message);\n}"),
                    S(2, "Giữ nguyên vết lỗi bằng throw;",
                        "Đầu vào: Bắt lỗi ở tầng dưới để ghi log rồi ném lên trên -> Xử lý: Dùng 'throw;' đứng một mình -> Đẩy kết quả: Giữ trọn vẹn StackTrace gốc.",
                        "catch (Exception ex) {\n    _logger.LogError(ex, \"Lỗi khi lưu dữ liệu\");\n    throw; // ĐÚNG: Giữ nguyên số dòng lỗi gốc trong StackTrace!\n    // throw ex; -> SAI: Xóa mất dấu vết dòng gây lỗi ban đầu\n}"),
                    S(3, "Khối finally & câu lệnh using",
                        "Đầu vào: Tài nguyên cần đóng (File, Connection) -> Xử lý: finally luôn chạy dù có lỗi hay không -> Đẩy kết quả: Không bao giờ bị rò rỉ tài nguyên.",
                        "finally {\n    // Luôn luôn chạy dù khối try thành công hay ném Exception\n    CleanupTempFiles();\n}"),
                    S(4, "Tạo Custom Domain Exception",
                        "Đầu vào: Lỗi nghiệp vụ riêng của dự án -> Xử lý: Kế thừa lớp Exception -> Đẩy kết quả: Tầng Middleware dễ dàng phân loại để trả mã HTTP 400/404.",
                        "public class InsufficientBalanceException : Exception {\n    public InsufficientBalanceException(string msg) : base(msg) { }\n}")
                },
                "Trong khối `catch (Exception ex)`, tại sao bạn phải viết `throw;` thay vì `throw ex;` khi muốn ném tiếp lỗi lên tầng trên?",
                new List<string>
                {
                    "Vì `throw ex;` không biên dịch được trong C# 10",
                    "Vì `throw;` giữ nguyên toàn bộ StackTrace gốc (vị trí dòng code thực sự gây lỗi), còn `throw ex;` làm mất dấu vết gốc",
                    "Vì `throw;` tự động sửa hết lỗi",
                    "Hai cách viết giống hệt nhau"
                },
                "Vì `throw;` giữ nguyên toàn bộ StackTrace gốc (vị trí dòng code thực sự gây lỗi), còn `throw ex;` làm mất dấu vết gốc",
                "`throw;` bảo toàn `StackTrace` ban đầu giúp lập trình viên đọc log biết ngay dòng nào trong Repository/Service gây lỗi."
            ),

            BuildSubLesson(topicId, "1.12",
                "Quản lý bộ nhớ tự động: Garbage Collector (GC), IDisposable và Từ khóa using",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/garbage-collection/fundamentals",
                """
                .NET tự động thu hồi vùng nhớ Heap không còn sử dụng thông qua **Garbage Collector (GC)** chia làm 3 thế hệ:
                • **Generation 0 (Gen 0):** Các đối tượng mới khởi tạo, vòng đời ngắn (như biến cục bộ, DTO trong 1 HTTP Request).
                • **Generation 1 & 2 (Gen 1, Gen 2):** Các đối tượng sống sót qua các đợt dọn dẹp trước (như Cache, Singleton Service).
                • **Unmanaged Resources (Kết nối DB, File Stream, Socket):** GC không tự đóng ngay được, bạn phải giải phóng thông qua giao diện `IDisposable` và từ khóa **`using`**.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Cấp phát trên Managed Heap (Gen 0)",
                        "Đầu vào: Khởi tạo đối tượng bằng new -> Xử lý: Đặt vào vùng nhớ Gen 0 -> Đẩy kết quả: Cấp phát cực nhanh.",
                        "var dto = new ProductDto { Id = 1, Name = \"Bàn phím cơ\" }; // Nằm ở Gen 0"),
                    S(2, "GC thu hồi đối tượng không còn tham chiếu",
                        "Đầu vào: Request kết thúc, không còn biến nào trỏ tới dto -> Xử lý: GC quét Gen 0 và giải phóng vùng nhớ -> Đẩy kết quả: Trả lại RAM cho hệ thống.",
                        "// Khi hàm kết thúc, dto mất tham chiếu -> GC tự dọn ở chu kỳ Gen 0 tiếp theo"),
                    S(3, "Tài nguyên Unmanaged & IDisposable",
                        "Đầu vào: Mở FileStream, SqlConnection, HttpClient -> Xử lý: Các lớp này thực thi IDisposable.Dispose() -> Đẩy kết quả: Giải phóng handle hệ điều hành.",
                        "// Nếu quên Dispose() -> Rò rỉ kết nối (Connection Leak) hoặc khóa file!"),
                    S(4, "Cú pháp using declaration hiện đại",
                        "Đầu vào: Khai báo đối tượng IDisposable với từ khóa using -> Xử lý: Tự động gọi Dispose() ngay khi ra khỏi scope hàm -> Đẩy kết quả: Code gọn và an toàn 100%.",
                        "using var stream = new FileStream(\"log.txt\", FileMode.OpenOrCreate);\nusing var writer = new StreamWriter(stream);\nawait writer.WriteLineAsync(\"Hoàng Việt Log\");\n// Tự động gọi Dispose() khi kết thúc hàm!")
                },
                "Từ khóa `using var stream = new FileStream(...);` trong C# có tác dụng gì?",
                new List<string>
                {
                    "Nhập thư viện ở đầu file",
                    "Đảm bảo phương thức `Dispose()` của `stream` được tự động gọi để giải phóng tài nguyên ngay khi biến ra khỏi phạm vi (scope), kể cả khi có Exception",
                    "Giữ file luôn mở vĩnh viễn",
                    "Tăng kích thước file lên gấp đôi"
                },
                "Đảm bảo phương thức `Dispose()` của `stream` được tự động gọi để giải phóng tài nguyên ngay khi biến ra khỏi phạm vi (scope), kể cả khi có Exception",
                "`using declaration` được trình biên dịch dịch thành khối `try ... finally { stream.Dispose(); }`, đảm bảo tài nguyên luôn được đóng an toàn."
            ),

            BuildSubLesson(topicId, "1.13",
                "Quy chuẩn đặt tên (Naming Conventions) & Tổ chức bình luận XML Documentation",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/coding-style/identifier-names",
                """
                Code của bạn ở công ty sẽ được đồng đội và Mentor đọc mỗi ngày. Tuân thủ chuẩn đặt tên của Microsoft giúp code chuyên nghiệp ngay từ cái nhìn đầu tiên:
                • **`PascalCase`:** Tên `Class`, `Struct`, `Record`, `Enum`, `Method`, `Property`, `Public Constant` (VD: `OrderService`, `CalculateTotal()`).
                • **`IPascalCase`:** Tên `Interface` luôn bắt đầu bằng chữ `I` hoa (VD: `IOrderRepository`).
                • **`camelCase`:** Biến cục bộ và tham số hàm (VD: `totalPrice`, `customerId`).
                • **`_camelCase`:** Biến `private readonly` field trong class (VD: `_dbContext`, `_logger`).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "PascalCase cho Class, Method, Property",
                        "Đầu vào: Định nghĩa lớp và thuộc tính công khai -> Xử lý: Viết hoa chữ cái đầu mỗi từ -> Đẩy kết quả: Đúng chuẩn Microsoft .NET.",
                        "public class CourseEnrollmentService {\n    public int MaxStudentCount { get; set; }\n    public void RegisterStudent() { }\n}"),
                    S(2, "Tiền tố I cho Interface & _camelCase cho Private Field",
                        "Đầu vào: Khai báo Interface và biến Dependency Injection -> Xử lý: Dùng I... và _... -> Đẩy kết quả: Nhìn vào tên biến biết ngay phạm vi của nó.",
                        "public interface IStudentRepository { }\n\npublic class StudentController {\n    private readonly IStudentRepository _studentRepository;\n}"),
                    S(3, "Hậu tố Async cho hàm bất đồng bộ",
                        "Đầu vào: Phương thức trả về Task hoặc Task<T> -> Xử lý: Thêm đuôi Async vào tên hàm -> Đẩy kết quả: Nhắc nhở người gọi phải dùng từ khóa await.",
                        "public async Task<StudentDto?> GetStudentByIdAsync(int studentId) {\n    return await _studentRepository.FindAsync(studentId);\n}"),
                    S(4, "XML Documentation Comments (///)",
                        "Đầu vào: Gõ /// phía trên hàm hoặc API -> Xử lý: Mô tả summary và param -> Đẩy kết quả: Hiển thị gợi ý trực tiếp trên IntelliSense và Swagger UI.",
                        "/// <summary>\n/// Tính điểm trung bình kỹ năng của học viên.\n/// </summary>\n/// <param name=\"studentId\">Mã định danh học viên</param>\npublic double CalculateAverageScore(int studentId) => 88.5;")
                },
                "Theo chuẩn đặt tên chính thức của Microsoft C#, một trường `private readonly` lưu trữ Dependency Injection trong class nên được đặt tên như thế nào?",
                new List<string>
                {
                    "`PRODUCT_SERVICE`",
                    "`_productService` (bắt đầu bằng dấu gạch dưới và viết theo camelCase)",
                    "`IProductService`",
                    "`product_service_private`"
                },
                "`_productService` (bắt đầu bằng dấu gạch dưới và viết theo camelCase)",
                "Chuẩn C# của Microsoft sử dụng `_camelCase` (ví dụ `_dbContext`, `_productService`) cho các `private field` để phân biệt tức thì với biến cục bộ và Property."
            )
        };
    }

    // =========================================================================
    // MENU 2 (DETAILED_TRACK) - CHỦ ĐỀ 2 (11 BÀI HỌC CON: 2.1 -> 2.11)
    // =========================================================================
    public static List<SubLesson> GetDetailedTopic2Lessons(int topicId)
    {
        return new List<SubLesson>
        {
            BuildSubLesson(topicId, "2.1",
                "Giải phẫu một Phương thức (Method Signature, Access Modifiers, Return Types)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/methods",
                """
                Mỗi phương thức (Method) là một đơn vị xử lý độc lập. Khi đọc một hàm trong C#, hãy chú ý 5 thành phần:
                1. **Access Modifier:** `public` (gọi từ mọi nơi), `private` (chỉ trong class đó), `protected` (class đó và class con), `internal` (trong cùng project/assembly).
                2. **Modifier bổ sung:** `static` (gọi trực tiếp qua tên Class), `async` (bất đồng bộ), `virtual`/`override` (đa hình).
                3. **Return Type:** Kiểu dữ liệu trả về (`void` nếu không trả về gì, `int`, `OrderDto`, `Task<List<User>>`...).
                4. **Method Signature (Chữ ký hàm):** Gồm **Tên hàm + Danh sách kiểu tham số theo thứ tự**.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Đọc Access Modifier & Static/Instance",
                        "Đầu vào: Dòng khai báo phương thức -> Xử lý: Xác định phạm vi truy cập public/private -> Đẩy kết quả: Biết hàm này là API công khai hay hàm phụ nội bộ.",
                        "public decimal CalculateFinalPrice(decimal unitPrice, int quantity) {\n    return ValidateAndCompute(unitPrice, quantity);\n}"),
                    S(2, "Hàm nội bộ Private Helper",
                        "Đầu vào: Logic phụ trợ bên trong class -> Xử lý: Đánh dấu private để ẩn khỏi bên ngoài -> Đẩy kết quả: Giữ bề mặt class gọn gàng.",
                        "private decimal ValidateAndCompute(decimal price, int qty) {\n    if (price < 0 || qty <= 0) return 0m;\n    return price * qty;\n}"),
                    S(3, "Expression-bodied Method (=>)",
                        "Đầu vào: Hàm chỉ có 1 biểu thức tính toán -> Xử lý: Dùng dấu suy ra => thay cho cặp ngoặc nhọn { return ...; } -> Đẩy kết quả: Code ngắn gọn, thanh thoát.",
                        "public bool IsEligibleForInternship(int completedLessons) => completedLessons >= 15;"),
                    S(4, "Local Functions (Hàm cục bộ trong hàm)",
                        "Đầu vào: Hàm nhỏ chỉ phục vụ duy nhất cho 1 phương thức mẹ -> Xử lý: Khai báo ngay bên trong phương thức mẹ -> Đẩy kết quả: Tránh làmรก class.",
                        "public string FormatSummary(string title, int score) {\n    string Normalize(string s) => s.Trim().ToUpperInvariant();\n    return $\"[{Normalize(title)}] Điểm: {score}\";\n}")
                },
                "Thành phần nào dưới đây dùng để chỉ định một phương thức CHỈ được phép gọi ở bên trong chính `class` chứa nó?",
                new List<string>
                {
                    "public",
                    "internal",
                    "private",
                    "static"
                },
                "private",
                "`private` giới hạn phạm vi truy cập ở mức hẹp nhất: chỉ các hàm nằm trong cùng một `class` mới gọi được."
            ),

            BuildSubLesson(topicId, "2.2",
                "Cơ chế truyền tham số: Pass-by-Value vs Pass-by-Reference (ref, out, in, params)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/keywords/method-parameters",
                """
                Hiểu rõ các từ khóa đi kèm tham số hàm giúp bạn đọc được toàn bộ các thư viện .NET:
                • **Mặc định (Pass-by-Value):** Truyền bản sao của giá trị (hoặc bản sao của địa chỉ tham chiếu).
                • **`ref`:** Truyền tham chiếu hai chiều — biến phải được khởi tạo trước khi truyền vào, hàm có thể đọc và sửa trực tiếp biến gốc.
                • **`out`:** Truyền tham chiếu đầu ra — hàm **bắt buộc phải gán giá trị** cho biến `out` trước khi `return` (dùng để trả về nhiều giá trị cùng lúc, như `TryParse`).
                • **`in`:** Truyền tham chiếu chỉ đọc (Read-only reference) giúp tránh copy `struct` lớn mà không cho phép sửa.
                • **`params`:** Cho phép truyền số lượng đối số tùy ý cách nhau bởi dấu phẩy.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Từ khóa ref (Đọc và sửa biến gốc)",
                        "Đầu vào: Biến balance đã có giá trị ban đầu -> Xử lý: Truyền ref balance vào hàm -> Đẩy kết quả: Biến gốc bên ngoài thay đổi trực tiếp.",
                        "void AddBonus(ref decimal balance, decimal bonus) {\n    balance += bonus;\n}\ndecimal myMoney = 100m;\nAddBonus(ref myMoney, 25m); // myMoney = 125m"),
                    S(2, "Từ khóa out (Trả về thêm kết quả phụ)",
                        "Đầu vào: Cần kiểm tra hợp lệ vừa lấy thông báo lỗi -> Xử lý: Khai báo out string error -> Đẩy kết quả: Hàm trả về bool kèm chuỗi error.",
                        "bool ValidateAge(int age, out string message) {\n    if (age < 18) { message = \"Chưa đủ 18 tuổi\"; return false; }\n    message = \"Hợp lệ\"; return true;\n}"),
                    S(3, "Từ khóa in (Tham chiếu chỉ đọc tối ưu bộ nhớ)",
                        "Đầu vào: Struct kích thước lớn -> Xử lý: Truyền bằng từ khóa in -> Đẩy kết quả: Không tốn chi phí copy struct và cấm sửa đổi.",
                        "void PrintPoint(in ReadOnlyPoint pt) {\n    // pt.X = 10; -> Lỗi biên dịch vì 'in' là chỉ đọc!\n    Console.WriteLine($\"{pt.X}, {pt.Y}\");\n}"),
                    S(4, "Từ khóa params (Số lượng tham số linh hoạt)",
                        "Đầu vào: Muốn tính tổng của 2, 3 hoặc N số -> Xử lý: Dùng params int[] numbers -> Đẩy kết quả: Gọi hàm tự nhiên không cần new int[].",
                        "int SumAll(params int[] numbers) => numbers.Sum();\nint total = SumAll(10, 20, 30, 40); // = 100")
                },
                "Sự khác biệt quan trọng nhất giữa tham số `ref` và tham số `out` trong C# là gì?",
                new List<string>
                {
                    "`ref` chỉ dùng cho kiểu `string`, còn `out` chỉ dùng cho kiểu `int`",
                    "Biến truyền vào `ref` phải được khởi tạo trước khi gọi hàm; còn biến `out` không cần khởi tạo trước nhưng bắt buộc phải được gán giá trị bên trong hàm",
                    "`out` không thể thay đổi giá trị của biến",
                    "`ref` và `out` giống hệt nhau 100%"
                },
                "Biến truyền vào `ref` phải được khởi tạo trước khi gọi hàm; còn biến `out` không cần khởi tạo trước nhưng bắt buộc phải được gán giá trị bên trong hàm",
                "`ref` là luồng 2 chiều (vào + ra), trong khi `out` là luồng đầu ra nên hàm nhận `out` bắt buộc phải gán giá trị trước khi kết thúc."
            ),

            BuildSubLesson(topicId, "2.3",
                "Nạp chồng phương thức (Method Overloading), Tham số mặc định & Named Arguments",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/named-and-optional-arguments",
                """
                Trong các thư viện như Entity Framework hay ASP.NET Core, bạn sẽ thấy một tên hàm xuất hiện nhiều lần với các bộ tham số khác nhau:
                • **Method Overloading (Nạp chồng hàm):** Các hàm có **cùng tên** nhưng **khác nhau về số lượng hoặc kiểu dữ liệu của tham số** (Lưu ý: chỉ khác kiểu trả về thì KHÔNG được coi là Overload).
                • **Default Parameters (Tham số mặc định):** Gán sẵn giá trị mặc định như `int pageSize = 10`.
                • **Named Arguments (Đối số định danh):** Ghi rõ `pageSize: 20` lúc gọi hàm giúp người đọc hiểu ngay ý nghĩa con số.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Method Overloading (Cùng tên, khác tham số)",
                        "Đầu vào: Nhu cầu tìm kiếm theo ID hoặc theo Tên -> Xử lý: Viết 2 hàm cùng tên FindUser -> Đẩy kết quả: Compiler tự chọn đúng hàm theo đối số truyền vào.",
                        "public User? FindUser(int id) => _users.FirstOrDefault(u => u.Id == id);\npublic User? FindUser(string email) => _users.FirstOrDefault(u => u.Email == email);"),
                    S(2, "Optional / Default Parameters",
                        "Đầu vào: Tham số phân trang thường cố định -> Xử lý: Đặt page = 1, pageSize = 10 ở cuối danh sách tham số -> Đẩy kết quả: Người gọi có thể lược bỏ.",
                        "public List<Course> GetCourses(string keyword, int page = 1, int pageSize = 10) {\n    return _courses.Where(c => c.Title.Contains(keyword)).ToList();\n}"),
                    S(3, "Gọi hàm với Named Arguments",
                        "Đầu vào: Hàm có nhiều tham số bool/int dễ nhầm lẫn -> Xử lý: Truyền theo cú pháp tên_tham_số: giá_trị -> Đẩy kết quả: Code tự giải thích cực rõ ràng.",
                        "var list = GetCourses(keyword: \"Web API\", pageSize: 25, page: 2);"),
                    S(4, "Kết hợp Overload gọi lẫn nhau (Chaining)",
                        "Đầu vào: Hàm ít tham số -> Xử lý: Gọi chuyển tiếp sang hàm đầy đủ tham số để không lặp lại logic -> Đẩy kết quả: Tuân thủ nguyên lý DRY (Don't Repeat Yourself).",
                        "public List<Course> GetCourses() => GetCourses(keyword: \"\", page: 1, pageSize: 10);")
                },
                "Hai phương thức trong cùng một class được coi là **Overload (Nạp chồng)** hợp lệ khi nào?",
                new List<string>
                {
                    "Khi chúng có cùng tên và khác nhau về số lượng hoặc kiểu dữ liệu của các tham số",
                    "Khi chúng có cùng tên, cùng tham số nhưng chỉ khác kiểu trả về (`int` và `string`)",
                    "Khi chúng có tên khác nhau hoàn toàn",
                    "Khi chúng nằm ở hai project khác nhau"
                },
                "Khi chúng có cùng tên và khác nhau về số lượng hoặc kiểu dữ liệu của các tham số",
                "Trình biên dịch C# phân biệt các hàm Overload dựa trên `Method Signature` (tên hàm + danh sách kiểu tham số), không dựa vào kiểu trả về."
            ),

            BuildSubLesson(topicId, "2.4",
                "Tuple, ValueTuple và Kỹ thuật Deconstruction (Trả về nhiều giá trị từ hàm)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/builtin-types/value-tuples",
                """
                Khi một hàm trong Service cần trả về 2–3 thông tin đi kèm nhau (ví dụ: `(bool IsSuccess, string Message, int OrderId)`) mà không muốn tạo thêm một class DTO riêng hoặc dùng `out`:
                • **`ValueTuple`:** Cú pháp `(Kiểu1 Tên1, Kiểu2 Tên2)` nhẹ nhàng vì là `struct` (Value Type trên Stack).
                • **Deconstruction (Phân rã):** Tách trực tiếp kết quả trả về thành các biến riêng biệt ngay tại dòng gọi hàm: `var (isSuccess, message) = await service.CreateAsync(...);`.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khai báo hàm trả về Named ValueTuple",
                        "Đầu vào: Hàm kiểm tra và tính học phí -> Xử lý: Đặt kiểu trả về là (bool IsValid, decimal FinalFee, string Note) -> Đẩy kết quả: Trả về bộ 3 giá trị.",
                        "public (bool IsValid, decimal FinalFee, string Note) CalculateFee(decimal baseFee, bool hasVoucher) {\n    if (baseFee <= 0) return (false, 0m, \"Học phí không hợp lệ\");\n    decimal fee = hasVoucher ? baseFee * 0.8m : baseFee;\n    return (true, fee, \"Áp dụng thành công\");\n}"),
                    S(2, "Truy cập qua tên thuộc tính của Tuple",
                        "Đầu vào: Biến nhận kết quả Tuple -> Xử lý: Chấm trực tiếp .IsValid, .FinalFee -> Đẩy kết quả: Rõ nghĩa như một DTO thực thụ.",
                        "var result = CalculateFee(1_000_000m, hasVoucher: true);\nConsole.WriteLine($\"{result.Note}: {result.FinalFee:N0}đ\");"),
                    S(3, "Kỹ thuật Deconstruction (Phân rã biến)",
                        "Đầu vào: Gọi hàm trả về Tuple -> Xử lý: Tách thẳng thành các biến cục bộ bằng var (a, b, c) -> Đẩy kết quả: Dùng ngay các biến trong hàm.",
                        "var (isValid, finalFee, note) = CalculateFee(2_000_000m, true);\nif (isValid) Console.WriteLine($\"Cần thanh toán: {finalFee}\");"),
                    S(4, "Bỏ qua giá trị không cần thiết với Discard (_)",
                        "Đầu vào: Chỉ cần lấy finalFee, không quan tâm note -> Xử lý: Đặt dấu _ ở vị trí không dùng -> Đẩy kết quả: Không tạo biến thừa.",
                        "var (ok, fee, _) = CalculateFee(1_500_000m, false);")
                },
                "Cú pháp `var (isSuccess, errorMessage) = ValidateOrder(order);` trong C# được gọi là kỹ thuật gì?",
                new List<string>
                {
                    "Boxing và Unboxing",
                    "Tuple Deconstruction (Phân rã ValueTuple thành các biến riêng biệt)",
                    "Kế thừa đa cấp",
                    "Ép kiểu tường minh"
                },
                "Tuple Deconstruction (Phân rã ValueTuple thành các biến riêng biệt)",
                "Tuple Deconstruction cho phép nhận nhiều giá trị trả về từ hàm vào thẳng các biến có tên rõ ràng mà không cần dùng tham số `out`."
            ),

            BuildSubLesson(topicId, "2.5",
                "Bản chất bất biến của Chuỗi (String Immutability), String Interning & Tối ưu với StringBuilder",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/base-types/stringbuilder",
                """
                Trong .NET, kiểu `string` là **Reference Type** nhưng có tính **Bất biến (Immutable)**:
                • Một khi chuỗi đã được tạo trên vùng nhớ Heap, nội dung của nó **không bao giờ thay đổi được**.
                • Mỗi khi bạn cộng chuỗi (`str += "abc"`), C# thực chất phải **cấp phát một đối tượng chuỗi hoàn toàn mới** trên Heap và copy dữ liệu cũ sang, bỏ chuỗi cũ cho GC dọn.
                • Vì vậy, nếu nối chuỗi trong vòng lặp hàng nghìn lần, bạn **bắt buộc phải dùng `StringBuilder`** để sửa đổi trên cùng một vùng đệm (Buffer).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Hiện tượng cấp phát mới khi cộng chuỗi (+=)",
                        "Đầu vào: Biến string s = \"Hoàng\" -> Xử lý: Thực hiện s += \" Việt\" -> Đẩy kết quả: Tạo vùng nhớ mới trên Heap chứa \"Hoàng Việt\", vùng nhớ \"Hoàng\" cũ thành rác.",
                        "string name = \"Hoàng\";\nname += \" Việt\"; // Tạo ra object string mới trên Heap!"),
                    S(2, "Thảm họa hiệu năng khi cộng chuỗi trong vòng lặp",
                        "Đầu vào: Vòng lặp 10.000 lần nối chuỗi CSV -> Xử lý: Dùng += tạo ra 10.000 đối tượng rác trên Heap -> Đẩy kết quả: Tốn RAM và làm chậm CPU.",
                        "// TRÁNH VIẾT TRONG VÒNG LẶP LỚN:\n// string report = \"\";\n// for (int i = 0; i < 10000; i++) report += i + \",\";"),
                    S(3, "Giải pháp tối ưu: Sử dụng StringBuilder",
                        "Đầu vào: Nối chuỗi nhiều lần -> Xử lý: StringBuilder giữ một mảng ký tự (char buffer) có thể mở rộng mà không tạo chuỗi rác -> Đẩy kết quả: Nhanh gấp hàng trăm lần.",
                        "var sb = new System.Text.StringBuilder();\nfor (int i = 1; i <= 1000; i++) {\n    sb.AppendLine($\"Dòng báo cáo số #{i}\");\n}"),
                    S(4, "Xuất kết quả cuối cùng bằng .ToString()",
                        "Đầu vào: Buffer của StringBuilder đã hoàn tất -> Xử lý: Gọi sb.ToString() đúng 1 lần ở cuối -> Đẩy kết quả: Sinh ra chuỗi kết quả duy nhất.",
                        "string finalReport = sb.ToString();")
                },
                "Tại sao trong C#, khi cần nối chuỗi liên tục bên trong một vòng lặp lớn, bạn nên dùng `StringBuilder` thay vì toán tử `+=` trên kiểu `string`?",
                new List<string>
                {
                    "Vì kiểu `string` không dùng được trong vòng lặp `for`",
                    "Vì `string` có tính bất biến (Immutable), mỗi phép `+=` sẽ tạo ra một đối tượng chuỗi mới trên Heap gây tốn bộ nhớ, còn `StringBuilder` thao tác trên một bộ đệm động",
                    "Vì `StringBuilder` tự động lưu dữ liệu vào SQL Server",
                    "Vì toán tử `+=` chỉ nối được tối đa 3 ký tự"
                },
                "Vì `string` có tính bất biến (Immutable), mỗi phép `+=` sẽ tạo ra một đối tượng chuỗi mới trên Heap gây tốn bộ nhớ, còn `StringBuilder` thao tác trên một bộ đệm động",
                "`StringBuilder` thay đổi trực tiếp trên bộ đệm ký tự nội bộ và chỉ cấp phát chuỗi kết quả một lần khi gọi `.ToString()`."
            ),

            BuildSubLesson(topicId, "2.6",
                "Các hàm xử lý chuỗi thực chiến (Split, Join, Trim, Substring, Replace, StringComparison)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/how-to/compare-strings",
                """
                Xử lý dữ liệu văn bản (email, số điện thoại, mã đơn hàng, từ khóa tìm kiếm) chiếm phần lớn công việc chuẩn hóa dữ liệu ở tầng Service:
                • **`string.IsNullOrWhiteSpace(input)`:** Kiểm tra chuỗi vừa khác `null`, vừa không rỗng `""`, vừa không phải toàn dấu cách `"   "`.
                • **`Trim()` / `Split()` / `string.Join()`:** Cắt khoảng trắng thừa hai đầu, tách chuỗi thành mảng và ghép mảng thành chuỗi.
                • **`StringComparison.OrdinalIgnoreCase`:** So sánh hai chuỗi không phân biệt chữ hoa/thường với hiệu năng cao nhất (không cần gọi `.ToLower()` tạo chuỗi phụ).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Kiểm tra rỗng bằng IsNullOrWhiteSpace",
                        "Đầu vào: Chuỗi người dùng gửi từ form -> Xử lý: Kiểm tra null, rỗng hoặc chỉ toàn khoảng trắng -> Đẩy kết quả: Chặn dữ liệu rác ngay đầu hàm.",
                        "if (string.IsNullOrWhiteSpace(rawEmail)) {\n    throw new ArgumentException(\"Email không được để trống\");\n}"),
                    S(2, "Chuẩn hóa với Trim() và Replace()",
                        "Đầu vào: Chuỗi có khoảng trắng thừa ở 2 đầu -> Xử lý: Gọi Trim() -> Đẩy kết quả: Chuỗi sạch sẽ trước khi lưu Database.",
                        "string cleanEmail = rawEmail.Trim();\nstring slug = \"Học C# Web API\".Trim().Replace(\" \", \"-\").ToLowerInvariant(); // \"học-c#-web-api\""),
                    S(3, "Tách và Ghép chuỗi bằng Split & string.Join",
                        "Đầu vào: Chuỗi danh sách tag ngăn cách bởi dấu phẩy -> Xử lý: Tách bằng Split(','), chuẩn hóa rồi ghép lại bằng string.Join -> Đẩy kết quả: Mảng/Chuỗi chuẩn.",
                        "string rawTags = \"CSharp, DotNet10, WebAPI\";\nstring[] tags = rawTags.Split(',', StringSplitOptions.TrimEntries);\nstring joined = string.Join(\" | \", tags); // \"CSharp | DotNet10 | WebAPI\""),
                    S(4, "So sánh không phân biệt hoa thường tối ưu",
                        "Đầu vào: Kiểm tra mã quyền hoặc email -> Xử lý: Dùng Equals với StringComparison.OrdinalIgnoreCase -> Đẩy kết quả: So sánh chính xác, không cấp phát chuỗi tạm.",
                        "bool isAdmin = roleName.Equals(\"admin\", StringComparison.OrdinalIgnoreCase);")
                },
                "Phương thức nào dưới đây trả về `true` cho cả 3 trường hợp: biến chuỗi là `null`, chuỗi rỗng `\"\"`, hoặc chuỗi chỉ chứa toàn dấu cách `\"   \"`?",
                new List<string>
                {
                    "`string.IsNullOrEmpty(input)`",
                    "`string.IsNullOrWhiteSpace(input)`",
                    "`input == \"\"`",
                    "`input.Trim()`"
                },
                "`string.IsNullOrWhiteSpace(input)`",
                "`string.IsNullOrWhiteSpace` kiểm tra đồng thời `null`, chuỗi rỗng `\"\"` và chuỗi chỉ toàn ký tự khoảng trắng (`' '`, `'\\t'`, `'\\n'`)."
            ),

            BuildSubLesson(topicId, "2.7",
                "Cấu trúc dữ liệu Mảng (Array, Jagged Array) và Thao tác cắt mảng không cấp phát bộ nhớ với Span<T>",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/memory-and-spans/memory-t-usage-guidelines",
                """
                Mảng (`T[]`) là cấu trúc lưu trữ các phần tử liên tiếp nhau trong bộ nhớ với kích thước cố định:
                • **Truy cập theo chỉ số $O(1)$:** Nhờ nằm liền kề trong RAM, việc đọc `arr[i]` diễn ra tức thì.
                • **Index từ cuối mảng (`^`) & Range (`..`):** `arr[^1]` là phần tử cuối cùng; `arr[1..4]` lấy từ vị trí 1 đến 3.
                • **`Span<T>` / `ReadOnlySpan<char>`:** Cho phép tạo một "cửa sổ nhìn (slice)" vào một phần của mảng hoặc chuỗi mà **không hề copy hay cấp phát mảng/chuỗi mới trên Heap**.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khai báo Mảng & Truy cập với toán tử ^ (Index from end)",
                        "Đầu vào: Mảng cố định các mốc điểm -> Xử lý: Dùng [0] lấy đầu, [^1] lấy phần tử cuối -> Đẩy kết quả: Không cần viết arr[arr.Length - 1].",
                        "int[] milestones = { 10, 25, 50, 75, 100 };\nint first = milestones[0];  // 10\nint last  = milestones[^1]; // 100 (phần tử cuối cùng)"),
                    S(2, "Cắt mảng với Range Operator (..)",
                        "Đầu vào: Mảng gốc -> Xử lý: Dùng cú pháp [start..end] (end không bao gồm) -> Đẩy kết quả: Lấy ra đoạn phần tử mong muốn.",
                        "int[] middle = milestones[1..4]; // { 25, 50, 75 }"),
                    S(3, "Tạo cửa sổ bộ nhớ Zero-Allocation với Span<T>",
                        "Đầu vào: Mảng lớn cần xử lý 1 đoạn con -> Xử lý: Dùng Span<int> trỏ trực tiếp vào vùng nhớ mảng gốc -> Đẩy kết quả: Không tốn 1 byte Heap nào.",
                        "Span<int> window = milestones.AsSpan(1, 3);\nwindow[0] = 30; // Sửa trực tiếp vào milestones[1] gốc mà không tạo mảng phụ!"),
                    S(4, "Tách chuỗi siêu tốc với ReadOnlySpan<char>",
                        "Đầu vào: Chuỗi mã đơn hàng \"ORD-2026-9981\" -> Xử lý: Cắt lấy năm bằng AsSpan().Slice() thay vì Substring() -> Đẩy kết quả: Không sinh chuỗi phụ trên Heap.",
                        "ReadOnlySpan<char> codeSpan = \"ORD-2026-9981\".AsSpan();\nReadOnlySpan<char> yearSpan = codeSpan.Slice(4, 4); // \"2026\"")
                },
                "Trong C# hiện đại, biểu thức `items[^1]` dùng để lấy phần tử nào trong mảng hoặc danh sách `items`?",
                new List<string>
                {
                    "Phần tử đầu tiên (index 0)",
                    "Phần tử cuối cùng của mảng/danh sách (tương đương `items[items.Length - 1]`)",
                    "Phần tử ở chính giữa mảng",
                    "Ném ra lỗi IndexOutOfRangeException"
                },
                "Phần tử cuối cùng của mảng/danh sách (tương đương `items[items.Length - 1]`)",
                "Toán tử `^` đếm từ cuối mảng ngược lên (`^1` là phần tử cuối cùng, `^2` là phần tử kế cuối)."
            ),

            BuildSubLesson(topicId, "2.8",
                "Danh sách động List<T>: Cơ chế tự mở rộng Capacity và Độ phức tạp thuật toán",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.collections.generic.list-1",
                """
                `List<T>` là cấu trúc dữ liệu được sử dụng nhiều nhất trong C#. Bên dưới `List<T>` thực chất là một mảng `T[]` tự động mở rộng:
                • **`Count` vs `Capacity`:** `Count` là số phần tử thực tế đang có; `Capacity` là kích thước mảng nội bộ đã cấp phát sẵn. Khi `Count` vượt `Capacity`, `List<T>` tự tạo mảng mới **gấp đôi kích thước** và copy sang.
                • **Độ phức tạp:**
                  - Đọc theo index `list[i]` hoặc `Add()` vào cuối: **$O(1)$** (cực nhanh).
                  - `Insert(0, item)` vào đầu hoặc `Contains()`/`Find()` duyệt tuần tự: **$O(n)$**.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khởi tạo List<T> (Với Capacity định trước nếu biết kích thước)",
                        "Đầu vào: Biết trước cần chứa khoảng 100 phần tử -> Xử lý: Truyền capacity = 100 vào Constructor -> Đẩy kết quả: Tránh việc List phải nhân đôi mảng nhiều lần.",
                        "var lessons = new List<string>(capacity: 100) {\n    \"Bài 1.1: CLR & JIT\",\n    \"Bài 1.2: Kiểu dữ liệu\"\n};"),
                    S(2, "Thêm và Truy cập phần tử O(1)",
                        "Đầu vào: Phần tử mới -> Xử lý: Gọi Add() hoặc AddRange() -> Đẩy kết quả: Đưa vào cuối danh sách cực nhanh.",
                        "lessons.Add(\"Bài 1.3: Stack vs Heap\");\nstring firstLesson = lessons[0]; // Truy cập theo index O(1)"),
                    S(3, "Tìm kiếm và Lọc dữ liệu trên List<T>",
                        "Đầu vào: Điều kiện Predicate -> Xử lý: Dùng Find(), Exists() hoặc RemoveAll() -> Đẩy kết quả: Thao tác gọn gàng bằng biểu thức Lambda.",
                        "bool hasStackLesson = lessons.Exists(x => x.Contains(\"Stack\"));\nlessons.RemoveAll(x => string.IsNullOrWhiteSpace(x));"),
                    S(4, "Trả về IReadOnlyList<T> để bảo vệ dữ liệu",
                        "Đầu vào: Danh sách nội bộ của class -> Xử lý: Trả ra ngoài qua giao diện IReadOnlyList<T> -> Đẩy kết quả: Bên ngoài chỉ đọc được, không thể tự ý Add/Clear.",
                        "public IReadOnlyList<string> GetPublishedLessons() => lessons.AsReadOnly();")
                },
                "Trong `List<T>`, sự khác biệt giữa hai thuộc tính `Count` và `Capacity` là gì?",
                new List<string>
                {
                    "`Count` và `Capacity` luôn luôn bằng nhau",
                    "`Count` là số lượng phần tử thực tế đang có trong danh sách, còn `Capacity` là tổng sức chứa của mảng nội bộ trước khi phải cấp phát mở rộng",
                    "`Capacity` là số phần tử đã bị xóa",
                    "`Count` chỉ đếm các số chẵn"
                },
                "`Count` là số lượng phần tử thực tế đang có trong danh sách, còn `Capacity` là tổng sức chứa của mảng nội bộ trước khi phải cấp phát mở rộng",
                "Khi `Count` chạm mức `Capacity`, lần `Add` tiếp theo sẽ khiến `List<T>` tự cấp phát mảng nội bộ mới lớn gấp đôi."
            ),

            BuildSubLesson(topicId, "2.9",
                "Bảng băm Dictionary<TKey, TValue> và Tập hợp duy nhất HashSet<T> (Tra cứu cực nhanh O(1))",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.collections.generic.dictionary-2",
                """
                Khi cần tra cứu dữ liệu theo khóa (ID, Mã code, Email) hoặc kiểm tra trùng lặp trên tập dữ liệu lớn, **đừng dùng `List.Contains()` ($O(n)$)** mà hãy dùng cấu trúc bảng băm (Hash Table) với tốc độ **$O(1)$**:
                • **`Dictionary<TKey, TValue>`:** Lưu cặp `Key -> Value` (Key là duy nhất). Luôn dùng **`TryGetValue(key, out var val)`** để lấy dữ liệu an toàn trong 1 bước băm duy nhất.
                • **`HashSet<T>`:** Tập hợp chỉ chứa các phần tử **không trùng lặp**, kiểm tra `set.Contains(x)` đạt tốc độ tức thì $O(1)$.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Khởi tạo & Thêm vào Dictionary<TKey, TValue>",
                        "Đầu vào: Cặp Mã bài học -> Điểm số -> Xử lý: Dùng indexer dict[key] = value (tự thêm mới hoặc cập nhật nếu đã có) -> Đẩy kết quả: Bảng tra cứu O(1).",
                        "var scoreByLesson = new Dictionary<string, int> {\n    [\"1.1\"] = 100,\n    [\"1.2\"] = 95\n};\nscoreByLesson[\"1.3\"] = 90; // An toàn: Thêm mới hoặc ghi đè nếu đã tồn tại"),
                    S(2, "Tra cứu chuẩn xác bằng TryGetValue",
                        "Đầu vào: Cần lấy điểm của bài \"1.2\" -> Xử lý: Gọi TryGetValue thay vì gọi ContainsKey rồi mới đọc dict[key] -> Đẩy kết quả: Chỉ tốn 1 lần tra bảng băm, không sợ KeyNotFoundException.",
                        "if (scoreByLesson.TryGetValue(\"1.2\", out int score)) {\n    Console.WriteLine($\"Điểm bài 1.2: {score}\");\n}"),
                    S(3, "Loại bỏ trùng lặp tức thì với HashSet<T>",
                        "Đầu vào: Danh sách email có thể bị trùng -> Xử lý: Đưa vào HashSet<string> (với StringComparer.OrdinalIgnoreCase) -> Đẩy kết quả: Tập hợp duy nhất, kiểm tra Contains cực nhanh.",
                        "var uniqueEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);\nuniqueEmails.Add(\"viet@dev.vn\");\nbool isAdded = uniqueEmails.Add(\"VIET@dev.vn\"); // false vì đã tồn tại!"),
                    S(4, "So sánh hiệu năng: List O(n) vs HashSet O(1)",
                        "Đầu vào: Kiểm tra 10.000 mã sản phẩm -> Xử lý: Chuyển danh sách mã sang HashSet trước vòng lặp -> Đẩy kết quả: Tăng tốc từ hàng giây xuống vài mili-giây.",
                        "HashSet<int> validIds = productIds.ToHashSet();\nvar matched = orders.Where(o => validIds.Contains(o.ProductId)).ToList(); // O(1) lookup!")
                },
                "Khi lấy giá trị từ `Dictionary<string, User>`, tại sao bạn nên dùng `dict.TryGetValue(key, out var user)` thay vì truy cập trực tiếp `dict[key]`?",
                new List<string>
                {
                    "Vì `dict[key]` sẽ ném ra ngoại lệ `KeyNotFoundException` nếu `key` không tồn tại, còn `TryGetValue` kiểm tra và lấy giá trị an toàn chỉ trong 1 lần tra cứu",
                    "Vì `dict[key]` là cú pháp đã bị xóa khỏi C#",
                    "Vì `TryGetValue` tự động sắp xếp Dictionary theo thứ tự bảng chữ cái",
                    "Vì `TryGetValue` xóa luôn phần tử sau khi đọc"
                },
                "Vì `dict[key]` sẽ ném ra ngoại lệ `KeyNotFoundException` nếu `key` không tồn tại, còn `TryGetValue` kiểm tra và lấy giá trị an toàn chỉ trong 1 lần tra cứu",
                "`TryGetValue` vừa tránh lỗi `KeyNotFoundException` vừa nhanh gấp đôi so với việc gọi `ContainsKey(key)` rồi mới gọi `dict[key]`."
            ),

            BuildSubLesson(topicId, "2.10",
                "Hàng đợi Queue<T> (FIFO) và Ngăn xếp Stack<T> (LIFO) trong bài toán thực tế",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.collections.generic.queue-1",
                """
                Hai cấu trúc điều phối thứ tự xử lý kinh điển trong kỹ thuật phần mềm:
                • **`Queue<T>` (Hàng đợi - First In, First Out - Vào trước Ra trước):** Giống hàng người xếp hàng mua vé. Dùng để xử lý hàng đợi gửi Email, thông báo, Background Job (`Enqueue` để thêm vào cuối, `Dequeue`/`TryDequeue` để lấy ra từ đầu).
                • **`Stack<T>` (Ngăn xếp - Last In, First Out - Vào sau Ra trước):** Giống chồng đĩa. Dùng cho tính năng **Undo/Redo (Hoàn tác)**, lịch sử nút Back trình duyệt, hoặc duyệt cây (`Push` để đặt lên đỉnh, `Pop`/`TryPop` để lấy từ đỉnh).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Queue<T> (FIFO): Xếp hàng gửi Email",
                        "Đầu vào: Các yêu cầu gửi email xác nhận -> Xử lý: Gọi Enqueue() để xếp vào cuối hàng đợi -> Đẩy kết quả: Đảm bảo ai đăng ký trước được gửi trước.",
                        "var emailQueue = new Queue<string>();\nemailQueue.Enqueue(\"user1@hoangviet.vn\");\nemailQueue.Enqueue(\"user2@hoangviet.vn\");"),
                    S(2, "Xử lý tuần tự Queue với TryDequeue",
                        "Đầu vào: Hàng đợi emailQueue -> Xử lý: Lấy từng phần tử đầu hàng ra bằng TryDequeue -> Đẩy kết quả: Xử lý hết hàng đợi an toàn.",
                        "while (emailQueue.TryDequeue(out string? nextEmail)) {\n    Console.WriteLine($\"Đang gửi mail tới: {nextEmail}\");\n}"),
                    S(3, "Stack<T> (LIFO): Lưu lịch sử thao tác Undo",
                        "Đầu vào: Các thao tác chỉnh sửa của người dùng -> Xử lý: Gọi Push() đẩy thao tác mới nhất lên đỉnh Stack -> Đẩy kết quả: Thao tác gần nhất nằm trên cùng.",
                        "var undoStack = new Stack<string>();\nundoStack.Push(\"Gõ chữ: Xin chào\");\nundoStack.Push(\"Đổi màu chữ: Cam\");"),
                    S(4, "Hoàn tác (Undo) với TryPop / TryPeek",
                        "Đầu vào: Người dùng bấm Ctrl+Z -> Xử lý: Gọi TryPop() lấy thao tác mới nhất ra khỏi đỉnh Stack -> Đẩy kết quả: Hoàn tác đúng bước vừa làm.",
                        "if (undoStack.TryPop(out string? lastAction)) {\n    Console.WriteLine($\"Hoàn tác bước: {lastAction}\"); // \"Đổi màu chữ: Cam\"\n}")
                },
                "Nếu bạn cần xây dựng chức năng **Hoàn tác (Undo / Ctrl+Z)** để lấy ra thao tác **vừa mới thực hiện gần nhất**, bạn nên chọn cấu trúc dữ liệu nào?",
                new List<string>
                {
                    "`Queue<T>` (Vào trước - Ra trước)",
                    "`Stack<T>` (Vào sau - Ra trước / LIFO)",
                    "`Dictionary<int, int>`",
                    "`int[]`"
                },
                "`Stack<T>` (Vào sau - Ra trước / LIFO)",
                "`Stack<T>` hoạt động theo nguyên tắc LIFO (Last-In, First-Out): thao tác vừa `Push` vào sau cùng sẽ được `Pop` ra đầu tiên khi bấm Undo."
            ),

            BuildSubLesson(topicId, "2.11",
                "Phân biệt các Interface bộ sưu tập: IEnumerable<T>, ICollection<T>, IList<T> và IReadOnlyList<T>",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/collections/",
                """
                Khi đọc chữ ký hàm trong các dự án công ty, bạn sẽ thấy người ta trả về các Interface thay vì trả thẳng `List<T>`:
                • **`IEnumerable<T>`:** Cấp thấp nhất — chỉ hỗ trợ **duyệt tuần tự (`foreach`)** và truy vấn LINQ. Không thể `.Add()`, không có `.Count` tức thì.
                • **`IReadOnlyCollection<T>` / `IReadOnlyList<T>`:** Đã nằm sẵn trong bộ nhớ, có `.Count` và truy cập chỉ số `[i]`, nhưng **cấm thêm/sửa/xóa** (rất chuẩn để làm kiểu trả về của Service/Repository).
                • **`ICollection<T>`:** Cho phép `.Add()`, `.Remove()`, `.Count` (thường dùng cho Navigation Property trong Entity Framework như `public ICollection<SubLesson> SubLessons`).
                • **`IList<T>`:** Đầy đủ nhất — vừa thêm/xóa được, vừa truy cập/sửa theo vị trí `list[i]`.
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "IEnumerable<T> (Chỉ duyệt tuần tự)",
                        "Đầu vào: Tham số đầu vào của hàm chỉ cần đọc qua foreach -> Xử lý: Nhận IEnumerable<T> -> Đẩy kết quả: Chấp nhận cả Mảng, List, HashSet hay kết quả LINQ.",
                        "public decimal SumPrices(IEnumerable<decimal> prices) {\n    decimal total = 0m;\n    foreach (var p in prices) total += p;\n    return total;\n}"),
                    S(2, "ICollection<T> (Dùng trong Entity của EF Core)",
                        "Đầu vào: Quan hệ 1-Nhiều giữa Topic và SubLesson -> Xử lý: Khai báo ICollection<SubLesson> -> Đẩy kết quả: Cho phép EF Core theo dõi và Add/Remove bài học con.",
                        "public class Topic {\n    public int Id { get; set; }\n    public ICollection<SubLesson> SubLessons { get; set; } = new List<SubLesson>();\n}"),
                    S(3, "IReadOnlyList<T> (Chuẩn trả về từ Service)",
                        "Đầu vào: Danh sách DTO trả về cho Controller -> Xử lý: Trả về IReadOnlyList<ProductDto> -> Đẩy kết quả: Controller đọc được Count và [i] nhưng không sửa được danh sách.",
                        "public async Task<IReadOnlyList<ProductDto>> GetAllAsync() {\n    return await _db.Products.Select(p => new ProductDto(p.Id, p.Name)).ToListAsync();\n}"),
                    S(4, "IList<T> (Thao tác đầy đủ theo Index)",
                        "Đầu vào: Cần chèn/xóa/sửa phần tử tại vị trí i cụ thể -> Xử lý: Dùng IList<T> hoặc List<T> -> Đẩy kết quả: Hỗ trợ InsertAt, RemoveAt, list[i] = val.",
                        "IList<string> steps = new List<string> { \"Bước 1\", \"Bước 3\" };\nsteps.Insert(1, \"Bước 2\");")
                },
                "Trong các tầng Service/Repository, tại sao các lập trình viên Senior thường chọn `IReadOnlyList<T>` làm kiểu trả về cho danh sách dữ liệu đã truy vấn xong?",
                new List<string>
                {
                    "Vì `IReadOnlyList<T>` cho phép nơi gọi xóa toàn bộ dữ liệu trong Database",
                    "Vì nó cung cấp sẵn `.Count` và truy cập `[index]` nhanh chóng, đồng thời ngăn chặn tầng trên vô tình gọi `.Add()` hoặc `.Clear()` làm hỏng dữ liệu",
                    "Vì `List<T>` không thể chứa kiểu `class`",
                    "Vì `IReadOnlyList<T>` tự động mã hóa mật khẩu"
                },
                "Vì nó cung cấp sẵn `.Count` và truy cập `[index]` nhanh chóng, đồng thời ngăn chặn tầng trên vô tình gọi `.Add()` hoặc `.Clear()` làm hỏng dữ liệu",
                "`IReadOnlyList<T>` thể hiện rõ ý đồ thiết kế: danh sách đã được tải vào bộ nhớ (không bị đánh giá lại nhiều lần như `IEnumerable<T>`) và được bảo vệ chỉ đọc."
            )
        };
    }

    // =========================================================================
    // BÀI HỌC KHỞI ĐẦU CHO CÁC CHỦ ĐỀ TIẾP THEO (CHỦ ĐỀ 3..5 MENU 1 & 3..16 MENU 2)
    // Có thể cập nhật/bổ sung thêm bất kỳ lúc nào qua màn hình Admin/ImportTopic
    // =========================================================================
    public static List<SubLesson> GetStarterLessonsForTopic(int topicId, int topicNumber, string topicTitle, string skillCategory)
    {
        string code1 = $"{topicNumber}.1";
        string code2 = $"{topicNumber}.2";

        return new List<SubLesson>
        {
            BuildSubLesson(topicId, code1,
                $"Tổng quan kiến trúc & Tư duy cốt lõi: {topicTitle}",
                skillCategory,
                "https://learn.microsoft.com/vi-vn/dotnet/",
                $"""
                Chào mừng bạn đến với **{topicTitle}**.
                • Bài học mở đầu này giúp bạn nắm vững bức tranh tổng thể, vai trò của chủ đề **{topicTitle}** trong dự án thực tế tại doanh nghiệp và cách các thành phần phối hợp với nhau.
                • Hãy đọc kỹ từng Hộp Bước Tương Tác bên dưới để nắm rõ luồng đi từ Đầu vào (Input) -> Xử lý (Process) -> Đầu ra (Output).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Xác định bài toán & Đầu vào",
                        $"Đầu vào: Yêu cầu nghiệp vụ thuộc phạm vi {topicTitle} -> Xử lý: Phân tích các thành phần tham gia -> Đẩy kết quả: Xác định đúng mắt xích cần đọc/viết.",
                        $"// Chủ đề {topicNumber}: {topicTitle}\n// Bước 1: Xác định rõ Input (Tham số, Request, Cấu hình)"),
                    S(2, "Thiết kế & Tổ chức mắt xích C#",
                        "Đầu vào: Quy tắc kiến trúc chuẩn .NET 10 -> Xử lý: Tách biệt trách nhiệm giữa Interface, Service và Data -> Đẩy kết quả: Mã nguồn sạch, dễ kiểm thử.",
                        "public interface ITopicHandler {\n    Task<bool> ExecuteAsync(CancellationToken ct = default);\n}"),
                    S(3, "Thực thi & Xử lý luồng dữ liệu",
                        "Đầu vào: Dữ liệu đã được kiểm tra hợp lệ -> Xử lý: Thực thi bất đồng bộ với async/await và ghi log -> Đẩy kết quả: Trả về kết quả chuẩn hóa.",
                        "public async Task<bool> ExecuteAsync(CancellationToken ct = default) {\n    await Task.CompletedTask;\n    return true;\n}"),
                    S(4, "Kiểm thử & Trả về kết quả",
                        "Đầu vào: Kết quả thực thi -> Xử lý: Kiểm tra mã trạng thái và đóng gói DTO -> Đẩy kết quả: Phản hồi an toàn cho tầng gọi.",
                        "return Ok(new { success = true });")
                },
                $"Khi làm việc với chủ đề \"{topicTitle}\" trong dự án thực tế, nguyên tắc nào giúp bạn đọc hiểu và bảo trì code nhanh nhất?",
                new List<string>
                {
                    "Viết toàn bộ mọi thứ vào chung một hàm duy nhất",
                    "Nắm rõ Đầu vào (Input) -> Xử lý (Process) -> Đầu ra (Output) của từng mắt xích và tuân thủ phân tầng trách nhiệm",
                    "Bỏ qua việc kiểm tra dữ liệu đầu vào",
                    "Không sử dụng bất kỳ tài liệu chính thức nào"
                },
                "Nắm rõ Đầu vào (Input) -> Xử lý (Process) -> Đầu ra (Output) của từng mắt xích và tuân thủ phân tầng trách nhiệm",
                "Đọc hiểu theo luồng Input -> Process -> Output giúp bạn làm chủ mọi chủ đề từ C# cơ bản đến kiến trúc hệ thống lớn."
            ),

            BuildSubLesson(topicId, code2,
                $"Thực hành đọc code & Checklist ứng dụng thực tế: {topicTitle}",
                skillCategory,
                "https://learn.microsoft.com/vi-vn/aspnet/core/",
                $"""
                Sau khi nắm khái niệm của **{topicTitle}**, bước tiếp theo là rèn luyện phản xạ đọc code thực tế và kiểm tra các lỗi thường gặp:
                • **Checklist đọc code:** Kiểm tra chữ ký hàm (Method Signature), kiểu trả về, cách xử lý `null` và ngoại lệ.
                • **Checklist kiểm thử:** Luôn thử nghiệm với cả dữ liệu hợp lệ (Happy Case) và dữ liệu biên/lỗi (Edge Case).
                """,
                new List<InteractiveFlowStep>
                {
                    S(1, "Đọc chữ ký hàm & Kiểu dữ liệu",
                        "Đầu vào: File mã nguồn trong dự án -> Xử lý: Quan sát tham số đầu vào và kiểu trả về -> Đẩy kết quả: Hiểu ngay mục đích của hàm mà chưa cần đọc chi tiết từng dòng.",
                        "// Luôn đọc Method Signature trước:\npublic async Task<ResultDto> HandleFeatureAsync(RequestDto request)"),
                    S(2, "Kiểm tra Guard Clauses & Validation",
                        "Đầu vào: Tham số request -> Xử lý: Đảm bảo các điều kiện biên được chặn sớm ở đầu hàm -> Đẩy kết quả: Không để dữ liệu lỗi đi sâu vào Database.",
                        "if (request is null) throw new ArgumentNullException(nameof(request));"),
                    S(3, "Theo dõi luồng gọi hàm (Call Hierarchy)",
                        "Đầu vào: Điểm gọi hàm -> Xử lý: Dùng Ctrl + Click (Go to Implementation) để lần theo luồng -> Đẩy kết quả: Nắm trọn vẹn đường đi của dữ liệu.",
                        "var output = await _service.ProcessAsync(request);"),
                    S(4, "Đối chiếu kết quả & Hoàn thành",
                        "Đầu vào: Kết quả trả về -> Xử lý: Kiểm tra trạng thái và log -> Đẩy kết quả: Sẵn sàng áp dụng vào task thực tập.",
                        "return ResultDto.Success(output);")
                },
                "Kỹ thuật nào trên Visual Studio / VS Code giúp bạn nhảy nhanh từ nơi gọi hàm (Interface) sang nơi viết code thực tế (Class Implementation)?",
                new List<string>
                {
                    "Tắt màn hình máy tính",
                    "Sử dụng Go to Implementation (Ctrl + F12 hoặc Ctrl + Click) để lần theo luồng thực thi",
                    "Xóa file Program.cs",
                    "Đổi tên toàn bộ biến thành a, b, c"
                },
                "Sử dụng Go to Implementation (Ctrl + F12 hoặc Ctrl + Click) để lần theo luồng thực thi",
                "`Go to Implementation` là công cụ quan trọng nhất khi đọc code dự án sử dụng Dependency Injection và Interface."
            )
        };
    }
}
