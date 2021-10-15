using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model;

public class AddClaim<T> : ISelecting<T, IdentityResult> where T : IdentityUser
{
	readonly IUsers<T>      _users;
	readonly Func<T, Claim> _claim;

	public AddClaim(IUsers<T> users, string type) : this(users, new Claim(type, string.Empty).Accept) {}

	public AddClaim(IUsers<T> users, Func<T, Claim> claim)
	{
		_users = users;
		_claim = claim;
	}

	public async ValueTask<IdentityResult> Get(T parameter)
	{
		using var users  = _users.Get();
		var       user   = await users.Subject.FindByIdAsync(parameter.Id.ToString()).ConfigureAwait(false);
		var       result = await users.Subject.AddClaimAsync(user, _claim(parameter)).ConfigureAwait(false);
		return result;
	}
}