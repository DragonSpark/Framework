using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using System.Security.Claims;

namespace DragonSpark.Entra;

sealed class LocateEasyAuthUser<T> : ISelecting<ClaimsPrincipal, T?> where T : class
{
	readonly IUsers<T> _users;

	public LocateEasyAuthUser(IUsers<T> users) => _users = users;

	public async ValueTask<T?> Get(ClaimsPrincipal parameter)
	{
		if (parameter.Identity?.AuthenticationType == "EasyAuth" &&
		    !parameter.HasClaim("EasyAuthTransformed", "true") && parameter.Email() is {} email)
		{
			using var users = _users.Get();
			return await users.Subject.FindByEmailAsync(email).Off();
		}

		return null;
	}
}