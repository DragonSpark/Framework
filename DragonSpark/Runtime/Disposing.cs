using System;
using System.Threading.Tasks;
using DragonSpark.Model.Operations;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

[MustDisposeResource]
public class Disposing : IAsyncDisposable
{
    readonly Operate _operate;

    protected Disposing(Operate operate) => _operate = operate;

    public ValueTask DisposeAsync() => _operate();
}
