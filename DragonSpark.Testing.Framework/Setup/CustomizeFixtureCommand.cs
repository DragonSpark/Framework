using DragonSpark.Runtime;
using Ploeh.AutoFixture;
using System.Windows.Markup;

namespace DragonSpark.Testing.Framework.Setup
{
	[ContentProperty( nameof(Customizations) )]
	public class CustomizeFixtureCommand : SetupAutoDataCommandBase
	{
		public Collection<ICustomization> Customizations { get; } = new Collection<ICustomization>();

		protected override void OnExecute( AutoData parameter ) => parameter.Fixture.Customize( new CompositeCustomization( Customizations ) );
	}
}