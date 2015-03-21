using Prism.Logging;
using Prism.Modularity;
using Prism.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Prism
{
    /// <summary>
    /// Base class that provides a basic bootstrapping sequence and hooks
    /// that specific implementations can override
    /// </summary>
    /// <remarks>
    /// This class must be overridden to provide application specific configuration.
    /// </remarks>
    public class Setup : ISetup
    {
        public Setup()
        {
            commands = new Lazy<Collection<ICommand>>( () => new Collection<ICommand>( DetermineDefaultCommands() ) );
        }

        public Collection<ICommand> Commands
        {
            get { return commands.Value; }
        }	readonly Lazy<Collection<ICommand>> commands;

        protected virtual IList<ICommand> DetermineDefaultCommands()
        {
            var result = new List<ICommand>();
            return result;
        }

        /// <summary>
        /// Run the bootstrapper process.
        /// </summary>
        /// <param name="arguments"></param>
        public virtual void Run(object arguments = null)
        {
            var context = CreateContext( arguments );

            foreach ( var command in DetermineRunCommands( context ) )
            {
                command.Execute( context );
            }
            
            context.Logger.Log(Resources.BootstrapperSequenceCompleted, Category.Debug, Priority.Low);
        }

        protected virtual IEnumerable<ICommand> DetermineRunCommands( SetupContext context )
        {
            return Commands.Where( command => command.CanExecute( context ) ).Select( Prepare );
        }

        protected virtual ICommand Prepare( ICommand command )
        {
            return command;
        }

        protected virtual SetupContext CreateContext( object arguments )
        {
            var result = new SetupContext( arguments );
            return result;
        }
    }

    public class Setup<TLoggingFacade, TModuleCatalog> : Setup
        where TLoggingFacade : ILoggerFacade, new()
        where TModuleCatalog : IModuleCatalog, new()
    {
        protected override IList<ICommand> DetermineDefaultCommands()
        {
            var result = base.DetermineDefaultCommands();
            result.AddRange( new ICommand[]
            {
                new SetupLoggingCommand<TLoggingFacade>(),
                new SetupModuleCatalogCommand<TModuleCatalog>()
            } );
            return result;
        }
    }
}
