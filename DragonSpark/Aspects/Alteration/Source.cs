namespace DragonSpark.Aspects.Alteration
{
	public sealed class Source : ConstructedSourceBase<IAlteration>
	{
		public static Source Default { get; } = new Source();
		Source() : base( Constructor.Default.Get ) {}
	}
}