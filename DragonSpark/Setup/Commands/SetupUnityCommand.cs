using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using System.Linq;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Setup.Commands
{
	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Locate, Required]
		public RegisterAllClassesCommand<OnlyIfNotRegistered> Command { [return: Required]get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			parameter.Logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );

			parameter	
				.Append( parameter.Items )
				.Except( Container.ToItem() )
				.Select( o => new InstanceRegistrationParameter( o ) )
				.Each( Command.ExecuteWith );

			base.OnExecute( parameter );
		}
	}
}