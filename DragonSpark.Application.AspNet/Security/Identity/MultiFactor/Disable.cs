using System.Threading.Tasks;
using DragonSpark.Compose;

namespace DragonSpark.Application.AspNet.Security.Identity.MultiFactor;

sealed class Disable<T>(IUsers<T> users) : IDisable<T>
	where T : IdentityUser
{
	readonly IUsers<T> _users = users;

	public async ValueTask Get(T parameter)
	{
		using var users = _users.Get();
		var       user  = await users.Subject.FindByIdAsync(parameter.Id.ToString()).Off();
		await users.Subject.SetTwoFactorEnabledAsync(user.Verify(), false).Off();
	}
}
