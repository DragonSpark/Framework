namespace DragonSpark.TypeSystem
{
	public sealed class PublicParts : PartsBase
	{
		public static PublicParts Default { get; } = new PublicParts();
		PublicParts() : base( AssemblyTypes.Public.Get ) {}
	}
}