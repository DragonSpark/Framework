namespace DragonSpark.TypeSystem
{
	public sealed class PublicParts : PartTypesBase
	{
		public static PublicParts Default { get; } = new PublicParts();
		PublicParts() : base( PublicPartsLocator.Default.Get ) {}
	}
}