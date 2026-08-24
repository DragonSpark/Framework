using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Persist;

sealed class WithIdentityClaimsRelay : ICommand<IServiceCollection>
{
	public static WithIdentityClaimsRelay Default { get; } = new();

	WithIdentityClaimsRelay() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<RefreshPrincipal>()
		         .Include(x => x.Dependencies)
		         .Singleton()
		         //
				 .Then.ConfigureOptions<ConfigureSecurityStamp>();
	}
}