using DragonSpark.Sources.Parameterized;
using System.Configuration;

namespace DragonSpark.Windows.Setup
{
	public class FileConfigurationFactory : ParameterizedSourceBase<string, object>
	{
		readonly ConfigurationFileMap map;

		public FileConfigurationFactory( string filePath ) : this( new ConfigurationFileMap( filePath ) )
		{ }

		public FileConfigurationFactory( ConfigurationFileMap map )
		{
			this.map = map;
		}

		public override object Get( string parameter ) => ConfigurationManager.OpenMappedMachineConfiguration( map ).GetSection( parameter );
	}
}