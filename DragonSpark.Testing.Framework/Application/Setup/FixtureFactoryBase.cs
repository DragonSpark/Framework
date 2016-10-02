using DragonSpark.Sources;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public abstract class FixtureFactoryBase : SourceBase<IFixture>
	{
		// public static FixtureFactory Default { get; } = new FixtureFactory();

		public override IFixture Get() => new Fixture( DefaultEngineParts.Default );
	}
}