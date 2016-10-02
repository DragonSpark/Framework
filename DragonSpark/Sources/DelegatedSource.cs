using System;

namespace DragonSpark.Sources
{
	public class DelegatedSource<T> : SourceBase<T>
	{
		readonly Func<T> get;

		public DelegatedSource( Func<T> get )
		{
			this.get = get;
		}

		public override T Get() => get();
	}
}