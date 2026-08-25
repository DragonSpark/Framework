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
        _callback();
    }
}