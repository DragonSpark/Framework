using DragonSpark.Activation.FactoryModel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class ConfiguredMoqAutoDataAttribute : AutoDataAttribute
	{
		public ConfiguredMoqAutoDataAttribute() : base( FixtureFactory<AutoConfiguredMoqCustomization>.Instance.Create )
		{}
	}

	public class MoqAutoDataAttribute : AutoDataAttribute
	{
		public MoqAutoDataAttribute() : base( FixtureFactory<AutoMoqCustomization>.Instance.Create )
		{}
	}

	public class FixtureFactory<TWith> : FactoryBase<IFixture> where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Instance { get; } = new FixtureFactory<TWith>();

		protected override IFixture CreateItem() => new Fixture( DefaultEngineParts.Instance ).Customize( new TWith() );
	}
}