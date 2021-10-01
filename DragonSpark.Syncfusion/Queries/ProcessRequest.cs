using DragonSpark.Application.Entities.Queries.Runtime.Shape;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	sealed class ProcessRequest<T> : IDataRequest
	{
		readonly Await<DataManagerRequest, Current<T>> _current;

		public ProcessRequest(IPaging<T> paging) : this(SelectQueryInput.Default.Then().Select(paging).Then()) {}

		public ProcessRequest(Await<DataManagerRequest, Current<T>> current) => _current = current;

		public async ValueTask<object> Get(DataManagerRequest parameter)
		{
			var evaluate = await _current(parameter);
			var result = evaluate.Total.HasValue
				             ? new DataResult { Result = evaluate, Count = evaluate.Total.Value.Degrade() }
				             : (object)evaluate;
			return result;
		}
	}
}