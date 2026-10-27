using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic6Seed
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
            // BÀI 6.1
            // =====================================================================
            Build(
                topicId,
                "6.1",
                "Nền tảng HTML5 Semantic & Cấu trúc cây DOM (Bộ khung xương của mọi trang Web)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/Client-side/",
                """
                • **💡 1. Hiểu bản chất của HTML, CSS và JavaScript trong 30 giây:**
                  - Hãy tưởng tượng một trang web giống như một **Ngôi nhà**:
                    1. **HTML (HyperText Markup Language):** Là **Bộ khung xương bê tông** (quy định chỗ nào là cửa chính, chỗ nào là phòng khách, chỗ nào đặt cái bàn, bức tranh).
                    2. **CSS (Cascading Style Sheets):** Là **Lớp sơn tường và Nội thất trang trí** (quy định tường màu xanh hay trắng, chữ to hay nhỏ, bố cục xếp ngang hay dọc).
                    3. **JavaScript / Blazor:** Là **Hệ thống điện nước thông minh** (bấm công tắc thì đèn sáng, bấm nút thì mở hộp thoại mà không cần xây lại nhà).
                • **🖼️ 2. Sơ đồ Cấu trúc HTML5 Ngữ nghĩa (`Semantic HTML5`) Chuẩn Hiện Đại:**
                ```
                ┌─────────────────────────────────────────────────────────────┐
                │ <header> : Thanh đầu trang (Chứa Logo và <nav> Menu chính)  │
                ├───────────────┬─────────────────────────────────────────────┤
                │ <aside>       │ <main> : Khu vực nội dung chính của trang   │
                │ (Sidebar      │   ├── <section> : Từng phân khu nội dung    │
                │  cột trái)    │   └── <article> : Thẻ bài viết / Sản phẩm   │
                ├───────────────┴─────────────────────────────────────────────┤
                │ <footer> : Chân trang (Thông tin bản quyền, Liên hệ)        │
                └─────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng Các Thẻ HTML Quan Trọng Nhất Khi Làm Web ASP.NET Core:**
                | Nhóm chức năng | Các thẻ HTML cốt lõi | Thuộc tính quan trọng bắt buộc nhớ |
                | :--- | :--- | :--- |
                | **Hiển thị Văn bản & Ảnh**| `<h1>` đến `<h6>`, `<p>`, `<span>`, `<img>`, `<a>` | `<a href="đường_dẫn">`, `<img src="ảnh.jpg" alt="mô_tả" />` |
                | **Biểu mẫu nhập liệu (Form)**| **`<form>`**, **`<input>`**, **`<select>`**, **`<textarea>`**, **`<button>`** | ⚠️ Thuộc tính **`name="..."`** trên thẻ `<input>` phải **trùng khớp 100% với tên Property trong C#** thì Backend mới nhận được dữ liệu! |
                | **Bảng dữ liệu (Table)** | `<table>`, `<thead>`, `<tbody>`, `<tr>` (Hàng), `<th>` (Tiêu đề cột), `<td>` (Ô dữ liệu) | Dùng để hiển thị danh sách Sinh viên, Sách, Đơn hàng trong trang Quản trị. |
                • **⚠️ 4. Bẫy Cực Kỳ Phổ Biến Khi Gửi Form Lên Controller C#:**
                  - Rất nhiều bạn đặt `<input id="BookTitle" />` nhưng lại **quên viết thuộc tính `name="BookTitle"`** (hoặc không dùng `asp-for="BookTitle"`). Trình duyệt chỉ gửi dữ liệu dựa theo thuộc tính **`name`**, nếu thiếu `name` thì biến nhận được ở Controller C# sẽ luôn luôn bị **`null`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Dựng khung trang bằng thẻ Semantic HTML5",
                        "📥 Đầu vào: Yêu cầu thiết kế khung trang Quản lý Thư viện. ⚙️ Cơ chế ngầm: Sử dụng các thẻ có ngữ nghĩa (<header>, <main>, <footer>) giúp trình duyệt và công cụ tìm kiếm hiểu rõ cấu trúc trang thay vì lạm dụng toàn bộ bằng thẻ <div>. 📤 Đầu ra: Khung trang rõ ràng, chuẩn SEO.",
                        "<header>\n    <nav>Menu Quản Lý Thư Viện</nav>\n</header>\n<main>\n    <section id=\"book-list\">Danh sách sách</section>\n</main>\n<footer>Bản quyền 2026 - Hoàng Việt</footer>"),
                    Step(2,
                        "2. Tạo Bảng hiển thị danh sách dữ liệu (<table>)",
                        "📥 Đầu vào: Danh sách các cuốn sách. ⚙️ Cơ chế ngầm: Tách biệt phần đầu bảng (<thead> chứa <th>) và phần thân bảng (<tbody> chứa các dòng <tr> và ô <td>). 📤 Đầu ra: Bảng dữ liệu chuẩn bị cho vòng lặp @foreach của Razor.",
                        "<table class=\"table\">\n    <thead>\n        <tr><th>Mã Sách</th><th>Tên Sách</th></tr>\n    </thead>\n    <tbody>\n        <tr><td>S01</td><td>Lập trình C# .NET 10</td></tr>\n    </tbody>\n</table>"),
                    Step(3,
                        "3. Tạo Form gửi dữ liệu về Backend (<form method=\"post\">)",
                        "📥 Đầu vào: Người dùng nhập Tên sách và Giá tiền. ⚙️ Cơ chế ngầm: Khi bấm nút button type=\"submit\", trình duyệt gom giá trị của các ô input dựa theo thuộc tính 'name' và bắn HTTP POST về đường dẫn 'action'. 📤 Đầu ra: Controller C# nhận đủ dữ liệu.",
                        "<form action=\"/Books/Create\" method=\"post\">\n    <input type=\"text\" name=\"Title\" placeholder=\"Nhập tên sách...\" required />\n    <input type=\"number\" name=\"Price\" />\n    <button type=\"submit\">Lưu Sách</button>\n</form>")
                },
                "Khi tạo một biểu mẫu <form method=\"post\"> bằng HTML thuần để gửi dữ liệu người dùng nhập lên một hàm Action trong ASP.NET Core MVC Controller, thuộc tính nào trên thẻ <input> bắt buộc phải trùng tên với tên tham số/thuộc tính trong C# để Model Binding hoạt động được?",
                new List<string>
                {
                    "A. Thuộc tính name (ví dụ: <input name=\"BookName\" />)",
                    "B. Thuộc tính placeholder",
                    "C. Thuộc tính class",
                    "D. Thuộc tính style"
                },
                "A",
                "Giao thức HTTP Form Submit chỉ đóng gói dữ liệu theo cặp name=value. Nếu thẻ <input> chỉ có id mà không có name (hoặc không dùng Tag Helper asp-for để tự sinh name), dữ liệu của ô đó sẽ không được gửi lên Server."
            ),

            // =====================================================================
            // BÀI 6.2
            // =====================================================================
            Build(
                topicId,
                "6.2",
                "Làm chủ CSS3 hiện đại: Box Model, Flexbox và CSS Grid (Bí quyết căn chỉnh bố cục không bao giờ lệch)",
                "Web API",
                "https://developer.mozilla.org/vi/docs/Learn/CSS/CSS_layout/Flexbox",
                """
                • **💡 1. Hiểu bản chất của `Box Model` (Mô hình cái hộp) trong 30 giây:**
                  - Trong CSS, **mọi thẻ HTML trên màn hình đều là một chiếc hộp chữ nhật** gồm 4 lớp từ trong ra ngoài (giống như một bức tranh đóng khung treo tường):
                    1. **`Content` (Ruột):** Chính là dòng chữ hoặc bức ảnh ở bên trong cùng.
                    2. **`Padding` (Lớp xốp đệm bên trong):** Khoảng trống **từ chữ ra tới đường viền** của hộp (giúp chữ không bị dính sát vào mép hộp).
                    3. **`Border` (Khung viền):** Đường viền bao quanh hộp.
                    4. **`Margin` (Khoảng cách bên ngoài):** Khoảng trống **đẩy chiếc hộp này ra xa các chiếc hộp hàng xóm** xung quanh!
                • **🖼️ 2. Sơ đồ Trực quan `CSS Box Model` & `Flexbox`:**
                ```
                [1. CSS Box Model (4 Lớp của một phần tử)]:
                ┌─────────────────── MARGIN (Khoảng cách với hộp bên cạnh) ───────────────────┐
                │  ┌──────────────── BORDER (Đường viền hộp: 1px solid) ───────────────────┐  │
                │  │  ┌───────────── PADDING (Khoảng thoáng bên trong hộp) ─────────────┐  │  │
                │  │  │                 CONTENT (Nội dung Chữ / Ảnh)                    │  │  │
                │  │  └─────────────────────────────────────────────────────────────────┘  │  │
                │  └───────────────────────────────────────────────────────────────────────┘  │
                └─────────────────────────────────────────────────────────────────────────────┘

                [2. Flexbox (display: flex) -> Dàn hàng ngang các phần tử chỉ với 1 dòng lệnh!]:
                Thẻ cha (display: flex; justify-content: space-between; align-items: center;)
                ┌─────────────────────────────────────────────────────────────────────────────┐
                │  [Logo Trái]                [Menu Giữa]                     [Tài khoản Phải]│
                └─────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng So sánh Hai Vũ Khí Dàn Trang: `Flexbox` vs `CSS Grid`:**
                | Công nghệ | Chiều điều khiển | Các thuộc tính quyền lực nhất | Khi nào sử dụng? |
                | :--- | :--- | :--- | :--- |
                | **`Flexbox` (`display: flex`)** | **1 Chiều** (Hoặc dàn theo Hàng ngang `row`, hoặc xếp theo Cột dọc `column`). | `justify-content: space-between` (Căn ngang), `align-items: center` (Căn giữa dọc), `gap: 16px` (Khoảng cách giữa các ô). | Thanh Header/Navbar, căn giữa nút bấm, thanh công cụ tìm kiếm. |
                | **`CSS Grid` (`display: grid`)**| **2 Chiều** (Chia lưới cả Hàng ngang VÀ Cột dọc cùng lúc). | `grid-template-columns: repeat(3, 1fr)`, `gap: 20px`. | Chia danh sách thẻ Sản phẩm/Sách thành lưới 3 cột hoặc 4 cột đều tăm tắp! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết lập Box-Sizing và Khoảng thoáng (Padding & Margin)",
                        "📥 Đầu vào: Một thẻ Card hiển thị thông tin sách. ⚙️ Cơ chế ngầm: Đặt padding: 20px để chữ cách viền trong 20px; đặt margin-bottom: 16px để các thẻ cách nhau 16px; đặt border-radius: 12px để bo tròn góc hiện đại. 📤 Đầu ra: Thẻ thoáng mắt, không bị dính chữ.",
                        ".book-card {\n    box-sizing: border-box;\n    padding: 20px;       /* Đệm trong */\n    margin-bottom: 16px; /* Cách ngoài */\n    border: 1px solid #334155;\n    border-radius: 12px;\n}"),
                    Step(2,
                        "2. Dùng Flexbox căn chỉnh Thanh Header trong 3 dòng",
                        "📥 Đầu vào: Thẻ <header> chứa Logo và cụm Menu. ⚙️ Cơ chế ngầm: display: flex xếp chúng lên cùng 1 hàng ngang; justify-content: space-between đẩy Logo sang sát trái, Menu sang sát phải; align-items: center căn giữa theo chiều dọc. 📤 Đầu ra: Thanh Header chuẩn chỉnh.",
                        ".app-header {\n    display: flex;\n    justify-content: space-between;\n    align-items: center;\n    padding: 12px 24px;\n}"),
                    Step(3,
                        "3. Dùng CSS Grid chia lưới 3 cột hiển thị danh sách Sách",
                        "📥 Đầu vào: Khung chứa 12 cuốn sách. ⚙️ Cơ chế ngầm: repeat(3, 1fr) tự động chia màn hình làm 3 cột bằng nhau (1fr = 1 phần bằng nhau), gap: 20px tạo rãnh ngăn cách 20px giữa các thẻ. 📤 Đầu ra: Lưới 3 cột tự động xuống dòng đẹp mắt.",
                        ".book-grid {\n    display: grid;\n    grid-template-columns: repeat(3, 1fr);\n    gap: 20px;\n}")
                },
                "Trong CSS, khi bạn muốn tạo khoảng trống ở bên trong đường viền của một chiếc hộp (giúp nội dung chữ không bị dính sát vào mép viền của chính hộp đó), bạn phải sử dụng thuộc tính nào?",
                new List<string>
                {
                    "A. Sử dụng thuộc tính padding",
                    "B. Sử dụng thuộc tính margin",
                    "C. Sử dụng thuộc tính z-index",
                    "D. Sử dụng thuộc tính float"
                },
                "A",
                "padding là khoảng đệm bên trong đường viền (border) của phần tử, trong khi margin là khoảng cách bên ngoài đường viền dùng để đẩy các phần tử khác ra xa."
            ),

            // =====================================================================
            // BÀI 6.3
            // =====================================================================
            Build(
                topicId,
                "6.3",
                "Hệ thống lưới 12 cột của Bootstrap 5 (Grid System: Container, Row, Col) & Thiết kế Responsive đa màn hình",
                "Web API",
                "https://getbootstrap.com/docs/5.3/layout/grid/",
                """
                • **💡 1. Tại sao mọi dự án ASP.NET Core MVC mặc định đều tích hợp sẵn `Bootstrap 5`?**
                  - Khi tạo mới project MVC trên Visual Studio, Microsoft cài sẵn thư viện **Bootstrap 5** trong `wwwroot/lib/bootstrap`. Nhờ Bootstrap, bạn **không cần tự viết file CSS dài dòng** mà chỉ cần gõ tên các `class` dựng sẵn (như `btn btn-primary`, `row`, `col-md-4`) ngay trên thẻ HTML là có ngay giao diện đẹp và tự động co giãn vừa khít cả màn hình Laptop lẫn Điện thoại (**Responsive**)!
                • **🖼️ 2. Sơ đồ Quy tắc "Bàn Cờ 12 Cột" Của Bootstrap Grid System (`container` ➔ `row` ➔ `col`):**
                ```
                Mọi dòng (<div class="row">) trong Bootstrap luôn được chia làm ĐÚNG 12 PHẦN BẰNG NHAU:
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 1  │ 2  │ 3  │ 4  │ 5  │ 6  │ 7  │ 8  │ 9  │ 10 │ 11 │ 12 │ (Tổng = 12 cột)│
                ├───────────────────┴──────────────┼─────────────────┴───────────────────────┤
                │      .col-md-4 (Chiếm 4 phần)    │        .col-md-8 (Chiếm 8 phần)         │
                │      (Dùng làm Sidebar trái)     │        (Dùng làm Nội dung chính)        │
                ├────────────────────────┬─────────┴──────────────┬──────────────────────────┤
                │ .col-md-4 (1/3 trang)  │ .col-md-4 (1/3 trang)  │ .col-md-4 (1/3 trang)    │
                └────────────────────────┴────────────────────────┴──────────────────────────┘
                *(Nếu muốn chia 3 cột bằng nhau: 12 / 3 = 4 -> Dùng 3 thẻ class="col-md-4")*
                *(Nếu muốn chia 4 cột bằng nhau: 12 / 4 = 3 -> Dùng 4 thẻ class="col-md-3")*
                ```
                • **📊 3. Bảng Các Mốc Kích Thước Màn Hình (`Breakpoints`) Trong Bootstrap 5:**
                | Ký hiệu trong class | Kích thước màn hình đại diện | Ý nghĩa thực tế khi ghép class (Ví dụ: `class="col-12 col-md-6 col-lg-3"`) |
                | :--- | :--- | :--- |
                | `col-12` (Không chữ) | Điện thoại nhỏ (`< 576px`) | Trên điện thoại: Mỗi thẻ chiếm trọn `12/12` phần (xếp dọc **1 cột**). |
                | `col-md-*` (**Medium**) | Máy tính bảng / Laptop nhỏ (`≥ 768px`)| Trên Tablet: Mỗi thẻ chiếm `6/12` phần (chia làm **2 cột**). |
                | `col-lg-*` (**Large**) | Màn hình PC / Laptop chuẩn (`≥ 992px`)| Trên Laptop/PC: Mỗi thẻ chiếm `3/12` phần (chia làm **4 cột**). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Bọc ngoài bằng .container (hoặc .container-fluid)",
                        "📥 Đầu vào: Nội dung trang web. ⚙️ Cơ chế ngầm: Class .container tự động căn giữa trang web và chừa khoảng trống 2 bên lề; còn .container-fluid trải rộng 100% toàn màn hình. 📤 Đầu ra: Khung chứa chuẩn mực.",
                        "<div class=\"container mt-4\">\n    <!-- Các dòng .row sẽ đặt ở trong này -->\n</div>"),
                    Step(2,
                        "2. Tạo hàng ngang bằng .row và chia tỷ lệ Sidebar 3 - Main 9",
                        "📥 Đầu vào: Bố cục 2 cột (Menu trái nhỏ, Nội dung phải lớn). ⚙️ Cơ chế ngầm: Tổng 3 + 9 = 12. Trên màn hình từ mức trung bình (md) trở lên sẽ chia tỷ lệ 3:9, còn trên điện thoại tự động xếp chồng lên nhau. 📤 Đầu ra: Layout đa thiết bị.",
                        "<div class=\"row\">\n    <div class=\"col-12 col-md-3\">Menu Danh Mục</div>\n    <div class=\"col-12 col-md-9\">Danh Sách Sản Phẩm</div>\n</div>"),
                    Step(3,
                        "3. Chia lưới 4 sản phẩm trên 1 hàng ngang",
                        "📥 Đầu vào: Danh sách các thẻ sản phẩm. ⚙️ Cơ chế ngầm: Lấy 12 chia 4 bằng 3 -> Mỗi sản phẩm bọc trong col-lg-3 col-md-6 col-12. Cứ đủ 12 phần là Bootstrap tự động ngắt xuống hàng mới! 📤 Đầu ra: Lưới sản phẩm Responsive hoàn hảo.",
                        "<div class=\"row g-3\">\n    <div class=\"col-12 col-md-6 col-lg-3\">Sản phẩm 1</div>\n    <div class=\"col-12 col-md-6 col-lg-3\">Sản phẩm 2</div>\n    <div class=\"col-12 col-md-6 col-lg-3\">Sản phẩm 3</div>\n    <div class=\"col-12 col-md-6 col-lg-3\">Sản phẩm 4</div>\n</div>")
                },
                "Trong hệ thống lưới 12 cột của Bootstrap 5, nếu bạn muốn hiển thị 3 thẻ bài viết nằm ngang bằng nhau trên một hàng đối với màn hình máy tính (md trở lên), bạn phải đặt class cho mỗi thẻ cột là gì?",
                new List<string>
                {
                    "A. class=\"col-md-3\"",
                    "B. class=\"col-md-4\" (Vì tổng 12 cột chia cho 3 phần bằng nhau thì mỗi phần chiếm 4 cột: 4 + 4 + 4 = 12)",
                    "C. class=\"col-md-6\"",
                    "D. class=\"col-md-12\""
                },
                "B",
                "Quy tắc của Bootstrap Grid là tổng độ rộng của một hàng (row) bằng 12 đơn vị. Muốn chia thành N cột đều nhau, bạn lấy 12 / N: chia 2 cột dùng col-md-6, chia 3 cột dùng col-md-4, chia 4 cột dùng col-md-3."
            ),

            // =====================================================================
            // BÀI 6.4
            // =====================================================================
            Build(
                topicId,
                "6.4",
                "Bộ Component & Utility Classes thực chiến của Bootstrap 5 (Buttons, Cards, Tables, Modals, Spacing)",
                "Web API",
                "https://getbootstrap.com/docs/5.3/components/card/",
                """
                • **💡 1. Hiểu bản chất các Class Tiện Ích (`Utility Classes`) của Bootstrap trong 30 giây:**
                  - Thay vì phải mở file `.css` ra viết `margin-top: 16px; display: flex; color: green;`, Bootstrap 5 chế tạo sẵn các **tên class viết tắt cực kỳ quy củ** để bạn gõ trực tiếp vào thẻ HTML trong 2 giây!
                • **🖼️ 2. Công thức Đọc & Viết Class Khoảng Cách (`Margin` & `Padding`) Của Bootstrap:**
                ```
                Cú pháp:  {Thuộc_tính}{Hướng}-{Kích_cỡ_từ_0_đến_5}
                          ├── Thuộc tính: 'm' = Margin (Cách ngoài) | 'p' = Padding (Đệm trong)
                          ├── Hướng:      't' = Top (Trên) | 'b' = Bottom (Dưới) | 's' = Start (Trái) | 'e' = End (Phải)
                          │               'x' = Cả Trái & Phải | 'y' = Cả Trên & Dưới | (Bỏ trống = Cả 4 phía)
                          └── Kích cỡ:    0 (0px) | 1 (4px) | 2 (8px) | 3 (16px) | 4 (24px) | 5 (48px)

                Ví dụ dịch nhanh:
                - "mt-3"  ──► Margin Top 16px (Cách lề trên 16px)
                - "p-4"   ──► Padding 4 phía 24px (Đệm trong 24px)
                - "py-2"  ──► Padding trục dọc (Trên + Dưới) 8px
                ```
                • **📊 3. Bảng 6 Nhóm Component Bootstrap Dùng Nhiều Nhất Trong Đồ Án MVC:**
                | Thành phần | Các Class chuẩn cần nhớ | Ứng dụng thực tế |
                | :--- | :--- | :--- |
                | **Bảng màu ý nghĩa** | `primary` (Xanh dương), `success` (Xanh lá), `danger` (Đỏ), `warning` (Vàng), `info` (Xanh ngọc), `dark` (Đen). | Ghép sau `btn-`, `bg-`, `text-`, `badge-`, `alert-`. |
                | **Nút bấm (`Buttons`)** | `btn btn-primary`, `btn btn-outline-danger`, `btn-sm` (nhỏ), `btn-lg` (lớn). | Nút *Thêm mới* (`btn-success`), nút *Xóa* (`btn-danger`). |
                | **Bảng dữ liệu (`Tables`)**| `table table-hover table-striped table-bordered align-middle` | Làm bảng danh sách trong trang Admin đẹp ngay lập tức. |
                | **Biểu mẫu (`Forms`)** | `form-label`, **`form-control`** (ô nhập chữ), **`form-select`** (menu thả xuống). | Làm đẹp mọi ô `<input>` và `<select>` trong form. |
                | **Khung thẻ (`Cards`)** | `card shadow-sm` ➔ `card-header` ➔ `card-body` ➔ `card-footer`. | Đóng khung từng bài học, từng sản phẩm, từng khối thống kê. |
                | **Hộp thoại nổi (`Modal`)**| `modal fade` kết hợp thuộc tính `data-bs-toggle="modal" data-bs-target="#idModal"`. | Bấm nút *"Xóa"* thì hiện hộp thoại hỏi xác nhận mà không chuyển trang! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Trang trí Bảng danh sách kèm Nút thao tác chuẩn Admin",
                        "📥 Đầu vào: Bảng HTML thô kệch. ⚙️ Cơ chế ngầm: Thêm class table-hover (di chuột sáng dòng), table-striped (dòng chẵn lẻ khác màu), badge (nhãn trạng thái) và btn-sm (nút nhỏ gọn). 📤 Đầu ra: Giao diện quản trị chuyên nghiệp.",
                        "<table class=\"table table-hover table-bordered align-middle\">\n    <thead class=\"table-dark\">\n        <tr><th>Tên sách</th><th>Trạng thái</th><th>Thao tác</th></tr>\n    </thead>\n    <tbody>\n        <tr>\n            <td>Lập trình C#</td>\n            <td><span class=\"badge bg-success\">Còn sách</span></td>\n            <td>\n                <a class=\"btn btn-sm btn-warning\">Sửa</a>\n                <a class=\"btn btn-sm btn-danger\">Xóa</a>\n            </td>\n        </tr>\n    </tbody>\n</table>"),
                    Step(2,
                        "2. Thiết kế Form nhập liệu đẹp mắt với form-control",
                        "📥 Đầu vào: Các thẻ label và input. ⚙️ Cơ chế ngầm: Class mb-3 tạo khoảng cách 16px giữa các dòng nhập; class form-control bo góc và tạo hiệu ứng viền sáng xanh khi click chuột vào ô input. 📤 Đầu ra: Form nhập liệu chuẩn chỉnh.",
                        "<div class=\"card shadow-sm p-4\">\n    <div class=\"mb-3\">\n        <label class=\"form-label fw-bold\">Tên độc giả</label>\n        <input type=\"text\" class=\"form-control\" placeholder=\"Nhập họ tên...\" />\n    </div>\n    <button type=\"submit\" class=\"btn btn-primary w-100\">Đăng ký thẻ</button>\n</div>"),
                    Step(3,
                        "3. Căn chỉnh nhanh bằng Flexbox Utility của Bootstrap",
                        "📥 Đầu vào: Tiêu đề trang ở bên trái và nút 'Thêm mới' ở bên phải. ⚙️ Cơ chế ngầm: Dùng class d-flex justify-content-between align-items-center mb-4 ngay trên thẻ div mà không cần viết 1 dòng CSS riêng nào! 📤 Đầu ra: Bố cục đầu trang cân đối.",
                        "<div class=\"d-flex justify-content-between align-items-center mb-4\">\n    <h2 class=\"text-primary m-0\">Quản Lý Sách</h2>\n    <a href=\"/Books/Create\" class=\"btn btn-success\">+ Thêm Sách Mới</a>\n</div>")
                },
                "Trong Bootstrap 5, tổ hợp class class=\"d-flex justify-content-between align-items-center mb-3\" gắn trên một thẻ <div> có tác dụng gì?",
                new List<string>
                {
                    "A. Bật chế độ Flexbox (d-flex), đẩy các phần tử con ra sát 2 đầu trái/phải (justify-content-between), căn giữa theo chiều dọc (align-items-center) và tạo khoảng cách lề dưới 16px (mb-3)",
                    "B. Ẩn hoàn toàn thẻ div đó đi",
                    "C. Đổi màu nền thẻ div thành màu đỏ",
                    "D. Chia thẻ div thành 12 dòng bằng nhau"
                },
                "A",
                "Đây là tổ hợp class tiện ích được dùng nhiều nhất trong Bootstrap 5 để dàn hàng ngang tiêu đề và các nút công cụ một cách nhanh chóng mà không cần viết CSS thủ công."
            ),

            // =====================================================================
            // BÀI 6.5
            // =====================================================================
            Build(
                topicId,
                "6.5",
                "Kiến trúc Layout trong ASP.NET Core MVC (_Layout.cshtml, @RenderBody, @RenderSection, _ViewStart & Partial View)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/views/layout",
                """
                • **💡 1. Hiểu bản chất của `_Layout.cshtml` (Khung giao diện dùng chung) trong 30 giây:**
                  - Một trang web có 20 màn hình (Trang chủ, Danh sách sách, Độc giả, Thống kê...). Cả 20 màn hình này đều có chung **Thanh Header trên cùng**, **Thanh Sidebar bên trái** và **Footer dưới đáy**.
                  - Nếu bạn copy-paste code HTML của Header và Sidebar vào cả 20 file `.cshtml`, thì khi muốn sửa tên Menu bạn phải mở cả 20 file ra sửa!
                  - Trong ASP.NET Core, file **`Views/Shared/_Layout.cshtml`** chính là **"Chiếc Khung Tranh Mẫu"**: Nó chứa sẵn Header, Sidebar, Footer và chừa ra một ô trống ở chính giữa viết lệnh **`@RenderBody()`**. Khi bạn chuyển sang trang nào, nội dung riêng của trang đó sẽ được lắp vừa khít vào đúng vị trí `@RenderBody()`!
                • **🖼️ 2. Sơ đồ Cơ chế Ghép Khung `_Layout.cshtml` và View Con (`Index.cshtml`):**
                ```
                ┌───────────────────────── File: Views/Shared/_Layout.cshtml ─────────────────────────┐
                │  [HEADER CHUNG & MENU ĐIỀU HƯỚNG]                                                   │
                ├──────────────────┬──────────────────────────────────────────────────────────────────┤
                │                  │                                                                  │
                │  [SIDEBAR CHUNG] │    👉 @RenderBody()                                              │
                │                  │       (Nội dung của Index.cshtml / LessonDetail.cshtml           │
                │                  │        sẽ tự động được nhét vào đúng ô trống này!)               │
                │                  │                                                                  │
                ├──────────────────┴──────────────────────────────────────────────────────────────────┤
                │  [FOOTER CHUNG & NHÚNG SCRIPT: @await RenderSectionAsync("Scripts", required: false)]│
                └─────────────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng Giải Mã 4 Thành Phần Quản Lý Bố Cục Trong Thư Mục `Views/`:**
                | Tên File / Lệnh | Vai trò & Cách hoạt động | Lưu ý quan trọng |
                | :--- | :--- | :--- |
                | **`_ViewStart.cshtml`** | Chứa dòng `Layout = "_Layout";` tự động áp dụng khung `_Layout.cshtml` cho tất cả các trang mà không cần khai báo lặp lại. | Nếu 1 trang riêng (như trang *Đăng nhập*) muốn dùng khung khác hoặc không dùng khung, đặt `@{ Layout = null; }` ở đầu trang đó! |
                | **`_ViewImports.cshtml`**| Nơi khai báo `using` các `Models` và kích hoạt Tag Helpers (`@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers`) dùng chung cho mọi View. | Nhờ file này bạn mới gõ được `asp-controller`, `asp-action` trong các file `.cshtml`. |
                | **`@RenderBody()`** | Đánh dấu vị trí duy nhất trong `_Layout.cshtml` để đổ nội dung của View con vào. | Mỗi file Layout bắt buộc phải có **đúng 1 lệnh `@RenderBody()`**. |
                | **`@RenderSectionAsync`**| Cho phép View con chèn thêm file `.js` hoặc `.css` riêng vào đúng cuối hoặc đầu trang Layout (`@section Scripts { ... }`). | Tham số `required: false` nghĩa là trang con nào không có `@section Scripts` thì cũng không bị báo lỗi! |
                | **`Partial View` (`_TenFile.cshtml`)**| Mảnh giao diện nhỏ tách riêng (như *Hộp Chat AI*, *Thẻ Sản phẩm*) để nhúng vào nhiều nơi bằng `<partial name="_TenFile" />`. | Tên file Partial View luôn quy ước bắt đầu bằng dấu gạch dưới `_`. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết kế khung xương chung tại Views/Shared/_Layout.cshtml",
                        "📥 Đầu vào: HTML của Header, Sidebar và Footer. ⚙️ Cơ chế ngầm: Đặt @RenderBody() ở vùng <main> để đón nội dung các trang con; đặt @await RenderSectionAsync(\"Scripts\", required: false) ở ngay trước thẻ đóng </body>. 📤 Đầu ra: Khung giao diện thống nhất toàn web.",
                        "<body>\n    <header><!-- Menu chung --></header>\n    <main class=\"container\">\n        @RenderBody() <!-- Ruột của các trang con nằm ở đây -->\n    </main>\n    @await RenderSectionAsync(\"Scripts\", required: false)\n</body>"),
                    Step(2,
                        "2. Viết trang con gọn nhẹ (Views/Books/Index.cshtml)",
                        "📥 Đầu vào: Giao diện danh sách sách. ⚙️ Cơ chế ngầm: Nhờ _ViewStart.cshtml đã gán sẵn Layout, trong file Index.cshtml bạn KHÔNG CẦN viết lại <html>, <head>, <body> hay Menu nữa, chỉ viết đúng phần bảng dữ liệu và @section Scripts riêng! 📤 Đầu ra: Code View con cực kỳ ngắn gọn.",
                        "@model IEnumerable<Book>\n<h2>Danh sách sách trong thư viện</h2>\n<table class=\"table\">...</table>\n\n@section Scripts {\n    <script src=\"~/js/book-filter.js\"></script>\n}"),
                    Step(3,
                        "3. Tách mảnh giao diện tái sử dụng bằng Partial View",
                        "📥 Đầu vào: Widget Chatbot hoặc Thanh phân trang xuất hiện ở nhiều màn hình. ⚙️ Cơ chế ngầm: Tách ra file Views/Shared/_ChatBotWidget.cshtml và nhúng vào _Layout.cshtml bằng thẻ <partial>. 📤 Đầu ra: Dễ bảo trì, sửa 1 nơi cập nhật toàn web.",
                        "<partial name=\"_ChatBotWidget\" />")
                },
                "Trong file Views/Shared/_Layout.cshtml của dự án ASP.NET Core MVC, câu lệnh @await RenderSectionAsync(\"Scripts\", required: false) có ý nghĩa gì, và tại sao tham số required lại thường được đặt là false?",
                new List<string>
                {
                    "A. Dùng để định nghĩa vị trí chèn khối @section Scripts { ... } từ các View con vào cuối trang Layout; đặt required: false để những trang View con nào không cần viết thêm JavaScript (không khai báo @section Scripts) vẫn chạy bình thường mà không bị văng lỗi",
                    "B. Dùng để tắt toàn bộ JavaScript của trang web",
                    "C. Bắt buộc mọi trang con đều phải viết ít nhất 100 dòng JavaScript",
                    "D. Dùng để kết nối với SQL Server"
                },
                "A",
                "Nếu đặt required: true, bất kỳ View con nào không có khối @section Scripts sẽ làm ứng dụng văng lỗi InvalidOperationException ngay khi mở trang. Do đó luôn để required: false cho các section tùy chọn như Scripts hoặc Styles."
            ),

            // =====================================================================
            // BÀI 6.6
            // =====================================================================
            Build(
                topicId,
                "6.6",
                "Cú pháp Razor Engine (@) và 4 Cách truyền dữ liệu từ Controller ra View (@model, ViewBag, ViewData, TempData)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/views/overview",
                """
                • **💡 1. Hiểu bản chất của `Razor Syntax (@)` trong 30 giây:**
                  - File `.cshtml` là sự kết hợp tuyệt vời giữa **HTML** và **C#**. Ký hiệu **`@`** chính là **"Công tắc chuyển mạch"**: Hễ gõ ký tự `@`, trình biên dịch biết ngay phần chữ đứng đằng sau nó là **Code C# chạy trên Server** (như `@DateTime.Now`, `@if`, `@foreach`), sau khi tính toán ra kết quả xong mới in thành HTML thuần gửi về trình duyệt!
                • **🖼️ 2. Sơ đồ 4 Con Đường Truyền Dữ Liệu Từ `Controller` Sang `View (.cshtml)`:**
                ```
                Controller (C#)                                            View (.cshtml)
                ┌──────────────────────────────────────────┐               ┌────────────────────────────────────────┐
                │ 1. return View(data);  ──────────────────┼─────────────► │ @model List<Book>  (Truy cập qua Model)│
                │    (Strongly-Typed: Khuyên dùng số 1!)   │               │ -> Tự gợi ý code, an toàn kiểu 100%!   │
                ├──────────────────────────────────────────┤               ├────────────────────────────────────────┤
                │ 2. ViewBag.Title = "Thư viện"; ──────────┼─────────────► │ <h1>@ViewBag.Title</h1>                │
                │    (Kiểu dynamic: Dùng cho dữ liệu phụ)  │               │ (Nhanh gọn, không cần ép kiểu)         │
                ├──────────────────────────────────────────┤               ├────────────────────────────────────────┤
                │ 3. ViewData["Count"] = 10; ──────────────┼─────────────► │ @((int)ViewData["Count"])              │
                │    (Kiểu Dictionary object)              │               │ (Phải ép kiểu khi tính toán)           │
                ├──────────────────────────────────────────┤               ├────────────────────────────────────────┤
                │ 4. TempData["Success"] = "Lưu xong!"; ───┼──(Qua bước)─► │ Hiển thị thông báo Toast sau khi đã    │
                │    (Sống sót qua 1 lần Redirect!)        │  Redirect     │ RedirectToAction("Index")!             │
                └──────────────────────────────────────────┘               └────────────────────────────────────────┘
                ```
                • **📊 3. Bảng So Sánh `Strongly-Typed @model` vs `ViewBag` vs `ViewData` vs `TempData` (Câu hỏi bảo vệ đồ án MVC):**
                | Công cụ | Bản chất kỹ thuật | Thời gian sống của dữ liệu | Khi nào nên dùng? |
                | :--- | :--- | :--- | :--- |
                | **`@model <Kiểu>`** (`Model`)| Định kiểu mạnh (*Strongly-typed*). Chữ `@model` (thường) ở dòng 1 để khai báo kiểu; chữ `@Model` (hoa) ở dưới để lấy dữ liệu. | Trong **1 lần Request** hiện tại. | ⭐ **Dùng cho dữ liệu chính của trang** (Danh sách sách, Chi tiết bài học, Form tạo mới). |
                | **`ViewBag`** | Kiểu động (`dynamic` bọc bên ngoài `ViewData`). | Trong **1 lần Request** hiện tại (Nếu `RedirectToAction` sẽ bị mất trắng!). | Truyền các thông tin phụ nhỏ (Tiêu đề trang, Danh sách `SelectList` cho ô thả xuống Dropdown). |
                | **`ViewData["Key"]`**| Từ điển `ViewDataDictionary` lưu giá trị kiểu `object`. | Trong **1 lần Request** hiện tại. | Truyền dữ liệu giữa View con lên `_Layout.cshtml` (VD: `ViewData["Title"]`). |
                | **`TempData["Key"]`**| Lưu trong Session/Cookie tạm thời, **đọc ra 1 lần là tự hủy**. | ⭐ **Sống sót qua 1 lần chuyển hướng (`RedirectToAction`)!** | Hiển thị thông báo *"Thêm mới / Cập nhật thành công!"* sau khi lưu Form và chuyển về trang Danh sách! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Truyền dữ liệu chính bằng Strongly-Typed Model (Chuẩn nhất)",
                        "📥 Đầu vào: Danh sách List<Book> lấy từ Database trong Controller. ⚙️ Cơ chế ngầm: Truyền biến books vào bên trong hàm return View(books). Ở dòng đầu tiên của file .cshtml khai báo @model IEnumerable<Book>. 📤 Đầu ra: Gõ @Model. được Visual Studio gợi ý từng thuộc tính chuẩn xác.",
                        "// 1. Tại BooksController.cs:\nvar books = await _db.Books.ToListAsync();\nreturn View(books);\n\n// 2. Tại Views/Books/Index.cshtml:\n@model IEnumerable<Book>\n@foreach (var item in Model) {\n    <p>@item.Title - @item.Price.ToString(\"N0\") đ</p>\n}"),
                    Step(2,
                        "2. Truyền dữ liệu phụ (Dropdown Danh mục) bằng ViewBag",
                        "📥 Đầu vào: Trang thêm mới Sách cần thêm 1 danh sách Thể loại để chọn. ⚙️ Cơ chế ngầm: Model chính đã dành cho class Book rồi, nên ta gắn danh sách Thể loại vào ViewBag.Categories. 📤 Đầu ra: View nhận được cả Model chính lẫn danh sách phụ.",
                        "// Tại BooksController.cs:\nViewBag.CategoryList = new SelectList(await _db.Categories.ToListAsync(), \"Id\", \"Name\");\nreturn View(new Book());"),
                    Step(3,
                        "3. Gửi thông báo qua RedirectToAction bằng TempData",
                        "📥 Đầu vào: Lưu sách mới vào DB thành công và chuyển hướng về trang Index. ⚙️ Cơ chế ngầm: Nếu dùng ViewBag sẽ bị mất ngay khi Redirect. Dùng TempData[\"Msg\"] giúp giữ lại câu thông báo sang trang Index để hiện hộp Alert xanh lá! 📤 Đầu ra: Trải nghiệm thông báo mượt mà.",
                        "// Tại hàm POST Create:\nawait _db.SaveChangesAsync();\nTempData[\"SuccessMsg\"] = \"Đã thêm sách mới thành công!\";\nreturn RedirectToAction(\"Index\");\n\n// Tại trang Index.cshtml:\n@if (TempData[\"SuccessMsg\"] != null) {\n    <div class=\"alert alert-success\">@TempData[\"SuccessMsg\"]</div>\n}")
                },
                "Khi đi bảo vệ bài tập lớn ASP.NET Core MVC, thầy cô hỏi: \"Sau khi lưu dữ liệu ở hàm [HttpPost] Create xong và gọi lệnh return RedirectToAction(\"Index\"); để quay về trang danh sách, nếu em muốn truyền câu thông báo 'Thêm mới thành công!' sang trang Index hiển thị thì em phải dùng ViewBag hay TempData? Vì sao?\". Bạn trả lời thế nào?",
                new List<string>
                {
                    "A. Bắt buộc phải dùng TempData! Vì RedirectToAction tạo ra một HTTP Request mới hoàn toàn làm xóa sạch dữ liệu trong ViewBag/ViewData, chỉ có TempData mới lưu giữ được dữ liệu băng qua một lần Redirect",
                    "B. Dùng ViewBag vì ViewBag lưu dữ liệu vĩnh viễn trong SQL Server",
                    "C. Dùng cái nào cũng được vì cả hai giống hệt nhau",
                    "D. Không thể truyền thông báo khi dùng RedirectToAction"
                },
                "A",
                "ViewBag và ViewData chỉ tồn tại trong phạm vi vòng đời của 1 HTTP Request đơn lẻ. Khi gọi RedirectToAction(), Server trả mã 302 bảo trình duyệt gọi một Request mới tới trang Index, do đó phải dùng TempData để giữ thông báo qua bước chuyển hướng này."
            ),

            // =====================================================================
            // BÀI 6.7
            // =====================================================================
            Build(
                topicId,
                "6.7",
                "Sức mạnh của Tag Helpers trong ASP.NET Core MVC (asp-controller, asp-action, asp-route-id, asp-for, asp-validation-for)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/mvc/views/tag-helpers/intro",
                """
                • **💡 1. Hiểu bản chất của `Tag Helpers` trong 30 giây:**
                  - Ở Bài 6.1, khi viết HTML thuần bạn phải tự gõ cứng đường dẫn `<a href="/Books/Edit/5">` và tự gõ `<input type="text" id="Price" name="Price" value="@Model.Price" />` rất dễ gõ sai chính tả!
                  - **Tag Helpers (`asp-...`)** là các thuộc tính thông minh của ASP.NET Core gắn trực tiếp vào thẻ HTML: Bạn chỉ cần chọn tên Controller, tên Action và chọn thuộc tính C# (`asp-for="Price"`), hệ thống sẽ **tự động sinh ra đầy đủ `href`, `id`, `name`, `type`, `value` và cả luật kiểm tra lỗi Validation** chuẩn xác 100%!
                • **🖼️ 2. Giải phẫu Quá trình Biến Hình Của `asp-for` Từ Razor Sang HTML Trình Duyệt:**
                ```html
                <!-- 1. Bạn chỉ cần gõ ngắn gọn trong file .cshtml: -->
                <input asp-for="Email" class="form-control" />

                <!-- 2. ASP.NET Core đọc thuộc tính Email trong Model C# và TỰ ĐỘNG SINH RA HTML: -->
                <input type="email"
                       id="Email"
                       name="Email"
                       value="viet@gmail.com"
                       data-val="true"
                       data-val-required="Email không được để trống"
                       class="form-control" />
                ```
                • **📊 3. Bảng 5 Tag Helpers Xương Sống Dùng Trong Mọi Đồ Án MVC:**
                | Tag Helper | Gắn trên thẻ nào? | Ví dụ sử dụng chuẩn | Tác dụng tự động |
                | :--- | :--- | :--- | :--- |
                | **`asp-controller` & `asp-action`**| Thẻ `<a>` hoặc `<form>` | `<a asp-controller="Books" asp-action="Index">` | Tự động sinh đường dẫn `href="/Books"` chuẩn theo cấu hình Route trong `Program.cs`. |
                | **`asp-route-{tên_tham_số}`** | Thẻ `<a>` hoặc `<form>` | `<a asp-action="Edit" asp-route-id="@item.Id">` | Tự động gắn mã ID của dòng hiện tại vào URL (VD: `/Books/Edit/15`) để truyền vào tham số `int id` của hàm C#! |
                | **`asp-for="Tên_Property"`** | `<label>`, `<input>`, `<select>`, `<textarea>` | `<input asp-for="Title" class="form-control" />` | Tự động gắn `id`, `name`, `type` (số, ngày tháng, mật khẩu) và tự đổ dữ liệu cũ vào ô khi làm trang Sửa (`Edit`)! |
                | **`asp-items="ViewBag..."`** | Thẻ `<select>` (Dropdown)| `<select asp-for="CategoryId" asp-items="ViewBag.Categories">` | Tự động sinh hàng loạt thẻ `<option value="...">` từ danh sách `SelectList` mà không cần viết vòng lặp `foreach`! |
                | **`asp-validation-for="..."`**| Thẻ `<span>` báo lỗi | `<span asp-validation-for="Title" class="text-danger"></span>` | Tự động hiện dòng chữ báo lỗi màu đỏ nếu người dùng bỏ trống trường `[Required]`! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo nút Xem chi tiết / Sửa / Xóa bằng asp-action và asp-route-id",
                        "📥 Đầu vào: Một dòng sách 'item' trong vòng lặp @foreach. ⚙️ Cơ chế ngầm: asp-route-id=\"@item.Id\" tự động ghép mã Id của cuốn sách đó vào URL để khớp với tham số (int id) trong BooksController. 📤 Đầu ra: Link điều hướng chính xác từng bản ghi.",
                        "<a asp-controller=\"Books\" asp-action=\"Edit\" asp-route-id=\"@item.Id\" class=\"btn btn-sm btn-warning\">\n    Chỉnh sửa\n</a>\n<!-- Tự sinh ra: <a href=\"/Books/Edit/12\" class=\"btn btn-sm btn-warning\">Chỉnh sửa</a> -->"),
                    Step(2,
                        "2. Tạo Form Thêm mới / Cập nhật tự động ràng buộc bằng asp-for",
                        "📥 Đầu vào: Model Book. ⚙️ Cơ chế ngầm: asp-for=\"Title\" vừa tự đặt name=\"Title\" để gửi về Backend, vừa tự điền tên sách cũ vào ô input khi mở trang Edit, đồng thời thêm thẻ chống giả mạo Request (AntiForgeryToken). 📤 Đầu ra: Form an toàn và đồng bộ 2 chiều.",
                        "<form asp-controller=\"Books\" asp-action=\"Edit\" method=\"post\">\n    <input type=\"hidden\" asp-for=\"Id\" />\n    <div class=\"mb-3\">\n        <label asp-for=\"Title\" class=\"form-label\">Tên cuốn sách</label>\n        <input asp-for=\"Title\" class=\"form-control\" />\n        <span asp-validation-for=\"Title\" class=\"text-danger\"></span>\n    </div>\n    <button type=\"submit\" class=\"btn btn-primary\">Lưu thay đổi</button>\n</form>"),
                    Step(3,
                        "3. Tạo ô chọn Danh mục (Dropdown) siêu gọn với asp-items",
                        "📥 Đầu vào: ViewBag.CategoryList (kiểu SelectList) truyền từ Controller. ⚙️ Cơ chế ngầm: Thẻ <select> tự động duyệt danh sách để vẽ các thẻ <option> và tự động chọn đúng danh mục hiện tại của cuốn sách. 📤 Đầu ra: Dropdown hoàn chỉnh không cần viết vòng lặp.",
                        "<select asp-for=\"CategoryId\" asp-items=\"ViewBag.CategoryList\" class=\"form-select\">\n    <option value=\"\">-- Chọn thể loại sách --</option>\n</select>")
                },
                "Trong trang Chỉnh sửa thông tin (Edit.cshtml) của ASP.NET Core MVC, tại sao bên trong thẻ <form asp-action=\"Edit\" method=\"post\"> chúng ta luôn phải đặt một thẻ ẩn <input type=\"hidden\" asp-for=\"Id\" />?",
                new List<string>
                {
                    "A. Để khi người dùng bấm nút Lưu (Submit), mã khóa chính Id của bản ghi đang sửa được gửi kèm về hàm [HttpPost] Edit trong Controller; nếu thiếu dòng này thì Id gửi về sẽ bằng 0 và EF Core không biết phải cập nhật dòng nào trong Database",
                    "B. Để trang trí cho giao diện đẹp hơn",
                    "C. Để xóa bản ghi đó khỏi Database",
                    "D. Vì HTML bắt buộc mọi form đều phải có ít nhất 10 thẻ hidden"
                },
                "A",
                "Người dùng không cần nhìn thấy hay sửa mã Id trên giao diện, nhưng Backend bắt buộc phải nhận lại được mã Id đó khi Submit Form để tìm đúng bản ghi trong SQL Server và cập nhật. Do đó <input type=\"hidden\" asp-for=\"Id\" /> là thành phần không thể thiếu trong mọi form Edit."
            ),

            // =====================================================================
            // BÀI 6.8
            // =====================================================================
            Build(
                topicId,
                "6.8",
                "Tư duy Component-Based UI với Blazor (Sự khác biệt giữa Razor MVC/Pages và Blazor Server / WebAssembly)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/blazor/",
                """
                • **💡 1. Hiểu bản chất của `Blazor` trong 30 giây:**
                  - Bình thường để làm giao diện web tương tác mượt mà không cần tải lại trang (SPA - Single Page Application), lập trình viên phải học thêm ngôn ngữ **JavaScript** (như React, Angular, Vue).
                  - **Blazor (`Browser + Razor`)** là công nghệ đột phá của Microsoft cho phép bạn **viết code xử lý sự kiện bấm nút, cập nhật giao diện trực tiếp bằng 100% ngôn ngữ C# ngay trên trình duyệt** mà hầu như không cần đụng đến JavaScript!
                • **🖼️ 2. Sơ đồ So Sánh 3 Công Nghệ Giao Diện Trong Hệ Sinh Thái .NET:**
                ```
                ┌──────────────────────────────────────────────────────────────────────────────┐
                │ 1. ASP.NET Core MVC / Razor Pages (.cshtml - Server-Side Rendering):         │
                │    Bấm nút Submit ──► Gửi lên Server ──► Server vẽ lại trang HTML mới trả về │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 2. Blazor Server (.razor - Tương tác thời gian thực qua cáp ngầm SignalR):   │
                │    Bấm nút (@onclick) ──► Tín hiệu bay qua ống SignalR lên Server chạy C#    │
                │                       ──► Chỉ cập nhật đúng con số thay đổi trên màn hình!   │
                ├──────────────────────────────────────────────────────────────────────────────┤
                │ 3. Blazor WebAssembly (.razor - Chạy C# trực tiếp ngay trong Trình duyệt!):  │
                │    Tải bộ máy .NET nhỏ gọn về trình duyệt ──► Bấm nút chạy C# ngay tại máy   │
                │    khách, gọi dữ liệu về Backend thông qua ASP.NET Core Web API!             │
                └──────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng So Sánh Khi Nào Dùng `MVC (.cshtml)` vs `Blazor (.razor)`:**
                | Tiêu chí | ASP.NET Core MVC (`.cshtml`) | Blazor (`.razor`) |
                | :--- | :--- | :--- |
                | **Đuôi file giao diện** | `.cshtml` | `.razor` (Chữ cái đầu của tên file luôn **Viết Hoa**, VD: `ProductCard.razor`). |
                | **Đơn vị cấu thành** | Theo từng Trang (*Page-based*) điều khiển bởi `Controller`. | Theo từng **Linh kiện lắp ghép (*Component-based*)** có thể tái sử dụng như xếp hình Lego! |
                | **Xử lý bấm nút** | Phải gửi `<form>` hoặc viết thêm `JavaScript / Fetch / Ajax`. | Gắn trực tiếp hàm C# vào nút bấm: `<button @onclick="TangSoLuong">+1</button>`! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cấu trúc 3 phần của một file Blazor Component (.razor)",
                        "📥 Đầu vào: Tạo file Counter.razor. ⚙️ Cơ chế ngầm: Đầu file là @page \"/counter\" (đường dẫn URL), ở giữa là giao diện HTML, và ở cuối là khối @code { ... } chứa các biến và hàm C# xử lý trực tiếp cho giao diện đó! 📤 Đầu ra: Một Component độc lập.",
                        "@page \"/counter\"\n\n<h3>Số lượng hiện tại: @currentCount</h3>\n<button class=\"btn btn-primary\" @onclick=\"IncrementCount\">Bấm tăng +1</button>\n\n@code {\n    private int currentCount = 0;\n    private void IncrementCount() {\n        currentCount++; // Giao diện tự động nhảy số ngay lập tức không cần load lại trang!\n    }\n}"),
                    Step(2,
                        "2. Tái sử dụng Component như một thẻ HTML mới",
                        "📥 Đầu vào: Đã có file Counter.razor hoặc AlertBox.razor. ⚙️ Cơ chế ngầm: Ở bất kỳ trang nào khác, bạn chỉ cần gõ tên file như một thẻ HTML (<Counter />) là cả khối giao diện và logic C# được nhúng vào ngay. 📤 Đầu ra: Lắp ghép giao diện kiểu Lego.",
                        "<!-- Tại trang Home.razor, nhúng Component Counter vào 2 lần: -->\n<div class=\"row\">\n    <div class=\"col-6\"><Counter /></div>\n    <div class=\"col-6\"><Counter /></div>\n</div>"),
                    Step(3,
                        "3. Tích hợp Blazor Component vào ngay trong dự án MVC hiện có",
                        "📥 Đầu vào: Dự án ASP.NET Core MVC (.cshtml). ⚙️ Cơ chế ngầm: Bạn hoàn toàn có thể nhúng một Blazor Component vào bên trong file .cshtml của MVC bằng Tag Helper <component type=\"typeof(...)\" />! 📤 Đầu ra: Kết hợp sức mạnh của cả MVC và Blazor.",
                        "<!-- Bên trong file Index.cshtml của MVC: -->\n<component type=\"typeof(Counter)\" render-mode=\"ServerPrerendered\" />")
                },
                "Điểm khác biệt lớn nhất giúp lập trình viên C# yêu thích Blazor (.razor) khi xây dựng các giao diện tương tác động trên trình duyệt là gì?",
                new List<string>
                {
                    "A. Cho phép viết toàn bộ logic xử lý sự kiện giao diện (như @onclick, @onchange, cập nhật trạng thái màn hình) trực tiếp bằng ngôn ngữ C# bên trong khối @code { ... } mà không bắt buộc phải viết JavaScript",
                    "B. Không cần sử dụng thẻ HTML hay CSS nữa",
                    "C. Tự động thiết kế cơ sở dữ liệu mà không cần SQL",
                    "D. Chỉ chạy được trên hệ điều hành Linux"
                },
                "A",
                "Blazor thống nhất cả ngôn ngữ phía Server và phía Client về chung C# (.NET), cho phép xây dựng giao diện hướng thành phần (Component-based) và bắt sự kiện người dùng trực tiếp bằng C#."
            ),

            // =====================================================================
            // BÀI 6.9
            // =====================================================================
            Build(
                topicId,
                "6.9",
                "Giao tiếp dữ liệu trong Blazor Components: Parameters ([Parameter]), Data Binding (@bind) và EventCallback",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/blazor/components/data-binding",
                """
                • **💡 1. Hiểu bản chất của `Two-Way Data Binding (@bind)` và `[Parameter]` trong 30 giây:**
                  - **1. `[Parameter]` (Truyền dữ liệu từ Cha xuống Con):** Khi Component Cha (`BookList.razor`) muốn truyền thông tin cuốn sách xuống cho Component Con (`BookItem.razor`) hiển thị, ở Component Con bạn chỉ cần gắn nhãn **`[Parameter]`** lên thuộc tính `public Book Item { get; set; }`.
                  - **2. `@bind` (Ràng buộc dữ liệu 2 chiều tự động):** Trong HTML truyền thống, muốn lấy chữ người dùng đang gõ trong ô `<input>` phải viết sự kiện JavaScript `oninput`. Trong Blazor, bạn chỉ cần viết **`<input @bind="searchKeyword" />`**:
                    + Chiều 1: Biến `searchKeyword` trong C# đổi giá trị $\rightarrow$ Ô input trên màn hình tự đổi chữ theo.
                    + Chiều 2: Người dùng gõ chữ vào ô input $\rightarrow$ Biến `searchKeyword` trong C# tự động cập nhật theo từng phím gõ!
                • **🖼️ 2. Sơ đồ Luồng Truyền Dữ Liệu Giữa Component Cha và Component Con Trong Blazor:**
                ```
                ┌────────────────────────── COMPONENT CHA (Parent.razor) ──────────────────────────┐
                │                                                                                  │
                │      1. Truyền dữ liệu xuống qua thuộc tính [Parameter]                          │
                │         (Ví dụ: <BookCard Price="150000" Title="Lập trình C#"/>)                │
                │                                  │                                               │
                │                                  ▼                                               │
                │      ┌─────────────── COMPONENT CON (BookCard.razor) ───────────────┐            │
                │      │  [Parameter] public string Title { get; set; }               │            │
                │      │  [Parameter] public EventCallback<int> OnAddToCart {get;set;}│            │
                │      └───────────────────────────┬──────────────────────────────────┘            │
                │                                  │                                               │
                │      2. Báo sự kiện ngược lên Cha qua EventCallback (Khi bấm nút Mua) ◄──────────┘
                └──────────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng 4 Vòng Đời & Thuộc Tính Cốt Lõi Của Blazor Component:**
                | Cú pháp / Hàm | Công dụng thực tế |
                | :--- | :--- |
                | **`[Parameter]`** | Khai báo thuộc tính nhận dữ liệu từ Component Cha truyền vào. |
                | **`@bind="tenBien"`** | Đồng bộ 2 chiều giữa ô `<input>` / `<select>` và biến C# (Thêm `@bind:event="oninput"` để cập nhật ngay từng nhịp gõ phím!). |
                | **`EventCallback<T>`** | Phát tín hiệu kèm dữ liệu từ Component Con bắn ngược lên báo cho Component Cha xử lý. |
                | **`OnInitializedAsync()`** | Hàm vòng đời tự động chạy đầu tiên khi Component vừa mở lên — **nơi chuẩn nhất để gọi API / Service lấy dữ liệu ban đầu**! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Ràng buộc 2 chiều tức thì với @bind và @bind:event=\"oninput\"",
                        "📥 Đầu vào: Ô tìm kiếm sách trực tiếp (Live Search). ⚙️ Cơ chế ngầm: Mỗi khi người dùng gõ 1 ký tự vào ô input, biến keyword trong @code tự động cập nhật ngay và lọc lại danh sách hiển thị bên dưới mà không cần bấm nút Enter! 📤 Đầu ra: Tìm kiếm tức thì siêu mượt.",
                        "<input @bind=\"keyword\" @bind:event=\"oninput\" class=\"form-control\" placeholder=\"Gõ để lọc ngay...\" />\n<p>Bạn đang tìm: <strong>@keyword</strong></p>\n\n@code {\n    private string keyword = \"\";\n}"),
                    Step(2,
                        "2. Tạo Component Con nhận dữ liệu qua [Parameter]",
                        "📥 Đầu vào: Thiết kế thẻ hiển thị bài học LessonCard.razor. ⚙️ Cơ chế ngầm: Gắn [Parameter] lên các thuộc tính public để bên ngoài có thể truyền giá trị vào giống như thuộc tính của thẻ HTML. 📤 Đầu ra: Component linh hoạt.",
                        "<!-- File: LessonCard.razor -->\n<div class=\"card p-3\">\n    <h5>@LessonTitle</h5>\n    <button class=\"btn btn-sm btn-success\" @onclick=\"() => OnCompleted.InvokeAsync(LessonId)\">Hoàn thành</button>\n</div>\n\n@code {\n    [Parameter] public int LessonId { get; set; }\n    [Parameter] public string LessonTitle { get; set; } = \"\";\n    [Parameter] public EventCallback<int> OnCompleted { get; set; }\n}"),
                    Step(3,
                        "3. Tải dữ liệu bất đồng bộ trong hàm vòng đời OnInitializedAsync",
                        "📥 Đầu vào: Component bắt đầu được hiển thị lên màn hình. ⚙️ Cơ chế ngầm: Ghi đè (override) hàm OnInitializedAsync() để gọi Service hoặc HttpClient lấy danh sách từ Database đổ vào biến. 📤 Đầu ra: Hiển thị dữ liệu lên giao diện.",
                        "@inject IBookService BookService\n\n@code {\n    private List<BookDto> books = new();\n\n    protected override async Task OnInitializedAsync() {\n        books = await BookService.GetAllAsync();\n    }\n}")
                },
                "Trong một Blazor Component (.razor), để khai báo một thuộc tính public string CourseName { get; set; } có thể nhận dữ liệu truyền từ Component Cha xuống (ví dụ: <CourseItem CourseName=\"Lập trình .NET\" />), bạn bắt buộc phải gắn Attribute nào lên trên thuộc tính đó?",
                new List<string>
                {
                    "A. Gắn thẻ [Parameter]",
                    "B. Gắn thẻ [HttpGet]",
                    "C. Gắn thẻ [FromBody]",
                    "D. Gắn thẻ [Key]"
                },
                "A",
                "Trong Blazor, thuộc tính [Parameter] đánh dấu một Property là cổng nhận tham số đầu vào từ Component cha."
            ),

            // =====================================================================
            // BÀI 6.10
            // =====================================================================
            Build(
                topicId,
                "6.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 6 (Xây dựng Giao diện MVC & Blazor kết hợp AJAX/Fetch gọi API)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/tutorials/first-web-api",
                """
                • **💡 1. Mảnh ghép cuối cùng cho Bài Tập Lớn MVC & Thực Tập: Gọi API ngầm bằng `Fetch API (AJAX)` không cần tải lại trang!**
                  - Trong dự án ASP.NET Core MVC (như trang web học tập *Hoàng Việt* của bạn hoặc dự án *Quản lý Thư viện*), có những nút bấm bạn muốn khi click vào thì **chỉ cập nhật đúng 1 góc màn hình mà không bị chớp trắng tải lại cả trang web** (ví dụ: bấm nút *"Gửi câu hỏi cho ViệtBot AI"* hoặc bấm nút *"Đánh dấu Hoàn thành bài học"*).
                  - Cách làm chuẩn nhất trong MVC là dùng hàm **`fetch()` của JavaScript hiện đại** (nằm trong `@section Scripts`) gọi ngầm lên một hàm Action trả về `JsonResult` (hoặc Web API Controller) ở Backend!
                • **🖼️ 2. Sơ đồ Luồng Gọi Ngầm `Fetch (AJAX)` Giữa Giao diện `.cshtml` và `Controller C#`:**
                ```
                Giao diện View (.cshtml)                                     Controller (C#)
                ┌────────────────────────────────────────┐                   ┌──────────────────────────────────────────┐
                │ Người dùng bấm nút "Hoàn thành bài"    │                   │ [HttpPost]                               │
                │               │                        │                   │ public async Task<IActionResult>         │
                │               ▼                        │  1. Gửi ngầm JSON │ CompleteLesson(int id) {                 │
                │ Hàm JS: await fetch('/Learning/Done') ─┼──────────────────►│     // Lưu SQL Server: IsCompleted=true  │
                │                                        │                   │     return Json(new { success = true }); │
                │ Nhận JSON ◄────────────────────────────┼───────────────────│ }                                        │
                │ Đổi màu nút sang Xanh lá ngay lập tức! │  2. Trả về JSON   └──────────────────────────────────────────┘
                └────────────────────────────────────────┘  (Trang web đứng im mượt mà, KHÔNG bị load lại!)
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Dựng giao diện chuẩn Bootstrap 5 & Razor Tag Helpers",
                        "📥 Đầu vào: Model bài học hoặc sách. ⚙️ Cơ chế ngầm: Sử dụng Grid System (row, col-md-8), Card Bootstrap và các Tag Helper asp-for để hiển thị dữ liệu. 📤 Đầu ra: Giao diện hiện đại, Responsive.",
                        "<div class=\"card shadow-sm p-4\">\n    <h4>@Model.Title</h4>\n    <button id=\"btnComplete\" onclick=\"markCompleted(@Model.Id)\" class=\"btn btn-success\">\n        Đánh dấu Hoàn thành\n    </button>\n</div>"),
                    Step(2,
                        "2. Viết hàm C# trong Controller trả về JsonResult",
                        "📥 Đầu vào: Mã id của bài học cần đánh dấu. ⚙️ Cơ chế ngầm: Cập nhật trạng thái xuống Database thông qua EF Core và trả về đối tượng JSON bằng hàm return Json(...) hoặc return Ok(...). 📤 Đầu ra: Phản hồi JSON gọn nhẹ.",
                        "[HttpPost]\npublic async Task<IActionResult> Complete(int id) {\n    await _learningService.MarkAsDoneAsync(id);\n    return Json(new { isSuccess = true, newProgress = 65 });\n}"),
                    Step(3,
                        "3. Dùng hàm async fetch() trong @section Scripts để gọi ngầm",
                        "📥 Đầu vào: Sự kiện click nút của người dùng. ⚙️ Cơ chế ngầm: Hàm fetch() gửi HTTP POST ngầm ở hậu trường và dùng await response.json() để đọc kết quả trả về. 📤 Đầu ra: Dữ liệu JSON từ Server.",
                        "@section Scripts {\n<script>\nasync function markCompleted(lessonId) {\n    const res = await fetch(`/Learning/Complete/${lessonId}`, { method: 'POST' });\n    const data = await res.json();"),
                    Step(4,
                        "4. Cập nhật trực tiếp phần tử trên DOM mà không tải lại trang",
                        "📥 Đầu vào: Kết quả data.isSuccess == true. ⚙️ Cơ chế ngầm: Đổi chữ và màu sắc của nút bấm ngay trên màn hình hiện tại. 📤 Đầu ra: Trải nghiệm mượt mà như ứng dụng Single Page App!",
                        "    if (data.isSuccess) {\n        const btn = document.getElementById('btnComplete');\n        btn.innerText = '✓ Đã hoàn thành!';\n        btn.classList.replace('btn-success', 'btn-secondary');\n    }\n}\n</script>\n}")
                },
                "(Câu hỏi tổng hợp Chủ đề 6) Trong dự án ASP.NET Core MVC, khi bạn muốn người dùng bấm vào nút \"Thả tim / Đánh dấu hoàn thành\" hoặc \"Gửi tin nhắn Chatbot\" mà trang web không bị chớp trắng tải lại (Full Page Reload), giải pháp kết hợp nào dưới đây là chuẩn xác nhất?",
                new List<string>
                {
                    "A. Dùng hàm fetch() (AJAX) trong JavaScript gọi ngầm tới một Action trong Controller trả về dữ liệu JSON (return Json(...) hoặc return Ok(...)), sau đó dùng JS cập nhật lại đúng phần tử HTML đó trên màn hình (hoặc sử dụng Blazor Component)",
                    "B. Bắt người dùng phải tắt trình duyệt mở lại",
                    "C. Chỉ dùng thẻ <a href=\"...\"> thông thường",
                    "D. Viết câu lệnh SQL trực tiếp vào trong file CSS"
                },
                "A",
                "Kết hợp fetch() (AJAX) gọi Action trả về JSON (hoặc nhúng Blazor Component) giúp ứng dụng MVC đạt được độ mượt mà của Single Page Application ở những tính năng tương tác tức thời mà vẫn giữ được sự đơn giản của kiến trúc MVC."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (6.1 -> 6.10) cho Chủ đề 6 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic6 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 6);
        if (topic6 == null)
            return;

        topic6.Title = "Chủ đề 6: Xây dựng giao diện với Blazor và Bootstrap";
        topic6.Description = "10 Bài học con siêu chi tiết từ HTML5 Semantic & DOM, CSS3 Box Model/Flexbox/Grid, Bootstrap 5 Grid 12 cột & Utility Classes, Kiến trúc _Layout.cshtml/@RenderBody, Razor (@model/ViewBag/TempData), Tag Helpers (asp-for), Blazor Component (.razor) đến Fetch API (AJAX).";

        var targetLessons = GetLessons(topic6.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic6.Id)
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
