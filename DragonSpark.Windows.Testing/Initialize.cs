using DragonSpark.Testing.Framework.Setup;
using PostSharp.Aspects;

namespace DragonSpark.Windows.Testing
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