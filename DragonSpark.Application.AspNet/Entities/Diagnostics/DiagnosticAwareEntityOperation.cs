using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Diagnostics;

public class DiagnosticAwareEntityOperation<T> : IOperation<T>
{
	readonly IOperation<T> _previous;
	readonly Log           _log;

	protected DiagnosticAwareEntityOperation(IOperation<T> previous, Log log)
	{
		_previous = previous;
		_log      = log;
	}

	public ValueTask Get(T parameter)
	{
		try
		{
			return _previous.Get(parameter);
		}
		catch (DbUpdateConcurrencyException e)
		{
			foreach (var entry in e.Entries)
			{
				_log.Execute(entry.Entity.GetType().AssemblyQualifiedName.Verify());
			}
			throw;
		}
	}

	public sealed class Log : LogWarning<string>
	{
		public Log(ILogger<Log> logger)
			: base(logger,
			       "An attempt was made to save changes via entity context, but an entity of {Type} was not current") {}
	}
}