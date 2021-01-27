using System.Linq;
using System.Linq.Dynamic.Core;

namespace DragonSpark.Presentation.Components.Content
{
	sealed class DefaultQueryAlteration<T> : IQueryAlteration<T>
	{
		public static DefaultQueryAlteration<T> Default { get; } = new DefaultQueryAlteration<T>();

		DefaultQueryAlteration() : this(OrderQueryAlteration<T>.Default) {}

		readonly IQueryAlteration<T> _previous;

		public DefaultQueryAlteration(IQueryAlteration<T> previous) => _previous = previous;

		public IQueryable<T> Get(QueryParameter<T> parameter)
		{
			var (_, load) = parameter;
			var previous = _previous.Get(parameter);
			var result   = !string.IsNullOrEmpty(load.Filter) ? previous.Where(load.Filter) : previous;
			return result;
		}
	}
}