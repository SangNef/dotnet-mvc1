using Bogus;
using HelloWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace HelloWorld.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db, bool isDevelopment = false)
    {
        if (!await db.Users.AnyAsync())
        {
            var faker = new Faker<User>("vi")
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.Email,    f => f.Internet.Email())
                .RuleFor(u => u.Role,     f => f.PickRandom("Admin", "Member"))
                .RuleFor(u => u.CreatedAt, f => f.Date.Between(DateTime.Now.AddDays(-30), DateTime.Now));

            await db.Users.AddRangeAsync(faker.Generate(1000));
            await db.SaveChangesAsync();
        }

        if (await db.Categories.AnyAsync() || await db.Courses.AnyAsync()) goto SeedMusic;

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
            new() { Title = "Xây dựng Website với HTML, CSS và JavaScript", Description = "Khóa học nền tảng giúp bạn tạo ra các trang web tĩnh chuyên nghiệp từ đầu.", Content = "<p>Học HTML5, CSS3 và JavaScript cơ bản để xây dựng giao diện web hiện đại.</p>", Price = 299000, StartDate = now.AddDays(5),  EndDate = now.AddDays(35), CategoryId = categories[0].Id, Status = 1 },
            new() { Title = "Lập trình Back-end với ASP.NET Core",           Description = "Xây dựng API RESTful và ứng dụng MVC bằng .NET 8.",                              Content = "<p>Tìm hiểu Entity Framework Core, Identity, Middleware và triển khai ứng dụng thực tế.</p>",     Price = 599000, StartDate = now.AddDays(7),  EndDate = now.AddDays(67), CategoryId = categories[0].Id, Status = 1 },
            new() { Title = "ReactJS từ cơ bản đến nâng cao",                Description = "Nắm vững React Hooks, Redux Toolkit và tích hợp REST API.",                      Content = "<p>Xây dựng Single Page Application hoàn chỉnh với React và TypeScript.</p>",                  Price = 499000, StartDate = now.AddDays(3),  EndDate = now.AddDays(43), CategoryId = categories[0].Id, Status = 1 },
            new() { Title = "Lập trình Node.js và Express.js",               Description = "Tạo REST API nhanh chóng với Node.js, Express và MongoDB.",                      Content = "<p>Học cách xây dựng server, xử lý middleware, xác thực JWT và kết nối MongoDB.</p>",           Price = 449000, StartDate = now.AddDays(10), EndDate = now.AddDays(50), CategoryId = categories[0].Id, Status = 1 },
            new() { Title = "Phát triển ứng dụng Android với Kotlin",        Description = "Xây dựng ứng dụng Android hiện đại bằng Kotlin và Jetpack Compose.",             Content = "<p>Từ Activity, Fragment đến ViewModel, LiveData và Room Database.</p>",                       Price = 549000, StartDate = now.AddDays(6),  EndDate = now.AddDays(56), CategoryId = categories[1].Id, Status = 1 },
            new() { Title = "Lập trình iOS với Swift và SwiftUI",            Description = "Tạo ứng dụng iPhone và iPad chuyên nghiệp từ đầu.",                              Content = "<p>Học Swift cơ bản, SwiftUI, CoreData và triển khai lên App Store.</p>",                       Price = 649000, StartDate = now.AddDays(14), EndDate = now.AddDays(74), CategoryId = categories[1].Id, Status = 1 },
            new() { Title = "Flutter - Lập trình đa nền tảng",              Description = "Một codebase duy nhất cho Android, iOS và Web.",                                  Content = "<p>Làm chủ Flutter Widget, State Management với Bloc và tích hợp Firebase.</p>",               Price = 599000, StartDate = now.AddDays(4),  EndDate = now.AddDays(44), CategoryId = categories[1].Id, Status = 1 },
            new() { Title = "React Native - Ứng dụng di động với JavaScript",Description = "Phát triển app di động bằng React Native cho cả Android lẫn iOS.",               Content = "<p>Navigation, Redux, Push Notification và xuất bản ứng dụng lên các store.</p>",              Price = 499000, StartDate = now.AddDays(8),  EndDate = now.AddDays(48), CategoryId = categories[1].Id, Status = 1 },
            new() { Title = "Machine Learning cơ bản với Python",            Description = "Nắm vững các thuật toán học máy thông dụng và ứng dụng thực tế.",                Content = "<p>Linear Regression, Decision Tree, SVM, K-Means với scikit-learn và pandas.</p>",            Price = 699000, StartDate = now.AddDays(2),  EndDate = now.AddDays(62), CategoryId = categories[2].Id, Status = 1 },
            new() { Title = "Deep Learning với TensorFlow và Keras",          Description = "Xây dựng mạng nơ-ron nhân tạo để giải quyết bài toán phân loại và nhận dạng.",   Content = "<p>CNN, RNN, LSTM và Transfer Learning với TensorFlow 2.x.</p>",                              Price = 799000, StartDate = now.AddDays(9),  EndDate = now.AddDays(79), CategoryId = categories[2].Id, Status = 1 },
            new() { Title = "Xử lý Ngôn ngữ Tự nhiên (NLP)",                Description = "Phân tích văn bản, phân loại cảm xúc và xây dựng chatbot.",                      Content = "<p>Tokenization, Word2Vec, BERT và fine-tuning mô hình ngôn ngữ lớn.</p>",                     Price = 749000, StartDate = now.AddDays(12), EndDate = now.AddDays(72), CategoryId = categories[2].Id, Status = 1 },
            new() { Title = "Thị giác Máy tính với OpenCV",                  Description = "Nhận diện khuôn mặt, phát hiện đối tượng và xử lý hình ảnh thực tế.",            Content = "<p>OpenCV, YOLO và ứng dụng computer vision trong giám sát và tự động hóa.</p>",              Price = 699000, StartDate = now.AddDays(15), EndDate = now.AddDays(65), CategoryId = categories[2].Id, Status = 1 },
            new() { Title = "SQL toàn diện từ cơ bản đến nâng cao",          Description = "Thành thạo truy vấn, thiết kế schema và tối ưu hiệu năng cơ sở dữ liệu.",        Content = "<p>SELECT, JOIN, Subquery, Index, Transaction và Stored Procedure trong MySQL.</p>",           Price = 349000, StartDate = now.AddDays(1),  EndDate = now.AddDays(31), CategoryId = categories[3].Id, Status = 1 },
            new() { Title = "MongoDB - Cơ sở dữ liệu NoSQL",                Description = "Lưu trữ và truy vấn dữ liệu phi cấu trúc với MongoDB.",                          Content = "<p>CRUD, Aggregation Pipeline, Indexing và kết nối MongoDB với Node.js.</p>",                 Price = 399000, StartDate = now.AddDays(6),  EndDate = now.AddDays(36), CategoryId = categories[3].Id, Status = 1 },
            new() { Title = "Redis - Bộ nhớ đệm và Message Broker",         Description = "Tăng tốc ứng dụng với Redis Cache và xây dựng hệ thống hàng đợi.",               Content = "<p>String, Hash, List, Pub/Sub và tích hợp Redis vào ứng dụng .NET và Node.js.</p>",          Price = 449000, StartDate = now.AddDays(11), EndDate = now.AddDays(41), CategoryId = categories[3].Id, Status = 1 },
            new() { Title = "PostgreSQL nâng cao cho lập trình viên",        Description = "Khai thác tối đa PostgreSQL với các tính năng nâng cao.",                        Content = "<p>JSON/JSONB, Full-text Search, Window Function và tối ưu hóa câu truy vấn phức tạp.</p>",    Price = 499000, StartDate = now.AddDays(13), EndDate = now.AddDays(53), CategoryId = categories[3].Id, Status = 1 },
            new() { Title = "Bảo mật ứng dụng Web - OWASP Top 10",          Description = "Hiểu và phòng chống 10 lỗ hổng bảo mật web phổ biến nhất.",                      Content = "<p>SQL Injection, XSS, CSRF, IDOR và cách vá lỗi trong ứng dụng thực tế.</p>",                Price = 599000, StartDate = now.AddDays(3),  EndDate = now.AddDays(33), CategoryId = categories[4].Id, Status = 1 },
            new() { Title = "Kiểm thử Xâm nhập với Kali Linux",             Description = "Học Penetration Testing từ đầu với bộ công cụ Kali Linux.",                       Content = "<p>Reconnaissance, Scanning, Exploitation với Metasploit và báo cáo kết quả kiểm thử.</p>",  Price = 799000, StartDate = now.AddDays(7),  EndDate = now.AddDays(67), CategoryId = categories[4].Id, Status = 1 },
            new() { Title = "Mật mã học và Bảo vệ Dữ liệu",                Description = "Nắm vững các thuật toán mã hóa và ứng dụng trong bảo mật thông tin.",             Content = "<p>Symmetric/Asymmetric Encryption, Hash, TLS/SSL và triển khai trong ứng dụng thực tế.</p>", Price = 649000, StartDate = now.AddDays(10), EndDate = now.AddDays(50), CategoryId = categories[4].Id, Status = 1 },
            new() { Title = "Phân tích Mã độc và Điều tra Số",              Description = "Phân tích malware, forensics hệ thống và ứng phó sự cố bảo mật.",                Content = "<p>Static/Dynamic Analysis, Memory Forensics và quy trình xử lý sự cố an toàn thông tin.</p>",Price = 849000, StartDate = now.AddDays(16), EndDate = now.AddDays(76), CategoryId = categories[4].Id, Status = 1 },
        };
        await db.Courses.AddRangeAsync(courses);
        await db.SaveChangesAsync();

        SeedMusic:

        if (isDevelopment)
        {
            await db.Songs.ExecuteDeleteAsync();
            await db.Singers.ExecuteDeleteAsync();
            await db.Composers.ExecuteDeleteAsync();
        }

        if (await db.Singers.AnyAsync()) return;

        var singers = new List<Singer>
        {
            new() { Name = "Hà Anh Tuấn",    Biography = "<p>Ca sĩ người Việt Nam với giọng hát trữ tình sâu lắng, nổi tiếng với dòng nhạc acoustic và ballad lãng mạn.</p>" },
            new() { Name = "Mỹ Tâm",          Biography = "<p>Nữ ca sĩ hàng đầu Việt Nam, được mệnh danh là 'Nữ hoàng nhạc Pop' với sự nghiệp âm nhạc kéo dài hơn 20 năm.</p>" },
            new() { Name = "Sơn Tùng M-TP",   Biography = "<p>Ca sĩ kiêm nhạc sĩ trẻ tài năng, người tiên phong mang phong cách âm nhạc hiện đại vào V-pop.</p>" },
            new() { Name = "Đen Vâu",         Biography = "<p>Rapper và nhạc sĩ với phong cách indie độc đáo, các bài hát mang đậm chất thơ và triết lý cuộc sống.</p>" },
            new() { Name = "Hòa Minzy",       Biography = "<p>Ca sĩ trẻ với giọng hát nội lực, thường thể hiện các bài hát pop ballad và nhạc trẻ sôi động.</p>" },
        };
        await db.Singers.AddRangeAsync(singers);
        await db.SaveChangesAsync();

        var composers = new List<Composer>
        {
            new() { Name = "Nguyễn Hải Phong", Biography = "<p>Nhạc sĩ tài ba với phong cách sáng tác lãng mạn, tinh tế. Tác giả của nhiều bản hit đình đám trong V-pop.</p>" },
            new() { Name = "Dương Khắc Linh",  Biography = "<p>Nhạc sĩ đa tài, producer nổi tiếng trong làng nhạc Việt với hàng trăm tác phẩm chất lượng cao.</p>" },
            new() { Name = "Khắc Hưng",        Biography = "<p>Nhạc sĩ trẻ tài năng, chuyên sáng tác những giai điệu ngọt ngào, dễ đi vào lòng người.</p>" },
            new() { Name = "Hứa Kim Tuyền",    Biography = "<p>Nhạc sĩ với phong cách indie, avant-garde, tạo ra những tác phẩm mang âm hưởng nghệ thuật độc đáo.</p>" },
            new() { Name = "Đinh Tiến Đạt",    Biography = "<p>Nhạc sĩ gạo cội của nền âm nhạc Việt Nam, tác giả của nhiều ca khúc bất hủ qua nhiều thập kỷ.</p>" },
        };
        await db.Composers.AddRangeAsync(composers);
        await db.SaveChangesAsync();

        var baseDate = DateTime.Now;
        var songs = new List<Song>
        {
            new() { Title = "Tháng Tư Là Lời Nói Dối Của Anh", Lyrics = "<p>Tháng tư rồi, em ơi tháng tư về mang mưa giăng khắp lối...</p>",  SingerId = singers[0].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-400), Status = 1 },
            new() { Title = "Bầu Trời Của Kẻ Khờ",             Lyrics = "<p>Có ai hỏi em đang nghĩ gì, thì em nói rằng em chẳng nghĩ chi...</p>",SingerId = singers[0].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-380), Status = 1 },
            new() { Title = "Nhật Ký Của Mẹ",                  Lyrics = "<p>Ký ức về mẹ ngày xưa, khi còn bé thơ ngây...</p>",                  SingerId = singers[0].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-360), Status = 1 },
            new() { Title = "Vẫn Còn Đây",                     Lyrics = "<p>Dù thời gian trôi qua, anh vẫn còn đây bên em...</p>",               SingerId = singers[0].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-340), Status = 1 },
            new() { Title = "Tình Yêu Muôn Màu",               Lyrics = "<p>Tình yêu như cầu vồng hiện lên sau cơn mưa...</p>",                  SingerId = singers[0].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-320), Status = 1 },
            new() { Title = "Thu Cuối",                         Lyrics = "<p>Thu về lặng lẽ đi qua khung cửa, lá vàng rơi nhẹ theo gió...</p>",   SingerId = singers[0].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-300), Status = 1 },
            new() { Title = "Mây Trắng Để Lâu Nghe Có Ngậm Ngùi", Lyrics = "<p>Mây trắng nhẹ trôi qua vùng trời xanh mát...</p>",               SingerId = singers[0].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-280), Status = 1 },
            new() { Title = "Thành Phố Buồn",                  Lyrics = "<p>Phố vắng khi chiều về, một mình ta bước đi...</p>",                   SingerId = singers[0].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-260), Status = 1 },
            new() { Title = "Một Thời Để Nhớ",                 Lyrics = "<p>Một thời tuổi trẻ qua đi không trở lại...</p>",                       SingerId = singers[0].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-240), Status = 1 },
            new() { Title = "Phi Công Trẻ Lái Máy Bay Bà Già", Lyrics = "<p>Yêu nhau chẳng kể tuổi tác, chỉ cần trái tim đồng điệu...</p>",       SingerId = singers[0].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-220), Status = 1 },

            new() { Title = "Điên Vì Yêu",                     Lyrics = "<p>Yêu anh đến điên, đến mức không còn biết mình là ai...</p>",          SingerId = singers[1].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-420), Status = 1 },
            new() { Title = "Đừng Hỏi Em",                     Lyrics = "<p>Đừng hỏi em vì sao lại khóc, vì anh biết rõ lý do hơn ai hết...</p>",SingerId = singers[1].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-400), Status = 1 },
            new() { Title = "Nơi Tình Yêu Bắt Đầu",           Lyrics = "<p>Nơi ấy có mưa, có nắng và có em bé nhỏ của anh...</p>",               SingerId = singers[1].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-380), Status = 1 },
            new() { Title = "Chờ Người Nơi Ấy",                Lyrics = "<p>Em ngồi đây chờ anh, ngày qua ngày không biết mỏi...</p>",             SingerId = singers[1].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-360), Status = 1 },
            new() { Title = "Giả Vờ Yêu",                      Lyrics = "<p>Giả vờ yêu nhau để che giấu nỗi đau thật sự...</p>",                  SingerId = singers[1].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-340), Status = 1 },
            new() { Title = "Bởi Vì Anh Là Lý Do",            Lyrics = "<p>Bởi vì anh là lý do em mỉm cười mỗi sáng thức dậy...</p>",             SingerId = singers[1].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-320), Status = 1 },
            new() { Title = "Mây Vẫn Bay",                     Lyrics = "<p>Mây vẫn bay trên bầu trời dù em đã không còn ở đây...</p>",            SingerId = singers[1].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-300), Status = 1 },
            new() { Title = "Không Cần Phải Hứa Đâu Anh",     Lyrics = "<p>Không cần anh hứa gì cả, chỉ cần anh ở bên em thôi...</p>",            SingerId = singers[1].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-280), Status = 1 },
            new() { Title = "Gọi Tên Em Trong Đêm",            Lyrics = "<p>Đêm khuya anh gọi tên em, tiếng vọng vào không gian vắng lặng...</p>", SingerId = singers[1].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-260), Status = 1 },
            new() { Title = "Tâm Hồn Tôi",                     Lyrics = "<p>Tâm hồn tôi là những giai điệu, là những nốt nhạc vang lên...</p>",   SingerId = singers[1].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-240), Status = 1 },

            new() { Title = "Nơi Này Có Anh",                  Lyrics = "<p>Nơi này có anh, có em và có tình yêu chúng ta...</p>",                 SingerId = singers[2].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-440), Status = 1 },
            new() { Title = "Hãy Trao Cho Anh",                Lyrics = "<p>Hãy trao cho anh thêm một lần nữa, tình yêu của em...</p>",             SingerId = singers[2].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-420), Status = 1 },
            new() { Title = "Chạy Ngay Đi",                    Lyrics = "<p>Chạy ngay đi, đừng nhìn lại, phía trước là tương lai rộng mở...</p>",  SingerId = singers[2].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-400), Status = 1 },
            new() { Title = "Lạc Trôi",                        Lyrics = "<p>Lạc trôi theo dòng đời, không biết đâu là bến bờ...</p>",               SingerId = singers[2].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-380), Status = 1 },
            new() { Title = "Em Của Ngày Hôm Qua",             Lyrics = "<p>Em của ngày hôm qua giờ đã khác, đã trưởng thành hơn...</p>",           SingerId = singers[2].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-360), Status = 1 },
            new() { Title = "Nắng Ấm Xa Dần",                  Lyrics = "<p>Nắng ấm dần xa, mùa đông đang đến gần bên ta...</p>",                   SingerId = singers[2].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-340), Status = 1 },
            new() { Title = "Cơn Mưa Ngang Qua",               Lyrics = "<p>Cơn mưa ngang qua mang theo ký ức về em...</p>",                        SingerId = singers[2].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-320), Status = 1 },
            new() { Title = "Âm Thầm Bên Em",                  Lyrics = "<p>Âm thầm bên em, không nói nên lời những gì trong tim...</p>",           SingerId = singers[2].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-300), Status = 1 },
            new() { Title = "Muộn Rồi Mà Sao Còn",            Lyrics = "<p>Muộn rồi mà sao còn đứng đây nhớ về em?</p>",                           SingerId = singers[2].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-280), Status = 1 },
            new() { Title = "Có Chắc Yêu Là Đây",              Lyrics = "<p>Có chắc đây là tình yêu hay chỉ là cảm giác nhất thời?</p>",            SingerId = singers[2].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-260), Status = 1 },

            new() { Title = "Mang Tiền Về Cho Mẹ",             Lyrics = "<p>Con đi xa nhà, lòng luôn hướng về mẹ già tóc bạc...</p>",               SingerId = singers[3].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-460), Status = 1 },
            new() { Title = "Hai Triệu Năm",                   Lyrics = "<p>Hai triệu năm tiến hóa để được gặp em trong khoảnh khắc này...</p>",    SingerId = singers[3].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-440), Status = 1 },
            new() { Title = "Tặng",                            Lyrics = "<p>Bài này tặng cho những ai đang cô đơn giữa đám đông...</p>",             SingerId = singers[3].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-420), Status = 1 },
            new() { Title = "Đưa Nhau Đi Trốn",               Lyrics = "<p>Đưa nhau đi trốn khỏi thành phố ồn ào, tìm về thiên nhiên yên bình...</p>", SingerId = singers[3].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-400), Status = 1 },
            new() { Title = "Cô Bé Lạc Quan",                  Lyrics = "<p>Cô bé nhỏ với nụ cười luôn tươi, nhìn cuộc đời bằng đôi mắt trong sáng...</p>", SingerId = singers[3].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-380), Status = 1 },
            new() { Title = "Trốn Tìm",                        Lyrics = "<p>Trốn tìm trong ký ức, tìm lại những ngày tháng đã qua...</p>",           SingerId = singers[3].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-360), Status = 1 },
            new() { Title = "Bài Này Chill Phết",              Lyrics = "<p>Ngồi đây một mình, nghe nhạc và thả hồn theo giai điệu...</p>",          SingerId = singers[3].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-340), Status = 1 },
            new() { Title = "Lối Nhỏ",                         Lyrics = "<p>Lối nhỏ dẫn về nhà, nơi có mẹ chờ và bữa cơm ấm áp...</p>",             SingerId = singers[3].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-320), Status = 1 },
            new() { Title = "Người Thầy",                      Lyrics = "<p>Người thầy ơi, bao năm tháng thầy đã dìu dắt chúng em...</p>",          SingerId = singers[3].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-300), Status = 1 },
            new() { Title = "Anh Đang Ở Đây",                  Lyrics = "<p>Anh đang ở đây, bên em dù em không hay biết...</p>",                     SingerId = singers[3].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-280), Status = 1 },

            new() { Title = "Không Làm Khó Dễ",               Lyrics = "<p>Yêu nhau thật lòng, đừng làm khó dễ nhau chi...</p>",                    SingerId = singers[4].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-480), Status = 1 },
            new() { Title = "Ông Bà Anh",                      Lyrics = "<p>Ngày xưa ông bà anh yêu nhau giản dị mà bền lâu...</p>",                 SingerId = singers[4].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-460), Status = 1 },
            new() { Title = "Nhớ Về Anh",                      Lyrics = "<p>Mỗi khi nhớ về anh, lòng em lại bâng khuâng không yên...</p>",          SingerId = singers[4].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-440), Status = 1 },
            new() { Title = "Cho Tôi Lại Từ Đầu",              Lyrics = "<p>Cho tôi được bắt đầu lại từ ngày đầu gặp gỡ em...</p>",                  SingerId = singers[4].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-420), Status = 1 },
            new() { Title = "Vì Sao Người Lại Thay Lòng",      Lyrics = "<p>Vì sao người lại thay lòng, khi lời thề chưa kịp phai...</p>",          SingerId = singers[4].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-400), Status = 1 },
            new() { Title = "Đừng Để Em Phải Khóc",            Lyrics = "<p>Đừng để em phải khóc, hãy mãi ở bên em anh nhé...</p>",                  SingerId = singers[4].Id, ComposerId = composers[4].Id, ReleaseDate = baseDate.AddDays(-380), Status = 1 },
            new() { Title = "Ai Chung Tình Được Mãi",          Lyrics = "<p>Ai chung tình được mãi, ai yêu nhau được lâu...</p>",                    SingerId = singers[4].Id, ComposerId = composers[0].Id, ReleaseDate = baseDate.AddDays(-360), Status = 1 },
            new() { Title = "Yêu Anh Dại Khờ",                 Lyrics = "<p>Yêu anh dại khờ mà sao em không thể dừng lại...</p>",                    SingerId = singers[4].Id, ComposerId = composers[1].Id, ReleaseDate = baseDate.AddDays(-340), Status = 1 },
            new() { Title = "Giả Vờ Như Chưa Từng Quen",       Lyrics = "<p>Giả vờ như chưa từng quen, như chưa từng yêu nhau...</p>",               SingerId = singers[4].Id, ComposerId = composers[2].Id, ReleaseDate = baseDate.AddDays(-320), Status = 1 },
            new() { Title = "Thôi Thì",                        Lyrics = "<p>Thôi thì duyên phận đã hết, ta mỗi người mỗi đường...</p>",              SingerId = singers[4].Id, ComposerId = composers[3].Id, ReleaseDate = baseDate.AddDays(-300), Status = 1 },
        };
        await db.Songs.AddRangeAsync(songs);
        await db.SaveChangesAsync();
    }
}
