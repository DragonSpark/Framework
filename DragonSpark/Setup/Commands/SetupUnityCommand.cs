using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Locate, Required]
		public IMessageLogger Logger { [return: Required]get; set; }

		protected override void OnExecute( object parameter )
		{
			Logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );
			base.OnExecute( parameter );
		}
	}
}