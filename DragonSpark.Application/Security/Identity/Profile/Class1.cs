using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public sealed class DisplayName : Text.Text
	{
		public static DisplayName Default { get; } = new DisplayName();

		DisplayName() : base($"{ClaimNamespace.Default}:displayname") {}
	}

	public sealed class ClaimNamespace : Text.Text
	{
		public static ClaimNamespace Default { get; } = new ClaimNamespace();

		ClaimNamespace() : base("urn:dragonspark") {}
	}

	public interface IAppliedPrincipal : ISelect<ExternalLoginInfo, ClaimsPrincipal> {}

	public sealed class DefaultAppliedPrincipal : AppliedPrincipal
	{
		public static DefaultAppliedPrincipal Default { get; } = new DefaultAppliedPrincipal();

		DefaultAppliedPrincipal() : base(new Dictionary<string, string>()) {}
	}

	public class AppliedPrincipal : IAppliedPrincipal
	{
		readonly IReadOnlyDictionary<string, string> _claims;

		public AppliedPrincipal(IReadOnlyDictionary<string, string> claims) => _claims = claims;

		public ClaimsPrincipal Get(ExternalLoginInfo parameter)
		{
			var key        = _claims.TryGetValue(parameter.LoginProvider, out var value) ? value : ClaimTypes.Name;
			var claim      = DisplayName.Default.Claim(parameter.Principal.FindFirst(key));
			var identity   = new ClaimsIdentity(claim.Yield());
			var identities = parameter.Principal.Identities.Append(identity);
			var result     = new ClaimsPrincipal(identities);
			return result;
		}
	}

	interface IAuthenticationProfile : IOperationResult<ExternalLoginInfo> {}

	sealed class AuthenticationProfile : IAuthenticationProfile
	{
		readonly IAuthenticationProfile _profile;
		readonly IAppliedAuthentication _applied;

		public AuthenticationProfile(IAuthenticationProfile profile, IAppliedAuthentication applied)
		{
			_profile = profile;
			_applied = applied;
		}

		public async ValueTask<ExternalLoginInfo> Get() => _applied.Get(await _profile.Get());
	}

	sealed class AuthenticationProfile<T> : IAuthenticationProfile where T : class
	{
		readonly SignInManager<T> _authentication;

		public AuthenticationProfile(SignInManager<T> authentication) => _authentication = authentication;

		public ValueTask<ExternalLoginInfo> Get() => _authentication.GetExternalLoginInfoAsync().ToOperation();
	}
}