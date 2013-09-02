using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Server
{
	public class ExceptionHandlingConfiguration : DragonSpark.Configuration.ExceptionHandlingConfiguration
	{
		protected override void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			base.ConfigureExceptionHandling( handler );
			ServerContext.Current.ApplicationInstance.Error += ( s, a ) => ServerContext.Current.Error.As<Exception>( handler.Process );
		}
	}
}