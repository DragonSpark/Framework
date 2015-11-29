using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Setup.Commands
{
	public class AssignServiceLocatorCommand : SetupCommand
	{
		[ComponentModel.Singleton( typeof(ServiceLocation) )]
		public IServiceLocation Location { get; set; }

		protected override void Execute( SetupContext context )
		{
			if ( !Services.Location.IsAvailable )
			{
				context.Container()
					.TryResolve<IServiceLocator>()
					.With( x => Assign( context, x ) );
			}
		}

		protected virtual void Assign( SetupContext context, IServiceLocator locator )
		{
			context.Logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			locator.Register( Location );
			Location.Assign( locator );
		}
	}
}