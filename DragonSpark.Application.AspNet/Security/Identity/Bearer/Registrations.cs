﻿using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<BearerSettings>()
		         //
		         .Start<BearerConfiguration>()
		         .And<TokenValidation>()
		         .Singleton()
		         //
		         .Then.Start<ISign>()
		         .Forward<Sign>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<IBearer>()
		         .Forward<Bearer>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<ICurrentBearer>()
		         .Forward<CurrentBearer>()
		         .Scoped();
	}
}