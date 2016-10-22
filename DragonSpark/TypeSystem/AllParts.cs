using DragonSpark.Sources.Parameterized;

namespace DragonSpark.TypeSystem
{
	public sealed class AllParts : PartsBase
	{
		public static AllParts Default { get; } = new AllParts();
		AllParts() : base( AssemblyTypes.All.AsEnumerable ) {}
	}
}