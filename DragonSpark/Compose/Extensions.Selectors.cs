using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose.Model.Results;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public partial class ExtensionMethods
{
    public static Task<TOut> Get<TIn, TOut>(this IAllocatingToken<TIn, TOut> @this, TIn parameter,
                                            CancellationToken stop)
        => @this.Get(new(parameter, stop));

	public static ResultComposer<T> Bind<T>(this Composer<Type, object> @this) => @this.Bind(A.Type<T>()).Cast<T>();

	public static Composer<T, string> Out<T>(this Composer<Type, string> @this)
		=> @this.Bind(A.Type<T>()).Accept<T>();

	public static ReferenceComposer<TIn, TOut> Stores<TIn, TOut>(this Composer<TIn, TOut> @this) where TIn : class
		=> new(@this.Get());

	public static TOut Get<TIn, TOut>(this Composer<TIn, TOut> @this, TIn parameter)
		=> @this.Get().Get(parameter);

	public static Composer<TIn, TOut> Or<TIn, TOut>(this Composer<TIn, TOut?> @this, ISelect<TIn, TOut> second)
		where TOut : class => @this.Or(second.Get);

	public static Composer<TIn, TOut> Or<TIn, TOut>(this Composer<TIn, TOut?> @this, Func<TIn, TOut> next)
		where TOut : class
		=> new Coalesce<TIn, TOut>(@this, next).Then();

	public static Composer<TIn, TOut?> OrMaybe<TIn, TOut>(this Composer<TIn, TOut?> @this,
	                                                      ISelect<TIn, TOut?> second)
		where TOut : class => @this.OrMaybe(second.Get);

	public static Composer<TIn, TOut?> OrMaybe<TIn, TOut>(this Composer<TIn, TOut?> @this,
	                                                      Func<TIn, TOut?> next)
		where TOut : class
		=> new Maybe<TIn, TOut>(@this, next).Then();
}