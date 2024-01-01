using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public sealed class ProcessEntry : IOperation<ProcessEntryInput>
{
	readonly IExceptionLogger _logger;

	public ProcessEntry(IExceptionLogger logger) => _logger = logger;

	public async ValueTask Get(ProcessEntryInput parameter)
	{
		var (message, handlers) = parameter;
		using var lease = handlers.AsValueEnumerable().ToArray(ArrayPool<IOperation<object>>.Shared);
		foreach (var operation in lease)
		{
			try
			{
				await operation.Await(message);
			}
			catch (Exception e)
			{
				var type = operation.AsTo<IReportedTypeAware, Type?>(x => x.Get()) ?? operation.GetType();
				await _logger.Await(new(type, e));
			}
		}
	}
}