using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Run;

public interface IApplication : IHost, IAsyncDisposable
{
	public IConfiguration Configuration { get; }

	public IHostEnvironment Environment { get; }

	public IHostApplicationLifetime Lifetime { get; }

	public ILogger Logger { get; }

	public ICollection<string> Urls { get; }
}