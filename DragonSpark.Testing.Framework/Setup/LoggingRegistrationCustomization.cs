using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Extensions;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework.Setup
{
	public class LoggingRegistrationCustomization : ICustomization
	{
		[Activate]
		public IUnityContainer Container { get; set; }

		public void Customize( IFixture fixture )
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