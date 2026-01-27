using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Portfolio component view component
    /// </summary>
    public class _PortfolioComponentPartial : ViewComponent
    {
        private readonly IRepository<Portfolio> _repository;

        public _PortfolioComponentPartial(IRepository<Portfolio> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var portfolios = await _repository.GetAllAsync();
                return View(portfolios);
            }
            catch
            {
                return View(new List<Portfolio>());
            }
        }
    }
}
