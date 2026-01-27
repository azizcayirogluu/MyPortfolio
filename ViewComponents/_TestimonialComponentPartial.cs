using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Testimonial component view component
    /// </summary>
    public class _TestimonialComponentPartial : ViewComponent
    {
        private readonly IRepository<Testimonial> _repository;

        public _TestimonialComponentPartial(IRepository<Testimonial> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var testimonials = await _repository.GetAllAsync();
                return View(testimonials);
            }
            catch
            {
                return View(new List<Testimonial>());
            }
        }
    }
}