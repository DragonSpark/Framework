using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using DragonSpark.Text;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class TaskDetailsFormatter : IFormatter<TaskDetails>
{
	public static TaskDetailsFormatter Default { get; } = new TaskDetailsFormatter();

	TaskDetailsFormatter() {}

	public string Get(TaskDetails parameter)
		=> $"Task: {parameter.TaskId.OrNone()}, Default/Current Scheduler: {parameter.Default.Id}/{parameter.Current.Id}";
}