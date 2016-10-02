using DragonSpark.Sources;

namespace DragonSpark.Testing.Objects.Setup
{
	public class CountingTarget : Scope<object>
	{
		public static CountingTarget Default { get; } = new CountingTarget();
		CountingTarget() : base( Factory.GlobalCache( () => new object() ) ) {}
	}
}