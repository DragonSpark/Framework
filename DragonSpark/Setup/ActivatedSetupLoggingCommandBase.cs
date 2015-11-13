using System;
using System.Reflection;
using DragonSpark.Logging;

namespace DragonSpark.Setup
{
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