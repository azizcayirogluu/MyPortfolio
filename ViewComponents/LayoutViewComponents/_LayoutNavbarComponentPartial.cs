using Microsoft.AspNetCore.Mvc;
using MyPortolioUdemy.DAL.Entities;
using MyPortolioUdemy.Services;

namespace MyPortolioUdemy.ViewComponents.LayoutViewComponents
{
	public class _LayoutNavbarComponentPartial : ViewComponent
	{
		private readonly IRepository<ToDoList> _repository;

		public _LayoutNavbarComponentPartial(IRepository<ToDoList> repository)
		{
			_repository = repository ?? throw new ArgumentNullException(nameof(repository));
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			try
			{
				var pendingTasks = await _repository.GetByConditionAsync(x => x.Status == false);
				ViewBag.toDoListCount = pendingTasks.Count();
				return View(pendingTasks);
			}
			catch
			{
				ViewBag.toDoListCount = 0;
				return View(new List<ToDoList>());
			}
		}
	}
}
