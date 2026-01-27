using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// About component view component
    /// </summary>
    public class _AboutComponentPartial : ViewComponent
    {
        private readonly IRepository<About> _repository;

        public _AboutComponentPartial(IRepository<About> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var about = await _repository.GetFirstByConditionAsync(x => x.AboutId > 0);
                if (about != null)
                {
                    ViewBag.aboutTitle = about.Title;
                    ViewBag.aboutSubDescription = about.SubDescription;
                    ViewBag.aboutDetail = about.Details;
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
