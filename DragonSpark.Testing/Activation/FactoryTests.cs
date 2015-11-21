using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using System;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class FactoryTests
	{
		[Theory, AutoData( typeof(Customizations.Default) ), Test]
		void CreateActivation( Factory<Class> sut )
		{
			var creation = sut.Create<Class>();
			Assert.NotNull( creation );
		}

		[Theory, AutoData( typeof(Customizations.Assigned) )]
		void CreateActivationException( Factory<ClassWithParameter> sut )
		{
			Assert.Throws<MissingMethodException>( () => sut.Create<ClassWithParameter>() );
		}

		[Theory, AutoData( typeof(Customizations.Assigned) )]
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