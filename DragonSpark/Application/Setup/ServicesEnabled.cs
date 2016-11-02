using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application.Setup
{
	public sealed class ServicesEnabled : Scope<bool>
	{
		public static ServicesEnabled Default { get; } = new ServicesEnabled();
		ServicesEnabled() {}
	}
}