using System;
using System.Reflection;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime;
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
			ConstructorCondition.Default.Get(Type<object>.Instance.GetConstructor(Empty<Type>.Array));
		}

		[Fact]
		void VerifyOptional()
		{
			ConstructorCondition.Default.Get(Type<Optional>.Instance.GetConstructors().Only<ConstructorInfo>());
		}
	}
}