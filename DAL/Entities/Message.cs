using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Message entity
    /// </summary>
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Ad-Soyad zorunludur")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Ad-Soyad 3-100 karakter arasında olmalıdır")]
        public string? NameSurname { get; set; }

        [Required(ErrorMessage = "Konu zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Konu 3-200 karakter arasında olmalıdır")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mesaj zorunludur")]
        [StringLength(5000, MinimumLength = 10, ErrorMessage = "Mesaj 10-5000 karakter arasında olmalıdır")]
        public string? MessageDetail { get; set; }

        public DateTime SendDate { get; set; } = DateTime.Now;

        public bool IsRead { get; set; } = false;
    }
}
