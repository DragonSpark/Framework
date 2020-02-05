using DragonSpark.Model.Commands;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Builder;

namespace DragonSpark.Server
{
	sealed class ConfigureFromEnvironment : SelectedCommand<IApplicationBuilder>
	{
		public static ConfigureFromEnvironment Default { get; } = new ConfigureFromEnvironment();

		ConfigureFromEnvironment() : base(ServiceConfigurationLocator.Default.Get) {}
	}

	public sealed class EnvironmentName : EnvironmentVariable
	{
		public static EnvironmentName Default { get; } = new EnvironmentName();

		EnvironmentName() : base("ASPNETCORE_ENVIRONMENT") {}
	}
}