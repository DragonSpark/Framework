using DragonSpark.Activation;
using DragonSpark.Activation.Build;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class FactoryTests
	{
		[Theory, Test, SetupAutoData]
		void CreateActivation( ActivateFactory<Class> sut )
		{
			var creation = sut.CreateUsing( typeof(Class) );
			Assert.NotNull( creation );
			Assert.IsType<Class>( creation );
		}

		[Theory, Test, SetupAutoData]
		void Create( [Modest]ConstructFactory<IInterface> sut )
		{
			var factory = sut.To<IFactoryWithParameter>();
			var result = factory.Create( typeof(Class) );
			Assert.IsType<Class>( result );

			/*var @class = factory.Create( null );
			Assert.NotNull( @class );*/
		}
	}																	        
}