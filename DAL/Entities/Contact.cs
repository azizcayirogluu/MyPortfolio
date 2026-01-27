using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Contact information entity
    /// </summary>
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        [StringLength(20)]
        public string? Phone1 { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin")]
        [StringLength(20)]
        public string? Phone2 { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        [StringLength(100)]
        public string? Email1 { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        [StringLength(100)]
        public string? Email2 { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }
    }
}
