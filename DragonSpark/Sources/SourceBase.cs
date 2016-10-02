using DragonSpark.Application.Setup;
using System;

namespace DragonSpark.Sources
{
	public abstract class SourceBase<T> : ISource<T>, IServiceAware
	{
		public abstract T Get();

		object ISource.Get() => Get();

		Type IServiceAware.ServiceType { get; } = typeof(T);
	}
}