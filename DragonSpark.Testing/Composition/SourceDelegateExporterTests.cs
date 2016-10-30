using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.TypeSystem;
using System;
using System.Composition;
using System.Composition.Hosting;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class SourceDelegateExporterTests
	{
		[Fact]
		public void Delegate()
		{
			var parts = typeof(Source).Yield().AsApplicationParts();
			var container = new ContainerConfiguration().WithProvider( SourceDelegateExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var number = container.GetExport<Func<int>>();
			Assert.Equal( 6776, number() );
		}

		[Fact]
		public void Dependency()
		{
			var parts = typeof(Source).Append( typeof(WithDependency) ).AsApplicationParts();
			
			var container = new ContainerConfiguration().WithProvider( SourceDelegateExporter.Default ).WithParts( parts.AsEnumerable() ).CreateContainer();
			var dependency = container.GetExport<WithDependency>();
			Assert.Equal( 6776, dependency.Number );
		}

		[Export]
		class Source : SourceBase<int>
		{
			public override int Get() => 6776;
		}

		[Export]
		class WithDependency
		{
			readonly Func<int> number;

			[ImportingConstructor]
			public WithDependency( Func<int> number )
			{
				this.number = number;
			}

			public int Number => number();
		}
	}
}