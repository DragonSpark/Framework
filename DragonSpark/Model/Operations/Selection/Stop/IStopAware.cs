using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IStopAware<TIn, TOut> : ISelecting<Stop<TIn>, TOut>;

// TODO

public interface IStopAdaptor<TIn, TOut> : IStopAware<TIn, TOut>
{
	ISelecting<TIn, TOut> Alternate { get; }
}

public class StopAdaptor<TIn, TOut> : StopAware<TIn, TOut>, IStopAdaptor<TIn, TOut>
{
	public StopAdaptor(IStopAware<TIn, TOut> stop) : this(stop, new StopAwareAdapter<TIn, TOut>(stop)) {}

	public StopAdaptor(IStopAware<TIn, TOut> stop, ISelecting<TIn, TOut> selecting)
		: base(stop) => Alternate = selecting;

	public ISelecting<TIn, TOut> Alternate { get; }

	// TODO: Remove
	public ValueTask<TOut> Get(TIn parameter) => base.Get(new(parameter));
}

public class StopAwareMaybe<TIn, TOut> : Maybe<Stop<TIn>, TOut?>, IStopAware<TIn, TOut>
{
	public StopAwareMaybe(ISelecting<Stop<TIn>, TOut?> first, ISelecting<Stop<TIn>, TOut?> second)
		: this(first.Off, second.Off) {}

	public StopAwareMaybe(Await<Stop<TIn>, TOut?> first, Await<Stop<TIn>, TOut?> second) : base(first, second) {}
}

sealed class StopAwareAdapter<TIn, TOut> : ISelecting<TIn, TOut>
{
	readonly ISelect<Stop<TIn>, ValueTask<TOut>> _previous;

	public StopAwareAdapter(ISelect<Stop<TIn>, ValueTask<TOut>> previous) => _previous = previous;

	public ValueTask<TOut> Get(TIn parameter) => _previous.Get(new(parameter));
}

public class StopAwareCoalesce<TIn, TOut> : IStopAware<TIn, TOut>
{
	readonly Await<Stop<TIn>, TOut?> _first;
	readonly Await<Stop<TIn>, TOut>   _second;

	public StopAwareCoalesce(ISelecting<Stop<TIn>, TOut?> first, ISelecting<Stop<TIn>, TOut> second)
		: this(first.Off, second.Off) {}

	public StopAwareCoalesce(Await<Stop<TIn>, TOut?> first, Await<Stop<TIn>, TOut> second)
	{
		_first  = first;
		_second = second;
	}

	public async ValueTask<TOut> Get(Stop<TIn> parameter) => await _first(parameter) ?? await _second(parameter);
}