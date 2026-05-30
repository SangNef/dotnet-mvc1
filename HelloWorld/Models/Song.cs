using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;

public class Song
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
    [MinLength(5, ErrorMessage = "Tiêu đề tối thiểu 5 ký tự")]
    [MaxLength(255)]
    [Display(Name = "Tiêu đề")]
    public string Title { get; set; } = string.Empty;

    [Column(TypeName = "longtext")]
    [Display(Name = "Lời bài hát")]
    public string? Lyrics { get; set; }

    [MaxLength(500)]
    [Display(Name = "Ảnh bìa")]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(500)]
    [Display(Name = "Link MP3")]
    public string? Mp3Link { get; set; }

    [Display(Name = "Ngày phát hành")]
    [DataType(DataType.Date)]
    public DateTime? ReleaseDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn ca sĩ")]
    [Display(Name = "Ca sĩ")]
    public int SingerId { get; set; }

    [ForeignKey("SingerId")]
    public virtual Singer? Singer { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn nhạc sĩ")]
    [Display(Name = "Nhạc sĩ")]
    public int ComposerId { get; set; }

    [ForeignKey("ComposerId")]
    public virtual Composer? Composer { get; set; }

    public int Status { get; set; } = 1;
}
