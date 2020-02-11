using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging.Text;
using DragonSpark.Runtime.Execution;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public sealed class ContextFormatterTests
	{
		[Theory, AutoData]
		void Verify(ContextFormatter sut, ContextDetails details)
		{
			var thread = details.Threading.Thread;
			sut.Get(details)
			   .Should()
			   .Be($"[{TimestampFormatter.Default.Get(Epoch.Default)}] {details.Details.Name}: Task: {details.Task.TaskId.OrNone()}, Default/Current Scheduler: {details.Task.Default.Id}/{details.Task.Default.Id}, Thread: #{thread.ManagedThreadId} {thread.Priority} {thread.Name.OrNone()}, SynchronizationContext: {details.Threading.Synchronization.OrNone()}");
		}
	}
}