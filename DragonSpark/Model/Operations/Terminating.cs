using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Terminating<TIn, TOut> : IOperation<TIn>
{
	readonly Await<TIn, TOut> _await;

	public Terminating(ISelecting<TIn, TOut> operation) : this(operation.Await) {}

	public Terminating(Await<TIn, TOut> await) => _await = @await;

	public async ValueTask Get(TIn parameter)
	{
		await _await(parameter);
	}
}

public class Terminating<T> : IOperation
{
	readonly AwaitOf<T> _await;

	public Terminating(IResulting<T> operation) : this(operation.Await) {}

	public Terminating(AwaitOf<T> await) => _await = @await;

	public async ValueTask Get()
	{
		await _await();
	}
}