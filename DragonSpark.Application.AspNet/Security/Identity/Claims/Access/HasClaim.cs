using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Conditions;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public class HasClaim<T> : IDepending<T> where T : IdentityUser
{
	readonly IUsers<T> _users;
	readonly string    _claim;

	protected HasClaim(IUsers<T> users, string claim)
	{
		_users = users;
		_claim = claim;
	}

	public async ValueTask<bool> Get(T parameter)
	{
		using var users  = _users.Get();
		var       user   = await users.Subject.FindByIdAsync(parameter.Id.ToString()).Await();
		var       claims = await users.Subject.GetClaimsAsync(user.Verify()).Await();
		var       result = claims.AsValueEnumerable().Select(x => x.Type).Contains(_claim);
		return result;
	}
}