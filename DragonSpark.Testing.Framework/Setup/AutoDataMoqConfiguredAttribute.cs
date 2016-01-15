using DragonSpark.Activation.FactoryModel;
using DragonSpark.Setup;
using DragonSpark.TypeSystem;
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

	public class SetupFixtureFactory<TAssemblyProvider, TSetup> : FixtureFactory<SetupCustomization<TAssemblyProvider, TSetup>> 
		where TAssemblyProvider : IAssemblyProvider 
		where TSetup : class, ISetup
	{}

	public class FixtureFactory<TWith> : FactoryBase<IFixture> where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Instance { get; } = new FixtureFactory<TWith>();

		protected override IFixture CreateItem() => new Fixture( DefaultEngineParts.Instance ).Customize( new TWith() );
	}
}