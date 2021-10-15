using DragonSpark.Model.Sequences.Memory;

namespace DragonSpark.Compose.Model.Memory;

sealed class OfType<T, TTo> : ILease<Leasing<T>, TTo>
{
	public static OfType<T, TTo> Default { get; } = new OfType<T, TTo>();

	OfType() : this(NewLeasing<TTo>.Default) {}

	readonly INewLeasing<TTo> _new;

	public OfType(INewLeasing<TTo> @new) => _new = @new;

	public Leasing<TTo> Get(Leasing<T> parameter)
	{
		var lease       = _new.Get(parameter.Length);
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