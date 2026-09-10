using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime.Invocation;

/// <summary>
/// Attribution: https://github.com/i3arnon/AsyncUtilities
/// </summary>
public sealed class Locks<T> : ISelect<T, object> where T : notnull
{
	public static Locks<T> Default { get; } = new();

	Locks() : this(MaximumParallelismSafe.Default) {}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static int SmearHashCode(int hashCode)
	{
		hashCode ^= (hashCode >> 20) ^ (hashCode >> 12);
		return hashCode ^ (hashCode >> 7) ^ (hashCode >> 4);
	}

	readonly IEqualityComparer<T> _comparer;
	readonly int                     _mask;
	readonly Array<object>            _stripes;

	public Locks(int stripes) : this(LockItem.Default.Get(stripes), EqualityComparer<T>.Default) {}

	public Locks((Array<object> Items, int Mask) item, IEqualityComparer<T> comparer)
		: this(item.Mask, item.Items, comparer) {}

	public Locks(int mask, Array<object> stripes, IEqualityComparer<T> comparer)
	{
		_mask     = mask;
		_stripes  = stripes;
		_comparer = comparer;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	int GetStripe(T key) => SmearHashCode(_comparer.GetHashCode(key) & int.MaxValue) & _mask;

	public object Get(T parameter) => _stripes[GetStripe(parameter)];
}