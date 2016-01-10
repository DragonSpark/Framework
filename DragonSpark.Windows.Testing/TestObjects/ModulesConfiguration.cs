using DragonSpark.Testing.Framework.Setup;

namespace DragonSpark.Windows.Testing.TestObjects
{
	public class ModulesConfiguration : ResourceConfigurationFactory
	{
		public static ModulesConfiguration Instance { get; } = new ModulesConfiguration();

		public ModulesConfiguration() : base( typeof(ModulesConfiguration) )
		{}
	}
}