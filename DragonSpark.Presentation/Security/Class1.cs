using DragonSpark.Application;
using DragonSpark.Application.Navigation;
using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Security.Claims;

namespace DragonSpark.Presentation.Security
{
	// TODO: Introduce DragonSpark.Presentation.Security.Identity

	class Class1 {}

	public class ExternalLogin : IAlteration<string>
	{
		readonly IResult<string> _return;
		readonly string          _path;

		protected ExternalLogin(string @return) : this(@return.Start().Get()) {}

		protected ExternalLogin(IResult<string> @return) : this(@return, ExternalLoginPath.Default) {}

		protected ExternalLogin(IResult<string> @return, string path)
		{
			_return = @return;
			_path   = path;
		}

		public string Get(string parameter)
			=> $"{_path}?provider={parameter}&returnUrl={WebUtility.UrlEncode(_return.Get())}";
	}

	public class NavigateToExternalLogin : Navigation<string>
	{
		protected NavigateToExternalLogin(NavigationManager navigation, IAlteration<string> login)
			: base(navigation, login.Get, true) {}
	}

	public class RefreshExternalLogin : Navigation
	{
		protected RefreshExternalLogin(NavigationManager navigation, CurrentExternalLogin current)
			: base(navigation, current.Get, true) {}
	}

	public sealed class DefaultExternalLogin : ExternalLogin
	{
		public DefaultExternalLogin(CurrentPath @return) : base(@return) {}
	}

	public sealed class DefaultCurrentExternalLogin : CurrentExternalLogin
	{
		public DefaultCurrentExternalLogin(DefaultExternalLogin @select, CurrentAuthenticationMethod current)
			: base(@select, current) {}
	}

	public class CurrentExternalLogin : DelegatedSelection<string, string>
	{
		protected CurrentExternalLogin(IAlteration<string> select, CurrentAuthenticationMethod current)
			: base(select, current) {}
	}

	public sealed class CurrentAuthenticationMethod : Result<string>
	{
		public CurrentAuthenticationMethod(ICurrentPrincipal source) : this(source, AuthenticationMethod.Default) {}

		public CurrentAuthenticationMethod(ICurrentPrincipal source, IReadClaim read)
			: base(source.Then().Select(read).Select(x => x.Value())) {}
	}

	sealed class CurrentPrincipal : ICurrentPrincipal
	{
		readonly IHttpContextAccessor _accessor;

		public CurrentPrincipal(IHttpContextAccessor accessor) => _accessor = accessor;

		public ClaimsPrincipal Get() => _accessor.HttpContext?.User ?? throw new InvalidOperationException();
	}

	public sealed class ExternalLoginPath : Text.Text
	{
		public static ExternalLoginPath Default { get; } = new ExternalLoginPath();

		ExternalLoginPath() : base("/Identity/Account/ExternalLogin") {}
	}
}