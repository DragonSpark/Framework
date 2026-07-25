using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class ThreadFormatter : IFormatter<Thread>
{
	public static ThreadFormatter Default { get; } = new();

	ThreadFormatter() {}

	public string Get(Thread parameter)
		=> $"#{parameter.ManagedThreadId} {parameter.Priority} {parameter.Name.OrNone()}";
}