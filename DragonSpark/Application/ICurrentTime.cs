using System;

namespace DragonSpark.Application
{
	public interface ICurrentTime
	{
		DateTimeOffset Now { get; }
	}
}
