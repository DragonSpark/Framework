using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Runtime.Invocation.Expressions;
using FluentAssertions;
using System;
using Xunit;

namespace DragonSpark.Testing.Reflection.Types
{
	public sealed class LambdaTests
	{
		[Fact]
		public void Verify()
		{
			Lambda<Func<object>>.Default
			                    .Get(ConstructorExpressions.Default.Get(A.Type<object>()
			                                                             .GetConstructor(Empty<Type>.Array)
			                                                             .Verify()))
			                    .Compile()()
			                    .Should()
			                    .NotBeNull();
		}
	}
}