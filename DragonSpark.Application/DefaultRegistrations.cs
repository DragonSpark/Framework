﻿using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Navigation;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new DefaultRegistrations();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IToken>()
		         .Forward<Token>()
		         .Scoped()
		         //
		         .Then.Start<INavigateToSignOut>()
		         .Forward<NavigateToSignOut>()
		         .Scoped()
		         //
		         .Then.Start<NavigateToSignIn>()
		         .And<CurrentRootPath>()
		         .And<RedirectLoginPath>()
		         .Scoped()
		         //
		         .Then.Start<RefreshCurrentPath>()
		         .And<CurrentPath>()
		         .Scoped()
		         //
		         .Then.Start<IScopedTable>()
		         .Forward<ScopedTable>()
		         .Scoped()
		         //
		         .Then.Start<IExceptionLogger>()
		         .Forward<ExceptionLogger>()
		         .Scoped()
		         //
		         .Then.Start<IExceptions>()
		         .Forward<Exceptions>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IExecuteOperation>()
		         .Forward<ExecuteOperation>()
		         .Scoped()
		         //
		         .Then.Start<ICurrentUserName>()
		         .Forward<CurrentUserName>()
		         .Singleton()
			;

	}
}