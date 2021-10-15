using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class ConfiguringResult<TSource, TResult> : ISelecting<TSource, TResult>
{
	readonly Await<TSource, TResult> _select;
	readonly Await<TResult>          _configure;

	public ConfiguringResult(IOperation<TResult> operation)
		: this(Start.A.Selection.Of.Type<TSource>().By.Instantiation<TResult>().Operation().Out(), operation) {}

	public ConfiguringResult(ISelecting<TSource, TResult> select, IOperation<TResult> operation)
		: this(select.Await, operation.Await) {}

	public ConfiguringResult(Await<TSource, TResult> select, Await<TResult> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<TResult> Get(TSource parameter)
	{
		var result = await _select(parameter);
		await _configure(result);
		return result;
	}
}

public class ConfiguringResult<T> : IResulting<T>
{
	readonly AwaitOf<T> _select;
	readonly Await<T>   _configure;

	public ConfiguringResult(IResulting<T> select, IOperation<T> operation) : this(select.Await, operation.Await) {}

	public ConfiguringResult(AwaitOf<T> select, Await<T> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<T> Get()
	{
		var result = await _select();
		await _configure(result);
		return result;
	}
}