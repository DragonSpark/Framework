using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Diagnostics;

sealed class DefaultLoggingProvider : Select<IServiceProvider, ILoggerProvider>
{
	public static DefaultLoggingProvider Default { get; } = new();

	DefaultLoggingProvider()
		: base(CreateConfiguration.Default.Then().Select(CreateLogger.Default).Select(CreateLoggingProvider.Default)) {}
}