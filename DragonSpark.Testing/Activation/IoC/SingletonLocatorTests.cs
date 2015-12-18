using DragonSpark.Activation.IoC;
using Xunit;

namespace DragonSpark.Testing.Activation.IoC
{
	public class SingletonLocatorTests
	{
		[Fact]
		public void SingletonFromItem()
		{
			var sut = new SingletonLocator();
			Assert.Same( SingletonItem.Instance, sut.Locate( typeof(SingletonItem) ) );
		}

		[Fact]
		public void SingletonFromMetadata()
		{
			var sut = new SingletonLocator();
			Assert.Same( SingletonMetadata.Temp, sut.Locate( typeof(SingletonMetadata) ) );
		}

		[Fact]
		public void SingletonFromOther()
		{
			var sut = new SingletonLocator( nameof(SingletonOther.Other) );
			Assert.Same( SingletonOther.Other, sut.Locate( typeof(SingletonOther) ) );
		}

		[Fact]
		public void SingletonFromConvention()
		{
			var sut = new SingletonLocator();
			Assert.Same( Singleton.Instance, sut.Locate( typeof(ISingleton) ) );
		}

		class SingletonItem
		{
			public static SingletonItem Instance { get; } = new SingletonItem();
		}

		class SingletonMetadata
		{
			[Singleton]
			public static SingletonItem Temp { get; } = new SingletonItem();
		}

		class SingletonOther
		{
			public static SingletonOther Other { get; } = new SingletonOther();
		}

		class Singleton : ISingleton
		{
			public static Singleton Instance { get; } = new Singleton();
		}

		interface ISingleton
		{}
	}
}