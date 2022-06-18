using DragonSpark.Application.Entities.Editing;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Entities;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : DbContext
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<AttachMany>()
		         .Scoped()
		         //
		         .Then.Start<IContexts<T>>()
		         .Forward<Contexts<T>>()
		         .Singleton()
		         //
		         .Then.Start<IStandardScopes>()
		         .Forward<StandardScopes<T>>()
		         .Singleton()
		         //
		         .Then.Start<IEnlistedScopes>()
		         .Forward<EnlistedScopes>()
		         .Singleton()
		         //
		         .Then.Start<ISessionScopes>()
		         .Forward<SessionScopes>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<RemoveForSession<object>>()
		         .Generic()
		         .Scoped()
		         //
		         .Then.Start<Remove<object>>()
		         .Generic()
		         .Singleton()
		         //
		         .Then.Start<Save>()
		         .Singleton()
		         //
		         .Then.Start<Save<object>>()
		         .Generic()
		         .Singleton()
		         //
		         .Then.Start<SaveMany<object>>()
		         .Generic()
		         .Singleton()
		         //
		         .Then.Start<IAmbientContext>()
		         .Forward<AmbientContext>()
		         .Decorate<ProviderAwareAmbientContext>()
		         .Singleton();
	}
}