using DragonSpark.Windows.Modularity;

namespace DragonSpark.Testing.TestObjects.Modules
{
	public class MockConfigurationStore : IConfigurationStore
	{
		private readonly ModulesConfigurationSection section = new ModulesConfigurationSection();

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