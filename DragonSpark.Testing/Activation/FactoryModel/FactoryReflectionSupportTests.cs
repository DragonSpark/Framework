using DragonSpark.Activation.FactoryModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.Activation.FactoryModel
{
	public class FactoryReflectionSupportTests
	{
		[Theory, Test, SetupAutoData]
		public void GetResultType()
		{
			var type = FactoryReflectionSupport.Instance.GetFactoryType( typeof(YetAnotherClass) );
			Assert.Equal( typeof(FactoryOfYAC), type );
		} 
	}
}