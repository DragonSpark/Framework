using System;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
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
}