using DragonSpark.Extensions;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ObjectExtensionsTests
	{
		[Theory, Test, SetupAutoData]
		public void Evaluate( ClassWithParameter sut )
		{
			Assert.Equal( sut.Parameter, sut.Evaluate<object>( nameof(sut.Parameter) ) );
		}

		[Theory, Test, SetupAutoData]
		void ProvidedValues( ClassWithProperties sut )
		{
			sut.PropertyOne = null;
			var cloned = sut.Clone( Mappings.OnlyProvidedValues() );
			Assert.Null( cloned.PropertyOne );
		}

		[Theory, Test, SetupAutoData]
		void Ignored( ClassWithProperties sut )
		{
			var other = sut.MapInto<ClassWithDifferentProperties>();
			Assert.Equal( 0, other.PropertyOne );
			Assert.Null( other.PropertyTwo );
			Assert.Equal( sut.PropertyThree, other.PropertyThree );
			Assert.Equal( sut.PropertyFour, other.PropertyFour );
		}

		[Theory, Test, SetupAutoData]
		void Clone( ClassWithProperties sut )
		{
			var cloned = sut.Clone();
			Assert.NotSame( sut, cloned );
			Assert.Equal( sut.PropertyOne, cloned.PropertyOne );
			Assert.Equal( sut.PropertyTwo, cloned.PropertyTwo );
			Assert.Equal( sut.PropertyThree, cloned.PropertyThree );
			Assert.Equal( sut.PropertyFour, cloned.PropertyFour );
		}

		[Theory, AutoData]
		public void WithNull( int number )
		{
			var item = new int?( number );
			var count = 0;
			item.With( new Action<int>( i => count++ ) );
			Assert.Equal( 1, count );
			new int?().With( new Action<int>( i => count++ ) );
			Assert.Equal( 1, count );
		}

		[Theory, AutoData]
		void Mapper( TestObjects.ClassWithProperties instance )
		{
			var mapped = instance.MapInto<ClassWithProperties>();
			Assert.Equal( instance.PropertyOne, mapped.PropertyOne );
			Assert.Equal( instance.PropertyTwo, mapped.PropertyTwo );
			Assert.Equal( instance.PropertyThree, mapped.PropertyThree );
			Assert.Equal( instance.PropertyFour, mapped.PropertyFour );
		}

		class ClassWithProperties
		{
			public string PropertyOne { get; set; }
 
			public int PropertyTwo { get; set; }

			public object PropertyThree { get; set; }

			public string PropertyFour { get; set; }

			public string this[ int index ]
			{
				get { return null; }
				set { }
			}

		}

		class ClassWithDifferentProperties
		{
			public int PropertyOne { get; set; }

			public string PropertyTwo { get; set; }

			public object PropertyThree { get; set; }

			public string PropertyFour { get; set; }
		}

		[Fact]
		void ThrowIfNull()
		{
			Class @class = null;
			Assert.Equal( "Value cannot be null.\r\nParameter name: class", Assert.Throws<ArgumentNullException>( () => @class.ThrowIfNull( "class" ) ).Message );
			Assert.Equal( "Value cannot be null.\r\nParameter name: parameter", Assert.Throws<ArgumentNullException>( () => @class.ThrowIfNull() ).Message );

			new Class().ThrowIfNull();
		}

		[Fact]
		void InvalidIfNull()
		{
			Class @class = null;
			Assert.Equal( "Test message", Assert.Throws<InvalidOperationException>( () => @class.InvalidIfNull( "Test message" ) ).Message );
			Assert.Equal( "This object is null.", Assert.Throws<InvalidOperationException>( () => @class.InvalidIfNull() ).Message );

			new Class().InvalidIfNull();
		}

		[Theory, AutoData]
		void TryDispose( Disposable sut )
		{
			Assert.False( sut.Disposed );
			sut.TryDispose();
			Assert.True( sut.Disposed );
		}

		[Fact]
		void Null()
		{
			Class @class = null;

			var called = false;
			@class.Null( () => called = true );
			Assert.True( called );
		}

		[Theory, AutoData]
		void Enumerate( List<object> sut )
		{
			var items = sut.GetEnumerator().Enumerate().ToList();
			Assert.True( items.Any() && items.All( x => sut.Contains( x ) && sut.ToList().IndexOf( x ) == items.IndexOf( x ) ) );
		}

		[Theory, AutoData]
		void GetAllPropertyValuesOf( ClassWithProperties sut )
		{
			var expected = new[] { sut.PropertyOne, sut.PropertyFour };

			var values = sut.GetAllPropertyValuesOf<string>();
			Assert.True( expected.Length == values.Count() && expected.All( x => values.Contains( x ) ) );
		}

		[Fact]
		void DetermineDefault()
		{
			var item = ObjectExtensions.DetermineDefault<IEnumerable<object>>();
			Assert.IsType<object[]>( item );
			Assert.Empty( item );

			Assert.Null( ObjectExtensions.DetermineDefault<object>() );
			Assert.Null( ObjectExtensions.DetermineDefault<Generic<object>>() );
		}

		[Theory, AutoData]
		void With( ClassWithParameter sut, string message )
		{
			Assert.Equal( sut.Parameter, sut.With( x => x.Parameter, () => message ) );
		}

		[Theory, AutoData]
		void WithNullable( int supplied )
		{
			var item = new int?( supplied );
			var value = 0;
			var result = item.With( i => value = i );
			Assert.Equal( supplied, result );
			Assert.Equal( supplied, value );
		}

		[Theory, AutoData]
		void WithSelf( int supplied, string message )
		{
			string item = null;
			Func<int, string> with = i => item = message;
			var result = supplied.WithSelf( with );
			Assert.Equal( message, item );
			Assert.Equal( supplied, result );
		}

		[Fact]
		public void As()
		{
			var called = false;
			Assert.NotNull( new Class().As<IInterface>( x => called = true ) );
			Assert.True( called );
			Assert.NotNull( new Class().As<IInterface>() );
		}

		[Theory, AutoData]
		void AsTo( ClassWithParameter sut, string message )
		{
			var value = sut.AsTo<Class, object>( x => x, () => message );
			Assert.Equal( value, message );
		}

		[Theory, AutoData]
		void ConvertTo( Class sample )
		{
			Assert.Equal( true, "true".ConvertTo<bool>() );
			Assert.Equal( 6776, "6776".ConvertTo<int>() );

			Assert.Equal( BindingDirection.OneWay, "OneWay".ConvertTo<BindingDirection>() );

			Assert.Equal( sample, sample.ConvertTo( typeof(Class) ) );
			Assert.Null( sample.ConvertTo( typeof(ClassWithParameter) ) );
		}

	}
}