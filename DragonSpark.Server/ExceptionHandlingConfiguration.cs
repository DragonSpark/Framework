using DragonSpark.Diagnostics;
using DragonSpark.Runtime;

namespace DragonSpark.Server
{
	public class ExceptionHandlingConfiguration : DragonSpark.IoC.Commands.ExceptionHandlingConfiguration
	{
		protected override void ConfigureExceptionHandling( IExceptionHandler handler )
		{
			base.ConfigureExceptionHandling( handler );
			// ServerContext.Current.ApplicationInstance.Error += ( s, a ) => ServerContext.Current.Error.As<Exception>( handler.Process );
		}
	}
}