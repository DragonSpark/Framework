using System.IO;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Configuration;

namespace DragonSpark.Application.AspNet.Entities.Configure;

sealed class EnvironmentalConfiguration : Result<IConfiguration>
{
	public static EnvironmentalConfiguration Default { get; } = new();

	EnvironmentalConfiguration() : this(EnvironmentName.Default) {}

	public EnvironmentalConfiguration(string environment)
		: base(new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
		                                 .AddJsonFile("appsettings.json")
		                                 .AddJsonFile($"appsettings.{environment}.json", true)
		                                 .AddEnvironmentVariables()
		                                 .Build) {}
}