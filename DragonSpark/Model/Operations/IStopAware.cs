using DragonSpark.Model.Selection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations;

public interface IStopAware<T> : IOperation<Stop<T>>;

public interface IStopAware : IOperation<CancellationToken>;

// TODO

public class StopAware<T> : Operation<Stop<T>>, IStopAware<T>
{
	public StopAware(ISelect<Stop<T>, ValueTask> select) : base(select) {}

	public StopAware(Func<Stop<T>, ValueTask> select) : base(select) {}
}

// TODO


sealed class StopAwareAmbientAdapter<T> : IOperation<T>
{
	readonly ISelect<Stop<T>, ValueTask> _previous;

	public StopAwareAmbientAdapter(ISelect<Stop<T>, ValueTask> previous) => _previous = previous;

	public ValueTask Get(T parameter) => _previous.Get(new(parameter));
}

sealed class StopAwareAdapter<T> : IStopAware<T>
{
	readonly IOperation<T> _previous;

	public StopAwareAdapter(IOperation<T> previous) => _previous = previous;

	public ValueTask Get(Stop<T> parameter) => _previous.Get(parameter.Subject);
}

public interface IStopAdaptor<T> : IStopAware<T>
{
	IOperation<T> Alternate { get; }
}

public class StopAdaptor<T> : StopAware<T>, IStopAdaptor<T>
{
	protected StopAdaptor(IStopAware<T> stop) : this(stop, new StopAwareAmbientAdapter<T>(stop)) {}

	protected StopAdaptor(IStopAware<T> stop, IOperation<T> selecting)
		: base(stop) => Alternate = selecting;

	public IOperation<T> Alternate { get; }

	// TODO: Stop: Remove
	public ValueTask Get(T parameter) => base.Get(new(parameter));
}