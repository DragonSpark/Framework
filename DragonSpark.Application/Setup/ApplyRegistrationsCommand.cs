using DragonSpark.Application.Runtime;
using DragonSpark.ComponentModel;
using DragonSpark.Setup;
using Prism;
using Prism.Unity;

namespace DragonSpark.Application.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.SetupRegistrationByConventionCommand
	{
		[Activate( typeof(AssemblyLocator) )]
		public IAssemblyLocator Locator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( Locator );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( context.Container(), context.Logger );
			return result;
		}
	}
}