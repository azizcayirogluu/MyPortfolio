using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

  [Authorize]
  public class FeatureController : Controller
  {
    private readonly IRepository<Feature> _repository;
    private readonly ILogger<FeatureController> _logger;

    public FeatureController(IRepository<Feature> repository, ILogger<FeatureController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    public async Task<IActionResult> Index()
    {
      try
      {
        var features = await _repository.GetAllAsync();
        return View(features);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving features: {ex.Message}");
        TempData["Error"] = "Öne çıkan alanlar yüklenemedi";
        return View(new List<Feature>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Feature feature)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(feature);
        }

        await _repository.AddAsync(feature);
        TempData["Success"] = "Öne çıkan alan başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating feature: {ex.Message}");
        TempData["Error"] = "Öne çıkan alan eklenirken hata oluştu";
        return View(feature);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var feature = await _repository.GetByIdAsync(id);
        if (feature == null)
        {
          TempData["Error"] = "Öne çıkan alan bulunamadı";
          return RedirectToAction("Index");
        }
        return View(feature);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving feature: {ex.Message}");
        TempData["Error"] = "Öne çıkan alan yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Feature feature)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(feature);
        }

        await _repository.UpdateAsync(feature);
        TempData["Success"] = "Öne çıkan alan başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating feature: {ex.Message}");
        TempData["Error"] = "Öne çıkan alan güncellenirken hata oluştu";
        return View(feature);
      }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Öne çıkan alan başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting feature: {ex.Message}");
        TempData["Error"] = "Öne çıkan alan silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
