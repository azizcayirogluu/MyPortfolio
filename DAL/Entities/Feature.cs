using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Feature entity
    /// </summary>
    public class Feature
    {
        [Key]
        public int FeatureId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Açıklama 10-1000 karakter arasında olmalıdır")]
        public string? Description { get; set; }
    }
}
