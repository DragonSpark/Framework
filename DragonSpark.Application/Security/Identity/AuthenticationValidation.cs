using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class AuthenticationValidation<T> : IAuthenticationValidation where T : class
	{
		readonly IStateViews<T> _views;
		readonly string           _type;

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
			var state  = await _views.Get(parameter).ConfigureAwait(false);
			var result = state.Hash is null || parameter.FindFirstValue(_type) == state.Hash;
			return result;
		}
	}
}