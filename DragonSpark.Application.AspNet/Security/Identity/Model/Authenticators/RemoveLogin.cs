using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class RemoveLogin<T>(IUsers<T> users) : IRemoveLogin<T> where T : IdentityUser
{
	readonly IUsers<T> _users = users;

	public async ValueTask<IdentityResult> Get(Stop<RemoveLoginInput<T>> parameter)
	{
		using var users = _users.Get();
		var ((user, (provider, identity)), _) = parameter;
		var found  = await users.Subject.FindByIdAsync(user.Id.ToString()).Off();
		var verify = found.Verify();
		var result = await users.Subject.RemoveLoginAsync(verify, provider, identity).Off();
		return result;
	}
}
