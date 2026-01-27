using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Feature component view component
    /// </summary>
    public class _FeatureComponentPartial : ViewComponent
    {
        private readonly IRepository<Feature> _repository;

        public _FeatureComponentPartial(IRepository<Feature> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var features = await _repository.GetAllAsync();
                return View(features);
            }
            catch
            {
                return View(new List<Feature>());
            }
        }
    }
}
