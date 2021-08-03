using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class Concatenate<T> : ILease<(Memory<T> First, Memory<T> Second), T>
	{
		public static Concatenate<T> Default { get; } = new Concatenate<T>();

		Concatenate() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public Concatenate(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get((Memory<T> First, Memory<T> Second) parameter)
		{
			var (first, second) = parameter;

			var result      = _leases.Get((uint)(first.Length + second.Length));
			var destination = result.AsMemory();

			first.CopyTo(destination[..first.Length]);
			second.CopyTo(destination[first.Length..]);
			return result;
		}
	}
}