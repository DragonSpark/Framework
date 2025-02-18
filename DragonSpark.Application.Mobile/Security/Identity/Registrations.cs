using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Uno.Extensions.Authentication;

namespace DragonSpark.Application.Mobile.Security.Identity;

public sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<UpdateCurrentPrincipal>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         .Then.AddSingleton<IPrincipalAccess, PrincipalAccess>()
		         .AddSingleton<ICurrentPrincipal, CurrentPrincipal>()
		         //
		         .Decorate<ITokenCache, CompensatingTokenCache>()
		         .Decorate<IAuthenticationService, CurrentPrincipalAwareAuthenticationService>()
			;
	}
}