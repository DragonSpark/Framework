using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Extents.Selections
{
	sealed class ParameterAdapter<TIn, TOut> : ISelect<(TIn Parameter, ValueTask Task), TOut>
	{
		readonly Parameter<TIn, TOut> _parameter;

		public ParameterAdapter(Parameter<TIn, TOut> parameter) => _parameter = parameter;

		public TOut Get((TIn Parameter, ValueTask Task) parameter) => _parameter(parameter);
	}
}