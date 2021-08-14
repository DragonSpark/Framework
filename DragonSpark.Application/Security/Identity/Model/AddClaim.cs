using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	public class AddClaim<T> : ISelecting<T, IdentityResult> where T : IdentityUser
	{
		readonly UserManager<T> _users;
		readonly Func<T, Claim> _claim;

		public AddClaim(UserManager<T> users, string type) : this(users, new Claim(type, string.Empty).Accept) {}

		public AddClaim(UserManager<T> users, Func<T, Claim> claim)
		{
			_users = users;
			_claim = claim;
		}

		public async ValueTask<IdentityResult> Get(T parameter)
		{
			var user = await _users.FindByNameAsync(parameter.UserName).ConfigureAwait(false);
			var result = await _users.AddClaimAsync(user, _claim(parameter))
			                         .ConfigureAwait(false);
			return result;
		}
	}
}