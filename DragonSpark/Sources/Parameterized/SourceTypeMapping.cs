using System;

namespace DragonSpark.Sources.Parameterized
{
	public struct SourceTypeMapping
	{
		public SourceTypeMapping( Type sourceType, Type resultType )
		{
			SourceType = sourceType;
			ResultType = resultType;
		}

		public Type SourceType { get; }
		public Type ResultType { get; }
	}
}