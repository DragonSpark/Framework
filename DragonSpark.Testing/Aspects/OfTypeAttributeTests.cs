using DragonSpark.Aspects;
using DragonSpark.Testing.Objects;
using System;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class OfTypeAttributeTests
	{
		[Fact]
		public void OfType()
		{
			Assert.Throws<ArgumentException>( () => new MyClass( typeof(OfTypeAttributeTests) ).Type );
		}

		class MyClass
		{
			public Type Type { get; }

			public MyClass( [OfType( typeof(IInterface) )]Type type )
			{
				Type = type;
			}
		}
	}
}