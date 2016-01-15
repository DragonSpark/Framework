using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using DragonSpark.Testing.Objects;
using Microsoft.Practices.ServiceLocation;
using Ploeh.AutoFixture.Xunit2;
using System;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class DefaultValueProviderTests
	{
		public class Activator : IActivator
		{
			readonly IServiceLocator locator;

			public Activator( IServiceLocator locator )
			{
				this.locator = locator;
			}

			public bool CanActivate( Type type, string name ) => true;

			public object Activate( Type type, string name = null ) => locator.GetInstance( type, name );

			public bool CanConstruct( Type type, params object[] parameters ) => true;

			public object Construct( Type type, params object[] parameters ) => locator.GetInstance( type );
		}

		[Theory, AutoData]
		void Apply()
		{
			var current = DateTime.Now;
			var target = new ClassWithDefaultProperties();

			Assert.Equal( 'd', target.Char );
			Assert.Equal( 7, target.Byte );
			Assert.Equal( 8, target.Short );
			Assert.Equal( 9, target.Int );
			Assert.Equal( 6776, target.Long );
			Assert.Equal( 6.7F, target.Float );
			Assert.Equal( 7.1, target.Double );
			Assert.True( target.Boolean );
			Assert.Equal( "Hello World", target.String );
			Assert.Equal( "Legacy", target.Legacy );
			
			Assert.Equal( typeof(ClassWithDefaultProperties), target.Object );

			Assert.NotEqual( DateTime.MinValue, target.CurrentDateTime );
			Assert.NotEqual( DateTimeOffset.MinValue, target.CurrentDateTimeOffset );

			Assert.True( target.CurrentDateTime >= current );
			Assert.True( target.CurrentDateTimeOffset >= current );

			Assert.NotNull( target.Activated );

			Assert.IsType<ClassWithParameter>( target.Factory );

			Assert.NotNull( target.Collection );
			Assert.IsType<Collection<object>>( target.Collection );
			Assert.NotNull( target.Classes );
			Assert.IsType<Collection<Class>>( target.Classes );

			Assert.Equal( 6776, target.ValuedInt );

			Assert.NotEqual( Guid.Empty, target.Guid );

			Assert.Equal( new Guid( "66570344-BA99-4C90-A7BE-AEC903441F97" ), target.ProvidedGuid );

			Assert.Equal( "Already Set", target.AlreadySet );
		}
	}
}