using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.SetupRegistrationByConventionCommand
	{
		public IAssemblyProvider AssemblyLocator { get; set; }

		public ISingletonLocator SingletonLocator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( AssemblyLocator ?? FilteredAssemblyProvider.Instance );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( context.Container(), context.Logger, SingletonLocator ?? DragonSpark.Setup.SingletonLocator.Instance );
			return result;
		}
	}
}