using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Experience component view component
    /// </summary>
    public class _ExperienceComponentPartial : ViewComponent
    {
        private readonly IRepository<Experience> _repository;

        public _ExperienceComponentPartial(IRepository<Experience> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var experiences = await _repository.GetAllAsync();
                var sortedExperiences = experiences.OrderByDescending(x => x.ExperienceId).ToList();
                return View(sortedExperiences);
            }
            catch
            {
                return View(new List<Experience>());
            }
        }
    }
}
