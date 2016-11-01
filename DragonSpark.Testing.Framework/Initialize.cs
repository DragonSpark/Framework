using DragonSpark.Activation;
using DragonSpark.Configuration;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Framework.Runtime;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Setup;
using PostSharp.Aspects;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			DragonSpark.Application.Execution.Context.Assign( ExecutionContext.Default );
			DragonSpark.Application.Clock.Default.Configuration.Assign( Time.Default.GlobalCache() );
			LoggingConfiguration.Default.Configurators.Assign( new LoggerExportedConfigurations( DefaultSystemLoggerConfigurations.Default.Unwrap() ).Global );

			Path.Default.Configuration.Assign<MockPath>();
			Directory.Default.Configuration.Assign<MockDirectory>();
			DirectoryInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.AssignGlobal );
			File.Default.Configuration.Assign<MockFile>();
			FileInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.AssignGlobal );

			UserSettingsFilePath.Default.Configuration.Assign( Application.Setup.UserSettingsFilePath.Default.GlobalCache() );
		}
	}
}