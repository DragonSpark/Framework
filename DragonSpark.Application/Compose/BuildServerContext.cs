using DragonSpark.Composition.Compose;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose;

public sealed class BuildServerContext
{
	readonly Action<IWebHostBuilder>    _application;
	readonly BuildHostContext           _context;
	readonly Action<IServiceCollection> _services;

	public BuildServerContext(BuildHostContext context, Action<IServiceCollection> services,
	                          Action<IWebHostBuilder> application)
	{
		_context     = context;
		_services    = services;
		_application = application;
	}

	public BuildHostContext Application()
		=> _context.Select(new ApplicationWebHostConfiguration(_application)).Configure(_services);

	public BuildHostContext Is()
		=> _context.Select(new WebHostConfiguration(_application)).Configure(_services);
}