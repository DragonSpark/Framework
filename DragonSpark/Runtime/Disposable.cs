using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;
using System;

namespace DragonSpark.Runtime;

[MustDisposeResource]
public class Disposable : IDisposable, IActivateUsing<Action>
{
    readonly Action _callback;

    public Disposable(Action callback) => _callback = callback;

    public void Dispose()
    {
        _callback();
    }
}

[MustDisposeResource]
public class Disposable<T>(T disposable) : IDisposable where T : IDisposable
{
    readonly T _disposable = disposable;

    public void Dispose()
    {
        _disposable.Dispose();
    }
}
