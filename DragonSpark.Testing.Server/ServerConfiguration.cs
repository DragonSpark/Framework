using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext Server(this ModelContext _)
			=> Start.A.Host().WithConfiguration(ServerConfiguration.Default);
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