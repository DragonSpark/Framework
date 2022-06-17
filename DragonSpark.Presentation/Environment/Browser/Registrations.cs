using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Presentation.Environment.Browser.Document;
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
		         .Scoped()
		         //
		         .Then.Start<CreateDocumentElementHandle>()
		         .Include(x => x.Dependencies)
		         .Scoped();
	}
}