using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public class IsAuthorized : IDepending<ClaimsPrincipal>
	{
		readonly IAuthorizationService _authorization;
		readonly string                _policy;

		protected IsAuthorized(IAuthorizationService authorization, string policy)
		{
			_authorization = authorization;
			_policy        = policy;
		}

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			var authorization = await _authorization.AuthorizeAsync(parameter, null, _policy).ConfigureAwait(false);
			var result        = authorization.Succeeded;
			return result;
		}
	}

}