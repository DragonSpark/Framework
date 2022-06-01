using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Model;

public class MemoryVariable<T> : IMutable<T?>
{
	readonly IMemoryCache                 _memory;
	readonly string                       _key;
	readonly IConfiguredMemoryResult<T> _configured;

	protected MemoryVariable(IMemoryCache memory, string key, ICommand<ICacheEntry> configure)
		: this(memory, key, new ConfiguredMemoryResult<T>(memory, configure.Execute)) {}

	protected MemoryVariable(IMemoryCache memory, string key, IConfiguredMemoryResult<T> configured)
	{
		_memory          = memory;
		_key             = key;
		_configured = configured;
	}

	public bool Pop(out T? result)
	{
		if (_memory.TryGetValue(_key, out result))
		{
			Remove();
			return true;
		}

		return false;
	}

	public void Remove()
	{
		_memory.Remove(_key);
	}

	public T? Get() => _memory.TryGetValue(_key, out var stored) ? stored.To<T?>() : default;

	public void Execute(T? parameter)
	{
		if (parameter is not null)
		{
			_configured.Get((parameter, _key));
		}
		else
		{
			Remove();
		}
	}
}