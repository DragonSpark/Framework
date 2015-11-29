using System;
using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;

namespace DragonSpark.Activation.IoC
{
	class ResolutionContext
	{
		readonly ILogger logger;

		public ResolutionContext( ILogger logger )
		{
			this.logger = logger;
		}

		public object Execute( Func<object> resolve )
		{
			try
			{
				var result = resolve();
				return result;
			}
			catch ( ResolutionFailedException e )
			{
				logger.Exception( string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None ), e );
				return null;
			}
		}
	}
}