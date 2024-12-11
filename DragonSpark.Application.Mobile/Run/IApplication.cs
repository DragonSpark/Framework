using Microsoft.Extensions.Hosting;
using Uno.Extensions.Hosting;

namespace DragonSpark.Application.Mobile.Run;

public interface IApplication : IApplicationBuilder
{
	IHost Host { get; }
	/*public IConfiguration Configuration { get; }

	public IHostEnvironment Environment { get; }

	public IHostApplicationLifetime Lifetime { get; }

	public ILogger Logger { get; }

	public ICollection<string> Urls { get; }*/
}
