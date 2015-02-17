using DragonSpark.Diagnostics;

namespace DragonSpark.Server.Legacy
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