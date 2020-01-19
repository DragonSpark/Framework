using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Composition
{
	sealed class Configure : IAlteration<IHostBuilder>
	{
		readonly Action<IServiceCollection> _configure;

		public Configure(Action<IServiceCollection> configure) => _configure = configure;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureServices(_configure);
	}
}