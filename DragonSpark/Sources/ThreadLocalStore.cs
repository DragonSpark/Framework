using System;
using System.Threading;

namespace DragonSpark.Sources
{
	public class ThreadLocalStore<T> : AssignableSourceBase<T>
	{
		readonly ThreadLocal<T> local;

		public ThreadLocalStore( Func<T> create ) : this( new ThreadLocal<T>( create ) ) {}

		public ThreadLocalStore( ThreadLocal<T> local )
		{
			this.local = local;
		}

		public override void Assign( T item ) => local.Value = item;

		public override T Get() => local.Value;

		protected override void OnDispose()
		{
			local.Dispose();
			
			base.OnDispose();
		}
	}
}