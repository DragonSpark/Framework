using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using PostSharp.Aspects;
using System.Reflection;

namespace DragonSpark.Windows
{
	[UsedImplicitly]
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execute()
		{
			Application.Execution.Context.Assign( ExecutionContext.Default );
			TypeSystem.Configuration.AssemblyLoader.Assign( o => Assembly.LoadFile );
			TypeSystem.Configuration.AssemblyPathLocator.Assign( o => AssemblyLocator.Default.Get );
		}
	}
}