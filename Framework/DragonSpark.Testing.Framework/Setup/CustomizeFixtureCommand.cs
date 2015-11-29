using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	[ContentProperty( nameof(Customizations) )]
	public class CustomizeFixtureCommand : SetupAutoDataCommandBase
	{
		public Collection<ICustomization> Customizations { get; } = new Collection<ICustomization>();

		protected override void OnSetup( SetupContext context, SetupAutoDataContext setup )
		{
			context.Container().EnsureRegistered( () => setup.Fixture );
			var customizations = new ICustomization[]
			{
				AmbientCustomizationsCustomization.Instance,
				new CurrentMethodCustomization( setup.Method ),
				new CompositeCustomization( Customizations ),
				new MetadataCustomization( setup.Method )
			};
			customizations.Each( customization => setup.Fixture.Customize( customization ) );
		}
	}
}