using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime;
using DragonSpark.Services.Compose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Services.Application
{
	sealed class StartupConfiguration : Command<IWebHostBuilder>
	{
		public static StartupConfiguration Default { get; } = new StartupConfiguration();

		StartupConfiguration() : base(StartupConfiguration<Startup>.Default) {}

		sealed class Startup : Configurator
		{
			public Startup() : base(Empty.Command<IServiceCollection>(), Empty.Command<IApplicationBuilder>()) {}
		}
	}

	sealed class StartupConfiguration<T> : ICommand<IWebHostBuilder> where T : class
	{
		public static StartupConfiguration<T> Default { get; } = new StartupConfiguration<T>();

		StartupConfiguration() {}

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseStartup<T>();
		}
	}

	sealed class EnvironmentalStartup : Configurator
	{
		public static EnvironmentalStartup Default { get; } = new EnvironmentalStartup();

		EnvironmentalStartup() : base(Composition.Compose.ConfigureFromEnvironment.Default.Execute,
		                              ConfigureFromEnvironment.Default.Execute) {}
	}



	sealed class WebHostConfiguration : IAlteration<IHostBuilder>
	{
		readonly Action<IWebHostBuilder> _configure;

		public WebHostConfiguration(Action<IWebHostBuilder> configure) => _configure = configure;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureWebHost(_configure);
	}
}
