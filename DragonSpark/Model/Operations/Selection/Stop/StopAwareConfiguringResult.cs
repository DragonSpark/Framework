using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAwareConfiguringResult<TSource, TResult> : IStopAware<TSource, TResult>
{
	readonly Await<Stop<TSource>, TResult> _select;
	readonly Await<Stop<TResult>>          _configure;

	public StopAwareConfiguringResult(IStopAware<TResult> operation)
		: this(Start.A.Selection.Of.Type<Stop<TSource>>().By.Instantiation<TResult>().Operation().Out(), operation) {}

	public StopAwareConfiguringResult(IStopAware<TSource, TResult> select, IStopAware<TResult> operation)
		: this(select.Off, operation.Off) {}

	public StopAwareConfiguringResult(Await<Stop<TSource>, TResult> select, Await<Stop<TResult>> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<TResult> Get(Stop<TSource> parameter)
	{
		var result = await _select(parameter);
		await _configure(new(result, parameter));
		return result;
	}
}