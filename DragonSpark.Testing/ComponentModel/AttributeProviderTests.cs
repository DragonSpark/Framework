using DragonSpark.Activation;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Abstractions;

namespace DragonSpark.Testing.ComponentModel
{
	public class AttributeProviderTests
	{
		readonly ITestOutputHelper output;

		public AttributeProviderTests( ITestOutputHelper output )
		{
			this.output = output;

			output.WriteLine( "HELLO????????" );
		}

		[Theory, AutoDataCustomization, Services]
		void ClassAttribute()
		{
			var attribute = typeof(Decorated).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Theory, AutoDataCustomization, Services]
		void Decorated()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			Assert.False( typeof(Class).IsDecoratedWith<Attribute>() );
		}

		[Theory, AutoDataCustomization, Services]
		void Convention()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			var attribute = typeof(Convention).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute through convention.", attribute.PropertyName );
		}

		[Theory, AutoDataCustomization, Services]
		void ConventionProperty()
		{
			var attribute = typeof(Convention).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute through convention.", attribute.PropertyName );
		}

		[Theory, AutoDataCustomization, Services]
		void PropertyAttribute()
		{
			var attribute = typeof(Decorated).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute.", attribute.PropertyName );
		}

		[Theory, AutoDataCustomization, Services]
		void RelayedPropertyAttribute()
		{
			var attribute = typeof(Relayed).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed property attribute.", attribute.PropertyName );
		}

		
		[Theory, AutoDataCustomization, Services]
		void RelayedAttribute()
		{
			var attribute = typeof(Relayed).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed class attribute.", attribute.PropertyName );
		}
	}
}	