using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class FactoryParameterCoercerQualifierTests
	{
		[Theory, AutoData]
		public void Construct( ConstructFactoryParameterCoercer<object> sut )
		{
			var parameter = sut.Coerce( typeof(Class) );
			Assert.Equal( parameter.Type, typeof(Class) );
		} 
	}
}