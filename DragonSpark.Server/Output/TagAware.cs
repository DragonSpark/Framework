using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Server.Output;

public class TagAware<TIn, TOut> : IStopAware<TIn, TOut?>
{
	readonly IStopAware<TIn, TOut?>                    _previous;
	readonly IStopAware<TagDefinitionInput<TIn, TOut>> _apply;

	protected TagAware(IStopAware<TIn, TOut?> previous, IStopAware<TagDefinitionInput<TIn, TOut>> apply)
	{
		_previous = previous;
		_apply    = apply;
	}

	public async ValueTask<TOut?> Get(Stop<TIn> parameter)
	{
		var (subject, stop) = parameter;
		var result = await _previous.Off(parameter);
		await _apply.Off(new(new(subject, result), stop));
		return result;
	}
}