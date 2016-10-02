using DragonSpark.Extensions;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework;
using System;
using Xunit;
using Xunit.Abstractions;
using Delegates = DragonSpark.Runtime.Delegates;

namespace DragonSpark.Testing.Runtime
{
	public class DelegateTests : TestCollectionBase
	{
		public DelegateTests( ITestOutputHelper output ) : base( output ) {}

		[Fact]
		public void DelegateType()
		{
			var sut = new TypeSubject();
			var cache = DragonSpark.Runtime.DelegateType.Default;
			Assert.Equal( typeof(Action), cache.Get( new Action( sut.Command ).Method ) );
			Assert.Equal( typeof(Action<int>), cache.Get( new Action<int>( sut.Command ).Method ) );
			Assert.Equal( typeof(Func<string, DateTime>), cache.Get( new Func<string, DateTime>( sut.Factory ).Method ) );
		}

		[Fact]
		public void GenericDelegate()
		{
			var instance = new Derived();
			var source = new Func<int, bool>( instance.IsSatisfiedBy );
			var method = typeof(GenericBase<>).GetMethod( nameof(GenericBase<int>.IsSatisfiedBy) );
			Assert.NotEqual( source.Method.ToString(), method.ToString() );
			Assert.Equal( source.Method.ToString(), method.AccountForClosedDefinition( instance.GetType() ).ToString() );
			var @delegate = Delegates.Default.Get( instance ).Get( method );
			Assert.Equal( source, @delegate );
		}

		class Derived : GenericBase<int> {}

		abstract class GenericBase<T> : SpecificationBase<T>
		{
			public override bool IsSatisfiedBy( T parameter ) => false;
		}


		class TypeSubject
		{
			public void Command( int number ) {}

			public void Command() {}

			public DateTime Factory( string message ) => DateTime.Now;
		}
	}
}