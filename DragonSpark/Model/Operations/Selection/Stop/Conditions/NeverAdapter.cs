using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Model.Operations.Selection.Stop.Conditions;

public class NeverAdapter<T> : IDepending<T>
{
	readonly IStopAware<T> _previous;

	public NeverAdapter(IStopAware<T> previous) => _previous = previous;

	public async ValueTask<bool> Get(Stop<T> parameter)
	{
		await _previous.Off(parameter);
		return false;
	}
}

public sealed class NeverAdapter : IDepending
{
	readonly IStopAware _previous;

	public NeverAdapter(IStopAware previous) => _previous = previous;

	public async ValueTask<bool> Get(Stop<None> parameter)
	{
		await _previous.Off(parameter);
		return false;
	}
}