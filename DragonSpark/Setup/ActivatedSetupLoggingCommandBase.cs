using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Setup
{
	public class ActivatedSetupLoggingCommandBase : SetupLoggingCommandBase 
	{
		public Type LoggerType { get; set; }

		protected override ILogger CreateLogger()
		{
			if ( LoggerType == null )
			{
				throw new InvalidOperationException( "LoggerType is null." );
			}

			if ( !typeof(ILogger).IsAssignableFrom( LoggerType ) )
			{
				throw new InvalidOperationException( $"{LoggerType.Name} is not of type ILogger." );
			}

			var result = Activator.CreateInstance( LoggerType ) as ILogger;
			return result;
		}
	}
}