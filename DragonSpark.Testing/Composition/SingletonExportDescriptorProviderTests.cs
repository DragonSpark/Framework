using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System.Composition;
using System.Composition.Hosting;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class SingletonExportDescriptorProviderTests
	{
		[Fact]
		public void Basic()
		{
			var types = new[] { typeof(Singleton) }.AsApplicationParts();
			var container = new ContainerConfiguration().WithParts( types.AsEnumerable() ).WithProvider( SingletonExportDescriptorProvider.Default ).CreateContainer();
			var export = container.GetExport<Singleton>();
			Assert.Same( Singleton.Default, export );
		}

		[Fact]
		public void Implementation()
		{
			var types = new[] { typeof(Implemented) }.AsApplicationParts();
			var container = new ContainerConfiguration().WithParts( types.AsEnumerable() ).WithProvider( SingletonExportDescriptorProvider.Default ).CreateContainer();
			Assert.Same( Implemented.Default, container.GetExport<ISingleton>() );
			Assert.Same( Implemented.Default, container.GetExport<Implemented>() );
		}

		[Fact]
		public void Many()
		{
			var types = new[] { typeof(Implemented), typeof(AnotherImplemented) }.AsApplicationParts();
			var container = new ContainerConfiguration().WithParts( types.AsEnumerable() ).WithProvider( SingletonExportDescriptorProvider.Default ).CreateContainer();
			var exports = container.GetExports<ISingleton>().Fixed();
			Assert.Contains( Implemented.Default, exports );
			Assert.Contains( AnotherImplemented.Default, exports );
		}

		[Fact]
		public void Source()
		{
			var types = new[] { typeof(Sourced) }.AsApplicationParts();
			var container = new ContainerConfiguration().WithParts( types.AsEnumerable() ).WithProvider( SingletonExportDescriptorProvider.Default ).CreateContainer();
			Assert.Same( Sourced.Default.Get(), container.GetExport<ISingleton>() );
		}

		class Singleton
		{
			[Export]
			public static Singleton Default { get; } = new Singleton();
			Singleton() {}
		}

		interface ISingleton {}

		class Implemented  : ISingleton
		{
			[Export( typeof(ISingleton) )]
			public static Implemented Default { get; } = new Implemented();
			Implemented() {}
		}

		class AnotherImplemented  : ISingleton
		{
			[Export( typeof(ISingleton) )]
			public static AnotherImplemented Default { get; } = new AnotherImplemented();
			AnotherImplemented() {}
		}

		class Sourced  : ISingleton
		{
			[Export( typeof(ISingleton) )]
			public static ISource<ISingleton> Default { get; } = new SuppliedSource<ISingleton>( new Sourced() );
			Sourced() {}
		}
	}
}