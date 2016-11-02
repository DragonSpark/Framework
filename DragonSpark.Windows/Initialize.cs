using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using PostSharp.Aspects;
using System.Reflection;

namespace DragonSpark.Windows
{
	public static class Initialize
	{
		[ModuleInitializer( 0 ), UsedImplicitly]
		public static void Execute()
		{
			Application.Execution.Default.Assign( ExecutionContext.Default );

			AssemblyLoader.Default.Assign( o => Assembly.LoadFile );
			AssemblyResourcePathSelector.Default.Assign( o => new AssemblyFilePathSelector().ToEqualityCache().Get );
		}
	}
}