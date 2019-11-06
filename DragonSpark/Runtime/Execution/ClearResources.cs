using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Runtime.Execution
{
	sealed class ClearResources : RemoveCommand<object, Disposables>
	{
		public static ClearResources Default { get; } = new ClearResources();

		ClearResources() : base(AssociatedResources.Default) {}
	}
}