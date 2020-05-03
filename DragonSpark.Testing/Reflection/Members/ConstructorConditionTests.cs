using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Reflection.Members;
using System;
using Xunit;

// ReSharper disable All

namespace DragonSpark.Testing.Reflection.Members
{
	public sealed class ConstructorConditionTests
	{
		sealed class Optional
		{
			public Optional(int number = 123) => Number = number;

			public int Number { get; }
		}

		[Fact]
		public void Verify()
		{
			ConstructorCondition.Default.Get(A.Type<object>().GetConstructor(Empty<Type>.Array).Verify());
		}

		[Fact]
		public void VerifyOptional()
		{
			ConstructorCondition.Default.Get(A.Type<Optional>().GetConstructors().Only());
		}
	}
}