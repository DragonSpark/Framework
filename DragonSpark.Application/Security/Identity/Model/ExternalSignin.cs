using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	sealed class ExternalSignin<T> : IExternalSignin where T : IdentityUser
	{
		readonly ILocateUser<T>   _locate;
		readonly IAuthenticate<T> _authenticate;

		public ExternalSignin(ILocateUser<T> locate, IAuthenticate<T> authenticate)
		{
			_locate       = locate;
			_authenticate = authenticate;
		}

		public async ValueTask<SignInResult> Get(ExternalLoginInfo parameter)
		{
			var user = await _locate.Await(parameter);
			if (user != null)
			{
				await _authenticate.Await(new(parameter, user));
				return SignInResult.Success;
			}

			return SignInResult.Failed;
		}
	}

	public interface IAuthenticate<T> : IOperation<Login<T>> where T : IdentityUser {}

	sealed class Authenticate<T> : IAuthenticate<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly IClaims<T>       _claims;
		readonly bool             _persist;

		public Authenticate(SignInManager<T> authentication, IClaims<T> claims, bool persist = true)
		{
			_authentication = authentication;
			_claims         = claims;
			_persist        = persist;
		}

		public async ValueTask Get(Login<T> parameter)
		{
			var (_, user) = parameter;
			var claims = _claims.Get(parameter);
			await _authentication.SignInWithClaimsAsync(user, _persist, claims);
		}
	}

	public interface IDisplayNameClaim : ISelect<ExternalLoginInfo, string> {}

	sealed class DisplayNameClaim : IDisplayNameClaim
	{
		public static DisplayNameClaim Default { get; } = new DisplayNameClaim();

		DisplayNameClaim() {}

		public string Get(ExternalLoginInfo parameter) => ClaimTypes.Name;
	}

	public interface IClaims<T> : ISelect<Login<T>, IEnumerable<Claim>> {}

	sealed class Claims<T> : IClaims<T>
	{
		readonly IDisplayNameClaim                  _display;
		readonly ISelect<ExternalLoginInfo, string> _formatter;

		public Claims(IDisplayNameClaim display) : this(display, ExternalIdentityFormatter.Default) {}

		public Claims(IDisplayNameClaim display, ISelect<ExternalLoginInfo, string> formatter)
		{
			_display   = display;
			_formatter = formatter;
		}

		public IEnumerable<Claim> Get(Login<T> parameter)
		{
			var (information, _) = parameter;

			yield return new Claim(ExternalIdentity.Default, _formatter.Get(information));
			yield return new Claim(ClaimTypes.AuthenticationMethod, information.LoginProvider);
			yield return new Claim(DisplayName.Default,
			                       information.Principal.FindFirstValue(_display.Get(information)));
		}
	}

	public sealed class ExternalLoginIdentity : Select<ExternalLoginInfo, ProviderIdentity>
	{
		public static ExternalLoginIdentity Default { get; } = new ExternalLoginIdentity();

		ExternalLoginIdentity() : base(x => new ProviderIdentity(x.LoginProvider, x.ProviderKey)) {}
	}
}