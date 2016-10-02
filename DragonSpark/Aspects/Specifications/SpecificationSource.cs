namespace DragonSpark.Aspects.Specifications
{
	public sealed class SpecificationSource : ConstructedSourceBase<ISpecification>
	{
		public static SpecificationSource Default { get; } = new SpecificationSource();
		SpecificationSource() : base( SpecificationConstructor.Default.Get ) {}
	}
}