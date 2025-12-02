using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Results;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

[MustDisposeResource]
public class Disposing : IAsyncDisposable
{
    readonly Operate _operate;

    protected Disposing(IAsyncDisposable previous) : this(previous.DisposeAsync) {}

    protected Disposing(Operate operate) => _operate = operate;

    public ValueTask DisposeAsync() => _operate();
}


public class Disposing<T> : IAsyncDisposable
{
    readonly IResulting<T> _previous;

    public Disposing(IResulting<T> previous) => _previous = previous;

    public async ValueTask DisposeAsync()
    {
        await _previous.Off();
    }
}