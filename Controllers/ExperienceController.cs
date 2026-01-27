using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Context;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;
using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.Controllers
{
	[Authorize]
	public class ExperienceController : Controller
	{
		private readonly IRepository<Experience> _repository;
		private readonly ILogger<ExperienceController> _logger;

		public ExperienceController(IRepository<Experience> repository, ILogger<ExperienceController> logger)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public async Task<IActionResult> ExperienceList()
		{
			try
			{
				var values = await _repository.GetAllAsync();
				return View(values);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error getting experience list: {ex.Message}");
				TempData["ErrorMessage"] = "İş deneyimleri yüklenirken bir hata oluştu.";
				return View(new List<Experience>());
			}
		}

		[HttpGet]
		public IActionResult CreateExperience()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateExperience(Experience experience)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(experience);
				}

				if (experience == null)
				{
					throw new ArgumentNullException(nameof(experience));
				}

				await _repository.AddAsync(experience);
				TempData["SuccessMessage"] = "İş deneyimi başarıyla eklendi.";
				return RedirectToAction(nameof(ExperienceList));
			}
			catch (ValidationException vex)
			{
				_logger.LogWarning($"Validation error: {vex.Message}");
				ModelState.AddModelError("", vex.Message);
				return View(experience);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error creating experience: {ex.Message}");
				TempData["ErrorMessage"] = "İş deneyimi eklenirken bir hata oluştu.";
				return View(experience);
			}
		}

		public async Task<IActionResult> DeleteExperience(int id)
		{
			try
			{
				if (id <= 0)
				{
					throw new ArgumentException("Geçersiz ID.", nameof(id));
				}

				var experience = await _repository.GetByIdAsync(id);
				if (experience == null)
				{
					TempData["ErrorMessage"] = "İş deneyimi bulunamadı.";
					return RedirectToAction(nameof(ExperienceList));
				}

				await _repository.RemoveAsync(experience);
				TempData["SuccessMessage"] = "İş deneyimi başarıyla silindi.";
				return RedirectToAction(nameof(ExperienceList));
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error deleting experience: {ex.Message}");
				TempData["ErrorMessage"] = "İş deneyimi silinirken bir hata oluştu.";
				return RedirectToAction(nameof(ExperienceList));
			}
		}

		[HttpGet]
		public async Task<IActionResult> UpdateExperience(int id)
		{
			try
			{
				if (id <= 0)
				{
					throw new ArgumentException("Geçersiz ID.", nameof(id));
				}

				var experience = await _repository.GetByIdAsync(id);
				if (experience == null)
				{
					TempData["ErrorMessage"] = "İş deneyimi bulunamadı.";
					return RedirectToAction(nameof(ExperienceList));
				}

				return View(experience);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error getting experience for update: {ex.Message}");
				TempData["ErrorMessage"] = "İş deneyimi yüklenirken bir hata oluştu.";
				return RedirectToAction(nameof(ExperienceList));
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateExperience(Experience experience)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return View(experience);
				}

				if (experience == null)
				{
					throw new ArgumentNullException(nameof(experience));
				}

				await _repository.UpdateAsync(experience);
				TempData["SuccessMessage"] = "İş deneyimi başarıyla güncellendi.";
				return RedirectToAction(nameof(ExperienceList));
			}
			catch (ValidationException vex)
			{
				_logger.LogWarning($"Validation error: {vex.Message}");
				ModelState.AddModelError("", vex.Message);
				return View(experience);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error updating experience: {ex.Message}");
				TempData["ErrorMessage"] = "İş deneyimi güncellenirken bir hata oluştu.";
				return View(experience);
			}
		}
	}
}
