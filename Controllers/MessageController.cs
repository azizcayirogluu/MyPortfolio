using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

    [Authorize]
    public class MessageController : Controller
    {
        private readonly IRepository<Message> _repository;
        private readonly ILogger<MessageController> _logger;
        private readonly IEmailService _emailService;

        public MessageController(IRepository<Message> repository, ILogger<MessageController> logger, IEmailService emailService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task<IActionResult> Inbox()
        {
            try
            {
                var values = await _repository.GetAllAsync();
                var sortedValues = values.OrderByDescending(x => x.SendDate).ToList();
                return View(sortedValues);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting inbox: {ex.Message}");
                TempData["ErrorMessage"] = "Mesajlar yüklenirken bir hata oluştu.";
                return View(new List<Message>());
            }
        }
        public async Task<IActionResult> ChangeIsReadToTrue(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var message = await _repository.GetByIdAsync(id);
                if (message == null)
                {
                    TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                message.IsRead = true;
                await _repository.UpdateAsync(message);
                return RedirectToAction(nameof(Inbox));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking message as read: {ex.Message}");
                TempData["ErrorMessage"] = "Mesaj işaretlenirken bir hata oluştu.";
                return RedirectToAction(nameof(Inbox));
            }
        }

        public async Task<IActionResult> ChangeIsReadToFalse(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var message = await _repository.GetByIdAsync(id);
                if (message == null)
                {
                    TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                message.IsRead = false;
                await _repository.UpdateAsync(message);
                return RedirectToAction(nameof(Inbox));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error marking message as unread: {ex.Message}");
                TempData["ErrorMessage"] = "Mesaj işaretlenirken bir hata oluştu.";
                return RedirectToAction(nameof(Inbox));
            }
        }
        public async Task<IActionResult> DeleteMessage(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var message = await _repository.GetByIdAsync(id);
                if (message == null)
                {
                    TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                await _repository.RemoveAsync(message);
                TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
                return RedirectToAction(nameof(Inbox));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting message: {ex.Message}");
                TempData["ErrorMessage"] = "Mesaj silinirken bir hata oluştu.";
                return RedirectToAction(nameof(Inbox));
            }
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Message message)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    var errorMessage = string.Join(", ", errors.Select(e => e.ErrorMessage));
                    _logger.LogError($"Validation error: {errorMessage}");
                    return BadRequest(errorMessage);
                }

                message.SendDate = DateTime.Now;
                await _repository.AddAsync(message);

                // Send email to admin
                var emailSent = await _emailService.SendAdminNotificationAsync(
                    message.NameSurname,
                    message.Email,
                    message.Subject,
                    message.MessageDetail
                );

                if (emailSent)
                {
                    _logger.LogInformation($"Message saved and email sent from {message.Email}");
                }
                else
                {
                    _logger.LogWarning($"Message saved but email failed for {message.Email}");
                }

                return Content("OK");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating message: {ex.Message}");
                return BadRequest($"Mesaj gönderilemedi: {ex.Message}");
            }
        }

        public async Task<IActionResult> MessageDetail(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var message = await _repository.GetByIdAsync(id);
                if (message == null)
                {
                    TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                if (!message.IsRead)
                {
                    message.IsRead = true;
                    await _repository.UpdateAsync(message);
                }

                return View(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting message detail: {ex.Message}");
                TempData["ErrorMessage"] = "Mesaj yüklenirken bir hata oluştu.";
                return RedirectToAction(nameof(Inbox));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Reply(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("Geçersiz ID.", nameof(id));
                }

                var message = await _repository.GetByIdAsync(id);
                if (message == null)
                {
                    TempData["ErrorMessage"] = "Mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                var replyModel = new
                {
                    MessageId = message.MessageId,
                    OriginalSenderName = message.NameSurname,
                    OriginalSenderEmail = message.Email,
                    OriginalSubject = message.Subject,
                    OriginalMessage = message.MessageDetail
                };

                ViewBag.Message = message;
                return View(replyModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting reply form: {ex.Message}");
                TempData["ErrorMessage"] = "Yanıt formunu yüklerken bir hata oluştu.";
                return RedirectToAction(nameof(Inbox));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(int messageId, string replyText)
        {
            try
            {
                if (messageId <= 0 || string.IsNullOrEmpty(replyText))
                {
                    TempData["ErrorMessage"] = "Geçersiz parametreler.";
                    return RedirectToAction(nameof(Inbox));
                }

                var originalMessage = await _repository.GetByIdAsync(messageId);
                if (originalMessage == null)
                {
                    TempData["ErrorMessage"] = "Orijinal mesaj bulunamadı.";
                    return RedirectToAction(nameof(Inbox));
                }

                // Prepare reply email
                var replyText_Formatted = replyText.Replace("\n", "<br/>");
                var originalMsg_Formatted = originalMessage.MessageDetail.Replace("\n", "<br/>");

                var replyEmailBody = $@"<html><body style='font-family: Arial, sans-serif;'>
                    <h2>Mesajınıza Yanıt Alındı</h2>
                    <p>Merhaba <strong>{originalMessage.NameSurname}</strong>,</p>
                    <p>Gönderdiğiniz mesaja yanıt verildi:</p>
                    <hr />
                    <p><strong>Yanıt:</strong></p>
                    <p>{replyText_Formatted}</p>
                    <hr />
                    <p><strong>Orijinal Mesajınız:</strong></p>
                    <p><strong>Başlık:</strong> {originalMessage.Subject}</p>
                    <p><strong>İçerik:</strong></p>
                    <p>{originalMsg_Formatted}</p>
                    <hr />
                    <p style='color: #999; font-size: 12px;'>Bu mesaj otomatik olarak gönderilmiştir.</p>
                </body></html>";

                // Try to send email but don't fail if it doesn't work
                bool emailSent = false;
                try
                {
                    emailSent = await _emailService.SendEmailAsync(
                        originalMessage.Email,
                        $"Re: {originalMessage.Subject}",
                        replyEmailBody,
                        "Admin"
                    );
                }
                catch (Exception emailEx)
                {
                    _logger.LogWarning($"Email sending failed but message was processed: {emailEx.Message}");
                    emailSent = false;
                }

                // Show appropriate message
                if (emailSent)
                {
                    _logger.LogInformation($"Reply sent to {originalMessage.Email}");
                    TempData["SuccessMessage"] = "Yanıt başarıyla gönderildi.";
                }
                else
                {
                    _logger.LogWarning($"Reply recorded but email could not be sent to {originalMessage.Email}");
                    TempData["WarningMessage"] = "Yanıt kaydedildi. E-mail gönderimi yapılandırılmamış.";
                }

                return RedirectToAction(nameof(Inbox));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in reply action: {ex.Message}");
                TempData["ErrorMessage"] = $"Hata: {ex.Message}";
                return RedirectToAction(nameof(Inbox));
            }
        }
    }
}
