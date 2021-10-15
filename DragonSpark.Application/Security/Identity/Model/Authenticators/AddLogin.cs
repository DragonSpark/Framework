using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class AddLogin<T> : IAddLogin<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public AddLogin(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(Login<T> parameter)
	{
		var (information, subject) = parameter;
		using var users  = _users.Get();
		var       result = await users.Subject.AddLoginAsync(subject, information).ConfigureAwait(false);
		return result;
	}
}