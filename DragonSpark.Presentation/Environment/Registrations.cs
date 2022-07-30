using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Presentation.Environment;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<ContextCache>()
		         .Singleton()
		         //
		         .Then.Start<ContextAwareInitializeConnection>()
		         .Include(x => x.Dependencies)
		         .Scoped()
			;
	}
}