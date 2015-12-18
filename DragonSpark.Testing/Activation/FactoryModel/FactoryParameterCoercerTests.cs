using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class FactoryParameterCoercerTests
	{
		[Theory, Test, SetupAutoData]
		void Parameter( [Modest]FactoryParameterCoercer<IntegerParameter> sut, int item )
		{
			var parameter = sut.Coerce( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.SomeInteger, item );
			
		}

		[Theory, Test, SetupAutoData]
		void ConstructParameter( [Modest]FactoryParameterCoercer<ConstructParameter> sut, Type item )
		{
			var parameter = sut.Coerce( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.Type, item );
			
		}

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