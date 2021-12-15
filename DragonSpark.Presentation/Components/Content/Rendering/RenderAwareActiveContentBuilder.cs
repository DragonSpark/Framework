using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class RenderAwareActiveContentBuilder<T> : IActiveContents<T>
{
	readonly IActiveContents<T>                _previous;
	readonly IRenderContentKey                 _key;
	readonly RenderStateAwareActiveContents<T> _contents;

	public RenderAwareActiveContentBuilder(IRenderContentKey key, RenderStateAwareActiveContents<T> contents)
		: this(ActiveContents<T>.Default, key, contents) {}

	[Candidate(false)]
	public RenderAwareActiveContentBuilder(IActiveContents<T> previous, IRenderContentKey key,
	                                       RenderStateAwareActiveContents<T> contents)
	{
		_previous = previous;
		_key      = key;
		_contents = contents;
	}

	public IActiveContent<T> Get(Func<ValueTask<T?>> parameter)
	{
		var previous = _previous.Get(parameter);
		var key      = _key.Get(parameter.Target.Verify().To<Microsoft.AspNetCore.Components.ComponentBase>());
		var input    = new RenderStateContentInput<T>(previous, key);
		var result   = _contents.Get(input);
		return result;
	}
}

// TODO:

public readonly record struct RenderStateContentInput<T>(IActiveContent<T> Previous, string Key);

sealed class RenderStateAwareActiveContents<T> : ISelect<RenderStateContentInput<T>, IActiveContent<T>>
{
	readonly IMemoryCache _memory;
	readonly RenderStates _states;

	public RenderStateAwareActiveContents(IMemoryCache memory, RenderStates states)
	{
		_memory = memory;
		_states = states;
	}

	public IActiveContent<T> Get(RenderStateContentInput<T> parameter)
	{
		var (previous, key) = parameter;
		var content = new RenderStateContent<T>(previous, _memory, key);
		var state   = new ComponentRenderState(key, _states);
		return new RenderStateAwareActiveContent<T>(state, content);
	}
}