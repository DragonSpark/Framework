using DragonSpark.Model.Commands;
using System.Buffers;

namespace DragonSpark.Model.Sequences;

sealed class Return<T> : ICommand<T[]>
{
	public static Return<T> Default { get; } = new Return<T>();

	Return() : this(ArrayPool<T>.Shared) {}

	readonly ArrayPool<T> _pool;

	public Return(ArrayPool<T> pool) => _pool = pool;

	public void Execute(T[] parameter)
	{
		_pool.Return(parameter);
	}
}