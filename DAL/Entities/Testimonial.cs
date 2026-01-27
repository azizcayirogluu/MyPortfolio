using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Testimonial entity
    /// </summary>
    public class Testimonial
    {
        [Key]
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "Ad-Soyad zorunludur")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad-Soyad 3-100 karakter arasında olmalıdır")]
        public string? NameSurname { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Açıklama zorunludur")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Açıklama 10-2000 karakter arasında olmalıdır")]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }
    }
}
