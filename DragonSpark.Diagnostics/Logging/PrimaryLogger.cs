using JetBrains.Annotations;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Activation;
using System;
using System.Collections.Generic;

// ReSharper disable TooManyArguments

namespace DragonSpark.Diagnostics.Logging
{
	public sealed class PrimaryLogger : Disposable, IPrimaryLogger, IActivateUsing<ILogger>
	{
		readonly ILogger _logger;

		[UsedImplicitly]
		public PrimaryLogger(ILogger logger) : this(logger, logger.To<IDisposable>()) {}

		public PrimaryLogger(ILogger logger, IDisposable disposable) : base(disposable.Dispose) => _logger = logger;

		public ILogger ForContext(ILogEventEnricher enricher) => _logger.ForContext(enricher);

		public ILogger ForContext(IEnumerable<ILogEventEnricher> enrichers) => _logger.ForContext(enrichers);

		public ILogger ForContext(string propertyName, object value, bool destructureObjects = false) =>
			_logger.ForContext(propertyName, value, destructureObjects);

		public ILogger ForContext<TSource>() => _logger.ForContext<TSource>();

		public ILogger ForContext(Type source) => _logger.ForContext(source);

		public void Write(LogEvent logEvent) => _logger.Write(logEvent);

		public void Write(LogEventLevel level, string messageTemplate) => _logger.Write(level, messageTemplate);

		public void Write<T>(LogEventLevel level, string messageTemplate, T propertyValue) =>
			_logger.Write(level, messageTemplate, propertyValue);

		public void Write<T0, T1>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Write(level, messageTemplate, propertyValue0, propertyValue1);

		public void Write<T0, T1, T2>(LogEventLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1,
		                              T2 propertyValue2) =>
			_logger.Write(level, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Write(LogEventLevel level, string messageTemplate, params object[] propertyValues) =>
			_logger.Write(level, messageTemplate, propertyValues);

		public void Write(LogEventLevel level, System.Exception exception, string messageTemplate) =>
			_logger.Write(level, exception, messageTemplate);

		public void Write<T>(LogEventLevel level, System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Write(level, exception, messageTemplate, propertyValue);

		public void Write<T0, T1>(LogEventLevel level, System.Exception exception, string messageTemplate, T0 propertyValue0,
		                          T1 propertyValue1) =>
			_logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1);

		public void Write<T0, T1, T2>(LogEventLevel level, System.Exception exception, string messageTemplate,
		                              T0 propertyValue0,
		                              T1 propertyValue1, T2 propertyValue2) =>
			_logger.Write(level, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Write(LogEventLevel level, System.Exception exception, string messageTemplate,
		                  params object[] propertyValues) =>
			_logger.Write(level, exception, messageTemplate, propertyValues);

		public bool IsEnabled(LogEventLevel level) => _logger.IsEnabled(level);

		public void Verbose(string messageTemplate) => _logger.Verbose(messageTemplate);

		public void Verbose<T>(string messageTemplate, T propertyValue) => _logger.Verbose(messageTemplate, propertyValue);

		public void Verbose<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Verbose(messageTemplate, propertyValue0, propertyValue1);

		public void Verbose<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Verbose(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Verbose(string messageTemplate, params object[] propertyValues) =>
			_logger.Verbose(messageTemplate, propertyValues);

		public void Verbose(System.Exception exception, string messageTemplate)
			=> _logger.Verbose(exception, messageTemplate);

		public void Verbose<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Verbose(exception, messageTemplate, propertyValue);

		public void Verbose<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
			=>
				_logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Verbose<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                T1 propertyValue1,
		                                T2 propertyValue2) =>
			_logger.Verbose(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Verbose(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Verbose(exception, messageTemplate, propertyValues);

		public void Debug(string messageTemplate) => _logger.Debug(messageTemplate);

		public void Debug<T>(string messageTemplate, T propertyValue) => _logger.Debug(messageTemplate, propertyValue);

		public void Debug<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Debug(messageTemplate, propertyValue0, propertyValue1);

		public void Debug<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Debug(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Debug(string messageTemplate, params object[] propertyValues) =>
			_logger.Debug(messageTemplate, propertyValues);

		public void Debug(System.Exception exception, string messageTemplate) => _logger.Debug(exception, messageTemplate);

		public void Debug<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Debug(exception, messageTemplate, propertyValue);

		public void Debug<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Debug(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Debug<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                              T1 propertyValue1,
		                              T2 propertyValue2) => _logger.Debug(exception, messageTemplate, propertyValue0,
		                                                                  propertyValue1, propertyValue2);

		public void Debug(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Debug(exception, messageTemplate, propertyValues);

		public void Information(string messageTemplate) => _logger.Information(messageTemplate);

		public void Information<T>(string messageTemplate, T propertyValue) =>
			_logger.Information(messageTemplate, propertyValue);

		public void Information<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Information(messageTemplate, propertyValue0, propertyValue1);

		public void
			Information<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Information(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Information(string messageTemplate, params object[] propertyValues) =>
			_logger.Information(messageTemplate, propertyValues);

		public void Information(System.Exception exception, string messageTemplate) =>
			_logger.Information(exception, messageTemplate);

		public void Information<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Information(exception, messageTemplate, propertyValue);

		public void Information<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                T1 propertyValue1) =>
			_logger.Information(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Information<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                    T1 propertyValue1,
		                                    T2 propertyValue2) =>
			_logger.Information(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Information(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Information(exception, messageTemplate, propertyValues);

		public void Warning(string messageTemplate) => _logger.Warning(messageTemplate);

		public void Warning<T>(string messageTemplate, T propertyValue) => _logger.Warning(messageTemplate, propertyValue);

		public void Warning<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Warning(messageTemplate, propertyValue0, propertyValue1);

		public void Warning<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Warning(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Warning(string messageTemplate, params object[] propertyValues) =>
			_logger.Warning(messageTemplate, propertyValues);

		public void Warning(System.Exception exception, string messageTemplate)
			=> _logger.Warning(exception, messageTemplate);

		public void Warning<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Warning(exception, messageTemplate, propertyValue);

		public void Warning<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
			=>
				_logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Warning<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                                T1 propertyValue1,
		                                T2 propertyValue2) =>
			_logger.Warning(exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Warning(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Warning(exception, messageTemplate, propertyValues);

		public void Error(string messageTemplate) => _logger.Error(messageTemplate);

		public void Error<T>(string messageTemplate, T propertyValue) => _logger.Error(messageTemplate, propertyValue);

		public void Error<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Error(messageTemplate, propertyValue0, propertyValue1);

		public void Error<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Error(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Error(string messageTemplate, params object[] propertyValues) =>
			_logger.Error(messageTemplate, propertyValues);

		public void Error(System.Exception exception, string messageTemplate) => _logger.Error(exception, messageTemplate);

		public void Error<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Error(exception, messageTemplate, propertyValue);

		public void Error<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Error(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Error<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                              T1 propertyValue1,
		                              T2 propertyValue2) => _logger.Error(exception, messageTemplate, propertyValue0,
		                                                                  propertyValue1, propertyValue2);

		public void Error(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Error(exception, messageTemplate, propertyValues);

		public void Fatal(string messageTemplate) => _logger.Fatal(messageTemplate);

		public void Fatal<T>(string messageTemplate, T propertyValue) => _logger.Fatal(messageTemplate, propertyValue);

		public void Fatal<T0, T1>(string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Fatal(messageTemplate, propertyValue0, propertyValue1);

		public void Fatal<T0, T1, T2>(string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2) =>
			_logger.Fatal(messageTemplate, propertyValue0, propertyValue1, propertyValue2);

		public void Fatal(string messageTemplate, params object[] propertyValues) =>
			_logger.Fatal(messageTemplate, propertyValues);

		public void Fatal(System.Exception exception, string messageTemplate) => _logger.Fatal(exception, messageTemplate);

		public void Fatal<T>(System.Exception exception, string messageTemplate, T propertyValue) =>
			_logger.Fatal(exception, messageTemplate, propertyValue);

		public void Fatal<T0, T1>(System.Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1) =>
			_logger.Fatal(exception, messageTemplate, propertyValue0, propertyValue1);

		public void Fatal<T0, T1, T2>(System.Exception exception, string messageTemplate, T0 propertyValue0,
		                              T1 propertyValue1,
		                              T2 propertyValue2) => _logger.Fatal(exception, messageTemplate, propertyValue0,
		                                                                  propertyValue1, propertyValue2);

		public void Fatal(System.Exception exception, string messageTemplate, params object[] propertyValues) =>
			_logger.Fatal(exception, messageTemplate, propertyValues);

		public bool BindMessageTemplate(string messageTemplate, object[] propertyValues, out MessageTemplate parsedTemplate,
		                                out IEnumerable<LogEventProperty> boundProperties) =>
			_logger.BindMessageTemplate(messageTemplate, propertyValues, out parsedTemplate, out boundProperties);

		public bool BindProperty(string propertyName, object value, bool destructureObjects, out LogEventProperty property) =>
			_logger.BindProperty(propertyName, value, destructureObjects, out property);
	}
}