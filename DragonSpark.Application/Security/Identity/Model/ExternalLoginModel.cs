using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	[AllowAnonymous, RedirectErrors]
	public class ExternalLoginModel<T> : PageModel where T : class
	{
		readonly ExternalLoginModelActions<T> _actions;

		public ExternalLoginModel(ExternalLoginModelActions<T> actions) => _actions = actions;

		public string? LoginProvider { get; private set; }

		public IActionResult OnGet([ModelBinder(typeof(ExternalLoginChallengingModelBinder))]
		                           Challenging context)
			=> _actions.Get(context);

		public IActionResult OnPost([ModelBinder(typeof(ExternalLoginChallengingModelBinder))]
		                            Challenging context)
			=> _actions.Get(context);

		public async Task<IActionResult> OnGetCallback([ModelBinder(typeof(ChallengedModelBinder))]
		                                               Challenged context)
			=> await _actions.Get(context) ?? (await _actions.Get((context.Login, ModelState))
				                                   ? LocalRedirect(context.Origin)
				                                   : Bind(context.Login));

		IActionResult Bind(UserLoginInfo login)
		{
			LoginProvider = login.LoginProvider;

			return Page();
		}
	}
}