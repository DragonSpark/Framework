using System;

namespace DragonSpark.Testing.Framework.Testing.TestObjects
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