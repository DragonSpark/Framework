using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class FactoryTests
	{
		[Theory, Test, SetupAutoData]
		void CreateActivation( Factory<Class> sut )
		{
			var creation = sut.Create();
			Assert.NotNull( creation );
			Assert.IsType<Class>( creation );
		}

		[Theory, Test, SetupAutoData]
		void Create( Factory<Class> sut )
		{
			var factory = sut.To<IFactory>();
			var result = factory.Create( typeof(object) );
			Assert.IsType<Class>( result );

			var @class = factory.Create( null );
			Assert.NotNull( @class );
		}
	}
}