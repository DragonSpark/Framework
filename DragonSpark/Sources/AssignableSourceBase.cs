using DragonSpark.Extensions;
using System;

namespace DragonSpark.Sources
{
	public abstract class AssignableSourceBase<T> : SourceBase<T>, IAssignableSource<T>, IDisposable
	{
		public abstract void Assign( T item );

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		void Dispose( bool disposing ) => disposing.IsTrue( OnDispose );

		protected virtual void OnDispose() {}
	}
}