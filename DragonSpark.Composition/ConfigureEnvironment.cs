using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Activation;
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

	sealed class ConfigureEnvironment : IAlteration<IHostBuilder>, IActivateUsing<string>
	{
		public static ISelect<string, IAlteration<IHostBuilder>> Defaults { get; }
			= Start.A.Selection<string>()
			       .By.StoredActivation<ConfigureEnvironment>()
			       .Stores()
			       .Reference();

		readonly string _environment;

		public ConfigureEnvironment(string environment) => _environment = environment;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.UseEnvironment(_environment);
	}
}