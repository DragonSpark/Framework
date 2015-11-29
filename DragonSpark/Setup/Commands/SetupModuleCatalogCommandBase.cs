using System;
using DragonSpark.Modularity;
using DragonSpark.Properties;

namespace DragonSpark.Setup.Commands
{
	public abstract class SetupModuleCatalogCommandBase : SetupCommand
    {
        protected override void Execute( SetupContext context )
        {
            context.Logger.Information( Resources.CreatingModuleCatalog, Priority.Low );
            var catalog = this.CreateModuleCatalog();
            if ( catalog == null )
            {
                throw new InvalidOperationException( Resources.NullModuleCatalogException );
            }

	        context.Register( catalog );

            context.Logger.Information( Resources.ConfiguringModuleCatalog, Priority.Low );
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