using System;

namespace DragonSpark.Sources
{
	public interface ISourceAware
	{
		Type SourceType { get; }
	}
}