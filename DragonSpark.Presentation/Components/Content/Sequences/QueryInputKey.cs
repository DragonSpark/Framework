using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class QueryInputKey : IFormatter<PageInput>
{
	readonly string                 _key;
	readonly IFormatter<PageInput> _previous;

	public QueryInputKey(string key) : this(key, QueryInputFormatter.Default) {}

	public QueryInputKey(string key, IFormatter<PageInput> previous)
	{
		_key      = key;
		_previous = previous;
	}

	public string Get(PageInput parameter)
	{
		var result = $"{_key}+{_previous.Get(parameter)}";
		return result;
	}
}