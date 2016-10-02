using DragonSpark.Testing.Framework.Application.Setup;

namespace DragonSpark.Windows.Testing.TestObjects
{
	public class ModulesConfiguration : ResourceConfigurationFactory
	{
		public static ModulesConfiguration Default { get; } = new ModulesConfiguration();

		public ModulesConfiguration() : base( typeof(ModulesConfiguration) )
		{}
	}
}