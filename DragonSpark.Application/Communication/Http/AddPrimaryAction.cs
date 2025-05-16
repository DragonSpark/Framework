using System.Net.Http;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

namespace DragonSpark.Application.Communication.Http;

sealed class AddPrimaryAction : ICommand<HttpMessageHandlerBuilder>
{
    public static AddPrimaryAction Default { get; } = new();

    AddPrimaryAction() {}

    public void Execute(HttpMessageHandlerBuilder parameter)
    {
        var handler = parameter.Services.GetRequiredService<HttpMessageHandler>();
        if (parameter.PrimaryHandler is DelegatingHandler primary)
        {
            if (primary.InnerHandler is not null && handler is DelegatingHandler innerDelegating)
            {
                innerDelegating.InnerHandler = primary.InnerHandler;
            }

            primary.InnerHandler = handler;
            handler              = primary;
        }

        parameter.PrimaryHandler = handler;
    }
}