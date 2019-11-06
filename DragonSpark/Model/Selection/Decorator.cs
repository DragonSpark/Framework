using System;

namespace DragonSpark.Model.Selection
{
	public class Decorator<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<Decoration<TIn, TOut>, TOut> _decorator;
		readonly Func<TIn, TOut>                   _source;

		public Decorator(ISelect<Decoration<TIn, TOut>, TOut> decorator,
		                 ISelect<TIn, TOut> select)
			: this(decorator.Get, select.Get) {}

		public Decorator(Func<Decoration<TIn, TOut>, TOut> decorator)
			: this(decorator, _ => default) {}

		public Decorator(Func<Decoration<TIn, TOut>, TOut> decorator,
		                 Func<TIn, TOut> source)
		{
			_decorator = decorator;
			_source    = source;
		}

		public TOut Get(TIn parameter)
			=> _decorator(new Decoration<TIn, TOut>(parameter, _source(parameter)));
	}
}