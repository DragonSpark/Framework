using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.Commands.SetupRegistrationByConventionCommand
	{
		[Activate]
		public IAssemblyProvider AssemblyLocator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( AssemblyLocator );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( Container, MessageLogger );
			return result;
		}
	}
}