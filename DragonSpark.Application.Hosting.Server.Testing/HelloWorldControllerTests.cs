using DragonSpark.Application.Hosting.Server.Testing.Environment;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Testing.Server;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Dependency = DragonSpark.Application.Hosting.Server.Testing.Environment.Development.Dependency;

namespace DragonSpark.Application.Hosting.Server.Testing
{
	// ReSharper disable once TestFileNameWarning
	public sealed class HelloWorldControllerTests
	{
		[Theory, InlineData("Development", typeof(Dependency)),
		 InlineData("Production", typeof(Environment.Production.Dependency))]
		public async Task VerifyEnvironment(string environment, Type expected)
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithDefaultComposition()
			                            .RegisterModularity()
			                            .WithEnvironment(environment)
			                            .WithServerApplication()
			                            .WithEnvironmentalConfiguration()
			                            .As.Is()
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<IDependency>().Should().BeOfType(expected);
		}

		[Theory, InlineData("/HelloWorld")]
		public async Task VerifyHelloWorld(string url)
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .NamedFromPrimaryAssembly()
			                            .As.Is()
			                            .Operations()
			                            .Run();

			var client   = host.GetTestServer().CreateClient();
			var response = await client.GetAsync(url);
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var content = await response.Content.ReadAsStringAsync();
			content.Should().Be("Hello World!");
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .As.Is()
			                            .Operations()
			                            .Run();

			host.Should().NotBeNull();
		}
	}
}