using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

  [Authorize]
  public class TestimonialController : Controller
  {
    private readonly IRepository<Testimonial> _repository;
    private readonly ILogger<TestimonialController> _logger;

    public TestimonialController(IRepository<Testimonial> repository, ILogger<TestimonialController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var testimonials = await _repository.GetAllAsync();
        return View(testimonials);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving testimonials: {ex.Message}");
        TempData["Error"] = "Referanslar yüklenemedi";
        return View(new List<Testimonial>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Testimonial testimonial)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(testimonial);
        }

        await _repository.AddAsync(testimonial);
        TempData["Success"] = "Referans başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating testimonial: {ex.Message}");
        TempData["Error"] = "Referans eklenirken hata oluştu";
        return View(testimonial);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var testimonial = await _repository.GetByIdAsync(id);
        if (testimonial == null)
        {
          TempData["Error"] = "Referans bulunamadı";
          return RedirectToAction("Index");
        }
        return View(testimonial);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving testimonial: {ex.Message}");
        TempData["Error"] = "Referans yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Testimonial testimonial)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(testimonial);
        }

        await _repository.UpdateAsync(testimonial);
        TempData["Success"] = "Referans başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating testimonial: {ex.Message}");
        TempData["Error"] = "Referans güncellenirken hata oluştu";
        return View(testimonial);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Referans başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting testimonial: {ex.Message}");
        TempData["Error"] = "Referans silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
