using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Xunit;

namespace DragonSpark.Windows.Testing.Setup.Registration
{
	public class LifetimeFactoryParameterCoercerTests
	{
		[Fact]
		public void ConstructorDefault()
		{
			var type = typeof(TransientLifetimeManager);
			var sut = new LifetimeFactoryParameterCoercer(type);
			var parameter = sut.Coerce( typeof(Class) );
			Assert.Equal( type, parameter.Type );
		}

		[Fact]
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