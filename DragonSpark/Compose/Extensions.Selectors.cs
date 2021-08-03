using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public partial class ExtensionMethods
	{
		public static ResultContext<T> Bind<T>(this Selector<Type, object> @this) => @this.Bind(A.Type<T>()).Cast<T>();

		public static Selector<T, string> Out<T>(this Selector<Type, string> @this)
			=> @this.Bind(A.Type<T>()).Accept<T>();

		public static ReferenceContext<TIn, TOut> Stores<TIn, TOut>(this Selector<TIn, TOut> @this) where TIn : class
			=> new ReferenceContext<TIn, TOut>(@this.Get());

		public static TOut Get<TIn, TOut>(this Selector<TIn, TOut> @this, TIn parameter)
			=> @this.Get().Get(parameter);

		public static Selector<TIn, TOut> Or<TIn, TOut>(this Selector<TIn, TOut?> @this, ISelect<TIn, TOut> second)
			where TOut : class => @this.Or(second.Get);

		public static Selector<TIn, TOut> Or<TIn, TOut>(this Selector<TIn, TOut?> @this, Func<TIn, TOut> next)
			where TOut : class
			=> new Coalesce<TIn, TOut>(@this, next).Then();

		public static Selector<TIn, TOut?> OrMaybe<TIn, TOut>(this Selector<TIn, TOut?> @this,
		                                                      ISelect<TIn, TOut?> second)
			where TOut : class => @this.OrMaybe(second.Get);

		public static Selector<TIn, TOut?> OrMaybe<TIn, TOut>(this Selector<TIn, TOut?> @this,
		                                                      Func<TIn, TOut?> next)
			where TOut : class
			=> new Maybe<TIn, TOut>(@this, next).Then();
	}
}