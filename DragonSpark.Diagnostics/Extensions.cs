using DragonSpark.Composition.Compose;
using Microsoft.Extensions.Configuration;
using System;
using ILogger = Serilog.ILogger;

namespace DragonSpark.Diagnostics;

public static class Extensions
{
	public static BuildHostContext WithSerilog(this BuildHostContext @this)
		=> @this.WithSerilog(DefaultLogger.Default.Get);

	public static BuildHostContext WithSerilog(this BuildHostContext @this, Func<IConfiguration, ILogger> logger)
		=> @this.Configure(new ConfigureSerilog(logger));
}
