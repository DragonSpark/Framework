using DragonSpark.Sources.Scopes;

namespace DragonSpark.Testing.Objects.Setup
{
	public class CountingTarget : ScopedSingleton<object>
	{
		public static CountingTarget Default { get; } = new CountingTarget();
		CountingTarget() : base( () => new object() ) {}
	}
}