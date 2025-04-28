using DragonSpark.Compose;
using Syncfusion.Blazor.Data;
using System.Threading.Tasks;

namespace DragonSpark.SyncfusionRendering.Queries;

sealed class FilterField<T> : IQuery<T>
{
	readonly string _field;

	public FilterField(string field) => _field = field;

	public ValueTask<Parameter<T>> Get(Parameter<T> parameter)
	{
		var where = parameter.Request.Where;
		if (where.Count > 0)
		{
			var source = where[0];
			where.Add(new WhereFilter
			{
				value        = source.value, Condition = source.Condition, Field = _field, Operator = "contains",
				IgnoreAccent = source.IgnoreAccent, IgnoreCase = source.IgnoreCase, IsComplex = source.IsComplex
			});
		}

		return parameter.ToOperation();
	}
}