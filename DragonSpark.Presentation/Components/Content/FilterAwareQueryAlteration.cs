using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class FilterAwareQueryAlteration<T> : IQueryAlteration<T>
	{
		readonly IQueryAlteration<T> _previous;
		readonly string              _filter;

		public FilterAwareQueryAlteration(string filter) : this(OrderQueryAlteration<T>.Default, filter) {}

		public FilterAwareQueryAlteration(IQueryAlteration<T> previous, string filter)
		{
			_previous = previous;
			_filter   = filter;
		}

		public IQueryable<T> Get(QueryParameter<T> parameter)
		{
			var (_, load) = parameter;
			var previous = _previous.Get(parameter);
			var result   = !string.IsNullOrEmpty(load.Filter) ? previous.Where(_filter, load.Filter) : previous;
			return result;
		}
	}
}