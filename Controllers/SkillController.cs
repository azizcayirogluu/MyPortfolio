using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

  [Authorize]
  public class SkillController : Controller
  {
    private readonly IRepository<Skill> _repository;
    private readonly ILogger<SkillController> _logger;

    public SkillController(IRepository<Skill> repository, ILogger<SkillController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var skills = await _repository.GetAllAsync();
        return View(skills);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving skills: {ex.Message}");
        TempData["Error"] = "Yetenekler yüklenemedi";
        return View(new List<Skill>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Skill skill)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(skill);
        }

        await _repository.AddAsync(skill);
        TempData["Success"] = "Yetenek başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating skill: {ex.Message}");
        TempData["Error"] = "Yetenek eklenirken hata oluştu";
        return View(skill);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var skill = await _repository.GetByIdAsync(id);
        if (skill == null)
        {
          TempData["Error"] = "Yetenek bulunamadı";
          return RedirectToAction("Index");
        }
        return View(skill);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving skill: {ex.Message}");
        TempData["Error"] = "Yetenek yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Skill skill)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(skill);
        }

        await _repository.UpdateAsync(skill);
        TempData["Success"] = "Yetenek başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating skill: {ex.Message}");
        TempData["Error"] = "Yetenek güncellenirken hata oluştu";
        return View(skill);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Yetenek başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting skill: {ex.Message}");
        TempData["Error"] = "Yetenek silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
