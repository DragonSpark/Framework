using DragonSpark.Runtime.Activation;

namespace DragonSpark.Runtime.Execution
{
	public sealed class ContextDetails : IActivateUsing<string>
	{
		public ContextDetails(string name) : this(new Details(name), new ThreadingDetails(), new TaskDetails()) {}

		public ContextDetails(Details details, ThreadingDetails threading, TaskDetails task)
		{
			Details   = details;
			Threading = threading;
			Task      = task;
		}

		public Details Details { get; }
		public ThreadingDetails Threading { get; }
		public TaskDetails Task { get; }
	}
}