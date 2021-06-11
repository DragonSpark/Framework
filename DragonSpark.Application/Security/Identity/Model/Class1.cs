using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Identity;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model
{
	class Class1 {}

	public interface IAuthenticate<T> : IOperation<Login<T>> {}

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

	sealed class ConfigureSecurityStamp : ICommand<SecurityStampValidatorOptions>
	{
		readonly Func<SecurityStampRefreshingPrincipalContext, Task> _refresh;

		public ConfigureSecurityStamp(Func<IKnownClaims> claims) : this(new RefreshPrincipal(claims).Get) {}

		public ConfigureSecurityStamp(Func<SecurityStampRefreshingPrincipalContext, Task> refresh)
			=> _refresh = refresh;

		public void Execute(SecurityStampValidatorOptions parameter)
		{
			parameter.OnRefreshingPrincipal = _refresh;
		}
	}

	public interface IKnownClaims : IResult<IEnumerable<string>> {}

	sealed class KnownClaims : IKnownClaims
	{
		public static KnownClaims Default { get; } = new KnownClaims();

		KnownClaims() {}

		public IEnumerable<string> Get()
		{
			yield return ClaimTypes.AuthenticationMethod;
			yield return ExternalIdentity.Default;
			yield return DisplayName.Default;
		}
	}

	sealed class RefreshPrincipal : IAllocated<SecurityStampRefreshingPrincipalContext>
	{
		readonly Func<IKnownClaims> _claims;

		public RefreshPrincipal(Func<IKnownClaims> claims)
		{
			_claims = claims;
			_claims = claims;
		}

		public Task Get(SecurityStampRefreshingPrincipalContext parameter)
		{
			var identity = new ClaimsIdentity();
			foreach (var claim in _claims().Get().AsValueEnumerable())
			{
				if (parameter.CurrentPrincipal.HasClaim(claim!))
				{
					identity.AddClaim(parameter.CurrentPrincipal.FindFirst(claim!)!);
				}
			}

			if (identity.Claims.Any())
			{
				parameter.NewPrincipal.AddIdentity(identity);
			}

			return Task.CompletedTask;
		}
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