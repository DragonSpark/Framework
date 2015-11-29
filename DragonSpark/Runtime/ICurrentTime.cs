using System;

namespace DragonSpark.Runtime
{
	public interface ICurrentTime
	{
		DateTimeOffset Now { get; }
	}
}
