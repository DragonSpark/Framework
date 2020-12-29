using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Compose.Extents
{
	public sealed class ExtentsTests
	{
		[Fact]
		public void VerifyCondition()
		{
			var parameter = new object();
			Start.An.Extent.Of.Any.Into.Condition.Always.Get()
			     .Get(parameter)
			     .Should()
			     .BeTrue();
		}

		[Fact]
		public void VerifyGuard()
		{
			Start.A.Selection<string>()
			     .By.Self.Ensure.Output.IsAssigned.Otherwise.Throw()
			     .Get()
			     .Invoking(x => x.Get(null!))
			     .Should()
			     .Throw<InvalidOperationException>();
			Start.A.Selection<string>()
			     .By.Self.Invoking(x => x.Get().Get(null!))
			     .Should()
			     .NotThrow();
		}

		[Fact]
		public void VerifyOnceStriped()
		{
			var count   = 0;
			var counter = new Select<string, int>(_ => count++).Then().OnceStriped().Get();
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
		public void VerifyOnlyOnce()
		{
			var count   = 0;
			var counter = new Select<string, int>(_ => count++).Then().OnlyOnce().Get();
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
		public void VerifySourceDelegated()
		{
			Start.A.Result(6776).Select(x => x.GetType().GetTypeInfo()).Instance().Should().Be(A.Metadata<int>());
		}

		[Fact]
		public void VerifySourceDirect()
		{
			Start.An.Instance(new Instance<string>("Hello World!"))
			     .Then()
			     .Accept()
			     .Type()
			     .Metadata()
			     .Bind()
			     .Instance()
			     .Should()
			     .Be(A.Metadata<string>());
		}
	}
}