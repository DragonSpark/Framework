namespace DragonSpark.Windows.Runtime
{
	public sealed class PublicParts : PartTypesBase
	{
		public static PublicParts Default { get; } = new PublicParts();
		PublicParts() : base( TypeSystem.PublicParts.Default.Get ) {}
	}
}