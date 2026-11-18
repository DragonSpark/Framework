using DragonSpark.Compose;
using Microsoft.Extensions.Hosting;
using Sentry.Extensibility;

namespace DragonSpark.Sentry;

public static class Extensions
{
	extension(IHostApplicationBuilder @this)
	{
		public IHostApplicationBuilder WithSentry(string? name = null)
			=> @this.WithSentry(DefaultExceptionFilter.Default, name);

		public IHostApplicationBuilder WithSentry(IExceptionFilter filter, string? name = null)
			=> new ConfigureSentry(name, filter).Parameter(@this);
	}
}