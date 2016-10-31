using DragonSpark.Specifications;
using DragonSpark.Testing.Objects;
using DragonSpark.TypeSystem;
using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class TypeAdapterTests
	{
		[Fact]
		public void EnumerableType()
		{
			var item = new TypeAdapter( typeof(List<int>) ).GetEnumerableType();
			Assert.Equal( typeof(int), item );
		}

		[Fact]
		public void GetMappedMethods()
		{
			var mappings = typeof(List<int>).Adapt().GetMappedMethods( typeof(ICollection<int>) );
			Assert.NotEmpty( mappings );
			Assert.Equal( typeof(ICollection<int>), mappings.First().InterfaceMethod.DeclaringType);
			Assert.Equal( typeof(List<int>), mappings.First().MappedMethod.DeclaringType);

			Assert.Empty( typeof(List<int>).Adapt().GetMappedMethods( typeof(ISpecification<int>) ) );
		}

		[Fact]
		public void Coverage_GenericMethods()
		{
			GetType().Adapt().GenericFactoryMethods[nameof( Generic )].Make( typeof(int) );
		}

		public static Type Generic<T>( int number ) where T : IAspect => typeof(T);

		[Fact]
		public void IsInstanceOfType()
		{
			var adapter = new TypeAdapter( typeof(Casted) );
			var instance = new Casted( 6776 );
			Assert.True( adapter.IsInstanceOfType( instance ) );
			Assert.Equal( 6776, instance.Item );
		}

		[Fact]
		public void Throws()
		{
			Assert.Throws<ArgumentNullException>( () => Create() );
		}

		TypeAdapter Create() => new TypeAdapter( null, null );

		class Casted
		{
			public Casted( int item )
			{
				Item = item;
			}

			public int Item { get; }
		}

		[Fact]
		public void Adapt()
		{
			Assert.NotNull( typeof(object).GetTypeInfo().Adapt() );
		}

		[Fact]
		public void GetHierarchy()
		{
			Assert.Equal( new[]{ typeof(Derived), typeof(Class), typeof(object) }, typeof(Derived).Adapt().GetHierarchy( true ).ToArray() );
			Assert.Equal( new[]{ typeof(Derived), typeof(Class) }, typeof(Derived).Adapt().GetHierarchy().ToArray() );
		}

		[Fact]
		public void GetAllInterfaces()
		{
			var interfaces = typeof(Derived).Adapt().GetAllInterfaces().OrderBy( x => x.Name ).ToArray();
			Assert.Equal( new[]{ typeof(IAnotherInterface), typeof(IInterface) }, interfaces );
		}

		[Fact]
		public void GetItemType()
		{
			Assert.Equal( typeof(Class), typeof(List<Class>).Adapt().GetInnerType() );
			Assert.Equal( typeof(Class), typeof(Class[]).Adapt().GetInnerType() );
			Assert.Null( typeof(Class).Adapt().GetInnerType() );
		}
	}
}