using Serilog;
using Serilog.Core;
using Serilog.Events;
using DragonSpark.Runtime.Execution;
using System;
using System.Collections.Generic;

// ReSharper disable TooManyArguments

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class Log<T> : Contextual<ILogger>, ILogger
	{
		public static Log<T> Default { get; } = new Log<T>();

		Log() : base(Logger.Default.Select(ContextSelector<T>.Default).Get) {}

		ILogger ILogger.ForContext(ILogEventEnricher enricher) => this.Get().ForContext(enricher);

		ILogger ILogger.ForContext(IEnumerable<ILogEventEnricher> enrichers) => this.Get().ForContext(enrichers);

		ILogger ILogger.ForContext(string propertyName, object value, bool destructureObjects)
			=> this.Get().ForContext(propertyName, value, destructureObjects);

		ILogger ILogger.ForContext<TSource>() => this.Get().ForContext<TSource>();

		ILogger ILogger.ForContext(Type source) => this.Get().ForContext(source);

		void ILogger.Write(LogEvent logEvent)
		{
			this.Get().Write(logEvent);
		}

		void ILogger.Write(LogEventLevel level, string messageTemplate)
		{
			this.Get().Write(level, messageTemplate);
		}

		void ILogger.Write<T1>(LogEventLevel level, string messageTemplate, T1 propertyValue)
		{
			this.Get().Write(level, messageTemplate, propertyValue);
		}

		void ILogger.Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Write(level, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
		                               T2 propertyValue2)
		{
			this.Get().Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Write(LogEventLevel level, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Write(level, messageTemplate, propertyValues);
		}

		void ILogger.Write(LogEventLevel level, System.Exception exception, string messageTemplate)
		{
			this.Get().Write(level, exception, messageTemplate);
		}

		void ILogger.Write<T1>(LogEventLevel level, System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Write(level, exception, messageTemplate, propertyValue);
		}

		void ILogger.Write<T0, T1>(LogEventLevel level, System.Exception exception, string messageTemplate, T0 propertyValue0,
		                           T1 propertyValue1)
		{
			this.Get().Write(level, exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Write<T0, T1, T2>(LogEventLevel level, System.Exception exception, string messageTemplate,
		                               T0 propertyValue0,
		                               T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Write(LogEventLevel level, System.Exception exception, string messageTemplate,
		                   params object[] propertyValues)
		{
			this.Get().Write(level, exception, messageTemplate, propertyValues);
		}

		bool ILogger.IsEnabled(LogEventLevel level) => this.Get().IsEnabled(level);

		void ILogger.Verbose(string messageTemplate)
		{
			this.Get().Verbose(messageTemplate);
		}

		void ILogger.Verbose<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Verbose(messageTemplate, propertyValue);
		}

		void ILogger.Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Verbose(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Verbose(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Verbose(messageTemplate, propertyValues);
		}

		void ILogger.Verbose(System.Exception exception, string messageTemplate)
		{
			this.Get().Verbose(exception, messageTemplate);
		}

		void ILogger.Verbose<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Verbose(exception, messageTemplate, propertyValue);
		}

		void ILogger.Verbose<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Verbose(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Verbose<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1,
		                                 T2 propertyValue2)
		{
			this.Get().Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Verbose(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Verbose(exception, messageTemplate, propertyValues);
		}

		void ILogger.Debug(string messageTemplate)
		{
			this.Get().Debug(messageTemplate);
		}

		void ILogger.Debug<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Debug(messageTemplate, propertyValue);
		}

		void ILogger.Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Debug(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Debug(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Debug(messageTemplate, propertyValues);
		}

		void ILogger.Debug(System.Exception exception, string messageTemplate)
		{
			this.Get().Debug(exception, messageTemplate);
		}

		void ILogger.Debug<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Debug(exception, messageTemplate, propertyValue);
		}

		void ILogger.Debug<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Debug(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Debug<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			this.Get().Debug(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Debug(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Debug(exception, messageTemplate, propertyValues);
		}

		void ILogger.Information(string messageTemplate)
		{
			this.Get().Information(messageTemplate);
		}

		void ILogger.Information<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Information(messageTemplate, propertyValue);
		}

		void ILogger.Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Information(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Information(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Information(messageTemplate, propertyValues);
		}

		void ILogger.Information(System.Exception exception, string messageTemplate)
		{
			this.Get().Information(exception, messageTemplate);
		}

		void ILogger.Information<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Information(exception, messageTemplate, propertyValue);
		}

		void ILogger.Information<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1)
		{
			this.Get().Information(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Information<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                     T1 propertyValue1,
		                                     T2 propertyValue2)
		{
			this.Get().Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Information(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Information(exception, messageTemplate, propertyValues);
		}

		void ILogger.Warning(string messageTemplate)
		{
			this.Get().Warning(messageTemplate);
		}

		void ILogger.Warning<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Warning(messageTemplate, propertyValue);
		}

		void ILogger.Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Warning(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Warning(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Warning(messageTemplate, propertyValues);
		}

		void ILogger.Warning(System.Exception exception, string messageTemplate)
		{
			this.Get().Warning(exception, messageTemplate);
		}

		void ILogger.Warning<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Warning(exception, messageTemplate, propertyValue);
		}

		void ILogger.Warning<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Warning(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Warning<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                 T1 propertyValue1,
		                                 T2 propertyValue2)
		{
			this.Get().Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Warning(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Warning(exception, messageTemplate, propertyValues);
		}

		void ILogger.Error(string messageTemplate)
		{
			this.Get().Error(messageTemplate);
		}

		void ILogger.Error<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Error(messageTemplate, propertyValue);
		}

		void ILogger.Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Error(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Error(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Error(messageTemplate, propertyValues);
		}

		void ILogger.Error(System.Exception exception, string messageTemplate)
		{
			this.Get().Error(exception, messageTemplate);
		}

		void ILogger.Error<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Error(exception, messageTemplate, propertyValue);
		}

		void ILogger.Error<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Error(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Error<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			this.Get().Error(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Error(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Error(exception, messageTemplate, propertyValues);
		}

		void ILogger.Fatal(string messageTemplate)
		{
			this.Get().Fatal(messageTemplate);
		}

		void ILogger.Fatal<T1>(string messageTemplate, T1 propertyValue)
		{
			this.Get().Fatal(messageTemplate, propertyValue);
		}

		void ILogger.Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Fatal(messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
		{
			this.Get().Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Fatal(string messageTemplate, params object[] propertyValues)
		{
			this.Get().Fatal(messageTemplate, propertyValues);
		}

		void ILogger.Fatal(System.Exception exception, string messageTemplate)
		{
			this.Get().Fatal(exception, messageTemplate);
		}

		void ILogger.Fatal<T1>(System.Exception exception, string messageTemplate, T1 propertyValue)
		{
			this.Get().Fatal(exception, messageTemplate, propertyValue);
		}

		void ILogger.Fatal<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
		{
			this.Get().Fatal(exception, messageTemplate, propertyValue0, propertyValue1);
		}

		void ILogger.Fatal<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                               T1 propertyValue1,
		                               T2 propertyValue2)
		{
			this.Get().Fatal(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
		}

		void ILogger.Fatal(System.Exception exception, string messageTemplate, params object[] propertyValues)
		{
			this.Get().Fatal(exception, messageTemplate, propertyValues);
		}

		bool ILogger.BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate,
		                                 out IEnumerable<LogEventProperty> boundProperties) => this.Get()
			.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);

		bool ILogger.BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property)
			=> this.Get().BindProperty(propertyName, value, destructureObjects, out property);
	}
}