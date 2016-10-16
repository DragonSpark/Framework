using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Commands
{
	public abstract class DisposingCommandBase<T> : CommandBase<T>, IDisposable
	{
		readonly IDisposable disposable;
		readonly Action apply;

		protected DisposingCommandBase() : this( new DisposableAction( () => {} ) ) {}

		protected DisposingCommandBase( IDisposable disposable )
		{
			this.disposable = disposable;
			apply = ApplyDispose;
		}

		~DisposingCommandBase()
		{
			Dispose( false );
		}

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing ) => disposing.IsTrue( apply );

		void ApplyDispose()
		{
			disposable.Dispose();
			OnDispose();
		}

		protected virtual void OnDispose() {}
	}
}