﻿using DragonSpark.Composition.Scopes;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IScopes>()
			         .Forward<Scopes.Scopes>()
			         .Singleton()
			         //
			         .Then.Start<IScoping>()
			         .Forward<Scoping>()
			         .Singleton()
				//
				;
		}
	}
}