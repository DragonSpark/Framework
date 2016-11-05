using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem.Metadata;
using JetBrains.Annotations;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Attribute = DragonSpark.Testing.Objects.Attribute;

namespace DragonSpark.Testing.TypeSystem.Metadata
{
	public class AttributesTests
	{
		public string PropertyName { get; set; }

		[SuppressMessage( "ReSharper", "ValueParameterNotUsed" )]
		public string Setter { set {} }

		[Fact]
		public void SameInstances()
		{
			var propertyInfo = GetType().GetProperty( nameof( PropertyName ) );

			var sut = Attributes.Get( propertyInfo );
			Assert.Same( sut, Attributes.Get( propertyInfo ) );

			var firstAll = sut.GetAttributes<Attribute>();
			var secondAll = sut.GetAttributes<Attribute>();
			Assert.Same( firstAll, secondAll );
		}

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
			var attribute = typeof(Convention).GetProperty( nameof(Objects.Convention.Property) ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute through convention.", attribute.PropertyName );
		}

		[Fact]
		void PropertyAttribute()
		{
			var attribute = typeof(Decorated).GetProperty( nameof(Objects.Decorated.Property) ).GetAttribute<Attribute>();
			Assert.Equal( "This is a property attribute.", attribute.PropertyName );
		}

		[Fact]
		public void SetterDoesApply()
		{
			var property = GetType().GetProperty( nameof(Setter) );
			Assert.False( DefaultValuePropertySpecification.Default.IsSatisfiedBy( property ) );
		}

		[Fact]
		public void ObjectProvider()
		{
			var instance = new Class();
			var provider = Attributes.Get( instance );
			Assert.NotNull( provider );
		}

		[Fact]
		public void ConstructorClass()
		{
			var type = typeof(ClassWithConstructor);
			var provider = Attributes.Get( type );
			var attribute = provider.GetAttribute<ConstructorAttribute>();
			Assert.Null( attribute );
		}

		[Fact]
		public void Constructor()
		{
			var constructor = typeof(ClassWithConstructor).GetConstructor( Type.EmptyTypes );
			var provider = Attributes.Get( constructor );
			var attribute = provider.GetAttribute<ConstructorAttribute>();
			Assert.NotNull( attribute );
			Assert.Equal( "DefaultConstructor", attribute.PropertyName );
		}

		[Fact]
		public void ConstructorMetadata()
		{
			var constructor = typeof(ClassWithConstructor).GetConstructor( new []{ typeof(int) } );
			var provider = Attributes.Get( constructor );
			var attribute = provider.GetAttribute<ConstructorAttribute>();
			Assert.NotNull( attribute );
			Assert.Equal( "With Number", attribute.PropertyName );
		}

		[Fact]
		public void Inheritance()
		{
			var provider = Attributes.Get( typeof(DerivedClass) );
			var attribute = provider.GetAttribute<Attribute>();
			Assert.Equal( "This is the base class", attribute.PropertyName );
		}

		class ClassWithConstructor
		{
			[Constructor( "DefaultConstructor" ), UsedImplicitly]
			public ClassWithConstructor() {}


			[UsedImplicitly]
			public ClassWithConstructor( int number ) {}
		}

		[UsedImplicitly]
		class ClassWithConstructorMetadata
		{
			[Constructor( "With Number" )]
			public ClassWithConstructorMetadata( int number ) {}
		}

		[AttributeUsage( AttributeTargets.Constructor )]
		class ConstructorAttribute : System.Attribute
		{
			public ConstructorAttribute( string propertyName )
			{
				PropertyName = propertyName;
			}

			public string PropertyName { get; }
		}

		[Attribute( "This is the base class" )]
		class BaseClass {}

		[UsedImplicitly]
		class DerivedClass : BaseClass {}
	}
}