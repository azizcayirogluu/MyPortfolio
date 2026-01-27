using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.Controllers
{

    public class LayoutController : Controller
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Portfolio> _portfolioRepository;
        private readonly IRepository<About> _aboutRepository;
        private readonly IRepository<Experience> _experienceRepository;
        private readonly IRepository<Contact> _contactRepository;
        private readonly ILogger<LayoutController> _logger;

        public LayoutController(
            IRepository<Skill> skillRepository,
            IRepository<Message> messageRepository,
            IRepository<Portfolio> portfolioRepository,
            IRepository<About> aboutRepository,
            IRepository<Experience> experienceRepository,
            IRepository<Contact> contactRepository,
            ILogger<LayoutController> logger)
        {
            _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _portfolioRepository = portfolioRepository ?? throw new ArgumentNullException(nameof(portfolioRepository));
            _aboutRepository = aboutRepository ?? throw new ArgumentNullException(nameof(aboutRepository));
            _experienceRepository = experienceRepository ?? throw new ArgumentNullException(nameof(experienceRepository));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var skillCount = await _skillRepository.GetCountAsync();
                var messages = await _messageRepository.GetAllAsync();
                var portfolios = await _portfolioRepository.GetCountAsync();
                var abouts = await _aboutRepository.GetCountAsync();
                var experiences = await _experienceRepository.GetCountAsync();
                var contacts = await _contactRepository.GetCountAsync();

                var totalMessages = messages.Count();
                var unreadMessages = messages.Count(x => x.IsRead == false);
                var readMessages = messages.Count(x => x.IsRead == true);

                ViewBag.SkillCount = skillCount;
                ViewBag.PortfolioCount = portfolios;
                ViewBag.AboutCount = abouts;
                ViewBag.ExperienceCount = experiences;
                ViewBag.ContactCount = contacts;
                ViewBag.TotalMessages = totalMessages;
                ViewBag.UnreadMessages = unreadMessages;
                ViewBag.ReadMessages = readMessages;
                ViewBag.UnreadPercentage = totalMessages > 0 ? (unreadMessages * 100) / totalMessages : 0;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading dashboard: {ex.Message}");
                return View();
            }
        }
    }
}
