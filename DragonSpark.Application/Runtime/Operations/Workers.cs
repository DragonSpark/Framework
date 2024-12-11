using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class Workers : ISelect<WorkerInput, Worker>
{
    public static Workers Default { get; } = new();

    Workers() { }

    [MustDisposeResource(false)]
    public Worker Get(WorkerInput parameter)
    {
        var (subject, complete) = parameter;
        var source = new TaskCompletionSource();
        var worker = new WorkerOperation(subject, source, complete).Get();
        return new(worker, source.Task);
    }
}
