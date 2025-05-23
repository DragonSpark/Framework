using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class HasValidState<T> : IHasValidState<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;

	public HasValidState(IAuthentications<T> authentications) => _authentications = authentications;

	public async ValueTask<bool> Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		using var session = _authentications.Get();
		var (authentication, users) = session;
		var user   = await users.FindByIdAsync(subject.Id.ToString()).Off();
		var result = await authentication.ValidateSecurityStampAsync(user, subject.SecurityStamp).Off();
		return result;
	}
}