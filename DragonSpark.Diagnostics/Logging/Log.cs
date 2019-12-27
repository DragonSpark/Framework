using DragonSpark.Runtime.Execution;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;

// ReSharper disable TooManyArguments

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class Log<T> : Contextual<ILogger>, ILogger
	{
		public static Log<T> Default { get; } = new Log<T>();

		Log() : base(Logger.Default.Select(ContextSelector<T>.Default).Get) {}

		ILogger ILogger.ForContext(ILogEventEnricher enricher) => Get().ForContext(enricher);

		ILogger ILogger.ForContext(IEnumerable<ILogEventEnricher> enrichers) => Get().ForContext(enrichers);

		ILogger ILogger.ForContext(string propertyName, object value, bool destructureObjects)
			=> Get().ForContext(propertyName, value, destructureObjects);

		ILogger ILogger.ForContext<TSource>() => Get().ForContext<TSource>();

		ILogger ILogger.ForContext(Type source) => Get().ForContext(source);

		void ILogger.Write(LogEvent logEvent)
		{
			Get().Write(logEvent);
		}

		void ILogger.Write(LogEventLevel level, string messageTemplate)
		{
			Get().Write(level, messageTemplate);
		}

		void ILogger.Write<T1>(LogEventLevel level, string messageTemplate, T1 propertyValue)
		{
			Get().Write(level, messageTemplate, propertyValue);
		}

		void ILogger.Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Write(level, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
		                               T2 propertyValue2)
		{
			Get().Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
		{
			Get().Write(level, messageTemplate, propertyValues);
		}

		void ILogger.Write(LogEventLevel level, System.Exception exception, string messageTemplate)
		{
			Get().Write(level, exception, messageTemplate);
		}

		void ILogger.Write<T1>(LogEventLevel level, System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Write(level, exception, messageTemplate, propertyValue);
		}

		void ILogger.Write<T0, T1>(LogEventLevel level, System.Exception exception, string messageTemplate, T0 propertyValue0,
		                           T1 propertyValue1)
		{
			Get().Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Write<T0, T1, T2>(LogEventLevel level, System.Exception exception, string messageTemplate,
		                               T0 propertyValue0,
		                               T1 propertyValue1, T2 propertyValue2)
		{
			Get().Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Write(LogEventLevel level, System.Exception exception, string messageTemplate,
		                   params object[] propertyValues)
		{
			Get().Write(level, exception, messageTemplate, propertyValues);
		}

		bool ILogger.IsEnabled(LogEventLevel level) => Get().IsEnabled(level);

		void ILogger.Verbose(string messageTemplate)
		{
			Get().Verbose(messageTemplate);
		}

		void ILogger.Verbose<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Verbose(messageTemplate, propertyValue);
		}

		void ILogger.Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Verbose(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Verbose(string messageTemplate, params object[] propertyValues)
		{
			Get().Verbose(messageTemplate, propertyValues);
		}

		void ILogger.Verbose(System.Exception exception, string messageTemplate)
		{
			Get().Verbose(exception, messageTemplate);
		}

		void ILogger.Verbose<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Verbose(exception, messageTemplate, propertyValue);
		}

		void ILogger.Verbose<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Verbose<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1,
		                                 T2 propertyValue2)
		{
			Get().Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Verbose(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Verbose(exception, messageTemplate, propertyValues);
		}

		void ILogger.Debug(string messageTemplate)
		{
			Get().Debug(messageTemplate);
		}

		void ILogger.Debug<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Debug(messageTemplate, propertyValue);
		}

		void ILogger.Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Debug(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Debug(string messageTemplate, params object[] propertyValues)
		{
			Get().Debug(messageTemplate, propertyValues);
		}

		void ILogger.Debug(System.Exception exception, string messageTemplate)
		{
			Get().Debug(exception, messageTemplate);
		}

		void ILogger.Debug<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Debug(exception, messageTemplate, propertyValue);
		}

		void ILogger.Debug<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Debug(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Debug<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			Get().Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Debug(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Debug(exception, messageTemplate, propertyValues);
		}

		void ILogger.Information(string messageTemplate)
		{
			Get().Information(messageTemplate);
		}

		void ILogger.Information<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Information(messageTemplate, propertyValue);
		}

		void ILogger.Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Information(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Information(string messageTemplate, params object[] propertyValues)
		{
			Get().Information(messageTemplate, propertyValues);
		}

		void ILogger.Information(System.Exception exception, string messageTemplate)
		{
			Get().Information(exception, messageTemplate);
		}

		void ILogger.Information<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Information(exception, messageTemplate, propertyValue);
		}

		void ILogger.Information<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1)
		{
			Get().Information(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Information<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                     T1 propertyValue1,
		                                     T2 propertyValue2)
		{
			Get().Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Information(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Information(exception, messageTemplate, propertyValues);
		}

		void ILogger.Warning(string messageTemplate)
		{
			Get().Warning(messageTemplate);
		}

		void ILogger.Warning<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Warning(messageTemplate, propertyValue);
		}

		void ILogger.Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Warning(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Warning(string messageTemplate, params object[] propertyValues)
		{
			Get().Warning(messageTemplate, propertyValues);
		}

		void ILogger.Warning(System.Exception exception, string messageTemplate)
		{
			Get().Warning(exception, messageTemplate);
		}

		void ILogger.Warning<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Warning(exception, messageTemplate, propertyValue);
		}

		void ILogger.Warning<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Warning(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Warning<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1,
		                                 T2 propertyValue2)
		{
			Get().Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Warning(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Warning(exception, messageTemplate, propertyValues);
		}

		void ILogger.Error(string messageTemplate)
		{
			Get().Error(messageTemplate);
		}

		void ILogger.Error<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Error(messageTemplate, propertyValue);
		}

		void ILogger.Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Error(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Error(string messageTemplate, params object[] propertyValues)
		{
			Get().Error(messageTemplate, propertyValues);
		}

		void ILogger.Error(System.Exception exception, string messageTemplate)
		{
			Get().Error(exception, messageTemplate);
		}

		void ILogger.Error<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Error(exception, messageTemplate, propertyValue);
		}

		void ILogger.Error<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Error(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Error<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			Get().Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Error(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Error(exception, messageTemplate, propertyValues);
		}

		void ILogger.Fatal(string messageTemplate)
		{
			Get().Fatal(messageTemplate);
		}

		void ILogger.Fatal<T1>(string messageTemplate, T1 propertyValue)
		{
			Get().Fatal(messageTemplate, propertyValue);
		}

		void ILogger.Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Fatal(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			Get().Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Fatal(string messageTemplate, params object[] propertyValues)
		{
			Get().Fatal(messageTemplate, propertyValues);
		}

		void ILogger.Fatal(System.Exception exception, string messageTemplate)
		{
			Get().Fatal(exception, messageTemplate);
		}

		void ILogger.Fatal<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			Get().Fatal(exception, messageTemplate, propertyValue);
		}

		void ILogger.Fatal<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			Get().Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Fatal<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			Get().Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Fatal(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			Get().Fatal(exception, messageTemplate, propertyValues);
		}

		bool ILogger.BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate,
		                                 out IEnumerable<LogEventProperty> boundProperties) => Get()
			.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);

		bool ILogger.BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property)
			=> Get().BindProperty(propertyName, value, destructureObjects, out property);
	}
}