using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using System.Security.Claims;

namespace DragonSpark.Entra;

sealed class EntraAwareAuthenticationValidation : IAuthenticationValidation
{
	readonly IAuthenticationValidation _previous;

	public EntraAwareAuthenticationValidation(IAuthenticationValidation previous) => _previous = previous;

	public ValueTask<bool> Get(ClaimsPrincipal parameter)
		=> parameter.Identity?.AuthenticationType == "EasyAuth" ? true.ToOperation() : _previous.Get(parameter);
}