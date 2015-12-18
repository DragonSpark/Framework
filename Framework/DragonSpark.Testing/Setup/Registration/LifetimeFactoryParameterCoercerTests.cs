using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Testing.Setup.Registration
{
	public class LifetimeFactoryParameterCoercerTests
	{
		[Theory, Test, SetupAutoData]
		public void ConstructorDefault()
		{
			var type = typeof(TransientLifetimeManager);
			var sut = new LifetimeFactoryParameterCoercer(type);
			var parameter = sut.Coerce( typeof(Class) );
			Assert.Equal( type, parameter.Type );
		}

		[Theory, Test, SetupAutoData]
		public void Constructor()
		{
			var type = typeof(TransientLifetimeManager);
			var sut = new LifetimeFactoryParameterCoercer(type);
			var parameter = sut.Coerce( typeof(Other) );
			Assert.Equal( typeof(ExternallyControlledLifetimeManager), parameter.Type );	
		}

		[LifetimeManager( typeof(ExternallyControlledLifetimeManager) )]
		class Other
		{
		}
	}
}