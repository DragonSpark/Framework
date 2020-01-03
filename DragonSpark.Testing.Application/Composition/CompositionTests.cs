using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Reflection.Selection;
using DragonSpark.Testing.Environment.Development;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
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
		async Task VerifyAssemblies()
		{
			using var host = await Start.A.Host()
			                            .RegisterModularity()
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<IReadOnlyList<Assembly>>().Should().NotBeNull();
		}

		[Fact]
		async Task VerifyDevelopmentEnvironmentalRegistration()
		{
			using var host = await Start.A.Host()
			                            .WithEnvironment("Development")
			                            .WithDefaultComposition()
			                            .RegisterModularity()
			                            .Configure(x => x.For<IHelloWorld>().FromEnvironment())
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<HelloWorld>();
		}

		[Fact]
		async Task VerifyEnvironmentalRegistration()
		{
			{
				using var host = await Start.A.Host()
				                            .WithEnvironment("Production")
				                            .WithDefaultComposition()
				                            .RegisterModularity()
				                            .Configure(x => x.For<IHelloWorld>().FromEnvironment())
				                            .Operations()
				                            .Start();
				host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<Environment.HelloWorld>();
			}

			{
				using var host = await Start.A.Host()
				                            .WithDefaultComposition()
				                            .RegisterModularity()
				                            .Configure(x => x.For<IHelloWorld>().FromEnvironment())
				                            .Operations()
				                            .Start();
				host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<Environment.HelloWorld>();
			}
		}

		[Fact]
		async Task VerifyAllAssemblyTypes()
		{
			using var host = await Start.A.Host()
			                            .RegisterModularity<AllAssemblyTypes>()
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<IReadOnlyList<Assembly>>().Should().NotBeNull();
		}

		[Fact]
		async Task VerifyActivation()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Operations()
			                            .Start();

			host.Services.GetRequiredService<Singleton>().Should().BeSameAs(Singleton.Default);

			host.Services.GetRequiredService<Activated>()
			    .Should()
			    .NotBeNull()
			    .And.Subject.Should()
			    .NotBeSameAs(host.Services.GetRequiredService<Activated>());
		}

		[Fact]
		async Task VerifyActivationThrowsWithoutConfiguration()
		{
			using var host = await Start.A.Host().Operations().Start();
			host.Services.Invoking(x => x.GetRequiredService<Singleton>())
			    .Should()
			    .Throw<InvalidOperationException>();

			host.Services.Invoking(x => x.GetRequiredService<Activated>())
			    .Should()
			    .Throw<InvalidOperationException>();
		}

		[Fact]
		async Task VerifyCompositionRoot()
		{
			using var host = await Start.A.Host()
			                            .ComposeUsingRoot<Root>()
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
			                            .ComposeUsingRoot<Decorate>()
			                            .Operations()
			                            .Start();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Decorated: Hello World!");
		}

		sealed class Singleton
		{
			public static Singleton Default { get; } = new Singleton();

			Singleton() {}
		}

		sealed class Activated {}

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