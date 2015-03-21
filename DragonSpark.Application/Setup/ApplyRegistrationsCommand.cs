using DragonSpark.Setup;
using Prism;
using Prism.Modularity;
using Prism.Unity;
using AssemblyProvider = DragonSpark.Application.Runtime.AssemblyProvider;

namespace DragonSpark.Application.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.SetupRegistrationByConventionCommand
	{
		public IAssemblyProvider Locator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( Locator ?? AssemblyProvider.Instance );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( context.Container(), context.Logger );
			return result;
		}
	}
}