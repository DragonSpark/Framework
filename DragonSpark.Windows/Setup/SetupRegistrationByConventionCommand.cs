using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Windows.Runtime;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DragonSpark.Setup.SetupRegistrationByConventionCommand
	{
		[ComponentModel.Singleton( typeof(FilteredAssemblyProvider) )]
		public IAssemblyProvider AssemblyLocator { get; set; }

		[ComponentModel.Singleton( typeof(SingletonLocator) )]
		public ISingletonLocator SingletonLocator { get; set; }

		[ComponentModel.Singleton( typeof(SystemActivator) )]
		public IActivator Activator { get; set; }

		protected override IConventionRegistrationProfileProvider DetermineProfileProvider( SetupContext context )
		{
			var result = new ConventionRegistrationProfileProvider( AssemblyLocator );
			return result;
		}

		protected override IConventionRegistrationService DetermineService( SetupContext context )
		{
			var result = new UnityConventionRegistrationService( context.Container(), Activator, context.Logger, SingletonLocator );
			return result;
		}
	}
}