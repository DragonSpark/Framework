using System.Diagnostics;
using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using ServiceLocator = DragonSpark.Activation.IoC.ServiceLocator;

namespace DragonSpark.Setup.Commands
{
	public class SetupServiceLocationCommand : SetupCommand
	{
		[Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		[Activate( typeof(ServiceLocator) )]
		public IServiceLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.Logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			Locator.Register( Location );
			Location.Assign( Locator );
		}
	}
}