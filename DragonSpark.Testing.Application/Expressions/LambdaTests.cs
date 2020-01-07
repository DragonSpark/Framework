using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Runtime.Invocation.Expressions;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Expressions
{
	public sealed class LambdaTests
	{
		[Fact]
		void Verify()
		{
			Lambda<Func<object>>
				.Default
				.Get(ConstructorExpressions.Default.Get(A.Type<object>().GetConstructor(Empty<Type>.Array)))
				.Compile()()
				.Should()
				.NotBeNull();
		}
	}
}