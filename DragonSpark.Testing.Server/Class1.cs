using DragonSpark.Model.Commands;
using DragonSpark.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Testing.Server
{
	public static class TestHost
	{
		public static ValueTask<IHost> ConfiguredWith<T>(string environment = "Production") where T : class
			=> ConfiguredTestHosts<T>.Default.Get(environment);
	}

	sealed class ConfiguredTestHosts<T> : OperationResult<string, IHost> where T : class
	{
		public static ConfiguredTestHosts<T> Default { get; } = new ConfiguredTestHosts<T>();

		ConfiguredTestHosts() : base(new TestHostBuilder(HostConfiguration<T>.Default.Execute)) {}
	}

	sealed class HostConfiguration<T> : ICommand<IWebHostBuilder> where T : class
	{
		public static HostConfiguration<T> Default { get; } = new HostConfiguration<T>();

		HostConfiguration() {}

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseTestServer()
			         .UseStartup<T>();
		}
	}

	sealed class TestHostBuilder : IOperationResult<string, IHost>
	{
		readonly Action<IWebHostBuilder> _configure;

		public TestHostBuilder(Action<IWebHostBuilder> configure) => _configure = configure;

		public ValueTask<IHost> Get(string parameter) => new HostBuilder().UseEnvironment(parameter)
		                                                                  .ConfigureWebHost(_configure)
		                                                                  .StartAsync()
		                                                                  .ToOperation();
	}
}
