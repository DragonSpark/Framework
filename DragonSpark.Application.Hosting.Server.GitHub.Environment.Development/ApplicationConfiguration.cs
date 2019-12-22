using DragonSpark.Model.Commands;
using DragonSpark.Services;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Application.Hosting.Server.GitHub.Environment
{
	public sealed class ApplicationConfiguration : Command<IApplicationBuilder>, IApplicationConfiguration
	{
		public static ApplicationConfiguration Default { get; } = new ApplicationConfiguration();

		ApplicationConfiguration() : base(Server.Environment.ApplicationConfiguration.Default) {}
	}
}
