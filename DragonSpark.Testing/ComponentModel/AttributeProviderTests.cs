using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class AttributeProviderTests
	{
		[Fact]
		void ClassAttribute()
		{
			var attribute = typeof(Decorated).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Fact]
		void Decorated()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			Assert.False( typeof(Class).IsDecoratedWith<Attribute>() );
		}

		[Fact]
		void Convention()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			var attribute = typeof(Convention).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute through convention.", attribute.PropertyName );
		}

		[Fact]
		void ConventionProperty()
		{
			var attribute = typeof(Convention).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute through convention.", attribute.PropertyName );
		}

		[Fact]
		void PropertyAttribute()
		{
			var attribute = typeof(Decorated).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute.", attribute.PropertyName );
		}

		[Theory, Test, SetupAutoData]
		void RelayedPropertyAttribute()
		{
			var attribute = typeof(Relayed).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed property attribute.", attribute.PropertyName );
		}

		
		[Theory, Test, SetupAutoData]
		void RelayedAttribute()
		{
			var attribute = typeof(Relayed).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed class attribute.", attribute.PropertyName );
		}
	}
}	