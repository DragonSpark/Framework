using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Load<TIn, TOut> : ISelecting<EntryKey<TIn>, TOut>
{
	readonly IDistributedCache              _memory;
	readonly Await<TIn, TOut>               _source;
	readonly Func<TOut, DistributedContent> _content;

	public Load(IDistributedCache memory, Await<TIn, TOut> source,
	            ICommand<DistributedCacheEntryOptions> configure)
		: this(memory, source, new Content<TOut>(configure).Get) {}

	public Load(IDistributedCache memory, Await<TIn, TOut> source, Func<TOut, DistributedContent> content)
	{
		_memory  = memory;
		_source  = source;
		_content = content;
	}

	public async ValueTask<TOut> Get(EntryKey<TIn> parameter)
	{
		var (@in, key) = parameter;
		var result = await _source(@in);
		var (content, options) = _content(result);
		await _memory.SetStringAsync(key, content, options).Off();
		return result;
	}
}

public readonly record struct DistributedContent(string Content, DistributedCacheEntryOptions Options);