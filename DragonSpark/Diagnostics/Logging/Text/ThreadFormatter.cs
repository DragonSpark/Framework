using DragonSpark.Compose;
using DragonSpark.Text;
using System.Threading;

namespace DragonSpark.Diagnostics.Logging.Text;

sealed class ThreadFormatter : IFormatter<Thread>
{
	public static ThreadFormatter Default { get; } = new ThreadFormatter();

	ThreadFormatter() {}

	public string Get(Thread parameter)
		=> $"#{parameter.ManagedThreadId} {parameter.Priority} {parameter.Name.OrNone()}";
}