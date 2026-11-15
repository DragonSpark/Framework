using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Hosting;

sealed class Always : IDepending
{
	readonly IStopAware _previous;

	public Always(IStopAware previous) => _previous = previous;

	public async ValueTask<bool> Get(Stop<None> parameter)
	{
		await _previous.Off(parameter);
		return true;
	}
}