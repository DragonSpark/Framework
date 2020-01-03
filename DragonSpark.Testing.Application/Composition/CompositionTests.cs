using DragonSpark.Compose;
using DragonSpark.Composition;
using FluentAssertions;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Application.Composition
{
	public sealed class CompositionTests
	{
		[Fact]
		async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Operations()
			                            .Start();
			host.Services.GetType()
			    .FullName.Should()
			    .Be("LightInject.Microsoft.DependencyInjection.LightInjectServiceProvider");
		}

		[Fact]
		async Task VerifyCompositionRoot()
		{
			using var host = await Start.A.Host()
			                            .ComposeUsing<Root>()
			                            .Operations()
			                            .Start();

			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Hello World from Root!");
		}

		[Fact]
		async Task VerifyConfiguration()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Hello World!");
		}

		[Fact]
		async Task VerifyDecoration()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .ComposeUsing(x => x.Decorate<string>((factory, s) => $"Decorated: {s}"))
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Decorated: Hello World!");
		}

		[Fact]
		async Task VerifyDecorationViaRoot()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .ComposeUsing<Decorate>()
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Decorated: Hello World!");
		}

		sealed class Decorate : ICompositionRoot
		{
			public void Compose(IServiceRegistry serviceRegistry)
			{
				serviceRegistry.Decorate<string>((factory, s) => $"Decorated: {s}");
			}
		}

		sealed class Root : ICompositionRoot
		{
			public void Compose(IServiceRegistry serviceRegistry)
			{
				serviceRegistry.RegisterInstance($"Hello World from {nameof(Root)}!");
			}
		}
	}
}