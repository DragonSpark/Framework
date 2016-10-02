namespace DragonSpark.Aspects.Coercion
{
	public sealed class CoercerSource : ConstructedSourceBase<ICoercer>
	{
		public static CoercerSource Default { get; } = new CoercerSource();
		CoercerSource() : base( Constructor.Default.Get ) {}
	}
}