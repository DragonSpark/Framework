namespace DragonSpark.Aspects.Coercion
{
	public sealed class Source : ConstructedSourceBase<ICoercer>
	{
		public static Source Default { get; } = new Source();
		Source() : base( Constructor.Default.Get ) {}
	}
}