using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.ComponentModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Properties;
using DragonSpark.Runtime.Specifications;
using System;
using System.Linq;

namespace DragonSpark.Setup.Commands
{
	public class SetupUnityCommand : ConfigureUnityCommand
	{
		[Activate]
		public PersistingServiceRegistry Registry { get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter )
		{
			parameter.Logger.Information( Resources.ConfiguringUnityContainer, Priority.Low );

			var command = new RegisterAllClassesCommand( Registry, Specifications.NotRegistered( Container ) );
			parameter.Append( parameter.Items ).Except( Container.ToItem() ).Each( o =>
			{
				command.ExecuteWith( new InstanceRegistrationParameter( o ) );
			} );

			base.OnExecute( parameter );
		}
	}
}