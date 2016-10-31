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

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			DragonSpark.Application.Execution.Context.Assign( ExecutionContext.Default );
			Time.Default.Configuration.Assign( CurrentTime.Default.Wrap() );
			LoggingConfiguration.Default.Configurators.Assign( new LoggerExportedConfigurations( DefaultSystemLoggerConfigurations.Default.Unwrap() ).Global );

			Path.Implementation.DefaultImplementation.Assign<MockPath>();
			Directory.Implementation.DefaultImplementation.Assign<MockDirectory>();
			DirectoryInfoFactory.Default.Configuration.Assign( DirectoryInfoFactory.Default.Configuration.GetFactory().Apply( MappedPathAlteration.Current.GetValue ).Global );
			DirectoryInfoFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.Global );
			File.Implementation.DefaultImplementation.Assign<MockFile>();
			FileInfoFactory.Default.Configuration.Assign( FileInfoFactory.Default.Configuration.GetFactory().Apply( MappedPathAlteration.Current.GetValue ).Global );
			FileInfoFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.Global );

			UserSettingsFilePath.Default.Configuration.Assign( Application.Setup.UserSettingsFilePath.Current.Global );
		}
	}
}