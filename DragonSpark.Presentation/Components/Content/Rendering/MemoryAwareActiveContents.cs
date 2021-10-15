using DragonSpark.Composition;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Rendering;

sealed class MemoryAwareActiveContents<T> : IActiveContents<T>
{
	readonly IActiveContents<T> _previous;
	readonly IMemoryCache       _memory;
	readonly IRenderContentKey  _key;

	public MemoryAwareActiveContents(IMemoryCache memory, IRenderContentKey key)
		: this(ActiveContents<T>.Default, memory, key) {}

	[Candidate(false)]
	public MemoryAwareActiveContents(IActiveContents<T> previous, IMemoryCache memory, IRenderContentKey key)
	{
		_previous = previous;
		_memory   = memory;
		_key      = key;
	}

	public IActiveContent<T> Get(Func<ValueTask<T>> parameter)
	{
		var previous = _previous.Get(parameter);
		var key      = _key.Get(parameter);
		return new MemoryAwareActiveContent<T>(previous, _memory, key);
	}
}