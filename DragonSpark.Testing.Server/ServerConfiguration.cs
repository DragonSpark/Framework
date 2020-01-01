using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DragonSpark.Testing.Server
{
	public sealed class Start
	{
		public static Start A { get; } = new Start();

		Start() {}

		public BuildHostContext Host()
			=> Compose.Start.A.Host().WithConfiguration(ServerConfiguration.Default);
	}

	sealed class ServerConfiguration : ICommand<IWebHostBuilder>
	{
		public static ServerConfiguration Default { get; } = new ServerConfiguration();

		ServerConfiguration() {}

		public void Execute(IWebHostBuilder parameter)
		{
			parameter.UseTestServer();
		}
	}
}