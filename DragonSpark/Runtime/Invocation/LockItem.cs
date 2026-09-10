using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System.Runtime.CompilerServices;

namespace DragonSpark.Runtime.Invocation;

/// <summary>
/// Attribution: https://github.com/i3arnon/AsyncUtilities
/// </summary>
sealed class LockItem : ISelect<int, (Array<object> Items, int Mask)>
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

	LockItem() : this(new Repeat<object>(() => new()).Get) {}

	readonly Func<uint, Array<object>> _create;

	public LockItem(Func<uint, Array<object>> create) => _create = create;

	public (Array<object> Items, int Mask) Get(int parameter)
	{
		var mask   = GetStripeMask(parameter);
		var result = (_create((uint)mask + 1), mask);
		return result;
	}
}