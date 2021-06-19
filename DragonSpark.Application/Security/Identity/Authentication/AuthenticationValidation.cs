using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class AuthenticationValidation<T> : IAuthenticationValidation where T : class
	{
		readonly IStateViews<T> _views;
		readonly string         _type;

		[UsedImplicitly]
		public AuthenticationValidation(IStateViews<T> views, IOptions<IdentityOptions> options)
			: this(views, options.Value.ClaimsIdentity.SecurityStampClaimType) {}

		public AuthenticationValidation(IStateViews<T> views, string type)
		{
			_views = views;
			_type  = type;
		}

		public async ValueTask<bool> Get(ClaimsPrincipal parameter)
		{
			var (_, hash) = await _views.Await(parameter);
			var result = hash is null || parameter.FindFirstValue(_type) == hash;
			return result;
		}
	}
}