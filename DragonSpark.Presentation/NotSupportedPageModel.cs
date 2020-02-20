using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DragonSpark.Presentation
{
	public sealed class NotSupportedPageModel : PageModel
	{
		readonly string _page;
		readonly object _routes;

		[UsedImplicitly]
		public NotSupportedPageModel() : this("~/Account/Manage/Index", new {area = "Identity"}) {}

		NotSupportedPageModel(string page, object routes)
		{
			_page   = page;
			_routes = routes;
		}

		public IActionResult OnGet() => RedirectToPage(Url.Content(_page), _routes);

		public IActionResult OnPost() => RedirectToPage(Url.Content(_page), _routes);
	}
}