using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime.Invocation;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Writer<TFrom, TTo> : IWriter<TFrom> where TFrom : class where TTo : class
{
	readonly IElement<TFrom, TTo> _element;
	readonly ushort               _parallelism;

	public Writer(IElement<TFrom, TTo> element) : this(element, MaximumParallelism.Default) {}

	public Writer(IElement<TFrom, TTo> element, ushort parallelism)
	{
		_element     = element;
		_parallelism = parallelism;
	}

	public async Task Get(Stop<WriterInput<TFrom>> parameter)
	{
		var ((origin, workspaces, from, writer), stop) = parameter;
		try
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
			await Parallel.ForEachAsync(from.Open(), options,
			                            new Each<TFrom, TTo>(new(origin, workspaces, from, writer), _element).Get)
			              .Off();

			writer.Complete();
		}
		catch (Exception ex)
		{
			writer.Complete(ex);
		}
	}
}