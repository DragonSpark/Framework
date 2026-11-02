using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Model;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Components.Content.Rendering;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class HistoryAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T> _previous;
	readonly IRenderContentKey  _key;
	readonly HistoryCache       _store;

	public HistoryAwareActiveContents(IActiveContents<T> previous, IRenderContentKey key, HistoryCache store)
	{
		_previous = previous;
		_key      = key;
		_store    = store;
	}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter)
	{
		var (owner, _) = parameter;
		var key     = _key.Get(owner);
		var content = _previous.Get(parameter);
		return new HistoryAwareActiveContent<T>(new HistoryVariable<T>(_store, key), content);
	}
}

public sealed class HistoryVariable<T> : MemoryVariable<T>
{
	public HistoryVariable(IMemoryCache memory, string key) : base(memory, key, Configuration.Default) {}
}

public sealed class HistoryCache : MemoryCache
{
	public HistoryCache() : base(new MemoryCacheOptions()) {}
}

sealed class HistoryAwareActiveContent<T> : IActiveContent<T>
{
	readonly MemoryVariable<T> _store;
	readonly IActiveContent<T> _previous;

	public HistoryAwareActiveContent(MemoryVariable<T> store, IActiveContent<T> previous)
	{
		_store    = store;
		_previous = previous;
	}

	public async ValueTask<T?> Get()
	{
		if (_store.Get() is {} current)
		{
			return current;
		}

		var previous = await _previous.Off();
		_store.Execute(previous);
		return previous;
	}

	public void Execute(None parameter)
	{
		_store.Remove();
		_previous.Execute(parameter);
	}

	public ICondition<None> Condition => _previous.Condition;
}

sealed class Configuration : SlidingExpiration
{
	public static Configuration Default { get; } = new();

	Configuration() : base(Window.Default) {}
}

sealed class Window : DragonSpark.Model.Results.Instance<TimeSpan>
{
	public static Window Default { get; } = new();

	Window() : base(TimeSpan.FromMinutes(15)) {}
}