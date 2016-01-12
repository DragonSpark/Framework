using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class RegistrationSupportTests
	{
		[Theory, Framework.Setup.AutoData]
		public void Mapping( [Factory, Frozen( Matching.ImplementedInterfaces )]UnityContainer sut, RegistrationSupport support )
		{
			Assert.Null( sut.TryResolve<IInterface>() );
			support.Mapping<IInterface, Class>();

			Assert.IsType<Class>( sut.TryResolve<IInterface>() );
		} 
	}
}