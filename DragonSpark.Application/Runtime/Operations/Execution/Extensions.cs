using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Runtime.Operations.Execution;

public static class Extensions
{
	public static BuildHostContext WithAmbientOperations(this BuildHostContext @this)
		=> @this.Configure(Registrations.Default);

	public static ISelecting<TIn, TOut> WithAmbientOperations<TIn, TOut>(
		this ISelecting<TIn, TOut> @this, object instance)
		=> new AmbientOperation<TIn, TOut>(@this, instance);
}