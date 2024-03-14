using SerilogTracing;
using System;

namespace DragonSpark.Diagnostics;

public readonly struct Tracing : IDisposable
{
	readonly IDisposable    _disposable;

	public Tracing(IDisposable disposable, LoggerActivity activity)
	{
		Activity   = activity;
		_disposable = disposable;
	}

	public LoggerActivity Activity { get; }

	public void Dispose()
	{
		Activity.Dispose();
		_disposable.Dispose();
	}
}