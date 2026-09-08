using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Entities;

/*
sealed class Registrations<T> : ICommand<IServiceCollection> where T : DbContext
{
    public static Registrations<T> Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<INewContext<T>>()
                 .Forward<NewContext<T>>()
                 .Singleton()
                 //
                 .Then.Start<INewContext>()
                 .Forward<NewStandardContext<T>>()
                 .Singleton()
                 //
                 .Then.Start<IScopes>()
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
*/

sealed class GeneralConfiguration<T> : ICommand<IServiceCollection> where T : DbContext
{
	public static GeneralConfiguration<T> Default { get; } = new();

	GeneralConfiguration() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<INewContext<T>>()
		         .Forward<NewContext<T>>()
		         .Singleton()
		         //
		         .Then.Start<INewContext>()
		         .Forward<NewStandardContext<T>>()
		         .Singleton();
	}
}

sealed class Registrations<T> : Commands<IServiceCollection> where T : DbContext
{
	public static Registrations<T> Default { get; } = new();

	Registrations() : base(PrimaryConfiguration<T>.Default, GeneralConfiguration<T>.Default) {}
}

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