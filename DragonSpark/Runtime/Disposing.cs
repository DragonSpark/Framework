using DragonSpark.Model.Operations;
using JetBrains.Annotations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

[MustDisposeResource]
public class Disposing : IAsyncDisposable
{
    readonly Operate _operate;

    protected Disposing(IAsyncDisposable previous) : this(previous.DisposeAsync) {}

    protected Disposing(Operate operate) => _operate = operate;

    public ValueTask DisposeAsync() => _operate();
}
