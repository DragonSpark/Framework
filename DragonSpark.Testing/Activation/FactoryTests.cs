using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class FactoryTests
	{
		[Theory, Test, AutoData]
		void CreateActivation( Factory<Class> sut )
		{
			var creation = sut.Create<Class>();
			Assert.NotNull( creation );
		}

		[Theory, Test, AutoData]
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