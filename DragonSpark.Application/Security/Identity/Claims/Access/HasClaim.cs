using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using NetFabric.Hyperlinq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Claims.Access;

public class HasClaim : ICondition<ClaimsPrincipal>
{
	readonly string _claim;
	readonly string _value;

	public HasClaim(string claim) : this(claim, bool.TrueString) {}

	public HasClaim(string claim, string value)
	{
		_claim = claim;
		_value = value;
	}

	public bool Get(ClaimsPrincipal parameter) => parameter.HasClaim(_claim, _value);
}

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
		var       user   = await users.Subject.FindByIdAsync(parameter.Id.ToString()).ConfigureAwait(false);
		var       claims = await users.Subject.GetClaimsAsync(user).ConfigureAwait(false);
		var       result = claims.AsValueEnumerable().Select(x => x.Type).Contains(_claim);
		return result;
	}
}