using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Skill component view component
    /// </summary>
    public class _SkillComponentPartial : ViewComponent
    {
        private readonly IRepository<Skill> _repository;

        public _SkillComponentPartial(IRepository<Skill> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var skills = await _repository.GetAllAsync();
                return View(skills);
            }
            catch
            {
                return View(new List<Skill>());
            }
        }
    }
}
