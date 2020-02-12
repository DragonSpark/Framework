using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	sealed class AddClaimsOperation<T> : IIdentityOperation<T> where T : class
	{
		readonly IClaims        _claims;
		readonly UserManager<T> _users;

		public AddClaimsOperation(UserManager<T> users, IClaims claims)
		{
			_users  = users;
			_claims = claims;
		}

		public ValueTask<IdentityResult> Get((ExternalLoginInfo Login, T User) parameter)
			=> _users.AddClaimsAsync(parameter.User, _claims.Get(parameter.Login.Principal.Claims).Open())
			         .ToOperation();
	}
}