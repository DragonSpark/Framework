using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace DragonSpark.Application.Model;

public class RemoveKeys : ICommand
{
	readonly IMemoryCache                _memory;
	readonly IReadOnlyCollection<string> _keys;

	protected RemoveKeys(IMemoryCache memory, IReadOnlyCollection<string> keys)
	{
		_memory = memory;
		_keys   = keys;
	}

	public void Execute(None parameter)
	{
		foreach (var key in _keys)
		{
			_memory.Remove(key);
		}
	}
}