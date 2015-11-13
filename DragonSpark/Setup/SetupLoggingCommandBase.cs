using System;
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
}