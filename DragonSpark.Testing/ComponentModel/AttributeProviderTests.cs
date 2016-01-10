using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
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
		public void InstanceAttribute()
		{
			var sut = new Decorated { Property = "Hello" };
			Assert.Equal( "Hello", sut.Property );
			var attribute = sut.GetType().GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Fact]
		void Decorated()
		{
			Assert.True( typeof(Convention).Has<Attribute>() );
			Assert.False( typeof(Class).Has<Attribute>() );
		}

		[Fact]
		void Convention()
		{
			Assert.True( typeof(Convention).Has<Attribute>() );
			var attribute = typeof(Convention).GetAttribute<Attribute>();
			Assert.Equal( "This is a class attribute through convention.", attribute.PropertyName );
		}

		[Fact]
		public void ConventionInstance()
		{
			var sut = new Convention { Property = "Hello" };
			Assert.Equal( "Hello", sut.Property );
		}

		[Fact]
		public void ConventionMetadataInstance()
		{
			var sut = new ConventionMetadata { Property = "Hello" };
			Assert.Equal( "Hello", sut.Property );
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
	}
}	