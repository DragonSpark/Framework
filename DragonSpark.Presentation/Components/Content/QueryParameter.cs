using Radzen;
using System.Linq;

namespace DragonSpark.Presentation.Components.Content
{
	public readonly struct QueryParameter<T>
	{
		public QueryParameter(IQueryable<T> query, LoadDataArgs parameter)
		{
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public LoadDataArgs Parameter { get; }

		public void Deconstruct(out IQueryable<T> query, out LoadDataArgs parameter)
		{
			query     = Query;
			parameter = Parameter;
		}
	}
}