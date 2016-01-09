using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Setup.Commands;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.Unity.Configuration;
using PostSharp.Patterns.Contracts;
using System.Linq;

namespace DragonSpark.Windows.Setup
{
	[RegisterFactory]
	public class UnityConfigurationSectionFactory : ConfigurationSectionFactory<UnityConfigurationSection>
	{
		public static UnityConfigurationSectionFactory Instance { get; } = new UnityConfigurationSectionFactory();

		public UnityConfigurationSectionFactory()
		{}

		public UnityConfigurationSectionFactory( ConfigurationFactory factory ) : base( factory )
		{}
	}

	public class SetupUnityFromConfigurationCommand : UnityCommand
	{
		[Required, Activate]
		public UnityConfigurationSection Section { [return: Required]get; set; }

		protected override void OnExecute( ISetupParameter parameter ) => Section.Containers.Any().IsTrue( () => Container.LoadConfiguration() );
	}
}