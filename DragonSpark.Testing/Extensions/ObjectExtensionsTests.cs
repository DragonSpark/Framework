using System.ComponentModel;
using System.Diagnostics;
using AutoMapper;
using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using Ploeh.AutoFixture.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Extensions
{
	public class ObjectExtensionsTests
	{
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

		[Fact]
		void ThrowIfNull()
		{
			Class @class = null;
			Assert.Equal( "Value cannot be null.\r\nParameter name: class", Assert.Throws<ArgumentNullException>( () => @class.ThrowIfNull( "class" ) ).Message );
			Assert.Equal( "Value cannot be null.\r\nParameter name: parameter", Assert.Throws<ArgumentNullException>( () => @class.ThrowIfNull() ).Message );

			Assert.DoesNotThrow( () => new Class().ThrowIfNull() );
		}

		[Fact]
		void InvalidIfNull()
		{
			Class @class = null;
			Assert.Equal( "Test message", Assert.Throws<InvalidOperationException>( () => @class.InvalidIfNull( "Test message" ) ).Message );
			Assert.Equal( "This object is null.", Assert.Throws<InvalidOperationException>( () => @class.InvalidIfNull() ).Message );

			Assert.DoesNotThrow( () => new Class().InvalidIfNull() );
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

		[Theory, Framework.AutoData]
		void Enumerate( List<object> sut )
		{
			var items = sut.GetEnumerator().Enumerate().ToList();
			Assert.True( items.Any() && items.All( x => sut.Contains( x ) && sut.ToList().IndexOf( x ) == items.IndexOf( x ) ) );
		}

		[Theory, Framework.AutoData]
		void GetAllPropertyValuesOf( ClassWithProperties sut )
		{
			var expected = new[] { sut.PropertyOne, sut.PropertyFour };

			var values = sut.GetAllPropertyValuesOf<string>();
			Assert.True( expected.Length == values.Count() && expected.All( values.Contains ) );
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

		[Theory, Framework.AutoData]
		void Transform( ClassWithParameter sut, string message )
		{
			Assert.Equal( sut.Parameter, sut.Transform( x => x.Parameter, () => message, parameter => true ) );
			Assert.Equal( message, sut.Transform( x => x.Parameter, () => message, parameter => false ) );
		}

		[Fact]
		public void As()
		{
			Assert.Throws<InvalidOperationException>( () =>
				new Class().As<string, InvalidOperationException>( x => {}, () => new InvalidOperationException( "Not a String" ) )
			);

			var called = false;
			Assert.NotNull( new Class().As<IInterface>( x => called = true ) );
			Assert.True( called );
			Assert.NotNull( new Class().As<IInterface>() );
		}

		[Theory, Framework.AutoData]
		void AsTo( ClassWithParameter sut, string message )
		{
			var value = sut.AsTo<Class, object>( x => x, () => message );
			Assert.Equal( value, message );
		}

		[Theory, Framework.AutoData]
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