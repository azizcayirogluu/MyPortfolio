using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.DAL.Entities
{
	/// <summary>
	/// Features (different from Feature) entity
	/// </summary>
	public class Features
	{
		[Key]
		public int FeaturesId { get; set; }

		[Required(ErrorMessage = "Başlık zorunludur")]
		[StringLength(200, MinimumLength = 3, ErrorMessage = "Başlık 3-200 karakter arasında olmalıdır")]
		public string? Title { get; set; }

		[Required(ErrorMessage = "Açıklama zorunludur")]
		[StringLength(1000, MinimumLength = 10, ErrorMessage = "Açıklama 10-1000 karakter arasında olmalıdır")]
		public string? FeatureDescription { get; set; }
	}
}