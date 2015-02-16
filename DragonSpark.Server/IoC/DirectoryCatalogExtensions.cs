using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.IoC
{
	public static class DirectoryCatalogExtensions
	{
		public static DirectoryCatalog Validate( this DirectoryCatalog @this, Func<Assembly, bool> validator = null )
		{
			var catalogs = typeof(DirectoryCatalog).GetField( "_assemblyCatalogs", DragonSparkBindingOptions.AllProperties ).GetValue( @this ).To<Dictionary<string, AssemblyCatalog>>();
			var collection = typeof(DirectoryCatalog).GetField( "_catalogCollection", DragonSparkBindingOptions.AllProperties ).GetValue( @this ).To<ICollection<ComposablePartCatalog>>();
			var assemblies = collection.OfType<AssemblyCatalog>().ToArray();
			validator = validator ?? ( x => x.IsValid() );
			catalogs.Where( y => !y.Value.Assembly.Transform( validator ) ).ToArray().Apply( y =>
			{
				catalogs.Remove( y.Key );
				assemblies.Where( z => AssemblyName.ReferenceMatchesDefinition( y.Value.Assembly.GetName(), z.Assembly.GetName() ) ).Apply( z => collection.Remove( z ) );
			} );
			return @this;
		}
	}
}