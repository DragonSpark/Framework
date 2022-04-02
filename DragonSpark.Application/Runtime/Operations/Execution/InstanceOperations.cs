using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime.Operations.Execution;

sealed class InstanceOperations : ReferenceValueStore<object, IOperations>, IInstanceOperations
{
	public static InstanceOperations Default { get; } = new();

	InstanceOperations() : base(Create.Instance) {}

	sealed class Create : ISelect<object, IOperations>
	{
		public static Create Instance { get; } = new();

		Create() {}

		public IOperations Get(object parameter)
		{
			var queue      = new DeferredOperationsQueue();
			var operations = new ProcessOperations(queue);
			var result     = new Operations(operations, queue);
			return result;
		}
	}
}