using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AuthenticationValidation<T> : IAuthenticationValidation where T : IdentityUser
{
	readonly IStateViews<T>              _views;
	readonly string                      _type;
	readonly ICondition<ClaimsPrincipal> _application;

	[UsedImplicitly]
	public AuthenticationValidation(IStateViews<T> views, IOptions<IdentityOptions> options)
		: this(views, options.Value.ClaimsIdentity.SecurityStampClaimType, IsApplicationPrincipal.Default) {}

	public AuthenticationValidation(IStateViews<T> views, string type, ICondition<ClaimsPrincipal> application)
	{
		_views            = views;
		_type             = type;
		_application = application;
	}

	public async ValueTask<bool> Get(ClaimsPrincipal parameter)
	{
		if (_application.Get(parameter))
		{
			var (_, hash) = await _views.Off(parameter);
			var result = hash is not null && parameter.FindFirstValue(_type) == hash;
			return result;
		}

		return true;
	}
}