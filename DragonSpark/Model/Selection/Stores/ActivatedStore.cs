using DragonSpark.Compose;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Stores;

public class ActivatedStore<TIn, TOut> : Store<TIn, TOut>
{
	protected ActivatedStore() : base(New<TIn, TOut>.Default.ToDelegateReference()) {}
}