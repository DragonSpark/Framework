using DragonSpark.Application.Entities.Queries.Runtime;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Presentation.Components.Content.Sequences;

sealed class AnyFormatter<T> : Formatter<IQueries<T>>
{
	public AnyFormatter(string key) : base(key.Accept) {}
}