using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DragonSpark.Application.Diagnostics;

sealed class ExceptionLogger : IExceptionLogger
{
	readonly ILoggerFactory _factory;
	readonly ILogException  _log;

	public ExceptionLogger(ILoggerFactory factory, ILogException log)
	{
		_factory = factory;
		_log     = log;
	}

	public ValueTask<Exception> Get(ExceptionInput parameter)
	{
		var (owner, exception) = parameter;
		var logger = _factory.CreateLogger(owner);
		var result = _log.Get(new(logger, exception));
		return result;
	}
}