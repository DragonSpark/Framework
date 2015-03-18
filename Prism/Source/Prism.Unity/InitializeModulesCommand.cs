using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Properties;
using System;

namespace Prism.Unity
{
	public class InitializeModulesCommand : Prism.InitializeModulesCommand
	{
		protected override IModuleManager DetermineManager( SetupContext context )
		{
			var container = context.Container();
			var result = container.IsRegistered<IModuleManager>() ? Resolve( container ) : null;
			return result;
		}

		static IModuleManager Resolve( IUnityContainer container )
		{
			try
			{
				return container.Resolve<IModuleManager>();
			}
			catch ( ResolutionFailedException ex )
			{
				if ( ex.Message.Contains( "IModuleCatalog" ) )
				{
					throw new InvalidOperationException( Resources.NullModuleCatalogException );
				}

				throw;
			}
		}
	}
}