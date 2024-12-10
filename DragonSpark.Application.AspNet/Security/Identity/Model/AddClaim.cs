using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model;

public class AddClaim<T> : ISelecting<T, IdentityResult> where T : IdentityUser
{
	readonly IUsers<T>       _users;
	readonly EditExisting<T> _edit;
	readonly Func<T, Claim>  _claim;

	protected AddClaim(IUsers<T> users, EditExisting<T> edit, string type)
		: this(users, edit, new Claim(type, string.Empty).Accept) {}

	protected AddClaim(IUsers<T> users, EditExisting<T> edit, Func<T, Claim> claim)
	{
		_users = users;
		_edit  = edit;
		_claim = claim;
	}

	public async ValueTask<IdentityResult> Get(T parameter)
	{
		using var edit   = await _edit.Get(parameter).ConfigureAwait(true);
		using var users  = _users.Get();
		var       claim  = _claim(parameter);
		var       result = await users.Subject.AddClaimAsync(parameter, claim).Await();
		return result;
	}
}
