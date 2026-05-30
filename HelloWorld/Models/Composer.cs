using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelloWorld.Models;

public class Composer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên nhạc sĩ là bắt buộc")]
    [MaxLength(255)]
    [Display(Name = "Tên nhạc sĩ")]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "longtext")]
    [Display(Name = "Tiểu sử")]
    public string? Biography { get; set; }

    [MaxLength(500)]
    [Display(Name = "Ảnh đại diện")]
    public string? ImageUrl { get; set; }

    public virtual ICollection<Song> Songs { get; set; } = [];
}
