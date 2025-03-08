using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Terminating<T> : IOperation
{
	readonly AwaitOf<T> _await;

	protected Terminating(IAllocatedResult<T> operation) : this(operation.Off) {}

	protected Terminating(Func<Task<T>> operation) : this(operation.Start().Out()) {}

	protected Terminating(AwaitOf<T> await) => _await = await;

	public async ValueTask Get()
	{
		await _await();
	}
}