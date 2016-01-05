using DragonSpark.Activation.FactoryModel;
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

	public class FixtureFactory<TWith> : FactoryBase<IFixture> where TWith : ICustomization, new()
	{
		public static FixtureFactory<TWith> Instance { get; } = new FixtureFactory<TWith>();

		protected override IFixture CreateItem() => new Fixture( DefaultEngineParts.Instance ).Customize( new TWith() );
	}

	/*public class SetupFixtureFactory : FactoryBase<IFixture>
	{
		public static SetupFixtureFactory Instance { get; } = new SetupFixtureFactory();

		protected override IFixture CreateItem() => new Fixture( SetupEngineParts.Instance );
	}*/

	/*public class SetupAutoDataAttribute : AutoDataAttribute
	{
		protected SetupAutoDataAttribute( Func<DelegatedAutoDataParameter, IEnumerable<object[]>> factory ) : base( SetupFixtureFactory.Instance.Create, factory )
		{ }
	}*/
}