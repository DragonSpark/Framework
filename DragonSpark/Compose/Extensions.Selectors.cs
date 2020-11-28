using DragonSpark.Compose.Model;
using DragonSpark.Model.Operations;
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

		public static OperationResultSelector<TIn, TOut> Or<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
		                                                               ISelecting<TIn, TOut> second)
			where TOut : class => @this.Or(second.Await);

		public static OperationResultSelector<TIn, TOut> Or<TIn, TOut>(this OperationResultSelector<TIn, TOut?> @this,
		                                                               Await<TIn, TOut> next)
			where TOut : class
			=> new Coalesce<TIn, TOut>(@this, next).Then();
	}
}