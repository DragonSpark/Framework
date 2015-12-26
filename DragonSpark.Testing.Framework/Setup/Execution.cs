using DragonSpark.Activation;
using DragonSpark.Runtime.Values;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentExecution : AssignedLogical<MethodBase>, IExecutionContext
	{
		public static CurrentExecution Instance { get; } = new CurrentExecution();

		object IValue<object>.Item => Item;

		void IWritableValue<object>.Assign( object item )
		{
			Assign( (MethodInfo)item );
		}
	}
}