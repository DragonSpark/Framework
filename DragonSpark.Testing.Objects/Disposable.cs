using System;

namespace DragonSpark.Testing.Objects
{
	public class Disposable : IDisposable
	{
		void IDisposable.Dispose()
		{
			Disposed = true;
		}

		public bool Disposed { get; private set; }
	}
}