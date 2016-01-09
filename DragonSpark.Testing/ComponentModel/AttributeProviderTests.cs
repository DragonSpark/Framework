using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using Xunit;

namespace DragonSpark.Testing.ComponentModel
{
	public class AttributeProviderTests
	{
		[Theory, AutoData]
		void ClassAttribute( AttributeProvider sut )
		{
			var attribute = sut.GetAttribute<Attribute>( typeof(Decorated) );
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Theory, AutoData]
		public void InstanceAttribute( AttributeProvider sut )
		{
			var decorated = new Decorated { Property = "Hello" };
			Assert.Equal( "Hello", decorated.Property );
			var attribute = sut.GetAttribute<Attribute>( decorated.GetType() );
			Assert.Equal( "This is a class attribute.", attribute.PropertyName );
		}

		[Theory, AutoData]
		void Decorated( AttributeProvider sut )
		{
			Assert.True( sut.IsDecoratedWith<Attribute>( typeof(Convention) ) );
			Assert.False( sut.IsDecoratedWith<Attribute>( typeof(Class) ) );
		}

		[Fact]
		void Convention( AttributeProvider sut )
		{
			Assert.True( sut.IsDecoratedWith<Attribute>( typeof( Convention ) ) );
			var attribute = sut.GetAttribute<Attribute>( typeof(Convention) );
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

		[Theory, AutoData]
		void ConventionProperty( AttributeProvider sut )
		{
			var attribute = sut.GetAttribute<Attribute>( typeof(Convention).GetProperty( nameof(Objects.Convention.Property) ) );
			Assert.Equal( "This is a property attribute through convention.", attribute.PropertyName );
		}

		[Theory, AutoData]
		void PropertyAttribute( AttributeProvider sut )
		{
			var attribute = sut.GetAttribute<Attribute>( typeof(Decorated).GetProperty( nameof(Objects.Decorated.Property) ) );
			Assert.Equal( "This is a property attribute.", attribute.PropertyName );
		}
	}
}	