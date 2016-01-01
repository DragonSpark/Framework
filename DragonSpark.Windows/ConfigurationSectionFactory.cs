using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Configuration;
using System.Linq;

namespace DragonSpark.Windows
{
	public class ConfigurationFactory : FactoryBase<Func<string, object>>
	{
		public static ConfigurationFactory Instance { get; } = new ConfigurationFactory();

		protected override Func<string, object> CreateItem()
		{
			return ConfigurationManager.GetSection;
		}
	}

	public class FileConfigurationFactory : ConfigurationFactory
	{
		readonly ConfigurationFileMap map;

		public FileConfigurationFactory( string filePath ) : this( new ConfigurationFileMap( filePath ) )
		{ }

		public FileConfigurationFactory( ConfigurationFileMap map )
		{
			this.map = map;
		}

		protected override Func<string, object> CreateItem()
		{
			return ConfigurationManager.OpenMappedMachineConfiguration( map ).GetSection;
		}
	}

	public class ConfigurationSectionFactory<T> : FactoryBase<T> where T : ConfigurationSection
	{
		readonly ConfigurationFactory factory;

		public ConfigurationSectionFactory() : this( ConfigurationFactory.Instance )
		{}

		public ConfigurationSectionFactory( [Required]ConfigurationFactory factory )
		{
			this.factory = factory;
		}

		protected override T CreateItem()
		{
			var name = typeof(T).Name.SplitCamelCase().First().ToLower();
			var resolver = factory.Create();
			var result = resolver( name ) as T;
			return result;
		}
	}
}