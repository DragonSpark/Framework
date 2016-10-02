using DragonSpark.Sources;

namespace DragonSpark.Configuration
{
	public sealed class EnableMethodCaching : Scope<bool>
	{
		public static EnableMethodCaching Default { get; } = new EnableMethodCaching();
		EnableMethodCaching() : base( () => true ) {}
	}
}
