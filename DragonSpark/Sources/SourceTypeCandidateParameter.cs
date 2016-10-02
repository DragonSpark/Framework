using System;

namespace DragonSpark.Sources
{
	public struct SourceTypeCandidateParameter
	{
		public SourceTypeCandidateParameter( Type targetType, Type candidate )
		{
			TargetType = targetType;
			Candidate = candidate;
		}

		public Type TargetType { get; }
		public Type Candidate { get; }
	}
}