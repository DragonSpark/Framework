using System;
using System.Reflection;
using DragonSpark.Logging;
using DragonSpark.Modularity;
using DragonSpark.Properties;

namespace DragonSpark.Setup
{
	public abstract class SetupModuleCatalogCommandBase : SetupCommand
    {
        protected override void Execute( SetupContext context )
        {
            context.Logger.Log( Resources.CreatingModuleCatalog, Category.Debug, Logging.Priority.Low );
            var catalog = this.CreateModuleCatalog();
            if ( catalog == null )
            {
                throw new InvalidOperationException( Resources.NullModuleCatalogException );
            }

	        context.Register( catalog );

            context.Logger.Log( Resources.ConfiguringModuleCatalog, Category.Debug, Logging.Priority.Low );
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