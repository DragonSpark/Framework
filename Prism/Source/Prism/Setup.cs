using Prism.Logging;
using Prism.Modularity;
using Prism.Properties;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Prism
{
    public class Setup<TLoggingFacade, TModuleCatalog> : Setup
        where TLoggingFacade : ILoggerFacade, new()
        where TModuleCatalog : IModuleCatalog, new()
    {
        protected Setup()
        {
            Commands.Add( new SetupLoggingCommand<TLoggingFacade>() );
            Commands.Add( new SetupModuleCatalogCommand<TModuleCatalog>() );
        }
    }

    /// <summary>
    /// Base class that provides a basic bootstrapping sequence and hooks
    /// that specific implementations can override
    /// </summary>
    /// <remarks>
    /// This class must be overridden to provide application specific configuration.
    /// </remarks>
    public class Setup : ISetup
    {
        public Collection<ICommand> Commands
        {
            get { return commands; }
        }	readonly Collection<ICommand> commands = new Collection<ICommand>();

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="arguments"></param>
        public virtual void Run(object arguments = null)
        {
            var context = CreateContext( arguments );

            foreach ( var command in DetermineCommands( context ) )
            {
                command.Execute( context );
            }
            
            context.Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        protected virtual IEnumerable<ICommand> DetermineCommands( SetupContext context )
        {
            return Commands.Where( command => command.CanExecute( context ) );
        }

        protected virtual SetupContext CreateContext( object arguments )
        {
            var result = new SetupContext( arguments );
            return result;
        }
    }
}
