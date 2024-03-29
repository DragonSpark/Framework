﻿using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory;

sealed class EnumerableLease<T> : ILease<EnumerableExtensions.ValueEnumerable<T>, T>
{
	public static EnumerableLease<T> Default { get; } = new();

	EnumerableLease() : this(ArrayPool<T>.Shared) {}

	readonly ArrayPool<T> _leases;

	public EnumerableLease(ArrayPool<T> leases) => _leases   = leases;

	public Leasing<T> Get(EnumerableExtensions.ValueEnumerable<T> parameter) => new(parameter.ToArray(_leases));
}