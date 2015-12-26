using DragonSpark.Activation;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentExecution : AssignedLogical<MethodBase>, IExecutionContext
	{
		public static CurrentExecution Instance { get; } = new CurrentExecution();

		CurrentExecution()
		{}
	}
}