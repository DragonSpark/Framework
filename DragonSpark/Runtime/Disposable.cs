using System;
using DragonSpark.Runtime.Activation;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

[MustDisposeResource]
public class Disposable : IDisposable, IActivateUsing<Action>
{
    readonly Action _callback;

    public Disposable(Action callback) => _callback = callback;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _callback();
    }
}

[MustDisposeResource]
public class Disposable<T>(T disposable) : IDisposable where T : IDisposable
{
    readonly T _disposable = disposable;

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _disposable.Dispose();
    }
}
