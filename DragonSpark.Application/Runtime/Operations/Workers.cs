using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations
{
	public sealed class Workers : ISelect<Task, Worker>
	{
		public static Workers Default { get; } = new();

		Workers() {}

		public Worker Get(Task parameter)
		{
			var source = new TaskCompletionSource();
			var worker = new WorkerOperation(parameter, source).Get();
			return new(worker, source.Task);
		}
	}
}