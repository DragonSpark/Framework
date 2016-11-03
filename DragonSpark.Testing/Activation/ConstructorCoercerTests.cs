using DragonSpark.Activation;
using DragonSpark.Testing.Objects;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class ConstructorCoercerTests
	{
		[Theory, AutoData]
		public void Construct( ConstructorCoercer sut )
		{
			var parameter = sut.Get( typeof(Class) );
			Assert.Equal( parameter.RequestedType, typeof(Class) );
		} 

		[Theory, AutoData]
		void Parameter( ConstructCoercer<IntegerParameter> sut, int item )
		{
			var parameter = sut.Get( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.SomeInteger, item );
			
		}

		[Theory, AutoData]
		void ConstructParameter( ConstructorCoercer sut, Type item )
		{
			var parameter = sut.Get( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.RequestedType, item );
		}

		[UsedImplicitly]
		class IntegerParameter
		{
			public IntegerParameter( int someInteger )
			{
				SomeInteger = someInteger;
			}

			public int SomeInteger { get; }
		}
	}
}