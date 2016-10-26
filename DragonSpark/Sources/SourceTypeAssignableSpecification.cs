using DragonSpark.Extensions;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public sealed class SourceTypeAssignableSpecification : SpecificationBase<SourceTypeCandidateParameter>
	{
		readonly Func<Type, ImmutableArray<Type>> typesSource;
		public static SourceTypeAssignableSpecification Default { get; } = new SourceTypeAssignableSpecification();
		SourceTypeAssignableSpecification() : this( SourceAccountedTypes.Default.Get ) {}

		[UsedImplicitly]
		public SourceTypeAssignableSpecification( Func<Type, ImmutableArray<Type>> typesSource )
		{
			this.typesSource = typesSource;
		}

		public override bool IsSatisfiedBy( SourceTypeCandidateParameter parameter )
		{
			foreach ( var candidate in typesSource( parameter.Candidate ) )
			{
				if ( candidate.Adapt().IsAssignableFrom( parameter.TargetType ) )
				{
					return true;
				}
			}
			return false;
		}
	}
}