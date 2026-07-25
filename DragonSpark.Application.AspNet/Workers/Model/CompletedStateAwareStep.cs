using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Workers.Model;

sealed class CompletedStateAwareStep<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous;
	readonly Guid          _identifier;

	public CompletedStateAwareStep(IStopAware<T> previous, Guid identifier)
	{
		_previous   = previous;
		_identifier = identifier;
	}

	public ValueTask Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		using var steps = subject.CompletedSteps.AsValueEnumerable()
		                         .Select(x => x.Identifier)
		                         .ToArray(ArrayPool<Guid>.Shared);
		return steps.Contains(_identifier) ? ValueTask.CompletedTask : _previous.Get(parameter);
	}
}