using Microsoft.Practices.Prism.Logging;

namespace DragonSpark.Logging
{
    public interface ILoggingParameterTranslator
    {
        Category Translate( string category );

        Microsoft.Practices.Prism.Logging.Priority Translate( Priority priority );
    }
}