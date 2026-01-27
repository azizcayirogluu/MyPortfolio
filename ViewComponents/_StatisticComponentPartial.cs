using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Statistics component view component
    /// </summary>
    public class _StatisticComponentPartial : ViewComponent
    {
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<Message> _messageRepository;

        public _StatisticComponentPartial(
            IRepository<Skill> skillRepository,
            IRepository<Message> messageRepository)
        {
            _skillRepository = skillRepository ?? throw new ArgumentNullException(nameof(skillRepository));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
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

                return View();
            }
            catch
            {
                ViewBag.SkillCount = 0;
                ViewBag.TotalMessages = 0;
                ViewBag.UnreadMessages = 0;
                ViewBag.ReadMessages = 0;
                return View();
            }
        }
    }
}
