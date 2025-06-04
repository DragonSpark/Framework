using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Communication.Http;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IAccessTokenProvider>()
                 .Forward<DefaultAccessTokenProvider>()
                 .Scoped()
                 //
                 .Then.Start<IComposeTokenView>()
                 .Forward<ComposeTokenView>()
                 .Singleton();
    }
}