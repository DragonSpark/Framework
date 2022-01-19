using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class ConfigureSecurityStamp : ICommand<SecurityStampValidatorOptions>
{
	readonly Func<SecurityStampRefreshingPrincipalContext, Task> _refresh;

	public ConfigureSecurityStamp(IServiceCollection services) : this(services.Deferred<RefreshPrincipal>()
	                                                                          .Start()
	                                                                          .Select(y => y.ToDelegate())
	                                                                          .Then()
	                                                                          .Assume()) {}

	public ConfigureSecurityStamp(Func<SecurityStampRefreshingPrincipalContext, Task> refresh) => _refresh = refresh;

	public void Execute(SecurityStampValidatorOptions parameter)
	{
		parameter.OnRefreshingPrincipal = _refresh;
	}
}