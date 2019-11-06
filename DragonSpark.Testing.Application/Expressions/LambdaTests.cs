using System;
using FluentAssertions;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Invocation.Expressions;
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
				.Get(ConstructorExpressions.Default.Get(Type<object>.Instance.GetConstructor(Empty<Type>.Array)))
				.Compile()()
				.Should()
				.NotBeNull();
		}
	}
}