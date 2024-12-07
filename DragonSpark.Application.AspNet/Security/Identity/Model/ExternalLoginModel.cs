using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model;

[AllowAnonymous, RedirectErrors]
public class ExternalLoginModel<T> : PageModel where T : class
{
	readonly IAuthentications<T>  _authentications;
	readonly IAuthenticateRequest _request;
	readonly IFormatter<string>   _path;

	protected ExternalLoginModel(IAuthentications<T> authentications, IAuthenticateRequest request,
	                             IFormatter<string> path)
	{
		_authentications = authentications;
		_request         = request;
		_path            = path;
	}

	public string? LoginProvider { get; private set; }

	public IActionResult OnGet([ModelBinder(typeof(ExternalLoginChallengingModelBinder))] Challenging parameter)
	{
		if (ModelState.IsValid)
		{
			var (provider, origin) = parameter;
			using var authentication = _authentications.Get();
			var       properties = authentication.Subject.ConfigureExternalAuthenticationProperties(provider, origin);
			properties.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1);
			return new ChallengeResult(provider, properties);
		}

		return BadRequest();
	}

	public IActionResult OnPost([ModelBinder(typeof(ExternalLoginChallengingModelBinder))] Challenging context)
		=> OnGet(context);

	public async Task<IActionResult> OnGetCallback([ModelBinder(typeof(ChallengedModelBinder))] Challenged context)
		=> ModelState.IsValid ? await _request.Await(context) ?? Bind(context) : BadRequest();

	IActionResult Bind(Challenged parameter)
	{
		var (login, origin) = parameter;
		LoginProvider       = login.LoginProvider;
		var result = ModelState.ErrorCount > 0 ? (IActionResult)Page() : new LocalRedirectResult(_path.Get(origin));
		return result;
	}
}