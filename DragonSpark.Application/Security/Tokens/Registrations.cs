using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Tokens;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<ApplyProof>()
                 .And<ProcessResponse>()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.Start<ITokens>()
                 .Forward<InMemoryTokens>()
                 .Singleton()
                 //
                 .Then.Start<DevicePoPHandler>()
                 .Scoped();
    }
}