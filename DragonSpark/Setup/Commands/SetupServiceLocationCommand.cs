using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Setup.Commands
{
	public class SetupServiceLocationCommand : SetupCommand
	{
		[Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		[Factory( typeof(ServiceLocatorFactory) )]
		public IServiceLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			Locator.Register( Location );
			Location.Assign( Locator );
		}
	}
}