using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit;
using System;
using DragonSpark.Activation;
using DragonSpark.Extensions;
using Xunit;
using Xunit.Extensions;
using Activator = DragonSpark.Activation.IoC.Activator;

namespace DragonSpark.Testing.Activation
{
	[Freeze( typeof(IActivator), typeof(Activator) )]
	public class FactoryTests
	{
		[Theory, AutoDataCustomization]
		void CreateActivation( FactoryBase<Class> sut )
		{
			var creation = sut.Create();
			Assert.NotNull( creation );
		}

		[Theory, AutoDataCustomization]
		void CreateActivationException( FactoryBase<ClassWithParameter> sut )
		{
			Assert.Throws<MissingMethodException>( () => sut.Create() );
		}

		[Theory, AutoDataCustomization]
		void Create( FactoryBase<Class> sut )
		{
			var factory = sut.To<IFactory>();
			var result = factory.Create( typeof(object) );
			Assert.IsType<Class>( result );

			var @class = factory.Create<Class>();
			Assert.NotNull( @class );
		}

		[Theory, AutoDataCustomization, AssignServiceLocation]
		void CreateLocation( [Frozen]ClassWithParameter item, FactoryBase<ClassWithParameter> sut )
		{
			var creation = sut.Create();
			Assert.NotNull( creation );
			Assert.Equal( item, creation );
		}
	}
}