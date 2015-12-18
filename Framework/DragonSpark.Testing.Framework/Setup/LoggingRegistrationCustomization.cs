using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Extensions;
using DragonSpark.Testing.Framework.Setup.Location;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class LoggingRegistrationCustomization : CustomizationBase
	{
		[Activate]
		public IUnityContainer Container { get; set; }

		protected override void Customize( IFixture fixture )
		{
			fixture.Item<OutputCustomization>().With( customization =>
			{
				customization.Register( Container.TryResolve<IMessageRecorder> );
				customization.Register( () => Container.TryResolve<IMessageLogger>() as IMessageRecorder );
				customization.Register( () => Container.DetermineLogger() as IMessageRecorder );
				customization.Register( () => Container.Extend().MessageLogger as IMessageRecorder );
			} );
		}
	}
}