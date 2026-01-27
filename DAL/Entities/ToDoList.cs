using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
    /// <summary>
    /// To-Do List entity
    /// </summary>
    public class ToDoList
    {
        [Key]
        public int ToDoListId { get; set; }

        [Required(ErrorMessage = "Başlık zorunludur")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public bool Status { get; set; } = false;
    }
}
