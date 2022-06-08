using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Azure.Storage;

static class Log
{
	readonly static Action<ILogger, string, string, Exception?> _executingFileResultWithNoFileName
		= LoggerMessage.Define<string, string>(eventId: new EventId(1, nameof(ExecutingBlobStorageResult)),
		                                       logLevel: LogLevel.Information,
		                                       formatString:
		                                       "Executing {FileResultType}, sending file with download name '{FileDownloadName}'");

	readonly static Action<ILogger, Exception?> _writingRangeToBody
		= LoggerMessage.Define(eventId: new EventId(17, nameof(WritingRangeToBody)),
		                       logLevel: LogLevel.Debug,
		                       formatString: "Writing the requested range of bytes to the body.");

	public static void ExecutingBlobStorageResult(this ILogger logger, BlobStorageResult result)
	{
		if (logger.IsEnabled(LogLevel.Information))
		{
			var resultType = result.GetType().Name;
			_executingFileResultWithNoFileName(logger, resultType, result.FileDownloadName, null);
		}
	}

	public static void WritingRangeToBody(this ILogger logger)
	{
		if (logger.IsEnabled(LogLevel.Debug))
		{
			_writingRangeToBody(logger, null);
		}
	}
}