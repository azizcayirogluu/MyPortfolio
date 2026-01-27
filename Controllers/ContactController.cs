using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{
  [Authorize]
  public class ContactController : Controller
  {
    private readonly IRepository<Contact> _repository;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IRepository<Contact> repository, ILogger<ContactController> logger)
    {
      _repository = repository ?? throw new ArgumentNullException(nameof(repository));
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IActionResult> Index()
    {
      try
      {
        var contacts = await _repository.GetAllAsync();
        return View(contacts);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving contacts: {ex.Message}");
        TempData["Error"] = "İletişim bilgileri yüklenemedi";
        return View(new List<Contact>());
      }
    }


    [HttpGet]
    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contact contact)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(contact);
        }

        await _repository.AddAsync(contact);
        TempData["Success"] = "İletişim bilgisi başarıyla eklendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error creating contact: {ex.Message}");
        TempData["Error"] = "İletişim bilgisi eklenirken hata oluştu";
        return View(contact);
      }
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
      try
      {
        var contact = await _repository.GetByIdAsync(id);
        if (contact == null)
        {
          TempData["Error"] = "İletişim bilgisi bulunamadı";
          return RedirectToAction("Index");
        }
        return View(contact);
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error retrieving contact: {ex.Message}");
        TempData["Error"] = "İletişim bilgisi yüklenemedi";
        return RedirectToAction("Index");
      }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Contact contact)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return View(contact);
        }

        await _repository.UpdateAsync(contact);
        TempData["Success"] = "İletişim bilgisi başarıyla güncellendi";
        return RedirectToAction("Index");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error updating contact: {ex.Message}");
        TempData["Error"] = "İletişim bilgisi güncellenirken hata oluştu";
        return View(contact);
      }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
      try
      {
        await _repository.RemoveByIdAsync(id);
        TempData["Success"] = "İletişim bilgisi başarıyla silindi";
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error deleting contact: {ex.Message}");
        TempData["Error"] = "İletişim bilgisi silinirken hata oluştu";
      }
      return RedirectToAction("Index");
    }
  }
}
