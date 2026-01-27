using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Portfolio project entity
    /// </summary>
    public class Portfolio
    {
        [Key]
        public int PortfolioId { get; set; }

        [Required(ErrorMessage = "Proje adı zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Proje adı 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Alt başlık zorunludur")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Alt başlık 3-300 karakter arasında olmalıdır")]
        public string? SubTitle { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Geçerli bir URL girin")]
        [StringLength(500)]
        public string? Url { get; set; }

        [StringLength(2000)]
        public string? Description { get; set; }
    }
}
