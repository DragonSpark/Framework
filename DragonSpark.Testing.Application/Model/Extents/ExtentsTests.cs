using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		void VerifyGuard()
		{
			Start.A.Selection<string>()
			     .By.Self.Guard()
			     .Invoking(x => x.Get(null))
			     .Should()
			     .Throw<InvalidOperationException>();
			Start.A.Selection<string>()
			     .By.Self.Invoking(x => x.Get(null))
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyOnceStriped()
		{
			var count   = 0;
			var counter = new Select<string, int>(x => count++).Then().OnceStriped().Get();
			count.Should().Be(0);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld 1");
			count.Should().Be(2);
			counter.Get("HelloWorld 2");
			count.Should().Be(3);
			counter.Get("HelloWorld 2");
			count.Should().Be(3);
		}

		[Fact]
		void VerifyOnlyOnce()
		{
			var count   = 0;
			var counter = new Select<string, int>(x => count++).Then().OnlyOnce().Get();
			count.Should().Be(0);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld");
			count.Should().Be(1);
			counter.Get("HelloWorld 1");
			count.Should().Be(1);
			counter.Get("HelloWorld 2");
			count.Should().Be(1);
		}

		[Fact]
		void VerifySourceDelegated()
		{
			6776.Start().ToSelect().Select(x => x.GetType().GetTypeInfo()).Get().Should().Be(Type<int>.Metadata);
		}

		[Fact]
		void VerifySourceDirect()
		{
			Start.An.Instance(new Instance<string>("Hello World!"))
			     .Then()
			     .Type()
			     .Metadata()
			     .Selector()()
			     .Should()
			     .Be(Type<string>.Metadata);
		}
	}
}