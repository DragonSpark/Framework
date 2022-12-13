using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class RemoveLogin<T> : IRemoveLogin<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public RemoveLogin(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(RemoveLoginInput<T> parameter)
	{
		using var users = _users.Get();
		var (user, (provider, identity)) = parameter;
		var found  = await users.Subject.FindByIdAsync(user.Id.ToString());
		var verify = found.Verify();
		var result = await users.Subject.RemoveLoginAsync(verify, provider, identity);
		if (result.Succeeded)
		{
			await users.Subject.UpdateSecurityStampAsync(verify).ConfigureAwait(false);
		}

		return result;
	}
}