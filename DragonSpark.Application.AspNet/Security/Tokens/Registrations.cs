using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Tokens;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<NonceCleanupService>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<IMarkUsed>()
                 .Forward<MarkUsed>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<CreateNonce<GeneralNonce>>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.AddHostedService<NonceCleanupService>();
    }
}