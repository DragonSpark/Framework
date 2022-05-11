using DragonSpark.Application.Compose.Runtime;
using DragonSpark.Application.Model.Sequences;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Compiled.Evaluation;

sealed class ToSelectedCollection<TList, T> : IEvaluate<T, TList> where TList : SelectedCollection<T>, new()
{
	public static ToSelectedCollection<TList, T> Default { get; } = new();

	ToSelectedCollection() : this(ToLease<T>.Default) {}

	readonly IEvaluate<T, Leasing<T>> _leasing;
	readonly CopyList<TList, T>       _copy;

	public ToSelectedCollection(IEvaluate<T, Leasing<T>> leasing) : this(leasing, CopyList<TList, T>.Default) {}

	public ToSelectedCollection(IEvaluate<T, Leasing<T>> leasing, CopyList<TList, T> copy)
	{
		_leasing = leasing;
		_copy    = copy;
	}

	public async ValueTask<TList> Get(IAsyncEnumerable<T> parameter)
	{
		using var leasing = await _leasing.Await(parameter);
		var       result  = _copy.Get(new(leasing.AsMemory(), new TList()));
		return result;
	}
}