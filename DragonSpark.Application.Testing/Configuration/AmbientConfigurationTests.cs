using DragonSpark.Application.Configuration;
using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;
using Xunit;

namespace DragonSpark.Application.Testing.Configuration;

public sealed class AmbientConfigurationTests
{
	[Theory]
	[InlineData("server-other", "Development")]
	[InlineData("server-other", "Testing")]
	[InlineData("server-other", "Production")]
	[InlineData("server-other", "Staging")]
	[InlineData("server-interactive", "Development")]
	[InlineData("server-interactive", "Testing")]
	[InlineData("server-interactive", "Production")]
	[InlineData("server-interactive", "Staging")]
	[InlineData("worker", "Development")]
	[InlineData("worker", "Testing")]
	[InlineData("worker", "Production")]
	[InlineData("worker", "Staging")]
	[InlineData("server", "Development")]
	[InlineData("server", "Testing")]
	[InlineData("server", "Production")]
	[InlineData("server", "Staging")]
	public void Verify(string category, string environment)
	{
		var root = PrimaryDirectory.Default.Get()
		                           .CreateSubdirectory("Resources")
		                           .CreateSubdirectory(category)
		                           .FullName;
		var       host  = new HostEnvironment { EnvironmentName = environment, ContentRootPath = root };
		using var lease = AmbientConfigurationSources.Default.Get(host);

		var expected = category.Contains("-") ? new[]
		{
			"appsettings.json",
			$"appsettings.{host.EnvironmentName}.json",
			@"server\appsettings.json",
			$@"server\appsettings.{host.EnvironmentName}.json",
			$@"{category.Replace('-', Path.DirectorySeparatorChar)}\appsettings.json",
			$@"{category.Replace('-', Path.DirectorySeparatorChar)}\appsettings.{host.EnvironmentName}.json",
		}
		:
		new[]
		{
			"appsettings.json",
			$"appsettings.{host.EnvironmentName}.json",
			$@"{category}\appsettings.json",
			$@"{category}\appsettings.{host.EnvironmentName}.json",
		};

		lease.AsValueEnumerable()
		     .Select(x => x.Path.Verify().Replace($@"{root}\.configuration\", string.Empty))
		     .ToArray()
		     .Should()
		     .BeEquivalentTo(expected);
	}

	sealed class HostEnvironment : IHostEnvironment
	{
		public string ApplicationName { get; set; } = null!;

		public string ContentRootPath { get; set; } = null!;

		public string EnvironmentName { get; set; } = null!;

		public IFileProvider ContentRootFileProvider { get; set; } = null!;
	}
}