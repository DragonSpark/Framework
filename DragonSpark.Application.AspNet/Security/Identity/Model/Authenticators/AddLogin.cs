using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class AddLogin<T> : IAddLogin<T> where T : IdentityUser
{
	readonly IUsers<T> _users;

	public AddLogin(IUsers<T> users) => _users = users;

	public async ValueTask<IdentityResult> Get(Stop<Login<T>> parameter)
	{
		var ((information, subject), _) = parameter;
		using var users  = _users.Get();
		var       result = await users.Subject.AddLoginAsync(subject, information).Off();
		return result;
	}
}