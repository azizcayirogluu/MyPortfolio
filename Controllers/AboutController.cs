using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{
  [Authorize]
  public class AboutController : Controller
  {
    private readonly IRepository<About> _repository;
    private readonly ILogger<AboutController> _logger;

    public AboutController(IRepository<About> repository, ILogger<AboutController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var abouts = await _repository.GetAllAsync();
        return View(abouts);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving abouts: {ex.Message}");
        TempData["Error"] = "Hakkımda bilgileri yüklenemedi";
        return View(new List<About>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(About about)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(about);
        }

        await _repository.AddAsync(about);
        TempData["Success"] = "Hakkımda bilgisi başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating about: {ex.Message}");
        TempData["Error"] = "Hakkımda bilgisi eklenirken hata oluştu";
        return View(about);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var about = await _repository.GetByIdAsync(id);
        if (about == null)
        {
          TempData["Error"] = "Hakkımda bilgisi bulunamadı";
          return RedirectToAction("Index");
        }
        return View(about);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving about: {ex.Message}");
        TempData["Error"] = "Hakkımda bilgisi yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(About about)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(about);
        }

        await _repository.UpdateAsync(about);
        TempData["Success"] = "Hakkımda bilgisi başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating about: {ex.Message}");
        TempData["Error"] = "Hakkımda bilgisi güncellenirken hata oluştu";
        return View(about);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Hakkımda bilgisi başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting about: {ex.Message}");
        TempData["Error"] = "Hakkımda bilgisi silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
