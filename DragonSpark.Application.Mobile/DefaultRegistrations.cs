using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Mobile;

public sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
    public static DefaultRegistrations Default { get; } = new();

    DefaultRegistrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Start<IApplicationErrorHandler>()
                 .Forward<ApplicationErrorHandler>()
                 .Singleton();
    }
}