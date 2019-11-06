using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Execution;
using Xunit;

// ReSharper disable All

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class TypesTests
	{
		sealed class Types : ArrayInstance<Type>
		{
			public Types(Counter counter, IEnumerable<Type> types) : base(types.Select(counter.Parameter)) {}
		}

		[Fact]
		void Count()
		{
			var counter = new Counter();
			var source  = typeof(object).Yield();
			counter.Get().Should().Be(0);
			var types = new Types(counter, source);
			counter.Get().Should().Be(1);
			var registered = DragonSpark.Runtime.Environment.Types.Default;
			registered.Execute(types);
			counter.Get().Should().Be(1);
			registered.Get().Open().Should().Equal(source);
			counter.Get().Should().Be(1);
			registered.Get().Open().Should().Equal(source);

			registered.Get().Open().Should().Equal(registered.Get().Open());
			types.Get().Should().BeEquivalentTo(types.Get());
			counter.Get().Should().Be(1);
		}

		[Fact]
		void Verify()
		{
			var types = DragonSpark.Runtime.Environment.Types.Default;
			types.Execute(GetType().Yield().ToArray());

			types.Get().Open().Should().HaveCount(1);
			types.Execute(default);

			types.Get().Open().Should().HaveCountGreaterThan(1);
		}
	}
}