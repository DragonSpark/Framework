using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class AddExternalSignin<T> : IAddExternalSignin where T : IdentityUser
{
	readonly IChallenged<T> _challenged;

	public AddExternalSignin(IChallenged<T> challenged) => _challenged = challenged;

	public async ValueTask<IdentityResult?> Get(Stop<ClaimsPrincipal> parameter)
	{
		var (_, stop) = parameter;
		var login  = await _challenged.Off(new(parameter, stop));
		var result = login?.Result;
		return result;
	}
}