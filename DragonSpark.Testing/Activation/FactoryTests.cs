using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Testing.TestObjects;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Activation
{
	public class FactoryTests
	{
		[Theory, Framework.AutoData]
		void CreateActivation( Factory<Class> sut )
		{
			var creation = sut.Create();
			Assert.NotNull( creation );
		}

		[Theory, Framework.AutoData]
		void CreateActivationException( Factory<ClassWithParameter> sut )
		{
			Assert.Throws<MissingMethodException>( () => sut.Create() );
		}

		[Theory, Framework.AutoData]
		void Create( Factory<Class> sut )
		{
			var factory = sut.To<IFactory>();
			var result = factory.Create( typeof(object), null );
			Assert.IsType<Class>( result );

			var @class = factory.Create<Class>();
			Assert.NotNull( @class );
		}

		[Theory, Framework.AutoData, AssignServiceLocation]
		void CreateLocation( [Frozen]ClassWithParameter item, Factory<ClassWithParameter> sut )
		{
			var creation = sut.Create();
			Assert.NotNull( creation );
			Assert.Equal( item, creation );
		}
	}
}