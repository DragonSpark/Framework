using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class PersistClaims<T> : IPersistClaims<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public PersistClaims(IUsers<T> users) => _users = users;

	public async ValueTask Get(Claims<T> parameter)
	{
		var (user, array) = parameter;
		var       claims  = array.Open();
		using var users   = _users.Get();
		var       subject = await users.Subject.FindByIdAsync(user.Id.ToString());
		await users.Subject.AddClaimsAsync(subject, claims).ConfigureAwait(false);
	}
}