using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace DragonSpark.Services.Application
{
	sealed class StartupConfiguration<T> : ICommand<IWebHostBuilder> where T : class
	{
		public static StartupConfiguration<T> Default { get; } = new StartupConfiguration<T>();

		StartupConfiguration() {}

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseStartup<T>();
		}
	}



	sealed class WebHostConfiguration : IAlteration<IHostBuilder>
	{
		readonly Action<IWebHostBuilder> _configure;

		public WebHostConfiguration(Action<IWebHostBuilder> configure) => _configure = configure;

		public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureWebHost(_configure);
	}
}
