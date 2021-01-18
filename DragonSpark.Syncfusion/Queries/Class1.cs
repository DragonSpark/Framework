using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Syncfusion.Queries
{
	public readonly struct Parameter<T>
	{
		public Parameter(DataManagerRequest request, IQueryable<T> query, uint? count = null)
		{
			Request = request;
			Query   = query;
			Count   = count;
		}

		public DataManagerRequest Request { get; }

		public IQueryable<T> Query { get; }
		public uint? Count { get; }

		public void Deconstruct(out DataManagerRequest request, out IQueryable<T> query, out uint? count)
		{
			request = Request;
			query   = Query;
			count   = Count;
		}
	}

	public interface IQuery<T> : IAltering<Parameter<T>> {}

	sealed class Query<T> : IQuery<T>
	{
		public static Query<T> Default { get; } = new Query<T>();

		Query() : this(DefaultQuery<T>.Default) {}

		readonly Array<IQuery<T>> _alterations;

		public Query(Array<IQuery<T>> alterations) => _alterations = alterations;

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter) => _alterations.Open().Alter(parameter);
	}

	sealed class DefaultQuery<T> : ArrayInstance<IQuery<T>>
	{
		public static DefaultQuery<T> Default { get; } = new DefaultQuery<T>();

		DefaultQuery() : base(Search<T>.Default, Sort<T>.Default, Where<T>.Default, Count<T>.Default, Skip<T>.Default,
		                      Take<T>.Default) {}
	}

	sealed class Count<T> : IQuery<T>
	{
		public static Count<T> Default { get; } = new Count<T>();

		Count() {}

		public async ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			return count is null && request.RequiresCounts ? new(request, query, (uint)await query.CountAsync()) : parameter;
		}
	}

	sealed class Search<T> : IQuery<T>
	{
		public static Search<T> Default { get; } = new Search<T>();

		Search() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Search?.Count > 0
				           ? new(request, DataOperations.PerformSearching(query, request.Search), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}

	sealed class Sort<T> : IQuery<T>
	{
		public static Sort<T> Default { get; } = new Sort<T>();

		Sort() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Sorted?.Count > 0
				           ? new(request, DataOperations.PerformSorting(query, request.Sorted), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}

	sealed class Where<T> : IQuery<T>
	{
		public static Where<T> Default { get; } = new Where<T>();

		Where() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, _) = parameter;
			var data = request.Where?.Count > 0
				           ? new(request,
				                 DataOperations.PerformFiltering(query, request.Where, request.Where[0].Operator))
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}

	sealed class Skip<T> : IQuery<T>
	{
		public static Skip<T> Default { get; } = new Skip<T>();

		Skip() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Skip > 0
				           ? new(request, DataOperations.PerformSkip(query, request.Skip), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}

	sealed class Take<T> : IQuery<T>
	{
		public static Take<T> Default { get; } = new Take<T>();

		Take() {}

		public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
		{
			var (request, query, count) = parameter;
			var data = request.Take > 0
				           ? new(request, DataOperations.PerformTake(query, request.Take), count)
				           : parameter;
			var result = data.ToOperation();
			return result;
		}
	}
}