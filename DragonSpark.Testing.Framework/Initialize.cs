using DragonSpark.Diagnostics;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Testing.Framework.Runtime;
using PostSharp.Aspects;
using System.Linq;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			DragonSpark.Application.Execution.Context.Assign( ExecutionContext.Default );
			LoggingConfiguration.Default.Configurators.Assign( o => new LoggerExportedConfigurations( DefaultSystemLoggerConfigurations.Default.Get().ToArray() ).Get().Wrap() );
		}
	}
}