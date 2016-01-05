using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AutoDataRegistrationAttribute : AutoDataAttribute
	{
		public AutoDataRegistrationAttribute() : base( FixtureFactory<MetadataCustomization>.Instance.Create )
		{ }
	}

	public class AutoDataMoqAttribute : AutoDataAttribute
	{
		public AutoDataMoqAttribute() : base( FixtureFactory<AutoMoqCustomization>.Instance.Create )
		{}
	}

	public class SetupFixtureFactory<T> : FixtureFactory<SetupCustomization<T>> where T : class, ISetup
	{}

	public class FixtureFactory<TWith> : FactoryBase<IFixture> where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Instance { get; } = new FixtureFactory<TWith>();

		protected override IFixture CreateItem() => new Fixture( DefaultEngineParts.Instance ).Customize( new TWith() );
	}
}