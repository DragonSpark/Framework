using DragonSpark.Application.Communication.Http;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class Registrations : ICommand<IServiceCollection>
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
		         .Then.Start<IBearer>()
		         .Forward<Bearer>()
		         .Decorate<ReferenceValueAwareBearer>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<ICurrentBearer>()
		         .Forward<CurrentBearer>()
		         .Scoped()
                 //
                 .Then.TryDecorate<IAccessTokenProvider, BearerAwareAccessTokenProvider>()
                 ;
	}
}