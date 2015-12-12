using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class FactoryParameterCoercerQualifierTests
	{
		[Theory, Test, SetupAutoData]
		public void Construct( [Modest]ConstructFactoryParameterCoercer<object> sut )
		{
			var parameter = sut.Coerce( typeof(Class) );
			Assert.Equal( parameter.Type, typeof(Class) );
		} 
	}
}