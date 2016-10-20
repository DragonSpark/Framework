using DragonSpark.Application;
using DragonSpark.Diagnostics;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.Runtime;
using PostSharp.Aspects;
using System.Linq;
using DragonSpark.Activation;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Windows.FileSystem;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			Time.Default.Configuration.Assign( CurrentTime.Default.Wrap() );
			DragonSpark.Application.Execution.Context.Assign( ExecutionContext.Default );
			LoggingConfiguration.Default.Configurators.Assign( o => new LoggerExportedConfigurations( DefaultSystemLoggerConfigurations.Default.Get().ToArray() ).Get().Wrap() );

			Path.Implementation.DefaultImplementation.Assign( o => new MockPath() );
			Directory.Implementation.DefaultImplementation.Assign( o => new MockDirectory() );
			File.Implementation.DefaultImplementation.Assign( o => new MockFile() );
			DirectoryFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockDirectoryInfo>.Default.Wrap() );
			FileFactory.Implementation.DefaultImplementation.Configuration.Assign( ParameterConstructor<string, MockFileInfo>.Default.Wrap() );
		}
	}
}