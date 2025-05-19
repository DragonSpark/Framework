using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class StopAdaptor<TIn, TOut> : StopAware<TIn, TOut>, IStopAdaptor<TIn, TOut>
{
	protected StopAdaptor(IStopAware<TIn, TOut> stop) : this(stop, new StopAwareAdapter<TIn, TOut>(stop)) {}

	protected StopAdaptor(IStopAware<TIn, TOut> stop, ISelecting<TIn, TOut> selecting)
		: base(stop) => Alternate = selecting;

	public ISelecting<TIn, TOut> Alternate { get; }

	// TODO: Stop: Remove
	public ValueTask<TOut> Get(TIn parameter) => base.Get(new(parameter));
}