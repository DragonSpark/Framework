using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class QueryInputKey : IFormatter<QueryInput>
{
	readonly string                 _key;
	readonly IFormatter<QueryInput> _previous;

	public QueryInputKey(string key) : this(key, QueryInputFormatter.Default) {}

	public QueryInputKey(string key, IFormatter<QueryInput> previous)
	{
		_key      = key;
		_previous = previous;
	}

	public string Get(QueryInput parameter)
	{
		var result = $"{_key}+{_previous.Get(parameter)}";
		return result;
	}
}