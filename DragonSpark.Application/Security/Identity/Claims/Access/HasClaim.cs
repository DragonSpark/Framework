using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Claims.Access
{
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
		readonly UserManager<T> _users;
		readonly string         _claim;

		public HasClaim(UserManager<T> users, string claim)
		{
			_users = users;
			_claim = claim;
		}

		public async ValueTask<bool> Get(T parameter)
		{
			var user   = await _users.FindByNameAsync(parameter.UserName).ConfigureAwait(false);
			var claims = await _users.GetClaimsAsync(user).ConfigureAwait(false);
			var result = claims.AsValueEnumerable().Select(x => x.Type).Contains(_claim);
			return result;
		}
	}
}