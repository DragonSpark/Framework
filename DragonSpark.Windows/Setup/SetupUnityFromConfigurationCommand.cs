using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity.Configuration;
using PostSharp.Patterns.Contracts;
using System.Linq;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Windows.Setup
{
	[Discoverable]
	public class UnityConfigurationSectionFactory : ConfigurationSectionFactory<UnityConfigurationSection>
	{
		public static UnityConfigurationSectionFactory Instance { get; } = new UnityConfigurationSectionFactory();

		public UnityConfigurationSectionFactory()
		{}
	}

	public class SetupUnityFromConfigurationCommand : UnityCommand
	{
		[Locate, Required]
		public UnityConfigurationSection Section { [return: Required]get; set; }

		protected override void OnExecute( IApplicationSetupParameter parameter ) => Section.Containers.Any().IsTrue( () => Container.LoadConfiguration() );
	}
}