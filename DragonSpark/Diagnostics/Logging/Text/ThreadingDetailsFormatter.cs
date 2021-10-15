using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using DragonSpark.Text;
using System.Threading;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class ThreadingDetailsFormatter : IFormatter<ThreadingDetails>
{
	public static ThreadingDetailsFormatter Default { get; } = new ThreadingDetailsFormatter();

	ThreadingDetailsFormatter() : this(ThreadFormatter.Default) {}

	readonly IFormatter<Thread> _thread;

	public ThreadingDetailsFormatter(IFormatter<Thread> thread) => _thread = thread;

	public string Get(ThreadingDetails parameter)
		=> $"Thread: {_thread.Get(parameter.Thread)}, SynchronizationContext: {parameter.Synchronization.OrNone()}";
}