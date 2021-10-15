using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime.Invocation;

sealed class Locks<T> : Locks<T, object> where T : notnull
{
	public static Locks<T> Default { get; } = new Locks<T>();

	Locks() : base(System.Environment.ProcessorCount) {}
}

/// <summary>
/// Attribution: https://github.com/i3arnon/AsyncUtilities
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TLock"></typeparam>
public class Locks<TKey, TLock> : ISelect<TKey, TLock> where TKey : notnull
{
	// ReSharper disable once ComplexConditionExpression
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static int SmearHashCode(int hashCode)
	{
		hashCode ^= (hashCode >> 20) ^ (hashCode >> 12);
		return hashCode ^ (hashCode >> 7) ^ (hashCode >> 4);
	}

	readonly IEqualityComparer<TKey> _comparer;
	readonly int                     _mask;
	readonly Array<TLock>            _stripes;

	public Locks(int stripes) : this(LockItem<TLock>.Default.Get(stripes), EqualityComparer<TKey>.Default) {}

	public Locks((Array<TLock> Items, int Mask) item, IEqualityComparer<TKey> comparer)
		: this(item.Mask, item.Items, comparer) {}

	public Locks(int mask, Array<TLock> stripes, IEqualityComparer<TKey> comparer)
	{
		_mask     = mask;
		_stripes  = stripes;
		_comparer = comparer;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	int GetStripe(TKey key) => SmearHashCode(_comparer.GetHashCode(key) & int.MaxValue) & _mask;

	public TLock Get(TKey parameter) => _stripes[GetStripe(parameter)];
}