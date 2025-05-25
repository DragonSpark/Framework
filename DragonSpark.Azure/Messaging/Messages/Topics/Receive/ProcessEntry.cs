using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

public sealed class ProcessEntry : IStopAware<ProcessEntryInput>
{
	readonly IExceptionLogger _logger;

	public ProcessEntry(IExceptionLogger logger) => _logger = logger;

	public async ValueTask Get(Stop<ProcessEntryInput> parameter)
	{
		var ((message, handlers), stop) = parameter;
		using var lease = handlers.AsValueEnumerable().ToArray(ArrayPool<IStopAware<object>>.Shared);
		foreach (var operation in lease)
		{
			try
			{
				await operation.Off(new(message, stop));
			}
			catch (Exception e)
			{
				var type = operation.AsTo<IReportedTypeAware, Type?>(x => x.Get()) ?? operation.GetType();
				await _logger.Off(new(type, e));
			}
		}
	}
}