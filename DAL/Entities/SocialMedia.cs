using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Social media link entity
    /// </summary>
    public class SocialMedia
    {
        [Key]
        public int SocialMediaId { get; set; }

        [Required(ErrorMessage = "Platform adı zorunludur")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Platform adı 2-50 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "URL zorunludur")]
        [Url(ErrorMessage = "Geçerli bir URL girin")]
        [StringLength(500)]
        public string? Url { get; set; }

        [Required(ErrorMessage = "İkon adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "İkon adı 2-100 karakter arasında olmalıdır")]
        public string? Icon { get; set; }
    }
}