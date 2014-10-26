using System;

namespace DragonSpark.Testing.TestObjects
{
	class Disposable : IDisposable
	{
		void IDisposable.Dispose()
		{
			Disposed = true;
		}

		public bool Disposed { get; private set; }
	}
}