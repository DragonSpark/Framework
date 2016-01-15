using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Windows.Setup
{
	public class SetupRegistrationByConventionCommand : DelegatedSetupCommand<RegisterFromMetadataCommand, ConventionRegistrationProfile> {}
}