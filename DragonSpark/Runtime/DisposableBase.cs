using System;

namespace DragonSpark.Runtime
{
	public abstract class DisposableBase : IDisposable
	{
		readonly ConditionMonitor monitor = new ConditionMonitor();

		~DisposableBase()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing )
		{
			if ( monitor.Apply() )
			{
				OnDispose( disposing );
			}
		}

		protected virtual void OnDispose( bool disposing ) {}
	}
}