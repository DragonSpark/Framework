using System.Threading;

namespace DragonSpark.Runtime.Execution;

public sealed class ThreadingDetails
{
	public ThreadingDetails() : this(SynchronizationContext.Current!, Thread.CurrentThread) {}

	public ThreadingDetails(SynchronizationContext synchronization, Thread thread)
	{
		Synchronization = synchronization;
		Thread          = thread;
	}

	public SynchronizationContext Synchronization { get; }

	public Thread Thread { get; }
}