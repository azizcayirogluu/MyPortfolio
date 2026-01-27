using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

  [Authorize]
  public class SocialMediaController : Controller
  {
    private readonly IRepository<SocialMedia> _repository;
    private readonly ILogger<SocialMediaController> _logger;

    public SocialMediaController(IRepository<SocialMedia> repository, ILogger<SocialMediaController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var socialMedias = await _repository.GetAllAsync();
        return View(socialMedias);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving social media: {ex.Message}");
        TempData["Error"] = "Sosyal medya bağlantıları yüklenemedi";
        return View(new List<SocialMedia>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SocialMedia socialMedia)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(socialMedia);
        }

        await _repository.AddAsync(socialMedia);
        TempData["Success"] = "Sosyal medya bağlantısı başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating social media: {ex.Message}");
        TempData["Error"] = "Sosyal medya bağlantısı eklenirken hata oluştu";
        return View(socialMedia);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var socialMedia = await _repository.GetByIdAsync(id);
        if (socialMedia == null)
        {
          TempData["Error"] = "Sosyal medya bağlantısı bulunamadı";
          return RedirectToAction("Index");
        }
        return View(socialMedia);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving social media: {ex.Message}");
        TempData["Error"] = "Sosyal medya bağlantısı yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(SocialMedia socialMedia)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(socialMedia);
        }

        await _repository.UpdateAsync(socialMedia);
        TempData["Success"] = "Sosyal medya bağlantısı başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating social media: {ex.Message}");
        TempData["Error"] = "Sosyal medya bağlantısı güncellenirken hata oluştu";
        return View(socialMedia);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Sosyal medya bağlantısı başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting social media: {ex.Message}");
        TempData["Error"] = "Sosyal medya bağlantısı silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
