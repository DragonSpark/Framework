using AsyncUtilities;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Navigation;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application
{
	sealed class DefaultRegistrations : ICommand<IServiceCollection>
	{
		public static DefaultRegistrations Default { get; } = new DefaultRegistrations();

		DefaultRegistrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IClear>()
			         .Forward<Clear>()
			         .Scoped()
			         //
			         .Then.Start<ISave>()
			         .Forward<Save>()
			         .Scoped()
			         //
			         .Then.Start<IUndo>()
			         .Forward<Undo>()
			         .Scoped()
			         //
			         .Then.Start<IStorageState>()
			         .Forward<StorageState>()
			         .Scoped()
			         //
			         .Then.AddScoped(typeof(ISaveChanges<>), typeof(SaveChanges<>))
			         .AddScoped(typeof(ISave<>), typeof(Save<>))
			         .AddScoped(typeof(IRemove<>), typeof(Remove<>))
			         .AddScoped(typeof(IUpdate<>), typeof(Update<>))
			         //
			         .Start<IToken>()
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
			         .Then.Start<AsyncLock>()
			         .Use<StateConnectionLock>()
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
				;
		}
	}
}