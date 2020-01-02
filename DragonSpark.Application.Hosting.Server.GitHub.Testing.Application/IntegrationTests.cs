using DragonSpark.Application.Hosting.Server.GitHub.Testing.Application.Controllers;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using DragonSpark.Testing.Server;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.GitHub.Testing.Application
{
	public sealed class IntegrationTests
	{
		[Theory]
		[InlineData("Development", "/HelloWorld")]
		public async Task VerifyHelloWorld(string environment, string url)
		{
			using var host = await Start.A.Server()
			                            .WithEnvironment(environment)
			                            .WithConfiguration<Configurator>()
			                            .WithComposition()
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

	sealed class Configurator : Server.Configurator
	{
		[UsedImplicitly]
		public Configurator() : base(Registrations.Default.Then(DefaultServiceConfiguration.Default)) {}
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