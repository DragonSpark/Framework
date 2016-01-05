using DragonSpark.Runtime;
using DragonSpark.Setup;
using Ploeh.AutoFixture;
using System.Windows.Markup;

namespace DragonSpark.Testing.Framework.Setup
{
	[ContentProperty( nameof(Customizations) )]
	public class CustomizeFixtureCommand : SetupAutoDataCommandBase
	{
		public Collection<ICustomization> Customizations { get; } = new Collection<ICustomization>();

		protected override void OnExecute( ISetupParameter<AutoData> parameter ) => parameter.Arguments.Fixture.Customize( new CompositeCustomization( Customizations ) );
	}
}