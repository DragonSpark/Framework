using System;
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

	public class SetupModuleCatalogCommand<TModuleCatalog> : SetupModuleCatalogCommandBase where TModuleCatalog : IModuleCatalog, new()
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            var result = new TModuleCatalog();
            return result;
        }
    }
}