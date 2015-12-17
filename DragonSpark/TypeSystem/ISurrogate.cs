using System;

namespace DragonSpark.TypeSystem
{
	public interface ISurrogate
	{
		Type For { get; }
	}
}