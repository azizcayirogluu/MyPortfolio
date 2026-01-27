using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Experience entity
    /// </summary>
    public class Experience
    {
        [Key]
        public int ExperienceId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Başlık 3-100 karakter arasında olmalıdır")]
        public string? Head { get; set; }

        [Required(ErrorMessage = "Pozisyon zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Pozisyon 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Tarih zorunludur")]
        [StringLength(50)]
        public string? Date { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Açıklama 10-2000 karakter arasında olmalıdır")]
        public string? Description { get; set; }
    }
}
