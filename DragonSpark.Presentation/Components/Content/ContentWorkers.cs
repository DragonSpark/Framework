using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class ContentWorkers : ISelect<Task, ContentWorker>
	{
		public static ContentWorkers Default { get; } = new();

		ContentWorkers() {}

		public ContentWorker Get(Task parameter)
		{
			var source = new TaskCompletionSource();
			var worker = new AllocatedWorker(parameter, source);
			return new(worker.Get(), source.Task);
		}
	}

	sealed class ContentWorkers<T> : ISelect<IResulting<T?>, ContentWorker<T>>
	{
		public static ContentWorkers<T> Default { get; } = new();

		ContentWorkers() {}

		public ContentWorker<T> Get(IResulting<T?> parameter)
		{
			var source = new TaskCompletionSource<T?>();
			var worker = new Worker<T>(parameter, source);
			return new(worker.Get(), source.Task);
		}
	}
}