using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Runtime.Activation;

public sealed class Activations<TIn, TOut> : ActivatedStore<TIn, TOut> where TOut : IActivateUsing<TIn>
{
	public static Activations<TIn, TOut> Default { get; } = new Activations<TIn, TOut>();

	Activations() {}
}