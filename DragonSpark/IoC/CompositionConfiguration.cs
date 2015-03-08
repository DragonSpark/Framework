using System.ComponentModel.Composition.Hosting;
using DragonSpark.Extensions;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Unity;

namespace DragonSpark.IoC
{
	public partial class CompositionConfiguration
	{
		public string DirectorySearchPath { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed elsewhere." )]
		partial void OnConfigure( IUnityContainer container )
		{
			DirectorySearchPath.NullIfEmpty().NotNull( x =>
			{
				var directoryCatalog = new DirectoryCatalog( x );
			    var composablePartCatalogs = container.Configure<CompositionIntegration>().Catalogs;
			    composablePartCatalogs.Add( directoryCatalog );
			} );
		}
	}
}