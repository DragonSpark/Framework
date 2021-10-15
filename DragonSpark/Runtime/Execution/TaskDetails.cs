using System.Threading.Tasks;

namespace DragonSpark.Runtime.Execution;

public sealed class TaskDetails
{
	public TaskDetails() : this(TaskScheduler.Default, TaskScheduler.Current, Task.CurrentId) {}

	public TaskDetails(TaskScheduler @default, TaskScheduler current, int? taskId)
	{
		Default = @default;
		Current = current;
		TaskId  = taskId;
	}

	public TaskScheduler Default { get; }

	public TaskScheduler Current { get; }

	public int? TaskId { get; }
}