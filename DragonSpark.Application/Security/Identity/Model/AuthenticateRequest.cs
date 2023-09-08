using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model;

sealed class AuthenticateRequest : IAuthenticateRequest
{
	readonly IAuthentication _authentication;

	public AuthenticateRequest(IAuthentication authentication) => _authentication = authentication;

	public async ValueTask<IActionResult?> Get(Challenged parameter)
	{
		var (login, origin) = parameter;

		var authentication = await _authentication.Await(login);
		var result = authentication.IsLockedOut
			             ? new RedirectToPageResult("./Lockout")
			             : authentication.Succeeded
				             ? new LocalRedirectResult(origin)
				             : default(IActionResult?);
		return result;
	}
}

// TODO

public sealed class ReturnUrlStore : TableVariable<string, string>
{
	public ReturnUrlStore(ExternalLoginInfo store) : this(store.AuthenticationProperties.Verify().Items) {}

	public ReturnUrlStore(IDictionary<string, string?> items) : this(items.ToTable()) {}

	public ReturnUrlStore(ITable<string, string?> store) : base(nameof(ReturnUrlStore), store) {}
}