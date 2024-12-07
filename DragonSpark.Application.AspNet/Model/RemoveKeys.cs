using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;
using NetFabric.Hyperlinq;
using System.Buffers;
using System.Collections.Concurrent;

namespace DragonSpark.Application.Model;

public class RemoveKeys : ICommand
{
	readonly IMemoryCache          _memory;
	readonly ConcurrentBag<string> _keys;

	protected RemoveKeys(IMemoryCache memory, ConcurrentBag<string> keys)
	{
		_memory = memory;
		_keys   = keys;
	}

	public void Execute(None parameter)
	{
		using var lease = _keys.AsValueEnumerable().ToArray(ArrayPool<string>.Shared);
		foreach (var key in lease)
		{
			_memory.Remove(key);
		}
		_keys.Clear();
	}
}