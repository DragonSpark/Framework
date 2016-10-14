namespace DragonSpark.Aspects.Specifications
{
	public sealed class Source : ConstructedSourceBase<ISpecification>
	{
		public static Source Default { get; } = new Source();
		Source() : base( Constructor.Default.Get ) {}
	}
}