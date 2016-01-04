using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework.Setup;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class FactoryParameterCoercerTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void Parameter( FactoryParameterCoercer<IntegerParameter> sut, int item )
		{
			var parameter = sut.Coerce( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.SomeInteger, item );
			
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void ConstructParameter( FactoryParameterCoercer<ConstructParameter> sut, Type item )
		{
			var parameter = sut.Coerce( item );
			Assert.NotNull( parameter );
			Assert.Equal( parameter.Type, item );
		}

		[Theory, Framework.Setup.ConfiguredMoqAutoData]
		public void Fixed( [Frozen]Guid guid, [Greedy]FixedFactoryParameterCoercer<Guid> sut, object parameter )
		{
			var result = sut.Coerce( parameter );
			Assert.Equal( guid, result );
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