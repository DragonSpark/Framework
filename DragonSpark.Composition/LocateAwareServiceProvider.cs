using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Composition;

[MustDisposeResource]
sealed class LocateAwareServiceProvider : IServiceProvider, IAsyncDisposable, IDisposable
{
    readonly IServiceProvider _previous;
    readonly ISelect<Type, Type?> _locate;

    [MustDisposeResource(false)]
    public LocateAwareServiceProvider(IServiceProvider previous, IServiceCollection collection)
        : this(previous, new LocateImplementation(collection)) { }

    public LocateAwareServiceProvider(IServiceProvider previous, ISelect<Type, Type?> locate)
    {
        _previous = previous;
        _locate = locate;
    }

    public object? GetService(Type serviceType)
    {
        try
        {
            return _previous.GetService(serviceType);
        }
        catch (InvalidOperationException)
        {
            var located = _locate.Get(serviceType);
            if (located != null)
            {
                return _previous.GetService(located);
            }

            throw;
        }
    }

    public void Dispose()
    {
        DisposeAny.Default.Execute(_previous);
    }

    public ValueTask DisposeAsync() => DisposingAny.Default.Get(_previous);
}
