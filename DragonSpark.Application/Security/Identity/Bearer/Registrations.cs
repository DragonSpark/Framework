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
		parameter.Register<BearerSettings>().Register<MessageBearerSettings>()
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
				 .Then.Start<IMessageBearer>()
				 .Forward<MessageBearer>()
				 .Include(x => x.Dependencies.Recursive())
				 .Scoped()
				 //
				 .Then.Start<IToken>()
				 .Forward<Token>()
				 .Include(x => x.Dependencies.Recursive())
				 .Singleton()
				 //
				 .Then.Start<ICurrentBearer>()
				 .Forward<CurrentBearer>()
				 .Scoped()
				 //
				 .Then.Start<CurrentMessageBearer>().Scoped()
				 //
				 .Then.TryDecorate<IAccessTokenProvider, BearerAwareAccessTokenProvider>()
				 ;
	}
}