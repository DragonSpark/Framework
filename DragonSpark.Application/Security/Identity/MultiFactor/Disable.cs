using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class Disable<T> : IDisable<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public Disable(IUsers<T> users) => _users = users;

	public async ValueTask Get(T parameter)
	{
		using var users = _users.Get();
		var       user  = await users.Subject.FindByIdAsync(parameter.Id.ToString()).ConfigureAwait(false);
		await users.Subject.SetTwoFactorEnabledAsync(user, false);
	}
}