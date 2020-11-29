﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable All

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	struct HashCodeCombiner
	{
		private long _combinedHash64;

		public int CombinedHash
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get { return _combinedHash64.GetHashCode(); }
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private HashCodeCombiner(long seed)
		{
			_combinedHash64 = seed;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(IEnumerable e)
		{
			if (e == null)
			{
				Add(0);
			}
			else
			{
				var count = 0;
				foreach (var o in e)
				{
					Add(o);
					count++;
				}

				Add(count);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator int(HashCodeCombiner self)
		{
			return self.CombinedHash;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(int i)
		{
			_combinedHash64 = ((_combinedHash64 << 5) + _combinedHash64) ^ i;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(string s)
		{
			var hashCode = (s != null) ? s.GetHashCode() : 0;
			Add(hashCode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(object? o)
		{
			var hashCode = (o != null) ? o.GetHashCode() : 0;
			Add(hashCode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add<TValue>(TValue value, IEqualityComparer<TValue> comparer)
		{
			var hashCode = value is null ? 0 : comparer.GetHashCode(value);
			Add(hashCode);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static HashCodeCombiner Start()
		{
			return new HashCodeCombiner(0x1505L);
		}
	}
}