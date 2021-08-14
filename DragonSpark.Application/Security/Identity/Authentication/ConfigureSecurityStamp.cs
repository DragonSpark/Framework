using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class ConfigureSecurityStamp : ICommand<SecurityStampValidatorOptions>
	{
		readonly Func<SecurityStampRefreshingPrincipalContext, Task> _refresh;

		public ConfigureSecurityStamp(Func<IKnownClaims> claims) : this(new DeferredCopyClaims(claims)) {}

		public ConfigureSecurityStamp(ICommand<(ClaimsPrincipal From, ClaimsPrincipal To)> copy)
			: this(new RefreshPrincipal(copy).Get) {}

		public ConfigureSecurityStamp(Func<SecurityStampRefreshingPrincipalContext, Task> refresh)
			=> _refresh = refresh;

		public void Execute(SecurityStampValidatorOptions parameter)
		{
			parameter.OnRefreshingPrincipal = _refresh;
		}
	}
}