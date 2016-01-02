using DragonSpark.Windows.Modularity;

namespace DragonSpark.Windows.Testing.TestObjects.Modules
{
	public class MockConfigurationStore : IConfigurationStore
	{
		readonly private ModulesConfigurationSection section = new ModulesConfigurationSection();

		public ModuleConfigurationElement[] Modules
		{
			set { section.Modules = new ModuleConfigurationElementCollection(value); }
		}

		public ModulesConfigurationSection RetrieveModuleConfigurationSection()
		{
			return section;
		}
	}
}