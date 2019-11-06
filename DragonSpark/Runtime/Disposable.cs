using System;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime
{
	public class Disposable : IDisposable, IActivateUsing<Action>
	{
		readonly Action _callback;

		public Disposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}