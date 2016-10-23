using DragonSpark.Activation;
using DragonSpark.Application;
using DragonSpark.Configuration;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Framework.Runtime;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Setup;
using PostSharp.Aspects;
using System.Configuration;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			DragonSpark.Application.Execution.Context.Assign( ExecutionContext.Default );
			Time.Default.Configuration.Assign( CurrentTime.Default.Wrap() );
			LoggingConfiguration.Default.Configurators.Assign( new LoggerExportedConfigurations( DefaultSystemLoggerConfigurations.Default.Unwrap() ).Global() );

			Path.Implementation.DefaultImplementation.Assign<MockPath>();
			Directory.Implementation.DefaultImplementation.Assign<MockDirectory>();
			
			File.Implementation.DefaultImplementation.Assign<MockFile>();
			DirectoryInfoFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.Global );
			FileInfoFactory.Default.Configuration.Assign( new AlteredParameterizedSource<string, IFileInfo>( MappedPathAlteration.Current.GetCurrent, FileInfoFactory.Default.Configuration.GetFactory() ).Global );
			FileInfoFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.Global );

			UserSettingsFilePath.Default.Configuration.Assign( Application.Setup.UserSettingsFilePath.Current.Global<ConfigurationUserLevel, string>() );
		}
	}
}