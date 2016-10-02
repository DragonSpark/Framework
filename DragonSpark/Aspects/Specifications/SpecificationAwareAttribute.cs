namespace DragonSpark.Aspects.Specifications
{
	public sealed class SpecificationAwareAttribute : SpecificationAttributeBase
	{
		protected override ISpecification DetermineSpecification() => SpecificationAwareConstructor.Default.Get( Instance.GetType() )( Instance ).Specification;
	}
}