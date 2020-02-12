﻿using DragonSpark.Compose;
using DragonSpark.Composition;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public sealed class ConfigureFromEnvironmentTests
	{
		[Fact]
		async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithEnvironment("Development")
			                            .WithDefaultComposition()
			                            .RegisterModularity()
			                            .ConfigureFromEnvironment()
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<string>().Should().Be("Hello World!  Configured from Development.");
		}
	}
}