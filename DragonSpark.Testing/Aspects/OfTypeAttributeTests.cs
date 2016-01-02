using DragonSpark.Aspects;
using DragonSpark.Testing.Objects;
using PostSharp.Patterns.Contracts;
using System;
using Xunit;

namespace DragonSpark.Testing.Aspects
{
	public class OfTypeAttributeTests
	{
		[Fact]
		public void OfType()
		{
			Assert.Throws<PostconditionFailedException>( () => new MyClass( typeof(OfTypeAttributeTests) ) );
		}

		class MyClass
		{
			readonly Type type;

			public MyClass( [OfType( typeof(IInterface) )]Type type )
			{
				this.type = type;
			}
		}
	}
}