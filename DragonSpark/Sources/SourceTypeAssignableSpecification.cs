using DragonSpark.Extensions;
using DragonSpark.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Sources
{
	public sealed class SourceTypeAssignableSpecification : SpecificationBase<SourceTypeCandidateParameter>
	{
		public static SourceTypeAssignableSpecification Default { get; } = new SourceTypeAssignableSpecification();
		SourceTypeAssignableSpecification() {}

		public override bool IsSatisfiedBy( SourceTypeCandidateParameter parameter )
		{
			foreach ( var candidate in Candidates( parameter.Candidate ) )
			{
				if ( candidate.Adapt().IsAssignableFrom( parameter.TargetType ) )
				{
					return true;
				}
			}
			return false;
		}

		static IEnumerable<Type> Candidates( Type type )
		{
			yield return type;
			var implementations = type.Adapt().GetImplementations( typeof(ISource<>) );
			if ( implementations.Any() )
			{
				yield return implementations.First().Adapt().GetInnerType();
			}
		}
	}
}