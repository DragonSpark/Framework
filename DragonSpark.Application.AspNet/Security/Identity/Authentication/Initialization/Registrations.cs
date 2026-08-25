using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Initialization;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() : this(x => x.GetRequiredService<AuthenticationStateSource>()) {}

	readonly Func<IServiceProvider, CascadingValueSource<Task<AuthenticationState>>> _state;

	public Registrations(Func<IServiceProvider, CascadingValueSource<Task<AuthenticationState>>> state)
		=> _state = state;

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<AuthenticationStateSource>()
		         .Scoped()
		         .Then.Start<IAuthenticationStateMonitor>()
		         .Forward<AuthenticationStateMonitor>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IProcessAuthentication<T>>()
		         .Forward<ProcessAuthentication<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IProcessAuthenticationUpdate>()
		         .Forward<ProcessAuthenticationUpdate>()
		         .Decorate<ProcessAuthenticationUpdate<T>>()
		         .Scoped()
		         //
		         .Then.Start<IInitializeAuthentication>()
		         .Forward<InitializeAuthentication>()
		         .Decorate<ValidationAwareInitializeAuthentication>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.AddCascadingValue(_state)
		         .AddCascadingValue<AuthenticationState<T>>(x => x.GetRequiredService<AuthenticationStateSource<T>>())
		         .AddCascadingValue<ProfileStatus>(x => x.GetRequiredService<ProfileStatusSource>());
	}
}