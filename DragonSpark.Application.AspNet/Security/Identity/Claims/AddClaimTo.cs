using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims;

public class AddClaimTo<T> : IStopAware<T, IdentityResult> where T : IdentityUser
{
	readonly IAuthentications<T>         _sessions;
	readonly Func<T, Claim>              _claim;
	readonly IRefreshUser                _refresh;
	
	public AddClaimTo(IAuthentications<T> sessions, IRefreshUser refresh, string type)
		: this(sessions, refresh, new Claim(type, string.Empty).Accept) {}

	protected AddClaimTo(IAuthentications<T> sessions, IRefreshUser refresh, Func<T, Claim> claim)
	{
		_sessions    = sessions;
		_claim       = claim;
		_refresh     = refresh;
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

			if (result.Succeeded)
			{
				await _refresh.Off(session.Subject.Context.User);	
			}

			return result;
		}

		return remove;
	}
}