using Bogus;
using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (!await db.Users.AnyAsync())
        {
            var faker = new Faker<User>("vi")
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email,    f => f.Internet.Email())
                .RuleFor(u => u.Role,     f => f.PickRandom("Admin", "Member"))
                .RuleFor(u => u.CreatedAt, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now));

            var users = faker.Generate(1000);
            await db.Users.AddRangeAsync(users);
            await db.SaveChangesAsync();
        }

        // if (!await db.Users.AnyAsync())
        // {
        //     var users = new List<User>
        //     {
        //         new User { FullName = "Nguyễn Văn A", Email = "a@gmail.com", Role = "Admin" },
        //         new User { FullName = "Trần Thị B", Email = "b@gmail.com", Role = "Member" },
        //         new User { FullName = "Lê Văn C", Email = "c@gmail.com", Role = "Member" }
        //     };
        //     await db.Users.AddRangeAsync(users);
        //     await db.SaveChangesAsync();
        // }

        if (await db.Categories.AnyAsync() || await db.Courses.AnyAsync()) return;

        var categories = new List<Category>
        {
            new() { Name = "Lập trình Web",       Description = "Các khóa học phát triển ứng dụng web front-end và back-end" },
            new() { Name = "Lập trình Di động",    Description = "Phát triển ứng dụng Android, iOS và đa nền tảng" },
            new() { Name = "Trí tuệ Nhân tạo",    Description = "Machine Learning, Deep Learning và ứng dụng AI thực tế" },
            new() { Name = "Cơ sở Dữ liệu",       Description = "Thiết kế, quản trị và tối ưu hóa cơ sở dữ liệu" },
            new() { Name = "An toàn Thông tin",    Description = "Bảo mật hệ thống, kiểm thử xâm nhập và phòng chống tấn công" },
        };

        await db.Categories.AddRangeAsync(categories);
        await db.SaveChangesAsync();

        var now = DateTime.Now;

        var courses = new List<Course>
        {
            // Lập trình Web (index 0)
            new()
            {
                Title       = "Xây dựng Website với HTML, CSS và JavaScript",
                Description = "Khóa học nền tảng giúp bạn tạo ra các trang web tĩnh chuyên nghiệp từ đầu.",
                Content     = "<p>Học HTML5, CSS3 và JavaScript cơ bản để xây dựng giao diện web hiện đại.</p>",
                Price       = 299000,
                StartDate   = now.AddDays(5),
                EndDate     = now.AddDays(35),
                CategoryId  = categories[0].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Lập trình Back-end với ASP.NET Core",
                Description = "Xây dựng API RESTful và ứng dụng MVC bằng .NET 8.",
                Content     = "<p>Tìm hiểu Entity Framework Core, Identity, Middleware và triển khai ứng dụng thực tế.</p>",
                Price       = 599000,
                StartDate   = now.AddDays(7),
                EndDate     = now.AddDays(67),
                CategoryId  = categories[0].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "ReactJS từ cơ bản đến nâng cao",
                Description = "Nắm vững React Hooks, Redux Toolkit và tích hợp REST API.",
                Content     = "<p>Xây dựng Single Page Application hoàn chỉnh với React và TypeScript.</p>",
                Price       = 499000,
                StartDate   = now.AddDays(3),
                EndDate     = now.AddDays(43),
                CategoryId  = categories[0].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Lập trình Node.js và Express.js",
                Description = "Tạo REST API nhanh chóng với Node.js, Express và MongoDB.",
                Content     = "<p>Học cách xây dựng server, xử lý middleware, xác thực JWT và kết nối MongoDB.</p>",
                Price       = 449000,
                StartDate   = now.AddDays(10),
                EndDate     = now.AddDays(50),
                CategoryId  = categories[0].Id,
                Status      = 1,
            },

            // Lập trình Di động (index 1)
            new()
            {
                Title       = "Phát triển ứng dụng Android với Kotlin",
                Description = "Xây dựng ứng dụng Android hiện đại bằng Kotlin và Jetpack Compose.",
                Content     = "<p>Từ Activity, Fragment đến ViewModel, LiveData và Room Database.</p>",
                Price       = 549000,
                StartDate   = now.AddDays(6),
                EndDate     = now.AddDays(56),
                CategoryId  = categories[1].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Lập trình iOS với Swift và SwiftUI",
                Description = "Tạo ứng dụng iPhone và iPad chuyên nghiệp từ đầu.",
                Content     = "<p>Học Swift cơ bản, SwiftUI, CoreData và triển khai lên App Store.</p>",
                Price       = 649000,
                StartDate   = now.AddDays(14),
                EndDate     = now.AddDays(74),
                CategoryId  = categories[1].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Flutter - Lập trình đa nền tảng",
                Description = "Một codebase duy nhất cho Android, iOS và Web.",
                Content     = "<p>Làm chủ Flutter Widget, State Management với Bloc và tích hợp Firebase.</p>",
                Price       = 599000,
                StartDate   = now.AddDays(4),
                EndDate     = now.AddDays(44),
                CategoryId  = categories[1].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "React Native - Ứng dụng di động với JavaScript",
                Description = "Phát triển app di động bằng React Native cho cả Android lẫn iOS.",
                Content     = "<p>Navigation, Redux, Push Notification và xuất bản ứng dụng lên các store.</p>",
                Price       = 499000,
                StartDate   = now.AddDays(8),
                EndDate     = now.AddDays(48),
                CategoryId  = categories[1].Id,
                Status      = 1,
            },

            // Trí tuệ Nhân tạo (index 2)
            new()
            {
                Title       = "Machine Learning cơ bản với Python",
                Description = "Nắm vững các thuật toán học máy thông dụng và ứng dụng thực tế.",
                Content     = "<p>Linear Regression, Decision Tree, SVM, K-Means với scikit-learn và pandas.</p>",
                Price       = 699000,
                StartDate   = now.AddDays(2),
                EndDate     = now.AddDays(62),
                CategoryId  = categories[2].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Deep Learning với TensorFlow và Keras",
                Description = "Xây dựng mạng nơ-ron nhân tạo để giải quyết bài toán phân loại và nhận dạng.",
                Content     = "<p>CNN, RNN, LSTM và Transfer Learning với TensorFlow 2.x.</p>",
                Price       = 799000,
                StartDate   = now.AddDays(9),
                EndDate     = now.AddDays(79),
                CategoryId  = categories[2].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Xử lý Ngôn ngữ Tự nhiên (NLP)",
                Description = "Phân tích văn bản, phân loại cảm xúc và xây dựng chatbot.",
                Content     = "<p>Tokenization, Word2Vec, BERT và fine-tuning mô hình ngôn ngữ lớn.</p>",
                Price       = 749000,
                StartDate   = now.AddDays(12),
                EndDate     = now.AddDays(72),
                CategoryId  = categories[2].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Thị giác Máy tính với OpenCV",
                Description = "Nhận diện khuôn mặt, phát hiện đối tượng và xử lý hình ảnh thực tế.",
                Content     = "<p>OpenCV, YOLO và ứng dụng computer vision trong giám sát và tự động hóa.</p>",
                Price       = 699000,
                StartDate   = now.AddDays(15),
                EndDate     = now.AddDays(65),
                CategoryId  = categories[2].Id,
                Status      = 1,
            },

            // Cơ sở Dữ liệu (index 3)
            new()
            {
                Title       = "SQL toàn diện từ cơ bản đến nâng cao",
                Description = "Thành thạo truy vấn, thiết kế schema và tối ưu hiệu năng cơ sở dữ liệu.",
                Content     = "<p>SELECT, JOIN, Subquery, Index, Transaction và Stored Procedure trong MySQL.</p>",
                Price       = 349000,
                StartDate   = now.AddDays(1),
                EndDate     = now.AddDays(31),
                CategoryId  = categories[3].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "MongoDB - Cơ sở dữ liệu NoSQL",
                Description = "Lưu trữ và truy vấn dữ liệu phi cấu trúc với MongoDB.",
                Content     = "<p>CRUD, Aggregation Pipeline, Indexing và kết nối MongoDB với Node.js.</p>",
                Price       = 399000,
                StartDate   = now.AddDays(6),
                EndDate     = now.AddDays(36),
                CategoryId  = categories[3].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Redis - Bộ nhớ đệm và Message Broker",
                Description = "Tăng tốc ứng dụng với Redis Cache và xây dựng hệ thống hàng đợi.",
                Content     = "<p>String, Hash, List, Pub/Sub và tích hợp Redis vào ứng dụng .NET và Node.js.</p>",
                Price       = 449000,
                StartDate   = now.AddDays(11),
                EndDate     = now.AddDays(41),
                CategoryId  = categories[3].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "PostgreSQL nâng cao cho lập trình viên",
                Description = "Khai thác tối đa PostgreSQL với các tính năng nâng cao.",
                Content     = "<p>JSON/JSONB, Full-text Search, Window Function và tối ưu hóa câu truy vấn phức tạp.</p>",
                Price       = 499000,
                StartDate   = now.AddDays(13),
                EndDate     = now.AddDays(53),
                CategoryId  = categories[3].Id,
                Status      = 1,
            },

            // An toàn Thông tin (index 4)
            new()
            {
                Title       = "Bảo mật ứng dụng Web - OWASP Top 10",
                Description = "Hiểu và phòng chống 10 lỗ hổng bảo mật web phổ biến nhất.",
                Content     = "<p>SQL Injection, XSS, CSRF, IDOR và cách vá lỗi trong ứng dụng thực tế.</p>",
                Price       = 599000,
                StartDate   = now.AddDays(3),
                EndDate     = now.AddDays(33),
                CategoryId  = categories[4].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Kiểm thử Xâm nhập với Kali Linux",
                Description = "Học Penetration Testing từ đầu với bộ công cụ Kali Linux.",
                Content     = "<p>Reconnaissance, Scanning, Exploitation với Metasploit và báo cáo kết quả kiểm thử.</p>",
                Price       = 799000,
                StartDate   = now.AddDays(7),
                EndDate     = now.AddDays(67),
                CategoryId  = categories[4].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Mật mã học và Bảo vệ Dữ liệu",
                Description = "Nắm vững các thuật toán mã hóa và ứng dụng trong bảo mật thông tin.",
                Content     = "<p>Symmetric/Asymmetric Encryption, Hash, TLS/SSL và triển khai trong ứng dụng thực tế.</p>",
                Price       = 649000,
                StartDate   = now.AddDays(10),
                EndDate     = now.AddDays(50),
                CategoryId  = categories[4].Id,
                Status      = 1,
            },
            new()
            {
                Title       = "Phân tích Mã độc và Điều tra Số",
                Description = "Phân tích malware, forensics hệ thống và ứng phó sự cố bảo mật.",
                Content     = "<p>Static/Dynamic Analysis, Memory Forensics và quy trình xử lý sự cố an toàn thông tin.</p>",
                Price       = 849000,
                StartDate   = now.AddDays(16),
                EndDate     = now.AddDays(76),
                CategoryId  = categories[4].Id,
                Status      = 1,
            },
        };

        await db.Courses.AddRangeAsync(courses);
        await db.SaveChangesAsync();
    }
}
