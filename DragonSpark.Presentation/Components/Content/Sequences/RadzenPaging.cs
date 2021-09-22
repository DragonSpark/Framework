using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content.Sequences
{
	sealed class RadzenPaging<T> : IRadzenPaging<T>
	{
		readonly IPaging<T> _paging;
		readonly bool       _includeCount;

		public RadzenPaging(IPaging<T> paging, bool includeCount = true)
		{
			_paging       = paging;
			_includeCount = includeCount;
		}

		public ulong Count { get; private set; }

		public IEnumerable<T>? Current { get; private set; }

		public async Task Get(LoadDataArgs parameter)
		{
			var input = new QueryInput
			{
				Filter  = parameter.Filter,
				OrderBy = parameter.OrderBy,
				Partition = parameter.Skip.HasValue || parameter.Top.HasValue
					            ? new (parameter.Skip, parameter.Top)
					            : null,
				IncludeTotalCount = _includeCount,
			};

			await Task.Delay(1500);

			var evaluate = await _paging.Await(input);
			Current = evaluate;
			Count   = evaluate.Total ?? evaluate.Count.Grade();
		}
	}
}