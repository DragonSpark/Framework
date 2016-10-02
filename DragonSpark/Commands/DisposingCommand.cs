using DragonSpark.Extensions;
using System;

namespace DragonSpark.Commands
{
	public abstract class DisposingCommand<T> : CommandBase<T>, IDisposable
	{
		readonly Action onDispose;

		protected DisposingCommand()
		{
			onDispose = OnDispose;
		}

		~DisposingCommand()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing ) => disposing.IsTrue( onDispose );

		protected virtual void OnDispose() {}
	}
}