using System.Buffers;
using System.Net.Http;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using NetFabric.Hyperlinq;

namespace DragonSpark.Application.Communication.Http;

sealed class AddAction : ICommand<HttpMessageHandlerBuilder>
{
    public static AddAction Default { get; } = new();

    AddAction() : this(ArrayPool<DelegatingHandler>.Shared) {}

    readonly ArrayPool<DelegatingHandler> _pool;

    public AddAction(ArrayPool<DelegatingHandler> pool) => _pool = pool;

    public void Execute(HttpMessageHandlerBuilder parameter)
    {
        using var handlers = parameter.Services.GetServices<DelegatingHandler>().AsValueEnumerable().ToArray(_pool);
        var       first    = handlers.AsValueEnumerable().First().OrDefault();
        if (first is not null)
        {
            var current = first;
            var span    = handlers.Memory.Span;
            for (var i = 1; i < handlers.Length; i++)
            {
                current.InnerHandler = span[i];
                current              = span[i];
            }

            first.InnerHandler       = parameter.PrimaryHandler;
            parameter.PrimaryHandler = first;
        }
    }
}