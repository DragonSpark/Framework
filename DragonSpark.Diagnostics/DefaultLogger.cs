using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DragonSpark.Diagnostics;

sealed class DefaultLogger : Select<IServiceCollection, ILogger>
{
	public static DefaultLogger Default { get; } = new();

	DefaultLogger() : base(Start.A.Selection<IServiceCollection>()
	                            .By.Calling(x => x.Configuration())
	                            .Select(CreateConfiguration.Default)
	                            .Select(CreateLogger.Default)) {}
}