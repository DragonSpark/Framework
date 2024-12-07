using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application.Run;

public interface IHostedApplicationBuilder : IHostApplicationBuilder
{
	IHostBuilder Builder { get; }

	IHostedApplicationBuilder With(IHostBuilder builder);
}