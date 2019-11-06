using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Runtime.Environment;
using DragonSpark.Testing.Objects;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class DefaultComponentLocatorTests
	{
		interface IInvalid {}

		public class Benchmarks
		{
			readonly static DefaultComponentLocator<string> DefaultComponentLocator =
				DefaultComponentLocator<string>.Default;

			public Benchmarks()
			{
				DefaultComponentLocator.Get();
			}

			[Benchmark]
			public string Measure() => DefaultComponentLocator.Get();
		}

		[Fact]
		void VerifyEnvironment()
		{
			DefaultComponentLocator<IHelloWorld>.Default.Get()
			                                    .GetMessage()
			                                    .Should()
			                                    .Be($"Hello From {PrimaryAssemblyDetails.Default.Get().Configuration}!");
		}

		[Fact]
		void VerifyInvalidEnvironment()
		{
			DefaultComponentLocator<IInvalid>.Default.Invoking(x => x.Get())
			                                 .Should()
			                                 .Throw<InvalidOperationException>();
		}
	}
}