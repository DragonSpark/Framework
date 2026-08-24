using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Persist;

sealed class ConfigureSecurityStamp : IConfigureOptions<SecurityStampValidatorOptions>
{
	readonly Func<SecurityStampRefreshingPrincipalContext, Task> _refresh;

	public ConfigureSecurityStamp(RefreshPrincipal refresh) : this(refresh.Get) {}

	public ConfigureSecurityStamp(Func<SecurityStampRefreshingPrincipalContext, Task> refresh) => _refresh = refresh;

	public void Configure(SecurityStampValidatorOptions options)
	{
		options.OnRefreshingPrincipal = _refresh;
	}
}