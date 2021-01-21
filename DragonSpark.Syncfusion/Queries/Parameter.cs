using Syncfusion.Blazor;
using System.Linq;

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
}