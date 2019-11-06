using DragonSpark.Model.Selection;

namespace DragonSpark.Aspects
{
	sealed class Cast<TI, TO, TIn, TOut> : IAspect<TIn, TOut> where TIn : TI where TOut : TO
	{
		readonly IAspect<TI, TO> _aspect;

		public Cast(IAspect<TI, TO> aspect) => _aspect = aspect;

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new Container(_aspect.Get(new Select(parameter)));

		internal sealed class Container : ISelect<TIn, TOut>
		{
			readonly ISelect<TI, TO> _select;

			public Container(ISelect<TI, TO> select) => _select = select;

			public TOut Get(TIn parameter) => (TOut)_select.Get(parameter);
		}

		sealed class Select : ISelect<TI, TO>
		{
			readonly ISelect<TIn, TOut> _select;

			public Select(ISelect<TIn, TOut> select) => _select = select;

			public TO Get(TI parameter) => _select.Get((TIn)parameter);
		}
	}
}