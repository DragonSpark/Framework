using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IActiveContent<T> _previous;
	readonly IResulting<T?>    _source;

	public ActiveContentAdapter(IActiveContent<T> previous, IResulting<T?> source)
		: this(previous, source, previous.Monitor) {}

	public ActiveContentAdapter(IActiveContent<T> previous, IResulting<T?> source, IUpdateMonitor monitor)
	{
		_previous = previous;
		_source   = source;
		Monitor  = monitor;
	}

	public IUpdateMonitor Monitor { get; }

	public ValueTask<T?> Get() => _source.Get();

	public void Execute(T parameter)
	{
		_previous.Execute(parameter);
	}

	public void Dispose()
	{
		_previous.Dispose();
	}
}