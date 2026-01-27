using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents
{
    /// <summary>
    /// Contact component view component
    /// </summary>
    public class _ContactComponentPartial : ViewComponent
    {
        private readonly IRepository<Contact> _repository;

        public _ContactComponentPartial(IRepository<Contact> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var contact = await _repository.GetFirstByConditionAsync(x => x.ContactId > 0);
                return View(contact ?? new Contact());
            }
            catch
            {
                return View(new Contact());
            }
        }
    }
}
