using DragonSpark.Composition;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Composition
{
	public class ExportMappingConfiguratorTests
	{
		[Fact]
		public void Verify()
		{
			GetType().Adapt().WithNested().Append( typeof(ExportMappingConfigurator) ).AsApplicationParts();
			var container = ExportSource<ContainerConfigurator>.Default.Get().Aggregate( new ContainerConfiguration(), ( current, configurator ) => configurator.Get( current ) ).CreateContainer();
			var export = container.GetExport<IAdditional>();
			Assert.IsType<Additional>( export );
		}

		[UsedImplicitly]
		sealed class Mappings : ItemSourceBase<ExportMapping>
		{
			[Export( typeof(IEnumerable<ExportMapping>) ), UsedImplicitly]
			public static Mappings Default { get; } = new Mappings();
			Mappings() {}

			protected override IEnumerable<ExportMapping> Yield()
			{
				yield return new ExportMapping<Additional, IAdditional>();
			}
		}

		interface IAdditional {}

		[Export( typeof(IAdditional) )]
		sealed class Additional : IAdditional {}
	}
}
