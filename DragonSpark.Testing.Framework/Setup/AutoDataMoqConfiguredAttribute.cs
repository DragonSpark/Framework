using DragonSpark.Activation.FactoryModel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class DefaultAutoDataCustomization : CompositeCustomization
	{
		public DefaultAutoDataCustomization() : base( MetadataCustomization.Instance, new AutoConfiguredMoqCustomization() ) { }
	}

	public class AutoDataMoqAttribute : AutoDataAttribute
	{
		public AutoDataMoqAttribute() : base( FixtureFactory<AutoMoqCustomization>.Instance.Create ) {}
	}

	/*public class SetupFixtureFactory<T> : FixtureFactory<T> where T : SetupCustomization, new() {}*/

	public class FixtureFactory<TWith> : FactoryBase<IFixture> where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Instance { get; } = new FixtureFactory<TWith>();

		protected override IFixture CreateItem() => new Fixture( DefaultEngineParts.Instance ).Customize( new TWith() );
	}
}