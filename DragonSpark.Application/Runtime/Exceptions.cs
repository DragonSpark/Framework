using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Runtime
{
	public interface IExceptions : IOperation<(Type Owner, Exception Exception)> {}
	sealed class Exceptions : IExceptions
	{
		readonly ILoggerFactory   _factory;

		public Exceptions(ILoggerFactory factory) => _factory    = factory;

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
				logger.LogError(exception.Demystify(), "A problem was encountered while performing this operation.");
			}

			return Task.CompletedTask.ToOperation();
		}
	}


}