using System;
using System.Net.Http;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok.Chat;

sealed class Registrations : ICommand<IServiceCollection>
{
    public static Registrations Default { get; } = new();

    Registrations() {}

    public void Execute(IServiceCollection parameter)
    {
        parameter.Register<GrokApiSettings>()
                 //
                 .Start<ConfigureClient>()
                 .Singleton()
                 //
                 .Then.Start<IChat>()
                 .Forward<Chat>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.Start<IExecuteTools>()
                 .Forward<ExecuteTools>()
                 .Singleton()
                 //
                 .Then.Start<IChatResponse>()
                 .Forward<ChatResponse>()
                 .Decorate<ExceptionAwareChatResponse>()
                 .Include(x => x.Dependencies)
                 .Singleton()
                 //
                 .Then.AddHttpClient(RegistrationName.Default, ClientConfiguration.Default.Execute)
                 .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                 {
                     PooledConnectionLifetime = TimeSpan.FromMinutes(10)
                 });
    }
}

/*public readonly record struct ChatInput(List<ChatMessage> Messages, string UserContext);*/