using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public class AddClaimTo<T> : IStopAware<T, IdentityResult> where T : IdentityUser
{
	readonly IAuthentications<T>         _sessions;
	readonly Func<T, Claim>              _claim;
	readonly ICondition<ClaimsPrincipal> _application;

	public AddClaimTo(IAuthentications<T> sessions, string type)
		: this(sessions, new Claim(type, string.Empty).Accept, IsApplicationPrincipal.Default) {}

	protected AddClaimTo(IAuthentications<T> sessions, Func<T, Claim> claim, ICondition<ClaimsPrincipal> application)
	{
		_sessions    = sessions;
		_claim       = claim;
		_application = application;
	}

	public async ValueTask<IdentityResult> Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		using var session = _sessions.Get();
		var       claim   = _claim(parameter);
		var       users   = session.Subject.UserManager;
		var       user    = await users.FindByIdAsync(subject.Id.ToString()).Off();
		var       verify  = user.Verify();

		var remove = await users.RemoveClaimsAsync(verify, claim.Yield()).Off();
		if (remove.Succeeded)
		{
			var result = await users.AddClaimAsync(verify, claim).Off();
			if (_application.Get(session.Subject.Context.User))
			{
				await session.Subject.RefreshSignInAsync(verify).Off();
			}

			return result;
		}

		return remove;
	}
}