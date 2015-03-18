using Prism.Logging;
using Prism.Modularity;
using Prism.Properties;
using System;
using System.Reflection;

namespace Prism
{
	public abstract class SetupModuleCatalogCommandBase : SetupCommand
    {
        protected override void Execute( SetupContext context )
        {
            context.Logger.Log( Resources.CreatingModuleCatalog, Category.Debug, Priority.Low );
            var catalog = this.CreateModuleCatalog();
            if ( catalog == null )
            {
                throw new InvalidOperationException( Resources.NullModuleCatalogException );
            }

	        context.Register( catalog );

            context.Logger.Log( Resources.ConfiguringModuleCatalog, Category.Debug, Priority.Low );
        }

        protected abstract IModuleCatalog CreateModuleCatalog();
    }

    public class SetupModuleCatalogCommand : SetupModuleCatalogCommand<ModuleCatalog>
    {}

    public class SetupModuleCatalogCommand<TModuleCatalog> : SetupModuleCatalogCommandBase where TModuleCatalog : IModuleCatalog, new()
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var result = new TModuleCatalog();
            return result;
        }
    }

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