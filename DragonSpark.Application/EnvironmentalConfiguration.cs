using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DragonSpark.Application
{
	sealed class EnvironmentalConfiguration : Result<IConfiguration>
	{
		public static EnvironmentalConfiguration Default { get; } = new EnvironmentalConfiguration();

		EnvironmentalConfiguration()
			: base(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
			                                 .AddJsonFile("appsettings.json")
			                                 .AddJsonFile($"appsettings.{EnvironmentName.Default}.json", true)
			                                 .Build) {}
	}
}