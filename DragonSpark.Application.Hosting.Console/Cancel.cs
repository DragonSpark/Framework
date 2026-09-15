namespace DragonSpark.Application.Hosting.Console;

public sealed class Cancel : IDisposable
{
	readonly CancellationTokenSource _source;

	public Cancel() : this(new(), AppDomain.CurrentDomain) {}

	public Cancel(CancellationTokenSource source, AppDomain domain)
	{
		_source                       =  source;
		domain.ProcessExit            += OnProcessExit;
		System.Console.CancelKeyPress += OnCancelKeyPress;
	}

	public CancellationToken Token => _source.Token;

	public bool IsCancellationRequested => _source.IsCancellationRequested;

	public void Signal()
	{
		if (!_source.IsCancellationRequested)
		{
			_source.Cancel();
		}
	}

	void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
	{
		e.Cancel = true; // Prevent abrupt process kill to allow cleanup tasks to execute
		Signal();
	}

	void OnProcessExit(object? sender, EventArgs e)
	{
		Signal();
	}

	public void Dispose()
	{
		AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
		System.Console.CancelKeyPress       -= OnCancelKeyPress;
		_source.Dispose();
	}
}