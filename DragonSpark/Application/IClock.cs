using DragonSpark.Sources;
using System;

namespace DragonSpark.Application
{
	public interface IClock : ISource<DateTimeOffset> {}
}
