using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition.Hosting;

namespace DragonSpark.IoC
{
	public class CompositionConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var composition = container.Configure<CompositionExtension>().CompositionContainer;
			container.RegisterInstance( composition );
			OnConfigure( container );
		}

		public string DirectorySearchPath { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Disposed elsewhere." )]
		void OnConfigure( IUnityContainer container )
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