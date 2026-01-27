using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// About page entity
    /// </summary>
    public class About
    {
        [Key]
        public int AboutId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Alt başlık zorunludur")]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Alt başlık 5-500 karakter arasında olmalıdır")]
        public string? SubDescription { get; set; }

        [Required(ErrorMessage = "Detay zorunludur")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Detay 10-5000 karakter arasında olmalıdır")]
        public string? Details { get; set; }
    }
}
