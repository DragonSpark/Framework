using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators
{
	sealed class AddExternalSignin<T> : IAddExternalSignin where T : IdentityUser
	{
		readonly IChallenged<T> _challenged;

		public AddExternalSignin(IChallenged<T> challenged) => _challenged = challenged;

		public async ValueTask<IdentityResult?> Get(ClaimsPrincipal parameter)
		{
			var login  = await _challenged.Await(parameter);
			var result = login?.Result;
			return result;
		}
	}
}