using System.ComponentModel.DataAnnotations;

namespace HelloWorld.Models;

public class SongViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MinLength(5, ErrorMessage = "Tiêu đề tối thiểu 5 ký tự")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Lời bài hát")]
    public string? Lyrics { get; set; }

    [Display(Name = "Ảnh bìa")]
    public IFormFile? ThumbnailFile { get; set; }

    public string? ThumbnailUrl { get; set; }

    [MaxLength(500)]
    [Display(Name = "Link MP3")]
    public string? Mp3Link { get; set; }

    [Display(Name = "Ngày phát hành")]
    [DataType(DataType.Date)]
    public DateTime? ReleaseDate { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ca sĩ")]
    [Display(Name = "Ca sĩ")]
    public int SingerId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhạc sĩ")]
    [Display(Name = "Nhạc sĩ")]
    public int ComposerId { get; set; }
}
