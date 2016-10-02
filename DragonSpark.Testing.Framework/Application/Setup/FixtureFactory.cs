using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public class FixtureFactory<TWith> : FixtureFactoryBase where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Default { get; } = new FixtureFactory<TWith>();
		FixtureFactory() {}

		public override IFixture Get() => base.Get().Customize( new TWith() );
	}
}