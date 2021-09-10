using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content
{
	class Class1 {}

	public sealed class RadzenCallback<T> : IAllocated<LoadDataArgs>
	{
		readonly IEvaluate<T> _evaluate;
		readonly bool         _includeCount;

		public RadzenCallback(IEvaluate<T> evaluate, bool includeCount = true)
		{
			_evaluate     = evaluate;
			_includeCount = includeCount;
		}

		public ulong Count { get; private set; }

		public IEnumerable<T> Current { get; private set; } = default!;

		public async Task Get(LoadDataArgs parameter)
		{
			var input = new QueryInput
			{
				Filter  = parameter.Filter,
				OrderBy = parameter.OrderBy,
				Partition = parameter.Skip.HasValue || parameter.Top.HasValue
					            ? new Partition(parameter.Skip, parameter.Top)
					            : null,
				IncludeTotalCount = _includeCount,
			};

			var evaluate = await _evaluate.Await(input);
			Current = evaluate;
			Count   = evaluate.Total ?? evaluate.Count.Grade();
		}
	}
}