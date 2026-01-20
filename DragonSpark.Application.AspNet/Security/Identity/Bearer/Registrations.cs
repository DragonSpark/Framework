using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<MessageBearerSettings>()
                 //
                 .Start<IMessageBearer>()
                 .Forward<MessageBearer>()
                 .Include(x => x.Dependencies.Recursive())
                 .Scoped()
                 //
                 .Then.Start<IToken>()
                 .Forward<Token>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<ISecureToken>()
                 .Forward<SecureToken>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IDecryptToken>()
                 .Forward<DecryptToken>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<ApplicationTokenValidation>()
                 .And<BearerConfiguration>()
                 .Singleton();
    }
}