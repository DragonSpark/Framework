using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Model.Sequences.Memory;

public sealed class NewLeasing<T> : INewLeasing<T>
{
	public static NewLeasing<T> Default { get; } = new NewLeasing<T>();

	NewLeasing() : this(ArrayPool<T>.Shared) {}

	readonly ArrayPool<T> _pool;

	public NewLeasing(ArrayPool<T> pool) => _pool = pool;

	public Leasing<T> Get(uint parameter) => new(_pool.Lease((int)parameter), parameter);
}