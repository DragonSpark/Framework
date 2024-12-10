using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class RemoveLogin<T>(IUsers<T> users) : IRemoveLogin<T> where T : IdentityUser
{
	readonly IUsers<T> _users = users;

	public async ValueTask<IdentityResult> Get(RemoveLoginInput<T> parameter)
	{
		using var users = _users.Get();
		var (user, (provider, identity)) = parameter;
		var found  = await users.Subject.FindByIdAsync(user.Id.ToString()).Await();
		var verify = found.Verify();
		var result = await users.Subject.RemoveLoginAsync(verify, provider, identity).Await();
		return result;
	}
}
