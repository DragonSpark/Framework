using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class ParameterizedSourceDelegateExporterTests
	{
		[Fact]
		public void Delegate()
		{
			var parts = typeof(Source).Yield().AsApplicationParts();
			var container = new ContainerConfiguration().WithProvider( ParameterizedSourceDelegateExporter.Default ).WithParts( parts.ToArray() ).CreateContainer();
			var number = container.GetExport<Func<bool, int>>();
			Assert.Equal( 6777, number( true ) );
		}

		[Fact]
		public void Dependency()
		{
			var parts = typeof(Source).Append( typeof(WithDependency) ).AsApplicationParts();
			var container = new ContainerConfiguration().WithProvider( ParameterizedSourceDelegateExporter.Default ).WithParts( parts.ToArray() ).CreateContainer();
			var dependency = container.GetExport<WithDependency>();
			Assert.Equal( 6775, dependency.Number( false ) );
		}

		[Export]
		class Source : ParameterizedSourceBase<bool, int>
		{
			public override int Get( bool parameter ) => 6776 + ( parameter ? 1 : -1 );
		}

		[Export]
		class WithDependency
		{
			readonly Func<bool, int> number;

			[ImportingConstructor]
			public WithDependency( Func<bool, int> number )
			{
				this.number = number;
			}

			public int Number( bool up ) => number( up );
		}
	}
}