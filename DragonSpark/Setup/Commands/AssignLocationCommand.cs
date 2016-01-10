using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class AssignLocationCommand : SetupCommandBase<IApplicationSetupParameter>
	{
		[Required, Singleton, Activate]
		public IServiceLocation Location { get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			parameter.Logger.Information( Resources.ConfiguringServiceLocatorSingleton, Priority.Low );
			
			Location.Assign( parameter.Locator );
		}
	}
}