using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using PostSharp.Patterns.Contracts;
using System.Windows.Markup;

namespace DragonSpark.Testing.Framework.Setup
{
	[ContentProperty( nameof(Customizations) )]
	public class CustomizeFixtureCommand : SetupAutoDataCommandBase
	{
		public Collection<ICustomization> Customizations { get; } = new Collection<ICustomization>();

		[Activate, Required]
		public IUnityContainer Container { [return: Required]get; set; }
		
		protected override void OnExecute( ISetupParameter<SetupAutoDataParameter> parameter )
		{
			var registration = Container.Registration<EnsuredRegistrationSupport>();
			var setup = parameter.Arguments;
			registration.Instance( setup.Fixture );
			var customizations = new ICustomization[]
			{
				new CurrentMethodCustomization( setup.Method ),
				new CompositeCustomization( Customizations ),
				new MetadataCustomization( setup.Method )
			};
			customizations.Each( setup.Fixture.Customize );
		}
	}
}