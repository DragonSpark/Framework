using DragonSpark.Application.Hosting.Server.GitHub.Testing.Application.Controllers;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Services;
using FluentAssertions;
using JetBrains.Annotations;
using LightInject;
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
		[Fact]
		public async Task Verify()
		{
			using var host = await DragonSpark.Testing.Server.Start.A.Host()
			                                  .WithConfiguration<Configurator>()
			                                  .WithComposition()
			                                  .Operations()
			                                  .Start();
			host.Services.GetType()
			    .FullName.Should()
			    .Be("LightInject.Microsoft.DependencyInjection.LightInjectServiceProvider");
		}

		[Fact]
		public async Task VerifyBasicComposition()
		{
			using var host = await DragonSpark.Testing.Server.Start.A.Host()
			                                  .WithConfiguration<Configurator>()
			                                  .WithComposition<Root>()
			                                  .Operations()
			                                  .Start();

			host.Services.GetRequiredService<string>().Should().Be("Hello World from Root!");
		}

		[Theory]
		[InlineData("Development", "/HelloWorld")]
		public async Task VerifyHelloWorld(string environment, string url)
		{
			using var host = await DragonSpark.Testing.Server.Start.A.Host()
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

	sealed class Root : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.RegisterInstance($"Hello World from {nameof(Root)}!");
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

		Registrations() : base(x => x.AddSingleton(Start.A.Registration.For.IssueComments()
		                                                .With<CustomHandler>()
		                                                .Get)
		                             .AddScoped<CustomHandler>()) {}
	}
}