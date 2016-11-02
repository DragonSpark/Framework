namespace DragonSpark.TypeSystem
{
	public sealed class AllParts : PartTypesBase
	{
		public static AllParts Default { get; } = new AllParts();
		AllParts() : base( AllPartsLocator.Default.Get ) {}
	}
}