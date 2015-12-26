using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;

namespace DragonSpark.Testing
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			DragonSpark.Activation.Execution.Initialize( CurrentExecution.Instance );
		}
	}
}