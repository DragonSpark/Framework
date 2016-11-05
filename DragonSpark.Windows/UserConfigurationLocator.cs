using System.Configuration;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Windows
{
	sealed class UserConfigurationLocator : ParameterizedSourceBase<IFileInfo, System.Configuration.Configuration>
	{
		public static UserConfigurationLocator Default { get; } = new UserConfigurationLocator();
		UserConfigurationLocator() {}

		public override System.Configuration.Configuration Get( IFileInfo parameter )
		{
			var source = parameter.Directory.Parent.GetFiles( parameter.Name ).Only();
			var result = source != null ?
				ConfigurationManager.OpenMappedExeConfiguration( new ExeConfigurationFileMap { ExeConfigFilename = source.FullName }, ConfigurationUserLevel.None ) : null;
			return result;
		}
	}
}