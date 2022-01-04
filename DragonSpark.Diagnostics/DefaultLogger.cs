using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class DefaultLogger : Select<IConfiguration, ILogger>
{
	public static DefaultLogger Default { get; } = new();

	DefaultLogger() : base(CreateConfiguration.Default.Then().Select(CreateLogger.Default)) {}
}