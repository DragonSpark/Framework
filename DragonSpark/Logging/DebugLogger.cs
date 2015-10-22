using DragonSpark.Properties;
using System;
using System.Diagnostics;
using System.Globalization;

namespace DragonSpark.Logging
{
	public class DebugLogger : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            string messageToLog = String.Format(CultureInfo.InvariantCulture, Resources.DefaultDebugLoggerPattern, DateTime.Now,
                                                category.ToString().ToUpper(), message, priority);

            Debug.WriteLine(messageToLog);
        }
    }
}
