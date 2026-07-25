using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;

namespace DragonSpark.Model.Operations.Selection.Conditions;

public class DependingOnAll<T> : IDepending<T>
{
	readonly Array<Await<T, bool>> _selections;

	protected DependingOnAll(params ISelect<T, ValueTask<bool>>[] selections)
		: this(selections.AsValueEnumerable().Select(x => new Await<T, bool>(x.Off)).ToArray()) {}

	protected DependingOnAll(params Await<T, bool>[] selections) => _selections = selections;

	public async ValueTask<bool> Get(T parameter)
	{
		for (var i = 0; i < _selections.Length; i++)
		{
			if (!await _selections[i](parameter))
			{
				return false;
			}
		}

		return true;
	}
}