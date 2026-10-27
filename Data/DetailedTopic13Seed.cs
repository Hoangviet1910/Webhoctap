using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic13Seed
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
            // BÀI 13.1
            // =====================================================================
            Build(
                topicId,
                "13.1",
                "Chuẩn bị Đóng gói Ứng dụng .NET lên Môi trường Thật (dotnet publish, Release Mode & Kestrel vs Reverse Proxy)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/dotnet/core/deploying/",
                """
                • **💡 1. Hiểu bản chất quá trình `Publish` (Đóng gói xuất xưởng) trong 30 giây:**
                  - Khi bạn đang gõ code trên máy cá nhân và bấm `F5` (chế độ **Debug**), Visual Studio giữ lại rất nhiều thông tin dò lỗi nặng nề và cần có bộ công cụ lập trình **`.NET SDK`** mới chạy được.
                  - Khi mang web lên máy chủ thật (**Server Production**) cho hàng nghìn người dùng truy cập: Bạn không bao giờ copy cả thư mục code `.cs` thô lên Server! Thay vào đó, bạn dùng lệnh **`dotnet publish -c Release`** để biên dịch và nén toàn bộ code thành các file nhị phân `.dll` siêu nhẹ, đã được tối ưu hóa tốc độ tối đa!
                • **🖼️ 2. Sơ đồ Kiến trúc Máy Chủ Web Thực Tế: `Nginx / IIS (Reverse Proxy)` Đứng Trước `Kestrel`:**
                ```
                Người dùng Internet (Gọi https://hoangviet.vn - Cổng 443)
                      │
                      ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🛡️ Tầng 1 - Reverse Proxy (Nginx trên Linux HOẶC IIS trên Windows Server): │
                │    - Chịu trận ngoài cùng: Quản lý chứng chỉ bảo mật SSL (HTTPS),          │
                │      chặn DDoS, nén tĩnh và chuyển tiếp gói tin vào cổng nội bộ 5000.      │
                └─────────────────────────────────────┬──────────────────────────────────────┘
                                                      │ (Chuyển tiếp nội bộ http://localhost:5000)
                                                      ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🚀 Tầng 2 - Kestrel Server (Máy chủ Web siêu tốc tích hợp sẵn trong .NET): │
                │    - Chạy file HoangViet.dll, xử lý C# Controller, Service và gọi SQL!     │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bảng So Sánh 2 Chế Độ Đóng Gói (`Framework-Dependent` vs `Self-Contained`):**
                | Chế độ Publish | Câu lệnh thực hiện | Đặc điểm & Khi nào sử dụng? |
                | :--- | :--- | :--- |
                | **1. Phụ thuộc Runtime (`Framework-Dependent`)** | `dotnet publish -c Release -o ./publish` | ⭐ **Phổ biến nhất (Gọn nhẹ ~5MB):** Yêu cầu trên máy chủ Server (hoặc Docker image) đã cài sẵn bộ chạy `.NET Runtime`. |
                | **2. Đóng gói độc lập (`Self-Contained`)** | `dotnet publish -c Release -r linux-x64 --self-contained true` | **Nặng (~70MB):** Gói kèm cả bộ máy `.NET` vào chung thư mục. Cắm vào bất kỳ máy chủ trắng nào cũng chạy được ngay mà không cần cài .NET! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm tra và biên dịch ở chế độ tối ưu (Release Mode)",
                        "📥 Đầu vào: Source code C# của dự án. ⚙️ Cơ chế ngầm: Cờ -c Release bảo trình biên dịch loại bỏ các ký hiệu Debug dư thừa và tối ưu hóa mã máy (JIT Optimization) giúp chạy nhanh hơn 30%. 📤 Đầu ra: Bản build sẵn sàng xuất xưởng.",
                        "dotnet build --configuration Release"),
                    Step(2,
                        "2. Đóng gói toàn bộ file chạy ra thư mục ./publish",
                        "📥 Đầu vào: Lệnh dotnet publish. ⚙️ Cơ chế ngầm: Gom toàn bộ các file .dll đã biên dịch, thư mục giao diện tĩnh wwwroot và file appsettings.json vào một thư mục duy nhất tên là 'publish'. 📤 Đầu ra: Thư mục ./publish gọn gàng để đưa lên Server.",
                        "dotnet publish -c Release -o ./publish"),
                    Step(3,
                        "3. Chạy thử bản Production trực tiếp bằng Kestrel CLI",
                        "📥 Đầu vào: File HoangViet.dll nằm trong thư mục ./publish. ⚙️ Cơ chế ngầm: Dùng lệnh 'dotnet HoangViet.dll' để khởi chạy máy chủ Kestrel lắng nghe ở cổng 5000 như trên Server thật. 📤 Đầu ra: Ứng dụng chạy độc lập không cần mở Visual Studio!",
                        "cd ./publish\ndotnet HoangViet.dll --urls \"http://0.0.0.0:5000\"")
                },
                "Khi đóng gói ứng dụng ASP.NET Core để đưa lên máy chủ thật (Production) bằng câu lệnh dotnet publish -c Release, sự khác biệt chính giữa chế độ Release và chế độ Debug (lúc bấm F5 trên máy cá nhân) là gì?",
                new List<string>
                {
                    "A. Chế độ Release lược bỏ các thông tin theo dõi dò lỗi (Debug symbols) dư thừa và bật tính năng tối ưu hóa mã thực thi của trình biên dịch, giúp ứng dụng trên Server chạy nhanh hơn và tốn ít bộ nhớ RAM hơn",
                    "B. Chế độ Release tự động xóa hết các bảng trong SQL Server",
                    "C. Chế độ Debug không cho phép kết nối Database",
                    "D. Hai chế độ hoàn toàn giống hệt nhau"
                },
                "A",
                "Trên máy cá nhân ta dùng Debug để đặt Breakpoint (F9) từng dòng; còn khi xuất bản lên Server hoặc đóng gói vào Docker, bắt buộc phải dùng -c Release để đạt hiệu năng xử lý cao nhất."
            ),

            // =====================================================================
            // BÀI 13.2
            // =====================================================================
            Build(
                topicId,
                "13.2",
                "Quản lý Cấu hình Đa Môi trường (Development, Staging, Production) & Ghi đè bằng Biến môi trường (Environment Variables)",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/fundamentals/environments",
                """
                • **💡 1. Hiểu bản chất của 3 Môi trường (`Environments`) trong dự án thực tế:**
                  Trong mọi công ty phần mềm, một ứng dụng luôn đi qua **3 môi trường tách biệt hoàn toàn**:
                  1. **`Development` (Máy cá nhân của lập trình viên):** Dùng Database `LocalDB` trên máy, bật trang `/swagger`, hiện chi tiết lỗi để dễ sửa.
                  2. **`Staging` (Máy chủ thử nghiệm nội bộ):** Giống hệt máy chủ thật để đội Tester vào kiểm thử trước khi giao cho khách hàng.
                  3. **`Production` (Máy chủ thật phục vụ khách hàng):** Tắt `/swagger`, ẩn chi tiết lỗi hệ thống (`500`), nối vào Database thật có bảo mật cao!
                • **🖼️ 2. Sơ đồ Thứ Tự Ưu Tiên Ghi Đè Cấu Hình Trong ASP.NET Core (Cái sau đè lên cái trước!):**
                ```
                [Thấp nhất] 1. appsettings.json (Cấu hình gốc mặc định)
                                   │
                                   ▼ (Ghi đè bởi)
                            2. appsettings.{ASPNETCORE_ENVIRONMENT}.json (VD: appsettings.Production.json)
                                   │
                                   ▼ (Ghi đè bởi)
                            3. User Secrets (Chỉ có tác dụng ở máy Development cá nhân)
                                   │
                                   ▼ (Ghi đè bởi)
                [CAO NHẤT!] 4. Environment Variables (Biến môi trường trên Server / Docker / Cloud!)
                ```
                • **📊 3. Quy tắc "Dấu gạch dưới kép (`__`)" Khi Đặt Tên Biến Môi Trường Cho Linux / Docker:**
                  - Trong file `appsettings.json`, cấu trúc phân cấp viết bằng dấu hai chấm `:`:
                    `"ConnectionStrings:DefaultConnection"` hoặc `"JwtSettings:SecretKey"`.
                  - Nhưng trên hệ điều hành Linux / Docker, tên biến môi trường không cho dùng dấu `:`. Vì vậy ASP.NET Core quy định: **Thay dấu `:` bằng 2 dấu gạch dưới liên tiếp `__`**!
                    + Ví dụ: **`ConnectionStrings__DefaultConnection`** hoặc **`JwtSettings__SecretKey`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm tra môi trường đang chạy thông qua biến app.Environment",
                        "📥 Đầu vào: Biến WebApplication app trong Program.cs. ⚙️ Cơ chế ngầm: Đọc biến môi trường ASPNETCORE_ENVIRONMENT của hệ điều hành để biết đang chạy ở Development hay Production. 📤 Đầu ra: Bật/tắt các tính năng nhạy cảm tự động.",
                        "if (app.Environment.IsDevelopment()) {\n    app.UseSwagger();\n    app.UseSwaggerUI();\n} else {\n    // Ở Production: Tắt Swagger, bật bảo mật HTTPS nghiêm ngặt (HSTS)!\n    app.UseHsts();\n}"),
                    Step(2,
                        "2. Tạo file cấu hình riêng cho từng môi trường",
                        "📥 Đầu vào: File appsettings.json và appsettings.Development.json. ⚙️ Cơ chế ngầm: Những khóa nào có trong appsettings.Development.json sẽ tự động ghi đè lên khóa cùng tên trong appsettings.json khi bạn chạy trên máy cá nhân. 📤 Đầu ra: Tách biệt cấu hình Local và Server.",
                        "// Trong appsettings.Development.json (Máy cá nhân dùng LocalDB):\n{\n  \"ConnectionStrings\": {\n    \"DefaultConnection\": \"Server=(localdb)\\\\mssqllocaldb;Database=HoangViet_DevDb;Trusted_Connection=True\"\n  }\n}"),
                    Step(3,
                        "3. Truyền mật khẩu DB thật qua Biến môi trường (Dùng dấu __)",
                        "📥 Đầu vào: Triển khai lên Docker hoặc Cloud Server (Production). ⚙️ Cơ chế ngầm: Truyền biến môi trường ConnectionStrings__DefaultConnection từ bên ngoài vào. Hệ thống tự động ghi đè lên giá trị trong appsettings.json mà không cần sửa file code! 📤 Đầu ra: An toàn bảo mật tuyệt đối.",
                        "// Thiết lập biến môi trường trên Server / Docker:\nASPNETCORE_ENVIRONMENT=Production\nConnectionStrings__DefaultConnection=\"Server=sql.hoangviet.vn;Database=ProdDb;User Id=sa;Password=MatKhauSieuManh!;\"\nJwtSettings__SecretKey=\"KhoaBiMatTrenServerProduction2026!\"")
                },
                "Khi triển khai ứng dụng ASP.NET Core lên máy chủ Linux hoặc Docker Container, để ghi đè cấu hình \"ConnectionStrings\": { \"DefaultConnection\": \"...\" } nằm trong appsettings.json thông qua Biến môi trường (Environment Variable), bạn phải đặt tên biến môi trường đó như thế nào?",
                new List<string>
                {
                    "A. ConnectionStrings__DefaultConnection (Sử dụng 2 dấu gạch dưới liên tiếp '__' để đại diện cho phân cấp dấu hai chấm ':')",
                    "B. ConnectionStrings/DefaultConnection",
                    "C. DefaultConnection",
                    "D. ConnectionStrings-DefaultConnection"
                },
                "A",
                "Ký hiệu hai dấu gạch dưới (__) được ASP.NET Core hỗ trợ trên mọi nền tảng (Windows, Linux, macOS, Docker, Kubernetes, Azure) để ánh xạ chính xác vào các nhánh JSON lồng nhau trong IConfiguration."
            ),

            // =====================================================================
            // BÀI 13.3
            // =====================================================================
            Build(
                topicId,
                "13.3",
                "Bản chất của Docker & Containerization: Giải quyết triệt để câu nói \"Sao chạy trên máy em ngon mà lên Server lại lỗi?\"",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/microservices/container-docker-introduction/",
                """
                • **💡 1. Hiểu bản chất của `Docker` trong 30 giây:**
                  - Tình huống kinh điển khi đi làm hoặc đi chấm đồ án: Trên laptop của bạn cài `.NET 10.0.1`, `SQL Server 2022`, múi giờ Việt Nam, chạy ngon lành. Nhưng khi chép sang máy thầy giáo (hoặc máy chủ công ty) đang cài `.NET 8.0`, thiếu thư viện font chữ ➔ **Bấm chạy báo lỗi sập ngay lập tức!**
                  - **Docker** giải quyết vĩnh viễn nỗi đau này bằng công nghệ **Container (Thùng hàng tiêu chuẩn hóa)**: Docker đóng gói **Code của bạn + Đúng phiên bản .NET Runtime + Toàn bộ thư viện phụ thuộc + Cấu hình hệ điều hành** vào chung một cái thùng kín gọi là **`Docker Image`**. Đem cái thùng này đặt lên máy của bạn, máy của đồng đội hay máy chủ đám mây AWS/Azure đều **chạy giống hệt nhau 100% không sai một li**!
                • **🖼️ 2. Sơ đồ Phân Biệt 3 Khái Niệm Cốt Lõi: `Dockerfile` ➔ `Docker Image` ➔ `Docker Container`:**
                ```
                [1. Dockerfile]                  [2. Docker Image]                 [3. Docker Container]
                (Tờ công thức hướng dẫn          (Khuôn bánh / Bản đóng gói tĩnh   (Chiếc bánh thật đang chạy sống
                 viết bằng chữ trong dự án)       nằm trên ổ cứng / Docker Hub)     trên RAM, có cổng mạng riêng!)
                      │                                 │                                 │
                      ├─────── docker build ───────────►├────────── docker run ──────────►│
                      │    (Đóng gói thành khuôn)       │     (Khởi chạy thành tiến trình)│
                *(So sánh với OOP C#: Docker Image chính là "Class", còn Docker Container chính là "Object"!)*
                ```
                • **📊 3. Tại sao `Docker Container` lại nhẹ và khởi động nhanh gấp 20 lần `Máy ảo (Virtual Machine - VM)`?**
                | Tiêu chí | Máy ảo truyền thống (`VMware / VirtualBox`) | `Docker Container` (Chuẩn hiện đại) |
                | :--- | :--- | :--- |
                | **Cấu trúc bên trong** | Phải cài nguyên một hệ điều hành Windows/Linux nặng `10GB - 20GB` cho mỗi máy ảo. | Dùng chung nhân (`Kernel`) của máy chủ, chỉ đóng gói đúng thư viện ứng dụng cần (`~100MB`)! |
                | **Thời gian bật lên** | Mất từ `2 đến 5 phút` để khởi động hệ điều hành. | Bật lên chạy ngay lập tức chỉ trong **`1 đến 2 giây`**! |
                | **Tiêu thụ RAM** | Ngốn hàng Gigabyte RAM ngay cả khi chưa chạy web. | Chỉ tốn vài chục Megabyte RAM! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kiểm tra cài đặt Docker và tải Image mẫu về máy",
                        "📥 Đầu vào: Cài đặt Docker Desktop trên máy tính. ⚙️ Cơ chế ngầm: Dùng lệnh docker pull để tải các Image chính thức của Microsoft từ kho lưu trữ đám mây về máy. 📤 Đầu ra: Sẵn sàng môi trường Docker.",
                        "docker --version\ndocker images // Xem danh sách các Image đang có trên máy\ndocker ps     // Xem danh sách các Container đang chạy sống"),
                    Step(2,
                        "2. Đóng gói ứng dụng thành Docker Image (docker build)",
                        "📥 Đầu vào: File Dockerfile ở thư mục gốc dự án. ⚙️ Cơ chế ngầm: Lệnh docker build -t đặt tên nhãn (tag) cho Image là 'hoangviet-web:v1', dấu chấm '.' ở cuối chỉ định thư mục hiện tại. 📤 Đầu ra: Tạo ra Image tĩnh sẵn sàng đem đi bất kỳ đâu.",
                        "docker build -t hoangviet-web:v1 ."),
                    Step(3,
                        "3. Khởi chạy Container và nối cổng mạng (Port Mapping -p)",
                        "📥 Đầu vào: Image hoangviet-web:v1 vừa tạo. ⚙️ Cơ chế ngầm: Cờ -d cho chạy ngầm (Detached), cờ -p 8080:8080 nối cổng 8080 của máy thật vào cổng 8080 bên trong thùng Container. 📤 Đầu ra: Mở trình duyệt gõ http://localhost:8080 là thấy web chạy!",
                        "docker run -d -p 8080:8080 --name my-running-app hoangviet-web:v1\n\n// Xem nhật ký Console của ứng dụng đang chạy bên trong Container:\ndocker logs -f my-running-app")
                },
                "Nếu ví von với các khái niệm trong Lập trình Hướng đối tượng (OOP) của C#, thì mối quan hệ giữa Docker Image và Docker Container tương đương với cặp khái niệm nào?",
                new List<string>
                {
                    "A. Docker Image tương đương với 'Class' (bản khuôn mẫu thiết kế tĩnh chỉ đọc), còn Docker Container tương đương với 'Object / Instance' (thực thể sống đang chạy trên RAM được khởi tạo từ Image đó)",
                    "B. Docker Image là biến int, còn Container là biến string",
                    "C. Docker Image là SQL Server, còn Container là HTML",
                    "D. Hai khái niệm không liên quan gì đến nhau"
                },
                "A",
                "Từ 1 Docker Image duy nhất (giống như 1 Class), bạn có thể dùng lệnh docker run để đẻ ra 1, 5 hay 10 Docker Containers chạy song song độc lập với nhau (giống như tạo nhiều Object)."
            ),

            // =====================================================================
            // BÀI 13.4
            // =====================================================================
            Build(
                topicId,
                "13.4",
                "Nghệ thuật viết Dockerfile Đa tầng (Multi-Stage Build) cho ASP.NET Core & Tối ưu Layer Caching",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/core/docker/build-container",
                """
                • **💡 1. Tại sao phải dùng kỹ thuật `Multi-Stage Build` (Đóng gói Đa tầng) khi viết `Dockerfile` cho .NET?**
                  - Để **biên dịch (`dotnet publish`)** được code C#, bạn cần bộ công cụ lập trình **`mcr.microsoft.com/dotnet/sdk`** rất nặng (khoảng **`800 MB`**).
                  - Nhưng sau khi đã biên dịch ra file `.dll` xong rồi, để **chạy (`dotnet App.dll`)** thì chỉ cần bộ **`mcr.microsoft.com/dotnet/aspnet` (Runtime)** siêu nhẹ (chỉ khoảng **`100 MB`**)!
                  - **Kỹ thuật `Multi-Stage Build` trong 1 file `Dockerfile`:**
                    + **Tầng 1 (`Stage 1: build` - Dùng ảnh `sdk` nặng 800MB):** Vào biên dịch code C# ra thư mục `/app/publish`.
                    + **Tầng 2 (`Stage 2: final` - Dùng ảnh `aspnet` nhẹ 100MB):** Chỉ nhặt đúng thư mục `/app/publish` từ Tầng 1 sang, rồi **vứt bỏ hoàn toàn Tầng 1 nặng 800MB đi**! Kết quả: Image cuối cùng nhẹ đi **8 lần** và cực kỳ bảo mật (vì bên trong không còn chứa source code `.cs` gốc)!
                • **🖼️ 2. Sơ đồ Cơ Chế "Đóng Gói 2 Tầng" (`Multi-Stage Dockerfile`) Chuẩn Microsoft:**
                ```
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🏗️ TẦNG 1 (FROM dotnet/sdk:10.0 AS build - Nặng 850MB):                    │
                │    1. COPY file .csproj vào trước ──► Chạy: dotnet restore (Tải NuGet)     │
                │    2. COPY toàn bộ code .cs vào   ──► Chạy: dotnet publish -c Release      │
                │    ──► Sản phẩm thu được nằm tại: /app/publish (Chỉ nặng 5MB!)             │
                └─────────────────────────────────────┬──────────────────────────────────────┘
                                                      │ Chỉ chép đúng thư mục /app/publish sang!
                                                      │ (Vứt bỏ toàn bộ SDK 850MB và code .cs!)
                                                      ▼
                ┌────────────────────────────────────────────────────────────────────────────┐
                │ 🚀 TẦNG 2 (FROM dotnet/aspnet:10.0 AS final - Siêu nhẹ, an toàn):          │
                │    COPY --from=build /app/publish .                                        │
                │    ENTRYPOINT ["dotnet", "HoangViet.dll"]                                  │
                └────────────────────────────────────────────────────────────────────────────┘
                ```
                • **📊 3. Bí quyết Tối ưu Tốc độ Build gấp 10 lần nhờ `Docker Layer Caching`:**
                  - Tại sao trong `Dockerfile` chuẩn, người ta luôn **`COPY *.csproj` rồi chạy `dotnet restore` TRƯỚC**, sau đó mới `COPY . .` toàn bộ code `.cs` vào?
                  - Vì các gói thư viện NuGet trong `.csproj` rất ít khi thay đổi, còn code `.cs` thì bạn sửa hàng ngày! Tách bước `dotnet restore` lên trước giúp Docker **lưu đệm (Cache) toàn bộ các gói thư viện**: lần sau bạn sửa code `.cs` và bấm `docker build`, nó bỏ qua bước tải mạng NuGet và build xong chỉ trong **3 giây**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo file .dockerignore (Chặn rác không cho chép vào Docker)",
                        "📥 Đầu vào: Thư mục dự án có bin/, obj/, .git/, .vs/. ⚙️ Cơ chế ngầm: Giống như .gitignore, file .dockerignore ngăn không cho lệnh COPY chép các thư mục rác từ máy tính vào trong Docker, giúp tốc độ build nhanh và sạch tuyệt đối. 📤 Đầu ra: File .dockerignore chuẩn.",
                        "bin/\nobj/\n.git/\n.vs/\n*.user\nDockerfile*"),
                    Step(2,
                        "2. Viết Tầng 1 (Stage Build) tận dụng tối đa Layer Cache cho NuGet",
                        "📥 Đầu vào: Ảnh nền mcr.microsoft.com/dotnet/sdk:10.0. ⚙️ Cơ chế ngầm: Copy file .csproj và chạy dotnet restore trước; sau đó mới copy toàn bộ source code và chạy dotnet publish -c Release. 📤 Đầu ra: Thư mục /app/publish đã biên dịch xong.",
                        "# TẦNG 1: BIÊN DỊCH CODE VỚI SDK\nFROM mcr.microsoft.com/dotnet/sdk:10.0 AS build\nWORKDIR /src\n\n# 1. Copy riêng file .csproj trước để Cache lại các gói NuGet:\nCOPY [\"HoangViet.csproj\", \"./\"]\nRUN dotnet restore \"HoangViet.csproj\"\n\n# 2. Copy toàn bộ source code còn lại và Publish:\nCOPY . .\nRUN dotnet publish \"HoangViet.csproj\" -c Release -o /app/publish /p:UseAppHost=false"),
                    Step(3,
                        "3. Viết Tầng 2 (Stage Runtime) siêu nhẹ để chạy ứng dụng",
                        "📥 Đầu vào: Ảnh nền mcr.microsoft.com/dotnet/aspnet:10.0. ⚙️ Cơ chế ngầm: Dùng lệnh COPY --from=build để chỉ nhặt thành phẩm từ Tầng 1 sang, mở cổng 8080 và thiết lập lệnh khởi chạy ENTRYPOINT. 📤 Đầu ra: Docker Image hoàn chỉnh nhẹ và bảo mật.",
                        "# TẦNG 2: CHẠY ỨNG DỤNG VỚI RUNTIME SIÊU NHẸ\nFROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final\nWORKDIR /app\nEXPOSE 8080\n\nCOPY --from=build /app/publish .\nENTRYPOINT [\"dotnet\", \"HoangViet.dll\"]")
                },
                "Trong một file Dockerfile chuẩn cho dự án C# .NET, tại sao người ta luôn thực hiện lệnh COPY [\"Project.csproj\", \"./\"] và RUN dotnet restore TRƯỚC KHI thực hiện lệnh COPY . . (chép toàn bộ các file code .cs)?",
                new List<string>
                {
                    "A. Để tận dụng cơ chế Docker Layer Caching: Khi bạn chỉ sửa code trong các file .cs mà không cài thêm gói NuGet mới vào file .csproj, Docker sẽ dùng lại luôn tầng Cache của bước dotnet restore đã tải trước đó, giúp lần build sau diễn ra trong vài giây thay vì phải tải lại thư viện từ Internet",
                    "B. Vì lệnh COPY . . không chép được file .csproj",
                    "C. Để xóa các gói thư viện NuGet đi",
                    "D. Vì Docker bắt buộc file đầu tiên phải có đuôi dài 7 ký tự"
                },
                "A",
                "Docker lưu bộ nhớ đệm theo từng dòng lệnh (Layer). Nếu bạn gộp COPY . . lên trên cùng, mỗi khi bạn sửa 1 dấu chấm phẩy trong file .cs, toàn bộ các tầng bên dưới đều bị mất Cache và phải chạy lại dotnet restore tải mạng từ đầu!"
            ),

            // =====================================================================
            // BÀI 13.5
            // =====================================================================
            Build(
                topicId,
                "13.5",
                "Điều phối Đa Container với Docker Compose (Chạy đồng bộ cả Web App .NET + SQL Server + Volumes chỉ bằng 1 lệnh!)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/architecture/microservices/multi-container-microservice-net-applications/multi-container-applications-docker-compose",
                """
                • **💡 1. Hiểu bản chất của `Docker Compose` trong 30 giây:**
                  - Một hệ thống thực tế không chỉ có mỗi thùng Container chạy Web C#, mà còn cần **1 thùng Container chạy SQL Server**, **1 thùng chạy Redis Cache**...
                  - Nếu không có `Docker Compose`, bạn phải gõ tay hàng chục lệnh `docker run` dài ngoằng, tự tạo mạng nối các thùng với nhau!
                  - **`Docker Compose` (File `docker-compose.yml`)** là **"Bản nhạc trưởng điều phối toàn bộ hệ thống"**: Bạn khai báo cả Web C# và SQL Server vào chung 1 file `docker-compose.yml`. Khi sang bất kỳ máy tính mới nào, chỉ cần gõ **ĐÚNG 1 LỆNH DUY NHẤT: `docker compose up -d`** ➔ Cả máy chủ SQL Server lẫn trang Web .NET tự động tải về, tự tạo mạng nội bộ nối với nhau và chạy lên hoàn chỉnh trong 30 giây!
                • **🖼️ 2. Sơ đồ Kiến trúc Mạng Nội Bộ & Ổ Cứng Bền Vững (`Volume`) Trong `Docker Compose`:**
                ```
                ╔══════════════════════ MẠNG NỘI BỘ DOCKER (hoangviet-net) ══════════════════════╗
                ║                                                                                ║
                ║  ┌─────────────────────────────┐              ┌─────────────────────────────┐  ║
                ║  │ 🌐 Container 1: web-app     │              │ 🗄️ Container 2: sql-db      │  ║
                ║  │ (ASP.NET Core 10.0)         │ ───────────► │ (SQL Server 2022 Linux)     │  ║
                ║  │ Gọi DB qua tên: "Server=sql-db"            │ Cổng nội bộ: 1433           │  ║
                ║  └──────────────┬──────────────┘              └──────────────┬──────────────┘  ║
                ╚═════════════════╪════════════════════════════════════════════╪═════════════════╝
                                  │ (Mở cổng 8080 ra máy thật)                 │ (Gắn vào Docker Volume)
                                  ▼                                            ▼
                         Trình duyệt: localhost:8080                 💾 [Ổ cứng Volume: sql-data]
                                                                     (Tắt/Xóa Container cũng KHÔNG mất dữ liệu DB!)
                ```
                • **📊 3. Hai Quy Tắc Sống Còn Khi Dùng `Docker Compose` Với SQL Server:**
                  1. **Tên `Server=...` trong chuỗi kết nối (`ConnectionString`):** Khi Web C# nằm trong Container 1 muốn gọi sang SQL Server ở Container 2, tuyệt đối **KHÔNG viết `Server=localhost`**, mà phải viết **`Server=sql-db`** (chính là tên dịch vụ của Container SQL Server khai báo trong file `docker-compose.yml`)!
                  2. **Bắt buộc phải gắn `Volumes` cho SQL Server:** Mặc định nếu xóa Container thì dữ liệu bên trong mất sạch. Gắn `volumes: - sql_data:/var/opt/mssql` giúp toàn bộ bảng và dữ liệu SQL được lưu vĩnh viễn ra ổ cứng bên ngoài!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo dịch vụ SQL Server kèm Volume bền vững trong docker-compose.yml",
                        "📥 Đầu vào: File docker-compose.yml ở thư mục gốc. ⚙️ Cơ chế ngầm: Sử dụng ảnh mcr.microsoft.com/mssql/server:2022-latest, đặt mật khẩu tài khoản 'sa' và gắn volume sql_data để không bao giờ bị mất dữ liệu khi tắt máy. 📤 Đầu ra: Máy chủ SQL Server trong Container.",
                        "services:\n  sql-db:\n    image: mcr.microsoft.com/mssql/server:2022-latest\n    container_name: hoangviet_sql\n    environment:\n      - ACCEPT_EULA=Y\n      - MSSQL_SA_PASSWORD=MatKhauManh@2026!\n    ports:\n      - \"1433:1433\"\n    volumes:\n      - sql_data:/var/opt/mssql"),
                    Step(2,
                        "2. Khai báo dịch vụ Web App .NET nối thẳng vào dịch vụ sql-db",
                        "📥 Đầu vào: Dịch vụ web-app nằm ngay dưới sql-db. ⚙️ Cơ chế ngầm: Dùng depends_on để đợi sql-db bật trước, và truyền biến môi trường ConnectionStrings__DefaultConnection trỏ tới 'Server=sql-db;...'. 📤 Đầu ra: Hai Container tự động nhìn thấy nhau.",
                        "  web-app:\n    build:\n      context: .\n      dockerfile: Dockerfile\n    container_name: hoangviet_web\n    ports:\n      - \"8080:8080\"\n    environment:\n      - ASPNETCORE_ENVIRONMENT=Production\n      - ConnectionStrings__DefaultConnection=Server=sql-db;Database=HoangVietDb;User Id=sa;Password=MatKhauManh@2026!;TrustServerCertificate=True;\n    depends_on:\n      - sql-db\n\nvolumes:\n  sql_data:"),
                    Step(3,
                        "3. Khởi chạy và Tắt toàn bộ hệ thống chỉ bằng 1 câu lệnh",
                        "📥 Đầu vào: File docker-compose.yml hoàn chỉnh. ⚙️ Cơ chế ngầm: Lệnh 'docker compose up -d --build' tự động build code C#, bật SQL Server, nối mạng và mở web ở cổng 8080. 📤 Đầu ra: Hệ thống chạy hoàn hảo.",
                        "// Bật toàn bộ hệ thống chạy ngầm:\ndocker compose up -d --build\n\n// Khi muốn tắt toàn bộ các Container (Dữ liệu trong Volume vẫn còn nguyên!):\ndocker compose down")
                },
                "Trong file docker-compose.yml gồm 2 dịch vụ là web-app (chạy ASP.NET Core) và sql-db (chạy SQL Server), bên trong chuỗi kết nối ConnectionString của web-app, tham số Server=... phải được điền là gì để web-app kết nối được tới sql-db?",
                new List<string>
                {
                    "A. Điền chính tên của dịch vụ cơ sở dữ liệu trong docker-compose là 'Server=sql-db' (vì Docker Compose tự động phân giải tên dịch vụ thành địa chỉ IP nội bộ giữa các Container)",
                    "B. Điền là 'Server=(localdb)\\mssqllocaldb'",
                    "C. Điền là 'Server=127.0.0.1'",
                    "D. Không cần điền tham số Server"
                },
                "A",
                "Bên trong mạng ảo của Docker, mỗi Container là một máy tính độc lập nên localhost chỉ trỏ vào chính bản thân Container đó. Docker Compose tích hợp sẵn DNS nội bộ giúp các Container gọi nhau trực tiếp thông qua tên service (Server=sql-db)."
            ),

            // =====================================================================
            // BÀI 13.6
            // =====================================================================
            Build(
                topicId,
                "13.6",
                "Bản chất của CI/CD (Continuous Integration & Continuous Deployment) - Tự động hóa Quy trình Kiểm thử & Triển khai",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/deliver/what-is-continuous-delivery",
                """
                • **💡 1. Hiểu bản chất của `CI / CD` trong 30 giây:**
                  - **Khi chưa có CI/CD (Làm thủ công):** Mỗi lần thành viên trong nhóm đẩy code mới lên GitHub, không ai biết code đó có làm lỗi tính năng cũ không. Đến ngày đưa lên Server, nhóm trưởng phải thức đêm tự gõ lệnh `dotnet test`, tự `dotnet publish`, tự copy file qua FTP lên Server, tự tắt web cũ bật web mới — rất dễ thao tác nhầm làm sập web!
                  - **Khi có `CI / CD Pipeline` (Dây chuyền robot tự động hóa 100%):**
                    1. **`CI` (Continuous Integration - Tích hợp liên tục):** Hễ bất kỳ ai vừa gõ `git push` hoặc tạo `Pull Request` lên GitHub, một con **Robot ảo trên đám mây GitHub** lập tức tự động tải code về, tự chạy `dotnet build` và tự chạy toàn bộ 200 bài `Unit Test (dotnet test)`. Nếu có 1 bài test bị sai ➔ Robot báo dấu ❌ màu đỏ và **khóa luôn nút Merge**, không cho code lỗi lọt vào nhánh chính!
                    2. **`CD` (Continuous Delivery / Deployment - Triển khai liên tục):** Ngay khi nhánh `main` vượt qua hết các bài kiểm tra của bước `CI` (hiện tích xanh ✅), Robot tự động đóng gói `Docker Image` mới và **tự động cập nhật thẳng lên Server Production** trong 1 phút mà con người không cần đụng tay!
                • **🖼️ 2. Sơ đồ Đường Ống Tự Động Hóa `CI / CD Pipeline` Chuẩn Công Nghiệp:**
                ```
                Lập trình viên gõ: git push / Tạo Pull Request
                      │
                      ▼
                ╔══════════════════════ GIAI ĐOẠN 1: CI (Continuous Integration) ══════════════════════╗
                ║  Robot tự động chạy:                                                                 ║
                ║  1. Checkout Code ──► 2. dotnet restore ──► 3. dotnet build ──► 4. dotnet test       ║
                ║  *(Nếu lỗi biên dịch hoặc Fail 1 bài Unit Test ──► Dừng ngay & Báo Đỏ ❌ cho tác giả!)*║
                ╚══════════════════════════════════════════╤═══════════════════════════════════════════╝
                                                           │ (Tất cả Pass 100% ✅ trên nhánh main)
                                                           ▼
                ╔══════════════════════ GIAI ĐOẠN 2: CD (Continuous Deployment) ═══════════════════════╗
                ║  5. Đóng gói Docker Image / Publish ──► 6. Đẩy lên Server Cloud ──► 7. Khởi động mới!║
                ╚══════════════════════════════════════════════════════════════════════════════════════╝
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Sự kiện kích hoạt đường ống (Trigger Event)",
                        "📥 Đầu vào: Lập trình viên đẩy nhánh mới lên hoặc mở Pull Request vào nhánh develop/main. ⚙️ Cơ chế ngầm: GitHub phát hiện sự kiện push/pull_request và cấp phát ngay một máy ảo Ubuntu mới tinh (Runner) để bắt đầu kiểm tra. 📤 Đầu ra: Khởi động CI Pipeline.",
                        "// Sự kiện kích hoạt tự động:\n// on: [push, pull_request] -> branches: [ main, develop ]"),
                    Step(2,
                        "2. Chốt chặn chất lượng tự động (CI Quality Gate)",
                        "📥 Đầu vào: Mã nguồn trên máy ảo Runner. ⚙️ Cơ chế ngầm: Chạy lần lượt dotnet build --configuration Release và dotnet test. Nếu pass 100% mới hiện dấu tích xanh trên Pull Request. 📤 Đầu ra: Đảm bảo nhánh chính không bao giờ bị hỏng.",
                        "dotnet restore\ndotnet build --configuration Release --no-restore\ndotnet test --no-build --verbosity normal"),
                    Step(3,
                        "3. Tự động xuất bản lên môi trường đích (CD Release)",
                        "📥 Đầu vào: Code đã vượt qua vòng kiểm tra CI và được gộp vào nhánh main. ⚙️ Cơ chế ngầm: Tự động chạy dotnet publish hoặc docker build/push và cập nhật ứng dụng đang chạy trên máy chủ Cloud. 📤 Đầu ra: Khách hàng thấy tính năng mới ngay lập tức.",
                        "docker build -t myregistry/hoangviet-app:latest .\ndocker push myregistry/hoangviet-app:latest")
                },
                "Trong quy trình CI/CD, vai trò cốt lõi nhất của giai đoạn CI (Continuous Integration - Tích hợp liên tục) mỗi khi có thành viên tạo Pull Request vào nhánh chung của nhóm là gì?",
                new List<string>
                {
                    "A. Tự động biên dịch (dotnet build) và chạy toàn bộ bộ kiểm thử tự động (dotnet test) trên máy chủ trung gian để phát hiện sớm lỗi biên dịch hoặc lỗi logic trước khi cho phép gộp code vào nhánh chính",
                    "B. Tự động viết thêm tính năng mới thay cho lập trình viên",
                    "C. Tự động xóa tài khoản GitHub của người gửi code",
                    "D. Dùng để thiết kế logo cho công ty"
                },
                "A",
                "CI đóng vai trò là \"Người gác cổng chất lượng tự động\": đảm bảo không một dòng code lỗi biên dịch hay làm hỏng tính năng cũ nào có thể lọt vào nhánh develop hoặc main của dự án."
            ),

            // =====================================================================
            // BÀI 13.7
            // =====================================================================
            Build(
                topicId,
                "13.7",
                "Thiết lập GitHub Actions Workflow (.github/workflows/ci-cd.yml) tự động Build, Test & Push Docker cho .NET",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/devops/github-actions-overview",
                """
                • **💡 1. Làm sao để bật Robot `CI/CD` hoàn toàn miễn phí ngay trên kho GitHub của bạn (`GitHub Actions`)?**
                  - GitHub tặng sẵn cho mọi tài khoản tính năng **GitHub Actions** (miễn phí 2.000 phút chạy máy ảo mỗi tháng).
                  - Bạn không cần cài đặt phần mềm gì cả! Chỉ cần tạo một file văn bản có đường dẫn: **`.github/workflows/dotnet-ci.yml`** ngay trong thư mục dự án của bạn và đẩy lên GitHub. Ngay lập tức, tab **`Actions`** trên web GitHub của bạn sẽ sáng lên và tự động chạy kiểm tra mỗi khi bạn `push` code!
                • **🖼️ 2. Giải phẫu Cấu trúc File `.github/workflows/dotnet-ci.yml`:**
                ```yaml
                name: .NET CI/CD Pipeline          # 1. Tên hiển thị của quy trình trên tab Actions của GitHub

                on:                                # 2. Khi nào thì Robot tự động chạy?
                  push:
                    branches: [ "main", "develop" ]
                  pull_request:
                    branches: [ "main", "develop" ]

                jobs:                              # 3. Danh sách các công việc cần làm
                  build-and-test:
                    runs-on: ubuntu-latest         # Mượn 1 máy chủ ảo Linux Ubuntu mới nhất của GitHub
                    steps:
                      - uses: actions/checkout@v4  # Bước 1: Kéo code của bạn về máy ảo
                      - name: Setup .NET 10        # Bước 2: Cài đặt .NET SDK lên máy ảo
                        uses: actions/setup-dotnet@v4
                        with:
                          dotnet-version: '10.0.x'
                      - name: Build & Test         # Bước 3: Biên dịch và chạy Unit Test!
                        run: |
                          dotnet restore
                          dotnet build --configuration Release --no-restore
                          dotnet test --no-build --verbosity normal
                ```
                • **📊 3. Cách lưu Mật khẩu Server / Docker Hub an toàn trong `GitHub Actions Secrets`:**
                  - Khi bước `CD` cần đăng nhập vào Docker Hub hoặc máy chủ Cloud để đẩy web lên, bạn vào mục **`Settings -> Secrets and variables -> Actions`** trên kho GitHub để cất mật khẩu, và gọi trong file `.yml` bằng cú pháp: **`${{ secrets.DOCKER_PASSWORD }}`**!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo thư mục .github/workflows và cấu hình sự kiện kích hoạt",
                        "📥 Đầu vào: Dự án C# đang quản lý bằng Git. ⚙️ Cơ chế ngầm: Tạo file .github/workflows/ci-cd.yml quy định tự động kích hoạt khi có lệnh push hoặc pull_request vào nhánh main và develop. 📤 Đầu ra: Khung kịch bản tự động hóa.",
                        "name: HoangViet .NET CI/CD\n\non:\n  push:\n    branches: [ \"main\", \"develop\" ]\n  pull_request:\n    branches: [ \"main\", \"develop\" ]"),
                    Step(2,
                        "2. Cấu hình Job 1 (CI): Tự động cài .NET, Build và chạy xUnit Test",
                        "📥 Đầu vào: Máy ảo ubuntu-latest của GitHub. ⚙️ Cơ chế ngầm: Tải code về, cài .NET, khôi phục gói NuGet, biên dịch Release và chạy toàn bộ bài kiểm thử dotnet test. 📤 Đầu ra: Hiện tích xanh ✅ ngay bên cạnh Commit trên GitHub!",
                        "jobs:\n  ci-build-test:\n    runs-on: ubuntu-latest\n    steps:\n      - name: 1. Tải mã nguồn về máy ảo\n        uses: actions/checkout@v4\n\n      - name: 2. Cài đặt .NET SDK\n        uses: actions/setup-dotnet@v4\n        with:\n          dotnet-version: '10.0.x'\n\n      - name: 3. Restore, Build & Run Unit Tests\n        run: |\n          dotnet restore\n          dotnet build --configuration Release --no-restore\n          dotnet test --configuration Release --no-build"),
                    Step(3,
                        "3. Cấu hình Job 2 (CD): Tự động đóng gói và đẩy Docker Image lên Docker Hub",
                        "📥 Đầu vào: Job 1 (ci-build-test) đã thành công (needs: ci-build-test) trên nhánh main. ⚙️ Cơ chế ngầm: Đăng nhập vào Docker Hub bằng GitHub Secrets, chạy docker build và docker push tự động. 📤 Đầu ra: Image mới nhất sẵn sàng trên đám mây.",
                        "  cd-docker-push:\n    needs: ci-build-test\n    if: github.ref == 'refs/heads/main'\n    runs-on: ubuntu-latest\n    steps:\n      - uses: actions/checkout@v4\n      - name: Đăng nhập Docker Hub & Đẩy Image\n        run: |\n          echo \"${{ secrets.DOCKER_PASSWORD }}\" | docker login -u \"${{ secrets.DOCKER_USERNAME }}\" --password-stdin\n          docker build -t ${{ secrets.DOCKER_USERNAME }}/hoangviet-app:latest .\n          docker push ${{ secrets.DOCKER_USERNAME }}/hoangviet-app:latest")
                },
                "Trong file cấu hình .github/workflows/ci-cd.yml của GitHub Actions, để Job thứ hai là cd-deploy (Triển khai lên Server) chỉ được phép chạy SAU KHI Job thứ nhất là ci-build-test (Biên dịch và chạy Unit Test) đã hoàn thành thành công 100%, bạn phải khai báo từ khóa nào trong cd-deploy?",
                new List<string>
                {
                    "A. Khai báo thuộc tính 'needs: ci-build-test' ở bên trong job cd-deploy",
                    "B. Khai báo từ khóa 'delete: ci-build-test'",
                    "C. Không thể nối 2 job với nhau",
                    "D. Phải tách ra làm 2 kho GitHub khác nhau"
                },
                "A",
                "Mặc định các jobs trong GitHub Actions sẽ chạy song song cùng một lúc. Từ khóa needs: <tên_job_trước> thiết lập sự phụ thuộc tuần tự: nếu bước ci-build-test bị lỗi, bước cd-deploy sẽ tự động bị hủy bỏ ngay lập tức để bảo vệ Server thật."
            ),

            // =====================================================================
            // BÀI 13.8
            // =====================================================================
            Build(
                topicId,
                "13.8",
                "Chiến lược Áp dụng Database Migrations An toàn trên Môi trường Production (Auto-Migration vs Idempotent SQL Scripts)",
                "SQL & EF Core",
                "https://learn.microsoft.com/vi-vn/ef/core/managing-schemas/migrations/applying",
                """
                • **💡 1. Bài toán thực tế khi đưa ứng dụng lên Server: "Làm sao để cập nhật bảng SQL Server trên máy chủ thật?"**
                  - Trên máy cá nhân, bạn gõ `dotnet ef database update` trên Terminal. Nhưng khi ứng dụng chạy tự động trên Docker hoặc Cloud Server, bạn không thể ngồi gõ tay được!
                  - Có **2 chiến lược cập nhật Database trên Server** mà bạn cần biết rõ khi nào dùng cách nào:
                    1. **Cách 1 - Tự động chạy `MigrateAsync()` lúc khởi động `Program.cs` (Dùng cho Đồ án, Dự án vừa và nhỏ, Docker Compose):**
                       - Ngay trước dòng `app.Run()`, viết 4 dòng code tự động kiểm tra và chạy các bản Migration còn thiếu kèm nạp dữ liệu mẫu (`DbInitializer.SeedAsync`)!
                    2. **Cách 2 - Xuất file SQL Lũy đẳng (`Idempotent SQL Script`) chạy trong bước CI/CD (Chuẩn Doanh nghiệp lớn / Ngân hàng):**
                       - Dùng lệnh **`dotnet ef migrations script --idempotent -o migrate.sql`**. Lệnh này sinh ra một file `.sql` thông minh có kiểm tra `IF NOT EXISTS (SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = ...)` cho từng bản Migration — chạy 1 lần hay 10 lần cũng an toàn tuyệt đối!
                • **🖼️ 2. Quy tắc "Zero-Downtime Migration" (Nâng cấp Database không làm gián đoạn người dùng đang truy cập):**
                ```
                ⚠️ Cẩn thận khi xóa hoặc đổi tên cột trên Server đang có khách hàng sử dụng:
                - Nếu bạn xóa ngay cột OldPhone trong DB trong khi Container Web cũ vẫn đang chạy thêm 30 giây nữa:
                  ──► Container cũ gọi SELECT OldPhone sẽ bị nổ lỗi 500 hàng loạt!
                - Quy trình chuẩn (Expand & Contract):
                  Bước 1: Thêm cột mới NewPhone (Cho phép NULL) ──► Bước 2: Cập nhật code Web mới ──► Bước 3: Xóa cột cũ sau!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tự động chạy MigrateAsync() và Seed Data khi bật ứng dụng (Dành cho Docker / Đồ án)",
                        "📥 Đầu vào: Ứng dụng vừa khởi động lên trong Container mới. ⚙️ Cơ chế ngầm: Tạo một Scope tạm thời trong Program.cs, lấy AppDbContext ra gọi await db.Database.MigrateAsync() để tự động tạo Database và các bảng nếu chưa có, sau đó nạp dữ liệu bài học ban đầu! 📤 Đầu ra: Bật Docker Compose lên là có sẵn đầy đủ bảng và dữ liệu ngay lập tức.",
                        "using (var scope = app.Services.CreateScope()) {\n    var services = scope.ServiceProvider;\n    try {\n        var context = services.GetRequiredService<AppDbContext>();\n        await context.Database.MigrateAsync(); // Tự động cập nhật bảng mới nhất!\n        await DbInitializer.SeedAsync(context); // Nạp dữ liệu khởi tạo nếu bảng trống\n    } catch (Exception ex) {\n        var logger = services.GetRequiredService<ILogger<Program>>();\n        logger.LogError(ex, \"Lỗi khi tự động cập nhật Database lúc khởi động!\");\n    }\n}"),
                    Step(2,
                        "2. Xuất file kịch bản SQL an toàn tuyệt đối bằng cờ --idempotent",
                        "📥 Đầu vào: Thư mục Migrations của dự án. ⚙️ Cơ chế ngầm: Cờ --idempotent giúp EF Core bọc mỗi bản Migration trong một câu lệnh IF NOT EXISTS kiểm tra bảng __EFMigrationsHistory. 📤 Đầu ra: File migrate.sql có thể chạy trên bất kỳ Database nào mà không bao giờ báo lỗi 'Bảng đã tồn tại'!",
                        "dotnet ef migrations script --idempotent -o ./publish/migrate.sql"),
                    Step(3,
                        "3. Giải phẫu nội dung bên trong file SQL Idempotent sinh ra",
                        "📥 Đầu vào: Mở file migrate.sql vừa xuất ở Bước 2. ⚙️ Cơ chế ngầm: Quan sát cách EF Core tự động kiểm tra lịch sử trước khi chạy lệnh CREATE TABLE hoặc ALTER TABLE. 📤 Đầu ra: Hiểu tận gốc cách vận hành của EF Core trên Production.",
                        "IF NOT EXISTS (\n    SELECT * FROM [__EFMigrationsHistory]\n    WHERE [MigrationId] = N'2026102601_InitialCreate'\n)\nBEGIN\n    CREATE TABLE [Books] (...);\n    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])\n    VALUES (N'2026102601_InitialCreate', N'10.0.0');\nEND;\nGO")
                },
                "Khi sử dụng câu lệnh dotnet ef migrations script --idempotent -o deploy.sql để xuất kịch bản cập nhật cơ sở dữ liệu cho môi trường Production, tùy chọn --idempotent mang lại tác dụng quan trọng gì?",
                new List<string>
                {
                    "A. Tự động thêm các câu lệnh kiểm tra IF NOT EXISTS đối chiếu với bảng __EFMigrationsHistory trước mỗi Migration, đảm bảo chỉ những Migration nào chưa từng chạy trên Server mới được thực thi và có thể chạy lại file SQL đó nhiều lần mà không gây lỗi trùng bảng",
                    "B. Xóa toàn bộ dữ liệu cũ trong bảng trước khi cập nhật",
                    "C. Mã hóa file SQL thành file ảnh",
                    "D. Chỉ tạo bảng tạm trên RAM"
                },
                "A",
                "Một kịch bản SQL thông thường nếu chạy lần thứ 2 trên cùng một Database sẽ nổ lỗi There is already an object named 'Books' in the database. Cờ --idempotent giải quyết triệt để bằng cách kiểm tra bảng lịch sử trước từng bước nâng cấp."
            ),

            // =====================================================================
            // BÀI 13.9
            // =====================================================================
            Build(
                topicId,
                "13.9",
                "Triển khai Thực tế lên Cloud (Azure App Service / Render / VPS Linux) & Cấu hình Nginx Reverse Proxy + HTTPS",
                "Web API",
                "https://learn.microsoft.com/vi-vn/aspnet/core/host-and-deploy/linux-nginx",
                """
                • **💡 1. Các lựa chọn đưa Web .NET của bạn lên Internet (Có tên miền thật `https://...` để gắn vào CV xin thực tập!):**
                  Khi làm xong dự án cá nhân hoặc đồ án, nếu trong CV của bạn có một đường link sống thực tế để nhà tuyển dụng bấm vào trải nghiệm ngay trên điện thoại/máy tính của họ, tỷ lệ đỗ phỏng vấn của bạn sẽ tăng gấp 3 lần! Có 2 hướng triển khai phổ biến nhất:
                  1. **Hướng 1 - `PaaS` (`Azure App Service`, `Render.com`, `Railway`, `MonsterASP.NET`):** Bạn chỉ cần nối tài khoản GitHub hoặc đẩy Docker Image lên, nền tảng Cloud sẽ tự động lo hết phần máy chủ và cấp sẵn tên miền `https://hoangviet.onrender.com` hoàn toàn miễn phí!
                  2. **Hướng 2 - `VPS Linux Ubuntu` + `Docker Compose` + `Nginx` (Chuẩn kỹ sư thực thụ):** Thuê một máy chủ ảo Linux, chạy `docker compose up -d` và cấu hình **Nginx** làm cổng đón khách bên ngoài kèm chứng chỉ bảo mật **SSL Let's Encrypt (`HTTPS` ổ khóa xanh)**!
                • **🖼️ 2. Lưu ý Bắt Buộc Trong `Program.cs` Khi Đứng Sau Nginx / Cloud Load Balancer (`UseForwardedHeaders`):**
                ```
                Khi chạy trên Cloud hoặc đứng sau Nginx:
                Khách hàng gọi bằng giao thức HTTPS ──► Đi tới cổng Nginx (hoặc Cloud Proxy)
                                                              │
                                                              ▼ (Nginx chuyển tiếp vào Kestrel bằng HTTP thường cổng 8080!)
                ⚠️ Nếu không cấu hình UseForwardedHeaders():
                   Ứng dụng C# của bạn sẽ tưởng nhầm người dùng đang gọi bằng HTTP không bảo mật (làm hỏng OAuth2 Google Login
                   và không biết được địa chỉ IP thật của khách hàng!).
                ✅ Giải pháp: Thêm app.UseForwardedHeaders() ở đầu Program.cs để đọc Header X-Forwarded-For và X-Forwarded-Proto!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cấu hình ForwardedHeadersMiddleware trong Program.cs (Chuẩn Cloud/Nginx)",
                        "📥 Đầu vào: Ứng dụng chuẩn bị đưa lên môi trường Cloud hoặc chạy sau Nginx Reverse Proxy. ⚙️ Cơ chế ngầm: Cấu hình ForwardedHeadersOptions nhận diện đúng IP thật (XForwardedFor) và giao thức gốc HTTPS (XForwardedProto) của người dùng. 📤 Đầu ra: Ứng dụng hoạt động chính xác 100% sau Proxy.",
                        "using Microsoft.AspNetCore.HttpOverrides;\n\nbuilder.Services.Configure<ForwardedHeadersOptions>(options => {\n    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;\n});\n// ...\nvar app = builder.Build();\napp.UseForwardedHeaders(); // Đặt ở ngay đầu Pipeline!"),
                    Step(2,
                        "2. Cấu hình file Nginx Reverse Proxy chuyển tiếp vào cổng 8080 của .NET",
                        "📥 Đầu vào: Tên miền hoangviet.vn trỏ về máy chủ VPS Linux. ⚙️ Cơ chế ngầm: Nginx lắng nghe ở cổng 80/443 ngoài Internet và dùng lệnh proxy_pass chuyển toàn bộ lưu lượng vào Container .NET đang chạy ở http://localhost:8080. 📤 Đầu ra: Web chạy mượt mà qua tên miền chính thức.",
                        "server {\n    listen 80;\n    server_name hoangviet.vn;\n\n    location / {\n        proxy_pass         http://localhost:8080;\n        proxy_http_version 1.1;\n        proxy_set_header   Upgrade $http_upgrade;\n        proxy_set_header   Connection keep-alive;\n        proxy_set_header   Host $host;\n        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;\n        proxy_set_header   X-Forwarded-Proto $scheme;\n    }\n}"),
                    Step(3,
                        "3. Bật chứng chỉ HTTPS miễn phí tự động bằng Certbot (Let's Encrypt)",
                        "📥 Đầu vào: Máy chủ đã cấu hình Nginx ở Bước 2. ⚙️ Cơ chế ngầm: Chạy đúng 1 lệnh certbot --nginx để tự động xin cấp phát chứng chỉ SSL và bật khóa xanh HTTPS tự động gia hạn. 📤 Đầu ra: Trang web đạt chuẩn bảo mật HTTPS quốc tế.",
                        "sudo apt install certbot python3-certbot-nginx -y\nsudo certbot --nginx -d hoangviet.vn")
                },
                "Khi triển khai ứng dụng ASP.NET Core chạy bên trong Docker Container và đặt phía sau một máy chủ trung gian (Reverse Proxy như Nginx hoặc Cloudflare / Azure Load Balancer), tại sao bạn cần gọi app.UseForwardedHeaders() trong Program.cs?",
                new List<string>
                {
                    "A. Để ứng dụng ASP.NET Core đọc được các Header 'X-Forwarded-For' và 'X-Forwarded-Proto' do Nginx gửi sang, từ đó nhận biết đúng địa chỉ IP thật của khách hàng và biết rằng kết nối gốc bên ngoài đang dùng HTTPS",
                    "B. Để tự động dịch trang web sang 10 thứ tiếng",
                    "C. Để tắt máy chủ Nginx đi",
                    "D. Để không cần dùng file Program.cs nữa"
                },
                "A",
                "Vì kết nối từ Nginx vào Container là kết nối nội bộ (http://localhost:8080), nếu không có UseForwardedHeaders(), ASP.NET Core sẽ thấy mọi Request đều đến từ IP 127.0.0.1 và giao thức là http, gây lỗi khi tạo đường link Redirect (như Google Login) hoặc khi giới hạn Rate Limiting theo IP."
            ),

            // =====================================================================
            // BÀI 13.10
            // =====================================================================
            Build(
                topicId,
                "13.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 13 (Thiết lập Trọn gói Dockerfile + Docker Compose + GitHub Actions cho Dự án)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/devops/",
                """
                • **💡 1. Bản Đồ Khép Kín Quy Trình DevOps Cho Một Dự Án .NET Hiện Đại:**
                ```
                [1. Trên máy cá nhân của Hoàng Việt]:
                    Viết code C# + Thêm file Dockerfile (Multi-stage) + docker-compose.yml + .github/workflows/ci-cd.yml
                          │
                          ▼  Gõ lệnh: git push origin main
                [2. Trên đám mây GitHub Actions (CI)]:
                    Tự động tải code ──► dotnet restore ──► dotnet build (Release) ──► Chạy toàn bộ Unit Test (xUnit)
                          │
                          ▼  (Pass 100% ✅)
                [3. Đóng gói & Triển khai (CD)]:
                    Tự động đóng gói Docker Image siêu nhẹ ──► Đẩy lên Registry / Cloud Server
                          │
                          ▼
                [4. Trên Máy chủ Production]:
                    Khởi chạy Container .NET + SQL Server (Volume bền vững) + Tự động chạy MigrateAsync() + Bật HealthCheck (/health)!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Chuẩn bị ứng dụng: HealthChecks & Auto-Migration trong Program.cs",
                        "📥 Đầu vào: File Program.cs của dự án. ⚙️ Cơ chế ngầm: Mở cổng /health để Docker kiểm tra tình trạng sống của web, bật UseForwardedHeaders và tự động MigrateAsync() khi khởi động. 📤 Đầu ra: Code sẵn sàng cho môi trường Container.",
                        "app.UseForwardedHeaders();\napp.MapHealthChecks(\"/health\");\n\nusing (var scope = app.Services.CreateScope()) {\n    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();\n    await db.Database.MigrateAsync();\n}"),
                    Step(2,
                        "2. Đóng gói tối ưu bằng Multi-stage Dockerfile",
                        "📥 Đầu vào: Tạo file Dockerfile. ⚙️ Cơ chế ngầm: Tầng 1 dùng sdk:10.0 để restore (có cache) và publish Release; Tầng 2 dùng aspnet:10.0 nhẹ nhàng để chạy file .dll ở cổng 8080. 📤 Đầu ra: Image chuẩn công nghiệp.",
                        "FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build\nWORKDIR /src\nCOPY [\"HoangViet.csproj\", \"./\"]\nRUN dotnet restore\nCOPY . .\nRUN dotnet publish -c Release -o /app/publish\n\nFROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final\nWORKDIR /app\nEXPOSE 8080\nCOPY --from=build /app/publish .\nENTRYPOINT [\"dotnet\", \"HoangViet.dll\"]"),
                    Step(3,
                        "3. Ghép nối Web + SQL Server bằng docker-compose.yml",
                        "📥 Đầu vào: Tạo file docker-compose.yml. ⚙️ Cơ chế ngầm: Định nghĩa dịch vụ sql-db (có gắn volume lưu trữ vĩnh viễn) và dịch vụ web-app (truyền chuỗi kết nối qua biến môi trường dùng dấu __). 📤 Đầu ra: Chạy cả hệ thống bằng 1 lệnh.",
                        "docker compose up -d --build"),
                    Step(4,
                        "4. Tự động hóa kiểm tra bằng GitHub Actions (.github/workflows/ci.yml)",
                        "📥 Đầu vào: Đẩy code lên GitHub. ⚙️ Cơ chế ngầm: GitHub Actions tự động chạy dotnet build, dotnet test và docker build để đảm bảo dự án luôn biên dịch và đóng gói thành công 100%. 📤 Đầu ra: Quy trình CI/CD chuyên nghiệp.",
                        "git add .\ngit commit -m \"ci: add Dockerfile, docker-compose and GitHub Actions workflow\"\ngit push origin main")
                },
                "(Câu hỏi tổng hợp Chủ đề 13) Khi bạn chạy hệ thống gồm Web .NET và SQL Server bằng Docker Compose, nếu bạn gõ lệnh docker compose down để tắt và xóa các Container đi rồi hôm sau bật lại bằng docker compose up -d, thành phần cấu hình nào trong file docker-compose.yml giúp toàn bộ dữ liệu các bảng trong SQL Server vẫn được giữ nguyên vẹn 100% không bị mất đi?",
                new List<string>
                {
                    "A. Cấu hình 'volumes' (ví dụ: volumes: - sql_data:/var/opt/mssql) giúp ánh xạ thư mục lưu trữ dữ liệu của SQL Server ra một vùng đĩa cứng bền vững bên ngoài vòng đời của Container",
                    "B. Cấu hình 'ports: 1433:1433'",
                    "C. Cấu hình 'container_name'",
                    "D. Nhờ trình duyệt web tự lưu lại toàn bộ SQL Server"
                },
                "A",
                "Mặc định hệ thống file bên trong một Container là tạm thời (Ephemeral) — xóa Container là mất dữ liệu. Chỉ khi gắn Docker Volume vào thư mục /var/opt/mssql của SQL Server thì dữ liệu mới được lưu trữ bền vững trên máy chủ vật lý."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (13.1 -> 13.10) cho Chủ đề 13 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic13 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 13);
        if (topic13 == null)
            return;

        topic13.Title = "Chủ đề 13: Triển khai ứng dụng với CI/CD và Docker";
        topic13.Description = "10 Bài học con siêu chi tiết từ dotnet publish & Kestrel/Reverse Proxy, Cấu hình đa môi trường & Environment Variables, Bản chất Docker, Multi-Stage Dockerfile & Layer Caching, Docker Compose (Web + SQL Server + Volumes), Bản chất CI/CD, GitHub Actions Workflow, Database Migrations trên Production (Idempotent Script), Nginx + HTTPS đến Checklist DevOps trọn gói.";

        var targetLessons = GetLessons(topic13.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic13.Id)
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
