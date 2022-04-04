using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<BearerSettings>()
		         //
		         .Start<ISign>()
		         .Forward<Sign>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<ICurrentBearer>()
		         .Forward<CurrentBearer>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped();
	}
}