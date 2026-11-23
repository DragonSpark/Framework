using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Model.Operations.Selection.Stop.Conditions;

public sealed class AlwaysAdapter : IDepending
{
	readonly IStopAware _previous;

	public AlwaysAdapter(IStopAware previous) => _previous = previous;

	public async ValueTask<bool> Get(Stop<None> parameter)
	{
		await _previous.Off(parameter);
		return true;
	}
}

public class AlwaysAdapter<T> : IDepending<T>
{
	readonly IStopAware<T> _previous;

	public AlwaysAdapter(IStopAware<T> previous) => _previous = previous;

	public async ValueTask<bool> Get(Stop<T> parameter)
	{
		await _previous.Off(parameter);
		return true;
	}
}