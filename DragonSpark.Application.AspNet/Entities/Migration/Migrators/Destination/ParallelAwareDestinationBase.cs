using DragonSpark.Application.AspNet.Entities.Migration.Migrators.Instances;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

class ParallelAwareDestinationBase<TFrom, TTo> : IDestination<TFrom> where TTo : class where TFrom : class
{
	readonly IRunner<TFrom> _runner;

	protected ParallelAwareDestinationBase(IEntry<TFrom, TTo> entry, IMap map)
		: this(new Element<TFrom, TTo>(entry, map)) {}

	protected ParallelAwareDestinationBase(IElement<TFrom, TTo> element) : this(new Writer<TFrom, TTo>(element)) {}

	protected ParallelAwareDestinationBase(IWriter<TFrom> writer) : this(new Runner<TFrom>(writer)) {}

	protected ParallelAwareDestinationBase(IRunner<TFrom> runner) => _runner = runner;

	public IAsyncEnumerable<DbContext> Get(Stop<DestinationInput<TFrom>> parameter)
	{
		var (_, stop) = parameter;

		var (work, reader) = _runner.Get(parameter);

		_ = Task.Run(work.Self, stop);

		return reader.ReadAllAsync(stop);
	}
}