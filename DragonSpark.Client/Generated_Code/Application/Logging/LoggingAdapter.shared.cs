using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Application.Logging
{
	[Singleton( typeof(ILogger), Priority = Priority.Lowest )]
	public class LoggingAdapter : ILogger
	{
		readonly ILoggerFacade facade;
		readonly ILoggingParameterTranslator translator;

		public LoggingAdapter( ILoggerFacade facade, ILoggingParameterTranslator translator )
		{
			this.facade = facade;
			this.translator = translator;
		}

		public void Write( string message, string category, Priority priority )
		{
			var translatedCategory = translator.Translate( category );
			var translatedPriority = translator.Translate( priority );
			facade.Log( message, translatedCategory, translatedPriority );
		}
	}
}