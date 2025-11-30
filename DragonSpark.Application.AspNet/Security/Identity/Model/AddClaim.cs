using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaim<T> : IStopAware<T, IdentityResult> where T : IdentityUser
{
	readonly IUsers<T>      _users;
	readonly Func<T, Claim> _claim;

	protected AddClaim(IUsers<T> users, string type) : this(users, new Claim(type, string.Empty).Accept) {}

	protected AddClaim(IUsers<T> users, Func<T, Claim> claim)
	{
		_users = users;
		_claim = claim;
	}

	public async ValueTask<IdentityResult> Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		using var users  = _users.Get();
		var       claim  = _claim(parameter);
		var       user   = await users.Subject.FindByIdAsync(subject.Id.ToString()).Off();
		var       result = await users.Subject.AddClaimAsync(user.Verify(), claim).Off();
		return result;
	}
}