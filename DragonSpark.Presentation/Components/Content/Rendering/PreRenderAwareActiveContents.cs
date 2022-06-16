using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

public readonly record struct RenderStateAwareActiveContentsInput<T>(IActiveContents<T> Previous, ComponentBase Owner,
                                                                     IResulting<T?> Source);

sealed class RenderStateAwareActiveContents<T> : ISelect<RenderStateAwareActiveContentsInput<T>, IActiveContent<T>>
{
	readonly PersistentComponentState _state;
	readonly IRenderContentKey        _key;

	public RenderStateAwareActiveContents(PersistentComponentState state, IRenderContentKey key)
	{
		_state = state;
		_key   = key;
	}

	public IActiveContent<T> Get(RenderStateAwareActiveContentsInput<T> parameter)
	{
		var (previous, owner, source) = parameter;
		var key          = _key.Get(owner);
		var store        = new RenderAwareResult<T?>(key, _state, source);
		var content      = previous.Get(new(owner, store));
		var assignment   = new StateAssignment<T>(content, _state, key);
		var subscription = _state.RegisterOnPersisting(assignment.Get);
		return new RenderStateAwareActiveContent<T>(content, subscription);
	}
}

sealed class RenderingAwareActiveContents<T> : IActiveContents<T>
{
	readonly IsPreRendering                    _condition;
	readonly RenderStateAwareActiveContents<T> _contents;
	readonly IActiveContents<T>                _previous;

	public RenderingAwareActiveContents(IsPreRendering condition, RenderStateAwareActiveContents<T> contents,
	                                    IActiveContents<T> previous)
	{
		_condition = condition;
		_contents  = contents;
		_previous  = previous;
	}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter)
	{
		var (owner, source) = parameter;
		var result = _condition.Get() ? _contents.Get(new(_previous, owner, source)) : _previous.Get(parameter);
		return result;
	}
}

sealed class StateAssignment<T> : IAllocated
{
	readonly IResulting<T?>           _previous;
	readonly PersistentComponentState _state;
	readonly string                   _key;

	public StateAssignment(IResulting<T?> previous, PersistentComponentState state, string key)
	{
		_previous = previous;
		_state    = state;
		_key      = key;
	}

	public async Task Get()
	{
		var instance = await _previous.Await();
		if (instance != null)
		{
			_state.PersistAsJson(_key, instance);
		}
	}
}

sealed class RenderStateAwareActiveContent<T> : IActiveContent<T>
{
	readonly IActiveContent<T>                    _previous;
	readonly PersistingComponentStateSubscription _subscription;

	public RenderStateAwareActiveContent(IActiveContent<T> previous, PersistingComponentStateSubscription subscription)
	{
		_previous     = previous;
		_subscription = subscription;
	}

	public void Execute(T parameter)
	{
		_previous.Execute(parameter);
	}

	public ValueTask<T?> Get() => _previous.Get();

	public IUpdateMonitor Monitor => _previous.Monitor;

	public void Dispose()
	{
		_subscription.Dispose();
		_previous.Dispose();
	}
}

// TODO

sealed class RenderAwareResult<T> : DragonSpark.Model.Operations.Coalesce<T>
{
	public RenderAwareResult(string key, PersistentComponentState state, IResulting<T> second)
		: this(new Persisted<T>(key, state).Then().Operation().Out(), second) {}

	public RenderAwareResult(IResulting<T?> first, IResulting<T> second) : base(first, second) {}
}

public class Persisted<T> : IResult<T?>
{
	readonly string                   _key;
	readonly PersistentComponentState _state;

	public Persisted(string key, PersistentComponentState state)
	{
		_key   = key;
		_state = state;
	}

	public T? Get()
	{
		var success = _state.TryTakeFromJson<T>(_key, out var found);
		return success ? found : default;
	}
}