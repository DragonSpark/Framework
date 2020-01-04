using DragonSpark.Application.Hosting.Server.GitHub.Testing.Application.Controllers;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using DragonSpark.Testing.Server;
using FluentAssertions;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.GitHub.Testing.Application
{
	public sealed class IntegrationTests
	{
		[Fact]
		async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithTestServer()
			                            .WithServerApplication()
			                            .WithLocatedStartup()
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

	sealed class Registrations : Command<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() : base(x => x.AddSingleton(Start.A.Registration()
		                                                .For.IssueComments()
		                                                .With<CustomHandler>()
		                                                .Get)
		                             .AddScoped<CustomHandler>()) {}
	}
}