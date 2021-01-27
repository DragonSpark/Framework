using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class OrderQueryAlteration<T> : IQueryAlteration<T>
	{
		public static OrderQueryAlteration<T> Default { get; } = new OrderQueryAlteration<T>();

		OrderQueryAlteration() {}

		public IQueryable<T> Get(QueryParameter<T> parameter)
		{
			var (query, load) = parameter;
			var result = !string.IsNullOrEmpty(load.OrderBy) ? query.OrderBy(load.OrderBy) : query;
			return result;
		}
	}
}