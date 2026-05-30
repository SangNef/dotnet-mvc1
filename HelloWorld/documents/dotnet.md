# MUSIC MANAGEMENT SYSTEM

## 1. Mục tiêu dự án

Thiết lập một ứng dụng quản lý âm nhạc dựa trên nền tảng ASP.NET Core 8.0 MVC và MySQL. Dự án yêu cầu giải quyết bài toán quản lý dữ liệu với nhiều khóa ngoại (1-N từ hai phía), thực hiện phân trang, tìm kiếm nâng cao, tích hợp dịch vụ lưu trữ ảnh và xử lý văn bản giàu.

---

## 2. Đặc tả cơ sở dữ liệu

Hệ thống gồm ba thực thể chính: bài hát, ca sĩ, nhạc sĩ. Một bài hát thuộc về một Ca sĩ và một Nhạc sĩ.

### 2.1. Thực thể Singer (Ca sĩ) và Composer (Nhạc sĩ)

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | `int` | Khóa chính, tự tăng |
| `Name` | `string` | Tên nghệ sĩ (Bắt buộc) |
| `Biography` | `string` | Tiểu sử chi tiết (Lưu mã HTML từ CKEditor) |
| `ImageUrl` | `string` | Ảnh đại diện nghệ sĩ (Lưu link Cloudinary) |

### 2.2. Thực thể Song (Bài hát)

| Trường | Kiểu | Mô tả |
|--------|------|-------|
| `Id` | `int` | Khóa chính, tự tăng |
| `Title` | `string` | Tiêu đề bài hát (Bắt buộc) |
| `Lyrics` | `string` | Lời bài hát hoặc nội dung chi tiết (Mã HTML) |
| `ThumbnailUrl` | `string` | Ảnh bìa bài hát (Lưu link Cloudinary) |
| `Mp3Link` | `string` | Link file mp3 |
| `ReleaseDate` | `DateTime` | Ngày phát hành |
| `CreatedAt` | `DateTime` | Ngày tạo |
| `UpdatedAt` | `DateTime` | Ngày update |
| `SingerId` | `int` | Khóa ngoại tham chiếu đến bảng Singer |
| `ComposerId` | `int` | Khóa ngoại tham chiếu đến bảng Composer |
| `Status` | `int` | `1` = Active (Hiển thị), `0` = Soft Deleted (Đã xóa) |

---

## 3. Yêu cầu chức năng chi tiết

### 3.1. Khởi tạo dữ liệu (Data Seeding)

Triển khai `DbInitializer` thực hiện:

- Reset dữ liệu cũ tại môi trường Development.
- Khởi tạo ít nhất 05 Ca sĩ và 05 Nhạc sĩ mẫu.
- Khởi tạo 50 bài hát mẫu.

### 3.2. Quản lý danh sách bài hát (Index) - Tìm kiếm & Phân trang

- **Tìm kiếm:** Cho phép tìm kiếm bài hát theo tiêu đề (`Title`).
- **Play:** Có button play bài hát, cho phép click và mở bài hát vừa chọn.
- **Hiển thị:** Hiển thị thông tin bài hát kèm tên Ca sĩ và Nhạc sĩ (Sử dụng `.Include()`).
- **Phân trang:** Chia danh sách bài hát thành nhiều trang (ví dụ: 10 bài hát/trang). Yêu cầu hiển thị thanh điều hướng trang (Previous, Next, các số trang).

### 3.3. Thêm mới và Cập nhật (Create/Update)

- **Dropdown:** Cung cấp 02 danh sách chọn (Select menu) cho Ca sĩ và Nhạc sĩ.
- **Hình ảnh:** Tải ảnh lên Cloudinary và lưu URL vào trường `ThumbnailUrl`.
- **Nội dung:** Tích hợp CKEditor 5 để nhập lời bài hát (`Lyrics`).
- **Validation:**
  - Tiêu đề bài hát tối thiểu 5 ký tự.
  - Bắt buộc chọn Ca sĩ và Nhạc sĩ.
  - Ngày phát hành không được lớn hơn ngày hiện tại.

### 3.4. Chi tiết và Xóa mềm

- **Details:** Hiển thị đầy đủ thông tin bài hát, render lời bài hát từ HTML bằng `@Html.Raw()`. Có nút play bài hát.
- **Delete:** Sử dụng JavaScript (AJAX/Fetch) để gọi Action xóa. Không xóa bản ghi khỏi DB mà chuyển `Status` về `0`.

---

## 4. Yêu cầu kỹ thuật

- **Framework:** .NET 8.0 MVC, EF Core (Pomelo Provider).
- **Kiến trúc:** Sử dụng ViewModel để nhận dữ liệu từ Form (bao gồm `IFormFile` cho ảnh).
- **Thư viện:** `CloudinaryDotNet`, CKEditor 5 (CDN), Bootstrap 5.
- **Quy tắc:** Thực hiện thủ công (No Scaffolding), áp dụng Dependency Injection.

---

## 5. Thang điểm (Tổng 10 điểm)

| Hạng mục | Điểm | Tiêu chí |
|----------|------|----------|
| Thiết lập Model & Quan hệ | 1.5đ | Cấu hình đúng thực thể, khóa ngoại và quan hệ Một - Nhiều giữa 3 bảng |
| Khởi tạo dữ liệu - Seeder | 1.0đ | Reset và Seed dữ liệu mẫu thành công bằng `DbInitializer` |
| Chức năng Danh sách & Tìm kiếm | 2.0đ | Hiển thị đúng dữ liệu liên kết và tìm kiếm chính xác theo từ khóa |
| Chức năng Phân trang | 1.5đ | Thực hiện phân trang logic tại Server và hiển thị thanh điều hướng tại View |
| Thêm mới & Cập nhật | 2.0đ | Tích hợp thành công Dropdown đôi, Cloudinary Upload và CKEditor |
| Xử lý Xóa mềm & JavaScript | 1.0đ | Thực hiện xóa qua AJAX và cập nhật trạng thái `Status` |
| Chất lượng mã nguồn & UI | 1.0đ | Code sạch, sử dụng ViewModel, giao diện Bootstrap dễ nhìn |

---

## 6. Hướng dẫn setup quan hệ đặc thù (2 Khóa ngoại)

### 6.1. Cấu trúc thực thể Song

```csharp
public class Song
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int SingerId { get; set; }

    [ForeignKey("SingerId")]
    public virtual Singer? Singer { get; set; }

    public int ComposerId { get; set; }

    [ForeignKey("ComposerId")]
    public virtual Composer? Composer { get; set; }

    public int Status { get; set; } = 1;
}
```

### 6.2. Cấu hình Fluent API trong AppDbContext

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Quan hệ Song - Singer
    modelBuilder.Entity<Song>()
        .HasOne(s => s.Singer)
        .WithMany(a => a.Songs)
        .HasForeignKey(s => s.SingerId)
        .OnDelete(DeleteBehavior.Restrict);

    // Quan hệ Song - Composer
    modelBuilder.Entity<Song>()
        .HasOne(s => s.Composer)
        .WithMany(c => c.Songs)
        .HasForeignKey(s => s.ComposerId)
        .OnDelete(DeleteBehavior.Restrict);
}
```

### 6.3. Logic Tìm kiếm và Phân trang gợi ý

```csharp
public async Task<IActionResult> Index(string searchTerm, int page = 1)
{
    int pageSize = 10;

    var query = _context.Songs
        .Include(s => s.Singer)
        .Include(s => s.Composer)
        .Where(s => s.Status == 1);

    if (!string.IsNullOrEmpty(searchTerm))
    {
        query = query.Where(s => s.Title.Contains(searchTerm));
    }

    var totalItems = await query.CountAsync();

    var songs = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // Truyền dữ liệu phân trang qua ViewModel hoặc ViewBag
    return View(songs);
}
```
