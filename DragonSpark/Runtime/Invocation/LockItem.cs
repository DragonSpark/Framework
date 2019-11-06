using System;
using System.Runtime.CompilerServices;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Runtime.Invocation
{
	/// <summary>
	/// Attribution: https://github.com/i3arnon/AsyncUtilities
	/// </summary>
	// ReSharper disable once PossibleInfiniteInheritance
	sealed class LockItem<T> : ISelect<int, (Array<T> Items, int Mask)>
	{
		public static LockItem<T> Default { get; } = new LockItem<T>();

		LockItem() : this(Repeat<T>.Default.Get) {}

		readonly Func<uint, Array<T>> _create;

		public LockItem(Func<uint, Array<T>> create) => _create = create;

		public (Array<T> Items, int Mask) Get(int parameter)
		{
			var mask   = GetStripeMask(parameter);
			var result = (_create((uint)mask + 1), mask);
			return result;
		}

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
	}
}