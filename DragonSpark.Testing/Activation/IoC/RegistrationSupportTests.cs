using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class RegistrationSupportTests
	{
		[Theory, AutoData]
		public void Mapping( [Frozen( Matching.ImplementedInterfaces )]UnityContainer sut, RegistrationSupport support )
		{
			
			Assert.Null( sut.TryResolve<IInterface>() );
			support.Mapping<IInterface, Class>();

			Assert.IsType<Class>( sut.TryResolve<IInterface>() );
		} 
	}
}