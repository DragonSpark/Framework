namespace DragonSpark.Model.Sequences.Memory
{
	sealed class CombineLeases<T> : ILease<(Lease<T> First, Lease<T> Second), T>
	{
		public static CombineLeases<T> Default { get; } = new CombineLeases<T>();

		CombineLeases() : this(Leases<T>.Default) {}

		readonly ILeases<T> _leases;

		public CombineLeases(ILeases<T> leases) => _leases = leases;

		public Lease<T> Get((Lease<T> First, Lease<T> Second) parameter)
		{
			var (first, second) = parameter;

			var total = first.Length + second.Length;

			if (first.ActualLength <= total)
			{
				second.AsSpan().CopyTo(first.AsSpan()[(int)first.Length..]);
				second.Dispose();
				return first;
			}

			var result      = _leases.Get(total);
			var destination = result.AsSpan();

			first.AsSpan().CopyTo(destination);
			second.AsSpan().CopyTo(destination[(int)first.Length..]);

			first.Dispose();
			second.Dispose();

			return result;
		}
	}
}