using DragonSpark.Diagnostics;
using DragonSpark.Properties;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;
using System;
using DragonSpark.Setup.Registration;

namespace DragonSpark.Activation.IoC
{
	// [Persistent]
	class ResolutionContext
	{
		readonly IMessageLogger messageLogger;

		public ResolutionContext( [Required]IMessageLogger messageLogger )
		{
			this.messageLogger = messageLogger;
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
				messageLogger.Exception( string.Format( Resources.Activator_CouldNotActivate, e.TypeRequested, e.NameRequested ?? Resources.Activator_None ), e );
				return null;
			}
		}
	}
}