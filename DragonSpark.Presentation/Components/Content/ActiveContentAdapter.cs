using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContentAdapter<T> : IActiveContent<T>
{
	readonly IResulting<T?> _previous;
	readonly ICommand<T>    _store;

	public ActiveContentAdapter(IActiveContent<T> source, IResulting<T?> previous)
		: this(source.Monitor, previous, source) {}

	public ActiveContentAdapter(IUpdateMonitor refresh, IResulting<T?> previous, ICommand<T> store)
	{
		Monitor     = refresh;
		_previous   = previous;
		_store = store;
	}

	public IUpdateMonitor Monitor { get; }

	public ValueTask<T?> Get() => _previous.Get();

	public void Execute(T parameter)
	{
		_store.Execute(parameter);
	}
}