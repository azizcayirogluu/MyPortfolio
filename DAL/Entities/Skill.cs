using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// Skill entity
    /// </summary>
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }

        [Required(ErrorMessage = "Beceri adı zorunludur")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Beceri adı 2-100 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Seviye zorunludur")]
        [Range(0, 100, ErrorMessage = "Seviye 0-100 arasında olmalıdır")]
        public int Value { get; set; }
    }
}
