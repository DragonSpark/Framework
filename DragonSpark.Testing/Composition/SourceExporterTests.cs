using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class SourceExporterTests
	{
		[Fact]
		public void BasicExport()
		{
			var parts = typeof(Source).Yield().AsApplicationParts();

			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var number = container.GetExport<int>();
			Assert.Equal( 6776, number );
		}

		[Fact]
		public void Parameterized()
		{
			var parts = typeof(Source).Append( typeof(ParameterizedSource), typeof(Result) ).AsApplicationParts();
			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var dependency = container.GetExport<Result>();
			Assert.Equal( 6776 + 123, dependency.Number );
		}

		[Fact]
		public void Dependency()
		{
			var parts = typeof(Source).Append( typeof(WithDependency) ).AsApplicationParts();

			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var dependency = container.GetExport<WithDependency>();
			Assert.Equal( 6776, dependency.Number );
		}

		[Fact]
		public void ParameterizedDependency()
		{
			var parts = typeof(Source).Append( typeof(WithDependency) ).AsApplicationParts();

			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var dependency = container.GetExport<WithDependency>();
			Assert.Equal( 6776, dependency.Number );
		}

		[Fact]
		public void Shared()
		{
			var parts = typeof(SharedCounter).Yield().AsApplicationParts();

			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			Assert.Equal( 1, container.GetExport<int>() );
			Assert.Equal( 1, container.GetExport<int>() );
		}

		[Fact]
		public void PerRequest()
		{
			var parts = typeof(Counter).Yield().AsApplicationParts();

			var container = new ContainerConfiguration().WithProvider( SourceExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			Assert.Equal( 1, container.GetExport<int>() );
			Assert.Equal( 2, container.GetExport<int>() );
		}

		[Fact]
		public void One()
		{
			var parts = typeof(Counter).Yield().AsApplicationParts();
			var type = ResultTypes.Default.Get( parts.Single() );
			Assert.Equal( typeof(int), type );
		}

		[Fact]
		public void Two()
		{
			var parts = typeof(SharedCounter).Yield().AsApplicationParts();
			var type = ResultTypes.Default.Get( parts.Single() );
			Assert.Equal( typeof(int), type );
		}

		class Count : Scope<int>
		{
			public static Count Default { get; } = new Count();
			Count() {}
		}

		[Export]
		class Source : SourceBase<int>
		{
			public override int Get() => 6776;
		}

		[Export]
		class ParameterizedSource : ParameterizedSourceBase<Source, Result>
		{
			public override Result Get( Source parameter ) => new Result( parameter.Get() + 123 );
		}

		struct Result
		{
			public Result( int number )
			{
				Number = number;
			}

			public int Number { get; }
		}

		[Export]
		class Counter : SourceBase<int>
		{
			public override int Get() => Count.Default.WithInstance( Count.Default.Get() + 1 );
		}

		[Export, Shared]
		class SharedCounter : SourceBase<int>
		{
			public override int Get() => Count.Default.WithInstance( Count.Default.Get() + 1 );
		}

		[Export]
		class WithDependency
		{
			[ImportingConstructor]
			public WithDependency( int number )
			{
				Number = number;
			}

			public int Number { get; }
		}
	}
}