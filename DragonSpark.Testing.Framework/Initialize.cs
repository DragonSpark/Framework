using DragonSpark.Activation;
using DragonSpark.Configuration;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
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

			Path.DefaultImplementation.Implementation.Assign<MockPath>();
			Directory.DefaultImplementation.Implementation.Assign<MockDirectory>();
			DirectoryInfoFactory.Default.Configuration.Assign( DirectoryInfoFactory.Default.Configuration.GetFactory().Apply( MappedPathAlteration.Current.GetValue ).AssignGlobal );
			DirectoryInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.AssignGlobal );
			File.DefaultImplementation.Implementation.Assign<MockFile>();
			FileInfoFactory.Default.Configuration.Assign( FileInfoFactory.Default.Configuration.GetFactory().Apply( MappedPathAlteration.Current.GetValue ).AssignGlobal );
			FileInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.AssignGlobal );

			UserSettingsFilePath.Default.Configuration.Assign( Application.Setup.UserSettingsFilePath.Current.Global );
		}
	}
}