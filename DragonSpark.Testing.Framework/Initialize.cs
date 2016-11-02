using DragonSpark.Activation;
using DragonSpark.Configuration;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Framework.Runtime;
using DragonSpark.Windows.FileSystem;
using DragonSpark.Windows.Setup;
using JetBrains.Annotations;
using PostSharp.Aspects;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 ), UsedImplicitly]
		public static void Execution()
		{
			DragonSpark.Application.Execution.Default.Assign( ExecutionContext.Default );
			DragonSpark.Application.Clock.Default.Assign( Time.Default.GlobalCache() );
			LoggingConfiguration.Default.Alterations.Assign( new LoggerExportedAlterations( DefaultSystemLoggerAlterations.Default.Unwrap() ).Global );

			Path.Default.Assign<MockPath>();
			Directory.Default.Assign<MockDirectory>();
			DirectoryInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.GlobalCache() );
			File.Default.Assign<MockFile>();
			FileInfoFactory.DefaultImplementation.Implementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.GlobalCache() );

			UserSettingsFilePath.Default.Assign( Application.Setup.UserSettingsFilePath.Default.GlobalCache() );
		}
	}
}