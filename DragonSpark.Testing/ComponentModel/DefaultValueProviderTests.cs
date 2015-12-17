﻿using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Microsoft.Practices.ServiceLocation;
using System;
using Ploeh.AutoFixture.Xunit2;
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

			public bool CanActivate( Type type, string name )
			{
				return true;
			}

			public object Activate( Type type, string name = null )
			{
				var result = locator.GetInstance( type, name );
				return result;
			}

			public bool CanConstruct( Type type, params object[] parameters )
			{
				return true;
			}

			public object Construct( Type type, params object[] parameters )
			{
				var result = locator.GetInstance( type );
				return result;
			}
		}

		[Theory, Test, SetupAutoData]
		void Apply( [Modest]ObjectBuilder sut )
		{
			var current = DateTime.Now;
			var target = new ClassWithDefaultProperties();
			sut.BuildUp( target );

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

			Assert.Equal( "Already Set", target.AlreadySet );
		}
	}
}