using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;

namespace DragonSpark.Compose.Model.Sequences;

sealed class GroupMapAdapter<T, TKey> : ISelect<Array<T>, IArrayMap<TKey, T>>
{
	readonly IReduce<T, IArrayMap<TKey, T>> _reduce;

	public GroupMapAdapter(IReduce<T, IArrayMap<TKey, T>> reduce) => _reduce = reduce;

	public IArrayMap<TKey, T> Get(Array<T> parameter) => _reduce.Get(parameter);
}