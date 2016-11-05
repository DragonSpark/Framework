using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Framework.Runtime;
using DragonSpark.Windows;
using DragonSpark.Windows.FileSystem;
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
			DragonSpark.Application.Clock.Default.Assign( Time.Default.Scoped );
			LoggerAlterations.Default.Scope.Assign( DefaultSystemLoggerAlterations.Default.IncludeExports().ToSource().Scoped );

			Path.Default.Assign<MockPath>();
			Directory.Default.Assign<MockDirectory>();
			DirectoryInfoFactory.DefaultImplementation.Implementation.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.Scoped );
			File.Default.Assign<MockFile>();
			FileInfoFactory.DefaultImplementation.Implementation.Assign( ParameterConstructor<string, MockFileInfo>.Default.Scoped );

			UserSettingsFilePath.Default.Assign( Application.Setup.UserSettingsFilePath.Default.Scoped );
		}
	}
}