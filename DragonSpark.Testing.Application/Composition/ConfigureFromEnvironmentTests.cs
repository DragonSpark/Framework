﻿using DragonSpark.Compose;
using DragonSpark.Composition;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Application.Composition
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
			                            .Start();
			host.Services.GetRequiredService<string>().Should().Be("Hello World!  Configured from Development.");
		}
	}
}
