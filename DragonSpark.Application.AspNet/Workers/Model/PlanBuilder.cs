using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Linq;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class PlanBuilder<T> : IPlanBuilder<T> where T : ExternalProcess
{
	readonly Func<Step<T>, IStopAware<T>> _step;

	protected PlanBuilder(IStepBuilder<T> step) : this(step.Get) {}

	protected PlanBuilder(Func<Step<T>, IStopAware<T>> step) => _step = step;

	public IStopAware<T> Get(Array<Step<T>> parameter)
	{
		using var lease = parameter.Open()
		                           .Select(_step)
		                           .AsValueEnumerable()
		                           .ToArray(ArrayPool<IStopAware<T>>.Shared);
		var result = lease.Aggregate(lease.First(), (current, next) => current.Then().Append(next).Out());
		return result;
	}
}