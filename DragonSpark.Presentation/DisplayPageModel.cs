using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DragonSpark.Presentation
{
	public sealed class DisplayPageModel : PageModel
	{
		public IActionResult OnGet() => Page();

		public IActionResult OnPost() => Page();
	}
}