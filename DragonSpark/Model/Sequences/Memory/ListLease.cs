using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DragonSpark.Model.Sequences.Memory;

sealed class ListLease<T> : ILease<List<T>, T>
{
	public static ListLease<T> Default { get; } = new ListLease<T>();

	ListLease() : this(NewLeasing<T>.Default) {}

	readonly INewLeasing<T> _new;

	public ListLease(INewLeasing<T> @new) => _new = @new;

	public Leasing<T> Get(List<T> parameter)
	{
		var result = _new.Get((uint)parameter.Count);
		CollectionsMarshal.AsSpan(parameter).CopyTo(result.AsSpan());
		return result;
	}
}