using System;
using DragonSpark.ComponentModel;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.ComponentModel
{
	public class DefaultValueProviderTests
	{
		[Theory, AutoData, AssignServiceLocation]
		void Apply( DefaultValueProvider sut )
		{
			var current = DateTime.UtcNow;
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

			Assert.True( target.CurrentDateTime > current );
			Assert.True( target.CurrentDateTimeOffset > current );

			Assert.NotNull( target.Activated );

			Assert.IsType<ClassWithParameter>( target.Factory );

			Assert.NotEqual( Guid.Empty, target.Guid );

			Assert.Equal( "Already Set", target.AlreadySet );
		}
	}
}