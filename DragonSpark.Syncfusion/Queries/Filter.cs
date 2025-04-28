using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class Filter<T> : IQuery<T>
{
	public static Filter<T> Default { get; } = new();

	Filter() : this(PerformSelect<T>.Default) {}

	readonly ISelect<PerformSelectInput<T>, IQueryable<T>> _select;

	public Filter(ISelect<PerformSelectInput<T>, IQueryable<T>> select) => _select = select;

	public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
	{
		var (request, query, _) = parameter;
		var data = request.Select.Count > 0
			           ? parameter with { Query = _select.Get(new PerformSelectInput<T>(query, request.Select)) }
			           : parameter;
		var result = data.ToOperation();
		return result;
	}
}