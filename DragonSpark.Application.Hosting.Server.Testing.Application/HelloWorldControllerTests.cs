using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Services;
using DragonSpark.Testing.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.Testing.Application
{
	public sealed class HelloWorldControllerTests
	{
		[Fact]
		async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .WithStartup<Startup>()
			                            .Operations()
			                            .Start();

			host.Should().NotBeNull();
		}

		[Theory]
		[InlineData("/HelloWorld")]
		async Task VerifyHelloWorld(string url)
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .WithLocatedStartup()
			                            .Operations()
			                            .Start();

			var client   = host.GetTestServer().CreateClient();
			var response = await client.GetAsync(url);
			response.StatusCode.Should().Be(HttpStatusCode.OK);

			var content = await response.Content.ReadAsStringAsync();
			content.Should().Be("Hello World!");
		}
	}

	sealed class Startup : IStartupMarker
	{
		public void Configure() {}
	}

	[ApiController, Route("[controller]")]
	public sealed class HelloWorldController : ControllerBase
	{
		public string Get() => "Hello World!";
	}
}
