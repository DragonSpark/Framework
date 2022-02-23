using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Connections.Initialization;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IEvaluate>()
		         .Forward<Evaluate>()
		         .Decorate<PolicyAwareEvaluate>()
		         .Scoped()
		         //
		         .Then.Decorate<IIsInitialized, BotAwareIsInitialized>();
	}
}