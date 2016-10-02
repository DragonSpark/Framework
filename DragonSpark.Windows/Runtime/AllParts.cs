namespace DragonSpark.Windows.Runtime
{
	public sealed class AllParts : PartTypesBase
	{
		public static AllParts Default { get; } = new AllParts();
		AllParts() : base( TypeSystem.AllParts.Default.Get ) {}
	}
}