using System;

namespace DragonSpark.Sources
{
	public class DelegatedSource<T> : SourceBase<T>
	{
		readonly Func<T> factory;

		public DelegatedSource( Func<T> factory )
		{
			this.factory = factory;
		}

		public override T Get() => factory();
	}
}