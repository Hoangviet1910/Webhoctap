using System.Text.Json;
using HoangViet.Models;
using Microsoft.EntityFrameworkCore;

namespace HoangViet.Data;

public static class DetailedTopic2Seed
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
            // BÀI 2.1
            // =====================================================================
            Build(
                topicId,
                "2.1",
                "Tư duy Hướng đối tượng (OOP), Lớp (Class) và Đối tượng (Object)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/types/classes",
                """
                • 1. Tại sao phải sinh ra Lập trình Hướng đối tượng (OOP)?
                  - Khi viết chương trình nhỏ vài chục dòng, bạn chỉ cần viết các hàm nối tiếp nhau (Lập trình hướng thủ tục). Nhưng khi làm một hệ thống lớn (như Web bán hàng có Khách hàng, Sản phẩm, Đơn hàng, Giỏ hàng), nếu để các biến và hàm nằm rời rạc sẽ cực kỳ hỗn loạn.
                  - OOP (Object-Oriented Programming) giúp bạn gom nhóm Dữ liệu (Đặc điểm) và Hành vi (Hành động) của một sự vật ngoài đời thực vào chung một khối độc lập.
                • 2. Phân biệt Lớp (Class) và Đối tượng (Object):
                  - Class (Lớp - Bản thiết kế / Khuôn đúc): Là bản vẽ trên giấy quy định một sinh viên sẽ có những thông tin gì (MaSV, HoTen, NamSinh) và làm được hành động gì (DangKyMonHoc(), XemDiem()). Bản thân Class chưa chiếm chỗ lưu dữ liệu thật.
                  - Object (Đối tượng - Thực thể cụ thể): Là sản phẩm thật được đúc ra từ cái khuôn Class nhờ từ khóa new. Ví dụ từ 1 bản vẽ class Student, bạn dùng new đúc ra 3 đối tượng sinh viên cụ thể là "Hoàng Việt", "Minh An", "Tuấn Kiệt" nằm trên vùng nhớ Heap.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Thiết kế khuôn mẫu (Khai báo Class)",
                        "Định nghĩa các đặc điểm (Properties) và hành động (Methods) chung cho mọi chiếc xe hơi.",
                        "public class Car {\n    public string Brand { get; set; }\n    public int MaxSpeed { get; set; }\n    public void Drive() => Console.WriteLine($\"Xe {Brand} đang chạy!\");\n}"),
                    Step(2,
                        "2. Đúc ra đối tượng thật bằng từ khóa new",
                        "Từ khóa 'new' cấp phát một vùng nhớ trên Heap để tạo ra đối tượng cụ thể từ bản vẽ Car.",
                        "Car myCar = new Car();\nmyCar.Brand = \"VinFast VF8\";\nmyCar.MaxSpeed = 200;"),
                    Step(3,
                        "3. Tạo nhiều Object độc lập từ 1 Class",
                        "Từ 1 Class duy nhất có thể tạo ra hàng nghìn Object khác nhau, mỗi Object giữ dữ liệu riêng của mình.",
                        "Car secondCar = new Car { Brand = \"Mazda 3\", MaxSpeed = 180 };\nmyCar.Drive();     // In ra: Xe VinFast VF8 đang chạy!\nsecondCar.Drive(); // In ra: Xe Mazda 3 đang chạy!")
                },
                "Sự khác biệt cốt lõi giữa Class (Lớp) và Object (Đối tượng) trong C# là gì?",
                new List<string>
                {
                    "A. Class là bản thiết kế (khuôn mẫu) định nghĩa cấu trúc chung, còn Object là một thực thể cụ thể được khởi tạo từ Class đó (bằng từ khóa new) và chiếm vùng nhớ thực tế trên Heap",
                    "B. Object là bản thiết kế, còn Class là biến được tạo ra từ Object",
                    "C. Mỗi Class chỉ được phép tạo ra tối đa 1 Object duy nhất",
                    "D. Class và Object hoàn toàn giống hệt nhau"
                },
                "A",
                "Class đóng vai trò như một kiểu dữ liệu do người dùng tự định nghĩa (bản thiết kế), còn Object là thể hiện (Instance) cụ thể sinh ra từ bản thiết kế đó khi chạy chương trình."
            ),

            // =====================================================================
            // BÀI 2.2
            // =====================================================================
            Build(
                topicId,
                "2.2",
                "Trường dữ liệu (Field), Thuộc tính (Property với get, set, init) và Tính Đóng gói (Encapsulation)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/properties",
                """
                • 1. Tính Đóng gói (Encapsulation) là gì?
                  - Hãy tưởng tượng chiếc thẻ ngân hàng của bạn: Số dư tài khoản nằm bên trong con chip được khóa kín (private), không ai được phép lấy bút xóa điền trực tiếp số tiền âm -50000 vào đó. Mọi thao tác xem số dư hay rút tiền đều phải đi qua cổng kiểm soát của cây ATM (public). Đó chính là Tính Đóng gói: Che giấu dữ liệu nội bộ bên trong đối tượng và chỉ cho phép bên ngoài tương tác thông qua các cổng kiểm soát an toàn.
                • 2. Phân biệt Field (Trường dữ liệu) và Property (Thuộc tính):
                  - Field (Biến nội bộ): Thường khai báo là private decimal _balance; (quy ước có dấu gạch dưới _ ở đầu tên). Dùng để cất giữ dữ liệu thô bên trong class.
                  - Property (Cổng giao tiếp có get và set): Khai báo là public để bên ngoài chấm truy cập:
                    + Khối get: Chạy khi bên ngoài muốn đọc/lấy giá trị ra.
                    + Khối set: Chạy khi bên ngoài muốn gán/sửa giá trị mới vào (thông qua từ khóa ẩn value). Bạn có thể viết if (value < 0) ngay trong set để chặn dữ liệu sai!
                    + Khối init (C# hiện đại): Thay cho set ({ get; init; }) — Chỉ cho phép gán giá trị đúng 1 lần lúc mới khởi tạo đối tượng, sau đó khóa cứng lại không cho ai sửa nữa!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Auto-Property (Viết gọn khi không cần kiểm tra)",
                        "Cách viết phổ biến nhất chiếm 90% trong C#: Trình biên dịch tự tạo biến private ngầm bên dưới cho bạn.",
                        "public class Product {\n    public string Name { get; set; } // Cho phép đọc và sửa thoải mái\n    public Guid Id { get; init; }    // Chỉ cho gán lúc new, sau đó khóa cứng\n}"),
                    Step(2,
                        "2. Full-Property (Có kiểm soát dữ liệu tại cổng set)",
                        "Kết hợp biến private _price và cổng public Price để chặn không cho ai gán giá tiền âm.",
                        "private decimal _price;\npublic decimal Price {\n    get { return _price; }\n    set {\n        if (value < 0) throw new ArgumentException(\"Giá không được âm!\");\n        _price = value;\n    }\n}"),
                    Step(3,
                        "3. Thuộc tính chỉ đọc (Read-only Property)",
                        "Chỉ có hàm get (hoặc dùng mũi tên =>) để tự động tính toán từ các thuộc tính khác.",
                        "public int Quantity { get; set; }\npublic decimal TotalAmount => Price * Quantity; // Chỉ đọc, tự động nhân")
                },
                "Trong C# hiện đại, khi một thuộc tính được khai báo là public string StudentCode { get; init; }, từ khóa init mang lại tác dụng gì?",
                new List<string>
                {
                    "A. Cho phép sửa đổi giá trị StudentCode ở bất kỳ đâu trong chương trình",
                    "B. Chỉ cho phép gán giá trị cho StudentCode trong lúc khởi tạo đối tượng (Constructor hoặc Object Initializer), sau khi khởi tạo xong thuộc tính này trở thành bất biến (không thể sửa đổi)",
                    "C. Tự động xóa thuộc tính đó khỏi Database",
                    "D. Biến thuộc tính đó thành kiểu số nguyên"
                },
                "B",
                "init (Init-only setter) giúp tạo ra các thuộc tính bất biến (Immutable): bạn vẫn dùng cú pháp new Student { StudentCode = \"SV01\" } rất tiện lợi, nhưng sau dòng đó không ai có thể gán đè sv.StudentCode = \"SV02\" được nữa."
            ),

            // =====================================================================
            // BÀI 2.3
            // =====================================================================
            Build(
                topicId,
                "2.3",
                "Toàn bộ 5 Phạm vi truy cập (Access Modifiers: public, private, protected, internal, protected internal)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers",
                """
                • 1. Tại sao cần Phạm vi truy cập (Access Modifiers)?
                  - Để thực hiện Tính Đóng gói, C# cung cấp các từ khóa đặt ở đầu class, field, property hoặc method nhằm quy định: "Ai được phép nhìn thấy và gọi thành phần này?".
                • 2. Bảng phân quyền 5 cấp độ từ kín đáo nhất đến mở rộng nhất:
                  1. private (Riêng tư tuyệt đối): Chỉ gọi được bên trong dấu ngoặc nhọn {} của chính Class đó. Ra ngoài class (kể cả lớp con kế thừa) cũng không nhìn thấy. (Lưu ý: Nếu bạn khai báo biến/hàm trong class mà quên viết từ khóa phạm vi, mặc định C# sẽ đặt là private).
                  2. protected (Gia sản truyền cho con cháu): Gọi được bên trong Class đó VÀ bên trong các Class con kế thừa (Inheritance) từ nó. Người ngoài không kế thừa thì không thấy.
                  3. internal (Nội bộ trong cùng 1 Project): Gọi thoải mái ở mọi file .cs miễn là nằm trong cùng 1 Project (.csproj). Sang Project khác trong cùng Solution sẽ không gọi được. (Mặc định của class khi không ghi gì là internal).
                  4. protected internal: Hợp của protected HOẶC internal (Cùng project gọi được, hoặc lớp con ở project khác cũng gọi được).
                  5. public (Công khai hoàn toàn): Không giới hạn! Bất kỳ file nào, bất kỳ project nào cũng nhìn thấy và chấm gọi được.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Mức 1: private (Chỉ nội bộ trong 1 Class)",
                        "Giấu các biến nhạy cảm hoặc hàm tính toán phụ trợ không muốn bên ngoài chạm vào.",
                        "public class BankAccount {\n    private string _pinCode = \"123456\"; // Bên ngoài không thể chấm ._pinCode\n}"),
                    Step(2,
                        "Mức 2: protected (Chỉ dành cho Lớp cha & Lớp con)",
                        "Lớp cha chia sẻ biến/hàm cho các lớp con kế thừa sử dụng mà vẫn giấu kín với người ngoài.",
                        "public class BaseEntity {\n    protected void UpdateTimestamp() { ... } // Chỉ lớp con kế thừa mới gọi được\n}"),
                    Step(3,
                        "Mức 3: internal & public (Cấp độ Project & Toàn cục)",
                        "internal giới hạn trong 1 Project (.dll); public mở cửa cho toàn bộ hệ thống.",
                        "internal class InternalHelper { } // Chỉ dùng trong project hiện tại\npublic class UserService { }      // Project khác tham chiếu tới dùng được ngay")
                },
                "Khi bạn tạo một lớp con public class Manager : Employee (kế thừa từ lớp Employee), những thành viên (biến/hàm) có phạm vi truy cập nào trong lớp cha Employee mà lớp con Manager có thể truy cập trực tiếp được (giả sử cùng 1 project)?",
                new List<string>
                {
                    "A. Chỉ truy cập được các thành viên private",
                    "B. Truy cập được các thành viên public, protected và internal; KHÔNG truy cập trực tiếp được thành viên private của lớp cha",
                    "C. Không truy cập được bất kỳ thành viên nào của lớp cha",
                    "D. Chỉ truy cập được các hàm static"
                },
                "B",
                "Thành viên private bị khóa chặt trong phạm vi lớp cha Employee. Lớp con kế thừa được phép sử dụng các thành viên protected (dành riêng cho kế thừa), internal (nếu cùng project) và public."
            ),

            // =====================================================================
            // BÀI 2.4
            // =====================================================================
            Build(
                topicId,
                "2.4",
                "Phương thức khởi tạo (Constructor), Nạp chồng Constructor và Cơ chế dọn rác (Garbage Collection)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/constructors",
                """
                • 1. Phương thức khởi tạo (Constructor) là gì?
                  - Là một hàm đặc biệt sẽ TỰ ĐỘNG CHẠY NGAY LẬP TỨC tại thời điểm bạn gõ từ khóa new Tên_Class().
                  - 3 dấu hiệu nhận diện Constructor:
                    1. Tên hàm trùng khớp 100% với tên Class.
                    2. Không có kiểu trả về (không có cả chữ void).
                    3. Dùng để gán giá trị mặc định ban đầu hoặc bắt buộc người dùng phải truyền đủ thông tin cần thiết khi đúc đối tượng.
                • 2. Các loại Constructor:
                  - Constructor mặc định (Không tham số): Nếu bạn không viết Constructor nào, C# tự sinh ngầm 1 hàm public Product() {}.
                  - Constructor có tham số & Nạp chồng (Overloading): Một class có thể có nhiều Constructor cùng tên nhưng khác số lượng tham số. Dùng từ khóa : this(...) để Constructor này gọi lại Constructor kia nhằm tránh lặp code.
                • 3. Cơ chế dọn rác tự động (Garbage Collector - GC):
                  - Trong C/C++, tạo đối tượng xong phải tự viết code xóa khỏi RAM nếu không sẽ tràn bộ nhớ. Còn trong C# (.NET), bộ phận Garbage Collector (GC) của CLR sẽ tự động tuần tra vùng nhớ Heap: hễ thấy đối tượng nào không còn biến nào trỏ tới nữa, GC sẽ tự động thu hồi vùng nhớ đó giúp bạn!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Constructor mặc định (Gán giá trị khởi đầu)",
                        "Tự động gán ngày tạo và mã Guid ngay khi đối tượng vừa được new ra.",
                        "public class Order {\n    public Guid Id { get; set; }\n    public DateTime CreatedAt { get; set; }\n\n    public Order() {\n        Id = Guid.NewGuid();\n        CreatedAt = DateTime.Now;\n    }\n}"),
                    Step(2,
                        "Constructor có tham số (Bắt buộc truyền dữ liệu)",
                        "Ép người gọi phải truyền Tên và Giá ngay trong ngoặc tròn lúc khởi tạo.",
                        "public class Product {\n    public string Name { get; set; }\n    public decimal Price { get; set; }\n\n    public Product(string name, decimal price) {\n        Name = name;\n        Price = price;\n    }\n}"),
                    Step(3,
                        "Gọi nối tiếp Constructor bằng từ khóa : this(...)",
                        "Constructor ít tham số ủy quyền gọi sang Constructor nhiều tham số.",
                        "// Nếu chỉ truyền tên, mặc định gán giá = 0:\npublic Product(string name) : this(name, 0m) {\n}")
                },
                "Đặc điểm cú pháp nào dưới đây dùng để nhận diện một phương thức là Constructor (Hàm khởi tạo) trong C#?",
                new List<string>
                {
                    "A. Phải có từ khóa void đứng trước tên hàm",
                    "B. Có tên trùng khớp hoàn toàn với tên của Class chứa nó và tuyệt đối không khai báo kiểu trả về (kể cả void)",
                    "C. Luôn phải đặt tên là Main()",
                    "D. Bắt buộc phải là hàm private"
                },
                "B",
                "Constructor luôn mang tên giống hệt tên Class và không có kiểu trả về, được kích hoạt tự động mỗi khi toán tử new được gọi."
            ),

            // =====================================================================
            // BÀI 2.5
            // =====================================================================
            Build(
                topicId,
                "2.5",
                "Tính Kế thừa (Inheritance: Lớp cha - Lớp con, từ khóa base và lớp niêm phong sealed)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/object-oriented/inheritance",
                """
                • 1. Bản chất của Tính Kế thừa (Inheritance):
                  - Trong một hệ thống có SinhVien, GiangVien, NhanVien. Cả 3 đều có chung Id, HoTen, NgaySinh, Email. Thay vì copy-paste 4 thuộc tính đó vào cả 3 class, bạn tạo ra một Lớp cha (Base Class / Parent Class) tên là Person chứa các điểm chung đó.
                  - Sau đó cho 3 Lớp con (Derived Class / Child Class) kế thừa từ Person bằng dấu hai chấm : (public class SinhVien : Person). Lập tức SinhVien có sẵn toàn bộ tài sản của Person và chỉ cần viết thêm thuộc tính riêng là DiemGPA!
                • 2. Quy tắc vàng của Kế thừa trong C#:
                  - Đơn kế thừa đối với Class: Một class trong C# chỉ được phép kế thừa trực tiếp từ DUY NHẤT 1 class cha (nhưng có thể thực thi nhiều Interface).
                  - Từ khóa base: Dùng ở lớp con khi muốn gọi lên Constructor hoặc hàm của lớp cha (VD: public SinhVien(string name) : base(name)).
                  - Từ khóa sealed (Niêm phong): Nếu khai báo public sealed class Admin, nghĩa là khóa sổ — cấm tất cả các class khác kế thừa từ Admin (dùng để bảo mật hoặc tối ưu tốc độ).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Lớp cha (Base Class) chứa phần chung",
                        "Gom các thuộc tính mà bảng nào cũng có (Id, CreatedAt, IsDeleted) vào BaseEntity.",
                        "public class BaseEntity {\n    public int Id { get; set; }\n    public DateTime CreatedAt { get; set; } = DateTime.Now;\n}"),
                    Step(2,
                        "2. Lớp con kế thừa bằng dấu hai chấm (:)",
                        "Class Product kế thừa BaseEntity nên tự động có sẵn Id và CreatedAt mà không cần khai báo lại.",
                        "public class Product : BaseEntity {\n    public string Name { get; set; }\n    public decimal Price { get; set; }\n}"),
                    Step(3,
                        "3. Gọi Constructor lớp cha bằng : base(...)",
                        "Chuyển tham số từ Constructor của lớp con lên cho Constructor của lớp cha xử lý.",
                        "public class Animal {\n    public string Name { get; set; }\n    public Animal(string name) { Name = name; }\n}\npublic class Dog : Animal {\n    public Dog(string name) : base(name) { } // Đẩy name lên cho Animal\n}")
                },
                "Trong ngôn ngữ C#, một Class có thể kế thừa trực tiếp từ tối đa bao nhiêu Class cha, và từ khóa nào dùng để ngăn chặn không cho phép bất kỳ lớp nào khác kế thừa từ một Class?",
                new List<string>
                {
                    "A. Kế thừa được tối đa 1 Class cha duy nhất (Đơn kế thừa class); và dùng từ khóa sealed để chặn kế thừa",
                    "B. Kế thừa được 10 Class cha cùng lúc; và dùng từ khóa static để chặn kế thừa",
                    "C. Không giới hạn số lượng Class cha; và dùng từ khóa private",
                    "D. Chỉ kế thừa được từ Interface chứ không kế thừa được từ Class"
                },
                "A",
                "C# chỉ hỗ trợ đơn kế thừa đối với Class (mỗi lớp chỉ có tối đa 1 lớp cha trực tiếp để tránh lỗi kim cương Diamond Problem), và từ khóa sealed dùng để niêm phong một lớp không cho phép kế thừa tiếp."
            ),

            // =====================================================================
            // BÀI 2.6
            // =====================================================================
            Build(
                topicId,
                "2.6",
                "Tính Đa hình (Polymorphism: Nạp chồng Overloading và Ghi đè Overriding với virtual / override)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/object-oriented/polymorphism",
                """
                • 1. Hiểu đơn giản Tính Đa hình (Polymorphism) là gì?
                  - Đa hình nghĩa là "Cùng một mệnh lệnh được phát ra, nhưng mỗi đối tượng khác nhau sẽ thực hiện theo cách riêng của nó". Ví dụ: Cùng ra lệnh TinhLuong(), nhưng đối tượng NhanVienFullTime thì tính Lương cứng + Thưởng, còn NhanVienPartTime thì tính Số giờ làm * 25.000đ.
                • 2. Hai dạng Đa hình trong C# (Câu hỏi phỏng vấn kinh điển):
                  1. Đa hình lúc biên dịch - Method Overloading (Nạp chồng hàm):
                     - Nằm trong cùng 1 Class, có nhiều hàm đặt trùng tên nhau nhưng khác nhau về số lượng hoặc kiểu dữ liệu của tham số (VD: Add(int a, int b) và Add(double a, double b, double c)).
                  2. Đa hình lúc chạy - Method Overriding (Ghi đè hàm giữa Lớp cha và Lớp con):
                     - Ở Lớp cha: Phải gắn từ khóa virtual lên hàm (ý nói: "Đây là cách tính mặc định của cha, con nào muốn sửa lại theo ý mình thì cứ việc!").
                     - Ở Lớp con: Dùng từ khóa override để viết lại nội dung mới cho hàm đó (giữ nguyên tên và tham số).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Nạp chồng hàm (Overloading) trong cùng 1 Class",
                        "Cùng tên hàm Print nhưng truyền số nguyên chạy hàm trên, truyền chuỗi chạy hàm dưới.",
                        "public class Printer {\n    public void Print(int number) => Console.WriteLine($\"Số: {number}\");\n    public void Print(string text) => Console.WriteLine($\"Chữ: {text}\");\n}"),
                    Step(2,
                        "2. Lớp cha mở quyền ghi đè bằng từ khóa virtual",
                        "Khai báo phương thức CalculateBonus() là virtual để cho phép các lớp con tùy biến.",
                        "public class Employee {\n    public virtual decimal CalculateBonus() => 500000m; // Mặc định thưởng 500k\n}"),
                    Step(3,
                        "3. Lớp con ghi đè bằng từ khóa override",
                        "Lớp Manager sửa lại công thức tính thưởng riêng mà vẫn giữ nguyên tên hàm.",
                        "public class Manager : Employee {\n    public override decimal CalculateBonus() => 2000000m; // Quản lý thưởng 2 triệu\n}")
                },
                "Để một lớp con có thể sử dụng từ khóa override nhằm viết đè lại logic của một phương thức từ lớp cha, phương thức đó ở lớp cha bắt buộc phải được khai báo với từ khóa nào?",
                new List<string>
                {
                    "A. Từ khóa static",
                    "B. Từ khóa virtual (hoặc abstract)",
                    "C. Từ khóa private",
                    "D. Từ khóa const"
                },
                "B",
                "Trong C#, mặc định các phương thức không cho phép ghi đè. Lớp cha phải chủ động đánh dấu phương thức là virtual (có thân hàm mặc định) hoặc abstract (chưa có thân hàm) thì lớp con mới dùng override được."
            ),

            // =====================================================================
            // BÀI 2.7
            // =====================================================================
            Build(
                topicId,
                "2.7",
                "Tính Trừu tượng (Abstraction): Phân biệt bản chất Abstract Class và Interface",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/language-reference/keywords/interface",
                """
                • 1. Tính Trừu tượng (Abstraction) là gì?
                  - Là việc chỉ nêu ra "Cần làm những việc gì" (Danh sách đầu mục công việc) mà ẩn đi "Chi tiết làm như thế nào", bắt các lớp con bên dưới phải tự viết code thực thi chi tiết.
                • 2. Bảng so sánh Interface vs Abstract Class (Câu hỏi phỏng vấn xuất hiện ở 99% công ty):
                  - Interface (Bản hợp đồng giao diện - Tên luôn bắt đầu bằng chữ I, VD: IPaymentService):
                    + Chỉ chứa danh sách chữ ký hàm (Tên hàm, đầu vào, đầu ra) chứ không chứa biến dữ liệu (fields).
                    + Đa thực thi: Một Class có thể ký hợp đồng thực thi NHIỀU Interface cùng lúc (VD: class User : ILogin, IExportExcel, ISendMail).
                    + Khi nào dùng? Dùng làm cầu nối giữa các tầng (IProductService, IProductRepository) trong Dependency Injection!
                  - Abstract Class (Lớp cha trừu tượng - Khai báo bằng abstract class):
                    + Vừa chứa được các hàm trừu tượng (abstract method - rỗng ruột) bắt con phải override, vừa chứa được các biến dữ liệu và các hàm đã viết sẵn code dùng chung cho các con.
                    + Đơn kế thừa: Một Class con chỉ kế thừa được 1 Abstract Class duy nhất.
                    + (Lưu ý: Cả Interface và Abstract Class đều KHÔNG THỂ khởi tạo trực tiếp bằng từ khóa new!).
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo bản hợp đồng Interface",
                        "Chỉ liệt kê tên hàm và kiểu trả về, không viết thân hàm {}.",
                        "public interface IPaymentGateway {\n    bool ProcessPayment(decimal amount);\n}"),
                    Step(2,
                        "2. Nhiều Class khác nhau cùng thực thi 1 Interface",
                        "VnPay và MoMo cùng ký hợp đồng IPaymentGateway nhưng mỗi bên xử lý theo cách riêng.",
                        "public class VnPayService : IPaymentGateway {\n    public bool ProcessPayment(decimal amount) { /* Gọi cổng VNPAY */ return true; }\n}\npublic class MomoService : IPaymentGateway {\n    public bool ProcessPayment(decimal amount) { /* Gọi ví MoMo */ return true; }\n}"),
                    Step(3,
                        "3. Khai báo Abstract Class (Vừa có sẵn code, vừa bắt buộc override)",
                        "Chia sẻ thuộc tính chung và ép lớp con phải tự viết hàm xuất báo cáo.",
                        "public abstract class ReportBase {\n    public string Title { get; set; } // Thuộc tính có sẵn\n    public abstract void Export();    // Hàm rỗng bắt buộc lớp con phải override\n}")
                },
                "Điểm khác biệt quan trọng nhất giữa Abstract Class và Interface khi một Class thực hiện kế thừa/thực thi chúng trong C# là gì?",
                new List<string>
                {
                    "A. Một Class chỉ có thể kế thừa từ tối đa 1 Abstract Class, nhưng có thể thực thi (implement) nhiều Interface cùng một lúc",
                    "B. Interface có thể khởi tạo trực tiếp bằng từ khóa new, còn Abstract Class thì không",
                    "C. Abstract Class không được phép chứa biến hay thuộc tính nào",
                    "D. Chỉ có Abstract Class mới dùng được trong ASP.NET Core"
                },
                "A",
                "Nhờ khả năng một Class thực thi được nhiều Interface cùng lúc và tính lỏng lẻo (Loose Coupling), Interface trở thành công cụ tiêu chuẩn để thiết kế Dependency Injection giữa các tầng trong .NET."
            ),

            // =====================================================================
            // BÀI 2.8
            // =====================================================================
            Build(
                topicId,
                "2.8",
                "Quan hệ Has-A (Composition - Chứa nhau) vs Is-A (Inheritance - Kế thừa) trong thiết kế phần mềm",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/standard/design-guidelines/",
                """
                • 1. Sai lầm của người mới học OOP: Cứ muốn dùng hàm của class khác là đi Kế thừa (:)!
                  - Không phải lúc nào cũng dùng Kế thừa. Trong thiết kế phần mềm thực tế có 2 mối quan hệ cốt lõi bạn phải phân biệt rõ:
                • 2. Quan hệ Is-A (Là một -> Dùng Kế thừa :):
                  - Khi đối tượng A thực sự LÀ MỘT phân loại con của đối tượng B về mặt bản chất.
                  - Ví dụ: Chó là một Động vật (Dog : Animal); ProductsController là một ControllerBase (ProductsController : ControllerBase).
                • 3. Quan hệ Has-A (Có chứa một -> Dùng Composition / Tiêm vào bên trong):
                  - Khi đối tượng A sử dụng / sở hữu đối tượng B như một bộ phận hoặc công cụ giúp việc.
                  - Ví dụ: Chiếc Ô tô có chứa một Động cơ (chứ Ô tô không phải là Động cơ!).
                  - Trong dự án Web API: OrderController có chứa một IOrderService (khai báo biến private readonly IOrderService _orderService; ở bên trong class) để nhờ OrderService xử lý đơn hàng. Đó chính là quan hệ Has-A (Composition) — linh hoạt và dễ thay thế hơn Kế thừa rất nhiều!
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "Quan hệ Is-A (Kế thừa bằng dấu :)",
                        "Dùng khi lớp con mang bản chất của lớp cha (VipCustomer là một Customer).",
                        "public class Customer { public int Id { get; set; } }\npublic class VipCustomer : Customer { public int VipLevel { get; set; } }"),
                    Step(2,
                        "Quan hệ Has-A (Chứa đối tượng khác làm thuộc tính/field)",
                        "Chiếc xe Car chứa một bộ phận Engine bên trong nó để gọi hàm nổ máy.",
                        "public class Engine {\n    public void Start() => Console.WriteLine(\"Động cơ nổ máy!\");\n}\npublic class Car {\n    private readonly Engine _engine = new Engine(); // Car HAS-A Engine\n    public void Run() => _engine.Start();\n}"),
                    Step(3,
                        "Ứng dụng Has-A trong Kiến trúc Controller - Service",
                        "Controller chứa IProductService bên trong để sai vặt chứ không bao giờ kế thừa từ ProductService!",
                        "public class ProductController : ControllerBase { // Is-A ControllerBase\n    private readonly IProductService _service;    // Has-A IProductService\n}")
                },
                "Trong kiến trúc ASP.NET Core, tại sao ProductController lại khai báo một biến private readonly IProductService _productService; ở bên trong nó (Quan hệ Has-A / Composition) thay vì viết public class ProductController : ProductService (Quan hệ Is-A / Kế thừa)?",
                new List<string>
                {
                    "A. Vì về mặt bản chất Controller không phải là một Service, nó chỉ cần 'sử dụng' một hoặc nhiều Service khác nhau để xử lý công việc; ngoài ra C# chỉ cho kế thừa 1 class và ProductController đã phải kế thừa từ ControllerBase rồi",
                    "B. Vì C# cấm kế thừa từ các class tự viết",
                    "C. Vì dùng kế thừa sẽ làm xóa mất dữ liệu trong SQL",
                    "D. Hai cách hoàn toàn giống nhau"
                },
                "A",
                "Một Controller có thể cần gọi cùng lúc 3-4 Service (IProductService, ICategoryService, ILogger). Vì C# chỉ cho đơn kế thừa class (đã dùng cho : ControllerBase), nên việc chứa các Service bên trong theo quan hệ Has-A (Composition kết hợp DI) là thiết kế chuẩn mực duy nhất."
            ),

            // =====================================================================
            // BÀI 2.9
            // =====================================================================
            Build(
                topicId,
                "2.9",
                "Từ khóa static (Lớp tĩnh, Hàm tĩnh, Biến tĩnh), partial class và Extension Methods",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/programming-guide/classes-and-structs/static-classes-and-static-class-members",
                """
                • 1. Từ khóa static (Tĩnh - Thuộc về bản thân Class chứ không thuộc về từng Object lẻ):
                  - Bình thường muốn gọi hàm, bạn phải đúc đối tượng bằng new trước (var sv = new Student(); sv.Study();).
                  - Nhưng nếu một hàm hoặc biến có gắn chữ static, bạn KHÔNG CẦN (và không được) dùng new, mà lấy Tên_Class chấm trực tiếp luôn! (Ví dụ: Math.Sqrt(9), Console.WriteLine(), Guid.NewGuid()).
                  - Biến static: Dùng chung cho toàn bộ các đối tượng của class đó trên toàn chương trình.
                  - static class: Lớp chỉ toàn chứa các hàm tiện ích dùng chung (Helper / Utility), cấm dùng từ khóa new.
                • 2. Extension Methods (Phương thức mở rộng):
                  - Giúp bạn "độ" thêm hàm mới vào một kiểu dữ liệu có sẵn (như string, int hoặc class của thư viện khác) mà không cần sửa code gốc của kiểu đó.
                  - Công thức: Viết một static method nằm trong một static class, và đặt từ khóa this ở ngay trước kiểu dữ liệu của tham số đầu tiên!
                • 3. partial class (Lớp ghép mảnh):
                  - Cho phép tách một Class lớn ra thành 2 hoặc nhiều file .cs khác nhau (đều ghi public partial class TenClass). Khi biên dịch, C# tự động khâu các file đó lại thành 1 class hoàn chỉnh.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo hàm tiện ích với static class",
                        "Gọi trực tiếp qua tên Class mà không tốn bộ nhớ khởi tạo đối tượng bằng new.",
                        "public static class CurrencyHelper {\n    public static string ToVnd(decimal amount) => $\"{amount:N0} VNĐ\";\n}\n// Cách gọi: string s = CurrencyHelper.ToVnd(50000);"),
                    Step(2,
                        "2. Nâng cấp thành Extension Method bằng từ khóa this",
                        "Thêm chữ 'this' vào trước tham số 'decimal amount' để biến mọi con số decimal đều tự có hàm .ToVnd().",
                        "public static class DecimalExtensions {\n    public static string ToVnd(this decimal amount) => $\"{amount:N0} VNĐ\";\n}\n// Cách gọi cực gọn: decimal price = 50000; string s = price.ToVnd();"),
                    Step(3,
                        "3. Tách file gọn gàng với partial class",
                        "Hai file .cs cùng khai báo partial class User sẽ được gộp làm một khi chạy.",
                        "// File 1 (User.Info.cs): public partial class User { public int Id { get; set; } }\n// File 2 (User.Auth.cs): public partial class User { public string Password { get; set; } }")
                },
                "Điều kiện bắt buộc để tạo ra một Extension Method (Phương thức mở rộng) trong C# là gì?",
                new List<string>
                {
                    "A. Phương thức đó phải là static, nằm bên trong một static class, và tham số đầu tiên của phương thức phải có từ khóa 'this' đứng trước kiểu dữ liệu cần mở rộng",
                    "B. Phương thức đó phải nằm trong một interface",
                    "C. Phương thức đó phải có từ khóa virtual",
                    "D. Phải kế thừa từ lớp Object"
                },
                "A",
                "Quy tắc bắt buộc của C# đối với Extension Method là: public static class + public static ReturnType MethodName(this TargetType obj, ...)."
            ),

            // =====================================================================
            // BÀI 2.10
            // =====================================================================
            Build(
                topicId,
                "2.10",
                "So sánh Class, Struct và Record trong C# .NET hiện đại (Khi nào dùng cái nào?)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/types/records",
                """
                • Trong .NET hiện đại (C# 10 / 12 / .NET 10), ngoài class ra bạn sẽ gặp struct và đặc biệt là record. Sự khác nhau giữa bộ ba này là gì?
                • 1. class (Kiểu Tham chiếu - Reference Type - Lưu trên Heap):
                  - Hỗ trợ đầy đủ Kế thừa, Đa hình.
                  - Khi so sánh obj1 == obj2: Mặc định nó so sánh địa chỉ vùng nhớ (dù 2 đối tượng có tên và tuổi giống hệt nhau nhưng nằm ở 2 ô nhớ khác nhau trên Heap thì obj1 == obj2 vẫn trả về false!).
                  - Dùng khi: Làm Entity (Bảng DB), Controller, Service, Repository.
                • 2. record (Kiểu Tham chiếu bất biến chuyên trị dữ liệu - Ra đời từ C# 9+):
                  - Viết siêu ngắn gọn chỉ trên 1 dòng duy nhất: public record ProductDto(int Id, string Name, decimal Price);.
                  - So sánh theo Giá trị (Value-based Equality): Nếu 2 biến record có dữ liệu bên trong giống nhau, phép rec1 == rec2 trả về true!
                  - Tự động cài đặt sẵn hàm .ToString() in ra toàn bộ thuộc tính rất đẹp mắt và dữ liệu mặc định là bất biến (init), cực kỳ an toàn khi truyền qua lại giữa các tầng.
                  - Dùng khi: Làm DTO (Data Transfer Object) trong Web API!
                • 3. struct (Kiểu Tham trị - Value Type - Lưu trên Stack):
                  - Nhẹ nhàng, cấp phát trực tiếp trên Stack, không hỗ trợ kế thừa từ struct/class khác.
                  - Dùng khi: Biểu diễn các cấu trúc dữ liệu rất nhỏ như tọa độ Point(x, y), màu sắc RgbColor, DateTime.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Khai báo DTO siêu tốc chỉ với 1 dòng bằng record",
                        "C# tự động sinh ngầm các thuộc tính { get; init; } và Constructor cho bạn.",
                        "public record UserDto(int Id, string FullName, string Email);"),
                    Step(2,
                        "2. So sánh theo nội dung giá trị của record",
                        "Hai record khác vùng nhớ nhưng giống hệt nội dung bên trong sẽ được hiểu là bằng nhau (true).",
                        "var u1 = new UserDto(1, \"Việt\", \"viet@gmail.com\");\nvar u2 = new UserDto(1, \"Việt\", \"viet@gmail.com\");\nbool isEqual = (u1 == u2); // Kết quả = true! (Nếu là class sẽ ra false)"),
                    Step(3,
                        "3. Tạo bản sao chỉnh sửa nhanh với từ khóa with",
                        "Vì record bất biến, khi muốn đổi 1 trường, dùng từ khóa 'with' để tạo bản sao mới cực nhanh.",
                        "var updatedUser = u1 with { Email = \"newmail@gmail.com\" };\n// Giữ nguyên Id = 1, FullName = \"Việt\", chỉ đổi Email!")
                },
                "Tại sao trong các dự án ASP.NET Core Web API hiện đại, các lập trình viên lại rất ưa chuộng dùng từ khóa record (thay vì class) để định nghĩa các DTO (Data Transfer Objects)?",
                new List<string>
                {
                    "A. Vì DTO chỉ dùng để vận chuyển dữ liệu không cần thay đổi (Immutable); dùng record giúp khai báo ngắn gọn chỉ trong 1 dòng, tự động hỗ trợ so sánh theo giá trị và bảo vệ dữ liệu không bị sửa đổi ngoài ý muốn trên đường truyền",
                    "B. Vì class đã bị xóa khỏi C#",
                    "C. Vì chỉ có record mới lưu được vào SQL Server",
                    "D. Vì record không cần khai báo kiểu dữ liệu cho thuộc tính"
                },
                "A",
                "record sinh ra để dành riêng cho các mô hình chứa dữ liệu (Data-centric models) như DTO, giúp giảm từ 15 dòng code khai báo class xuống chỉ còn 1 dòng duy nhất mà vẫn đầy đủ tính năng bất biến và hiển thị rõ ràng."
            ),

            // =====================================================================
            // BÀI 2.11
            // =====================================================================
            Build(
                topicId,
                "2.11",
                "Bài tập tổng kết và Kiểm tra năng lực Chủ đề 2 (Làm chủ OOP trong C#)",
                "OOP thực tế",
                "https://learn.microsoft.com/vi-vn/dotnet/csharp/fundamentals/object-oriented/",
                """
                • Bản đồ tổng kết 4 trụ cột OOP & Ứng dụng thực tế trong .NET:
                  1. Tính Đóng gói (Encapsulation): Bảo vệ dữ liệu bằng private field và mở cổng kiểm soát qua public Property ({ get; set; }, { get; init; }).
                  2. Tính Kế thừa (Inheritance): Tái sử dụng code từ lớp cha qua dấu :, gọi lớp cha bằng base, khóa kế thừa bằng sealed.
                  3. Tính Đa hình (Polymorphism): Cùng tên hàm nhưng hành xử khác nhau — Nạp chồng (Overloading) trong cùng class, hoặc Ghi đè (virtual ở cha -> override ở con).
                  4. Tính Trừu tượng (Abstraction): Dùng Interface làm bản hợp đồng kết nối các tầng (Controller -> IService -> IRepository) và dùng Abstract Class làm khuôn mẫu cơ sở.
                  5. Thiết kế hiện đại: Ưu tiên quan hệ Has-A (Composition + Dependency Injection), dùng record cho DTO và dùng Extension Methods cho các hàm chuyển đổi tiện ích.
                """,
                new List<InteractiveFlowStep>
                {
                    Step(1,
                        "1. Tạo Hợp đồng Trừu tượng (Interface)",
                        "Định nghĩa chuẩn giao tiếp cho dịch vụ thông báo.",
                        "public interface INotificationService {\n    void Send(string receiver, string message);\n}"),
                    Step(2,
                        "2. Thực thi Đa hình qua các Class cụ thể",
                        "EmailNotification và SmsNotification cùng thực thi Interface theo cách riêng.",
                        "public class EmailNotification : INotificationService {\n    public void Send(string receiver, string message) => Console.WriteLine($\"Gửi Email tới {receiver}: {message}\");\n}"),
                    Step(3,
                        "3. Áp dụng Đóng gói & Quan hệ Has-A tại OrderProcessor",
                        "Khoá kín biến _notifier bằng private readonly và nhận qua Constructor.",
                        "public class OrderProcessor {\n    private readonly INotificationService _notifier;\n    public OrderProcessor(INotificationService notifier) {\n        _notifier = notifier;\n    }\n    public void CompleteOrder(string email) => _notifier.Send(email, \"Đơn hàng hoàn tất!\");\n}"),
                    Step(4,
                        "4. Vận chuyển dữ liệu bằng Record DTO",
                        "Đóng gói dữ liệu đầu vào/đầu ra gọn nhẹ, bất biến.",
                        "public record OrderCompletedDto(Guid OrderId, string CustomerEmail, DateTime CompletedAt);")
                },
                "(Câu hỏi tổng hợp Chủ đề 2) Giả sử bạn cần thiết kế hệ thống tính phí vận chuyển cho nhiều hãng giao hàng khác nhau (GiaoHangNhanh, ViettelPost, GrabExpress) sao cho OrderService có thể dễ dàng đổi hãng giao hàng mà không cần sửa code bên trong OrderService. Bạn nên áp dụng tính chất OOP nào?",
                new List<string>
                {
                    "A. Tạo một Interface IShippingService có hàm CalculateFee(), cho các class GiaoHangNhanh, ViettelPost thực thi Interface đó, và tiêm IShippingService vào Constructor của OrderService (Trừu tượng + Đa hình + Composition)",
                    "B. Viết tất cả vào một hàm static dài 1000 dòng",
                    "C. Cho OrderService kế thừa trực tiếp từ class ViettelPost",
                    "D. Đặt tất cả các hàm là private"
                },
                "A",
                "Thiết kế qua Interface (IShippingService) kết hợp Đa hình và Dependency Injection giúp OrderService hoàn toàn độc lập với từng hãng vận chuyển cụ thể (Loose Coupling), đúng chuẩn kiến trúc phần mềm chuyên nghiệp."
            )
        };
    }

    /// <summary>
    /// Thay thế/cập nhật đầy đủ 11 bài học con (2.1 -> 2.11) cho Chủ đề 2 (Menu 2: DETAILED_TRACK) vào Database.
    /// </summary>
    public static async Task EnsureSeededAsync(AppDbContext context)
    {
        var detailedTrack = await context.Tracks
            .Include(t => t.Topics)
            .FirstOrDefaultAsync(t => t.Code == "DETAILED_TRACK");

        if (detailedTrack == null)
            return;

        var topic2 = detailedTrack.Topics.FirstOrDefault(tp => tp.TopicNumber == 2);
        if (topic2 == null)
            return;

        topic2.Title = "Chủ đề 2: Lập trình hướng đối tượng trong C# (OOP)";
        topic2.Description = "11 Bài học con chi tiết từ Tư duy OOP (Class/Object), Đóng gói (Field/Property/init), Phạm vi truy cập, Constructor & GC, Kế thừa, Đa hình, Trừu tượng (Abstract Class vs Interface), Has-A vs Is-A, static & Extension Methods, Class vs Struct vs Record đến Bài tập tổng kết.";

        var targetLessons = GetLessons(topic2.Id);
        var existingLessons = await context.SubLessons
            .Where(s => s.TopicId == topic2.Id)
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
