using System;

namespace DragonSpark.Sources
{
	public class SuppliedDeferredSource<T> : SourceBase<T>
	{
		readonly Lazy<T> lazy;

		public SuppliedDeferredSource( Func<T> factory ) : this( new Lazy<T>( factory ) ) {}

		public SuppliedDeferredSource( Lazy<T> lazy )
		{
			this.lazy = lazy;
		}

		public override T Get() => lazy.Value;
	}
}