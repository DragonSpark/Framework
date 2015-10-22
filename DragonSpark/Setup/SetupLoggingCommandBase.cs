using System;
using System.Reflection;
using DragonSpark.Logging;
using DragonSpark.Properties;

namespace DragonSpark.Setup
{
    public abstract class SetupLoggingCommandBase : SetupCommand
    {
        protected override void Execute( SetupContext context )
        {
            var logger = this.CreateLogger();
            if ( logger == null)
            {
                throw new InvalidOperationException(Resources.NullLoggerFacadeException);
            }

            context.Register( logger );

            logger.Log(Resources.LoggerCreatedSuccessfully, Category.Debug, Logging.Priority.Low);
        }

        protected abstract ILoggerFacade CreateLogger();
    }

    public class SetupLoggingCommand<TLoggingFacade> : SetupLoggingCommandBase where TLoggingFacade : ILoggerFacade, new()
    {
        protected override ILoggerFacade CreateLogger()
        {
            var result = new TLoggingFacade();
            return result;
        }
    }

    public class ActivatedSetupLoggingCommandBase : SetupLoggingCommandBase 
    {
        public Type LoggerType { get; set; }

        protected override ILoggerFacade CreateLogger()
        {
            if ( LoggerType == null )
            {
                throw new InvalidOperationException( "LoggerType is null." );
            }

            if ( !typeof(ILoggerFacade).GetTypeInfo().IsAssignableFrom( LoggerType.GetTypeInfo() ) )
            {
                throw new InvalidOperationException( string.Format( "{0} is not of type ILoggerFacade.", LoggerType.Name ) );
            }

            var result = Activator.CreateInstance( LoggerType ) as ILoggerFacade;
            return result;
        }
    }
}