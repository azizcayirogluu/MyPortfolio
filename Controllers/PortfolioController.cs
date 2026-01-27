using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{
  [Authorize]
  public class PortfolioController : Controller
  {
    private readonly IRepository<Portfolio> _repository;
    private readonly ILogger<PortfolioController> _logger;

    public PortfolioController(IRepository<Portfolio> repository, ILogger<PortfolioController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var portfolios = await _repository.GetAllAsync();
        return View(portfolios);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving portfolios: {ex.Message}");
        TempData["Error"] = "Portfolyo öğeleri yüklenemedi";
        return View(new List<Portfolio>());
      }
    }

    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Portfolio portfolio)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(portfolio);
        }

        await _repository.AddAsync(portfolio);
        TempData["Success"] = "Portfolyo öğesi başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating portfolio: {ex.Message}");
        TempData["Error"] = "Portfolyo öğesi eklenirken hata oluştu";
        return View(portfolio);
      }
    }

    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var portfolio = await _repository.GetByIdAsync(id);
        if (portfolio == null)
        {
          TempData["Error"] = "Portfolyo öğesi bulunamadı";
          return RedirectToAction("Index");
        }
        return View(portfolio);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving portfolio: {ex.Message}");
        TempData["Error"] = "Portfolyo öğesi yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Portfolio portfolio)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(portfolio);
        }

        await _repository.UpdateAsync(portfolio);
        TempData["Success"] = "Portfolyo öğesi başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating portfolio: {ex.Message}");
        TempData["Error"] = "Portfolyo öğesi güncellenirken hata oluştu";
        return View(portfolio);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "Portfolyo öğesi başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting portfolio: {ex.Message}");
        TempData["Error"] = "Portfolyo öğesi silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
