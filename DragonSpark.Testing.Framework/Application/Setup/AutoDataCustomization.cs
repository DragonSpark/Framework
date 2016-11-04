using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class DefaultAutoDataCustomization : CompositeCustomization
	{
		public DefaultAutoDataCustomization() : base( ServicesCustomization.Default, SingletonCustomization.Default, AutoMoqCustomization.Default ) {}
	}

	public sealed class AutoDataCustomization : CompositeCustomization
	{
		public AutoDataCustomization() : base( ServicesCustomization.Default, SingletonCustomization.Default, new Ploeh.AutoFixture.AutoMoq.AutoMoqCustomization() ) {}
	}

	sealed class AutoMoqCustomization : CustomizationBase
	{
		readonly static MockRelay Relay = new MockRelay();
		readonly static Postprocessor Postprocessor = new Postprocessor( new MockPostprocessor( new MethodInvoker( new MockConstructorQuery() ) ),
																		 new CompositeSpecimenCommand(
																			 new MockVirtualMethodsCommand(),
																			 new AutoMockPropertiesCommand()
																		 ) );

		public static AutoMoqCustomization Default { get; } = new AutoMoqCustomization();
		AutoMoqCustomization() {}

		protected override void OnCustomize( IFixture fixture )
		{
			fixture.Customizations.Add( Postprocessor );
			fixture.ResidueCollectors.Add( Relay );
		}
	}
}