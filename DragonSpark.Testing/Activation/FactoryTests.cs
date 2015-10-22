using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;
using Activator = DragonSpark.Activation.IoC.Activator;

namespace DragonSpark.Testing.Activation
{
	[Freeze( typeof(IActivator), typeof(Activator) )]
	public class FactoryTests
	{
		[Theory, AutoDataCustomization]
		void CreateActivation( Factory<Class> sut )
		{
			var creation = sut.Create<Class>();
			Assert.NotNull( creation );
		}

		[Theory, AutoDataCustomization]
		void CreateActivationException( Factory<ClassWithParameter> sut )
		{
			Assert.Throws<MissingMethodException>( () => sut.Create<ClassWithParameter>() );
		}

		[Theory, AutoDataCustomization]
		void Create( Factory<Class> sut )
		{
			var factory = sut.To<IFactory>();
			var result = factory.Create( typeof(object) );
			Assert.IsType<Class>( result );

			var @class = factory.Create<Class>();
			Assert.NotNull( @class );
		}
	}
}