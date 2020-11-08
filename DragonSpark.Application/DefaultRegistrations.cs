using AsyncUtilities;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Security;
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
			parameter.Start<IStorageState>()
			         .Forward<StorageState>()
			         .Scoped()
			         //
			         .Then.AddScoped(typeof(IUpdate<>), typeof(Update<>))
			         //
			         .Start<IToken>()
			         .Forward<Token>()
			         .Scoped()
			         //
			         .Then.Start<INavigateToSignOut>()
			         .Forward<NavigateToSignOut>()
			         .Scoped()
					 //
			         .Then.Start<AsyncLock>()
			         .Use<StateConnectionLock>()
			         .Scoped();
		}
	}
}