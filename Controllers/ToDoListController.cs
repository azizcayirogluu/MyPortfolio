using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;
using System.ComponentModel.DataAnnotations;

namespace MyPortolioUdemy.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly IRepository<ToDoList> _repository;
        private readonly ILogger<ToDoListController> _logger;

        public ToDoListController(IRepository<ToDoList> repository, ILogger<ToDoListController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var values = await _repository.GetAllAsync();
                var sortedValues = values.OrderByDescending(x => x.Date).ToList();
                return View(sortedValues);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting to-do list: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacaklar yüklenirken bir hata oluştu.";
                return View(new List<ToDoList>());
            }
        }
        [HttpGet]
        public IActionResult CreateToDoList()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateToDoList(ToDoList toDoList)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(toDoList);
                }

                if (toDoList == null)
                {
                    throw new ArgumentNullException(nameof(toDoList));
                }

                toDoList.Status = false;
                toDoList.Date = DateTime.Now;
                await _repository.AddAsync(toDoList);
                TempData["SuccessMessage"] = "Yapılacak başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning($"Validation error: {vex.Message}");
                ModelState.AddModelError("", vex.Message);
                return View(toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating to-do item: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak eklenirken bir hata oluştu.";
                return View(toDoList);
            }
        }
        public async Task<IActionResult> DeleteToDoList(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var toDoList = await _repository.GetByIdAsync(id);
                if (toDoList == null)
                {
                    TempData["ErrorMessage"] = "Yapılacak bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                await _repository.RemoveAsync(toDoList);
                TempData["SuccessMessage"] = "Yapılacak başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting to-do item: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak silinirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateToDoList(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var toDoList = await _repository.GetByIdAsync(id);
                if (toDoList == null)
                {
                    TempData["ErrorMessage"] = "Yapılacak bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                return View(toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting to-do item for update: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateToDoList(ToDoList toDoList)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(toDoList);
                }

                if (toDoList == null)
                {
                    throw new ArgumentNullException(nameof(toDoList));
                }

                await _repository.UpdateAsync(toDoList);
                TempData["SuccessMessage"] = "Yapılacak başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException vex)
            {
                _logger.LogWarning($"Validation error: {vex.Message}");
                ModelState.AddModelError("", vex.Message);
                return View(toDoList);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating to-do item: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak güncellenirken bir hata oluştu.";
                return View(toDoList);
            }
        }

        public async Task<IActionResult> ChangeToDoListStatusToTrue(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var toDoList = await _repository.GetByIdAsync(id);
                if (toDoList == null)
                {
                    TempData["ErrorMessage"] = "Yapılacak bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                toDoList.Status = true;
                await _repository.UpdateAsync(toDoList);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating to-do status: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak durumu güncellenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> ChangeToDoListStatusToFalse(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var toDoList = await _repository.GetByIdAsync(id);
                if (toDoList == null)
                {
                    TempData["ErrorMessage"] = "Yapılacak bulunamadı.";
                    return RedirectToAction(nameof(Index));
                }

                toDoList.Status = false;
                await _repository.UpdateAsync(toDoList);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating to-do status: {ex.Message}");
                TempData["ErrorMessage"] = "Yapılacak durumu güncellenirken bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
