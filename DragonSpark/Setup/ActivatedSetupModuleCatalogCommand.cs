using System;
using System.Reflection;
using DragonSpark.Modularity;

namespace DragonSpark.Setup
{
	public class ActivatedSetupModuleCatalogCommand : SetupModuleCatalogCommandBase 
	{
		public Type ModuleCatalogType { get; set; }

		protected override IModuleCatalog CreateModuleCatalog()
		{
			if ( ModuleCatalogType == null )
			{
				throw new InvalidOperationException( "ModuleCatalogType is null." );
			}

			if ( !typeof(IModuleCatalog).GetTypeInfo().IsAssignableFrom( ModuleCatalogType.GetTypeInfo() ) )
			{
				throw new InvalidOperationException( string.Format( "{0} is not of type IModuleCatalog.", ModuleCatalogType.Name ) );
			}

			var result = Activator.CreateInstance( ModuleCatalogType ) as IModuleCatalog;
			return result;
		}
	}
}