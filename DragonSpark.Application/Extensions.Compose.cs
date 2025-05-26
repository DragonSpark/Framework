using System;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Application.Model;
using DragonSpark.Application.Model.Selections;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DragonSpark.Application;

// ReSharper disable once MismatchedFileName
public static partial class Extensions
{
	/**/

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValueTask<T> Get<T>(this INumber<T> @this, Stop<ulong> parameter)
		=> @this.Get(new(parameter.Subject.Contract(), parameter));


	public static BuildHostContext WithInitializationLogging<T>(this BuildHostContext @this)
		=> new(new InitializationAwareHostBuilder<T>(@this));

	/**/
	public static StoreContext<TIn, TOut> Store<TIn, TOut>(this Composer<TIn, TOut> @this) => new(@this);

	public static Compose.Store.Operations.StoreContext<TIn, TOut> Store<TIn, TOut>(
		this DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> @this)
		=> new(@this);

	public static Slide Slide(this TimeSpan @this) => new(@this);

	public static IWindow WithinLast(this ITime @this, TimeSpan within) => new WithinLast(@this, within);

	public static IWindow IsPast(this ITime @this, TimeSpan window) => new FromNow(@this, window);

	public static IWindow FromThen(this ITime @this, TimeSpan window) => new FromThen(@this, window);
    
    public static IWindow GreaterThan(this ITime @this, TimeSpan window) => new GreaterThan(@this, window);

	public static IWindow Outside(this ITime @this, TimeSpan window) => new Outside(@this, window);

	/**/

	public static DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> UsingSelf<TIn, TOut>(
		this Compose.Store.Operations.Memory.ConfiguredStoreContext<TIn, TOut> @this) where TIn : class
		=> @this.Using(Object<TIn>.Default);

	/**/

	/*public static QueryComposer<T> Query<T>(this ModelContext _) where T : class => Set<T>.Default.Then();

	public static ComposeComposer<T> Compose<T>(this ModelContext _) where T : class => new();

	public static ContextsComposer<T> Then<T>(this INewContext<T> @this) where T : DbContext => new(@this);

	public static ScopesComposer Then(this IScopes @this) => new(@this);

	public static QueryComposer<TIn, T> Then<TIn, T>(this IQuery<TIn, T> @this) => new(@this);

	public static TrackingComposer<TIn, T> Tracking<TIn, T>(this QueryComposer<TIn, T> @this) where T : class
		=> new(@this);

	public static QueryComposer<T> Then<T>(this IQuery<None, T> @this) => new(@this);

	public static IQuery<T> Out<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());

	public static PlaceholderParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, None, T>> @this)
		=> new(@this);

	public static ElidedParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, T>> @this) => new(@this);*/

	/*public static In<None> Subject<T>(this In<T> @this) => new(@this.Context, None.Default);

	public static In<TTo> Subject<T, TTo>(this In<T> @this, TTo subject) => new(@this.Context, subject);*/

	/*public static QueryComposer<TIn, T?> Account<TIn, T>(this QueryComposer<TIn, T> @this) where T : struct
		=> @this.Select(x => new T?(x));

	public static QueryComposer<TIn, TEntity> Include<TIn, TEntity, TOther>(this QueryComposer<TIn, TEntity> source,
	                                                                        Expression<Func<TEntity, TOther>> path)
		where TEntity : class
		=> source.Select(q => q.Include(path));

	public static QueryComposer<TIn, TEntity> Include<TIn, TEntity>(this QueryComposer<TIn, TEntity> source,
	                                                                string include)
		where TEntity : class
		=> source.Select(q => q.Include(include));

	public static QueryComposer<TIn, TEntity> Includes<TIn, TEntity>(this QueryComposer<TIn, TEntity> source,
	                                                                 params string[] includes)
		where TEntity : class
		=> includes.Aggregate(source, (current, include) => current.Include(include));*/

	/**/
	public static OperationResultComposer<_, T> Then<_, T>(this DragonSpark.Compose.Model.Operations.OperationResultComposer<_,T> @this)
		=> new(@this.Out());

	/*public static InstanceComposer<TIn, T> Then<TIn, T>(this IInstance<TIn, T> @this) => new(@this);

	public static InstanceComposer<T> Then<T>(this IInstance<T> @this) => new(@this);

	public static IQuery<T> Then<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());*/

	/*public static OperationResultComposer<T?> Handle<T>(this OperationResultComposer<T?> @this,
	                                                    IExceptions exceptions, Type? reportedType = null)
		=> new(new ExceptionAwareResult<T>(@this, exceptions, reportedType));*/

	/**/

	/*public static ApplicationProfileContext WithEnvironmentalConfiguredSender(this ApplicationProfileContext @this)
		=> @this.Append(Messaging.Registrations.Default);*/

}