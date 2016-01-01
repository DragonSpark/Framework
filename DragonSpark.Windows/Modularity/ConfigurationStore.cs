using DragonSpark.Setup.Registration;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Modularity
{
	[RegisterFactoryForResult]
	public class ModulesConfigurationSectionFactory : ConfigurationSectionFactory<ModulesConfigurationSection>
	{
		public static ModulesConfigurationSectionFactory Instance { get; } = new ModulesConfigurationSectionFactory();

		public ModulesConfigurationSectionFactory()
		{}

		public ModulesConfigurationSectionFactory( ConfigurationFactory factory ) : base( factory )
		{}
	}

	/// <summary>
	/// Defines a store for the module metadata.
	/// </summary>
	public class ConfigurationStore : IConfigurationStore
	{
		readonly ModulesConfigurationSection section;

		public ConfigurationStore() : this( ModulesConfigurationSectionFactory.Instance.Create() )
		{}

		public ConfigurationStore( [Required]ModulesConfigurationSection section )
		{
			this.section = section;
		}

		/// <summary>
		/// Gets the module configuration data.
		/// </summary>
		/// <returns>A <see cref="ModulesConfigurationSection"/> instance.</returns>
		public ModulesConfigurationSection RetrieveModuleConfigurationSection() => section;
	}
}