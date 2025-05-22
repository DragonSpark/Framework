using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Distributed;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

sealed class Content<T> : ISelect<T, DistributedContent>
{
	readonly ICommand<DistributedCacheEntryOptions> _configure;
	readonly ISelect<T?, string>                    _content;

	public Content(ICommand<DistributedCacheEntryOptions> configure) : this(configure, DefaultSerialize<T?>.Default) {}

	public Content(ICommand<DistributedCacheEntryOptions> configure, ISelect<T?, string> content)
	{
		_configure = configure;
		_content   = content;
	}

	public DistributedContent Get(T parameter)
	{
		var content = _content.Get(parameter);
		var options = _configure.Parameter(new DistributedCacheEntryOptions());
		return new(content, options);
	}
}