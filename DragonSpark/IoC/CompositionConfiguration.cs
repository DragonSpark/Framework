using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition.Hosting;

namespace DragonSpark.IoC
{
	public static class DirectoryCatalogExtensions
	{
		public static DirectoryCatalog Validate( this DirectoryCatalog @this )
		{
			var catalogs = typeof(DirectoryCatalog).GetField( "_assemblyCatalogs", DragonSparkBindingOptions.AllProperties ).GetValue( @this ).To<Dictionary<string, AssemblyCatalog>>();
			var collection = typeof(DirectoryCatalog).GetField( "_catalogCollection", DragonSparkBindingOptions.AllProperties ).GetValue( @this ).To<ICollection<ComposablePartCatalog>>();
			var assemblies = collection.OfType<AssemblyCatalog>().ToArray();
			catalogs.Where( y => !y.Value.Assembly.IsValid() ).ToArray().Apply( y =>
			{
				catalogs.Remove( y.Key );
				assemblies.FirstOrDefault( z => y.Value == z ).NotNull( z => collection.Remove( z ) );
			} );
			return @this;
		}
	}

	public class CompositionConfiguration : IContainerConfigurationCommand
	{
		public void Configure( IUnityContainer container )
		{
			var composition = container.AddNewExtension<CompositionExtension>().Configure<CompositionExtension>().CompositionContainer;
			container.RegisterInstance( composition );
			DirectorySearchPath.NullIfEmpty().NotNull( x =>
			{
				var directoryCatalog = new DirectoryCatalog( x ).Validate();
				var composablePartCatalogs = container.Configure<CompositionIntegration>().Catalogs;
				composablePartCatalogs.Add( directoryCatalog );

				AssemblySupport.Instance.Register( y => y.Where( z => z.IsValid() ).Select( z => new AssemblyCatalog( z ) ).Apply( z => container.RegisterCatalog( z ) ), false );
		    } );
		}

		public string DirectorySearchPath { get; set; }
	}
}