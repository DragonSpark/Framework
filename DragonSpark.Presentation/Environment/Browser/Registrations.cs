using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment.Browser.Document;
using DragonSpark.Presentation.Environment.Browser.Window;
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
		         .Decorate<ConnectionAwareEvaluate>()
		         .Decorate<PolicyAwareEvaluate>()
		         .Decorate<LogAwareEvaluate>()
		         .Scoped()
		         //
		         .Then.Start<CreateDocumentElementHandle>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<CreateWindowFocusElement>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.ForDefinition<SessionClientVariables<object>>()
		         .Scoped()
			;
	}
}