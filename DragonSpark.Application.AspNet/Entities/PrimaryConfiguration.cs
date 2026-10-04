using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

sealed class PrimaryConfiguration<T> : ICommand<IServiceCollection> where T : DbContext
{
	public static PrimaryConfiguration<T> Default { get; } = new();

	PrimaryConfiguration() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IScopes>()
		         .Forward<Scopes<T>>()
		         .Singleton()
		         //
		         .Then.Start<IEnlistedScopes>()
		         .Forward<EnlistedScopes>()
		         .Singleton()
		         //
		         .Then.Start<Remove<object>>()
		         .Generic()
		         .Singleton()
		         //
		         .Then.Start<SaveAndCommit<object>>()
		         .Generic()
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