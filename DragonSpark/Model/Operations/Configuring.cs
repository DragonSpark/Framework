using DragonSpark.Compose;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public class Configuring<TSource, TIntermediary, TResult> : ISelecting<TSource, TResult>
{
	readonly Await<TSource, TResult>                  _instance;
	readonly Await<(TSource, TResult), TIntermediary> _intermediary;
	readonly Await<TIntermediary>                     _configure;

	public Configuring(Func<(TSource, TResult), TIntermediary> intermediary, IOperation<TIntermediary> configure)
		: this(parameter => intermediary(parameter).ToOperation().ConfigureAwait(false), configure) {}

	public Configuring(Await<(TSource, TResult), TIntermediary> intermediary, IOperation<TIntermediary> configure)
		: this(intermediary, configure.Await) {}

	public Configuring(Await<(TSource, TResult), TIntermediary> intermediary,
	                   Await<TIntermediary> configure)
		: this(Start.A.Selection.Of.Type<TSource>().By.Instantiation<TResult>().Operation(), intermediary,
		       configure) {}

	public Configuring(Await<TSource, TResult> instance, Await<(TSource, TResult), TIntermediary> intermediary,
	                   Await<TIntermediary> configure)
	{
		_instance     = instance;
		_intermediary = intermediary;
		_configure    = configure;
	}

	public async ValueTask<TResult> Get(TSource parameter)
	{
		var result       = await _instance(parameter);
		var intermediary = await _intermediary((parameter, result));
		await _configure(intermediary);
		return result;
	}
}

public class Configuring<TSource, TResult> : ISelecting<TSource, TResult>
{
	readonly Await<TSource, TResult>     _select;
	readonly Operate<(TSource, TResult)> _configure;

	protected Configuring(Await<TSource, TResult> select, Operate<(TSource, TResult)> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public async ValueTask<TResult> Get(TSource parameter)
	{
		var result = await _select(parameter);
		await _configure((parameter, result));
		return result;
	}
}