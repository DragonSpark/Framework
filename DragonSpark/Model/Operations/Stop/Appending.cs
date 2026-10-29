using DragonSpark.Compose;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Stop;

public class Appending<T> : IStopAware<T>
{
	readonly ISelect<Stop<T>, ValueTask> _first, _second;

	public Appending(ISelect<Stop<T>, ValueTask> first, ISelect<Stop<T>, ValueTask> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		await _first.Off(parameter);
		await _second.Off(parameter);
	}
}