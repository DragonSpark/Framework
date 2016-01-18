using System;
using System.Reflection;
using DragonSpark.Activation;
using DragonSpark.Activation.IoC;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.Unity;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class RegistrationSupportTests
	{
		[Theory, Framework.Setup.AutoData]
		public void Mapping( [Factory, Frozen( As = typeof(IUnityContainer) )]UnityContainer sut, TransientServiceRegistry registry )
		{
			Assert.Null( sut.TryResolve<IInterface>() );
			registry.Register<IInterface, Class>();

			var first = sut.TryResolve<IInterface>();
			Assert.IsType<Class>( first );

			Assert.NotSame( first, sut.TryResolve<IInterface>() );
		}

		[Theory, Framework.Setup.AutoData]
		public void Persisting( [Factory, Frozen( As = typeof(IUnityContainer) )]UnityContainer sut, PersistentServiceRegistry registry )
		{
			Assert.Null( sut.TryResolve<IInterface>() );
			registry.Register<IInterface, Class>();

			var first = sut.TryResolve<IInterface>();
			Assert.IsType<Class>( first );

			Assert.Same( first, sut.TryResolve<IInterface>() );
		}
	}
}