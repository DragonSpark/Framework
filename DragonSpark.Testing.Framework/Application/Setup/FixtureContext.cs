using DragonSpark.Sources;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class FixtureContext : Scope<IFixture>
	{
		public static FixtureContext Default { get; } = new FixtureContext();
		FixtureContext() {}
	}
}