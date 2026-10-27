using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic11Seed
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
            // BÀI 11.1
            // =====================================================================
            Build(
                topicId,
                "11.1",
                "Tư duy Kiểm thử Phần mềm (Software Testing), Kim tự tháp Kiểm thử & Quy tắc 3A (Arrange - Act - Assert)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/core/testing/unit-testing-best-practices",
                """
                • **💡 1. Hiểu bản chất của `Automated Testing` (Kiểm thử tự động) trong 30 giây:**
                  - Khi mới học lập trình, mỗi lần sửa 1 dòng code trong hàm `TinhTienPhat()`, bạn lại phải bấm `F5` chạy web, mở trình duyệt, đăng nhập, nhập form rồi bấm nút xem kết quả (mất 3 phút). Nhưng nếu dự án có 200 hàm và bạn sửa cấu trúc chung, chẳng lẽ bạn ngồi bấm tay kiểm tra lại cả 200 màn hình mất 2 ngày?
                  - **Kiểm thử tự động (`Unit Test` / `Integration Test`)** là việc bạn **viết các đoạn code C# tự động gọi và kiểm tra chính các hàm C# của bạn**! Chỉ cần bấm 1 nút `Run All Tests`, máy tính sẽ tự động kiểm tra toàn bộ 200 hàm trong **2 giây**: hàm nào đúng hiện tích xanh ✅, hàm nào sai báo đỏ ❌ ngay lập tức!
                • **🖼️ 2. Sơ đồ Kim Tự Tháp Kiểm Thử (`Testing Pyramid`) Chuẩn Công Nghiệp:**
                ```
                                    /\
                                   /  \       3. E2E / UI Testing (Playwright / Selenium / FlaUI)
                                  / 10%\      ──► Chạy thật trên giao diện (Chậm nhất, chi phí cao)
                                 /──────\
                                /        \    2. Integration Testing (WebApplicationFactory / Postman)
                               /   20%    \   ──► Kiểm tra ghép nối nhiều tầng (Controller + Service + DB)
                              /────────────\
                             /              \ 1. Unit Testing (xUnit / NUnit + Moq) - NỀN TẢNG LỚN NHẤT!
                            /      70%       \──► Kiểm tra từng hàm C# độc lập trên RAM (Chạy 1000 test mất 1 giây!)
                           /__________________\
                ```
                • **📊 3. Cấu trúc Vàng `3A (Arrange - Act - Assert)` Trong Mọi Bài Test:**
                  Mọi hàm Unit Test trên thế giới đều được chia làm **3 phần rõ rệt** theo đúng công thức `AAA`:
                | Bước | Tên tiếng Anh | Ý nghĩa thực hiện | Ví dụ trong code C# |
                | :--- | :--- | :--- | :--- |
                | **Bước 1** | **`Arrange` (Chuẩn bị nguyên liệu)**| Khởi tạo đối tượng `Service`, chuẩn bị các biến đầu vào và dữ liệu giả lập. | `var service = new FineService(); int daysLate = 5;` |
                | **Bước 2** | **`Act` (Hành động chạy hàm)** | Gọi đúng **1 hàm duy nhất** mà bạn đang muốn kiểm tra và hứng lấy kết quả. | `decimal actualFine = service.CalculateFine(daysLate);` |
                | **Bước 3** | **`Assert` (Khẳng định đối chiếu)**| So sánh kết quả thực tế (`actual`) với kết quả kỳ vọng (`expected`). Nếu khớp ➔ Pass ✅! | `Assert.Equal(25000m, actualFine);` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Đặt tên hàm Test theo chuẩn [TênHàm]_[TìnhHuống]_[KếtQuảKỳVọng]",
                        "📥 Đầu vào: Cần test hàm CalculateLateFee khi trả sách trễ hạn. ⚙️ Cơ chế ngầm: Đặt tên hàm rõ ràng bằng tiếng Anh hoặc tiếng Việt không dấu để khi Test báo đỏ nhìn tên hàm là biết ngay kịch bản nào bị hỏng. 📤 Đầu ra: Chữ ký hàm Test chuẩn mực.",
                        "[Fact]\npublic void CalculateLateFee_WhenOverdue5Days_Returns25000()\n{"),
                    Step(2,
                        "2. Thực hiện bước Arrange (Chuẩn bị) và Act (Chạy hàm)",
                        "📥 Đầu vào: Quy định trễ 1 ngày phạt 5.000đ. ⚙️ Cơ chế ngầm: Khởi tạo LoanService (Arrange) và gọi hàm CalculateLateFee(5) (Act). 📤 Đầu ra: Biến result chứa giá trị hàm trả về.",
                        "    // 1. Arrange (Chuẩn bị)\n    var service = new LoanService();\n    int overdueDays = 5;\n\n    // 2. Act (Thực thi hàm cần test)\n    decimal result = service.CalculateLateFee(overdueDays);"),
                    Step(3,
                        "3. Thực hiện bước Assert (Kiểm chứng kết quả)",
                        "📥 Đầu vào: Biến result vừa tính ở Bước 2. ⚙️ Cơ chế ngầm: Dùng lớp Assert của xUnit để kiểm tra xem result có đúng bằng 25000m hay không. Nếu hàm tính sai ra 20000m, Assert sẽ ném lỗi báo đỏ bài test ngay! 📤 Đầu ra: Xác nhận logic đúng 100%.",
                        "    // 3. Assert (Kiểm chứng)\n    Assert.Equal(25000m, result);\n}")
                },
                "Trong cấu trúc chuẩn AAA (Arrange - Act - Assert) khi viết một hàm Unit Test, nhiệm vụ của phần Assert ở cuối hàm là gì?",
                new List<string>
                {
                    "A. Kiểm tra và đối chiếu kết quả thực tế mà hàm trả về (Actual) với kết quả đúng mà ta kỳ vọng (Expected) để quyết định bài Test đó Đạt (Pass) hay Thất bại (Fail)",
                    "B. Dùng để xóa cơ sở dữ liệu",
                    "C. Dùng để khai báo thư viện ở đầu file",
                    "D. Dùng để thiết kế giao diện HTML"
                },
                "A",
                "Arrange là bày nguyên liệu ra bàn, Act là bấm nút chạy máy, và Assert là kiểm tra sản phẩm đầu ra có đúng tiêu chuẩn hay không."
            ),

            // =====================================================================
            // BÀI 11.2
            // =====================================================================
            Build(
                topicId,
                "11.2",
                "Làm chủ xUnit.net trong .NET: Phân biệt [Fact] vs [Theory], [InlineData] và các hàm Assert cốt lõi",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/core/testing/unit-testing-with-dotnet-test",
                """
                • **💡 1. Tại sao `xUnit.net` là thư viện Unit Test số 1 trong hệ sinh thái .NET hiện đại?**
                  - Trong .NET có 3 thư viện test là `MSTest`, `NUnit` và **`xUnit`**. Hiện nay chính đội ngũ kỹ sư Microsoft phát triển ASP.NET Core và EF Core đều dùng **`xUnit`** làm tiêu chuẩn mặc định vì nó gọn nhẹ, cô lập từng bài test sạch sẽ và hỗ trợ truyền dữ liệu test hàng loạt cực kỳ thông minh!
                • **🖼️ 2. Phân biệt 2 nhãn đánh dấu bài Test quan trọng nhất của xUnit: `[Fact]` vs `[Theory]`:**
                ```
                [1. [Fact] (Kiểm tra 1 kịch bản cố định - Hàm KHÔNG có tham số đầu vào)]:
                [Fact]
                public void Divide_ByZero_ThrowsException() { ... } ──► Chạy đúng 1 lần với dữ liệu cố định bên trong.

                [2. [Theory] + [InlineData(...)] (Chạy 1 hàm Test cho NHIỀU bộ dữ liệu khác nhau!)]:
                [Theory]
                [InlineData(2, 3, 5)]     ──► Lượt 1: Truyền a = 2, b = 3, kỳ vọng sum = 5  (✅ Pass)
                [InlineData(0, 0, 0)]     ──► Lượt 2: Truyền a = 0, b = 0, kỳ vọng sum = 0  (✅ Pass)
                [InlineData(-5, 10, 5)]   ──► Lượt 3: Truyền a = -5, b = 10, kỳ vọng sum = 5 (✅ Pass)
                public void Add_MultipleCases_ReturnsCorrectSum(int a, int b, int expected) { ... }
                *(Viết 1 hàm duy nhất nhưng xUnit tự động nhân bản thành 3 bài Test độc lập!)*
                ```
                • **📊 3. Bảng Các Hàm `Assert` Cốt Lõi Của `xUnit` Bạn Cần Thuộc Lòng:**
                | Hàm kiểm chứng (`Assert`) | Ý nghĩa kiểm tra |
                | :--- | :--- |
                | **`Assert.Equal(expected, actual)`** | Kiểm tra 2 giá trị bằng nhau (Chú ý: đặt giá trị kỳ vọng `expected` đứng trước!). |
                | **`Assert.True(condition)` / `Assert.False(...)`**| Kiểm tra biểu thức trả về `true` hoặc `false`. |
                | **`Assert.Null(obj)` / `Assert.NotNull(obj)`** | Kiểm tra đối tượng trả về là `null` hay khác `null`. |
                | **`Assert.Contains(item, list)`** / `Assert.Empty(list)`| Kiểm tra phần tử có nằm trong danh sách không / Danh sách có rỗng không. |
                | **`await Assert.ThrowsAsync<TException>(() => ...)`**| ⭐ Kiểm tra xem hàm **có ném ra đúng loại Exception như thiết kế hay không** (khi truyền dữ liệu sai)! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Project xUnit và tham chiếu tới Project chính",
                        "📥 Đầu vào: Solution dự án hiện tại. ⚙️ Cơ chế ngầm: Dùng lệnh dotnet new xunit tạo project test riêng biệt và nối tham chiếu (Project Reference) sang project chính cần test. 📤 Đầu ra: Môi trường test tách biệt hoàn toàn với code sản phẩm.",
                        "dotnet new xunit -n HoangViet.UnitTests\ndotnet add HoangViet.UnitTests/HoangViet.UnitTests.csproj reference HoangViet/HoangViet.csproj\ndotnet test // Lệnh chạy toàn bộ bài test"),
                    Step(2,
                        "2. Dùng [Theory] và [InlineData] để test hàng loạt trường hợp biên",
                        "📥 Đầu vào: Hàm kiểm tra mật khẩu mạnh IsStrongPassword(string pwd). ⚙️ Cơ chế ngầm: Thay vì viết 4 hàm [Fact] lặp lại, dùng [Theory] kèm 4 dòng [InlineData] để kiểm tra cả mật khẩu ngắn, mật khẩu thiếu số, mật khẩu rỗng và mật khẩu chuẩn. 📤 Đầu ra: Code test ngắn gọn gấp 4 lần.",
                        "[Theory]\n[InlineData(\"123\", false)]          // Quá ngắn -> Kỳ vọng false\n[InlineData(\"\", false)]             // Chuỗi rỗng -> Kỳ vọng false\n[InlineData(\"abcdefg\", false)]      // Thiếu chữ hoa và số -> Kỳ vọng false\n[InlineData(\"HoangViet@2026\", true)]// Đạt chuẩn -> Kỳ vọng true\npublic void IsStrongPassword_VariousInputs_ReturnsExpected(string input, bool expected) {\n    bool actual = PasswordValidator.IsStrongPassword(input);\n    Assert.Equal(expected, actual);\n}"),
                    Step(3,
                        "3. Kiểm tra hàm ném lỗi ngoại lệ bằng Assert.ThrowsAsync",
                        "📥 Đầu vào: Khi truyền số lượng mượn âm (-1), hàm BorrowAsync phải ném lỗi ArgumentException. ⚙️ Cơ chế ngầm: Assert.ThrowsAsync<ArgumentException> coi việc hàm ném ra đúng lỗi đó là THÀNH CÔNG (Pass)! Nếu hàm im lặng không ném lỗi thì bài test mới bị Fail! 📤 Đầu ra: Đảm bảo các câu lệnh Guard Clause hoạt động tốt.",
                        "[Fact]\npublic async Task BorrowAsync_WhenQuantityIsNegative_ThrowsArgumentException() {\n    var service = new BorrowService();\n    // Kỳ vọng hàm bắt buộc phải ném ra lỗi ArgumentException:\n    await Assert.ThrowsAsync<ArgumentException>(() => service.BorrowAsync(bookId: 1, quantity: -5));\n}")
                },
                "Trong thư viện kiểm thử xUnit.net, khi bạn muốn viết 1 hàm kiểm thử duy nhất có nhận tham số để chạy kiểm tra lặp lại cho 5 bộ dữ liệu đầu vào/đầu ra khác nhau, bạn phải gắn cặp Attribute nào lên trên hàm đó?",
                new List<string>
                {
                    "A. Gắn [Theory] kết hợp với 5 dòng [InlineData(...)]",
                    "B. Gắn 5 thẻ [Fact] chồng lên nhau",
                    "C. Gắn thẻ [HttpGet] và [FromQuery]",
                    "D. Gắn thẻ [Required]"
                },
                "A",
                "Trong xUnit, [Fact] dùng cho bài test không có tham số, còn [Theory] đi kèm [InlineData(...)] (Data-driven test) cho phép truyền nhiều bộ tham số khác nhau vào cùng một hàm kiểm thử."
            ),

            // =====================================================================
            // BÀI 11.3
            // =====================================================================
            Build(
                topicId,
                "11.3",
                "Kỹ thuật Mocking với thư viện Moq (Giả lập IRepository & IService để test logic độc lập không cần Database thật)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/controllers/testing",
                """
                • **💡 1. Tại sao khi viết `Unit Test` cho `BookService` lại KHÔNG ĐƯỢC kết nối vào SQL Server thật?**
                  - Hãy nhớ lại: Trong `BookService` có tiêm `private readonly IBookRepository _bookRepo;`.
                  - Nếu khi chạy Unit Test cho `BookService` mà bạn lại kết nối vào SQL Server thật:
                    1. Chạy 500 bài test sẽ mất cả chục phút (vì phải chờ mạng và ổ cứng SQL).
                    2. Nếu dữ liệu trong SQL Server bị ai đó xóa mất cuốn sách số 5, bài test của bạn tự nhiên bị báo đỏ dù code C# trong `BookService` viết hoàn toàn đúng!
                    3. Nguy hiểm nhất: Chạy test hàm `Delete` sẽ xóa mất dữ liệu thật trong Database!
                  - **Giải pháp `Mocking` (Thư viện `Moq`) — Nhờ tới sức mạnh của `Interface` (`IBookRepository`):**
                    Thay vì truyền `BookRepository` thật (nối với SQL Server), ta dùng thư viện **Moq** tạo ra một **"Diễn viên đóng thế giả lập" (`new Mock<IBookRepository>()`)** chạy 100% trên RAM! Bạn có thể ra lệnh cho diễn viên đóng thế này: *"Hễ `BookService` gọi `GetByIdAsync(1)` thì cậu cứ giả vờ trả về cuốn sách 'Lập trình C#' có tồn kho = 10 trên RAM ngay lập tức nhé!"*.
                • **🖼️ 2. Sơ đồ Cơ Chế Hoạt Động Của `Mock<IBookRepository>` Khi Test `BookService`:**
                ```
                [Khi chạy Web thật]:
                BookService ──(gọi)──► IBookRepository ──► BookRepository thật ──► SQL Server thật (Chậm, phụ thuộc DB)

                [Khi chạy Unit Test với Moq]:
                BookService ──(gọi)──► IBookRepository ──► Mock<IBookRepository>.Object (Diễn viên giả lập trên RAM!)
                                                             ├── Setup: Giả vờ trả về cuốn sách mẫu trong 0.001 giây!
                                                             └── Verify: Kiểm tra xem Service có gọi hàm AddAsync đúng 1 lần không!
                ```
                • **📊 3. Ba Câu Lệnh Thần Thánh Của Thư Viện `Moq`:**
                | Câu lệnh Moq | Ý nghĩa thực hiện |
                | :--- | :--- |
                | **`var mockRepo = new Mock<IBookRepository>();`** | Tạo một đối tượng giả lập thực thi giao diện `IBookRepository`. |
                | **`mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(fakeBook);`** | **Dạy kịch bản:** Hễ ai gọi `GetByIdAsync(1)` thì lập tức trả về `fakeBook` (không cần DB!). |
                | **`mockRepo.Object`** | Lấy ra đối tượng thật từ vỏ bọc Mock để truyền vào Constructor của `new BookService(mockRepo.Object)`. |
                | **`mockRepo.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);`** | **Kiểm tra hành vi:** Xác nhận xem bên trong `BookService` có thực sự gọi xuống hàm `AddAsync` đúng **1 lần (`Times.Once`)** hay không! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Arrange: Tạo Mock<IBookRepository> và dạy kịch bản bằng .Setup()",
                        "📥 Đầu vào: Cần test hàm GetByIdAsync(1) của BookService. ⚙️ Cơ chế ngầm: Dùng new Mock<IBookRepository>(), gọi .Setup(...).ReturnsAsync(...) để giả lập rằng trong kho đang có cuốn sách mã số 1. 📤 Đầu ra: Đối tượng mockRepo đã thuộc kịch bản.",
                        "// 1. Arrange: Tạo Mock Repository và cấu hình kết quả trả về giả lập\nvar mockRepo = new Mock<IBookRepository>();\nvar fakeBook = new Book { Id = 1, Title = \"Làm chủ .NET 10\", Price = 150000m };\n\nmockRepo.Setup(repo => repo.GetByIdAsync(1))\n        .ReturnsAsync(fakeBook);"),
                    Step(2,
                        "2. Act: Tiêm mockRepo.Object vào BookService và chạy hàm",
                        "📥 Đầu vào: Truyền mockRepo.Object vào Constructor của BookService. ⚙️ Cơ chế ngầm: Khi BookService gọi _repo.GetByIdAsync(1), đối tượng Mock lập tức nhả ra fakeBook trên RAM mà không hề đụng tới SQL Server! 📤 Đầu ra: Kết quả BookDto.",
                        "// Tiêm đối tượng giả lập qua thuộc tính .Object:\nvar bookService = new BookService(mockRepo.Object);\n\n// 2. Act: Chạy hàm của Service\nvar resultDto = await bookService.GetBookDetailAsync(1);"),
                    Step(3,
                        "3. Assert & Verify: Kiểm tra kết quả và xác nhận lời gọi hàm",
                        "📥 Đầu vào: resultDto từ Bước 2. ⚙️ Cơ chế ngầm: Kiểm tra dữ liệu DTO trả về có khớp với fakeBook không, đồng thời dùng mockRepo.Verify(...) để chắc chắn hàm GetByIdAsync(1) đã được gọi đúng 1 lần! 📤 Đầu ra: Bài test chạy xong trong 2 mili-giây!",
                        "// 3. Assert: Kiểm tra giá trị trả về\nAssert.NotNull(resultDto);\nAssert.Equal(\"Làm chủ .NET 10\", resultDto.Title);\n\n// Kiểm tra hành vi: Chắc chắn Repository đã được gọi đúng 1 lần\nmockRepo.Verify(repo => repo.GetByIdAsync(1), Times.Once);")
                },
                "Tại sao trong kiến trúc phần mềm chuyên nghiệp, các tầng Controller và Service luôn phụ thuộc vào Interface (IBookRepository) thay vì phụ thuộc trực tiếp vào class cụ thể (BookRepository)? Điều này giúp ích gì khi viết Unit Test với Moq?",
                new List<string>
                {
                    "A. Vì có Interface (IBookRepository) thì thư viện Moq mới có thể dễ dàng tạo ra đối tượng giả lập (Mock<IBookRepository>) để truyền vào Service khi chạy Unit Test, giúp kiểm thử logic của Service hoàn toàn độc lập trên RAM mà không cần kết nối Database SQL Server thật",
                    "B. Vì Interface tự động viết hết bài Test cho lập trình viên",
                    "C. Vì class cụ thể không thể chứa được phương thức",
                    "D. Vì dùng Interface giúp không cần cài đặt .NET SDK"
                },
                "A",
                "Đây chính là câu trả lời thuyết phục nhất cho câu hỏi phỏng vấn \"Tại sao phải tạo Interface cho mọi Service/Repository?\": ngoài việc giảm sự phụ thuộc (Loose Coupling), nó cho phép áp dụng kỹ thuật Mocking trong Unit Testing để cô lập hoàn toàn tầng đang được kiểm thử."
            ),

            // =====================================================================
            // BÀI 11.4
            // =====================================================================
            Build(
                topicId,
                "11.4",
                "Kiểm thử Tích hợp (Integration Testing) với EF Core InMemory / SQLite & WebApplicationFactory",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/test/integration-tests",
                """
                • **💡 1. Khi nào `Unit Test` (dùng Mock) là chưa đủ và cần đến `Integration Test` (Kiểm thử tích hợp)?**
                  - `Unit Test` kiểm tra từng bánh răng riêng lẻ (đã giả lập `Repository`). Nhưng làm sao bạn biết khi lắp ghép tất cả các bánh răng lại với nhau (`Middleware` ➔ `Routing` ➔ `Controller` ➔ `Service` ➔ `Repository` ➔ `LINQ của EF Core`), câu lệnh LINQ trong `Repository` có dịch ra SQL đúng hay không, hay `Program.cs` đã đăng ký đủ `AddScoped` chưa?
                  - **`Integration Testing` (Kiểm thử tích hợp)** sẽ khởi chạy một **Máy chủ Web ảo và Database ảo nằm ngay trên bộ nhớ RAM (`InMemory Database` hoặc `WebApplicationFactory`)** để bắn thử một HTTP Request thật xuyên suốt từ đầu đến cuối luồng!
                • **🖼️ 2. Sơ đồ Sự Khác Biệt Giữa `Unit Test (Moq)` và `Integration Test (WebApplicationFactory)`:**
                ```
                [1. Unit Test (Cô lập 1 tầng)]:
                Test ──► BookService ──► [Mock IBookRepository (Giả lập)] (Chỉ test mỗi logic trong BookService)

                [2. Integration Test (Chạy xuyên suốt toàn bộ hệ thống trên RAM)]:
                HttpClient ảo ──(GET /api/books)──► Program.cs ──► BooksController ──► BookService ──► BookRepository ──► DbContext (InMemory DB)
                (Kiểm tra được cả Routing, DI Container, Validation, Mapping và câu lệnh LINQ của EF Core!)
                ```
                • **📊 3. Hai Công Cụ Cốt Lõi Của Kiểm Thử Tích Hợp Trong .NET:**
                | Công cụ | Gói NuGet cần cài | Công dụng thực tế |
                | :--- | :--- | :--- |
                | **`UseInMemoryDatabase`** | `Microsoft.EntityFrameworkCore.InMemory` | Tạo một `AppDbContext` thật nhưng lưu bảng trên RAM thay vì SQL Server — giúp bạn test trực tiếp các câu lệnh LINQ trong `BookRepository`! |
                | **`WebApplicationFactory<Program>`**| `Microsoft.AspNetCore.Mvc.Testing` | Khởi tạo toàn bộ Web API của bạn trên bộ nhớ và cấp một `HttpClient` ảo để gọi `await client.GetAsync("/api/books")` y hệt như Postman! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo AppDbContext ảo trên RAM bằng UseInMemoryDatabase",
                        "📥 Đầu vào: Cần kiểm tra hàm tìm kiếm bằng LINQ bên trong BookRepository. ⚙️ Cơ chế ngầm: Khởi tạo DbContextOptionsBuilder dùng UseInMemoryDatabase(Guid.NewGuid().ToString()) để mỗi bài test có 1 Database sạch mới tinh trên RAM. 📤 Đầu ra: Đối tượng AppDbContext hoạt động y như thật.",
                        "var options = new DbContextOptionsBuilder<AppDbContext>()\n    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // DB riêng biệt cho mỗi test\n    .Options;\n\nusing var context = new AppDbContext(options);\ncontext.Books.Add(new Book { Id = 1, Title = \"C# Nâng Cao\", Price = 200000m });\nawait context.SaveChangesAsync();"),
                    Step(2,
                        "2. Chạy hàm Repository thật trên InMemory Database",
                        "📥 Đầu vào: Đối tượng context ở Bước 1 đã có sẵn 1 cuốn sách. ⚙️ Cơ chế ngầm: Khởi tạo BookRepository(context) thật (không dùng Mock!) và gọi hàm tìm kiếm để kiểm tra câu lệnh LINQ. 📤 Đầu ra: Kết quả truy vấn thật.",
                        "var repo = new BookRepository(context);\nvar books = await repo.SearchByKeywordAsync(\"Nâng Cao\");\n\nAssert.Single(books); // Khẳng định tìm thấy đúng 1 cuốn sách\nAssert.Equal(200000m, books[0].Price);"),
                    Step(3,
                        "3. Bắn HTTP Request toàn luồng bằng WebApplicationFactory",
                        "📥 Đầu vào: Class Program của dự án Web API. ⚙️ Cơ chế ngầm: WebApplicationFactory khởi chạy toàn bộ Pipeline của ứng dụng trên RAM và tạo HttpClient để bắn Request kiểm tra mã 200 OK. 📤 Đầu ra: Kiểm chứng toàn bộ hệ thống ghép nối trơn tru.",
                        "public class BooksApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>> {\n    private readonly HttpClient _client;\n    public BooksApiIntegrationTests(WebApplicationFactory<Program> factory) {\n        _client = factory.CreateClient();\n    }\n\n    [Fact]\n    public async Task GetBooks_ReturnsSuccessAndJsonContentType() {\n        var response = await _client.GetAsync(\"/api/v1/books\");\n        response.EnsureSuccessStatusCode(); // Kiểm tra Status 200-299\n    }\n}")
                },
                "Khi viết Integration Test cho tầng Repository sử dụng UseInMemoryDatabase(databaseName: ...) của EF Core, tại sao ở phần tên Database người ta luôn truyền Guid.NewGuid().ToString() thay vì viết một cái tên cố định như \"TestDb\" cho tất cả các bài test?",
                new List<string>
                {
                    "A. Để đảm bảo mỗi hàm Test khi chạy song song sẽ được cấp một vùng Database trên RAM hoàn toàn riêng biệt và mới tinh, tránh việc dữ liệu thêm/xóa ở bài Test này làm ảnh hưởng hoặc gây sai lệch kết quả của bài Test kia (Test Isolation)",
                    "B. Vì UseInMemoryDatabase bắt buộc tên phải dài đúng 36 ký tự",
                    "C. Để kết nối lên đám mây Azure",
                    "D. Để mã hóa dữ liệu trong RAM"
                },
                "A",
                "Nguyên tắc Cô lập bài kiểm thử (Test Isolation) yêu cầu các bài test không được dùng chung dữ liệu làm bẩn nhau. Đặt tên Database bằng Guid.NewGuid().ToString() đảm bảo mỗi bài test sở hữu một bộ nhớ Database độc lập 100%."
            ),

            // =====================================================================
            // BÀI 11.5
            // =====================================================================
            Build(
                topicId,
                "11.5",
                "Tự động hóa Kiểm thử API chuyên nghiệp trên Postman (Post-response Scripts, Tự động gán Token & Collection Runner)",
                "Kiểm thử & Debug",
                "https://learning.postman.com/docs/tests-and-scripts/write-scripts/test-scripts/",
                """
                • **💡 1. Nỗi khổ khi test bằng Postman thủ công & Cách các Senior tự động hóa 100%:**
                  - *Cách thủ công của người mới:* Mỗi lần bấm gọi API `Login`, bạn lại phải lấy chuột bôi đen chuỗi `accessToken` dài ngoằng ở kết quả trả về ➔ bấm `Ctrl + C` ➔ mở từng tab API `CreateBook`, `UpdateBook`, `DeleteBook` ➔ vào tab Authorization bấm `Ctrl + V` dán đè Token mới vào! Làm 20 lần một ngày cực kỳ mất thời gian!
                  - *Cách chuyên nghiệp trên Postman:* Viết **3 dòng kịch bản tự động (`Post-response Script`)** ở API `Login`: Hễ bấm `Login` thành công, Postman **tự động nhặt `accessToken` lưu vào biến môi trường `{{jwt_token}}`** dùng chung cho toàn bộ 100 API còn lại! Đồng thời tự động kiểm tra mã `200 OK` và thời gian phản hồi `< 500ms`!
                • **🖼️ 2. Sơ đồ Luồng Tự Động Hóa Kiểm Thử Bằng `Postman Collection Runner`:**
                ```
                Bấm nút "Run Collection" (Chỉ 1 cú Click chuột duy nhất!):
                  │
                  ├──► 1. Chạy API POST /login        ──► Tự động chép Token vào biến {{jwt_token}} ✅
                  ├──► 2. Chạy API POST /books        ──► Tự lấy {{jwt_token}} tạo sách mới, lưu {{new_book_id}} ✅
                  ├──► 3. Chạy API GET /books/{{id}}  ──► Kiểm tra sách vừa tạo có đúng tên không ✅
                  └──► 4. Chạy API DELETE /books/{{id}}──► Xóa dọn dẹp sách vừa test xong, kiểm tra mã 204 ✅
                ──► Xuất báo cáo tổng kết: 4/4 Passed trong 0.8 giây!
                ```
                • **📊 3. Bảng Các Đoạn Script Kiểm Thử Tự Động (`pm.test`) Quan Trọng Nhất Trên Postman:**
                | Mục đích trong tab `Scripts -> Post-response` | Đoạn mã JavaScript chuẩn trong Postman (`pm.*`) |
                | :--- | :--- |
                | **1. Tự động lưu Token sau khi Login** | `const res = pm.response.json(); pm.environment.set("jwt_token", res.accessToken);` |
                | **2. Kiểm tra mã trạng thái `200 OK`** | `pm.test("Status is 200", () => pm.response.to.have.status(200));` |
                | **3. Kiểm tra tốc độ API nhanh `< 300ms`**| `pm.test("Fast response < 300ms", () => pm.expect(pm.response.responseTime).to.be.below(300));`|
                | **4. Kiểm tra dữ liệu JSON trả về** | `pm.test("Success is true", () => pm.expect(pm.response.json().isSuccess).to.eql(true));` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Viết Script tự động bắt và lưu JWT Token tại API Login",
                        "📥 Đầu vào: API POST /api/auth/login trên Postman. ⚙️ Cơ chế ngầm: Mở tab 'Scripts -> Post-response' của API Login, đọc cục JSON trả về và dùng pm.collectionVariables.set để cất accessToken vào biến {{token}}. 📤 Đầu ra: Không bao giờ phải copy-paste Token bằng tay nữa!",
                        "// Dán vào tab Scripts -> Post-response của API Login:\nif (pm.response.code === 200) {\n    const jsonData = pm.response.json();\n    pm.collectionVariables.set(\"token\", jsonData.accessToken);\n    console.log(\"Đã tự động lưu Token mới!\");\n}"),
                    Step(2,
                        "2. Cấu hình kế thừa {{token}} cho cả thư mục Collection",
                        "📥 Đầu vào: Thư mục Collection chứa 50 API của dự án. ⚙️ Cơ chế ngầm: Bấm vào tên Collection ngoài cùng -> chọn tab Authorization -> chọn Bearer Token -> điền {{token}}. Tất cả 50 API con bên trong để chế độ 'Inherit auth from parent' sẽ tự động có sẵn Token! 📤 Đầu ra: Đồng bộ toàn bộ dự án.",
                        "// Tại tab Authorization của Collection gốc:\n// Type: Bearer Token\n// Token: {{token}}"),
                    Step(3,
                        "3. Viết các bài kiểm tra tự động (pm.test) về Tốc độ và Cấu trúc JSON",
                        "📥 Đầu vào: Phản hồi của API lấy danh sách sách. ⚙️ Cơ chế ngầm: Kiểm tra đồng thời 3 tiêu chí: Mã HTTP 200, thời gian chạy dưới 500ms, và mảng dữ liệu trả về không bị rỗng. 📤 Đầu ra: Hiện 3 thẻ PASS màu xanh lá trong tab Test Results của Postman.",
                        "pm.test(\"1. Mã phản hồi phải là 200 OK\", function () {\n    pm.response.to.have.status(200);\n});\n\npm.test(\"2. Thời gian xử lý phải nhanh dưới 500ms\", function () {\n    pm.expect(pm.response.responseTime).to.be.below(500);\n});")
                },
                "Trên phần mềm Postman, để không phải copy-paste chuỗi JWT Token thủ công sang từng API sau mỗi lần đăng nhập, bạn có thể viết đoạn lệnh pm.collectionVariables.set(\"token\", pm.response.json().accessToken); vào mục nào của request Login?",
                new List<string>
                {
                    "A. Viết vào tab 'Scripts -> Post-response' (hoặc tab 'Tests' ở các bản Postman cũ) của request Login để lệnh tự động chạy ngay khi Server trả kết quả đăng nhập về",
                    "B. Viết vào thanh địa chỉ URL",
                    "C. Viết vào file CSS của trang web",
                    "D. Viết vào SQL Server"
                },
                "A",
                "Tab Scripts -> Post-response (trước đây gọi là tab Tests) trong Postman cho phép chạy mã JavaScript ngay sau khi nhận được HTTP Response từ Server, giúp tự động trích xuất Token hoặc ID vừa tạo để truyền tiếp cho các API tiếp theo."
            ),

            // =====================================================================
            // BÀI 11.6
            // =====================================================================
            Build(
                topicId,
                "11.6",
                "Đo lường & Chẩn đoán Điểm nghẽn Hiệu năng (Profiling, EF Core SQL Logging, Stopwatch & Health Checks)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/ef/core/logging-events-diagnostics/",
                """
                • **💡 1. Nguyên tắc vàng của Tối ưu Hiệu suất: "Không bao giờ đoán mò — Hãy đo đạc trước khi sửa!"**
                  - Khi một màn hình trên web quay vòng tròn mất **5 giây** mới hiện ra dữ liệu, nguyên nhân nằm ở đâu? Do mạng chậm? Do vòng lặp C#? Hay do câu lệnh SQL thiếu Index?
                  - Để bắt tận tay kẻ làm chậm hệ thống (**Bottleneck**), bạn sử dụng 3 công cụ chẩn đoán chuẩn của .NET:
                    1. **Bật `LogTo(Console.WriteLine)` trong `DbContext`:** Nhìn tận mắt câu lệnh SQL do EF Core sinh ra và xem SQL Server chạy câu đó mất bao nhiêu mili-giây (`Executed DbCommand (1,250ms)`).
                    2. **Dùng `Stopwatch` (Đồng hồ bấm giờ độ chính xác cao):** Đo chính xác từng đoạn code C# trong Service mất bao nhiêu `ms`.
                    3. **Thiết lập `Health Checks` (`/health`):** Cổng kiểm tra sức khỏe tự động xem Server và kết nối SQL Server có đang sống khỏe (`Healthy`) hay đang bị sập (`Unhealthy`).
                • **🖼️ 2. Sơ đồ Phân Tích Thời Gian Thực Thi Của 1 HTTP Request Để Tìm Điểm Nghẽn:**
                ```
                Tổng thời gian Postman báo: 2.500 ms (2,5 giây - QUÁ CHẬM!)
                ├── Bước 1 (Controller Validation):        2 ms  (Bình thường)
                ├── Bước 2 (Truy vấn SQL lấy đơn hàng): 2.480 ms  ◄── 🚨 TÌM THẤY THỦ PHẠM! (Thiếu Index hoặc N+1 Queries!)
                └── Bước 3 (Map sang DTO trả về JSON):    18 ms  (Bình thường)
                ```
                • **📊 3. Ngưỡng Thời Gian Phản Hồi Chuẩn Của Một API Tốt Trong Thực Tế:**
                | Thời gian phản hồi (`Response Time`) | Đánh giá chất lượng | Hành động cần làm |
                | :--- | :--- | :--- |
                | **`< 100 ms`** (`0.1 giây`) | ⭐ **Xuất sắc (Chuẩn mực)** | Hệ thống đã được tối ưu rất tốt. |
                | **`100 ms - 500 ms`** | ✅ **Chấp nhận được** | Phù hợp với các API tổng hợp dữ liệu vừa phải. |
                | **`> 1.000 ms` (`> 1 giây`)** | ⚠️ **Cảnh báo Chậm!** | Cần bật SQL Logging kiểm tra lại `.AsNoTracking()`, `Index`, `Pagination` hoặc thêm `Cache`! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bật chế độ in câu lệnh SQL kèm thời gian chạy ra cửa sổ Console",
                        "📥 Đầu vào: Cấu hình AddDbContext trong Program.cs. ⚙️ Cơ chế ngầm: Thêm .LogTo(Console.WriteLine, LogLevel.Information) để mỗi khi EF Core chạy câu lệnh SQL nào, nó đều in rõ câu lệnh đó và số mili-giây thực thi ra màn hình Terminal. 📤 Đầu ra: Phát hiện ngay các câu SQL chậm hoặc bị lặp N+1.",
                        "builder.Services.AddDbContext<AppDbContext>(options => {\n    options.UseSqlServer(connectionString)\n           .LogTo(Console.WriteLine, LogLevel.Information);\n    if (builder.Environment.IsDevelopment()) {\n        options.EnableSensitiveDataLogging(); // Hiện luôn giá trị của tham số @p0 khi đang code!\n    }\n});"),
                    Step(2,
                        "2. Dùng Stopwatch.GetTimestamp() đo thời gian xử lý từng đoạn code",
                        "📥 Đầu vào: Một hàm xử lý nghiệp vụ dài nghi ngờ bị chậm. ⚙️ Cơ chế ngầm: Sử dụng Stopwatch.GetTimestamp() và Stopwatch.GetElapsedTime() (cách mới siêu nhẹ không tốn RAM của .NET 8/9/10) để đo thời gian. 📤 Đầu ra: Biết chính xác đoạn nào ngốn thời gian.",
                        "long startTime = System.Diagnostics.Stopwatch.GetTimestamp();\n\n// Đoạn code cần kiểm tra tốc độ:\nvar data = await _repo.GetHeavyReportAsync();\n\nTimeSpan elapsed = System.Diagnostics.Stopwatch.GetElapsedTime(startTime);\n_logger.LogInformation(\"Hàm báo cáo chạy hết: {TotalMs} ms\", elapsed.TotalMilliseconds);"),
                    Step(3,
                        "3. Tích hợp trạm kiểm tra sức khỏe hệ thống (Health Checks)",
                        "📥 Đầu vào: Cần một đường dẫn '/health' để biết Web và SQL Server có đang hoạt động tốt không. ⚙️ Cơ chế ngầm: Đăng ký AddHealthChecks() và MapHealthChecks(\"/health\"). 📤 Đầu ra: Gọi GET /health trả về chữ 'Healthy' (200 OK).",
                        "// Trong Program.cs:\nbuilder.Services.AddHealthChecks();\n// ...\napp.MapHealthChecks(\"/health\");")
                },
                "Khi đang phát triển ứng dụng ở môi trường Development, bạn muốn nhìn thấy câu lệnh SQL thực tế do EF Core sinh ra kèm theo giá trị cụ thể của các tham số @p0, @p1 ngay trên cửa sổ Console để kiểm tra xem vì sao truy vấn chạy chậm. Bạn cần bật tùy chọn nào trong AddDbContext?",
                new List<string>
                {
                    "A. Kết hợp .LogTo(Console.WriteLine, LogLevel.Information) và .EnableSensitiveDataLogging()",
                    "B. Gọi lệnh _db.Database.EnsureDeleted()",
                    "C. Xóa file appsettings.json",
                    "D. Tắt SQL Server đi"
                },
                "A",
                "Mặc định EF Core ẩn giá trị của các tham số @p0 trong Log để bảo mật. Khi bật .EnableSensitiveDataLogging() ở môi trường Development cùng với .LogTo(...), bạn sẽ thấy toàn bộ câu lệnh SQL hoàn chỉnh kèm thời gian thực thi tính bằng mili-giây."
            ),

            // =====================================================================
            // BÀI 11.7
            // =====================================================================
            Build(
                topicId,
                "11.7",
                "Tối ưu Hiệu năng Truy vấn Dữ liệu Cấp cao: Compiled Queries, IAsyncEnumerable & Tránh Lỗi Lọc Trên RAM (Client Evaluation)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/performance/advanced-performance-topics",
                """
                • **💡 1. Cái bẫy chết người số 1 làm sập RAM máy chủ: "Vô tình kéo cả bảng về RAM rồi mới lọc"!**
                  - Hãy so sánh 2 dòng code chỉ khác nhau đúng vị trí của chữ `.ToListAsync()` dưới đây (Giả sử bảng `Books` có **1 triệu cuốn sách**):
                • **🖼️ 2. Sơ đồ So Sánh: Lọc Tại SQL Server (`IQueryable`) vs Lọc Trên RAM (`IEnumerable`):**
                ```
                ❌ CÁCH VIẾT THẢM HỌA (Gọi ToListAsync() hoặc chuyển sang IEnumerable trước khi Where):
                   var allBooks = await _db.Books.ToListAsync();          // 1. Kéo cả 1.000.000 dòng từ SQL lên RAM Web! (Tốn 500MB RAM!)
                   var result = allBooks.Where(b => b.Id == 5).ToList();  // 2. Dùng CPU của Web quét 1 triệu vòng để nhặt ra 1 dòng!

                ✅ CÁCH VIẾT CHUẨN SENIOR (Giữ nguyên IQueryable cho đến tận phút chót!):
                   var result = await _db.Books
                       .AsNoTracking()
                       .Where(b => b.Id == 5)                             // 1. Dịch thành WHERE Id = 5 gửi xuống SQL Server!
                       .ToListAsync();                                    // 2. SQL Server chỉ gửi đúng 1 dòng (1KB) về cho RAM Web!
                ```
                • **📊 3. Ba Kỹ Thuật Tối Ưu Truy Vấn Cấp Cao Trong .NET 8 / 9 / 10:**
                | Kỹ thuật nâng cao | Bản chất hoạt động | Khi nào áp dụng? |
                | :--- | :--- | :--- |
                | **1. `EF.CompileAsyncQuery`** | Bình thường mỗi lần gọi LINQ, EF Core phải tốn thời gian dịch cây biểu thức C# sang chuỗi SQL. **Compiled Query** biên dịch sẵn câu LINQ đó 1 lần duy nhất và dùng lại mãi mãi! | Dùng cho các câu truy vấn được gọi hàng nghìn lần mỗi giây (như `GetUserById`, `GetBookByCode`). |
                | **2. `AsAsyncEnumerable()`** (`yield return`) | Thay vì đợi gom đủ 10.000 dòng vào `List` trên RAM rồi mới trả về, nó **đọc tới dòng nào thì truyền dòng đó đi ngay (Streaming)**! | Khi xuất dữ liệu lớn hoặc truyền luồng câu trả lời từng chữ của AI! |
                | **3. Không gọi hàm C# tự viết trong `.Where()` của `IQueryable`**| SQL Server không thể hiểu hàm C# tự viết của bạn (như `Where(b => TinhGia(b.Price) > 100)`) và sẽ văng lỗi `InvalidOperationException`! | Luôn viết biểu thức toán học trực tiếp (`Where(b => b.Price * 0.9m > 100)`) để EF Core dịch được sang SQL! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Compiled Query biên dịch sẵn để tăng tốc truy vấn lặp lại",
                        "📥 Đầu vào: Truy vấn lấy thông tin sách theo Id được gọi liên tục trên trang chủ. ⚙️ Cơ chế ngầm: Dùng EF.CompileAsyncQuery lưu vào một biến static readonly để câu lệnh LINQ chỉ cần dịch sang SQL đúng 1 lần duy nhất khi khởi động web. 📤 Đầu ra: Tốc độ truy vấn nhanh thêm 15-20%.",
                        "private static readonly Func<AppDbContext, int, Task<Book?>> _getBookByIdCompiled =\n    EF.CompileAsyncQuery((AppDbContext db, int id) =>\n        db.Books.AsNoTracking().FirstOrDefault(b => b.Id == id));\n\n// Khi sử dụng trong hàm Service/Repository:\npublic Task<Book?> GetFastByIdAsync(int id) => _getBookByIdCompiled(_db, id);"),
                    Step(2,
                        "2. Truyền luồng dữ liệu từng dòng (Streaming) với IAsyncEnumerable",
                        "📥 Đầu vào: Cần duyệt qua 50.000 bản ghi để xuất file mà không muốn nạp hết 50.000 dòng vào 1 cục List trên RAM. ⚙️ Cơ chế ngầm: Dùng await foreach kết hợp AsAsyncEnumerable() để đọc cuốn chiếu từng dòng từ SQL. 📤 Đầu ra: Mức tiêu thụ RAM phẳng lặng gần như bằng 0!",
                        "await foreach (var book in _db.Books.AsNoTracking().AsAsyncEnumerable()) {\n    // Đọc đến dòng nào xử lý giải phóng ngay dòng đó, không bị tích tụ trên RAM!\n    await WriteToExportStreamAsync(book);\n}"),
                    Step(3,
                        "3. Tối ưu câu lệnh tìm kiếm chuỗi (StartsWith vs Contains)",
                        "📥 Đầu vào: Tìm kiếm theo Mã sách (BookCode). ⚙️ Cơ chế ngầm: Nếu tìm theo Mã có quy tắc ở đầu (VD: 'CNTT...'), dùng .StartsWith(\"CNTT\") sẽ sinh ra LIKE 'CNTT%' tận dụng được 100% cây Index B-Tree (Index Seek), nhanh hơn rất nhiều so với .Contains() (LIKE '%CNTT%' phải quét Index Scan)! 📤 Đầu ra: Tận dụng tối đa Index SQL.",
                        "// Tận dụng được Index Seek (Nhanh gấp bội khi tìm theo tiền tố Mã):\nvar books = await _db.Books\n    .AsNoTracking()\n    .Where(b => b.BookCode.StartsWith(prefixCode))\n    .ToListAsync();")
                },
                "Khi tìm kiếm trên một cột đã được đánh Non-Clustered Index trong SQL Server (ví dụ cột BookCode), tại sao câu lệnh LINQ Where(b => b.BookCode.StartsWith(\"IT\")) lại chạy nhanh hơn rất nhiều so với Where(b => b.BookCode.Contains(\"IT\")) khi bảng có hàng triệu dòng?",
                new List<string>
                {
                    "A. Vì .StartsWith(\"IT\") dịch ra SQL là LIKE 'IT%' (không có dấu % ở đầu) nên SQL Server dùng được thuật toán tìm kiếm nhị phân Index Seek nhảy thẳng tới nhánh chữ 'IT'; còn .Contains(\"IT\") dịch ra LIKE '%IT%' (có dấu % ở đầu) bắt buộc SQL phải quét toàn bộ từ đầu đến cuối (Scan)",
                    "B. Vì hàm Contains bị lỗi trong C#",
                    "C. Vì StartsWith không cần kết nối tới SQL Server",
                    "D. Hai hàm có tốc độ hoàn toàn bằng nhau"
                },
                "A",
                "Giống như tra từ điển tiếng Anh: nếu bảo tìm các từ bắt đầu bằng chữ \"IT\" (StartsWith ➔ LIKE 'IT%'), bạn lật thẳng tới vần I ngay lập tức. Nhưng nếu bảo tìm các từ có chứa chữ \"IT\" ở giữa bất kỳ (Contains ➔ LIKE '%IT%'), bạn buộc phải đọc từng từ một từ trang đầu đến trang cuối!"
            ),

            // =====================================================================
            // BÀI 11.8
            // =====================================================================
            Build(
                topicId,
                "11.8",
                "Tối ưu Băng thông & Bộ nhớ trong C# .NET: Response Compression (Brotli/Gzip), IHttpClientFactory & Quản lý Bộ nhớ",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/performance/response-compression",
                """
                • **💡 1. Ba Kỹ Thuật Tối Ưu Cấp Hệ Thống Mà Mọi Senior .NET Đều Cấu Hình:**
                  1. **Nén dữ liệu phản hồi (`Response Compression` với `Brotli` & `Gzip`):**
                     - Giả sử API trả về một danh sách JSON hoặc trang HTML nặng **`500 KB`**. Nếu bật `UseResponseCompression()` trong `Program.cs`, ASP.NET Core sẽ tự động nén cục JSON đó bằng thuật toán **Brotli (`br`)** xuống chỉ còn **`40 KB` (giảm 90% dung lượng!)** trước khi truyền qua mạng Internet về trình duyệt! Trang web tải nhanh hơn gấp 5 lần!
                  2. **Sử dụng `IHttpClientFactory` thay vì `new HttpClient()`:**
                     - *Sai lầm kinh điển:* Mỗi lần gọi API bên ngoài (như gọi Gemini AI API) lại viết `using var client = new HttpClient();`. Khi `Dispose()`, cổng mạng vật lý (`Socket`) của Windows chưa kịp đóng ngay mà rơi vào trạng thái chờ `TIME_WAIT` trong 4 phút ➔ Gọi nhiều lần sẽ làm **cạn kiệt cổng mạng (`Socket Exhaustion`)** khiến Server mất mạng!
                     - *Giải pháp chuẩn:* Đăng ký **`builder.Services.AddHttpClient()`** để tái sử dụng kết nối mạng ngầm an toàn 100%!
                  3. **Giảm tải cho bộ dọn rác `Garbage Collector (GC)`:**
                     - Không cộng chuỗi `string +=` trong vòng lặp (dùng `StringBuilder`).
                     - Khi đã biết trước số lượng phần tử của `List<T>`, truyền sẵn sức chứa vào Constructor (`new List<Book>(100)`) để `List` không phải cấp phát và copy lại mảng ngầm nhiều lần!
                • **🖼️ 2. Sơ đồ Hiệu Quả Của Nén Dữ Liệu `Brotli Response Compression`:**
                ```
                [Khi CHƯA bật Response Compression]:
                Backend C# ──(Gửi cục JSON thô nặng 500 KB qua mạng 4G mất 800ms)──► Trình duyệt

                [Khi ĐÃ BẬT app.UseResponseCompression() (Thuật toán Brotli)]:
                Backend C# ──(Nén siêu tốc còn 45 KB -> Truyền qua mạng chỉ mất 70ms!)──► Trình duyệt tự giải nén!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bật nén dữ liệu tự động Brotli & Gzip trong Program.cs",
                        "📥 Đầu vào: Các phản hồi JSON và HTML từ Server. ⚙️ Cơ chế ngầm: Đăng ký AddResponseCompression bật cả BrotliCompressionProvider và GzipCompressionProvider cho giao thức HTTPS. 📤 Đầu ra: Giảm 70% - 90% dung lượng băng thông mạng.",
                        "builder.Services.AddResponseCompression(options => {\n    options.EnableForHttps = true;\n    options.Providers.Add<BrotliCompressionProvider>();\n    options.Providers.Add<GzipCompressionProvider>();\n});\n// ...\napp.UseResponseCompression();"),
                    Step(2,
                        "2. Quản lý kết nối gọi API bên ngoài chuẩn mực với AddHttpClient",
                        "📥 Đầu vào: Dịch vụ AIService cần gọi sang API của Google Gemini. ⚙️ Cơ chế ngầm: Đăng ký AddHttpClient<IAIService, AIService>() trong Program.cs để .NET tự động quản lý vòng đời Socket Pool, chống lỗi Socket Exhaustion. 📤 Đầu ra: Gọi API bên thứ ba ổn định tuyệt đối.",
                        "// Trong Program.cs:\nbuilder.Services.AddHttpClient<IGeminiService, GeminiService>(client => {\n    client.Timeout = TimeSpan.FromSeconds(15); // Đặt giới hạn chờ tối đa 15 giây\n});"),
                    Step(3,
                        "3. Khởi tạo List<T> với Capacity định trước khi biết số lượng",
                        "📥 Đầu vào: Biến đổi mảng 1.000 phần tử sang List mới. ⚙️ Cơ chế ngầm: Truyền con số 1000 vào new List<BookDto>(1000) để cấp phát đúng 1 lần duy nhất trên Heap thay vì phải nhân đôi mảng từ 4 -> 8 -> 16 -> ... -> 1024. 📤 Đầu ra: Tiết kiệm CPU và giảm rác bộ nhớ.",
                        "var dtoList = new List<BookDto>(entities.Count); // Cấp phát đúng kích thước ngay từ đầu!\nforeach (var item in entities) {\n    dtoList.Add(new BookDto(item.Id, item.Title));\n}")
                },
                "Tại sao khi cần gọi tới các API bên thứ ba (như gọi Gemini API, cổng thanh toán VNPAY) trong ASP.NET Core, Microsoft khuyến cáo KHÔNG NÊN viết using (var client = new HttpClient()) lặp đi lặp lại trong mỗi hàm, mà phải đăng ký builder.Services.AddHttpClient() (IHttpClientFactory)?",
                new List<string>
                {
                    "A. Vì việc khởi tạo và hủy (Dispose) đối tượng HttpClient liên tục sẽ giữ các cổng kết nối mạng ở trạng thái chờ TIME_WAIT, dẫn đến lỗi cạn kiệt cổng mạng (Socket Exhaustion) khi có nhiều người dùng; IHttpClientFactory giúp tái sử dụng các kết nối mạng ngầm một cách an toàn",
                    "B. Vì từ khóa new không dùng được với HttpClient",
                    "C. Vì new HttpClient() chỉ gọi được phương thức GET chứ không gọi được POST",
                    "D. Vì IHttpClientFactory giúp không cần mạng Internet vẫn gọi được API"
                },
                "A",
                "Đây là câu hỏi phỏng vấn nâng cao rất hay gặp: HttpClient dù thực thi IDisposable, nhưng việc new và Dispose nó liên tục trong mỗi Request sẽ gây ra lỗi Socket Exhaustion. Dùng IHttpClientFactory (AddHttpClient) là giải pháp chuẩn duy nhất trong .NET Core."
            ),

            // =====================================================================
            // BÀI 11.9
            // =====================================================================
            Build(
                topicId,
                "11.9",
                "Phòng chống Rò rỉ bộ nhớ (Memory Leaks) & Tối ưu tải Tài nguyên Tĩnh (Static Files, Async/Await & Background Tasks)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/core/diagnostics/debug-memory-leak",
                """
                • **💡 1. Tại sao C# đã có bộ dọn rác tự động (`Garbage Collector - GC`) rồi mà Server vẫn có thể bị "Tràn RAM" (`Memory Leak`)?**
                  - Bộ dọn rác `GC` chỉ dám xóa những đối tượng **không còn ai giữ sợi dây tham chiếu tới nó nữa**.
                  - Nếu bạn vô tình tạo ra một biến **sống vĩnh viễn từ lúc bật web đến lúc tắt web** (như một danh sách **`static List<...>`** hoặc một dịch vụ **`Singleton`**) và cứ mỗi lần có người dùng truy cập bạn lại `.Add()` thêm dữ liệu vào danh sách `static` đó mà không bao giờ xóa đi ➔ Bộ dọn rác `GC` tưởng bạn vẫn đang cần dùng nên **không bao giờ dám dọn**! Sau vài ngày chạy, RAM Server phình lên 16GB rồi sập (`OutOfMemoryException`)!
                • **🖼️ 2. Sơ đồ 3 Nguyên Nhân Gây Rò Rỉ Bộ Nhớ (`Memory Leak`) Điển Hình Trong .NET:**
                ```
                1. Biến Static / Singleton phình to vô hạn:
                   private static readonly List<string> _logs = new();
                   ──► Cứ mỗi Request lại _logs.Add(...) mà không xóa ──► Phình RAM đến chết!

                2. Lưu Cache trên RAM (IMemoryCache) nhưng QUÊN đặt thời gian hết hạn (Expiration):
                   ──► Dữ liệu lưu vào RAM nằm lì ở đó mãi mãi không giải phóng!

                3. Bắt dịch vụ Scoped (như DbContext) nhét vào trong dịch vụ Singleton (Captive Dependency!):
                   Singleton (Sống vĩnh viễn) ──(giữ chặt)──► DbContext (Đáng lẽ phải hủy sau mỗi Request!)
                   ──► DbContext không thể giải phóng được và tích tụ hàng triệu bản ghi trong ChangeTracker!
                ```
                • **📊 3. Xử lý các tác vụ nặng tốn thời gian bằng `BackgroundService` (`IHostedService`) thay vì bắt người dùng đứng chờ:**
                  - Nếu người dùng bấm nút *"Đặt hàng"* mà bạn bắt họ đứng chờ gửi Email hóa đơn + tạo file PDF mất 5 giây rồi mới trả kết quả, trải nghiệm sẽ rất tệ!
                  - **Giải pháp:** Lưu đơn hàng vào DB xong là trả về `200 OK` cho khách ngay lập tức (mất `50ms`), còn việc gửi Email đẩy vào hàng đợi chạy ngầm ở hậu trường bằng **`BackgroundService`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Luôn đặt thời gian hết hạn (Expiration) và giới hạn khi dùng IMemoryCache",
                        "📥 Đầu vào: Lưu dữ liệu vào bộ nhớ đệm IMemoryCache. ⚙️ Cơ chế ngầm: Luôn thiết lập cả SlidingExpiration (không ai hỏi trong 5 phút thì xóa) và AbsoluteExpirationRelativeToNow (tối đa 30 phút là xóa) để RAM luôn được giải phóng. 📤 Đầu ra: Không bao giờ bị tràn RAM do Cache.",
                        "var cacheOptions = new MemoryCacheEntryOptions()\n    .SetSlidingExpiration(TimeSpan.FromMinutes(5))\n    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));\n\n_cache.Set(cacheKey, data, cacheOptions);"),
                    Step(2,
                        "2. Tránh lỗi Captive Dependency (Không tiêm Scoped vào Singleton)",
                        "📥 Đầu vào: Một dịch vụ Singleton (hoặc BackgroundService) cần dùng AppDbContext (vốn là Scoped). ⚙️ Cơ chế ngầm: Không tiêm trực tiếp AppDbContext vào Constructor của Singleton, mà tiêm IServiceScopeFactory để tạo scope ngắn hạn dùng xong hủy ngay! 📤 Đầu ra: DbContext được giải phóng đúng chuẩn.",
                        "public class MySingletonWorker {\n    private readonly IServiceScopeFactory _scopeFactory;\n    public MySingletonWorker(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;\n\n    public async Task DoWorkAsync() {\n        using var scope = _scopeFactory.CreateScope(); // Tạo scope ngắn hạn\n        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();\n        await db.SaveChangesAsync();\n    } // Ra khỏi hàm là scope và db tự động Dispose!\n}"),
                    Step(3,
                        "3. Bật Cache Header cho Tài nguyên tĩnh (Ảnh, CSS, JS trong wwwroot)",
                        "📥 Đầu vào: Các file logo, hình ảnh, bootstrap.css. ⚙️ Cơ chế ngầm: Cấu hình StaticFileOptions gắn Header Cache-Control để trình duyệt của người dùng lưu sẵn file ảnh/CSS ở lần vào đầu tiên; các lần chuyển trang sau không cần tải lại từ Server nữa! 📤 Đầu ra: Tải trang tức thì.",
                        "app.UseStaticFiles(new StaticFileOptions {\n    OnPrepareResponse = ctx => {\n        // Bảo trình duyệt lưu đệm file tĩnh (ảnh, css, js) trong 7 ngày (604800 giây):\n        ctx.Context.Response.Headers.Append(\"Cache-Control\", \"public,max-age=604800\");\n    }\n});")
                },
                "Hiện tượng lỗi kiến trúc gọi là \"Captive Dependency\" (Phụ thuộc bị giam cầm) gây rò rỉ bộ nhớ và lỗi xung đột dữ liệu trong ASP.NET Core xảy ra khi nào?",
                new List<string>
                {
                    "A. Khi bạn tiêm trực tiếp một dịch vụ có vòng đời ngắn (như AppDbContext - Scoped) vào bên trong Constructor của một dịch vụ có vòng đời vĩnh viễn (Singleton), khiến đối tượng Scoped bị giữ chặt không bao giờ được giải phóng trong suốt thời gian chạy Server",
                    "B. Khi bạn tiêm một dịch vụ Singleton vào trong Controller",
                    "C. Khi bạn sử dụng từ khóa async/await",
                    "D. Khi bạn đặt tên class quá dài"
                },
                "A",
                "Quy tắc vòng đời DI: dịch vụ ngắn hạn (Scoped) có thể dùng dịch vụ dài hạn (Singleton), nhưng dịch vụ Singleton tuyệt đối không được tiêm trực tiếp dịch vụ Scoped vào Constructor (mà phải dùng IServiceScopeFactory hoặc IDbContextFactory)."
            ),

            // =====================================================================
            // BÀI 11.10
            // =====================================================================
            Build(
                topicId,
                "11.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 11 (Quy trình Kiểm thử TDD & Checklist Tối ưu Toàn diện)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/aspnet/core/performance/performance-best-practices",
                """
                • **💡 1. Tổng kết Quy trình Kiểm thử & Tối ưu Hóa Hiệu Suất Chuẩn Doanh Nghiệp:**
                ```
                ╔══════════════════════════════════════════════════════════════════════════════╗
                ║ PHẦN A - ĐẢM BẢO CHẤT LƯỢNG CODE (TESTING):                                  ║
                ║   1. Unit Test (xUnit + Moq): Áp dụng chuẩn 3A (Arrange - Act - Assert),     ║
                ║      dùng [Fact], [Theory]/[InlineData] và Mock<IRepository> kiểm tra Service║
                ║   2. Integration Test: Dùng InMemoryDatabase / WebApplicationFactory kiểm tra║
                ║      truy vấn LINQ và toàn bộ đường ống HTTP Request.                        ║
                ║   3. Postman Automation: Dùng Post-response Scripts tự động lưu {{token}}    ║
                ║      và kiểm tra tốc độ phản hồi < 300ms.                                    ║
                ╠══════════════════════════════════════════════════════════════════════════════╣
                ║ PHẦN B - TỐI ƯU HÓA HIỆU SUẤT (PERFORMANCE OPTIMIZATION):                    ║
                ║   1. Tầng Database & EF Core: Index cột khóa ngoại/tìm kiếm, AsNoTracking(), ║
                ║      Projection (.Select sang DTO), Phân trang Skip/Take, Tránh N+1 Queries. ║
                ║   2. Tầng Backend C#: Async/Await + CancellationToken, IMemoryCache /        ║
                ║      OutputCache (có đặt hạn Expiration), IHttpClientFactory.                ║
                ║   3. Tầng Mạng & Giao diện: Nén Brotli/Gzip (UseResponseCompression),        ║
                ║      Cache-Control cho file tĩnh trong wwwroot.                              ║
                ╚══════════════════════════════════════════════════════════════════════════════╝
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Viết hàm Nghiệp vụ trong Service đã được tối ưu hóa",
                        "📥 Đầu vào: Yêu cầu lấy chi tiết sách có áp dụng MemoryCache. ⚙️ Cơ chế ngầm: Kiểm tra trong IMemoryCache trước, nếu chưa có mới gọi IBookRepository lấy từ DB và lưu Cache 10 phút. 📤 Đầu ra: Hàm Service đạt hiệu năng tối đa.",
                        "public async Task<BookDto?> GetBookOptimizedAsync(int id) {\n    string key = $\"BOOK_{id}\";\n    if (_cache.TryGetValue(key, out BookDto? cachedDto))\n        return cachedDto;\n\n    var entity = await _bookRepo.GetByIdAsync(id);\n    if (entity == null) return null;\n\n    var dto = new BookDto(entity.Id, entity.Title, entity.Price);\n    _cache.Set(key, dto, TimeSpan.FromMinutes(10));\n    return dto;\n}"),
                    Step(2,
                        "2. Viết Unit Test kịch bản Thành công với xUnit & Moq",
                        "📥 Đầu vào: Kiểm tra hàm GetBookOptimizedAsync khi sách tồn tại. ⚙️ Cơ chế ngầm: Tạo Mock<IBookRepository> trả về cuốn sách mẫu, dùng MemoryCache thật trên RAM và gọi hàm. 📤 Đầu ra: Khẳng định kết quả trả về đúng tên sách.",
                        "[Fact]\npublic async Task GetBookOptimizedAsync_WhenBookExists_ReturnsDtoAndCachesIt() {\n    // 1. Arrange\n    var mockRepo = new Mock<IBookRepository>();\n    mockRepo.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(new Book { Id = 10, Title = \"Clean Code\", Price = 100m });\n    var cache = new MemoryCache(new MemoryCacheOptions());\n    var service = new BookService(mockRepo.Object, cache);"),
                    Step(3,
                        "3. Thực thi gọi hàm 2 lần liên tiếp để kiểm chứng hiệu quả của Cache",
                        "📥 Đầu vào: Gọi GetBookOptimizedAsync(10) lần 1 và lần 2. ⚙️ Cơ chế ngầm: Lần 1 chưa có Cache nên gọi xuống mockRepo; Lần 2 đã có sẵn trong Cache nên trả về ngay mà KHÔNG gọi xuống mockRepo nữa! 📤 Đầu ra: Chứng minh Cache hoạt động thật 100%.",
                        "    // 2. Act: Gọi hàm 2 lần liên tiếp\n    var firstCall = await service.GetBookOptimizedAsync(10);\n    var secondCall = await service.GetBookOptimizedAsync(10);"),
                    Step(4,
                        "4. Dùng mockRepo.Verify chứng minh DB chỉ bị hỏi ĐÚNG 1 LẦN!",
                        "📥 Đầu vào: Đối tượng mockRepo. ⚙️ Cơ chế ngầm: Mặc dù hàm Service được gọi 2 lần ở Bước 3, nhưng lệnh Verify(..., Times.Once) khẳng định Repository chỉ bị gọi đúng 1 lần duy nhất nhờ có Cache đỡ cho lần thứ 2! 📤 Đầu ra: Bài Test hoàn hảo điểm 10.",
                        "    // 3. Assert & Verify\n    Assert.Equal(\"Clean Code\", secondCall?.Title);\n    // Dù gọi Service 2 lần, nhưng Repository chỉ bị gọi ĐÚNG 1 LẦN DUY NHẤT:\n    mockRepo.Verify(r => r.GetByIdAsync(10), Times.Once);\n}")
                },
                "(Câu hỏi tổng hợp Chủ đề 11) Trong bài Unit Test ở trên, hàm service.GetBookOptimizedAsync(10) được gọi 2 lần liên tiếp, nhưng ở cuối bài test câu lệnh mockRepo.Verify(r => r.GetByIdAsync(10), Times.Once); (kiểm tra Repository chỉ được gọi đúng 1 lần) lại Pass (Thành công). Điều này chứng minh được điều gì?",
                new List<string>
                {
                    "A. Chứng minh cơ chế Caching trong hàm Service đã hoạt động chính xác: Lần gọi đầu tiên lấy từ Repository và lưu vào Cache; lần gọi thứ hai lấy thẳng kết quả từ Cache trả về mà không cần truy vấn xuống Repository/Database lần nữa",
                    "B. Chứng minh hàm Verify bị hỏng",
                    "C. Chứng minh lần gọi thứ hai bị lỗi không chạy",
                    "D. Chứng minh Database đã bị xóa"
                },
                "A",
                "Đây là cách các Senior Developer dùng Unit Test (Moq.Verify với Times.Once hoặc Times.Never) để kiểm chứng chính xác xem logic bộ nhớ đệm (Caching) có thực sự cắt giảm được số lần gọi xuống Database hay không."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (11.1 -> 11.10) cho Chủ đề 11 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic11 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 11);
        if (topic11 == null)
            return;

        topic11.Title = "Chủ đề 11: Kiểm thử ứng dụng và tối ưu hóa hiệu suất";
        topic11.Description = "10 Bài học con siêu chi tiết từ Kim tự tháp Kiểm thử & Quy tắc 3A, xUnit ([Fact]/[Theory]/[InlineData]), Mocking với Moq, Integration Test (InMemory DB & WebApplicationFactory), Tự động hóa Postman, Profiling (SQL Logging/Stopwatch/Health Checks), Compiled Queries & IAsyncEnumerable, Brotli/Gzip & IHttpClientFactory đến phòng chống Memory Leak & Captive Dependency.";

        var targetLessons = GetLessons(topic11.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic11.Id)
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
