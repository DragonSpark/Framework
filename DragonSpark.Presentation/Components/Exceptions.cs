using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Selection.Alterations;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq.Dynamic.Core.Exceptions;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Presentation.Components
{
	sealed class Exceptions : IExceptions
	{
		readonly ILoggerFactory   _factory;
		readonly Alter<Exception> _exceptions;

		public Exceptions(ILoggerFactory factory) : this(factory, ExceptionCompensations.Default.Get) {}

		public Exceptions(ILoggerFactory factory, Alter<Exception> exceptions)
		{
			_factory    = factory;
			_exceptions = exceptions;
		}

		public ValueTask Get((Type Owner, Exception Exception) parameter)
		{
			var (owner, exception) = parameter;

			var logger = _factory.CreateLogger(owner);

			if (exception is TemplateException template)
			{
				logger.LogError(template.InnerException.Demystify(), template.Message, template.Parameters.Open());
			}
			else
			{
				var compensated = _exceptions(exception).Demystify();
				logger.LogError(compensated, "A problem was encountered while performing this operation.");
			}

			return Task.CompletedTask.ToOperation();
		}
	}

	sealed class ExceptionCompensations : IAlteration<Exception>
	{
		public static ExceptionCompensations Default { get; } = new ExceptionCompensations();

		ExceptionCompensations() {}

		public Exception Get(Exception parameter) => parameter switch
		{
			ParseException parse => new InvalidOperationException($"{parse.Message}: {parse}", parse),
			_ => parameter
		};
	}
}