using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Model;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Presentation.Components.Content.History;

/*sealed class HistoryAwareActiveContents<T> : IActiveContents<T>
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
}*/

public sealed class HistoryVariable<T> : MemoryVariable<T>
{
	public HistoryVariable(IMemoryCache memory, string key) : base(memory, key, Configuration.Default) {}
}

/*public sealed class HistoryCache : MemoryCache
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
}*/

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

/*
// TODO

sealed class HistoryAwarePaging<T> : IPaging<T>
{
	readonly HistoryStateAwarePagingContents<T> _contents;
	readonly IPaging<T>                        _previous;

	public HistoryAwarePaging(IRenderState state, HistoryStateAwarePagingContents<T> contents, IPaging<T> previous)
	{
		_contents = contents;
		_previous = previous;
	}

	public IPages<T> Get(PagingInput<T> parameter)
	{
		var (owner, _, _) = parameter;
		var previous = _previous.Get(parameter);
		var result   =  _contents.Get(new(owner, previous));
		return result;
	}
}

sealed class HistoryStateAwarePagingContents<T> : ISelect<RenderStateAwarePagingContentsInput<T>, IPages<T>>
{
	readonly HistoryCache      _memory;
	readonly IRenderContentKey _key;

	public HistoryStateAwarePagingContents(HistoryCache memory, IRenderContentKey key)
	{
		_memory = memory;
		_key    = key;
	}

	public IPages<T> Get(RenderStateAwarePagingContentsInput<T> parameter)
	{
		var (owner, source) = parameter;
		var key = _key.Get(owner);
		return new Selection(source.Then().Store().In(_memory).For(), new(_memory, key));
	}

	sealed class Selection : HistoryAwareSelection<Stop<PageInput>, PageResult<T>>, IPages<T>
	{
		public Selection(IPages<T> previous, RenderVariable<PageResult<T>> store) : base(previous, store) {}
	}
}

public class HistoryAwareSelection<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly RenderVariable<TOut>  _store;
	readonly ISelecting<TIn, TOut> _previous;

	protected HistoryAwareSelection(ISelecting<TIn, TOut> previous, RenderVariable<TOut> store)
	{
		_store = store;
		_previous = previous;
	}

	public async ValueTask<TOut> Get(TIn parameter)
	{
		if (_store.Get() is {} current)
		{
			return current;
		}

		var previous = await _previous.Off(parameter);
		_store.Execute(previous);
		return previous;
	}
}*/