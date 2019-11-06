using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Runtime.Execution
{
	sealed class AssociatedResources : AssociatedResource<object, Disposables>
	{
		public static AssociatedResources Default { get; } = new AssociatedResources();

		AssociatedResources() {}
	}
}