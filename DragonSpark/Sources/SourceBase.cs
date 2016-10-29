using System;

namespace DragonSpark.Sources
{
	public abstract class SourceBase<T> : ISource<T>
	{
		public abstract T Get();

		Type ISource.SourceType => typeof(T);

		object ISource.Get() => Get();
	}
}