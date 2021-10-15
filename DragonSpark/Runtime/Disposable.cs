using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime;

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

public class Disposable<T> : IDisposable where T : IDisposable
{
	readonly T _disposable;

	public Disposable(T disposable) => _disposable = disposable;

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_disposable.Dispose();
	}
}