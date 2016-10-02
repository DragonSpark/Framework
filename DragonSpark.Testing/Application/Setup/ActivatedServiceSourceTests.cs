using DragonSpark.Activation.Location;
using DragonSpark.Application.Setup;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System;
using Xunit;

namespace DragonSpark.Testing.Application.Setup
{
	public class ActivatedServiceSourceTests
	{
		[Fact]
		public void RecursionCheck()
		{
			var count = 0;
			IParameterizedSource<Type, object> sut = null;
			var provider = new DecoratedActivator( type => true, type =>
														 {
															 ++count;
															 if ( type == typeof(int) )
																 return count;

															 if ( count > 3 )
															 {
																 throw new InvalidOperationException( "Recursion detected" );
															 }

															 // ReSharper disable once AccessToModifiedClosure
															 return sut.Get( type );
														 } );

			sut = new ActivatedServiceSource( provider );
			var first = sut.Get( typeof(int) );
			Assert.Null( first );
			ServicesEnabled.Default.Assign( true );
			Assert.Equal( 0, count );
			Assert.Equal( 1,  sut.Get( typeof(int) ) );
			Assert.Equal( 1, count );

			Assert.Null( sut.Get( GetType() ) );
			Assert.Equal( 2, count );
		}
	}
}