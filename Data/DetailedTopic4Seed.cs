using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic4Seed
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
            // BÀI 4.1
            // =====================================================================
            Build(
                topicId,
                "4.1",
                "Bản chất Ngoại lệ (Exception), Cấu trúc Cây phân cấp Exception và đọc hiểu StackTrace",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/exceptions/",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Hãy phân biệt rõ **Lỗi biên dịch (Compile-time Error)** và **Lỗi ngoại lệ lúc chạy (Runtime Exception)**:
                    + *Compile-time Error:* Bạn gõ thiếu dấu chấm phẩy `;` hoặc sai tên biến — Visual Studio gạch đỏ ngay lập tức, không cho bấm chạy.
                    + *Exception (Ngoại lệ):* Code viết đúng ngữ pháp 100%, bấm chạy lên bình thường. Nhưng đang chạy thì gặp "tai nạn bất ngờ" (như chia một số cho `0`, mở một file không tồn tại, mất mạng đột ngột khi đang gọi SQL Server). Nếu không có túi khí bảo vệ, chương trình sẽ **văng (Crash) ngay lập tức**!
                • **🖼️ 2. Sơ đồ Cây Phân Cấp Exception & Giải phẫu đối tượng `System.Exception`:**
                ```
                                      ┌────────────────────────┐
                                      │    System.Exception    │ (Lớp tổ tiên của mọi lỗi trong .NET)
                                      └───────────┬────────────┘
                                                  │
                       ┌──────────────────────────┴──────────────────────────┐
                       ▼                                                     ▼
                ┌─────────────────────────────┐               ┌─────────────────────────────┐
                │       SystemException       │               │     ApplicationException    │
                │ (Lỗi do CLR/.NET ném ra)    │               │ (Lỗi nghiệp vụ tự định nghĩa)│
                └──────────────┬──────────────┘               └─────────────────────────────┘
                               ├── NullReferenceException (Chấm . từ biến đang null)
                               ├── IndexOutOfRangeException (Truy cập quá kích thước mảng)
                               ├── DivideByZeroException (Chia số nguyên cho 0)
                               ├── FormatException (Ép kiểu chuỗi chữ sang số bị sai)
                               └── InvalidOperationException (Thao tác sai trạng thái đối tượng)
                ```
                • **📊 3. Bảng 3 Thuộc Tính Quan Trọng Nhất Của Một `Exception` (Dùng để bắt bệnh):**
                | Thuộc tính | Ý nghĩa chi tiết | Ví dụ kết quả thực tế |
                | :--- | :--- | :--- |
                | **`ex.Message`** | Mô tả ngắn gọn nguyên nhân vì sao bị lỗi. | `"Attempted to divide by zero."` |
                | **`ex.StackTrace`** | **Bản đồ hiện trường vụ tai nạn:** Chỉ rõ lỗi nổ ra ở **File `.cs` nào**, **Hàm nào**, và chính xác **Dòng code số mấy (`line X`)**! | `at HoangViet.OrderService.Calculate() in C:\...\OrderService.cs:line 42` |
                | **`ex.InnerException`**| Lỗi gốc rễ nằm sâu bên trong (rất hay gặp khi EF Core lưu DB bị lỗi khóa ngoại SQL). | Chứa lỗi `SqlException` thực sự bên dưới lớp vỏ `DbUpdateException`. |
                • **⚠️ 4. Kỹ năng đọc `StackTrace` từ dưới lên hay từ trên xuống?**
                  - Trong chuỗi `StackTrace`, **dòng đầu tiên trên cùng** chính là nơi lỗi trực tiếp phát nổ, còn các dòng bên dưới là lịch sử các hàm đã gọi dẫn tới hàm đó!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Sự cố xảy ra tại thời điểm chạy (Runtime)",
                        "📥 Đầu vào: Phép chia hai số nguyên a = 10, b = 0. ⚙️ Cơ chế ngầm: CPU không thể thực hiện phép chia cho 0, CLR lập tức tạm dừng luồng chạy bình thường và khởi tạo đối tượng DivideByZeroException trên Heap. 📤 Đầu ra: Đối tượng Exception chứa đầy đủ thông tin hiện trường.",
                        "int a = 10, b = 0;\nint result = a / b; // Nổ ra đối tượng DivideByZeroException tại dòng này!"),
                    Step(2,
                        "2. Cơ chế lan truyền ngược (Exception Bubbling)",
                        "📥 Đầu vào: Exception vừa nổ ở Bước 1. ⚙️ Cơ chế ngầm: Nếu hàm hiện tại không có khối try-catch để đỡ lỗi, Exception sẽ bắn ngược lên hàm đã gọi nó (ví dụ từ Repository -> bắn lên Service -> bắn lên Controller -> bắn ra màn hình). 📤 Đầu ra: Dừng chương trình nếu không ai bắt.",
                        "void HamA() => HamB(); // 3. Lỗi bay tiếp lên HamA\nvoid HamB() => HamC(); // 2. Lỗi bay ngược lên HamB\nvoid HamC() => throw new Exception(\"Lỗi ở tầng sâu nhất!\"); // 1. Nổ lỗi"),
                    Step(3,
                        "3. Trích xuất thông tin từ Exception để ghi Log",
                        "📥 Đầu vào: Biến ex trong khối catch. ⚙️ Cơ chế ngầm: Đọc thuộc tính Message, StackTrace và InnerException?.Message để biết chính xác dòng code gây lỗi. 📤 Đầu ra: Bản ghi Log chi tiết phục vụ sửa lỗi.",
                        "catch (Exception ex) {\n    Console.WriteLine($\"Lỗi: {ex.Message}\");\n    Console.WriteLine($\"Gốc rễ: {ex.InnerException?.Message}\");\n    Console.WriteLine($\"Vị trí dòng lỗi: {ex.StackTrace}\");\n}")
                },
                "Khi lưu dữ liệu xuống SQL Server bằng Entity Framework Core (await _db.SaveChangesAsync()), hệ thống văng ra lỗi DbUpdateException với thông báo chung chung: \"An error occurred while saving the entity changes\". Bạn phải kiểm tra thuộc tính nào của biến ex để đọc được chi tiết lỗi SQL thật sự bên dưới (ví dụ: trùng khóa Unique hoặc sai khóa ngoại)?",
                new List<string>
                {
                    "A. Kiểm tra thuộc tính ex.InnerException (đặc biệt là ex.InnerException?.Message)",
                    "B. Kiểm tra thuộc tính ex.HelpLink",
                    "C. Kiểm tra thuộc tính ex.Source",
                    "D. Không thể xem được lỗi chi tiết của SQL"
                },
                "A",
                "Khi một thư viện cấp cao (như EF Core) gặp lỗi từ tầng thấp hơn (như SQL Server Driver), nó sẽ gói lỗi gốc (SqlException) vào bên trong thuộc tính InnerException của DbUpdateException."
            ),

            // =====================================================================
            // BÀI 4.2
            // =====================================================================
            Build(
                topicId,
                "4.2",
                "Cơ chế Try - Catch - Finally và Quy tắc bắt nhiều tầng ngoại lệ (Multiple Catch Blocks)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/exception-handling-statements",
                """
                • **💡 1. Hiểu bản chất bộ ba `try - catch - finally` trong 30 giây:**
                  - **`try` (Khu vực thử nghiệm):** Nơi bạn đặt những dòng code "có nguy cơ xảy ra tai nạn" (như gọi Database, đọc file, gọi API bên thứ 3).
                  - **`catch` (Đệm hứng cứu hộ):** Chỉ kích hoạt chạy khi có lỗi nổ ra trong khối `try`. Giúp chương trình không bị sập và xử lý tình huống êm đẹp.
                  - **`finally` (Dọn dẹp hiện trường - LUÔN LUÔN CHẠY 100%):** Dù khối `try` chạy thành công mỹ mãn hay bị lỗi nhảy vào `catch` (thậm chí trong `try`/`catch` có lệnh `return`), thì khối **`finally` vẫn luôn luôn được chạy trước khi rời khỏi hàm**! Dùng để đóng kết nối mạng, đóng file, tắt biểu tượng Loading.
                • **🖼️ 2. Sơ đồ Quy tắc "Phễu Lọc" Khi Dùng Nhiều Khối `catch` (Multiple Catch Blocks):**
                ```
                try {
                    // Code có thể sinh ra nhiều loại lỗi khác nhau
                }
                catch (DivideByZeroException ex)     ──► [Phễu nhỏ - Lỗi cụ thể nhất phải đặt TRÊN CÙNG!]
                { ... }
                catch (ArithmeticException ex)       ──► [Phễu vừa - Lớp cha trung gian đặt Ở GIỮA]
                { ... }
                catch (Exception ex)                 ──► [Phễu khổng lồ - Lớp tổ tiên đặt DƯỚI CÙNG để vét hết lỗi còn lại]
                { ... }
                finally                              ──► [Luôn luôn chạy cuối cùng dù có lỗi hay không!]
                { ... }
                ```
                • **📊 3. Giải phẫu Cú pháp Nâng cao: Bộ lọc điều kiện `catch ... when (...)`:**
                ```
                catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                //                              │
                //                              └── Từ khóa 'when': Chỉ nhảy vào khối catch này nếu điều kiện bên trong là TRUE!
                ```
                • **⚠️ 4. Bẫy Cực Nguy Hiểm Cần Tránh:**
                  - Nếu bạn đặt `catch (Exception ex)` ở ngay dòng đầu tiên, rồi bên dưới mới viết `catch (FormatException ex)`, Visual Studio sẽ **báo lỗi đỏ không cho biên dịch**! Vì `Exception` là lớp tổ tiên lớn nhất, đặt ở trên cùng nó đã "nuốt trọn" mọi lỗi rồi, các khối `catch` bên dưới sẽ không bao giờ có cơ hội chạy tới.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bắt lỗi phân tầng từ Cụ thể đến Tổng quát",
                        "📥 Đầu vào: Chuỗi nhập từ người dùng để chia số. ⚙️ Cơ chế ngầm: Nếu nhập chữ -> nhảy vào FormatException. Nếu nhập số 0 -> nhảy vào DivideByZeroException. Các lỗi khác -> nhảy vào Exception cuối. 📤 Đầu ra: Thông báo lỗi chính xác từng trường hợp.",
                        "try {\n    int num = int.Parse(input);\n    int res = 100 / num;\n} catch (FormatException) {\n    Console.WriteLine(\"Bạn phải nhập chữ số!\");\n} catch (DivideByZeroException) {\n    Console.WriteLine(\"Không thể chia cho số 0!\");\n} catch (Exception ex) {\n    Console.WriteLine($\"Lỗi không xác định: {ex.Message}\");\n}"),
                    Step(2,
                        "2. Lọc ngoại lệ thông minh với từ khóa when",
                        "📥 Đầu vào: Lỗi SqlException có mã lỗi Number. ⚙️ Cơ chế ngầm: Dùng từ khóa when để bắt riêng mã lỗi 2627 (Trùng khóa chính/Unique trong SQL Server). 📤 Đầu ra: Xử lý riêng biệt từng mã lỗi.",
                        "catch (SqlException ex) when (ex.Number == 2627) {\n    Console.WriteLine(\"Dữ liệu bị trùng lặp trong Database!\");\n}"),
                    Step(3,
                        "3. Đảm bảo dọn dẹp 100% với khối finally",
                        "📥 Đầu vào: Kết nối file hoặc trạng thái bận. ⚙️ Cơ chế ngầm: Khối finally luôn chạy bất chấp try thành công hay nhảy vào catch. 📤 Đầu ra: Tài nguyên được giải phóng an toàn.",
                        "finally {\n    Console.WriteLine(\"Khối finally luôn chạy để đóng kết nối!\");\n}")
                },
                "Khi sử dụng nhiều khối catch nối tiếp nhau cho cùng một khối try, quy tắc sắp xếp thứ tự các khối catch nào dưới đây là BẮT BUỘC trong C#?",
                new List<string>
                {
                    "A. Phải sắp xếp từ loại Exception cụ thể nhất (lớp con) ở phía trên, rồi mới đến loại Exception tổng quát nhất (System.Exception - lớp cha) ở dưới cùng",
                    "B. Phải đặt catch (Exception ex) ở trên cùng trước tất cả các lỗi khác",
                    "C. Phải sắp xếp theo thứ tự bảng chữ cái A-Z",
                    "D. Đặt thứ tự thế nào cũng được, C# không quan tâm"
                },
                "A",
                "C# kiểm tra các khối catch tuần tự từ trên xuống dưới. Vì mọi ngoại lệ đều kế thừa từ System.Exception, nên các ngoại lệ chuyên biệt (như DivideByZeroException, FormatException) phải đứng trước Exception."
            ),

            // =====================================================================
            // BÀI 4.3
            // =====================================================================
            Build(
                topicId,
                "4.3",
                "Chủ động ném lỗi (throw), Sự khác biệt chí mạng giữa \"throw;\" vs \"throw ex;\" và Custom Exception",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/exceptions/creating-and-throwing-exceptions",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Không chỉ ngồi chờ hệ thống tự văng lỗi, bạn hoàn toàn có thể **chủ động ném ra một quả bom lỗi** bằng từ khóa **`throw new ...Exception("Lý do")`** ngay khi phát hiện dữ liệu vi phạm quy tắc nghiệp vụ (ví dụ: Số tiền rút lớn hơn số dư tài khoản!).
                • **🖼️ 2. Câu hỏi phỏng vấn kinh điển: Tại sao trong khối `catch`, người ta viết `throw;` chứ CẤM viết `throw ex;`?**
                ```
                Giả sử lỗi gốc phát nổ tại Dòng 15 của file ProductRepository.cs, bay lên Service ở Dòng 50:

                [Cách 1 - Viết ĐÚNG: throw; (Giữ nguyên hiện trường gốc)]
                catch (Exception ex) {
                    _logger.LogError(ex, "Ghi log lỗi");
                    throw;  ──► Giữ nguyên StackTrace báo lỗi ở Dòng 15 file ProductRepository.cs!
                }

                [Cách 2 - Viết SAI: throw ex; (Xóa sạch dấu vết hiện trường gốc!)]
                catch (Exception ex) {
                    _logger.LogError(ex, "Ghi log lỗi");
                    throw ex; ──► XÓA MẤT dòng 15! Báo lỗi giả tạo rằng lỗi bắt đầu từ Dòng 50 của Service!
                }
                ```
                • **📊 3. Tự tạo Ngoại lệ Nghiệp vụ riêng (`Custom Exception`):**
                  - Trong dự án thực tế, để phân biệt rõ lỗi hệ thống và lỗi nghiệp vụ (như *Không tìm thấy dữ liệu*, *Tài khoản bị khóa*, *Không đủ số dư*), bạn tạo các class kế thừa từ `Exception`:
                ```
                public class InsufficientBalanceException : Exception
                {
                    // Chuyển tiếp thông báo lỗi lên Constructor của lớp cha Exception bằng : base(message)
                    public InsufficientBalanceException(string message) : base(message) { }
                }
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo class Custom Exception kế thừa từ Exception",
                        "📥 Đầu vào: Tên lỗi nghiệp vụ muốn tạo (NotFoundException). ⚙️ Cơ chế ngầm: Kế thừa lớp Exception của .NET và đẩy chuỗi message lên lớp cha qua : base(message). 📤 Đầu ra: Một loại Exception mới mang tên nghiệp vụ rõ ràng.",
                        "public class NotFoundException : Exception {\n    public NotFoundException(string message) : base(message) {\n    }\n}"),
                    Step(2,
                        "2. Chủ động ném lỗi bằng từ khóa throw new",
                        "📥 Đầu vào: Kiểm tra sản phẩm bằng null. ⚙️ Cơ chế ngầm: Ngắt ngay luồng chạy hiện tại của hàm và bắn đối tượng NotFoundException ra ngoài. 📤 Đầu ra: Chuyển quyền điều khiển cho khối catch gần nhất.",
                        "public Product GetProduct(int id) {\n    var product = _repo.Find(id);\n    if (product == null)\n        throw new NotFoundException($\"Không tìm thấy SP mã {id}\");\n    return product;\n}"),
                    Step(3,
                        "3. Ném tiếp lỗi đúng chuẩn bằng lệnh throw; trần",
                        "📥 Đầu vào: Bắt lỗi ở tầng trung gian để ghi Log rồi đẩy tiếp lên tầng trên. ⚙️ Cơ chế ngầm: Dùng 'throw;' đứng một mình để bảo toàn 100% lịch sử StackTrace gốc. 📤 Đầu ra: Tầng ngoài cùng biết chính xác dòng code đầu tiên gây lỗi.",
                        "catch (Exception ex) {\n    _logger.LogError(ex, \"Đã ghi nhận lỗi tại Service\");\n    throw; // ĐÚNG CHUẨN: Giữ nguyên StackTrace gốc (Không viết throw ex;)\n}")
                },
                "Bên trong khối catch (Exception ex), sự khác biệt nghiêm trọng giữa việc viết câu lệnh throw; (đứng một mình) và viết throw ex; là gì?",
                new List<string>
                {
                    "A. Viết 'throw;' sẽ giữ nguyên vẹn toàn bộ dấu vết StackTrace ban đầu (chỉ rõ dòng code gốc nơi phát sinh lỗi); còn viết 'throw ex;' sẽ xóa mất lịch sử StackTrace trước đó và ghi đè vị trí lỗi thành dòng hiện tại",
                    "B. Viết 'throw ex;' chạy nhanh hơn gấp đôi",
                    "C. Viết 'throw;' không biên dịch được trong C#",
                    "D. Hai cách viết cho kết quả StackTrace giống hệt nhau"
                },
                "A",
                "throw; tái phát sinh ngoại lệ hiện tại mà không làm thay đổi StackTrace, trong khi throw ex; coi dòng lệnh đó là điểm khởi phát mới của ngoại lệ, làm mất toàn bộ thông tin về hàm và dòng code thực sự gây ra lỗi ở tầng sâu hơn."
            ),

            // =====================================================================
            // BÀI 4.4
            // =====================================================================
            Build(
                topicId,
                "4.4",
                "Quản lý tài nguyên Unmanaged với IDisposable và Câu lệnh using (Chống rò rỉ bộ nhớ & treo kết nối)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/statements/using",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Bộ dọn rác tự động **Garbage Collector (GC)** của .NET rất giỏi dọn các biến C# thông thường trên Heap. NHƯNG có những **Tài nguyên nằm ngoài tầm kiểm soát của .NET (Unmanaged Resources)** như: **Kết nối SQL Server (`SqlConnection`, `DbContext`)**, **File đang mở trên ổ cứng (`FileStream`)**, **Kết nối mạng (`HttpClient`)**.
                  - Nếu bạn mở một file hoặc mở kết nối Database mà quên đóng lại, file đó sẽ bị khóa cứng (không ai mở hay xóa được) và SQL Server sẽ bị **cạn kiệt cổng kết nối (Connection Pool Exhaustion)** làm sập web!
                • **🖼️ 2. Sự tiến hóa từ `try-finally` sang câu lệnh `using` tự động dọn dẹp:**
                ```
                [Cách thủ công dài dòng]:
                var stream = new FileStream("data.txt", FileMode.Open);
                try { /* Đọc file */ }
                finally { stream.Dispose(); } // Phải tự nhớ gọi hàm Dispose() để trả lại file cho Windows

                [Cách hiện đại chuẩn C# (using declaration) - Tự động gọi Dispose() 100%]:
                using var stream = new FileStream("data.txt", FileMode.Open);
                // Dùng thoải mái... Khi chạy đến dấu } cuối hàm (dù thành công hay nổ lỗi Exception),
                // C# TỰ ĐỘNG gọi stream.Dispose() để đóng file ngay lập tức!
                ```
                • **📊 3. Giao diện `IDisposable` và câu lệnh `using`:**
                | Thành phần | Bản chất & Công dụng | Khi nào bạn nhìn thấy trong dự án? |
                | :--- | :--- | :--- |
                | **Interface `IDisposable`**| Chứa duy nhất 1 hàm `void Dispose()` dùng để giải phóng tài nguyên ngầm ngay lập tức. | Các class như `DbContext`, `SqlConnection`, `MemoryStream`, `StreamReader`, `DbTransaction`. |
                | **`using (var x = ...)`** | Khối `using` truyền thống: Ra khỏi cặp ngoặc nhọn `{}` của `using` là tự động gọi `x.Dispose()`. | Khi muốn giải phóng tài nguyên ngay sau vài dòng code. |
                | **`using var x = ...;`** | Cú pháp `using` hiện đại (không cần `{}`): Tự động gọi `x.Dispose()` khi kết thúc hàm hiện tại. | Dùng khi mở `Transaction` hoặc đọc ghi `Stream` trong các hàm Service. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Mở tài nguyên kèm từ khóa using",
                        "📥 Đầu vào: Đối tượng StreamWriter để ghi file văn bản (thực thi IDisposable). ⚙️ Cơ chế ngầm: Trình biên dịch C# tự động bọc đoạn code bên dưới vào một khối try - finally ẩn. 📤 Đầu ra: Mở luồng ghi file an toàn.",
                        "public void WriteLog(string message) {\n    using var writer = new StreamWriter(\"app_log.txt\", append: true);\n    writer.WriteLine($\"{DateTime.Now}: {message}\");\n} // Vừa chạm dấu } này, writer.Dispose() tự động chạy để đóng file!"),
                    Step(2,
                        "2. Điều gì xảy ra nếu có Exception nổ ra ở giữa?",
                        "📥 Đầu vào: Đang ghi file thì bị lỗi bất ngờ. ⚙️ Cơ chế ngầm: Nhờ khối finally ngầm của từ khóa using, file 'app_log.txt' VẪN ĐƯỢC ĐÓNG LẠI AN TOÀN trước khi văng lỗi ra ngoài. 📤 Đầu ra: File không bao giờ bị treo hay bị khóa.",
                        "using (var conn = new SqlConnection(connectionString)) {\n    conn.Open();\n    // Dù dòng dưới có nổ lỗi SQL, conn.Dispose() vẫn tự động đóng kết nối!\n}"),
                    Step(3,
                        "3. Vì sao DbContext tiêm qua DI không cần bạn viết using?",
                        "📥 Đầu vào: AppDbContext đăng ký bằng AddScoped trong Program.cs. ⚙️ Cơ chế ngầm: Khi HTTP Request kết thúc, DI Container của ASP.NET Core sẽ tự động gọi _dbContext.Dispose() thay cho bạn! 📤 Đầu ra: Kết nối SQL luôn được trả về Pool đúng hạn.",
                        "// Với DbContext tiêm qua Constructor (AddScoped), ASP.NET Core tự gọi Dispose() cuối mỗi Request.")
                },
                "Tác dụng thực sự của từ khóa using khi khai báo using var stream = new FileStream(...); (không phải using thư viện ở đầu file) trong C# là gì?",
                new List<string>
                {
                    "A. Đảm bảo phương thức .Dispose() của đối tượng sẽ luôn được gọi tự động ngay khi ra khỏi phạm vi hàm (kể cả khi có xảy ra lỗi Exception ở giữa chừng), giúp giải phóng ngay các tài nguyên như File, Kết nối mạng, Database",
                    "B. Giúp biến stream tồn tại vĩnh viễn trên RAM không bao giờ bị xóa",
                    "C. Giúp tăng gấp đôi dung lượng ổ cứng",
                    "D. Chỉ dùng để khai báo tên Namespace"
                },
                "A",
                "Câu lệnh using được trình biên dịch C# dịch thành cấu trúc try { ... } finally { stream?.Dispose(); }, đảm bảo tài nguyên IDisposable luôn được thu hồi tức thì mà không bị rò rỉ."
            ),

            // =====================================================================
            // BÀI 4.5
            // =====================================================================
            Build(
                topicId,
                "4.5",
                "Tư duy Lập trình Đồng bộ (Synchronous) vs Bất đồng bộ (Asynchronous): Tại sao Web Server cần Async?",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/asynchronous-programming/",
                """
                • **💡 1. Hiểu bản chất qua ví dụ "Quán Phở" trong 30 giây:**
                  - Giả sử quán phở (Web Server) chỉ có **1 nhân viên chạy bàn (`Thread` - Luồng xử lý)**.
                  - **Cách 1 - Đồng bộ (`Synchronous` - Chặn luồng / Blocking):** Khách A vào gọi bát phở. Anh nhân viên chạy vào bếp báo nấu (mất 5 phút hầm xương — tương đương chờ SQL Server truy vấn hoặc chờ gọi API ngân hàng). Trong suốt 5 phút đó, anh nhân viên **đứng chôn chân nhìn nồi nước sôi, không làm bất cứ việc gì khác**! Khách B, Khách C bước vào quán đều phải đứng chờ ngoài cửa vì không có ai tiếp!
                  - **Cách 2 - Bất đồng bộ (`Asynchronous` - Không chặn luồng / Non-blocking):** Khách A gọi phở. Anh nhân viên bấm nút báo bếp nấu (`await`), rồi **lập tức quay ra ngoài bàn đón tiếp Khách B, Khách C luôn**! Khi nào bếp nấu xong bát phở của Khách A và bấm chuông báo, anh nhân viên mới quay lại bưng phở ra cho Khách A. Kết quả: Cùng 1 nhân viên nhưng phục vụ được hàng nghìn khách cùng lúc!
                • **🖼️ 2. Sơ đồ So sánh Luồng xử lý (Thread Pool) trên Web Server:**
                ```
                [Đồng bộ - Synchronous (Blocking Thread)]:
                Request 1 ──► [Gọi SQL 3 giây: Thread đứng chơi chờ đợi...] ──► Trả kết quả (Tắc nghẽn!)

                [Bất đồng bộ - Asynchronous (Non-blocking với async/await)]:
                Request 1 ──► Bắn lệnh SQL (await) ──► Giải phóng Thread đi phục vụ Request 2, 3, 4 ngay!
                              (3 giây sau SQL trả dữ liệu về) ──► Lấy Thread rảnh hoàn tất Request 1!
                ```
                • **📊 3. Phân biệt hai khái niệm hay bị nhầm: `Bất đồng bộ (Asynchronous)` vs `Đa luồng song song (Multi-threading)`:**
                | Tiêu chí | Bất đồng bộ (`async / await` - I/O Bound) | Đa luồng song song (`Parallel` - CPU Bound) |
                | :--- | :--- | :--- |
                | **Mục đích chính** | **Giải phóng luồng trong lúc chờ đợi bên ngoài** (Chờ Database, chờ đọc File, chờ gọi API mạng). | Chia một phép tính toán nặng cho nhiều nhân CPU cùng gồng mình tính một lúc. |
                | **Có tốn thêm Thread không?**| **KHÔNG!** Trong lúc `await`, nó trả Thread về lại hồ chứa (`ThreadPool`) để phục vụ người khác. | **CÓ!** Chiếm dụng nhiều Thread chạy cùng lúc. |
                | **Ứng dụng trong Web API** | **Dùng 99% ở mọi hàm Controller, Service, Repository!** | Hiếm dùng trong Web API (chỉ dùng khi xử lý ảnh/video/AI nặng). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tác vụ I/O Bound bắt đầu (Gọi Database / Gọi API)",
                        "📥 Đầu vào: Request yêu cầu lấy danh sách đơn hàng từ SQL Server. ⚙️ Cơ chế ngầm: CPU gửi gói tin truy vấn qua dây mạng tới máy chủ SQL Server. Thời gian chờ SQL xử lý mất khoảng 50ms - 500ms. 📤 Đầu ra: Bắt đầu trạng thái chờ I/O.",
                        "// Nếu viết đồng bộ: var list = _db.Orders.ToList(); -> Thread bị khóa cứng 500ms!"),
                    Step(2,
                        "2. Từ khóa await giải phóng Thread ngay lập tức",
                        "📥 Đầu vào: Gặp lệnh await _db.Orders.ToListAsync(). ⚙️ Cơ chế ngầm: .NET đánh dấu vị trí hiện tại, trả ngay Thread đang đứng chờ về lại ThreadPool để nó đi đón các HTTP Request khác của người dùng khác. 📤 Đầu ra: Server không bao giờ bị treo dù có 1.000 người truy cập cùng lúc.",
                        "var list = await _db.Orders.ToListAsync();\n// Trong lúc chờ SQL trả lời, Thread được tự do đi làm việc khác!"),
                    Step(3,
                        "3. SQL trả kết quả xong -> Tiếp tục chạy phần còn lại",
                        "📥 Đầu vào: SQL Server gửi dữ liệu về. ⚙️ Cơ chế ngầm: .NET lấy một Thread từ ThreadPool nhảy vào ngay dòng code phía dưới dòng await để đóng gói DTO và trả về cho Client. 📤 Đầu ra: Hoàn tất Request mượt mà.",
                        "return Ok(list); // Tự động chạy tiếp ngay khi dữ liệu từ SQL về tới nơi")
                },
                "Lợi ích lớn nhất của việc sử dụng lập trình bất đồng bộ (async / await) cho các thao tác truy vấn Cơ sở dữ liệu hoặc gọi API trong một ứng dụng Web Server (ASP.NET Core) là gì?",
                new List<string>
                {
                    "A. Giúp giải phóng luồng xử lý (Thread) về lại ThreadPool trong khoảng thời gian chờ đợi Database/Mạng phản hồi, nhờ đó Server có thể phục vụ đồng thời hàng nghìn người dùng khác mà không bị tắc nghẽn (Thread Starvation)",
                    "B. Giúp ổ cứng máy tính tăng thêm dung lượng",
                    "C. Giúp câu lệnh SQL không cần kiểm tra cú pháp",
                    "D. Biến máy tính 1 nhân thành máy tính 100 nhân"
                },
                "A",
                "Trong môi trường Web Server, tài nguyên quý giá nhất là các luồng xử lý (ThreadPool). async/await giúp không một Thread nào phải 'đứng không ngồi chờ' I/O, giúp tăng khả năng chịu tải (Scalability) của hệ thống lên gấp nhiều lần."
            ),

            // =====================================================================
            // BÀI 4.6
            // =====================================================================
            Build(
                topicId,
                "4.6",
                "Giải phẫu bộ ba Task, Task<T>, async và await (Quy tắc bất di bất dịch khi viết hàm Bất đồng bộ)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/asynchronous-programming/task-asynchronous-programming-model",
                """
                • **💡 1. Hiểu bản chất của `Task` (Lời hứa / Phiếu hẹn) trong 30 giây:**
                  - Khi bạn gọi một hàm bất đồng bộ, nó không trả ra kết quả ngay lập tức mà trao cho bạn một tờ **"Phiếu hẹn" gọi là `Task`**.
                  - Tờ phiếu `Task` này cam kết: *"Công việc đang được thực hiện rồi nhé, hãy dùng từ khóa **`await`** đặt trước tờ phiếu này để chờ lấy kết quả cuối cùng khi nó hoàn tất!"*.
                • **🖼️ 2. Giải phẫu Cú pháp Một Hàm Bất Đồng Bộ Chuẩn 100%:**
                ```
                public async Task<ProductDto> GetProductByIdAsync(int id)
                //       │          │                     │
                //       │          │                     └── 3. Quy ước đặt tên: Luôn có hậu tố "Async" ở cuối tên hàm
                //       │          └── 2. Kiểu trả về: Bọc kết quả thật (ProductDto) vào trong Task<...>
                //       └── 1. Từ khóa 'async': Bật chế độ cho phép sử dụng từ khóa 'await' ở bên trong thân hàm
                {
                    var entity = await _db.Products.FindAsync(id);
                //                 │
                //                 └── 4. Từ khóa 'await': Mở hộp phiếu hẹn Task<Product> để lấy ra đối tượng Product thật!
                    return new ProductDto(entity.Id, entity.Name);
                }
                ```
                • **📊 3. Bảng Đối Chiếu Kiểu Trả Về Giữa Hàm Đồng Bộ và Hàm Bất Đồng Bộ:**
                | Mục đích của hàm | Khi viết Đồng bộ (Synchronous) | Khi viết Bất đồng bộ (Asynchronous - Chuẩn) | Lưu ý sống còn |
                | :--- | :--- | :--- | :--- |
                | **Hàm KHÔNG trả về dữ liệu** | `public void SaveData()` | **`public async Task SaveDataAsync()`** | ⚠️ **CẤM dùng `async void`** (trừ Event của WinForms), phải dùng **`async Task`**! |
                | **Hàm trả về 1 giá trị (`int`, `bool`...)**| `public int CountItems()` | **`public async Task<int> CountItemsAsync()`**| Trong thân hàm vẫn chỉ viết `return 10;` (C# tự gói số `10` vào `Task<int>`). |
                | **Hàm trả về Danh sách** | `public List<User> GetAll()` | **`public async Task<List<User>> GetAllAsync()`**| Khi gọi nhớ đặt `await` ở trước để mở hộp lấy `List<User>` ra. |
                • **⚠️ 4. Tại sao CẤM viết `async void` trong Web API / Service?**
                  - Vì hàm `async void` không trả về tờ phiếu `Task`, nên nơi gọi nó không thể `await` để biết khi nào hàm chạy xong, và nguy hiểm nhất: **mọi lỗi Exception nổ ra bên trong hàm `async void` sẽ không thể bắt được bằng `try-catch` ở bên ngoài**, làm sập thẳng cả tiến trình Server!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Chuyển đổi hàm void sang async Task",
                        "📥 Đầu vào: Hàm gửi email không có giá trị trả về. ⚙️ Cơ chế ngầm: Thay chữ void bằng chữ Task để người gọi ở tầng trên có thể await chờ gửi xong và bắt được lỗi nếu gửi thất bại. 📤 Đầu ra: Hàm bất đồng bộ an toàn.",
                        "public async Task SendWelcomeEmailAsync(string email) {\n    await Task.Delay(1000); // Giả lập gửi mail mất 1 giây\n    Console.WriteLine($\"Đã gửi mail tới {email}\");\n}"),
                    Step(2,
                        "2. Chuyển đổi hàm có kết quả sang async Task<T>",
                        "📥 Đầu vào: Hàm tính tổng tiền trả về kiểu decimal. ⚙️ Cơ chế ngầm: Khai báo kiểu trả về là Task<decimal>. Bên trong thân hàm dùng await và cuối hàm return giá trị decimal bình thường. 📤 Đầu ra: Task<decimal>.",
                        "public async Task<decimal> GetTotalRevenueAsync() {\n    decimal sum = await _db.Orders.SumAsync(o => o.Total);\n    return sum; // Chỉ cần return decimal, C# tự bọc vào Task<decimal>!\n}"),
                    Step(3,
                        "3. Mở hộp Task bằng từ khóa await tại nơi gọi",
                        "📥 Đầu vào: Lời gọi hàm GetTotalRevenueAsync(). ⚙️ Cơ chế ngầm: Nếu quên gõ chữ await, biến nhận được chỉ là cái vỏ hộp Task<decimal>. Có chữ await, nó sẽ mở hộp lấy thẳng con số decimal ra! 📤 Đầu ra: Giá trị decimal thật.",
                        "// Nếu KHÔNG có await -> kiểu là Task<decimal> (chưa dùng cộng trừ được)\n// Có await -> lấy thẳng giá trị decimal ra:\ndecimal revenue = await GetTotalRevenueAsync();")
                },
                "Khi viết một phương thức bất đồng bộ trong tầng Service chỉ thực hiện hành động xóa bản ghi mà không trả về dữ liệu gì (tương đương với void trong lập trình đồng bộ), bạn phải khai báo kiểu trả về của phương thức đó là gì?",
                new List<string>
                {
                    "A. Khai báo là: public async Task DeleteProductAsync(int id) (Sử dụng Task thay cho void)",
                    "B. Khai báo là: public async void DeleteProductAsync(int id)",
                    "C. Khai báo là: public async null DeleteProductAsync(int id)",
                    "D. Khai báo là: public Task<void> DeleteProductAsync(int id)"
                },
                "A",
                "Trong C#, Task (không có <T>) đại diện cho một tác vụ bất đồng bộ không trả về giá trị. Luôn luôn dùng async Task thay vì async void để nơi gọi có thể await và bắt được Exception."
            ),

            // =====================================================================
            // BÀI 4.7
            // =====================================================================
            Build(
                topicId,
                "4.7",
                "Chạy song song nhiều tác vụ độc lập với Task.WhenAll và Task.WhenAny (Tối ưu tốc độ gấp bội)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.threading.tasks.task.whenall",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Giả sử để hiển thị trang Hồ sơ cá nhân, bạn phải gọi 3 API độc lập: (1) Lấy thông tin cá nhân (mất `2 giây`), (2) Lấy lịch sử thời tiết (mất `2 giây`), (3) Lấy tin tức mới (mất `2 giây`).
                  - Nếu bạn viết `await` từng dòng nối tiếp nhau: Hết việc 1 mới làm việc 2, hết việc 2 mới làm việc 3 -> Tổng thời gian khách phải chờ là **`2 + 2 + 2 = 6 giây`**!
                  - Nếu bạn phát lệnh cho cả 3 việc cùng xuất phát một lúc rồi dùng **`await Task.WhenAll(t1, t2, t3)`**: Cả 3 việc chạy song song cùng lúc, tổng thời gian chỉ mất đúng **`2 giây`** (bằng thời gian của anh lâu nhất)!
                • **🖼️ 2. Sơ đồ So Sánh Chạy Tuần Tự (`await` từng dòng) vs Chạy Song Song (`Task.WhenAll`):**
                ```
                [Cách 1 - Chạy tuần tự từng cái (Mất tổng cộng 6 giây)]:
                |-- Tác vụ 1 (2s) --|
                                    |-- Tác vụ 2 (2s) --|
                                                        |-- Tác vụ 3 (2s) --| ──► Xong ở giây thứ 6!

                [Cách 2 - Chạy đồng loạt với Task.WhenAll (Chỉ mất 2 giây!)]:
                |-- Tác vụ 1 (2s) --|
                |-- Tác vụ 2 (2s) --| ──► await Task.WhenAll(t1, t2, t3) ──► Cả 3 cùng xong ở giây thứ 2!
                |-- Tác vụ 3 (2s) --|
                ```
                • **📊 3. Phân biệt `Task.WhenAll` vs `Task.WhenAny` và Lưu ý Sống Còn với `DbContext`:**
                | Hàm hỗ trợ | Cách hoạt động | ⚠️ CẢNH BÁO QUAN TRỌNG KHI ĐI LÀM |
                | :--- | :--- | :--- |
                | **`Task.WhenAll(t1, t2)`** | Chờ cho đến khi **TẤT CẢ các Task** trong danh sách đều hoàn thành xong. | **KHÔNG ĐƯỢC dùng `Task.WhenAll` cho 2 câu truy vấn chạy trên CÙNG 1 biến `_dbContext`!** (Vì 1 `DbContext` không cho phép chạy 2 lệnh SQL cùng một thời điểm — sẽ nổ lỗi `InvalidOperationException`). |
                | **`Task.WhenAny(t1, t2)`** | Chỉ cần **1 Task bất kỳ về đích đầu tiên** là tiếp tục chạy luôn (dùng khi gọi 2 server dự phòng, bên nào trả lời trước thì lấy). | Chỉ dùng `Task.WhenAll` khi gọi các dịch vụ độc lập (gọi nhiều `HttpClient` API bên ngoài, gửi nhiều Email, đọc nhiều File). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi chạy đồng loạt các Task (Chưa gõ await vội)",
                        "📥 Đầu vào: 2 hàm gọi API bên ngoài độc lập nhau. ⚙️ Cơ chế ngầm: Gọi hàm nhưng KHÔNG viết chữ await ở đầu để cả 2 cuộc gọi mạng cùng bay đi một lúc. 📤 Đầu ra: 2 đối tượng phiếu hẹn Task.",
                        "// Chú ý: Chưa viết await ở 2 dòng này để chúng bắt đầu chạy cùng lúc!\nTask<WeatherDto> weatherTask = _weatherApi.GetTodayAsync();\nTask<NewsDto> newsTask = _newsApi.GetLatestAsync();"),
                    Step(2,
                        "2. Gom lại chờ chung tại một điểm với Task.WhenAll",
                        "📥 Đầu vào: weatherTask và newsTask đang chạy song song. ⚙️ Cơ chế ngầm: Task.WhenAll đứng đợi cho đến khi cả 2 cuộc gọi API đều trả kết quả về xong xuôi. 📤 Đầu ra: Hoàn tất trong thời gian ngắn nhất.",
                        "await Task.WhenAll(weatherTask, newsTask);"),
                    Step(3,
                        "3. Lấy kết quả ra từ các Task đã hoàn thành",
                        "📥 Đầu vào: Các Task đã chạy xong sau WhenAll. ⚙️ Cơ chế ngầm: Dùng await (hoặc .Result vì lúc này đã xong 100% rồi) để lấy dữ liệu thật ra ghép vào Dashboard. 📤 Đầu ra: Dữ liệu tổng hợp.",
                        "WeatherDto weather = await weatherTask;\nNewsDto news = await newsTask;")
                },
                "Một bạn lập trình viên muốn tăng tốc độ hàm trong Service nên đã viết: var t1 = _dbContext.Users.ToListAsync(); var t2 = _dbContext.Products.ToListAsync(); await Task.WhenAll(t1, t2); (dùng chung một biến _dbContext để chạy 2 câu lệnh SQL cùng một lúc). Khi chạy đoạn code này sẽ xảy ra hiện tượng gì?",
                new List<string>
                {
                    "A. Chạy nhanh gấp đôi bình thường",
                    "B. Văng lỗi InvalidOperationException ('A second operation was started on this context instance before a previous operation completed') vì một đối tượng DbContext của EF Core KHÔNG hỗ trợ chạy đa luồng/song song nhiều câu truy vấn cùng một thời điểm",
                    "C. Tự động tạo thêm một Database mới",
                    "D. Xóa dữ liệu bảng Users"
                },
                "B",
                "Đây là cái bẫy thực tế cực kỳ phổ biến! DbContext không phải là Thread-Safe và chỉ xử lý được 1 thao tác SQL tại một thời điểm trên cùng 1 instance. Với _dbContext, bạn phải await từng câu lệnh tuần tự; còn Task.WhenAll dùng cho các cuộc gọi HttpClient, gửi Mail, đọc File hoặc khi mỗi Task dùng một DbContext riêng (tạo từ IDbContextFactory)."
            ),

            // =====================================================================
            // BÀI 4.8
            // =====================================================================
            Build(
                topicId,
                "4.8",
                "Hủy tác vụ đang chạy với CancellationToken (Tiết kiệm tài nguyên khi người dùng tắt trang)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/threading/cancellation-in-managed-threads",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Hãy tưởng tượng một người dùng bấm nút *"Xuất báo cáo 5 năm"* trên trang web (câu lệnh SQL nặng mất `10 giây`). Nhưng mới chờ được `1 giây`, người dùng sốt ruột **bấm nút X tắt trình duyệt đi (hoặc bấm F5 tải lại trang)**!
                  - Nếu bạn không dùng **`CancellationToken` (Tín hiệu báo hủy)**: Dù người dùng đã bỏ đi rồi, nhưng ở dưới Server của bạn, câu lệnh SQL và code C# vẫn cặm cụi chạy tiếp suốt 9 giây còn lại một cách vô ích — nếu người dùng bấm F5 liên tục 20 lần, Server sẽ bị quá tải vì chạy 20 tác vụ "ma" không ai nhận kết quả!
                  - Với **`CancellationToken`**: Ngay khi người dùng tắt tab trình duyệt, ASP.NET Core tự động phát tín hiệu **HỦY (`IsCancellationRequested = true`)** truyền xuống `Controller` -> `Service` -> `EF Core` -> **Ra lệnh cho SQL Server dừng ngay câu truy vấn đang chạy dở ở giây thứ 1**!
                • **🖼️ 2. Sơ đồ Luồng Truyền Tín Hiệu Hủy (`CancellationToken`) Xuyên Suốt Các Tầng:**
                ```
                Trình duyệt (Bấm nút X tắt trang)
                     │
                     ▼  (ASP.NET Core tự động phát tín hiệu hủy vào tham số CancellationToken ct)
                Controller: GetReport(CancellationToken ct)
                     │
                     ▼  (Truyền tiếp biến ct xuống tầng dưới)
                Service:    GetReportAsync(ct)
                     │
                     ▼  (Truyền tiếp biến ct vào hàm của EF Core / HttpClient)
                Repository: _db.Orders.ToListAsync(ct) ──► Dừng ngay câu lệnh SQL, giải phóng CPU & RAM!
                ```
                • **📊 3. Cách sử dụng `CancellationToken` trong thực tế:**
                  - Trong hàm `Controller`, chỉ cần khai báo thêm tham số `CancellationToken cancellationToken = default` ở cuối danh sách tham số (ASP.NET Core sẽ tự động truyền vào).
                  - Sau đó truyền biến `cancellationToken` đó vào mọi hàm bất đồng bộ có hỗ trợ như `.ToListAsync(cancellationToken)`, `.FirstOrDefaultAsync(..., cancellationToken)`, `.SaveChangesAsync(cancellationToken)`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Nhận CancellationToken tự động tại Controller",
                        "📥 Đầu vào: HTTP Request từ trình duyệt. ⚙️ Cơ chế ngầm: Model Binder của ASP.NET Core tự động gắn kết nối của trình duyệt với biến CancellationToken ct. 📤 Đầu ra: Biến ct sẵn sàng truyền đi.",
                        "[HttpGet(\"heavy-report\")]\npublic async Task<IActionResult> GetReport(CancellationToken ct) {\n    var data = await _reportService.GenerateAsync(ct);\n    return Ok(data);\n}"),
                    Step(2,
                        "2. Truyền CancellationToken xuống EF Core / HttpClient",
                        "📥 Đầu vào: Biến ct từ Controller truyền xuống. ⚙️ Cơ chế ngầm: Đưa ct vào bên trong ngoặc tròn của ToListAsync(ct). Nếu trình duyệt ngắt kết nối, EF Core lập tức hủy câu lệnh SQL. 📤 Đầu ra: Tiết kiệm 100% tài nguyên thừa.",
                        "public async Task<List<ReportDto>> GenerateAsync(CancellationToken ct) {\n    return await _db.Transactions\n        .AsNoTracking()\n        .Select(t => new ReportDto(t.Id, t.Amount))\n        .ToListAsync(ct); // Truyền ct vào đây!\n}"),
                    Step(3,
                        "3. Chủ động kiểm tra hủy trong vòng lặp dài",
                        "📥 Đầu vào: Vòng lặp xử lý 10.000 dòng Excel. ⚙️ Cơ chế ngầm: Gọi ct.ThrowIfCancellationRequested() ở đầu mỗi vòng lặp để dừng ngay nếu người dùng đã bấm Hủy. 📤 Đầu ra: Ngắt vòng lặp tức thì.",
                        "foreach (var row in excelRows) {\n    ct.ThrowIfCancellationRequested(); // Nếu đã hủy -> Thoát ngay!\n    await ProcessRowAsync(row, ct);\n}")
                },
                "Trong một Action của ASP.NET Core Web API có khai báo tham số CancellationToken ct, khi bạn truyền ct này xuống hàm _db.Products.ToListAsync(ct), lợi ích thực tế đạt được là gì?",
                new List<string>
                {
                    "A. Nếu người dùng đóng trình duyệt hoặc hủy Request giữa chừng khi câu truy vấn chưa chạy xong, EF Core sẽ tự động dừng câu lệnh SQL đang chạy trên Database ngay lập tức để giải phóng tài nguyên cho Server",
                    "B. Giúp bỏ qua bước kiểm tra mật khẩu đăng nhập",
                    "C. Giúp tự động xóa bảng Products nếu truy vấn lâu quá 5 giây",
                    "D. Không có tác dụng gì, chỉ viết cho đẹp code"
                },
                "A",
                "Truyền CancellationToken xuyên suốt từ Controller xuống các hàm I/O (ToListAsync(ct), HttpClient.GetAsync(..., ct)) là chuẩn mực vàng trong các dự án .NET chuyên nghiệp để tránh lãng phí CPU và Database cho những Request đã bị phía Client hủy bỏ."
            ),

            // =====================================================================
            // BÀI 4.9
            // =====================================================================
            Build(
                topicId,
                "4.9",
                "Các Cấm kỵ (Anti-patterns) trong Bất đồng bộ: Tại sao CẤM dùng .Result / .Wait() gây treo cứng (Deadlock)?",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/archive/msdn-magazine/2013/march/async-await-best-practices-in-asynchronous-programming",
                """
                • **💡 1. Hiểu bản chất hiện tượng `Deadlock` (Khóa chết chờ nhau mãi mãi) trong 30 giây:**
                  - Khi gọi một hàm trả về `Task<string>`, một số bạn lười không muốn thêm chữ `async/await` nên chấm luôn **`.Result`** hoặc gọi **`.Wait()`** (VD: `var data = GetDataAsync().Result;`) để ép hàm bất đồng bộ phải chạy theo kiểu đồng bộ!
                  - Đây gọi là lỗi **"Sync over Async" (Ép đồng bộ trên nền bất đồng bộ)** — nguyên nhân số 1 gây ra hiện tượng **Deadlock (Treo cứng toàn bộ ứng dụng)** và **Thread Pool Starvation (Cạn kiệt luồng làm sập Web Server)**!
                • **🖼️ 2. Sơ đồ Cơ chế Gây Treo Cứng (`Deadlock`) Khi Gọi `.Result` hoặc `.Wait()`:**
                ```
                Luồng chính (Thread A) gọi: GetDataAsync().Result
                  │
                  ├──► 1. Lệnh .Result KHÓA CỨNG (Block) Thread A lại, đứng lì chờ Task chạy xong.
                  │
                  └──► 2. Bên trong GetDataAsync(), sau khi tải dữ liệu xong, nó cần xin lại chính Thread A
                          để chạy nốt dòng lệnh return cuối cùng...
                          NHƯNG Thread A đang bị chính lệnh .Result khóa cứng ở bước 1 rồi!
                          ──► Thread A chờ Task xong, còn Task lại chờ Thread A rảnh ──► KHÓA CHẾT VĨNH VIỄN (DEADLOCK)!
                ```
                • **📊 3. Bảng 3 Cấm Kỵ Tuyệt Đối Trong Lập Trình `async / await` ("Async All The Way"):**
                | Cấm kỵ (Anti-pattern) | Cách viết SAI ❌ | Cách viết ĐÚNG CHUẨN ✅ | Lý do kỹ thuật |
                | :--- | :--- | :--- | :--- |
                | **1. Chấm `.Result` hoặc `.Wait()`**| `var x = GetAsync().Result;` | **`var x = await GetAsync();`** | Chặn cứng Thread (Blocking), gây cạn kiệt ThreadPool và nguy cơ Deadlock. |
                | **2. Dùng `Thread.Sleep()` trong hàm async**| `Thread.Sleep(3000);` | **`await Task.Delay(3000);`** | `Thread.Sleep` làm đóng băng Thread, còn `Task.Delay` giải phóng Thread trong lúc chờ. |
                | **3. Dùng `async void`** | `public async void Handle()` | **`public async Task HandleAsync()`**| `async void` làm mất khả năng bắt lỗi `try-catch` và không biết khi nào hàm kết thúc. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Quy tắc \"Async All The Way\" (Bất đồng bộ từ đáy lên đỉnh)",
                        "📥 Đầu vào: Tầng Repository dùng hàm Async của EF Core. ⚙️ Cơ chế ngầm: Đã dùng Async ở Repository thì lên Service cũng phải async/await, và lên tới Controller cũng phải async/await xuyên suốt! 📤 Đầu ra: Luồng bất đồng bộ thông suốt 100%.",
                        "// Repository: public async Task<User> GetAsync()\n// Service:    public async Task<UserDto> GetAsync()\n// Controller: public async Task<IActionResult> Get()"),
                    Step(2,
                        "2. Thay thế Thread.Sleep bằng await Task.Delay",
                        "📥 Đầu vào: Cần chờ 2 giây trước khi thử kết nối lại (Retry). ⚙️ Cơ chế ngầm: Dùng await Task.Delay(2000) để nhả Thread ra cho người khác dùng trong 2 giây đó. 📤 Đầu ra: Chờ đợi không chặn luồng.",
                        "// SAI: Thread.Sleep(2000); -> Đóng băng Thread!\n// ĐÚNG CHUẨN:\nawait Task.Delay(2000);"),
                    Step(3,
                        "3. Tối ưu bỏ async/await thừa ở hàm chỉ chuyển tiếp (Eliding Async)",
                        "📥 Đầu vào: Một hàm chỉ đóng vai trò trả thẳng Task của hàm khác mà không có try-catch hay xử lý gì thêm. ⚙️ Cơ chế ngầm: Có thể return trực tiếp Task nhưng nếu có try-catch hoặc using bên trong thì BẮT BUỘC phải có async/await! 📤 Đầu ra: Bắt được lỗi chuẩn xác.",
                        "public async Task<User> GetUserSafeAsync(int id) {\n    try {\n        // Có try-catch thì BẮT BUỘC phải có await ở đây mới bắt được lỗi!\n        return await _repo.GetByIdAsync(id);\n    } catch (Exception ex) {\n        _logger.LogError(ex, \"Lỗi!\");\n        throw;\n    }\n}")
                },
                "Tại sao trong một hàm bất đồng bộ, khi cần tạm dừng chờ một khoảng thời gian (ví dụ chờ 3 giây), bạn phải dùng await Task.Delay(3000); thay vì dùng Thread.Sleep(3000);?",
                new List<string>
                {
                    "A. Vì Thread.Sleep(3000) sẽ khóa cứng (Block) luồng hiện tại suốt 3 giây không làm được việc gì khác, còn await Task.Delay(3000) tạo bộ hẹn giờ bất đồng bộ và giải phóng luồng về lại ThreadPool ngay lập tức",
                    "B. Vì Thread.Sleep không nhận tham số mili-giây",
                    "C. Vì Task.Delay chạy nhanh hơn thời gian thực",
                    "D. Vì Thread.Sleep làm xóa bộ nhớ RAM"
                },
                "A",
                "Thread.Sleep là thao tác đồng bộ làm đóng băng luồng hiện tại, trong khi await Task.Delay là thao tác bất đồng bộ hoàn toàn không chiếm giữ luồng trong thời gian chờ."
            ),

            // =====================================================================
            // BÀI 4.10
            // =====================================================================
            Build(
                topicId,
                "4.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 4 (Kết hợp Exception Handling & Async/Await chuẩn Doanh nghiệp)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/asynchronous-programming/async-scenarios",
                """
                • **💡 1. Công thức "Bất khả chiến bại" khi kết hợp Xử lý Ngoại lệ và Bất đồng bộ:**
                  Trong một dự án thực tế, `Exception Handling` và `Async/Await` luôn đi đôi với nhau như hình với bóng. Dưới đây là 4 nguyên tắc tổng kết bạn cần khắc cốt ghi tâm:
                  1. **Nguyên tắc 1 (`await` bên trong `try-catch`):** Nếu bạn đặt một hàm trả về `Task` ở bên trong khối `try { ... }`, bạn **BẮT BUỘC phải có từ khóa `await`** trước hàm đó! Nếu quên `await`, hàm chưa kịp chạy xong thì luồng đã nhảy thoát ra khỏi khối `try` rồi — lúc lỗi nổ ra sau đó thì khối `catch` không còn ở đó để đỡ nữa!
                  2. **Nguyên tắc 2 (`using` + `async/await` + `CancellationToken`):** Luôn dùng `using` cho các tài nguyên `IDisposable` (như `Transaction`, `Stream`) và truyền `CancellationToken` vào các hàm `Async`.
                  3. **Nguyên tắc 3 (`throw;` bảo toàn `StackTrace`):** Trong khối `catch`, sau khi ghi log xong nếu muốn ném tiếp lỗi lên tầng trên thì chỉ viết đúng chữ `throw;`.
                  4. **Nguyên tắc 4 (`Task.WhenAll` cho các API độc lập):** Dùng `Task.WhenAll` để tăng tốc khi gọi nhiều dịch vụ mạng độc lập, nhưng không dùng chung 1 `DbContext` cho các Task song song.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Mở Transaction với using & Nhận CancellationToken",
                        "📥 Đầu vào: Yêu cầu thanh toán đơn hàng. ⚙️ Cơ chế ngầm: Mở phiên giao dịch bất đồng bộ có gắn using để tự động giải phóng khi kết thúc hàm. 📤 Đầu ra: Đối tượng transaction an toàn.",
                        "public async Task ProcessOrderAsync(OrderDto dto, CancellationToken ct) {\n    using var transaction = await _db.Database.BeginTransactionAsync(ct);\n    try {"),
                    Step(2,
                        "2. Kiểm tra nghiệp vụ & Chủ động ném Custom Exception",
                        "📥 Đầu vào: Kiểm tra dữ liệu sản phẩm. ⚙️ Cơ chế ngầm: Dùng await FirstOrDefaultAsync kèm CancellationToken. Nếu hết hàng, chủ động throw BadRequestException. 📤 Đầu ra: Chặn sớm dữ liệu lỗi.",
                        "        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId, ct);\n        if (product == null)\n            throw new NotFoundException(\"Sản phẩm không tồn tại!\");"),
                    Step(3,
                        "3. Lưu thay đổi & Commit Transaction bất đồng bộ",
                        "📥 Đầu vào: Dữ liệu hợp lệ. ⚙️ Cơ chế ngầm: Thực hiện SaveChangesAsync và CommitAsync để ghi nhận chính thức xuống ổ cứng SQL Server. 📤 Đầu ra: Giao dịch hoàn tất.",
                        "        product.Stock -= dto.Quantity;\n        await _db.SaveChangesAsync(ct);\n        await transaction.CommitAsync(ct);\n    }"),
                    Step(4,
                        "4. Bắt lỗi, Rollback, Ghi Log và Bảo toàn StackTrace (throw;)",
                        "📥 Đầu vào: Bất kỳ lỗi nào nổ ra ở Bước 2 hoặc Bước 3. ⚙️ Cơ chế ngầm: Hoàn tác dữ liệu bằng RollbackAsync, ghi Log kèm biến ex và dùng lệnh 'throw;' để đẩy lên GlobalExceptionMiddleware. 📤 Đầu ra: Hệ thống an toàn tuyệt đối.",
                        "    catch (Exception ex) {\n        await transaction.RollbackAsync(ct);\n        _logger.LogError(ex, \"Lỗi khi xử lý đơn hàng của sản phẩm {ProductId}\", dto.ProductId);\n        throw; // Giữ nguyên StackTrace gốc!\n    }\n}")
                },
                "(Câu hỏi tổng hợp Chủ đề 4) Giả sử bên trong khối try { _orderService.CreateOrderAsync(dto); } catch (Exception ex) { ... }, bạn VÔ TÌNH QUÊN không viết từ khóa await ở trước lời gọi _orderService.CreateOrderAsync(dto);. Nếu bên trong hàm CreateOrderAsync chạy được 1 giây rồi văng ra lỗi Exception, chuyện gì sẽ xảy ra với khối catch đó?",
                new List<string>
                {
                    "A. Khối catch vẫn bắt được lỗi bình thường như khi có await",
                    "B. Khối catch sẽ KHÔNG THỂ bắt được lỗi đó! Vì khi thiếu từ khóa await, hàm chỉ trả về tờ phiếu Task rồi thoát khỏi khối try ngay lập tức trước khi lỗi kịp xảy ra ở bên trong Task",
                    "C. Chương trình tự động thêm chữ await vào lúc chạy",
                    "D. Lỗi sẽ tự động biến mất"
                },
                "B",
                "Đây là lỗi thực tế cực kỳ tinh vi: nếu bạn gọi một hàm trả về Task bên trong try mà không await nó, luồng thực thi sẽ đi qua khối try ngay lập tức. Khi Task chạy ngầm và phát nổ lỗi sau đó, nó đã nằm ngoài phạm vi bảo vệ của khối try-catch ban đầu!"
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (4.1 -> 4.10) cho Chủ đề 4 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic4 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 4);
        if (topic4 == null)
            return;

        topic4.Title = "Chủ đề 4: Xử lý ngoại lệ và lập trình bất đồng bộ";
        topic4.Description = "10 Bài học con siêu chi tiết từ Cây phân cấp Exception & StackTrace, Try-Catch-Finally, throw vs throw ex & Custom Exception, IDisposable & using, Synchronous vs Asynchronous, Task/Task<T>/async/await, Task.WhenAll/WhenAny, CancellationToken đến phòng chống Deadlock (.Result/.Wait).";

        var targetLessons = GetLessons(topic4.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic4.Id)
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
