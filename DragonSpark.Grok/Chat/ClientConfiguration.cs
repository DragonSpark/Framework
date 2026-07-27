using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok.Chat;

sealed class ClientConfiguration : ICommand<ConfigureClientInput>
{
    public static ClientConfiguration Default { get; } = new();

    ClientConfiguration() {}

    public void Execute(ConfigureClientInput parameter)
    {
        var (services, subject) = parameter;
        services.GetRequiredService<ConfigureClient>().Execute(subject);
    }

    public void Execute(IServiceProvider services, HttpClient subject)
    {
        Execute(new(services, subject));
    }
}