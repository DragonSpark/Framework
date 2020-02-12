using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Testing.Server
{
	sealed class ServerConfiguration : ICommand<IHostBuilder>
	{
		public static ServerConfiguration Default { get; } = new ServerConfiguration();

		ServerConfiguration() {}

		public void Execute(IHostBuilder parameter)
		{
			parameter.ConfigureServices(services => services.AddSingleton<IServer, TestServer>()
			                                                .AddScoped<IHttpContextAccessor, Accessor>());
		}
	}
}