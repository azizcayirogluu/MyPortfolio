using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly ILogger<StatisticController> _logger;

        public StatisticController(
            IRepository<Skill> skillRepository,
            IRepository<Message> messageRepository,
            ILogger<StatisticController> logger)
        {
            _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var skillCount = await _skillRepository.GetCountAsync();
                var allMessages = await _messageRepository.GetAllAsync();
                var totalMessages = allMessages.Count();
                var unreadMessages = allMessages.Count(x => x.IsRead == false);
                var readMessages = allMessages.Count(x => x.IsRead == true);

                ViewBag.SkillCount = skillCount;
                ViewBag.TotalMessages = totalMessages;
                ViewBag.UnreadMessages = unreadMessages;
                ViewBag.ReadMessages = readMessages;
                ViewBag.UnreadPercentage = totalMessages > 0 ? (unreadMessages * 100) / totalMessages : 0;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting statistics: {ex.Message}");
                ViewBag.SkillCount = 0;
                ViewBag.TotalMessages = 0;
                ViewBag.UnreadMessages = 0;
                ViewBag.ReadMessages = 0;
                ViewBag.UnreadPercentage = 0;
                return View();
            }
        }
    }
}
