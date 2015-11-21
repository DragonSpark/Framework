using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using System;

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

            logger.Information(Resources.LoggerCreatedSuccessfully, Priority.Low);
        }

        protected abstract ILogger CreateLogger();
    }
}