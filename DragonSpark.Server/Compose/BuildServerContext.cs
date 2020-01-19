using DragonSpark.Composition.Compose;
using DragonSpark.Server.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server.Compose {
	public sealed class BuildServerContext
	{
		readonly BuildHostContext                  _context;
		readonly System.Action<IServiceCollection> _services;
		readonly System.Action<IWebHostBuilder>    _application;

		public BuildServerContext(BuildHostContext context, System.Action<IServiceCollection> services,
		                          System.Action<IWebHostBuilder> application)
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
}