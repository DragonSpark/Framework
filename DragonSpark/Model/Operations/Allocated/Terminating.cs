using DragonSpark.Compose;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Allocated;

public class Terminating<TIn, TOut> : IOperation<TIn>
{
	readonly Func<TIn, Task<TOut>> _await;

	public Terminating(Func<TIn, Task<TOut>> await) => _await = @await;

	public async ValueTask Get(TIn parameter)
	{
		await _await(parameter).ConfigureAwait(false);
	}
}

public class Terminating<T> : IOperation
{
	readonly AwaitOf<T> _await;

	protected Terminating(IAllocatedResult<T> operation) : this(new AwaitOf<T>(operation.Await)) {}

	protected Terminating(Func<Task<T>> operation) : this(operation.Start().Out()) {}

	protected Terminating(AwaitOf<T> await) => _await = @await;

	public async ValueTask Get()
	{
		await _await();
	}
}

public delegate ConfiguredTaskAwaitable<T> AwaitOf<T>();