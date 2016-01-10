using System;
using System.Configuration;
using DragonSpark.Activation.FactoryModel;

namespace DragonSpark.Windows.Setup
{
	public class FileConfigurationFactory : FactoryBase<string, object>
	{
		readonly ConfigurationFileMap map;

		public FileConfigurationFactory( string filePath ) : this( new ConfigurationFileMap( filePath ) )
		{ }

		public FileConfigurationFactory( ConfigurationFileMap map )
		{
			this.map = map;
		}

		protected override object CreateItem( string parameter ) => ConfigurationManager.OpenMappedMachineConfiguration( map ).GetSection( parameter );
	}
}