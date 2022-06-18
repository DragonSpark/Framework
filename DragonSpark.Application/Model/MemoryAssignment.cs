using DragonSpark.Application.Compose.Store;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Model;

public class MemoryAssignment<T> : IAssign<string, T?>, ISelect<string, T?>
{
	readonly IMemoryCache               _subject;
	readonly IConfiguredMemoryResult<T> _configured;

	public MemoryAssignment(IMemoryCache subject, ICommand<ICacheEntry> configure)
		: this(subject, new ConfiguredMemoryResult<T>(subject, configure.Execute)) {}

	public MemoryAssignment(IMemoryCache subject, IConfiguredMemoryResult<T> configured)
	{
		_subject    = subject;
		_configured = configured;
	}

	public void Execute(Pair<string, T?> parameter)
	{
		var (key, value) = parameter;
		_configured.Execute((key, value));
	}

	public bool Pop(string key, out T? result)
	{
		if (_subject.TryGetValue(key, out result))
		{
			Remove(key);
			return true;
		}

		return false;
	}

	public void Remove(string key)
	{
		_subject.Remove(key);
	}

	public T? Get(string parameter) => _subject.TryGetValue(parameter, out var stored) ? stored.To<T?>() : default;
}