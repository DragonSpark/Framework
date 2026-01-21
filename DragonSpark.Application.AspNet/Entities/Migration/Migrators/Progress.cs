using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Progress<T> : ICommand<decimal>
{
	readonly ILogger _logger;
	readonly uint    _total;
	readonly Type    _type;

	public Progress(ILogger logger, uint total) : this(logger, total, A.Type<T>()) {}

	public Progress(ILogger logger, uint total, Type type)
	{
		_logger = logger;
		_total  = total;
		_type   = type;
	}

	public void Execute(decimal parameter)
	{
		var processed = (long)Math.Round(parameter * _total);
		_logger.LogDebug("[{Type}] Progress (BulkInsertOrUpdate): {Processed}/{Total} ({Percent:F1}%)",
		                 _type, processed, _total, parameter * 100);
	}
}