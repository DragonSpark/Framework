using System;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using DragonSpark.Aspects;
using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Xunit;

namespace DragonSpark.Testing.Application.Aspects
{
	public class AssignedAspectTests
	{
		public class Benchmarks
		{
			readonly ISelect<object, object> _subject;

			public Benchmarks() : this(A.Self<object>()) {}

			public Benchmarks(ISelect<object, object> subject) => _subject = subject;

			[Benchmark(Baseline = true)]
			public object Once() => _subject.Configured();
		}

		[Fact]
		void RuntimeRegistration()
		{
			var single    = new Registration(typeof(AssignedAspect<,>));
			var parameter = new[] {A.Type<string>(), A.Type<string>()};
			single.Get(parameter).Should().BeSameAs(AssignedAspect<string, string>.Default);
		}

		[Fact]
		void Verify()
		{
			AspectRegistry.Default.Execute(new Registration(typeof(AssignedAspect<,>)));

			var subject = Start.A.Selection<string>().By.Self;
			subject.Invoking(x => x.Get(null)).Should().NotThrow();
			subject.Configured()
			       .Invoking(x => x.Get(null))
			       .Should()
			       .Throw<InvalidOperationException>();
		}
	}
}