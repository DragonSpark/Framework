using DragonSpark.Specifications;

namespace DragonSpark.Composition
{
	public sealed class IsValidConventionMappingSpecification : SpecificationBase<ConventionMapping>
	{
		public static IsValidConventionMappingSpecification Default { get; } = new IsValidConventionMappingSpecification();
		IsValidConventionMappingSpecification() {}

		public override bool IsSatisfiedBy( ConventionMapping parameter ) => ConventionCandidateNames.Default.Get( parameter.InterfaceType ).Equals( parameter.ImplementationType.Name );
	}
}