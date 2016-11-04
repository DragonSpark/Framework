using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using DragonSpark.Windows.Legacy.Entity;
using JetBrains.Annotations;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Xunit;
// ReSharper disable ValueParameterNotUsed

namespace DragonSpark.Testing.Extensions
{
	public class ObjectExtensionsTests
	{
		[Fact]
		public void Convert()
		{
			var temp = "123";
			var converted = temp.ConvertTo<int>();
			Assert.Equal( 123, converted );
		}

		[Fact]
		public void GetMemberInfo()
		{
			var info = Check( parameter => parameter.Parameter );
			Assert.Equal( nameof(ClassWithParameter.Parameter), info.Name );
		}

		static MemberInfo Check( Expression<Func<ClassWithParameter, object>> expression ) => expression.GetMemberInfo();

		[Theory, AutoData]
		void Ignored( ClassWithProperties sut )
		{
			var other = sut.MapInto<ClassWithDifferentProperties>();
			Assert.Equal( 0, other.PropertyOne );
			Assert.Null( other.PropertyTwo );
			Assert.Equal( sut.PropertyThree, other.PropertyThree );
			Assert.Equal( sut.PropertyFour, other.PropertyFour );
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
		void Mapper( Objects.ClassWithProperties instance )
		{
			var mapped = instance.MapInto<ClassWithProperties>();
			Assert.Equal( instance.PropertyOne, mapped.PropertyOne );
			Assert.Equal( instance.PropertyTwo, mapped.PropertyTwo );
			Assert.Equal( instance.PropertyThree, mapped.PropertyThree );
			Assert.Equal( instance.PropertyFour, mapped.PropertyFour );
		}

		class ClassWithProperties
		{
			public string PropertyOne { get; [UsedImplicitly] set; }
 
			public int PropertyTwo { get; [UsedImplicitly] set; }

			public object PropertyThree { get; [UsedImplicitly] set; }

			public string PropertyFour { get; [UsedImplicitly] set; }

			[UsedImplicitly]
			public string this[ int index ]
			{
				get { return null; }
				set { }
			}

		}

		class ClassWithDifferentProperties
		{
			public int PropertyOne { get; [UsedImplicitly] set; }

			public string PropertyTwo { get; [UsedImplicitly] set; }

			public object PropertyThree { get; [UsedImplicitly] set; }

			public string PropertyFour { get; [UsedImplicitly] set; }
		}

		[Theory, AutoData]
		void TryDispose( Disposable sut )
		{
			Assert.False( sut.Disposed );
			sut.TryDispose();
			Assert.True( sut.Disposed );
		}

		[Theory, AutoData]
		void AsValid( Class sut )
		{
			var applied = false;
			var valid = sut.AsValid<IInterface>( i => applied = true );
			Assert.True( applied );
			Assert.IsType<Class>( valid );
		}

		[Theory, AutoData]
		public void AsInvalid( string sut )
		{
			Assert.Throws<InvalidOperationException>( () => sut.AsValid<int>( i => Assert.True( false ) ) );
		}

		[Fact]
		void DetermineDefault()
		{
			var item = Items<object>.Default;
			Assert.IsType<object[]>( item );
			Assert.Empty( item );
			Assert.Same( item, Items<object>.Default );
			Assert.Same( Items<object>.Default, Enumerable.Empty<object>() );
			var objects = Items<object>.Default;
			Assert.Same( item, objects );

			var ints = Items<int>.Default;
			Assert.Empty( ints );
			Assert.Same( ints, Items<int>.Default );
		}

		[Theory, AutoData]
		void With( ClassWithParameter sut, string message )
		{
			Assert.Equal( sut.Parameter, sut.With( x => x.Parameter, () => message ) );

			object subject = null;
			Assert.Null( subject.With() );
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