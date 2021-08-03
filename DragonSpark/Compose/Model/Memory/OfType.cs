using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Compose.Model.Memory
{
	sealed class OfType<T, TTo> : ILease<Lease<T>, TTo>
	{
		public static OfType<T, TTo> Default { get; } = new OfType<T, TTo>();

		OfType() : this(Leases<TTo>.Default) {}

		readonly ILeases<TTo> _leases;

		public OfType(ILeases<TTo> leases) => _leases = leases;

		public Lease<TTo> Get(Lease<T> parameter)
		{
			var lease       = _leases.Get(parameter.Length);
			var span        = parameter.AsSpan();
			var destination = lease.AsSpan();
			var length      = 0;
			for (var i = 0; i < span.Length; i++)
			{
				if (span[i] is TTo to)
				{
					destination[length++] = to;
				}
			}

			return lease.Size(length);
		}
	}
}