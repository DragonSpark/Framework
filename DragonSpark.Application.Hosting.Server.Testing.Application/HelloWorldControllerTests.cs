using DragonSpark.Application.Hosting.Server.Testing.Application.Environment;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Testing.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.Testing.Application
{
	public sealed class HelloWorldControllerTests
	{
		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .As.Is()
			                            .Operations()
			                            .Start();

			host.Should().NotBeNull();
		}

		[Theory]
		[InlineData("Development", typeof(Environment.Development.Dependency))]
		[InlineData("Production", typeof(Environment.Production.Dependency))]
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
			                            .Start();

			host.Services.GetRequiredService<IDependency>().Should().BeOfType(expected);
		}

		[Theory]
		[InlineData("/HelloWorld")]
		public async Task VerifyHelloWorld(string url)
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .NamedFromPrimaryAssembly()
			                            .As.Is()
			                            .Operations()
			                            .Start();

			var client   = host.GetTestServer().CreateClient();
			var response = await client.GetAsync(url);
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var content = await response.Content.ReadAsStringAsync();
			content.Should().Be("Hello World!");
		}
	}

	[ApiController, Route("[controller]")]
	public sealed class HelloWorldController : ControllerBase
	{
		public string Get() => "Hello World!";
	}
}