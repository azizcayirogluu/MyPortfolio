using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    public class _FooterComponentPartial : ViewComponent
    {
        private readonly IRepository<SocialMedia> _repository;

        public _FooterComponentPartial(IRepository<SocialMedia> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var socialMediaList = await _repository.GetAllAsync();
                return View(socialMediaList ?? new List<SocialMedia>());
            }
            catch
            {
                return View(new List<SocialMedia>());
            }
        }
    }
}
