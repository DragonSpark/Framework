using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.ComponentModel
{
	public class AttributeProviderTests
	{
		[Theory, AutoData, AssignServiceLocation]
		void ClassAttribute()
		{
			var attribute = typeof(Decorated).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Theory, AutoData, AssignServiceLocation]
		void Decorated()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			Assert.False( typeof(Class).IsDecoratedWith<Attribute>() );
		}

		[Theory, AutoData, AssignServiceLocation]
		void Convention()
		{
			Assert.True( typeof(Convention).IsDecoratedWith<Attribute>() );
			var attribute = typeof(Convention).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute through convention.", attribute.PropertyName );
		}

		[Theory, AutoData, AssignServiceLocation]
		void ConventionProperty()
		{
			var attribute = typeof(Convention).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute through convention.", attribute.PropertyName );
		}

		[Theory, AutoData, AssignServiceLocation]
		void PropertyAttribute()
		{
			var attribute = typeof(Decorated).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute.", attribute.PropertyName );
		}

		[Theory, AutoData, AssignServiceLocation]
		void RelayedPropertyAttribute()
		{
			var attribute = typeof(Relayed).GetProperty( "Property" ).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed property attribute.", attribute.PropertyName );
		}

		
		[Theory, AutoData, AssignServiceLocation]
		void RelayedAttribute()
		{
			var attribute = typeof(Relayed).GetAttribute<Attribute>();
			Assert.Equal( "This is a relayed class attribute.", attribute.PropertyName );
		}
	}
}	