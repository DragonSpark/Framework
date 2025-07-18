using DragonSpark.Model.Commands;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition.Compose;

sealed class AssignHostPlatform : ICommand<IHostBuilder>
{
    readonly string _platform;

    public AssignHostPlatform(string platform) => _platform = platform;

    public void Execute(IHostBuilder parameter)
    {
        parameter.ConfigureAppConfiguration((context, _) => new AccessPlatform(context).Execute(_platform));
    }
}