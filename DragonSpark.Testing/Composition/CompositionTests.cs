using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Testing.Environment.Development;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using JetBrains.Annotations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	// ReSharper disable once FilesNotPartOfProjectWarning
	// ReSharper disable once TestFileNameWarning
	public sealed class CompositionTests
	{
		public interface IDoesNotExist {}

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
				serviceRegistry.Decorate<string>((_, s) => $"Decorated: {s}");
			}
		}

		sealed class Root : ICompositionRoot
		{
			public void Compose(IServiceRegistry serviceRegistry)
			{
				serviceRegistry.RegisterInstance($"Hello World from {nameof(Root)}!");
			}
		}

		[Fact]
		public async Task Verify()
		{
			using var host = await Start.A.Host()
			                            .WithComposition()
			                            .Operations()
			                            .Run();
			host.Services.Should().BeOfType<ActivationAwareServiceProvider>();
		}

		[Fact]
		public async Task VerifyArray()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Configure(x => x.AddSingleton<Service>())
			                            .Operations()
			                            .Run();
			host.Services.Invoking(x => x.GetRequiredService<Service>())
			    .Should()
			    .Throw<InvalidOperationException>()
			    .WithMessage("Unable to resolve type: DragonSpark.Testing.Composition.CompositionTests+Service, service name: ");
		}

		sealed class Service : ISelect<object, object>
		{
			[UsedImplicitly] readonly Array<object> _dependency;

			public Service(Array<object> dependency) => _dependency = dependency;

			public object Get(object parameter) => parameter;
		}

		[Fact]
		public async Task VerifyActivation()
		{
			using var host = await Start.A.Host()
			                            .WithDefaultComposition()
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<Singleton>().Should().BeSameAs(Singleton.Default);

			host.Services.GetRequiredService<Activated>()
			    .Should()
			    .NotBeNull()
			    .And.Subject.Should()
			    .NotBeSameAs(host.Services.GetRequiredService<Activated>());
		}

		[Fact]
		public async Task VerifyActivationThrowsWithoutConfiguration()
		{
			using var host = await Start.A.Host().Operations().Run();
			host.Services.Invoking(x => x.GetRequiredService<Singleton>())
			    .Should()
			    .Throw<InvalidOperationException>();

			host.Services.Invoking(x => x.GetRequiredService<Activated>())
			    .Should()
			    .Throw<InvalidOperationException>();
		}

		[Fact]
		public async Task VerifyAllAssemblyTypes()
		{
			using var host = await Start.A.Host()
			                            .RegisterModularity<AllAssemblyTypes>()
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<IArray<Assembly>>().Should().NotBeNull();
		}

		[Fact]
		public async Task VerifyAssemblies()
		{
			using var host = await Start.A.Host()
			                            .RegisterModularity()
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<IArray<Assembly>>().Should().NotBeNull();
		}

		[Fact]
		public async Task VerifyCompositionRoot()
		{
			using var host = await Start.A.Host()
			                            .ComposeUsingRoot<Root>()
			                            .Operations()
			                            .Run();

			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Hello World from Root!");
		}

		[Fact]
		public async Task VerifyConfiguration()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Hello World!");
		}

		[Fact]
		public async Task VerifyDecoration()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .ComposeUsing(x => x.Decorate<string>((_, s) => $"Decorated: {s}"))
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Decorated: Hello World!");
		}

		[Fact]
		public async Task VerifyDecorationViaRoot()
		{
			using var host = await Start.A.Host()
			                            .Configure(x => x.AddSingleton("Hello World!"))
			                            .ComposeUsingRoot<Decorate>()
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<string>()
			    .Should()
			    .Be("Decorated: Hello World!");
		}

		[Fact]
		public async Task VerifyDevelopmentEnvironmentalRegistration()
		{
			using var host = await Start.A.Host()
			                            .WithEnvironment("Development")
			                            .WithDefaultComposition()
			                            .RegisterModularity()
			                            .Configure(x => x.Start<IHelloWorld>().UseEnvironment().Singleton())
			                            .Operations()
			                            .Run();
			host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<HelloWorld>();
		}

		[Fact]
		public async Task VerifyEnvironmentalRegistration()
		{
			{
				using var host = await Start.A.Host()
				                            .WithEnvironment("Production")
				                            .WithDefaultComposition()
				                            .RegisterModularity()
				                            .Configure(x => x.Start<IHelloWorld>().UseEnvironment().Singleton())
				                            .Operations()
				                            .Run();
				host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<Environment.HelloWorld>();
			}

			{
				using var host = await Start.A.Host()
				                            .WithDefaultComposition()
				                            .RegisterModularity()
				                            .Configure(x => x.Start<IHelloWorld>().UseEnvironment().Singleton())
				                            .Operations()
				                            .Run();
				host.Services.GetRequiredService<IHelloWorld>().Should().BeOfType<Environment.HelloWorld>();
			}
		}

		[Fact]
		public async Task VerifyUnexistingComponentThrows()
		{
			using var host = await Start.A.Host()
			                     .WithDefaultComposition()
			                     .RegisterModularity()
			                     .Configure(x => x.Start<IDoesNotExist>()
			                                      .Invoking(y => y.UseEnvironment())
			                                      .Should()
			                                      .ThrowExactly<InvalidOperationException>()
			                                      .WithMessage(
			                                                   "Could not locate an external/environmental component type for DragonSpark.Testing.Composition.CompositionTests+IDoesNotExist.  Please ensure there is a primary assembly registered with an applied attribute of type DragonSpark.Runtime.Environment.HostingAttribute, and that there is a corresponding assembly either named <PrimaryAssemblyName>.Environment for environmental-specific components. Please also ensure that the component libraries contains one public type that implements or is of the requested type."))
			                     .Operations()
			                     .Run().ConfigureAwait(false);
		}
	}
}