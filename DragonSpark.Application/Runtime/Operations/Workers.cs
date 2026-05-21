using DragonSpark.Model.Operations.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class Workers : ISelect<WorkerInput, Work>
{
	public static Workers Default { get; } = new();

	Workers() {}

	public Work Get(WorkerInput parameter)
	{
		var (subject, complete) = parameter;
		var source = new TaskCompletionSource();
		var worker = new WorkerOperation(subject, source, complete).Get();
		return new(worker, source.Task);
	}
}

public sealed class Workers<T> : ReferenceValueStore<IResulting<T?>, Worker>
{
	public Workers(ICompleted<T?> completed) : base(new ComposeWorkers<T>(completed)) {}
}