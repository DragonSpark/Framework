using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Setup.Commands
{
	public class SetupServiceLocationCommand : SetupCommand
	{
		[ComponentModel.Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		[Factory( typeof(ServiceLocatorFactory) )]
		public IServiceLocator Locator { get; set; }

		protected override void Execute( SetupContext context )
		{
			context.MessageLogger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			Locator.Register( Location );
			Location.Assign( Locator );

			var temp = Services.Location.Locator;
			var asdf = temp == Locator;
			var asdfasd = Activator.Current;
		}
	}
}