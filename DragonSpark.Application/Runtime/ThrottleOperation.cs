using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Runtime;

public class ThrottleOperation<T> : IOperation<T> where T : notnull
{
	readonly Func<T, Task>                      _subject;
	readonly TimeSpan                           _interval;
	readonly ITable<T, CancellationTokenSource> _sources;

	public ThrottleOperation(Func<T, Task> subject, TimeSpan interval) : this(subject, interval, Sources<T>.Default) {}

	public ThrottleOperation(Func<T, Task> subject, TimeSpan interval, ITable<T, CancellationTokenSource> sources)
	{
		_subject  = subject;
		_interval = interval;
		_sources  = sources;
	}

	public ValueTask Get(T parameter)
	{
		if (_sources.TryPop(parameter, out var previous))
		{
			previous.Cancel();
			previous.Dispose();
		}

		var source = _sources.Get(parameter);

		_ = Run(parameter, source);

		return ValueTask.CompletedTask;
	}

	async Task Run(T parameter, CancellationTokenSource source)
	{
		try
		{
			await Task.Delay(_interval, source.Token).On();
			await _subject(parameter).Off();
		}
		catch (OperationCanceledException) {}
		finally
		{
			if (_sources.TryPop(parameter, out var current) && current == source)
			{
				source.Dispose();
			}
			else if (current.Account() is not null)
			{
				_sources.Assign(parameter, current);
			}
		}
	}
}

public class ThrottleOperation : ThrottleOperation<None>, IOperation
{
	public ThrottleOperation(Func<Task> subject, TimeSpan interval) : base(_ => subject(), interval) {}

	public ValueTask Get() => Get(None.Default);
}