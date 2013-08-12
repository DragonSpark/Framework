using System;
using System.Web;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Server
{
	public class ExceptionHandlingConfiguration : DragonSpark.Configuration.ExceptionHandlingConfiguration
	{
		protected override void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			base.ConfigureExceptionHandling( handler );
			HttpContext.Current.ApplicationInstance.Error += ( s, a ) => HttpContext.Current.Error.As<Exception>( handler.Process );
		}
	}
}