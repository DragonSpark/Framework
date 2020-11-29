using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime
{
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
}