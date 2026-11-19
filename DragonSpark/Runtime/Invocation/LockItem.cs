using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime.Invocation;

/// <summary>
/// Attribution: https://github.com/i3arnon/AsyncUtilities
/// </summary>
sealed class LockItem : ISelect<int, (Array<Lock> Locks, int Mask)>
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static int GetStripeMask(int stripes)
	{
		stripes |= stripes >> 1;
		stripes |= stripes >> 2;
		stripes |= stripes >> 4;
		stripes |= stripes >> 8;
		stripes |= stripes >> 16;
		return stripes;
	}

	public static LockItem Default { get; } = new();

	LockItem() : this(new Repeat<Lock>(() => new()).Get) {}

	readonly Func<uint, Array<Lock>> _create;

	public LockItem(Func<uint, Array<Lock>> create) => _create = create;

	public (Array<Lock> Locks, int Mask) Get(int parameter)
	{
		var mask   = GetStripeMask(parameter);
		var result = (_create((uint)mask + 1), mask);
		return result;
	}
}