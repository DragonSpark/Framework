using DragonSpark.Application.AspNet.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Invocation;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Writer<TFrom, TTo> : IWriter<TFrom> where TFrom : class where TTo : class
{
	readonly IElement<TFrom, TTo>  _element;
	readonly ushort                _parallelism;
	readonly ICondition<Exception> _process;

	public Writer(IElement<TFrom, TTo> element) : this(element, MaximumParallelism.Default) {}

	public Writer(IElement<TFrom, TTo> element, ushort parallelism)
		: this(element, parallelism, ShouldProcess.Default) {}

	public Writer(IElement<TFrom, TTo> element, ushort parallelism, ICondition<Exception> process)
	{
		_element     = element;
		_parallelism = parallelism;
		_process     = process;
	}

	public async Task Get(Stop<WriterInput<TFrom>> parameter)
	{
		var ((_, from, writer), stop) = parameter;
		try
		{
			var options = new ParallelOptions { MaxDegreeOfParallelism = _parallelism, CancellationToken = stop };
			await Parallel.ForEachAsync(from.Open(), options,
			                            new Each<TFrom, TTo>(parameter, _element).Get)
			              .Off();

			writer.Complete();
		}
		catch (Exception ex) when (_process.Get(ex))
		{
			writer.Complete(ex);
		}
	}
}