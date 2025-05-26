using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Stop;

sealed class Terminate<TIn, TTo> : IStopAware<TIn>
{
	readonly ISelect<Stop<TIn>, ValueTask<TTo>> _previous;
	readonly Func<Stop<TTo>, ValueTask>         _command;

	public Terminate(ISelect<Stop<TIn>, ValueTask<TTo>> previous, Func<Stop<TTo>, ValueTask> command)
	{
		_previous = previous;
		_command  = command;
	}

	public async ValueTask Get(Stop<TIn> parameter)
	{
		var previous = await _previous.Off(parameter);
		await _command(new(previous, parameter)).Off();
	}
}