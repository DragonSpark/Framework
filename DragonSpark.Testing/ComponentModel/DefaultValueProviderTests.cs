using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using System;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	[Freeze( typeof(IActivator), typeof(Activator) )]
	public class DefaultValueProviderTests
	{
		public class Activator : IActivator
		{
			readonly IServiceLocator locator;

			public Activator( IServiceLocator locator )
			{
				this.locator = locator;
			}

			public bool CanActivate( Type type, string name )
			{
				return true;
			}

			public TResult CreateInstance<TResult>( Type type, string name )
			{
				var result = (TResult)locator.GetInstance( type, name );
				return result;
			}

			public TResult Create<TResult>( params object[] parameters )
			{
				var result = (TResult)locator.GetInstance( typeof(TResult) );
				return result;
			}
		}

		[Theory, AutoDataCustomization, Framework.Services]
		void Apply( DefaultValueProvider sut )
		{
			var current = DateTime.Now;
			var target = new ClassWithDefaultProperties();
			sut.Apply( target );

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

			Assert.NotEqual( Guid.Empty, target.Guid );

			Assert.Equal( "Already Set", target.AlreadySet );
		}
	}
}