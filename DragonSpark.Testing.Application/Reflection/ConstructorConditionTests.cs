using DragonSpark.Compose;
using DragonSpark.Reflection.Members;
using DragonSpark.Runtime;
using System;
using System.Reflection;
using Xunit;

// ReSharper disable All

namespace DragonSpark.Testing.Application.Reflection
{
	public sealed class ConstructorConditionTests
	{
		sealed class Optional
		{
			public Optional(int number = 123) {}
		}

		[Fact]
		void Verify()
		{
			ConstructorCondition.Default.Get(A.Type<object>().GetConstructor(Empty<Type>.Array));
		}

		[Fact]
		void VerifyOptional()
		{
			ConstructorCondition.Default.Get(A.Type<Optional>().GetConstructors().Only<ConstructorInfo>());
		}
	}
}