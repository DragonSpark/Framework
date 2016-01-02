using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;

namespace DragonSpark.Testing.Framework
{
	public static class Initialize
	{
		[ModuleInitializer( 0 )]
		public static void Execution()
		{
			Activation.Execution.Initialize( CurrentExecution.Instance );
		}
	}
}