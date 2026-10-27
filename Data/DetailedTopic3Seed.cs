using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic3Seed
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
            // BÀI 3.1
            // =====================================================================
            Build(
                topicId,
                "3.1",
                "Tại sao cần Collection? So sánh Non-Generic (ArrayList) và Generic Collection (List<T>) - Bản chất Boxing & Unboxing",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/generics/",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Ở Chủ đề 1, bạn đã học **Mảng (`Array` - `int[]`)**. Nhược điểm chí mạng của Mảng là **kích thước cố định**: lúc tạo `new int[5]` là đóng đinh đúng 5 ô, muốn nhét thêm phần tử thứ 6 là bất khả thi.
                  - Để giải quyết, .NET sinh ra **Collection (Bộ sưu tập động)** có khả năng **tự động co giãn kích thước** khi bạn thêm hoặc xóa phần tử.
                  - Lịch sử .NET trải qua 2 thế hệ Collection:
                    1. **Thế hệ cũ - Non-Generic (`System.Collections` như `ArrayList`, `Hashtable`):** Giống như một **cái bao tải thập cẩm**, bạn nhét cái gì vào cũng được (số nguyên, chuỗi, ngày tháng, đối tượng trộn lẫn vào nhau) vì nó coi mọi thứ là kiểu `object`.
                    2. **Thế hệ mới - Generic (`System.Collections.Generic` như `List<T>`, `Dictionary<TKey, TValue>`):** Giống như **hộp chuyên dụng có dán nhãn `<T>`**. Khi khai báo `List<int>`, chiếc hộp này khóa chặt chỉ nhận đúng số `int`, cố tình nhét chữ `"abc"` vào sẽ bị trình biên dịch báo lỗi đỏ ngay lập tức!
                • **🖼️ 2. Sơ đồ Bộ nhớ: Tại sao `ArrayList` (Non-Generic) lại chậm và nguy hiểm? (Bản chất Boxing & Unboxing):**
                ```
                [ArrayList - Non-Generic] -> Phải gói số int (Stack) vào hộp object trên Heap (BOXING):
                Stack (int x = 10)  ===Boxing (Tốn CPU & RAM)===>  Heap [ Đối tượng object chứa 10 ]
                Khi lấy ra tính toán: Phải đập hộp ép kiểu ngược lại (int)list[0] (UNBOXING -> Dễ sập lỗi!)

                [List<int> - Generic] -> Lưu thẳng số int nguyên bản, KHÔNG Boxing, nhanh gấp chục lần:
                Heap của List<int>: [ 10 | 20 | 30 | 40 ] (An toàn kiểu 100% ngay lúc gõ code)
                ```
                • **📊 3. Bảng So sánh Chi tiết: `Array` vs `ArrayList` (Non-Generic) vs `List<T>` (Generic):**
                | Tiêu chí | Mảng (`int[]`) | `ArrayList` (Non-Generic) | `List<T>` (Generic - Chuẩn hiện đại) |
                | :--- | :--- | :--- | :--- |
                | **Kích thước** | Cố định, không thể thêm/bớt ô. | Tự động co giãn linh hoạt. | Tự động co giãn linh hoạt. |
                | **An toàn kiểu (Type Safety)** | Cao (chỉ chứa 1 kiểu). | **Rất kém** (chứa tạp nham `object`, dễ lỗi lúc chạy). | **Tuyệt đối** (nhờ ký hiệu `<T>` kiểm tra ngay lúc biên dịch). |
                | **Hiệu năng (Performance)** | Nhanh nhất. | **Chậm** (do bị vướng quá trình *Boxing* và *Unboxing*). | **Rất nhanh** (không tốn chi phí Boxing/Unboxing). |
                • **⚠️ 4. Quy tắc Thực chiến & Phỏng vấn:**
                  - Trong các dự án C# hiện đại, **TUYỆT ĐỐI KHÔNG BAO GIỜ dùng `ArrayList` hay `Hashtable`** nữa. Luôn luôn dùng các lớp trong thư viện `System.Collections.Generic` (`List<T>`, `Dictionary<K,V>`).
                  - Khi đi phỏng vấn, nếu được hỏi *"Boxing và Unboxing là gì?"*, hãy trả lời: **Boxing** là quá trình chuyển một biến Tham trị (`int`, `double` trên Stack) thành kiểu Tham chiếu (`object` trên Heap); **Unboxing** là quá trình ép kiểu ngược lại từ `object` về kiểu Tham trị ban đầu.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Nhược điểm của ArrayList (Boxing & Unboxing)",
                        "📥 Đầu vào: Số nguyên 10 và chuỗi \"Việt\". ⚙️ Cơ chế ngầm: ArrayList bọc số 10 vào hộp object trên Heap (Boxing). Khi lấy ra tính tổng, gặp chuỗi \"Việt\" sẽ nổ lỗi InvalidCastException! 📤 Đầu ra: Nguy cơ sập chương trình lúc chạy.",
                        "using System.Collections;\nArrayList oldList = new ArrayList();\noldList.Add(10);      // Boxing: int -> object\noldList.Add(\"Việt\");  // Nhét lẫn lộn chuỗi vào cũng không báo lỗi!\n// int sum = (int)oldList[0] + (int)oldList[1]; -> NỔ LỖI SẬP APP!"),
                    Step(2,
                        "2. Khai báo Generic List<T> an toàn tuyệt đối",
                        "📥 Đầu vào: Kiểu dữ liệu T = int. ⚙️ Cơ chế ngầm: Trình biên dịch khóa chặt đầu vào, chỉ cấp phát mảng động kiểu int thuần túy. 📤 Đầu ra: Chặn đứng mọi dữ liệu sai kiểu ngay khi đang gõ phím.",
                        "using System.Collections.Generic;\nList<int> numbers = new List<int>();\nnumbers.Add(10); // Lưu thẳng int, KHÔNG Boxing\nnumbers.Add(20);\n// numbers.Add(\"Việt\"); -> Báo lỗi đỏ ngay trên Visual Studio!"),
                    Step(3,
                        "3. Tự tạo một Class Generic <T> của riêng bạn",
                        "📥 Đầu vào: Tham số kiểu <T>. ⚙️ Cơ chế ngầm: Chữ T đóng vai trò là biến đại diện cho kiểu dữ liệu sẽ truyền vào sau. 📤 Đầu ra: Tái sử dụng 1 class cho hàng trăm kiểu dữ liệu khác nhau.",
                        "public class ApiResponse<T> {\n    public bool IsSuccess { get; set; }\n    public T Data { get; set; } // T có thể là ProductDto, UserDto, hoặc int\n}")
                },
                "Lý do quan trọng nhất khiến List<int> (Generic Collection) có tốc độ thực thi nhanh hơn rất nhiều và an toàn hơn so với ArrayList (Non-Generic Collection) là gì?",
                new List<string>
                {
                    "A. Vì List<int> xác định rõ kiểu dữ liệu ngay lúc biên dịch (Type-safe), lưu trực tiếp giá trị int mà không phải thực hiện quá trình đóng gói (Boxing) sang kiểu object và mở gói (Unboxing) tốn kém bộ nhớ như ArrayList",
                    "B. Vì ArrayList chỉ chứa được tối đa 10 phần tử",
                    "C. Vì List<int> lưu dữ liệu ở trong ổ cứng thay vì trên RAM",
                    "D. Vì ArrayList không dùng được vòng lặp foreach"
                },
                "A",
                "Nhờ cơ chế Generic <T>, List<int> làm việc trực tiếp với kiểu int nguyên thủy, loại bỏ hoàn toàn chi phí cấp phát bộ nhớ trên Heap của quá trình Boxing/Unboxing và ngăn chặn lỗi ép kiểu sai lúc chạy chương trình."
            ),

            // =====================================================================
            // BÀI 3.2
            // =====================================================================
            Build(
                topicId,
                "3.2",
                "Làm chủ List<T> (Danh sách động): Cơ chế bộ nhớ (Count vs Capacity) & Toàn bộ các thao tác xử lý",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.collections.generic.list-1",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - `List<T>` là cấu trúc dữ liệu được sử dụng **nhiều số 1 (chiếm tới 80%)** trong mọi dự án C#. Hãy hình dung `List<T>` như một **danh sách lớp học thông minh**: có đánh số thứ tự ghế ngồi từ `0` (`Index`), ai vào sau thì xếp ngồi nối tiếp ở cuối (`.Add`), nếu có người chuyển trường rút tên khỏi giữa danh sách (`.Remove`), những người ngồi sau sẽ tự động dồn lên 1 ghế để không bị trống chỗ!
                • **🖼️ 2. Cơ chế Bộ nhớ bên dưới lớp vỏ của `List<T>` (`Count` vs `Capacity`):**
                ```
                Khi khởi tạo: var list = new List<int>(); -> Mảng ngầm bên trong chưa cấp phát (Capacity = 0)
                Khi gọi list.Add(10), list.Add(20), list.Add(30):
                ┌───────────┬───────────┬───────────┬───────────┐
                │ Index [0] │ Index [1] │ Index [2] │ Index [3] │
                │    10     │    20     │    30     │  (Trống)  │
                └───────────┴───────────┴───────────┴───────────┘
                  ◄─────────── Count = 3 ──────────► (Số phần tử thực tế đang có)
                  ◄─────────────── Capacity = 4 ───────────────► (Sức chứa mảng ngầm)
                * Nếu Add thêm phần tử thứ 5 -> Capacity tự động NHÂN ĐÔI từ 4 lên 8 ô!
                ```
                • **📊 3. Bảng Phân Tích Toàn Bộ Các Hàm Quan Trọng Của `List<T>`:**
                | Nhóm chức năng | Tên hàm / Thuộc tính | Giải thích chi tiết cách hoạt động | Ví dụ sử dụng |
                | :--- | :--- | :--- | :--- |
                | **Đếm số lượng** | `.Count` | Trả về số phần tử thực tế đang có trong List *(Chú ý: Mảng dùng `.Length`, còn List dùng `.Count`)*. | `int n = list.Count;` |
                | **Thêm vào cuối** | `.Add(item)` | Đưa 1 phần tử vào cuối danh sách. | `names.Add("Việt");` |
                | **Thêm hàng loạt** | `.AddRange(listKhac)` | Nối toàn bộ các phần tử của một danh sách khác vào cuối danh sách hiện tại. | `names.AddRange(newStudents);` |
                | **Chèn vào vị trí** | `.Insert(index, item)` | Chèn phần tử vào vị trí `index`, đẩy các phần tử phía sau lùi lại 1 bậc. | `names.Insert(0, "Đầu bảng");` |
                | **Xóa theo giá trị** | `.Remove(item)` | Tìm và xóa phần tử **đầu tiên** bằng với `item`. Trả về `true` nếu xóa thành công. | `names.Remove("An");` |
                | **Xóa theo vị trí** | `.RemoveAt(index)` | Xóa phần tử nằm tại ô chỉ số `index`. | `names.RemoveAt(0);` |
                | **Xóa theo điều kiện**| `.RemoveAll(x => ...)` | Xóa **tất cả** các phần tử thỏa mãn điều kiện Lambda (trả về số lượng đã xóa). | `scores.RemoveAll(s => s < 5);` |
                | **Xóa sạch** | `.Clear()` | Làm rỗng toàn bộ danh sách (`Count` về `0`). | `names.Clear();` |
                | **Kiểm tra & Sắp xếp**| `.Contains(item)` / `.Sort()`| Kiểm tra có chứa phần tử không (`bool`) / Sắp xếp tăng dần tại chỗ. | `if (names.Contains("Việt"))` |
                • **⚠️ 4. Bẫy Hay Gặp & Kinh Nghiệm Thực Chiến:**
                  - **Tuyệt đối không gọi `.Remove()` bên trong vòng lặp `foreach`:** Nếu bạn đang duyệt `foreach (var x in list)` mà gọi `list.Remove(x)`, chương trình sẽ nổ lỗi `InvalidOperationException` ngay lập tức! Nếu muốn xóa theo điều kiện, hãy dùng **`list.RemoveAll(x => điều_kiện)`** hoặc duyệt ngược bằng vòng lặp `for (int i = list.Count - 1; i >= 0; i--)`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo, Thêm mới (Add, Insert) và Truy xuất Index",
                        "📥 Đầu vào: Danh sách tên môn học. ⚙️ Cơ chế ngầm: Cấp phát mảng động trên Heap, tự động mở rộng Capacity khi thêm phần tử. 📤 Đầu ra: Danh sách có thứ tự, truy cập qua [index] từ 0.",
                        "List<string> courses = new List<string> { \"C# Cơ bản\", \"OOP\" };\ncourses.Add(\"LINQ\");              // Thêm vào cuối -> Index [2]\ncourses.Insert(1, \"SQL Server\");  // Chèn vào vị trí [1], đẩy OOP sang [2]\nstring first = courses[0];        // Lấy phần tử đầu: \"C# Cơ bản\""),
                    Step(2,
                        "2. Tìm kiếm (Contains, IndexOf) và Sắp xếp (Sort)",
                        "📥 Đầu vào: Danh sách các con số điểm thi. ⚙️ Cơ chế ngầm: Sử dụng thuật toán QuickSort/IntroSort để đảo vị trí các phần tử theo thứ tự tăng dần. 📤 Đầu ra: Danh sách đã được sắp xếp gọn gàng.",
                        "List<int> points = new List<int> { 85, 40, 95, 60 };\nbool hasMax = points.Contains(95); // true\npoints.Sort();    // Sắp xếp tăng dần: 40, 60, 85, 95\npoints.Reverse(); // Đảo ngược thành giảm dần: 95, 85, 60, 40"),
                    Step(3,
                        "3. Xóa phần tử an toàn với RemoveAll",
                        "📥 Đầu vào: Danh sách điểm có cả các điểm liệt (< 50). ⚙️ Cơ chế ngầm: Quét và loại bỏ toàn bộ phần tử thỏa mãn điều kiện mà không gây lỗi vòng lặp. 📤 Đầu ra: Chỉ còn lại các điểm đạt yêu cầu.",
                        "int removedCount = points.RemoveAll(p => p < 50);\n// Đã xóa số 40 (removedCount = 1)\n// Danh sách points hiện tại còn: 95, 85, 60 (points.Count = 3)")
                },
                "Bạn có một danh sách List<Product> products. Bạn muốn xóa toàn bộ những sản phẩm đã ngừng kinh doanh (IsActive == false) ra khỏi danh sách này. Cách làm nào dưới đây sẽ VĂNG LỖI SẬP CHƯƠNG TRÌNH (InvalidOperationException)?",
                new List<string>
                {
                    "A. Dùng hàm products.RemoveAll(p => p.IsActive == false);",
                    "B. Dùng vòng lặp foreach (var p in products) { if (!p.IsActive) products.Remove(p); } (Sửa đổi kích thước List trực tiếp khi đang duyệt bằng foreach)",
                    "C. Dùng vòng lặp for chạy ngược từ cuối danh sách về 0 để gọi RemoveAt(i)",
                    "D. Lọc lấy các sản phẩm đang hoạt động gán sang danh sách mới bằng LINQ Where"
                },
                "B",
                "Bộ lặp (Enumerator) của foreach yêu cầu danh sách phải giữ nguyên trạng thái trong suốt quá trình duyệt. Gọi .Remove() hoặc .Add() bên trong foreach làm thay đổi phiên bản của List và lập tức ném ngoại lệ InvalidOperationException: Collection was modified."
            ),

            // =====================================================================
            // BÀI 3.3
            // =====================================================================
            Build(
                topicId,
                "3.3",
                "Làm chủ Dictionary<TKey, TValue> (Từ điển tra cứu siêu tốc O(1)): Cơ chế Bảng băm & TryGetValue",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/api/system.collections.generic.dictionary-2",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Giả sử trường đại học có 30.000 sinh viên. Nếu lưu trong `List<Student>`, mỗi khi muốn tìm sinh viên có mã `"SV9999"`, máy tính phải lật tìm từng người từ đầu đến cuối (mất tới 30.000 bước — độ phức tạp O(n)).
                  - **`Dictionary<TKey, TValue>` (Từ điển Cặp Khóa - Giá trị)** giải quyết bài toán này bằng cơ chế **Tủ gửi đồ siêu thị có đánh mã số (`Key`)**: Bạn cầm chìa khóa `"SV9999"` (`TKey`) cắm thẳng vào đúng ngăn tủ đó để lấy hồ sơ sinh viên (`TValue`) ra **ngay lập tức chỉ trong 1 bước duy nhất (tốc độ O(1))** bất kể danh sách có 10 người hay 10 triệu người!
                • **🖼️ 2. Sơ đồ Cấu trúc Cặp `Key - Value` (Bảng băm - Hash Table):**
                ```
                TKey (Chìa khóa - BẮT BUỘC DUY NHẤT)         TValue (Giá trị - Trùng lặp thoải mái)
                ┌──────────────────────────────────┐         ┌──────────────────────────────────────┐
                │ Key: "VN"                        │ ──────► │ Value: "Việt Nam"                    │
                ├──────────────────────────────────┤         ├──────────────────────────────────────┤
                │ Key: "JP"                        │ ──────► │ Value: "Nhật Bản"                    │
                ├──────────────────────────────────┤         ├──────────────────────────────────────┤
                │ Key: "US"                        │ ──────► │ Value: "Hoa Kỳ"                      │
                └──────────────────────────────────┘         └──────────────────────────────────────┘
                * Quy tắc sống còn: TKey KHÔNG ĐƯỢC PHÉP TRÙNG NHAU và KHÔNG ĐƯỢC NULL!
                ```
                • **📊 3. Bảng Các Thao Tác Quan Trọng Nhất Với `Dictionary<TKey, TValue>`:**
                | Thao tác | Cú pháp chuẩn | Lưu ý quan trọng (So sánh độ an toàn) |
                | :--- | :--- | :--- |
                | **Thêm bằng `.Add(k, v)`** | `dict.Add("VN", "Việt Nam");` | ⚠️ **Nguy hiểm nếu trùng Key:** Nếu `"VN"` đã có sẵn, gọi `.Add("VN", ...)` tiếp sẽ **văng lỗi sập hàm (`ArgumentException`)**! |
                | **Thêm/Cập nhật qua `[key]`** | `dict["VN"] = "Việt Nam";` | ✅ **An toàn hơn:** Nếu `"VN"` chưa có thì nó **Thêm mới**; nếu `"VN"` đã có rồi thì nó **Ghi đè giá trị mới** lên mà không báo lỗi. |
                | **Kiểm tra tồn tại Key** | `dict.ContainsKey("VN")` | Trả về `true` hoặc `false` cực nhanh (O(1)). |
                | **Lấy giá trị AN TOÀN NHẤT** | `dict.TryGetValue("VN", out var val)` | ⭐ **Khuyên dùng số 1:** Vừa kiểm tra xem Key có tồn tại không, nếu có thì lấy luôn `Value` ra biến `val` chỉ trong **1 lần tra cứu duy nhất**! |
                • **⚠️ 4. Bẫy Hay Gặp & Kinh Nghiệm Đi Làm:**
                  - **Bẫy `KeyNotFoundException`:** Nếu bạn viết `string name = dict["KR"];` mà khóa `"KR"` chưa hề có trong từ điển, chương trình sẽ nổ lỗi `KeyNotFoundException` ngay lập tức! Hãy luôn dùng **`dict.TryGetValue("KR", out string name)`**.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo và Thêm dữ liệu (Add vs Indexer [])",
                        "📥 Đầu vào: Mã sinh viên (string) làm Key và Điểm số (double) làm Value. ⚙️ Cơ chế ngầm: Băm (Hash) chuỗi Key thành vị trí ô nhớ. 📤 Đầu ra: Cặp dữ liệu được lưu trữ tối ưu.",
                        "var studentScores = new Dictionary<string, double>();\nstudentScores.Add(\"SV01\", 8.5);\nstudentScores[\"SV02\"] = 9.0;\nstudentScores[\"SV01\"] = 9.5; // Ghi đè điểm của SV01 từ 8.5 lên 9.5 an toàn!"),
                    Step(2,
                        "2. Tra cứu siêu tốc và an toàn bằng TryGetValue",
                        "📥 Đầu vào: Mã cần tìm \"SV02\". ⚙️ Cơ chế ngầm: Nhảy thẳng tới ô nhớ của \"SV02\" mà không cần quét qua các phần tử khác. 📤 Đầu ra: Lấy được điểm 9.0 an toàn tuyệt đối.",
                        "if (studentScores.TryGetValue(\"SV02\", out double score)) {\n    Console.WriteLine($\"Điểm của SV02 là: {score}\");\n} else {\n    Console.WriteLine(\"Không tìm thấy mã sinh viên này!\");\n}"),
                    Step(3,
                        "3. Duyệt qua toàn bộ Từ điển bằng KeyValuePair",
                        "📥 Đầu vào: Dictionary studentScores. ⚙️ Cơ chế ngầm: Mỗi phần tử trong vòng lặp foreach là 1 cặp KeyValuePair chứa thuộc tính .Key và .Value. 📤 Đầu ra: In ra danh sách Mã và Điểm.",
                        "foreach (KeyValuePair<string, double> item in studentScores) {\n    Console.WriteLine($\"Mã: {item.Key} - Điểm: {item.Value}\");\n}")
                },
                "Khi làm việc với Dictionary<string, int> stockDict, tại sao các lập trình viên chuyên nghiệp luôn dùng if (stockDict.TryGetValue(productCode, out int quantity)) thay vì viết 2 dòng: if (stockDict.ContainsKey(productCode)) { int quantity = stockDict[productCode]; }?",
                new List<string>
                {
                    "A. Vì TryGetValue vừa tránh được lỗi KeyNotFoundException, vừa chỉ cần băm và tra cứu bảng Dictionary ĐÚNG 1 LẦN DUY NHẤT (trong khi viết ContainsKey rồi lại gọi dict[key] sẽ phải tra cứu lặp lại 2 lần)",
                    "B. Vì ContainsKey không hoạt động với kiểu string",
                    "C. Vì TryGetValue tự động thêm phần tử mới vào Dictionary nếu không tìm thấy",
                    "D. Vì C# 10 đã cấm dùng hàm ContainsKey"
                },
                "A",
                "TryGetValue gộp cả bước kiểm tra sự tồn tại của Key và bước trích xuất Value vào làm một thao tác duy nhất, giúp tăng gấp đôi hiệu suất tra cứu và code gọn gàng, an toàn tuyệt đối."
            ),

            // =====================================================================
            // BÀI 3.4
            // =====================================================================
            Build(
                topicId,
                "3.4",
                "Các cấu trúc dữ liệu đặc biệt: HashSet<T> (Chống trùng lặp), Queue<T> (Hàng đợi FIFO) & Stack<T> (Ngăn xếp LIFO)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/collections/selecting-a-collection-class",
                """
                • **💡 1. Hiểu bản chất 3 công cụ chuyên dụng trong 30 giây:**
                  Ngoài `List` và `Dictionary`, trong lập trình thực tế có 3 bài toán kinh điển cần dùng đúng "vũ khí" chuyên biệt:
                  1. **`HashSet<T>` (Tập hợp Toán học - Chuyên gia Lọc Trùng):** Giống như danh sách điểm danh vân tay — mỗi giá trị chỉ được xuất hiện **ĐÚNG 1 LẦN DUY NHẤT**. Nếu bạn cố tình `.Add(10)` lần thứ hai, nó âm thầm bỏ qua và trả về `false` chứ không thêm trùng!
                  2. **`Queue<T>` (Hàng đợi mua vé - FIFO: First In, First Out):** **Vào trước - Ra trước**. Ai xếp hàng đầu tiên sẽ được phục vụ trước (dùng cho hàng đợi gửi Email, xử lý đơn hàng theo thứ tự).
                  3. **`Stack<T>` (Chồng đĩa / Nút Undo Ctrl+Z - LIFO: Last In, First Out):** **Vào sau cùng - Ra đầu tiên**. Chiếc đĩa nào đặt lên trên cùng sẽ được nhấc ra rửa trước (dùng cho nút *Quay lại / Undo*, lịch sử duyệt trang).
                • **🖼️ 2. Sơ đồ Trực quan Cơ chế Hoạt động:**
                ```
                [1. HashSet<int>] (Tự động loại bỏ phần tử trùng lặp):
                Đầu vào: 5, 10, 5, 20, 10  ===>  Bên trong HashSet chỉ lưu: { 5, 10, 20 }

                [2. Queue<T> - Hàng đợi FIFO (Vào trước Ra trước)]:
                Enqueue (Xếp vào cuối) ──► [ Người 3 | Người 2 | Người 1 ] ──► Dequeue (Người 1 ra trước)

                [3. Stack<T> - Ngăn xếp LIFO (Vào sau Ra trước)]:
                Push (Đặt vào đỉnh) ──► [ Trang C (Mới nhất) ] ──► Pop (Lấy Trang C ra trước tiên)
                                        [ Trang B            ]
                                        [ Trang A (Cũ nhất)  ]
                ```
                • **📊 3. Bảng Tra cứu Cú pháp của `HashSet<T>`, `Queue<T>` và `Stack<T>`:**
                | Cấu trúc | Hàm Thêm vào | Hàm Lấy ra (Và xóa khỏi danh sách) | Hàm Xem trước (Không xóa) | Ứng dụng thực tế |
                | :--- | :--- | :--- | :--- | :--- |
                | **`HashSet<T>`** | `.Add(item)` (Trả về `bool`) | `.Remove(item)` | `.Contains(item)` (Tốc độ O(1)) | Lọc danh sách Email/ID bị trùng lặp, phép giao/hợp 2 tập hợp. |
                | **`Queue<T>`** | **`.Enqueue(item)`** (Xếp hàng) | **`.Dequeue()`** (Lấy người đầu hàng) | `.Peek()` | Xử lý hàng đợi tin nhắn, thông báo, in ấn theo thứ tự. |
                | **`Stack<T>`** | **`.Push(item)`** (Đẩy lên đỉnh) | **`.Pop()`** (Nhấc phần tử trên đỉnh) | `.Peek()` | Chức năng Hoàn tác (`Ctrl + Z`), nút Back trên trình duyệt. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Lọc trùng lặp siêu tốc với HashSet<T>",
                        "📥 Đầu vào: Mảng các thẻ Tag có nhiều từ bị lặp lại. ⚙️ Cơ chế ngầm: HashSet kiểm tra mã băm của từng từ, tự động loại bỏ các từ trùng. 📤 Đầu ra: Tập hợp chỉ chứa các từ duy nhất.",
                        "var uniqueEmails = new HashSet<string>();\nuniqueEmails.Add(\"viet@gmail.com\"); // true (Thêm thành công)\nuniqueEmails.Add(\"an@gmail.com\");   // true\nbool isAdded = uniqueEmails.Add(\"viet@gmail.com\"); // false (Bị trùng, tự động bỏ qua!)\n// uniqueEmails.Count = 2"),
                    Step(2,
                        "2. Xử lý tuần tự với Hàng đợi Queue<T> (FIFO)",
                        "📥 Đầu vào: 2 yêu cầu đặt hàng liên tiếp. ⚙️ Cơ chế ngầm: Enqueue đưa vào cuối hàng, Dequeue rút yêu cầu đầu tiên ra xử lý. 📤 Đầu ra: Đảm bảo công bằng ai gửi trước xử lý trước.",
                        "var orderQueue = new Queue<string>();\norderQueue.Enqueue(\"Đơn hàng #101 (Đến lúc 8:00)\");\norderQueue.Enqueue(\"Đơn hàng #102 (Đến lúc 8:01)\");\n\nstring processing = orderQueue.Dequeue(); // Lấy ra \"Đơn hàng #101\" trước!"),
                    Step(3,
                        "3. Lưu lịch sử thao tác với Ngăn xếp Stack<T> (LIFO)",
                        "📥 Đầu vào: Lịch sử 3 thao tác gõ văn bản. ⚙️ Cơ chế ngầm: Push đẩy lên đỉnh chồng, Pop rút thao tác vừa mới làm gần nhất ra để hoàn tác (Undo). 📤 Đầu ra: Thao tác cuối cùng được lấy ra đầu tiên.",
                        "var undoStack = new Stack<string>();\nundoStack.Push(\"1. Gõ chữ Xin chào\");\nundoStack.Push(\"2. Tô đậm chữ\");\nundoStack.Push(\"3. Đổi màu đỏ (Mới nhất)\");\n\nstring lastAction = undoStack.Pop(); // Lấy ra \"3. Đổi màu đỏ\" để Undo!")
                },
                "Bạn đang phát triển tính năng gửi mã OTP cho danh sách 50.000 số điện thoại do khách hàng import từ file Excel, nhưng trong file Excel có rất nhiều số điện thoại bị trùng nhau. Cấu trúc dữ liệu nào giúp bạn vừa lưu trữ vừa tự động loại bỏ các số điện thoại trùng lặp với tốc độ kiểm tra nhanh nhất (O(1))?",
                new List<string>
                {
                    "A. Dùng HashSet<string>",
                    "B. Dùng Queue<string>",
                    "C. Dùng Stack<string>",
                    "D. Dùng mảng string[] thông thường"
                },
                "A",
                "HashSet<T> được thiết kế chuyên biệt để lưu tập hợp các phần tử duy nhất (Unique elements) dựa trên bảng băm. Bất kỳ phần tử trùng lặp nào đưa vào đều bị loại bỏ ngay lập tức với độ phức tạp O(1)."
            ),

            // =====================================================================
            // BÀI 3.5
            // =====================================================================
            Build(
                topicId,
                "3.5",
                "Hiểu tận gốc Delegate (Đại diện hàm), Func<>, Action<> và Predicate<> - Nền tảng đứng sau LINQ",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/delegates/",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Bình thường khi gọi một hàm, bạn truyền vào đó **Dữ liệu** (số `int`, chuỗi `string`, đối tượng `Product`).
                  - Nhưng bây giờ có bài toán: Bạn muốn **truyền cả một HÀM (một công thức tính toán) vào bên trong một hàm khác** như một tham số thì làm thế nào?
                  - Trong C#, **`Delegate` (Người đại diện hàm)** chính là kiểu dữ liệu dùng để **chứa địa chỉ của một Hàm**! Nhờ có `Delegate`, bạn có thể coi một Hàm giống như một biến bình thường: cất hàm vào biến và truyền hàm đó đi khắp nơi.
                • **🖼️ 2. Ba loại Delegate dựng sẵn của Microsoft (99% dự án dùng 3 loại này thay vì tự khai báo `delegate`):**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 1. Action<T1, T2...>  ──► Đại diện cho hàm KHÔNG trả về giá trị (void)     │
                │    Ví dụ: Action<string> (Nhận vào 1 chuỗi string, thực thi xong là hết)   │
                ├────────────────────────────────────────────────────────────────────────────┤
                │ 2. Func<TIn..., TOut> ──► Đại diện cho hàm CÓ trả về kết quả (TOut ở cuối) │
                │    Ví dụ: Func<int, int, double> (Nhận vào 2 số int, trả về kết quả double)│
                ├────────────────────────────────────────────────────────────────────────────┤
                │ 3. Predicate<T>       ──► Đại diện cho hàm kiểm tra điều kiện trả về BOOL  │
                │    (Hoặc Func<T, bool>)   (Nhận vào 1 đối tượng T, trả về true hoặc false) │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng Giải phẫu `Func` và `Action` (Chìa khóa để đọc hiểu mọi hàm của LINQ):**
                | Loại Delegate | Kiểu trả về của hàm | Tham số cuối cùng trong `<...>` | Ví dụ thực tế trong .NET |
                | :--- | :--- | :--- | :--- |
                | **`Action<string>`** | Luôn là **`void`** | Là tham số đầu vào (vì không có đầu ra). | Hàm in thông báo: `void Log(string msg)` |
                | **`Func<int, string>`** | Luôn có **`return`** | **Kiểu đứng CUỐI CÙNG luôn là Kiểu trả về (`string`)**, các kiểu đứng trước là đầu vào (`int`). | Hàm đổi số thành chữ: `string ReadNumber(int n)` |
                | **`Func<Product, bool>`**| Trả về **`bool`** | Nhận vào 1 `Product`, trả ra `true/false`. | **Chính là tham số nằm bên trong `.Where(...)` của LINQ!** |
                • **⚠️ 4. Điểm cốt lõi cần nhớ:**
                  - Khi bạn di chuột vào hàm `.Where(...)` của LINQ trên Visual Studio, bạn sẽ thấy nó hiện chữ: `Where(Func<Product, bool> predicate)`. Bây giờ bạn đã hiểu: Hàm `Where` chỉ đơn giản là đang yêu cầu bạn truyền vào cho nó **một hàm nhận vào `Product` và trả về `bool` (`true` thì giữ lại, `false` thì loại bỏ)**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Sử dụng Action<T> cho hàm không có giá trị trả về (void)",
                        "📥 Đầu vào: Một chuỗi thông báo. ⚙️ Cơ chế ngầm: Biến printAlert trỏ tới hàm in ra màn hình Console. 📤 Đầu ra: Thực thi hành động in chữ.",
                        "Action<string> printAlert = (message) => {\n    Console.WriteLine($\"[THÔNG BÁO]: {message}\");\n};\nprintAlert(\"Hệ thống khởi động thành công!\");"),
                    Step(2,
                        "2. Sử dụng Func<TIn, TResult> cho hàm có tính toán trả về",
                        "📥 Đầu vào: 2 số nguyên a và b. ⚙️ Cơ chế ngầm: Hai kiểu int đầu là tham số vào, kiểu int cuối cùng là kiểu trả về. 📤 Đầu ra: Tổng của hai số.",
                        "// Func<Đầu_vào_1, Đầu_vào_2, Kiểu_Trả_Về>\nFunc<int, int, int> tinhTong = (a, b) => a + b;\nint ketQua = tinhTong(15, 25); // ketQua = 40"),
                    Step(3,
                        "3. Sử dụng Func<T, bool> làm bộ lọc điều kiện động",
                        "📥 Đầu vào: Đối tượng số nguyên n. ⚙️ Cơ chế ngầm: Kiểm tra xem n có phải số chẵn không (trả về bool). 📤 Đầu ra: Truyền thẳng biến Delegate này vào hàm .Where() của LINQ!",
                        "Func<int, bool> laSoChan = n => (n % 2 == 0);\nList<int> nums = new List<int> { 1, 2, 3, 4, 5, 6 };\nvar chanList = nums.Where(laSoChan).ToList(); // Kết quả: 2, 4, 6")
                },
                "Khi nhìn thấy khai báo kiểu Func<string, int, bool> trong C#, bạn đọc hiểu chữ ký của hàm mà nó đại diện như thế nào là chính xác nhất?",
                new List<string>
                {
                    "A. Là một hàm nhận vào 2 tham số (tham số 1 kiểu string, tham số 2 kiểu int) và trả về kết quả có kiểu là bool (Kiểu đứng cuối cùng trong Func luôn là kiểu trả về)",
                    "B. Là một hàm nhận vào 3 tham số string, int, bool và không trả về gì cả (void)",
                    "C. Là một hàm trả về cùng lúc 3 giá trị",
                    "D. Là một hàm nhận vào bool và trả về string"
                },
                "A",
                "Quy tắc bất di bất dịch của Func<...>: tất cả các kiểu dữ liệu đứng trước là tham số đầu vào, còn kiểu dữ liệu đứng ở vị trí cuối cùng bên phải luôn luôn là kiểu trả về (Return Type) của hàm."
            ),

            // =====================================================================
            // BÀI 3.6
            // =====================================================================
            Build(
                topicId,
                "3.6",
                "Biểu thức Lambda Expression (=>): Sự tiến hóa từ Hàm truyền thống sang Cú pháp mũi tên siêu gọn",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/operators/lambda-expressions",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Ở Bài 3.5, bạn biết hàm `.Where()` của LINQ cần truyền vào một hàm kiểm tra điều kiện. Nếu cứ mỗi lần lọc danh sách bạn lại phải ra ngoài viết một hàm `public bool KiemTraGiaLonHon100(Product p) { return p.Price > 100; }` dài 4 dòng rồi mới truyền tên hàm vào thì quá mất thời gian!
                  - **Biểu thức Lambda (`Lambda Expression` dùng dấu mũi tên `=>`)** chính là **cách viết rút gọn tối đa một hàm ngay tại chỗ gọi** chỉ còn đúng vài ký tự: **`p => p.Price > 100`**.
                • **🖼️ 2. Sơ đồ 3 Bước Tiến Hóa Rút Gọn Thành Lambda Expression:**
                ```
                [Bước 1 - Hàm đầy đủ truyền thống (4 dòng)]:
                bool CheckHighScore(int s) {
                    return s >= 8;
                }

                [Bước 2 - Bỏ tên hàm, bỏ kiểu int (vì trình biên dịch tự biết), thay bằng mũi tên =>]:
                (s) => { return s >= 8; }

                [Bước 3 - Rút gọn tối đa (Bỏ ngoặc tròn, bỏ ngoặc nhọn và bỏ luôn chữ return)]:
                s => s >= 8
                │    │    └── Biểu thức kết quả trả về (Tự động hiểu là return s >= 8)
                │    └── Toán tử Lambda ("Đi tới" / "Sao cho")
                └── Tham số đầu vào (Đại diện cho 1 phần tử đang xét)
                ```
                • **📊 3. Bảng 4 Quy Tắc Viết Lambda Expression Bạn Cần Thuộc Lòng:**
                | Trường hợp | Cú pháp chuẩn | Giải thích |
                | :--- | :--- | :--- |
                | **1. Không có tham số đầu vào** | `() => Console.WriteLine("Hi")` | Bắt buộc phải có cặp ngoặc tròn rỗng `()` ở trước `=>`. |
                | **2. Có đúng 1 tham số (Phổ biến nhất)** | `x => x * 2` hoặc `p => p.IsActive` | Không cần ngoặc tròn `()`, không cần chữ `return`. |
                | **3. Có từ 2 tham số trở lên** | `(a, b) => a + b` | Bắt buộc phải bọc các tham số trong ngoặc tròn `(a, b)`. |
                | **4. Thân hàm dài nhiều dòng lệnh** | `x => { var y = x + 1; return y * 2; }` | Nếu mở ngoặc nhọn `{ ... }` viết nhiều dòng thì **bắt buộc phải viết chữ `return`**. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Lambda 1 tham số trên 1 dòng (Dùng 95% trong LINQ)",
                        "📥 Đầu vào: Đối tượng sinh viên 's'. ⚙️ Cơ chế ngầm: C# tự suy luận kiểu của 's' và tự động return kết quả của biểu thức bên phải dấu =>. 📤 Đầu ra: Biểu thức gọn nhất có thể.",
                        "// Lọc sinh viên có GPA từ 3.2 trở lên:\nvar goodStudents = students.Where(s => s.Gpa >= 3.2).ToList();"),
                    Step(2,
                        "2. Lambda nhiều tham số",
                        "📥 Đầu vào: 2 số nguyên x và y. ⚙️ Cơ chế ngầm: Đặt 2 tham số trong cặp ngoặc tròn (x, y) trước dấu =>. 📤 Đầu ra: Kết quả phép nhân.",
                        "Func<int, int, int> multiply = (x, y) => x * y;\nint result = multiply(6, 7); // 42"),
                    Step(3,
                        "3. Statement Lambda (Lambda nhiều dòng có ngoặc nhọn {})",
                        "📥 Đầu vào: Đối tượng Product 'p'. ⚙️ Cơ chế ngầm: Khi dùng {} để viết nhiều câu lệnh, phải chủ động dùng từ khóa return ở dòng cuối. 📤 Đầu ra: Giá trị đã qua nhiều bước tính.",
                        "var discounted = products.Select(p => {\n    decimal tax = p.Price * 0.1m;\n    decimal finalPrice = p.Price + tax;\n    return finalPrice; // Có ngoặc nhọn {} thì bắt buộc phải viết chữ return!\n}).ToList();")
                },
                "Khi viết biểu thức Lambda có sử dụng cặp dấu ngoặc nhọn { ... } để chứa nhiều dòng lệnh bên phải dấu => (ví dụ trong hàm .Where(x => { ... })), quy tắc bắt buộc nào dưới đây phải tuân thủ?",
                new List<string>
                {
                    "A. Bắt buộc phải viết rõ từ khóa return ở cuối khối ngoặc nhọn {} để trả về kết quả",
                    "B. Không được phép viết từ khóa return bên trong ngoặc nhọn {}",
                    "C. Phải đổi dấu => thành dấu ==",
                    "D. Không được phép khai báo biến bên trong ngoặc nhọn {}"
                },
                "A",
                "Với Lambda 1 biểu thức ngắn (x => x > 5), C# tự động hiểu là return x > 5. Nhưng ngay khi bạn mở cặp ngoặc nhọn x => { ... } (Statement Lambda), bạn bắt buộc phải viết từ khóa return tường minh để trả giá trị ra."
            ),

            // =====================================================================
            // BÀI 3.7
            // =====================================================================
            Build(
                topicId,
                "3.7",
                "Tổng quan về LINQ (Language Integrated Query): Hai cú pháp truy vấn & Cơ chế Trì hoãn thực thi",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Trước khi có **LINQ**, lập trình viên C# rất khổ sở: muốn lọc Mảng thì viết vòng lặp `for`, muốn hỏi SQL Server thì phải ghép chuỗi `"SELECT * FROM..."`, muốn đọc file XML lại phải học ngôn ngữ XPath.
                  - **LINQ (Language Integrated Query - Truy vấn tích hợp ngôn ngữ)** là phát minh vĩ đại của Microsoft giúp bạn dùng **DUY NHẤT một bộ cú pháp C#** (`.Where()`, `.Select()`, `.OrderBy()`) để truy vấn mọi nguồn dữ liệu: từ `List<T>` trên RAM (*LINQ to Objects*), cho đến Bảng SQL Server (*LINQ to Entities / EF Core*)!
                • **🖼️ 2. Hai cách viết LINQ trong C# (Query Syntax vs Method Syntax):**
                ```
                [Cách 1 - Query Syntax (Viết giống câu lệnh SQL)]:
                var res = from p in products
                          where p.Price > 100
                          orderby p.Name
                          select p;

                [Cách 2 - Method Syntax (Chấm nối đuôi hàm kèm Lambda -> DÙNG 99% KHI ĐI LÀM!)]:
                var res = products
                          .Where(p => p.Price > 100)
                          .OrderBy(p => p.Name)
                          .ToList();
                *(Lưu ý: Khi biên dịch, C# cũng tự dịch Cách 1 sang Cách 2. Đi làm thực tế hầu như chỉ dùng Cách 2!)*
                ```
                • **📊 3. Cơ chế "Trì hoãn thực thi" (Deferred Execution) vs "Thực thi ngay" (Immediate Execution):**
                | Chế độ thực thi | Các hàm đại diện | Chuyện gì xảy ra khi dòng code chạy qua? |
                | :--- | :--- | :--- |
                | **1. Trì hoãn thực thi (`Deferred`)** | `.Where()`, `.Select()`, `.OrderBy()`, `.Skip()`, `.Take()` | **CHƯA CHẠY THẬT!** Nó chỉ mới ghi nhận "công thức lọc" vào một biến `IEnumerable<T>` / `IQueryable<T>`. |
                | **2. Thực thi ngay lập tức (`Immediate`)**| **`.ToList()`**, **`.ToArray()`**, **`.FirstOrDefault()`**, **`.Count()`**, **`.Sum()`**, **`.Any()`** | **CHẠY NGAY LẬP TỨC!** Lúc này nó mới thực sự quét danh sách (hoặc bắn lệnh xuống SQL) để lấy kết quả chốt hạ. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo câu truy vấn bằng Method Syntax (Chưa thực thi)",
                        "📥 Đầu vào: Danh sách numbers = { 10, 20, 30 }. ⚙️ Cơ chế ngầm: Hàm Where chỉ mới lưu công thức 'lấy số > 15' chứ chưa hề duyệt danh sách. 📤 Đầu ra: Biến query kiểu IEnumerable<int>.",
                        "var numbers = new List<int> { 10, 20, 30 };\nvar query = numbers.Where(n => n > 15); // Chưa chạy lọc tại dòng này!"),
                    Step(2,
                        "2. Thêm phần tử mới vào danh sách gốc trước khi chốt",
                        "📥 Đầu vào: Thêm số 40 vào danh sách numbers. ⚙️ Cơ chế ngầm: Vì query ở Bước 1 chưa chạy, nên lát nữa khi chạy nó sẽ nhìn thấy cả số 40 mới thêm này! 📤 Đầu ra: Danh sách gốc có 4 số.",
                        "numbers.Add(40); // Thêm số 40 vào sau khi đã viết dòng query"),
                    Step(3,
                        "3. Kích hoạt thực thi ngay bằng .ToList()",
                        "📥 Đầu vào: Gọi query.ToList(). ⚙️ Cơ chế ngầm: Bây giờ vòng lặp lọc mới thực sự chạy qua { 10, 20, 30, 40 } và lấy ra tất cả số > 15. 📤 Đầu ra: Kết quả có cả 20, 30 và 40!",
                        "List<int> finalResult = query.ToList(); // Bây giờ mới chạy thật!\n// Kết quả finalResult gồm 3 số: 20, 30, 40")
                },
                "Cho danh sách var list = new List<int> { 1, 2, 3 };. Bạn viết dòng lệnh var query = list.Where(x => x >= 2);, sau đó viết tiếp list.Add(4);, và cuối cùng gọi int count = query.Count();. Hỏi biến count nhận giá trị bằng bao nhiêu?",
                new List<string>
                {
                    "A. Bằng 2 (chỉ tính số 2 và 3)",
                    "B. Bằng 3 (tính cả số 2, 3 và 4) vì hàm .Where() có cơ chế Trì hoãn thực thi (Deferred Execution), câu truy vấn chỉ thực sự chạy tại thời điểm gọi hàm .Count() ở dòng cuối cùng",
                    "C. Bằng 0",
                    "D. Báo lỗi vì không được thêm phần tử sau khi đã viết Where"
                },
                "B",
                "Nhờ cơ chế Deferred Execution, dòng list.Where(...) chỉ định nghĩa truy vấn. Đến khi .Count() được gọi, truy vấn mới quét qua list (lúc này đã có {1, 2, 3, 4}) và đếm được 3 số lớn hơn hoặc bằng 2 là 2, 3, 4."
            ),

            // =====================================================================
            // BÀI 3.8
            // =====================================================================
            Build(
                topicId,
                "3.8",
                "Nhóm hàm LINQ Lọc, Sắp xếp & Biến đổi dữ liệu: Where, OrderBy, ThenBy, Select và Distinct",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/standard-query-operators/projection-operations",
                """
                • **💡 1. Hiểu bản chất bộ tứ quyền lực nhất của LINQ trong 30 giây:**
                  - **`.Where(điều_kiện)` (Cái rây lọc):** Đổ 100 phần tử vào, chỉ giữ lại những phần tử thỏa mãn điều kiện (`true`). Số lượng đầu ra <= 100, kiểu dữ liệu giữ nguyên.
                  - **`.Select(biến_đổi)` (Máy chế biến / Chiếu cột):** Đổ 100 đối tượng `Product` vào để nhặt ra chỉ cột `Name` (thành `List<string>`) hoặc đóng gói sang hộp mới `ProductDto` (thành `List<ProductDto>`).
                  - **`.OrderBy()` / `.ThenBy()` (Máy sắp xếp đa tầng):** Sắp xếp theo tiêu chí 1 trước, nếu tiêu chí 1 bằng nhau thì mới xét tiếp tiêu chí 2 bằng `.ThenBy()`.
                  - **`.Distinct()` (Máy khử trùng):** Loại bỏ các giá trị bị lặp lại trong danh sách kết quả.
                • **🖼️ 2. Sơ đồ Sự khác biệt giữa `.Where()` (Lọc hàng ngang) và `.Select()` (Chọn/Biến đổi cột dọc):**
                ```
                Danh sách gốc List<User> (3 người, đủ cột Id, Name, Age):
                ├── .Where(u => u.Age >= 18)  ──► Giữ nguyên kiểu User, nhưng chỉ còn 2 người đủ tuổi.
                └── .Select(u => u.Name)      ──► Giữ nguyên 3 người, nhưng biến thành List<string> chỉ chứa Tên!
                ```
                • **📊 3. Bảng Cú pháp và Lưu ý Quan trọng Khi Sắp xếp Nhiều Tiêu chí:**
                | Hàm LINQ | Công dụng chi tiết | ⚠️ Bẫy cực kỳ hay gặp cần tránh |
                | :--- | :--- | :--- |
                | **`.OrderBy(x => x.A)`** | Sắp xếp tăng dần theo thuộc tính `A`. | Không bao giờ gọi 2 lần `.OrderBy().OrderBy()` liên tiếp! |
                | **`.OrderByDescending(x => x.A)`**| Sắp xếp giảm dần theo thuộc tính `A`. | Lần `.OrderBy` thứ 2 sẽ **xóa sạch** thứ tự của lần 1! |
                | **`.ThenBy(x => x.B)`** | Sắp xếp phụ tăng dần theo `B` (khi `A` bằng nhau). | **Bắt buộc dùng `.ThenBy` / `.ThenByDescending`** cho tiêu chí thứ 2 trở đi! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kết hợp Lọc (.Where) và Sắp xếp đa tầng (.OrderBy + .ThenBy)",
                        "📥 Đầu vào: Danh sách sinh viên. ⚙️ Cơ chế ngầm: Lọc sinh viên đỗ -> Xếp điểm GPA giảm dần -> Nếu GPA bằng nhau thì xếp Tên theo bảng chữ cái A-Z. 📤 Đầu ra: Danh sách đã lọc và xếp chuẩn.",
                        "var rankedList = students\n    .Where(s => s.Gpa >= 2.0)\n    .OrderByDescending(s => s.Gpa) // Tiêu chí 1: Điểm cao xếp trước\n    .ThenBy(s => s.FirstName)      // Tiêu chí 2: Cùng điểm thì xếp tên A-Z\n    .ToList();"),
                    Step(2,
                        "2. Biến đổi sang đối tượng mới bằng .Select() (Projection)",
                        "📥 Đầu vào: Danh sách Entity từ DB. ⚙️ Cơ chế ngầm: Ánh xạ từng phần tử sang class DTO gọn nhẹ chỉ chứa những trường cần thiết. 📤 Đầu ra: List<StudentSummaryDto>.",
                        "var summaries = rankedList\n    .Select(s => new StudentSummaryDto {\n        Code = s.StudentCode,\n        FullInfo = $\"{s.FullName} (GPA: {s.Gpa})\"\n    })\n    .ToList();"),
                    Step(3,
                        "3. Lấy danh sách các giá trị không trùng lặp bằng .Distinct()",
                        "📥 Đầu vào: Danh sách quê quán của 100 sinh viên. ⚙️ Cơ chế ngầm: Nhặt cột Tỉnh/Thành phố rồi dùng Distinct() loại bỏ các tên tỉnh bị lặp lại. 📤 Đầu ra: Danh sách các tỉnh duy nhất.",
                        "List<string> provinces = students\n    .Select(s => s.Province)\n    .Distinct()\n    .ToList();")
                },
                "Bạn muốn sắp xếp danh sách sản phẩm theo Giá tăng dần, và nếu hai sản phẩm có cùng mức giá thì sắp xếp theo Tên từ A đến Z. Câu lệnh LINQ nào dưới đây là ĐÚNG?",
                new List<string>
                {
                    "A. products.OrderBy(p => p.Price).OrderBy(p => p.Name).ToList();",
                    "B. products.OrderBy(p => p.Price).ThenBy(p => p.Name).ToList();",
                    "C. products.Where(p => p.Price).Select(p => p.Name).ToList();",
                    "D. products.ThenBy(p => p.Price).OrderBy(p => p.Name).ToList();"
                },
                "B",
                "Nếu gọi .OrderBy(p => p.Price).OrderBy(p => p.Name), lần OrderBy thứ hai sẽ hủy bỏ hoàn toàn thứ tự sắp xếp theo giá trước đó. Phải dùng .OrderBy(...) cho tiêu chí chính và .ThenBy(...) cho tiêu chí phụ."
            ),

            // =====================================================================
            // BÀI 3.9
            // =====================================================================
            Build(
                topicId,
                "3.9",
                "Nhóm hàm LINQ Tìm kiếm, Kiểm tra & Phân trang: FirstOrDefault, SingleOrDefault, Any, All, Skip & Take",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/standard-query-operators/quantifier-operations",
                """
                • **💡 1. Phân biệt rõ bộ tứ lấy 1 phần tử (`First`, `FirstOrDefault`, `Single`, `SingleOrDefault`):**
                  Đây là câu hỏi lý thuyết xuất hiện trong hầu hết các bài test đầu vào vị trí C# Developer. Hãy nhìn bảng ma trận dưới đây để nhớ trọn đời:
                • **📊 2. Bảng Ma Trận So Sánh 4 Hàm Lấy 1 Phần Tử:**
                | Tên hàm | Khi có 1 bản ghi khớp | Khi KHÔNG tìm thấy bản ghi nào (0) | Khi có NHIỀU HƠN 1 bản ghi khớp (>= 2) | Khi nào nên dùng? |
                | :--- | :--- | :--- | :--- | :--- |
                | **`First()`** | Trả về bản ghi đó. | ❌ **Nổ lỗi Exception!** | Trả về bản ghi đầu tiên. | Hầu như không dùng vì dễ nổ lỗi khi rỗng. |
                | **`FirstOrDefault()`**| Trả về bản ghi đó. | ✅ **Trả về `null` (hoặc `0`) an toàn.** | Trả về bản ghi đầu tiên. | ⭐ **Dùng 95% trong mọi trường hợp tìm kiếm!** |
                | **`Single()`** | Trả về bản ghi đó. | ❌ **Nổ lỗi Exception!** | ❌ **Nổ lỗi Exception!** | Rất hiếm dùng. |
                | **`SingleOrDefault()`**| Trả về bản ghi đó. | ✅ **Trả về `null` an toàn.** | ❌ **Nổ lỗi Exception!** | Chỉ dùng khi muốn đảm bảo dữ liệu không được phép có bản ghi thứ 2 trùng lặp. |
                • **🔍 3. Bộ đôi Kiểm tra Logic (`.Any()` vs `.All()`) và Bộ đôi Cắt trang (`.Skip()` & `.Take()`):**
                  - **`.Any(điều_kiện)` (Có ít nhất 1 cái thỏa mãn không?):** Giống toán tử `OR` — chỉ cần thấy **1 phần tử thỏa mãn** là dừng quét và trả về `true` ngay lập tức!
                  - **`.All(điều_kiện)` (Tất cả có cùng thỏa mãn không?):** Giống toán tử `AND` — **100% các phần tử** trong danh sách đều phải đúng điều kiện mới trả về `true`.
                  - **`.Skip(k).Take(n)`:** Bỏ qua `k` phần tử đầu tiên và nhặt lấy `n` phần tử tiếp theo (Công thức phân trang: `.Skip((page - 1) * size).Take(size)`).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tìm kiếm 1 phần tử an toàn với FirstOrDefault",
                        "📥 Đầu vào: Danh sách sản phẩm và mã cần tìm. ⚙️ Cơ chế ngầm: Duyệt từ đầu, gặp phần tử đầu tiên khớp là dừng ngay. Nếu duyệt hết không thấy thì trả về null. 📤 Đầu ra: Đối tượng Product hoặc null.",
                        "var item = products.FirstOrDefault(p => p.Code == \"SP01\");\nif (item == null) {\n    Console.WriteLine(\"Không tìm thấy sản phẩm!\");\n}"),
                    Step(2,
                        "2. Kiểm tra điều kiện tập hợp với Any và All",
                        "📥 Đầu vào: Giỏ hàng gồm nhiều món. ⚙️ Cơ chế ngầm: Any kiểm tra xem có món nào hết hàng không; All kiểm tra xem tất cả các món đã có giá hợp lệ chưa. 📤 Đầu ra: Giá trị bool.",
                        "bool hasOutOfStock = cartItems.Any(c => c.Stock == 0);\nbool allValidPrice = cartItems.All(c => c.Price > 0);"),
                    Step(3,
                        "3. Cắt trang danh sách bằng Skip và Take",
                        "📥 Đầu vào: Trang số 2 (page = 2), mỗi trang 5 món (size = 5). ⚙️ Cơ chế ngầm: Skip((2-1)*5) bỏ qua 5 món đầu, Take(5) lấy 5 món của trang 2. 📤 Đầu ra: 5 phần tử của trang thứ 2.",
                        "int page = 2, size = 5;\nvar page2Data = products\n    .OrderBy(p => p.Id)\n    .Skip((page - 1) * size)\n    .Take(size)\n    .ToList();")
                },
                "Trong danh sách users hiện đang có 2 người dùng cùng mang tên là \"An\". Nếu bạn gọi lệnh var u = users.SingleOrDefault(x => x.FirstName == \"An\");, chuyện gì sẽ xảy ra?",
                new List<string>
                {
                    "A. Trả về người dùng tên An đầu tiên tìm thấy",
                    "B. Trả về giá trị null",
                    "C. Chương trình văng lỗi InvalidOperationException (Sequence contains more than one matching element) vì SingleOrDefault chỉ chấp nhận tối đa 1 phần tử khớp điều kiện",
                    "D. Trả về danh sách gồm cả 2 người tên An"
                },
                "C",
                "Chữ OrDefault trong SingleOrDefault chỉ cứu bạn khi có 0 phần tử (trả về null). Nhưng chữ Single bắt buộc kết quả không được phép vượt quá 1 phần tử — nếu có từ 2 phần tử trở lên thỏa mãn, nó sẽ ném lỗi ngay lập tức."
            ),

            // =====================================================================
            // BÀI 3.10
            // =====================================================================
            Build(
                topicId,
                "3.10",
                "Nhóm hàm LINQ Thống kê & Gom nhóm (Báo cáo dữ liệu): Count, Sum, Min, Max, Average và GroupBy",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/standard-query-operators/grouping-data",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Khi làm trang Dashboard thống kê hoặc báo cáo doanh thu, bạn luôn cần tính: *Tổng tiền bán được là bao nhiêu (`Sum`)? Đơn hàng đắt nhất là bao nhiêu (`Max`)? Mỗi danh mục sản phẩm có bao nhiêu món hàng (`GroupBy`)?*
                  - LINQ cung cấp trọn bộ các hàm **Aggregation (Tổng hợp)** và **Grouping (Gom nhóm)** tương đương 1-1 với `COUNT, SUM, MIN, MAX, AVG, GROUP BY` trong SQL!
                • **🖼️ 2. Sơ đồ Hình dung Cách hoạt động của `.GroupBy()` (Chia giỏ phân loại):**
                ```
                Danh sách 5 Sản phẩm (Gồm Điện thoại và Laptop):
                         │
                         ▼  .GroupBy(p => p.CategoryName)
                ┌─────────────────────────────────────────────────────────────┐
                │ Giỏ 1 (g.Key = "Điện thoại"):                               │
                │   ├── iPhone 16 (25tr)  ──► g.Count() = 2 món               │
                │   └── Samsung S25 (22tr)──► g.Sum(x => x.Price) = 47tr      │
                ├─────────────────────────────────────────────────────────────┤
                │ Giỏ 2 (g.Key = "Laptop"):                                   │
                │   ├── ASUS ROG (30tr)                                       │
                │   ├── ThinkPad (25tr)   ──► g.Count() = 3 món               │
                │   └── MacBook (35tr)    ──► g.Max(x => x.Price) = 35tr      │
                └─────────────────────────────────────────────────────────────┘
                * Mỗi một "Giỏ" (g) có 1 chiếc nhãn tên là g.Key (chính là tiêu chí đem ra gom nhóm)
                  và bên trong giỏ chứa danh sách các phần tử thuộc nhóm đó!
                ```
                • **📊 3. Bảng Các Hàm Thống Kê Trong LINQ:**
                | Hàm thống kê | Cú pháp mẫu | Ý nghĩa kết quả |
                | :--- | :--- | :--- |
                | **`.Count()`** | `orders.Count(o => o.IsPaid)` | Đếm số lượng phần tử thỏa mãn điều kiện. |
                | **`.Sum()`** | `orders.Sum(o => o.TotalAmount)` | Cộng tổng giá trị của cột được chọn (nếu danh sách rỗng trả về `0`). |
                | **`.Max()` / `.Min()`**| `products.Max(p => p.Price)` | Tìm giá trị lớn nhất / nhỏ nhất. |
                | **`.Average()`** | `students.Average(s => s.Score)` | Tính trung bình cộng. |
                | **`.GroupBy()`** | `products.GroupBy(p => p.CategoryId)` | Chia danh sách thành các nhóm có chung `Key`, thường đi kèm `.Select()` ngay phía sau để tính tổng/đếm từng nhóm. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tính toán các chỉ số thống kê nhanh (Count, Sum, Average)",
                        "📥 Đầu vào: Danh sách đơn hàng trong tháng. ⚙️ Cơ chế ngầm: Duyệt và cộng dồn giá trị. 📤 Đầu ra: Số lượng đơn và tổng doanh thu.",
                        "int paidCount = orders.Count(o => o.Status == OrderStatus.Paid);\ndecimal totalRevenue = orders\n    .Where(o => o.Status == OrderStatus.Paid)\n    .Sum(o => o.TotalAmount);"),
                    Step(2,
                        "2. Gom nhóm dữ liệu theo Danh mục bằng GroupBy",
                        "📥 Đầu vào: Danh sách sản phẩm. ⚙️ Cơ chế ngầm: Những sản phẩm có cùng CategoryId sẽ được xếp chung vào 1 nhóm 'g' có nhãn là g.Key. 📤 Đầu ra: Các nhóm đã phân loại.",
                        "var grouped = products.GroupBy(p => p.CategoryId);"),
                    Step(3,
                        "3. Chiếu báo cáo từng nhóm bằng Select sau GroupBy",
                        "📥 Đầu vào: Các nhóm 'g' từ Bước 2. ⚙️ Cơ chế ngầm: Với mỗi nhóm 'g', lấy nhãn g.Key, đếm số món g.Count() và tính tổng tiền g.Sum(). 📤 Đầu ra: Bảng thống kê theo từng danh mục!",
                        "var report = grouped.Select(g => new {\n    CategoryId = g.Key,\n    ProductCount = g.Count(),\n    TotalValue = g.Sum(p => p.Price),\n    MaxPrice = g.Max(p => p.Price)\n}).ToList();")
                },
                "Khi sử dụng câu lệnh products.GroupBy(p => p.BrandName).Select(g => ...) để thống kê sản phẩm theo từng hãng sản xuất, bên trong hàm .Select(g => ...), thuộc tính g.Key chứa giá trị gì?",
                new List<string>
                {
                    "A. Chứa giá trị của tiêu chí dùng để gom nhóm (ở đây chính là chuỗi BrandName của từng nhóm hãng sản xuất)",
                    "B. Luôn luôn chứa con số 0",
                    "C. Chứa tổng số lượng sản phẩm của cả bảng",
                    "D. Chứa khóa chính Id của sản phẩm đầu tiên"
                },
                "A",
                "Trong GroupBy, mỗi nhóm g là một đối tượng IGrouping<TKey, TElement>, trong đó g.Key lưu trữ chính giá trị của thuộc tính được chọn làm tiêu chí gom nhóm (BrandName), còn bản thân g là tập hợp các phần tử thuộc nhóm đó."
            ),

            // =====================================================================
            // BÀI 3.11
            // =====================================================================
            Build(
                topicId,
                "3.11",
                "Nhóm hàm LINQ Kết nối tập hợp & Làm phẳng dữ liệu: Join, SelectMany và Tập hợp (Intersect, Except, Union)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/standard-query-operators/set-operations",
                """
                • **💡 1. Hiểu bản chất của `.SelectMany()` (Làm phẳng danh sách lồng nhau) trong 30 giây:**
                  - Giả sử bạn có danh sách `3 Lớp học`, bên trong mỗi Lớp học lại chứa một `List<Student> Students` (mỗi lớp 10 bạn).
                  - Nếu bạn dùng `.Select(c => c.Students)` bình thường: Bạn sẽ nhận về một **"Hộp lớn chứa 3 hộp nhỏ"** (`List<List<Student>>`) — rất khó duyệt!
                  - Nếu bạn dùng **`.SelectMany(c => c.Students)`**: Nó sẽ **đổ toàn bộ sinh viên từ cả 3 hộp nhỏ ra gộp chung thành 1 danh sách phẳng duy nhất** (`List<Student>` gồm 30 bạn)!
                • **🖼️ 2. Sơ đồ Trực quan `.Select()` vs `.SelectMany()`:**
                ```
                3 Đơn hàng (Mỗi đơn có danh sách các món hàng OrderItems):
                ├── Dùng .Select(o => o.OrderItems)     ──► [ [Món 1, Món 2], [Món 3], [Món 4, Món 5] ] (Lồng 2 tầng)
                └── Dùng .SelectMany(o => o.OrderItems) ──► [ Món 1, Món 2, Món 3, Món 4, Món 5 ]       (Làm phẳng 1 tầng!)
                ```
                • **📊 3. Bảng Các Hàm Kết Nối (`Join`) và Phép Toán Tập Hợp Trong LINQ:**
                | Tên hàm | Công dụng thực tế | Tương đương trong SQL / Toán học |
                | :--- | :--- | :--- |
                | **`.SelectMany()`** | Làm phẳng danh sách con bên trong danh sách cha thành 1 danh sách duy nhất. | `CROSS APPLY` / Unnest |
                | **`.Join()`** | Kết nối 2 danh sách rời rạc dựa trên cột khóa chung (`OuterKey == InnerKey`). | `INNER JOIN` trong SQL |
                | **`.Union(list2)`** | Gộp 2 danh sách lại và tự động loại bỏ các phần tử trùng nhau. | Phép Hợp (A U B) |
                | **`.Intersect(list2)`**| Tìm các phần tử **cùng xuất hiện ở cả 2 danh sách**. | Phép Giao (A ∩ B) |
                | **`.Except(list2)`** | Tìm các phần tử **có ở danh sách 1 nhưng KHÔNG CÓ ở danh sách 2**. | Phép Hiệu (A \ B) |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Làm phẳng danh sách con bằng SelectMany",
                        "📥 Đầu vào: Danh sách các Phòng ban (Departments), mỗi phòng có danh sách Nhân viên (Employees). ⚙️ Cơ chế ngầm: Gom tất cả nhân viên của mọi phòng ban về chung 1 danh sách phẳng. 📤 Đầu ra: List<Employee> toàn công ty.",
                        "List<Employee> allEmployees = departments\n    .SelectMany(dept => dept.Employees)\n    .ToList();"),
                    Step(2,
                        "2. Tìm điểm chung và điểm khác biệt bằng Intersect & Except",
                        "📥 Đầu vào: Danh sách môn học kỳ 1 và kỳ 2. ⚙️ Cơ chế ngầm: So sánh phần tử giữa 2 tập hợp. 📤 Đầu ra: Các môn học trùng hoặc các môn mới hoàn toàn.",
                        "var listA = new List<int> { 1, 2, 3, 4 };\nvar listB = new List<int> { 3, 4, 5, 6 };\n\nvar chung = listA.Intersect(listB).ToList(); // Kết quả: 3, 4\nvar chiCoOA = listA.Except(listB).ToList();  // Kết quả: 1, 2"),
                    Step(3,
                        "3. Kết nối 2 danh sách rời rạc bằng hàm Join",
                        "📥 Đầu vào: Danh sách Products và danh sách Categories. ⚙️ Cơ chế ngầm: Khớp p.CategoryId == c.Id để ghép tên danh mục vào sản phẩm. 📤 Đầu ra: Danh sách đã ghép nối.",
                        "var joined = products.Join(\n    categories,\n    p => p.CategoryId, // Khóa ngoại ở bảng Products\n    c => c.Id,         // Khóa chính ở bảng Categories\n    (p, c) => new { ProductName = p.Name, CategoryName = c.Name }\n).ToList();")
                },
                "Mỗi đối tượng Order (Đơn hàng) có chứa một danh sách List<OrderDetail> Details (Chi tiết các món hàng trong đơn). Nếu bạn có một danh sách List<Order> allOrders và muốn lấy ra tất cả các OrderDetail của toàn bộ các đơn hàng gộp chung thành một danh sách phẳng duy nhất (List<OrderDetail>), bạn phải dùng hàm LINQ nào?",
                new List<string>
                {
                    "A. allOrders.Select(o => o.Details).ToList();",
                    "B. allOrders.SelectMany(o => o.Details).ToList();",
                    "C. allOrders.Where(o => o.Details).ToList();",
                    "D. allOrders.GroupBy(o => o.Details).ToList();"
                },
                "B",
                "Nếu dùng .Select(o => o.Details) kết quả trả về sẽ là danh sách lồng nhau List<List<OrderDetail>>. Phải dùng .SelectMany(o => o.Details) để trải phẳng (flatten) các danh sách con thành một List<OrderDetail> duy nhất."
            ),

            // =====================================================================
            // BÀI 3.12
            // =====================================================================
            Build(
                topicId,
                "3.12",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 3 (Làm chủ Collection, Generic & LINQ)",
                "C# toàn diện",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/linq/write-linq-queries",
                """
                • **💡 1. Bản đồ chọn đúng Cấu trúc Dữ liệu (Collection) trong 5 giây:**
                  - Cần danh sách động thêm/xóa/duyệt thông thường -> Dùng **`List<T>`**.
                  - Cần tra cứu siêu tốc theo Mã/ID duy nhất -> Dùng **`Dictionary<TKey, TValue>`** (kèm `TryGetValue`).
                  - Cần lưu danh sách đảm bảo không có phần tử nào trùng nhau -> Dùng **`HashSet<T>`**.
                  - Cần xử lý theo thứ tự xếp hàng (Vào trước ra trước) -> Dùng **`Queue<T>`**.
                  - Cần xử lý đảo ngược / Hoàn tác Undo (Vào sau ra trước) -> Dùng **`Stack<T>`**.
                • **📊 2. Quy trình Chuẩn 5 Bước Khi Viết Một Câu Truy Vấn LINQ Thực Tế:**
                ```
                Nguồn dữ liệu (List / DbSet)
                  │
                  ├─► Bước 1: .Where(...)                [Lọc bỏ dữ liệu rác / không thỏa mãn TRƯỚC TIÊN cho nhẹ]
                  ├─► Bước 2: .OrderBy(...) / .ThenBy(...) [Sắp xếp đúng thứ tự mong muốn]
                  ├─► Bước 3: .Skip(...) .Take(...)      [Cắt trang nếu làm phân trang]
                  ├─► Bước 4: .Select(...)               [Biến đổi / Chiếu sang DTO gọn nhẹ]
                  └─► Bước 5: .ToList() / .FirstOrDefault() [Chốt hạ thực thi!]
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Lọc điều kiện phức tạp bằng Where",
                        "📥 Đầu vào: Danh sách sản phẩm toàn hệ thống. ⚙️ Cơ chế ngầm: Giữ lại các sản phẩm đang mở bán và còn hàng tồn kho. 📤 Đầu ra: IEnumerable đã lọc.",
                        "var query = products\n    .Where(p => p.IsActive && p.Stock > 0 && p.Price >= 100000m);"),
                    Step(2,
                        "2. Sắp xếp đa tiêu chí bằng OrderByDescending & ThenBy",
                        "📥 Đầu vào: Kết quả lọc từ Bước 1. ⚙️ Cơ chế ngầm: Ưu tiên sản phẩm bán chạy nhất lên đầu, nếu bằng nhau thì xếp theo giá rẻ hơn đứng trước. 📤 Đầu ra: IOrderedEnumerable.",
                        "var sorted = query\n    .OrderByDescending(p => p.SoldCount)\n    .ThenBy(p => p.Price);"),
                    Step(3,
                        "3. Phân trang & Chiếu sang Record DTO bằng Select",
                        "📥 Đầu vào: Danh sách đã sắp xếp. ⚙️ Cơ chế ngầm: Lấy 10 sản phẩm đầu tiên và gói vào DTO. 📤 Đầu ra: Danh sách DTO.",
                        "var top10Dtos = sorted\n    .Take(10)\n    .Select(p => new ProductCardDto(p.Id, p.Name, p.Price))\n    .ToList();"),
                    Step(4,
                        "4. Đưa vào Dictionary để tra cứu nhanh theo Id",
                        "📥 Đầu vào: Danh sách top10Dtos. ⚙️ Cơ chế ngầm: Dùng hàm .ToDictionary() của LINQ để biến List thành Dictionary với Key là Id. 📤 Đầu ra: Dictionary<int, ProductCardDto>.",
                        "Dictionary<int, ProductCardDto> fastLookup = top10Dtos\n    .ToDictionary(dto => dto.Id, dto => dto);")
                },
                "(Câu hỏi tổng hợp Chủ đề 3) Bạn muốn chuyển đổi nhanh một List<Product> products (trong đó mỗi sản phẩm có thuộc tính Id là duy nhất) thành một từ điển Dictionary<int, string> với Key là Id của sản phẩm và Value là Name của sản phẩm. Hàm LINQ nào dưới đây giúp bạn làm việc đó chỉ trong 1 dòng lệnh?",
                new List<string>
                {
                    "A. products.ToDictionary(p => p.Id, p => p.Name);",
                    "B. products.ToList(p => p.Id);",
                    "C. products.GroupBy(p => p.Id);",
                    "D. products.Where(p => p.Id == p.Name);"
                },
                "A",
                "Hàm mở rộng .ToDictionary(keySelector, elementSelector) của LINQ cho phép chuyển đổi trực tiếp bất kỳ tập hợp nào thành một Dictionary<TKey, TValue> bằng cách chỉ định biểu thức Lambda cho Key (p => p.Id) và Value (p => p.Name)."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 12 bài học con (3.1 -> 3.12) cho Chủ đề 3 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic3 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 3);
        if (topic3 == null)
            return;

        topic3.Title = "Chủ đề 3: Làm việc với Collection và LINQ";
        topic3.Description = "12 Bài học con siêu chi tiết từ Non-Generic vs Generic (Boxing/Unboxing), List<T> (Count vs Capacity), Dictionary<TKey,TValue> O(1), HashSet/Queue/Stack, Delegate/Func/Action, Lambda Expression đến trọn bộ LINQ thực chiến.";

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
