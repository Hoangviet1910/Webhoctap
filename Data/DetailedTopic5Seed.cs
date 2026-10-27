using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic5Seed
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
            // BÀI 5.1
            // =====================================================================
            Build(
                topicId,
                "5.1",
                "Bản chất của Git (Distributed VCS) và 3 Vùng trạng thái cốt lõi (Working Directory, Staging Area, Repository)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/what-is-git",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Khi làm bài tập lớn hoặc dự án công ty mà không biết dùng Git, sinh viên thường lưu file kiểu: `DoAn_Ban1.zip`, `DoAn_SuaLoi.zip`, `DoAn_ChotCuoi_ThaySua.zip` rồi gửi qua Zalo. Khi 3 người cùng sửa chung 1 file `HomeController.cs`, việc ngồi copy-paste ghép code bằng tay là một thảm họa!
                  - **Git** chính là một **"Cỗ máy thời gian & Quản gia hợp nhất code"**: Nó chụp ảnh lại từng cột mốc thay đổi của dự án (`Commit`), cho phép bạn quay ngược thời gian về bất kỳ thời điểm nào trong quá khứ và tự động ghép code của 10 người làm việc cùng lúc mà không bị đè mất code của nhau!
                • **🖼️ 2. Sơ đồ 3 Vùng Trạng Thái Cốt Lõi Của Git Trên Máy Tính Của Bạn:**
                ```
                [1. Working Directory]          [2. Staging Area (Index)]          [3. Local Repository (.git)]
                (Thư mục bạn đang gõ code)       (Khu vực đóng thùng chờ duyệt)     (Kho lưu trữ lịch sử vĩnh viễn)
                          │                                   │                                    │
                          ├─────── git add <tên_file> ───────►│                                    │
                          │        (Hoặc: git add .)          ├─────── git commit -m "..." ───────►│
                          │                                   │                                    │
                          ◄─────────────────────────── git checkout / restore ─────────────────────┘
                ```
                • **📊 3. Bảng Giải Thích Chi Tiết 3 Vùng & Các Trạng Thái Của File Trong Git:**
                | Vùng / Trạng thái | Tên kỹ thuật | Giải thích dễ hiểu | Lệnh tương tác chính |
                | :--- | :--- | :--- | :--- |
                | **Vùng 1: Thư mục làm việc** | `Working Directory` | Chính là các file `.cs`, `.cshtml` bạn đang mở và gõ code trên Visual Studio. | `git status` (Kiểm tra xem file nào vừa bị sửa). |
                | **Vùng 2: Khu vực chờ** | `Staging Area` (`Index`) | Giống như "Giỏ hàng Shopee": Bạn chọn lọc những file nào đã code xong xuôi bỏ vào giỏ trước khi bấm thanh toán. | `git add TenFile.cs` hoặc `git add .` (Bỏ tất cả vào giỏ). |
                | **Vùng 3: Kho lịch sử máy trạm**| `Local Repository` | Nằm ẩn trong thư mục `.git`. Mỗi lần `commit`, Git chụp 1 bức ảnh (`Snapshot`) cất vào đây kèm mã định danh (Hash). | `git commit -m "Mô tả công việc vừa làm"`. |
                • **⚠️ 4. Phân biệt rõ `Git` và `GitHub / GitLab` (Câu hỏi phỏng vấn kinh điển):**
                  - **Git:** Là **phần mềm cài trên máy tính của bạn** để theo dõi lịch sử code (chạy hoàn toàn không cần mạng Internet).
                  - **GitHub / GitLab / Azure Repos:** Là **trang web đám mây trên Internet** dùng để lưu trữ kho Git trực tuyến, giúp các thành viên trong nhóm đẩy (`push`) code lên và kéo (`pull`) code về chia sẻ cho nhau!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khởi tạo kho Git (git init) & Kiểm tra trạng thái (git status)",
                        "📥 Đầu vào: Thư mục dự án C# mới tạo. ⚙️ Cơ chế ngầm: Lệnh git init tạo ra một thư mục ẩn '.git' chứa toàn bộ cơ sở dữ liệu lịch sử. Lệnh git status quét các file hiện tại. 📤 Đầu ra: Các file mới hiện màu đỏ (Untracked).",
                        "git init\ngit status // Hiển thị các file màu đỏ (chưa đưa vào Staging)"),
                    Step(2,
                        "2. Đưa file vào vùng chờ Staging Area (git add)",
                        "📥 Đầu vào: Các file vừa thêm hoặc sửa đổi. ⚙️ Cơ chế ngầm: Git ghi nhận bản chụp tạm thời của các file được chọn vào file Index trong thư mục .git. 📤 Đầu ra: Các file chuyển sang màu xanh lá (Staged - sẵn sàng để commit).",
                        "git add Controllers/HomeController.cs // Thêm 1 file cụ thể\ngit add .                             // Hoặc thêm toàn bộ các file đã sửa"),
                    Step(3,
                        "3. Chốt lưu cột mốc lịch sử vào Repository (git commit)",
                        "📥 Đầu vào: Các file đang nằm trong Staging Area. ⚙️ Cơ chế ngầm: Tạo một đối tượng Commit vĩnh viễn kèm mã băm SHA-1 (ví dụ: a1b2c3d), tên tác giả, ngày giờ và lời nhắn. 📤 Đầu ra: Cột mốc lịch sử an toàn.",
                        "git commit -m \"feat: tao giao dien trang chu va HomeController\"\ngit log --oneline // Xem lại danh sách các mốc lịch sử vừa lưu")
                },
                "Trong quy trình làm việc của Git, vùng Staging Area (Index) nằm ở giữa Working Directory và Local Repository có tác dụng chính là gì?",
                new List<string>
                {
                    "A. Giúp lập trình viên chọn lọc chính xác những file nào đã hoàn thiện để gom vào một lần Commit (thay vì bắt buộc phải lưu tất cả mọi file đang làm dở dang trong thư mục dự án)",
                    "B. Dùng để tự động biên dịch code C# ra file .exe",
                    "C. Dùng để xóa vĩnh viễn các file bị lỗi",
                    "D. Là nơi lưu trữ mật khẩu tài khoản GitHub"
                },
                "A",
                "Giả sử bạn đang sửa 5 file nhưng mới chỉ có 2 file chạy hoàn chỉnh, vùng Staging Area (git add file1 file2) cho phép bạn chỉ đóng gói đúng 2 file đó vào một Commit sạch sẽ, 3 file đang làm dở vẫn giữ nguyên ở Working Directory."
            ),

            // =====================================================================
            // BÀI 5.2
            // =====================================================================
            Build(
                topicId,
                "5.2",
                "Cấu hình Git chuẩn cho dự án .NET và tầm quan trọng sống còn của file .gitignore",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/dotnet/core/tools/dotnet-new",
                """
                • **💡 1. Hiểu bản chất của file `.gitignore` trong 30 giây:**
                  - Khi bạn bấm nút `Run (F5)` hoặc `Build` một dự án C# trong Visual Studio, hệ thống sẽ tự động sinh ra hàng nghìn file tạm và file biên dịch nằm trong 2 thư mục **`bin/`** và **`obj/`**, cùng thư mục cài đặt cá nhân **`.vs/`** (nặng hàng trăm Megabyte).
                  - Nếu bạn đẩy (`push`) cả thư mục `bin/`, `obj/`, `.vs/` lên GitHub: Kho chứa sẽ bị nặng khủng khiếp, và **chắc chắn 100% đồng đội của bạn kéo code về sẽ bị lỗi xung đột (`Conflict`) hàng trăm file nhị phân không thể mở lên được**!
                  - File **`.gitignore`** chính là **"Danh sách đen"**: Bất kỳ tên thư mục hoặc file nào được ghi trong `.gitignore` sẽ bị Git lơ đi hoàn toàn, không bao giờ đưa lên GitHub!
                • **🖼️ 2. Sơ đồ Những Thứ PHẢI ĐẨY LÊN GIT vs Những Thứ BẮT BUỘC PHẢI CHẶN BẰNG `.gitignore`:**
                ```
                Dự án C# ASP.NET Core MVC / Web API:
                ├── ✅ PHẢI ĐƯA LÊN GIT (Source Code gốc):
                │     ├── Controllers/, Models/, Views/, Services/, Data/, Migrations/
                │     └── Program.cs, HoangViet.csproj, HoangViet.sln
                │
                └── ❌ BẮT BUỘC CHẶN TRONG .gitignore (Rác biên dịch & Bí mật cá nhân):
                      ├── bin/ , obj/      (Thư mục chứa file .dll, .exe tự sinh ra khi bấm Build)
                      ├── .vs/ , .vscode/  (Cấu hình cửa sổ riêng trên máy từng người)
                      └── appsettings.Development.json (Nếu chứa API Key thật hoặc Mật khẩu riêng!)
                ```
                • **📊 3. Lệnh Tạo Nhanh File `.gitignore` Chuẩn Microsoft Chỉ Trong 1 Giây:**
                  Thay vì phải tự gõ tay từng dòng vào file `.gitignore`, .NET CLI cung cấp sẵn một lệnh thần thánh tạo ra file `.gitignore` chuẩn chỉnh 100% cho Visual Studio:
                  **`dotnet new gitignore`**
                • **⚠️ 4. Bẫy Kinh Điển: "Tại sao tôi đã thêm `bin/` vào `.gitignore` rồi mà Git vẫn cứ theo dõi nó?"**
                  - Đó là vì bạn đã lỡ tay `git add .` và `git commit` thư mục `bin/` **TRƯỚC KHI** bạn tạo file `.gitignore`!
                  - **Cách khắc phục ngay lập tức:** Dùng lệnh xóa khỏi bộ nhớ đệm của Git (mà không làm mất file trên ổ cứng):
                    `git rm -r --cached bin/ obj/ .vs/` rồi `git commit -m "chore: xoa thu muc tam khoi git"`.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo danh tính tác giả lần đầu cài Git",
                        "📥 Đầu vào: Tên và Email của bạn. ⚙️ Cơ chế ngầm: Lưu thông tin vào cấu hình toàn cục (--global) trên máy tính để mọi Commit bạn tạo đều ghi danh tên bạn. 📤 Đầu ra: Định danh chuẩn trên GitHub.",
                        "git config --global user.name \"Nguyen Hoang Viet\"\ngit config --global user.email \"viet@example.com\""),
                    Step(2,
                        "2. Sinh file .gitignore chuẩn .NET TRƯỚC KHI git add",
                        "📥 Đầu vào: Thư mục gốc của dự án C#. ⚙️ Cơ chế ngầm: Lệnh dotnet new gitignore tự động tạo file .gitignore chặn sẵn bin/, obj/, .vs/, *.user, *.suo. 📤 Đầu ra: Kho code sạch bóng file rác.",
                        "dotnet new gitignore\ngit add .\ngit commit -m \"Initial commit with standard .NET gitignore\""),
                    Step(3,
                        "3. Xử lý khi lỡ tay đẩy nhầm bin/ obj/ lên Git trước đó",
                        "📥 Đầu vào: Kho Git đã lỡ lưu thư mục bin và obj. ⚙️ Cơ chế ngầm: Cờ --cached bảo Git ngừng theo dõi thư mục đó trên GitHub nhưng vẫn giữ nguyên file trên ổ cứng máy tính của bạn. 📤 Đầu ra: Làm sạch kho chứa từ xa.",
                        "git rm -r --cached bin obj .vs\ngit commit -m \"fix: remove bin and obj from git tracking\"")
                },
                "Khi khởi tạo một dự án C# .NET mới để làm việc nhóm trên GitHub, tại sao bạn BẮT BUỘC phải đưa các thư mục bin/, obj/ và .vs/ vào trong file .gitignore?",
                new List<string>
                {
                    "A. Vì bin/, obj/ là các file nhị phân tự động sinh ra lại mỗi khi bấm Build trên máy từng người, và .vs/ là cấu hình IDE cá nhân; nếu đẩy lên Git sẽ gây nặng kho chứa và làm xung đột (Conflict) liên tục giữa các thành viên",
                    "B. Vì GitHub cấm tải lên các thư mục có tên ngắn hơn 4 ký tự",
                    "C. Vì xóa bin/ và obj/ giúp code C# tự động biến thành ngôn ngữ Java",
                    "D. Không cần .gitignore, đẩy hết lên càng tốt"
                },
                "A",
                "Nguyên tắc vàng của quản lý mã nguồn là chỉ lưu trữ Source Code gốc. Khi người khác kéo code về và chạy dotnet build (hoặc bấm F5), máy của họ sẽ tự động tạo lại thư mục bin/ và obj/ tương thích với máy đó."
            ),

            // =====================================================================
            // BÀI 5.3
            // =====================================================================
            Build(
                topicId,
                "5.3",
                "Làm việc với Kho chứa từ xa trên GitHub (Remote Repository: clone, remote, push, pull, fetch)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/pushing",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - `Local Repository` nằm trong ổ cứng máy tính của bạn (nếu hỏng ổ cứng là mất hết).
                  - **`Remote Repository` (Kho chứa từ xa trên GitHub / GitLab)** là bản sao nằm trên máy chủ đám mây, đóng vai trò là **"Trạm trung chuyển trung tâm"** để cả nhóm đồng bộ code với nhau hàng ngày. Tên gọi mặc định của kết nối từ máy bạn lên GitHub luôn được đặt bí danh là **`origin`**.
                • **🖼️ 2. Sơ đồ Luồng Đồng Bộ Dữ Liệu Giữa Máy Của Bạn (`Local`) và `GitHub (origin)`:**
                ```
                                      ┌──────────────────────────────────────┐
                                      │   GITHUB REMOTE REPOSITORY (origin)  │
                                      └──────┬───────────────▲───────────────┘
                                             │               │
                  1. Lần đầu tải về:         │               │ 2. Đẩy các Commit mới của bạn lên:
                     git clone <url>         │               │    git push -u origin <tên_nhánh>
                  3. Kéo code mới của nhóm:  │               │
                     git pull origin <nhánh> ▼               │
                                      ┌──────────────────────┴───────────────┐
                                      │     MÁY TÍNH CỦA BẠN (Local Git)     │
                                      └──────────────────────────────────────┘
                ```
                • **📊 3. Phân biệt rõ 5 lệnh làm việc với GitHub (`clone`, `remote`, `push`, `fetch`, `pull`):**
                | Câu lệnh Git | Chức năng chi tiết | Khi nào sử dụng? |
                | :--- | :--- | :--- |
                | **`git clone <url>`** | Tải toàn bộ một dự án có sẵn trên GitHub về máy tính của bạn. | Chỉ chạy **ĐÚNG 1 LẦN ĐẦU TIÊN** khi mới nhận dự án về máy! |
                | **`git remote add origin <url>`**| Nối dự án đang có sẵn trên máy bạn với một kho trống mới tạo trên GitHub. | Khi bạn tạo project trên Visual Studio trước rồi mới lên GitHub tạo kho. |
                | **`git push origin <nhánh>`** | Đẩy các `commit` đã lưu ở máy bạn lên trên GitHub. | Sau khi bạn đã `git add` và `git commit` xong tính năng ở máy. |
                | **`git fetch origin`** | Kiểm tra và tải lịch sử mới từ GitHub về xem trước, nhưng **CHƯA gộp vào code đang mở của bạn**. | Khi muốn kiểm tra xem đồng đội vừa đẩy gì lên một cách an toàn. |
                | **`git pull origin <nhánh>`** | Bằng **`git fetch` + `git merge` gộp làm một**: Kéo code mới nhất trên GitHub về và trộn thẳng vào nhánh hiện tại của bạn! | Chạy mỗi buổi sáng trước khi bắt đầu gõ code mới! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kết nối Project có sẵn trên máy lên GitHub mới tạo",
                        "📥 Đầu vào: Đường link HTTPS của Repository trên GitHub. ⚙️ Cơ chế ngầm: Đăng ký địa chỉ URL đó dưới cái tên đại diện là 'origin' và đẩy nhánh main lên. 📤 Đầu ra: Code xuất hiện trên GitHub.",
                        "git remote add origin https://github.com/hoangviet/QuanLyThuVien.git\ngit branch -M main\ngit push -u origin main"),
                    Step(2,
                        "2. Tải dự án của nhóm về máy lần đầu (git clone)",
                        "📥 Đầu vào: Link GitHub của nhóm bài tập lớn hoặc công ty. ⚙️ Cơ chế ngầm: Tự động tạo thư mục dự án, tải toàn bộ lịch sử commit và thiết lập sẵn kết nối origin. 📤 Đầu ra: Mở file .sln lên chạy được ngay.",
                        "git clone https://github.com/nhom-uneti/QuanLyThuVien.git\ncd QuanLyThuVien"),
                    Step(3,
                        "3. Cập nhật code mới của đồng đội hàng ngày (git pull)",
                        "📥 Đầu vào: Đồng đội vừa đẩy code mới lên nhánh develop. ⚙️ Cơ chế ngầm: Tải các commit mới từ origin/develop về và tự động ghép (merge) vào code trên máy của bạn. 📤 Đầu ra: Máy bạn có code mới nhất của cả nhóm.",
                        "git pull origin develop")
                },
                "Sự khác biệt cốt lõi giữa hai câu lệnh git fetch và git pull khi làm việc với kho chứa từ xa (GitHub) là gì?",
                new List<string>
                {
                    "A. git fetch chỉ tải thông tin lịch sử mới nhất từ GitHub về máy để bạn kiểm tra chứ CHƯA tự động gộp vào file code bạn đang làm việc; còn git pull sẽ vừa tải về (fetch) vừa tự động gộp (merge) thẳng vào nhánh hiện tại của bạn",
                    "B. git pull dùng để đẩy code lên GitHub, còn git fetch dùng để xóa code",
                    "C. git fetch chỉ chạy được khi tắt máy tính",
                    "D. Hai lệnh hoàn toàn giống hệt nhau 100%"
                },
                "A",
                "Công thức bất biến trong Git là: git pull = git fetch + git merge. Dùng git fetch giúp bạn xem trước đồng đội đã sửa những gì trước khi quyết định gộp vào code của mình."
            ),

            // =====================================================================
            // BÀI 5.4
            // =====================================================================
            Build(
                topicId,
                "5.4",
                "Làm chủ Nhánh (Branching): Tại sao không bao giờ code trực tiếp trên nhánh main?",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/branching",
                """
                • **💡 1. Hiểu bản chất của Nhánh (`Branch`) trong 30 giây:**
                  - Hãy tưởng tượng nhánh **`main` (hoặc `master`)** là **Bản sản phẩm chính thức đang trưng bày cho khách hàng (hoặc nộp cho giảng viên)** — nó bắt buộc phải luôn luôn chạy tốt 100%, không bao giờ được phép có lỗi biên dịch!
                  - Khi bạn muốn làm thêm chức năng *"Quản lý Mượn Trả Sách"*, nếu bạn viết thẳng vào nhánh `main`, trong lúc bạn đang gõ dở dang bị lỗi đỏ thì cả nhóm kéo nhánh `main` về sẽ không ai chạy được web nữa!
                  - **Giải pháp `Branch` (Tách nhánh):** Bạn tạo ra một **"Vũ trụ song song"** tên là `feature/muon-tra-sach` tách ra từ `main`. Trong nhánh riêng này, bạn thoải mái đập phá, viết code, sửa lỗi. Nhánh `main` gốc hoàn toàn không bị ảnh hưởng! Khi nào làm xong 100%, bạn mới ghép (`Merge`) nhánh của bạn trở lại nhánh chính!
                • **🖼️ 2. Sơ đồ Tách Nhánh Tính Năng (`Feature Branch`) và Con Trỏ `HEAD`:**
                ```
                Nhánh chính (main):     (C1) ─────────► (C2) ─────────────────────────► (C5 - Merge xong!)
                                                          \                             /
                Nhánh của Việt                             \                           /
                (feature/login):                            └──► (C3: Code) ──► (C4: Test OK)
                                                                                   ▲
                                                                                 HEAD (Con trỏ cho biết bạn đang đứng ở nhánh nào)
                ```
                • **📊 3. Bảng Các Câu Lệnh Quản Lý Nhánh Hiện Đại (`git switch` & `git checkout`):**
                | Mục đích thao tác | Cú pháp truyền thống (`checkout`) | Cú pháp Git mới chuẩn (`switch`) | Giải thích |
                | :--- | :--- | :--- | :--- |
                | **Xem danh sách nhánh** | `git branch` | `git branch -a` | Nhánh nào có dấu sao `*` màu xanh đứng trước chính là nhánh bạn đang đứng (`HEAD`). |
                | **Tạo nhánh mới & Nhảy sang luôn**| **`git checkout -b <tên_nhánh>`** | **`git switch -c <tên_nhánh>`** | Vừa tạo nhánh mới từ vị trí hiện tại, vừa chuyển sang nhánh đó ngay lập tức! |
                | **Chuyển đổi qua lại giữa 2 nhánh**| `git checkout <tên_nhánh>` | `git switch <tên_nhánh>` | Khi chuyển nhánh, toàn bộ file trong Visual Studio sẽ tự động biến hình theo đúng code của nhánh đó! |
                | **Xóa nhánh đã gộp xong** | `git branch -d <tên_nhánh>` | `git branch -D <tên_nhánh>` | Cờ `-d` xóa an toàn (khi đã merge); cờ `-D` ép xóa cưỡng bức. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo nhánh tính năng mới và chuyển sang đó ngay",
                        "📥 Đầu vào: Đang đứng ở nhánh develop (hoặc main) sạch sẽ. ⚙️ Cơ chế ngầm: Tạo một con trỏ nhánh mới tên là feature/book-crud trỏ vào commit hiện tại và di chuyển HEAD sang nhánh mới. 📤 Đầu ra: Bạn đang đứng trong không gian riêng an toàn.",
                        "git checkout -b feature/book-crud\n// Hoặc lệnh mới tương đương:\ngit switch -c feature/book-crud"),
                    Step(2,
                        "2. Code và Commit độc lập trên nhánh riêng",
                        "📥 Đầu vào: Viết thêm BookController.cs và BookService.cs. ⚙️ Cơ chế ngầm: Mọi commit lúc này chỉ nối dài trên nhánh feature/book-crud, nhánh main hoàn toàn giữ nguyên không bị tác động. 📤 Đầu ra: Lịch sử nhánh riêng tiến về phía trước.",
                        "git add .\ngit commit -m \"feat: hoan thanh CRUD cho bang Sach\"\ngit push -u origin feature/book-crud"),
                    Step(3,
                        "3. Chuyển về nhánh chính và xóa nhánh cũ sau khi đã gộp",
                        "📥 Đầu vào: Nhánh feature/book-crud đã được gộp vào develop thành công. ⚙️ Cơ chế ngầm: Di chuyển HEAD về lại develop và dọn dẹp nhánh tạm cho gọn kho. 📤 Đầu ra: Sẵn sàng tách nhánh cho nhiệm vụ tiếp theo.",
                        "git switch develop\ngit pull origin develop\ngit branch -d feature/book-crud")
                },
                "Câu lệnh nào dưới đây giúp bạn vừa tạo mới một nhánh có tên là feature/cart-api vừa lập tức chuyển sang đứng ở nhánh mới đó chỉ trong 1 dòng lệnh duy nhất?",
                new List<string>
                {
                    "A. git checkout -b feature/cart-api (hoặc git switch -c feature/cart-api)",
                    "B. git branch -d feature/cart-api",
                    "C. git init feature/cart-api",
                    "D. git clone feature/cart-api"
                },
                "A",
                "Cờ -b trong git checkout -b (hoặc -c / --create trong git switch -c) thực hiện đồng thời 2 bước: tạo nhánh mới (git branch <tên>) và chuyển con trỏ HEAD sang nhánh đó (git checkout <tên>)."
            ),

            // =====================================================================
            // BÀI 5.5
            // =====================================================================
            Build(
                topicId,
                "5.5",
                "Hợp nhất nhánh: Phân biệt bản chất Git Merge (Fast-Forward vs 3-Way Merge) và Git Rebase",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/merging",
                """
                • **💡 1. Hiểu bản chất trong 30 giây:**
                  - Sau khi bạn đã code xong tính năng ở nhánh `feature/login`, làm sao để đưa code đó vào nhánh chung `develop`? Có 2 trường phái hợp nhất nhánh trong Git là **`git merge`** và **`git rebase`**:
                    1. **`git merge` (Hợp nhất giữ nguyên lịch sử thật):** Giữ nguyên toàn bộ lịch sử rẽ nhánh của bạn và tạo thêm một **"Nút thắt hợp nhất" (`Merge Commit`)** nối 2 nhánh lại với nhau.
                    2. **`git rebase` (Viết lại lịch sử thẳng tắp):** Nhấc toàn bộ các commit của nhánh bạn đem **cắm nối đuôi lên ngọn mới nhất** của nhánh `develop`, làm cho lịch sử nhìn thẳng băng như một đường thẳng duy nhất!
                • **🖼️ 2. Sơ đồ Trực quan So sánh `Git Merge` vs `Git Rebase`:**
                ```
                [Trạng thái ban đầu trước khi gộp]:
                develop:        (C1) ───► (C2) ───► (C3 - Đồng đội vừa thêm)
                                            \
                feature/viet:                └───► (V1) ───► (V2 - Việt vừa làm)

                [Cách 1 - Dùng git merge (Đứng ở develop gõ: git merge feature/viet)]:
                develop:        (C1) ───► (C2) ───► (C3) ───────────► (M - Merge Commit nối 2 nhánh!)
                                            \                       /
                feature/viet:                └───► (V1) ───► (V2) ─┘  (Giữ nguyên lịch sử thật 100%)

                [Cách 2 - Dùng git rebase (Đứng ở feature/viet gõ: git rebase develop)]:
                develop:        (C1) ───► (C2) ───► (C3) ───► (V1') ───► (V2')  (Lịch sử thẳng tắp trên 1 đường!)
                ```
                • **📊 3. Bảng So sánh Chi tiết `Git Merge` và `Git Rebase` (Câu hỏi phỏng vấn tầm Fresher/Junior):**
                | Tiêu chí | `git merge` | `git rebase` |
                | :--- | :--- | :--- |
                | **Cơ chế hoạt động** | Tạo thêm 1 `Merge Commit` có 2 cha để chập 2 nhánh lại. | Tạo ra các bản sao commit mới (`V1'`, `V2'`) đặt lên đầu nhánh đích. |
                | **Hình dáng lịch sử (`git log`)**| Có các nhánh rẽ ra và chập vào đúng như thực tế diễn ra. | Thẳng tắp một đường (Linear History), rất sạch đẹp. |
                | **Độ an toàn cho người mới** | **An toàn tuyệt đối 100%** (Không làm thay đổi các commit cũ). | ⚠️ **Cần cẩn thận:** Tuyệt đối **KHÔNG `rebase` trên nhánh chung (`main`/`develop`)** mà nhiều người đang dùng! |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Quy trình chuẩn khi dùng git merge",
                        "📥 Đầu vào: Muốn gộp nhánh feature/login vào nhánh develop. ⚙️ Cơ chế ngầm: Bắt buộc phải chuyển sang đứng ở nhánh nhận (develop) trước, sau đó mới gọi lệnh merge nhánh con vào! 📤 Đầu ra: Tạo ra Merge Commit hợp nhất.",
                        "git switch develop          // Bước 1: Đứng ở nhánh đích (develop)\ngit pull origin develop     // Bước 2: Cập nhật develop mới nhất\ngit merge feature/login     // Bước 3: Hút code từ nhánh feature/login vào develop"),
                    Step(2,
                        "2. Trường hợp Fast-Forward Merge (Trượt thẳng)",
                        "📥 Đầu vào: Trong lúc bạn làm ở nhánh con, nhánh develop không có ai thêm commit mới nào. ⚙️ Cơ chế ngầm: Git chỉ cần trượt thẳng con trỏ develop lên trùng với nhánh con mà không cần tạo Merge Commit. 📤 Đầu ra: Gộp tức thì.",
                        "// Nếu muốn ép luôn tạo Merge Commit để lưu dấu vết nhánh con, dùng cờ --no-ff:\ngit merge --no-ff feature/login"),
                    Step(3,
                        "3. Làm sạch nhánh cá nhân bằng git rebase trước khi tạo PR",
                        "📥 Đầu vào: Đứng ở nhánh cá nhân feature/login. ⚙️ Cơ chế ngầm: Kéo các thay đổi mới nhất của develop垫 xuống dưới đáy nhánh của mình để lịch sử thẳng đẹp. 📤 Đầu ra: Sẵn sàng Merge không tì vết.",
                        "git switch feature/login\ngit rebase develop")
                },
                "Giả sử bạn đã code xong tính năng ở nhánh feature/payment và bây giờ muốn dùng lệnh git merge trên máy để gộp code từ nhánh feature/payment vào nhánh develop. Trình tự câu lệnh nào dưới đây là ĐÚNG?",
                new List<string>
                {
                    "A. Đứng ở nhánh feature/payment rồi gõ git merge feature/payment",
                    "B. Chuyển sang đứng ở nhánh đích trước bằng lệnh 'git switch develop' (hoặc git checkout develop), sau đó gõ lệnh 'git merge feature/payment'",
                    "C. Xóa nhánh develop đi rồi đổi tên nhánh",
                    "D. Gõ lệnh git init develop"
                },
                "B",
                "Quy tắc của git merge <nhánh_nguồn> là: câu lệnh luôn hút code từ <nhánh_nguồn> gộp vào nhánh mà bạn đang đứng hiện tại (HEAD). Do đó muốn gộp vào develop thì phải chuyển sang đứng ở develop trước."
            ),

            // =====================================================================
            // BÀI 5.6
            // =====================================================================
            Build(
                topicId,
                "5.6",
                "Giải mã \"Nỗi sợ lớn nhất\": Xử lý Đụng độ Code (Merge Conflict) từng bước một trên VS Code / Visual Studio",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/resolution",
                """
                • **💡 1. Tại sao lại xảy ra `Merge Conflict` (Đụng độ code) và tại sao bạn KHÔNG CẦN SỢ nó?**
                  - Nếu bạn sửa **Dòng 10** của file `Program.cs`, còn bạn cùng nhóm sửa **Dòng 80** của cùng file `Program.cs` đó $\rightarrow$ Git đủ thông minh để **tự động ghép cả hai** mà không hề hỏi bạn câu nào!
                  - **NHƯNG:** Nếu cả bạn VÀ người bạn cùng nhóm đều sửa vào **ĐÚNG DÒNG SỐ 25 của cùng 1 file `Program.cs`** (ví dụ: bạn đổi tên Database thành `Db_Viet`, còn bạn kia đổi đúng dòng đó thành `Db_Hung`), thì khi `git pull` hoặc `git merge`, Git sẽ lịch sự dừng lại và hỏi bạn:
                    *"Cả hai người cùng sửa dòng số 25 này thành 2 nội dung khác nhau, tôi không dám tự ý xóa của ai cả! Bạn là con người, hãy mở file đó ra và chọn giúp tôi xem giữ dòng nào nhé!"* $\rightarrow$ Đó chính là **Merge Conflict**!
                • **🖼️ 2. Giải phẫu các ký hiệu đánh dấu Conflict (`<<<<<<<`, `=======`, `>>>>>>>`) trong file code:**
                ```csharp
                <<<<<<< HEAD (Current Change - Code đang có trên máy CỦA BẠN)
                string dbName = "QuanLyThuVien_Viet";
                ======= (Vách ngăn phân chia giữa 2 bên)
                string dbName = "QuanLyThuVien_Nhom14";
                >>>>>>> origin/develop (Incoming Change - Code của ĐỒNG ĐỘI mới kéo về)
                ```
                • **📊 3. Bốn lựa chọn chỉ với 1 cú Click chuột trên VS Code / Visual Studio:**
                  Khi mở file bị báo đỏ Conflict trên VS Code hoặc Visual Studio, bạn không cần xóa tay mà chỉ cần bấm vào 1 trong 4 nút hiện sẵn ngay trên đầu đoạn code đó:
                | Nút bấm trên IDE | Ý nghĩa hành động | Khi nào chọn? |
                | :--- | :--- | :--- |
                | **1. `Accept Current Change`** | Giữ lại code của **BẠN** (`HEAD`), xóa bỏ phần của đồng đội. | Khi code của bạn mới là bản đúng nhất. |
                | **2. `Accept Incoming Change`**| Lấy code mới của **ĐỒNG ĐỘI** vừa kéo về, bỏ phần của bạn. | Khi đồng đội đã viết bản chuẩn hơn. |
                | **3. `Accept Both Changes`** | **Giữ lại CẢ HAI** đoạn code (xếp nối tiếp nhau)! | ⭐ **Dùng rất nhiều khi cả 2 cùng đăng ký thêm `AddScoped` trong `Program.cs`!** |
                | **4. Tự sửa tay & Xóa 3 dòng mốc**| Tự chỉnh lại nội dung kết hợp và xóa sạch các dòng `<<<`, `===`, `>>>`. | Khi muốn gộp logic bên trong cùng 1 dòng. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Phát hiện Conflict khi kéo code về (git pull)",
                        "📥 Đầu vào: Bạn và đồng đội cùng thêm dịch vụ mới vào dòng số 15 của file Program.cs. ⚙️ Cơ chế ngầm: Khi gõ git pull origin develop, Git báo 'CONFLICT (content): Merge conflict in Program.cs' và tạm dừng quá trình merge. 📤 Đầu ra: File Program.cs hiện dấu chấm than đỏ.",
                        "git pull origin develop\n// Auto-merging Program.cs\n// CONFLICT (content): Merge conflict in Program.cs\n// Automatic merge failed; fix conflicts and then commit the result."),
                    Step(2,
                        "2. Mở file Program.cs và chọn Accept Both Changes",
                        "📥 Đầu vào: Bạn thêm IBookService, đồng đội thêm IReaderService ở cùng một chỗ. ⚙️ Cơ chế ngầm: Vì cả 2 dịch vụ đều cần thiết cho dự án, bạn bấm chọn 'Accept Both Changes' (hoặc xóa 3 dòng ký hiệu <<<, ===, >>> để giữ lại cả 2 dòng code). 📤 Đầu ra: Code sạch sẽ, biên dịch không lỗi.",
                        "// Sau khi chọn Accept Both Changes, cả 2 dòng đều được giữ lại đẹp đẽ:\nbuilder.Services.AddScoped<IBookService, BookService>();\nbuilder.Services.AddScoped<IReaderService, ReaderService>();"),
                    Step(3,
                        "3. Lưu file, git add và git commit để hoàn tất giải quyết",
                        "📥 Đầu vào: File đã hết sạch các ký hiệu <<<, ===, >>>. ⚙️ Cơ chế ngầm: Gõ git add để báo cho Git biết bạn đã phân xử xong đụng độ, sau đó git commit để đóng gói bản hợp nhất. 📤 Đầu ra: Nhánh hợp nhất thành công 100%!",
                        "git add Program.cs\ngit commit -m \"merge: giai quyet conflict file Program.cs\"\ngit push origin feature/my-branch")
                },
                "Trong dự án nhóm ASP.NET Core MVC, bạn thêm dòng builder.Services.AddScoped<IBookRepo, BookRepo>(); vào Program.cs, đồng thời bạn cùng nhóm cũng thêm dòng builder.Services.AddScoped<IUserRepo, UserRepo>(); vào đúng vị trí đó. Khi git pull về xảy ra Merge Conflict tại Program.cs, bạn phải xử lý như thế nào?",
                new List<string>
                {
                    "A. Chọn 'Accept Both Changes' (Giữ lại cả hai dòng đăng ký dịch vụ của cả bạn và đồng đội), xóa bỏ các dòng ký hiệu <<<, ===, >>> rồi thực hiện git add và git commit",
                    "B. Chọn 'Accept Current Change' để xóa mất dịch vụ IUserRepo của bạn cùng nhóm",
                    "C. Xóa luôn file Program.cs đi",
                    "D. Cài lại Windows vì máy đã bị hỏng Git"
                },
                "A",
                "Đây là tình huống Conflict phổ biến nhất trong mọi dự án .NET (cùng đăng ký DI trong Program.cs hoặc cùng thêm DbSet trong DbContext). Vì cả 2 tính năng của 2 thành viên đều cần chạy, bạn phải giữ lại cả 2 dòng (Accept Both Changes)."
            ),

            // =====================================================================
            // BÀI 5.7
            // =====================================================================
            Build(
                topicId,
                "5.7",
                "Quy trình làm việc nhóm chuẩn Doanh nghiệp: Git Flow, Pull Request (PR) và Code Review trên GitHub",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/gitworkflow",
                """
                • **💡 1. Hiểu bản chất của mô hình `Git Flow` và `Pull Request (PR)` trong 30 giây:**
                  - Trong các dự án chuyên nghiệp (và bài tập lớn điểm A+), nhóm không bao giờ cho phép từng thành viên tự ý gộp (`merge`) code ở dưới máy cá nhân vào nhánh chính rồi đẩy lên!
                  - Thay vào đó, nhóm khóa nhánh `main` và `develop` lại. Mỗi người làm trên nhánh `feature/...` riêng, đẩy nhánh riêng đó lên GitHub rồi bấm nút tạo một **"Đơn xin gộp code" gọi là `Pull Request (PR)`**.
                  - Trên giao diện `Pull Request` của GitHub, nhóm trưởng (hoặc anh Mentor ở công ty) sẽ nhìn thấy rõ từng dòng màu xanh (mới thêm) và màu đỏ (vừa xóa) của bạn để nhận xét (**Code Review**). Khi đạt chuẩn mới bấm nút **`Merge pull request`** màu xanh lá trên web GitHub!
                • **🖼️ 2. Sơ đồ Hệ Thống Các Nhánh Chuẩn Trong Mô Hình `Git Flow`:**
                ```
                [1. main / master]  ──► Nhánh Sản phẩm chính thức (Chỉ nhận code hoàn hảo để đem đi Bảo vệ / Release)
                        ▲
                        │ (Khi kết thúc giai đoạn Sprint mới gộp từ develop lên main)
                [2. develop]        ──► Nhánh Tích hợp chung của cả nhóm (Nơi gom code hàng ngày của mọi thành viên)
                     ▲     ▲
                     │     └─── Pull Request 1 ─── [feature/viet-book-module]  (Nhánh riêng của Hoàng Việt)
                     └───────── Pull Request 2 ─── [feature/hung-user-module]  (Nhánh riêng của bạn cùng nhóm)
                ```
                • **📊 3. Chuẩn Đặt Tên Nhánh & Đặt Tên Commit (`Conventional Commits`) Khi Đi Thực Tập:**
                | Thành phần | Quy ước chuẩn doanh nghiệp | Ví dụ thực tế ghi điểm tuyệt đối |
                | :--- | :--- | :--- |
                | **Đặt tên nhánh Tính năng**| `feature/<tên-ngắn-gọn>` | `feature/library-borrow-book` |
                | **Đặt tên nhánh Sửa lỗi** | `fix/<tên-lỗi>` hoặc `bugfix/...` | `fix/login-null-exception` |
                | **Tiền tố Commit: `feat:`**| Khi thêm mới một chức năng. | `git commit -m "feat: them trang danh sach doc gia"` |
                | **Tiền tố Commit: `fix:`** | Khi sửa một lỗi bug. | `git commit -m "fix: sua loi sai cong thuc tinh tien phat"` |
                | **Tiền tố Commit: `refactor:`**| Khi tối ưu/dọn dẹp lại code mà không đổi tính năng.| `git commit -m "refactor: tach logic sang BookRepository"` |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Kéo nhánh develop mới nhất về và tách nhánh feature",
                        "📥 Đầu vào: Bắt đầu làm một nhiệm vụ mới. ⚙️ Cơ chế ngầm: Luôn đứng từ nhánh develop mới nhất để tách nhánh con nhằm hạn chế tối đa khả năng bị Conflict về sau. 📤 Đầu ra: Nhánh feature/borrow-book mới tinh.",
                        "git switch develop\ngit pull origin develop\ngit switch -c feature/borrow-book"),
                    Step(2,
                        "2. Code, Test và Commit theo chuẩn Conventional Commits",
                        "📥 Đầu vào: Hoàn thành tính năng mượn sách. ⚙️ Cơ chế ngầm: Kiểm tra dotnet build không có lỗi đỏ, đặt lời nhắn commit có tiền tố feat: rõ ràng. 📤 Đầu ra: Commit sạch sẽ.",
                        "git add .\ngit commit -m \"feat: xay dung luong muon sach va cap nhat ton kho\""),
                    Step(3,
                        "3. Trước khi Push: Kéo develop vào nhánh mình để kiểm tra trước",
                        "📥 Đầu vào: Chuẩn bị đẩy nhánh lên GitHub. ⚙️ Cơ chế ngầm: Gõ git pull origin develop ngay tại nhánh feature của mình. Nếu có Conflict thì giải quyết ngay trên máy mình trước khi gửi lên! 📤 Đầu ra: Đảm bảo PR trên GitHub sẽ hiện chữ 'Able to merge' màu xanh.",
                        "git pull origin develop // Tự kiểm tra xem có đụng độ với ai vừa mới nộp bài không\ngit push -u origin feature/borrow-book"),
                    Step(4,
                        "4. Lên GitHub tạo Pull Request (PR) vào nhánh develop",
                        "📥 Đầu vào: Nhánh feature/borrow-book đã có trên GitHub. ⚙️ Cơ chế ngầm: Chọn Base: develop <- Compare: feature/borrow-book, điền mô tả những gì đã làm và bấm Create Pull Request. 📤 Đầu ra: Nhóm trưởng review và bấm Merge!",
                        "// Trên giao diện Web GitHub:\n// Base: develop  <--  Compare: feature/borrow-book  --> [Create Pull Request]")
                },
                "Trong quy trình làm việc nhóm với Git Flow trên GitHub, trước khi bạn đẩy nhánh feature/... của mình lên để tạo Pull Request (PR) vào nhánh develop, vì sao bạn nên chạy lệnh git pull origin develop ngay bên trong nhánh feature/... của mình?",
                new List<string>
                {
                    "A. Để lấy những thay đổi mới nhất mà đồng đội vừa gộp vào develop trong lúc bạn đang code; nếu có xảy ra Merge Conflict thì bạn chủ động giải quyết êm xuôi ngay trên nhánh cá nhân của mình trước khi gửi PR cho nhóm trưởng duyệt",
                    "B. Để xóa toàn bộ code trên nhánh develop của nhóm",
                    "C. Để tự động đóng gói web thành phần mềm cài đặt",
                    "D. Vì không chạy lệnh đó thì bàn phím sẽ bị khóa"
                },
                "A",
                "Đây là thói quen của mọi lập trình viên chuyên nghiệp: luôn đồng bộ nhánh develop mới nhất vào nhánh feature của mình và xử lý sạch mọi Conflict dưới máy trước khi đẩy lên tạo Pull Request."
            ),

            // =====================================================================
            // BÀI 5.8
            // =====================================================================
            Build(
                topicId,
                "5.8",
                "Cất tạm code đang làm dở với Git Stash (Chuyển nhánh khẩn cấp không cần Commit rác)",
                "Kiểm thử & Debug",
                "https://git-scm.com/docs/git-stash",
                """
                • **💡 1. Hiểu bản chất của `git stash` qua tình huống thực tế trong 30 giây:**
                  - Giả sử bạn đang đứng ở nhánh `feature/gio-hang`, đang sửa dở dang 4 file (code còn đang gạch đỏ chưa chạy được, chưa thể `commit`).
                  - Đột nhiên nhóm trưởng gọi: *"Việt ơi, nhánh `develop` đang có lỗi gấp ở trang Đăng nhập, cậu chuyển về nhánh `develop` kiểm tra giúp tớ ngay lập tức!"*.
                  - Nếu bạn gõ `git switch develop` ngay lúc đó, Git sẽ chặn lại không cho chuyển (hoặc mang đống code đang lỗi dở sang làm bẩn nhánh `develop`). Mà nếu `git commit` đại đống code đang lỗi đỏ thì lại làm bẩn lịch sử!
                  - **Giải pháp `git stash` (Chiếc ngăn kéo cất đồ tạm thời):** Chỉ cần gõ **`git stash`**, Git sẽ gom toàn bộ những dòng code bạn đang gõ dở cất gọn vào một ngăn kéo bí mật, đưa thư mục của bạn về trạng thái sạch bóng để bạn thoải mái chuyển sang nhánh khác! Khi nào quay lại nhánh cũ, gõ **`git stash pop`** là toàn bộ code đang gõ dở lại hiện ra nguyên vẹn!
                • **🖼️ 2. Sơ đồ Cơ chế "Cất Vào Ngăn Kéo & Lấy Ra Dùng Tiếp" của `Git Stash`:**
                ```
                Đang gõ dở dang ở nhánh A ──► Gõ: git stash (Hoặc: git stash save "Dang lam gio hang")
                                                     │
                                                     ▼ (Code dở được cất vào ngăn kéo Stash Stack, nhánh A sạch tinh!)
                Thoải mái nhảy sang nhánh B sửa việc gấp ──► Quay lại nhánh A
                                                     │
                                                     ▼
                Gõ: git stash pop ◄──────────────────┘ (Lấy code dở từ ngăn kéo trả lại đúng vị trí đang gõ!)
                ```
                • **📊 3. Bảng 4 Lệnh `Git Stash` Cần Nhớ:**
                | Câu lệnh | Công dụng chi tiết |
                | :--- | :--- |
                | **`git stash`** (hoặc `git stash push -m "Ghi chú"`) | Cất toàn bộ các thay đổi đang làm dở vào ngăn kéo tạm, làm sạch `Working Directory`. |
                | **`git stash -u`** (`--include-untracked`) | Cất cả những file vừa mới tạo (`Untracked files`) vào ngăn kéo luôn! |
                | **`git stash list`** | Xem danh sách các gói đồ đang cất trong ngăn kéo (`stash@{0}`, `stash@{1}`...). |
                | **`git stash pop`** | Mở ngăn kéo lấy gói đồ mới nhất trả lại vào code và xóa gói đó khỏi ngăn kéo. |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Cất tạm công việc đang làm dở vào ngăn kéo (git stash -u)",
                        "📥 Đầu vào: Đang sửa dở 3 file và tạo mới 1 file nhưng chưa xong để commit. ⚙️ Cơ chế ngầm: Lưu toàn bộ thay đổi vào ngăn xếp Stash và trả thư mục làm việc về trạng thái sạch của commit gần nhất. 📤 Đầu ra: Có thể chuyển nhánh ngay lập tức.",
                        "git stash push -u -m \"Dang code do phan tinh tien phat\"\n// Working directory lúc này sạch bóng!"),
                    Step(2,
                        "2. Chuyển sang nhánh khác xử lý việc đột xuất",
                        "📥 Đầu vào: Thư mục đã sạch. ⚙️ Cơ chế ngầm: Nhảy sang nhánh develop hoặc tạo nhánh hotfix để sửa lỗi khẩn cấp mà không bị vướng code dở dang. 📤 Đầu ra: Xử lý xong việc gấp.",
                        "git switch develop\n// Kiểm tra, sửa lỗi gấp xong xuôi rồi quay lại nhánh cũ:\ngit switch feature/borrow-book"),
                    Step(3,
                        "3. Lấy lại code đang làm dở ra gõ tiếp (git stash pop)",
                        "📥 Đầu vào: Đã quay về lại nhánh feature/borrow-book. ⚙️ Cơ chế ngầm: Khôi phục toàn bộ 3 file đang sửa dở và 1 file mới tạo về lại nguyên trạng như trước lúc cất. 📤 Đầu ra: Tiếp tục code mượt mà.",
                        "git stash list // Kiểm tra các bản lưu tạm\ngit stash pop  // Bung code ra làm tiếp!")
                },
                "Khi bạn đang viết dở một tính năng (code chưa hoàn chỉnh nên chưa muốn git commit), nhưng bạn cần làm sạch thư mục làm việc ngay lập tức để chuyển sang nhánh khác kiểm tra lỗi hoặc kéo code mới về. Câu lệnh Git nào giúp bạn cất tạm những thay đổi đang làm dở đó đi và lát nữa lấy lại sau?",
                new List<string>
                {
                    "A. Dùng lệnh git stash (để cất tạm) và sau đó dùng git stash pop (để lấy lại làm tiếp)",
                    "B. Dùng lệnh git init",
                    "C. Dùng lệnh git clone",
                    "D. Phải xóa hết code đang viết đi rồi lát nữa ngồi gõ lại từ đầu"
                },
                "A",
                "git stash được thiết kế chuyên biệt cho tình huống cần tạm dừng công việc hiện tại đang dang dở để chuyển ngữ cảnh sang nhánh khác mà không phải tạo ra những Commit lỗi nửa vời."
            ),

            // =====================================================================
            // BÀI 5.9
            // =====================================================================
            Build(
                topicId,
                "5.9",
                "\"Cỗ máy thời gian\" cứu nguy khi làm sai: Phân biệt git restore, git reset (--soft, --hard) và git revert",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/undo",
                """
                • **💡 1. Hiểu bản chất 3 cấp độ "Quay ngược thời gian" khi lỡ tay làm hỏng code:**
                  Trong quá trình làm dự án, chắc chắn sẽ có lúc bạn sửa nhầm làm hỏng hết code, hoặc lỡ tay `commit` nhầm file. Git cung cấp 3 chiếc nút "Hoàn tác" tương ứng với 3 tình huống:
                  1. **Tình huống 1 (Chưa `commit` — Muốn vứt bỏ những gì vừa gõ sai trong file):** Dùng **`git restore <tên_file>`** để trả file đó về nguyên trạng như lúc mới mở máy!
                  2. **Tình huống 2 (Đã `commit` ở dưới máy Local, CHƯA `push` lên GitHub — Muốn hủy commit vừa tạo):** Dùng **`git reset`**.
                  3. **Tình huống 3 (Đã lỡ `push` lên nhánh chung của nhóm trên GitHub rồi — Muốn hủy bỏ thay đổi đó một cách an toàn):** Dùng **`git revert <mã_commit>`**.
                • **🖼️ 2. Sơ đồ Phân Biệt Cực Rõ `git reset --soft` vs `git reset --hard` vs `git revert`:**
                ```
                Lịch sử hiện tại: (C1) ───► (C2: Chạy tốt) ───► (C3: Commit bị lỗi!) ◄── HEAD

                [1. git reset --soft HEAD~1] (Lùi lại 1 mốc, nhưng GIỮ NGUYÊN code vừa gõ của C3 để bạn sửa lại!):
                Lịch sử: (C1) ───► (C2) ◄── HEAD   (Code của C3 vẫn nằm nguyên trong Staging Area, không mất 1 chữ nào!)

                [2. git reset --hard HEAD~1] (CỰC NGUY HIỂM: Lùi về C2 và XÓA SẠCH VĨNH VIỄN toàn bộ code của C3!):
                Lịch sử: (C1) ───► (C2) ◄── HEAD   (Mọi dòng code bạn gõ trong C3 biến mất hoàn toàn!)

                [3. git revert C3] (An toàn cho nhóm: Tạo thêm 1 Commit C4 mới làm ngược lại C3 để triệt tiêu lỗi!):
                Lịch sử: (C1) ───► (C2) ───► (C3: Lỗi) ───► (C4: Revert C3 - Khôi phục lại như C2!) ◄── HEAD
                ```
                • **📊 3. Bảng Tra Cứu Khẩn Cấp Khi Cần Hoàn Tác Trong Git:**
                | Bạn đang gặp sự cố gì? | Lệnh cứu nguy chuẩn xác | Độ an toàn đối với code đang gõ |
                | :--- | :--- | :--- |
                | Sửa nhầm 1 file, chưa commit, muốn trả về như cũ | `git restore TenFile.cs` | Xóa các dòng vừa gõ thêm trong file đó. |
                | Đã `git add .` nhầm file, muốn rút ra khỏi giỏ Staging | `git restore --staged TenFile.cs`| ✅ **An toàn 100%** (Chỉ rút khỏi giỏ chờ, code trong file giữ nguyên). |
                | Vừa `commit` xong ở máy thì phát hiện thiếu file / sai lời nhắn | **`git reset --soft HEAD~1`** | ⭐ **An toàn 100%** (Hủy vỏ commit nhưng giữ nguyên toàn bộ code để sửa và commit lại!). |
                | Commit lỗi đã đẩy lên nhánh chung của cả nhóm trên GitHub | **`git revert <mã_hash>`** | ✅ **Chuẩn mực làm việc nhóm** (Không làm loạn lịch sử của đồng đội). |
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Hủy commit vừa tạo nhưng giữ nguyên 100% code (git reset --soft)",
                        "📥 Đầu vào: Bạn vừa gõ git commit xong (chưa push) thì nhớ ra mình quên sửa 1 dòng nhỏ. ⚙️ Cơ chế ngầm: Lệnh HEAD~1 lùi con trỏ lịch sử lại 1 bước, cờ --soft giữ nguyên toàn bộ code bạn đã viết ở vùng Staging. 📤 Đầu ra: Sửa thêm rồi commit lại gọn gàng.",
                        "git reset --soft HEAD~1\n// Sửa thêm code cho hoàn chỉnh rồi commit lại:\ngit add .\ngit commit -m \"feat: hoan thien day du tinh nang\""),
                    Step(2,
                        "2. Đạo ngược một commit lỗi đã đẩy lên GitHub (git revert)",
                        "📥 Đầu vào: Commit có mã 'a1b2c3d' đã push lên nhánh develop bị phát hiện làm lỗi giao diện. ⚙️ Cơ chế ngầm: Git tự động tạo một Commit mới đảo ngược lại các dòng đã thêm/xóa trong commit 'a1b2c3d'. 📤 Đầu ra: Nhánh develop trở lại bình thường mà không cần xóa lịch sử.",
                        "git log --oneline // Xem mã hash của commit bị lỗi (VD: a1b2c3d)\ngit revert a1b2c3d\ngit push origin develop"),
                    Step(3,
                        "3. Bảo bối cuối cùng khi lỡ tay reset --hard nhầm (git reflog)",
                        "📥 Đầu vào: Lỡ gõ git reset --hard làm mất commit vừa viết suốt 3 tiếng! ⚙️ Cơ chế ngầm: Git vẫn âm thầm ghi nhật ký mọi bước di chuyển của HEAD trong 'git reflog' suốt 30 ngày. 📤 Đầu ra: Cứu sống lại commit tưởng như đã mất!",
                        "git reflog // Hiện ra toàn bộ mã hash của các commit cũ trước khi bị xóa\ngit reset --hard <mã_hash_cũ> // Khôi phục lại nguyên vẹn!")
                },
                "Bạn vừa thực hiện lệnh git commit -m \"Xong bài\" ở trên máy tính cá nhân (chưa push), nhưng ngay sau đó bạn nhận ra mình muốn hủy bỏ lần commit này để sửa thêm vài dòng code nữa rồi mới commit lại, đồng thời TUYỆT ĐỐI KHÔNG MUỐN bị mất đi những dòng code đã vất vả gõ trong commit đó. Bạn phải dùng câu lệnh nào?",
                new List<string>
                {
                    "A. Dùng lệnh: git reset --soft HEAD~1 (Hủy commit cuối cùng nhưng giữ nguyên toàn bộ các thay đổi code của bạn ở trong vùng Staging)",
                    "B. Dùng lệnh: git reset --hard HEAD~1",
                    "C. Xóa thư mục .git đi",
                    "D. Format lại ổ cứng"
                },
                "A",
                "Cần phân biệt cực rõ: --soft chỉ tháo vỏ Commit ra và giữ nguyên toàn bộ code bạn đã gõ; ngược lại --hard sẽ xóa sổ hoàn toàn các dòng code bạn đã viết trong Commit đó!"
            ),

            // =====================================================================
            // BÀI 5.10
            // =====================================================================
            Build(
                topicId,
                "5.10",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 5 (Thực chiến Git Workflow trong dự án nhóm ASP.NET Core)",
                "Kiểm thử & Debug",
                "https://learn.microsoft.com/vi-vn/devops/develop/git/",
                """
                • **💡 1. Cẩm nang "10 Quy tắc Vàng" khi dùng Git trong dự án nhóm पर trường (như dự án Quản lý Thư viện) & khi đi Thực tập:**
                  1. **Luôn có file `.gitignore` chuẩn .NET** (`dotnet new gitignore`) ngay từ giây phút đầu tiên tạo project để chặn `bin/`, `obj/`, `.vs/`.
                  2. **Không bao giờ gõ code trực tiếp trên nhánh `main` hoặc `develop`**. Mỗi chức năng phải tách một nhánh riêng: `git switch -c feature/<tên-chức-năng>`.
                  3. **Buổi sáng mở máy lên:** Luôn kéo code mới nhất của nhóm về trước (`git pull origin develop`).
                  4. **Chia nhỏ các lần `commit`** theo từng đơn vị công việc hoàn chỉnh, đặt lời nhắn rõ nghĩa (`feat: ...`, `fix: ...`).
                  5. **Riêng với Entity Framework Core Migrations trong làm việc nhóm:** Trước khi chạy lệnh `Add-Migration`, hãy luôn `git pull origin develop` về trước để đảm bảo thư mục `Migrations/` của bạn đã có đủ các bảng mới nhất của đồng đội!
                • **🖼️ 2. Sơ đồ Quy Trình Khép Kín 1 Ngày Làm Việc Với Git Của Lập Trình Viên .NET:**
                ```
                [1. Mở máy đầu ca] ──► git switch develop  ──► git pull origin develop
                                            │
                [2. Nhận Task mới] ◄────────┘
                     │
                     ▼
                git switch -c feature/ten-task ──► Viết code C# & Chạy thử F5 ngon lành
                     │
                     ▼
                [3. Đóng gói & Đồng bộ] ──► git add . ──► git commit -m "feat: ..."
                     │
                     ▼
                [4. Kiểm tra đụng độ]   ──► git pull origin develop (Xử lý Conflict nếu có ngay tại máy)
                     │
                     ▼
                [5. Đẩy lên & Tạo PR]   ──► git push -u origin feature/ten-task ──► Lên GitHub tạo Pull Request!
                ```
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Đồng bộ nhánh chung & Tách nhánh tính năng mới",
                        "📥 Đầu vào: Bắt đầu làm chức năng Quản lý Độc giả cho dự án nhóm. ⚙️ Cơ chế ngầm: Cập nhật nhánh develop mới nhất và tách nhánh feature/quan-ly-doc-gia. 📤 Đầu ra: Môi trường code độc lập.",
                        "git switch develop\ngit pull origin develop\ngit switch -c feature/quan-ly-doc-gia"),
                    Step(2,
                        "2. Kiểm tra file thay đổi và đóng gói Commit chuẩn",
                        "📥 Đầu vào: Đã viết xong ReaderController, ReaderRepository và View. ⚙️ Cơ chế ngầm: Kiểm tra git status đảm bảo không dính file rác, đưa vào Staging và tạo Commit. 📤 Đầu ra: Lưu mốc lịch sử dưới máy.",
                        "git status\ngit add .\ngit commit -m \"feat: hoan thien chuc nang them sua xoa doc gia\""),
                    Step(3,
                        "3. Kéo develop về nhánh cá nhân để tự giải quyết Conflict (nếu có)",
                        "📥 Đầu vào: Trong lúc bạn code, bạn cùng nhóm vừa gộp chức năng Quản lý Sách vào develop. ⚙️ Cơ chế ngầm: Kéo origin/develop vào nhánh của bạn, chọn Accept Both Changes cho các đăng ký DI trong Program.cs. 📤 Đầu ra: Code của cả hai chạy mượt cùng nhau.",
                        "git pull origin develop\n// Nếu có Conflict ở Program.cs -> Chọn Accept Both Changes -> git add . -> git commit"),
                    Step(4,
                        "4. Đẩy nhánh lên GitHub và mở Pull Request",
                        "📥 Đầu vào: Nhánh đã tích hợp sạch sẽ. ⚙️ Cơ chế ngầm: Push nhánh lên origin và mở Pull Request trên web GitHub để gộp vào develop. 📤 Đầu ra: Hoàn thành trọn vẹn quy trình chuyên nghiệp!",
                        "git push -u origin feature/quan-ly-doc-gia")
                },
                "(Câu hỏi tổng hợp Chủ đề 5) Khi làm dự án nhóm ASP.NET Core MVC có sử dụng Entity Framework Core (Code-First Migration) trên GitHub, quy tắc nào dưới đây giúp cả nhóm tránh bị lỗi xung đột cấu trúc Database (ModelSnapshot) khi nhiều người cùng tạo bảng mới?",
                new List<string>
                {
                    "A. Trước khi gõ lệnh Add-Migration để tạo bảng mới, luôn phải git pull code mới nhất từ nhánh chung (develop) về và chạy Update-Database trước để máy mình cập nhật đầy đủ các Migration trước đó của đồng đội",
                    "B. Mỗi người tự xóa thư mục Migrations của người khác đi rồi tạo lại từ đầu",
                    "C. Đưa thư mục Migrations vào file .gitignore để không ai nhìn thấy bảng của nhau",
                    "D. Không bao giờ chạy lệnh Update-Database"
                },
                "A",
                "Trong EF Core, file AppDbContextModelSnapshot.cs lưu trạng thái tổng hợp của toàn bộ Database theo trình tự thời gian. Do đó trước khi tạo một Migration mới, bạn luôn phải git pull để lấy Snapshot mới nhất của nhóm về trước."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 10 bài học con (5.1 -> 5.10) cho Chủ đề 5 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic5 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 5);
        if (topic5 == null)
            return;

        topic5.Title = "Chủ đề 5: Quản lý source code với Git";
        topic5.Description = "10 Bài học con siêu chi tiết từ 3 Vùng trạng thái Git (Working Directory, Staging, Repository), .gitignore chuẩn .NET, Remote GitHub (clone, push, pull, fetch), Branching (switch/checkout), Merge vs Rebase, xử lý Merge Conflict, Git Flow & Pull Request, Git Stash đến git restore/reset/revert.";

        var targetLessons = GetLessons(topic5.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic5.Id)
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
