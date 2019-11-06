using System;

namespace DragonSpark.Model.Selection.Conditions
{
	sealed class ParameterSelection<_, TFrom, TTo> : ISelect<IConditional<TTo, _>, IConditional<TFrom, _>>
	{
		readonly Func<TFrom, TTo> _select;

		public ParameterSelection(Func<TFrom, TTo> select) => _select = select;

		public IConditional<TFrom, _> Get(IConditional<TTo, _> parameter)
			=> new SelectedConditional<TFrom, TTo, _>(parameter, _select);
	}
}