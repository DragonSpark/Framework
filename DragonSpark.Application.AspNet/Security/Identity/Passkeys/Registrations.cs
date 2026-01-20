using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class Registrations<T> : ICommand<IServiceCollection> where T : class
{
    public static Registrations<T> Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<PasskeySettings>()
                 //
                 .Start<IComposePasskeyCreationOptions<T>>()
                 .Forward<ComposePasskeyCreationOptions<T>>()
                 .Singleton()
                 //
                 .Then.Start<LoginWithExchangeCode>()
                 .Singleton()
                 //
                 .Then.Start<PasskeyResponseInterceptionMiddleware>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
            ;
    }
}