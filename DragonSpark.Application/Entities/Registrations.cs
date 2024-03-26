﻿using DragonSpark.Application.Entities.Editing;
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
		parameter.Start<INewContext<T>>()
		         .Forward<NewContext<T>>()
		         .Singleton()
		         //
		         .Then.Start<IContexts>()
		         .Forward<Contexts<T>>()
		         .Singleton()
		         //
		         .Then.Start<IEnlistedContexts>()
		         .Forward<EnlistedContexts>()
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