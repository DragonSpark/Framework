using DragonSpark.Specifications;
using DragonSpark.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Immutable;

namespace DragonSpark.Sources
{
	public sealed class SourceTypeAssignableSpecification : SpecificationBase<SourceTypeCandidateParameter>
	{
		public static SourceTypeAssignableSpecification Default { get; } = new SourceTypeAssignableSpecification();
		SourceTypeAssignableSpecification() : this( SourceAccountedTypes.Default.Get ) {}

		readonly Func<Type, ImmutableArray<TypeAdapter>> typesSource;

		[UsedImplicitly]
		public SourceTypeAssignableSpecification( Func<Type, ImmutableArray<TypeAdapter>> typesSource )
		{
			this.typesSource = typesSource;
		}

		public override bool IsSatisfiedBy( SourceTypeCandidateParameter parameter ) => 
			typesSource( parameter.Candidate ).IsAssignableFrom( parameter.TargetType );
	}
}