using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.SetupRegistrationByConventionCommand
	{
		[ComponentModel.Singleton( typeof(AssemblyProvider) )]
		public IAssemblyProvider AssemblyLocator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( AssemblyLocator );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( context.Container(), context.Logger );
			return result;
		}
	}
}