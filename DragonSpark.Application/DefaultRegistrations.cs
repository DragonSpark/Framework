using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IScopedToken>()
		         .Forward<ScopedToken>()
		         .Decorate<AmbientAwareToken>()
		         .Scoped()
		         //
		         .Then.Start<ILogException>()
		         .Forward<LogException>()
		         .Decorate<TemplateAwareLogException>()
		         .Singleton()
		         //
		         .Then.Start<IExceptionLogger>()
		         .Forward<ExceptionLogger>()
		         .Singleton()
		         //
		         .Then.Start<IExceptions>()
		         .Forward<Exceptions>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IExecuteOperation>()
		         .Forward<ExecuteOperation>()
		         .Scoped();
	}
}