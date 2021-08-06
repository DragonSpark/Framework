using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class DependingOnAny<T> : IDepending<T>
	{
		readonly Array<Await<T, bool>> _selections;

		public DependingOnAny(params ISelect<T, ValueTask<bool>>[] selections)
			: this(selections.AsValueEnumerable().Select(x => new Await<T, bool>(x.Await)).ToArray()) {}

		public DependingOnAny(params Await<T, bool>[] selections) => _selections = selections;

		public async ValueTask<bool> Get(T parameter)
		{
			var length = _selections.Length;
			for (var i = 0; i < length; i++)
			{
				if (await _selections[i](parameter))
				{
					return true;
				}
			}

			return false;
		}
	}

	public class DependingOnAll<T> : IDepending<T>
	{
		readonly Array<Await<T, bool>> _selections;

		public DependingOnAll(params ISelect<T, ValueTask<bool>>[] selections)
			: this(selections.AsValueEnumerable().Select(x => new Await<T, bool>(x.Await)).ToArray()) {}

		public DependingOnAll(params Await<T, bool>[] selections) => _selections = selections;

		public async ValueTask<bool> Get(T parameter)
		{
			var length = _selections.Length;
			for (var i = 0; i < length; i++)
			{
				if (!await _selections[i](parameter))
				{
					return false;
				}
			}

			return true;
		}
	}
}