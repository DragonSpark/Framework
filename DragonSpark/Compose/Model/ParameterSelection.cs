using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	sealed class ParameterSelection<TIn, TOut> : ISelect<(TIn Parameter, ValueTask Task), TOut>
	{
		readonly Func<TIn, TOut> _select;

		public ParameterSelection(Func<TIn, TOut> select) => _select = select;

		public TOut Get((TIn Parameter, ValueTask Task) parameter) => _select(parameter.Parameter);
	}
}