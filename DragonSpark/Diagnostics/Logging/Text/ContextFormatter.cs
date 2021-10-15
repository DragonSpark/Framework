using DragonSpark.Runtime.Execution;
using DragonSpark.Text;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class ContextFormatter : IFormatter<ContextDetails>
{
	public static ContextFormatter Default { get; } = new ContextFormatter();

	ContextFormatter() : this(DetailsFormatter.Default, TaskDetailsFormatter.Default,
	                          ThreadingDetailsFormatter.Default) {}

	readonly IFormatter<Details>          _details;
	readonly IFormatter<TaskDetails>      _task;
	readonly IFormatter<ThreadingDetails> _thread;

	public ContextFormatter(IFormatter<Details> details, IFormatter<TaskDetails> task,
	                        IFormatter<ThreadingDetails> thread)
	{
		_details = details;
		_task    = task;
		_thread  = thread;
	}

	public string Get(ContextDetails parameter)
		=> $"{_details.Get(parameter.Details)}: {_task.Get(parameter.Task)}, {_thread.Get(parameter.Threading)}";
}