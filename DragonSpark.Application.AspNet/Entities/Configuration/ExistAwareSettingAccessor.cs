using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

sealed class ExistAwareSettingAccessor : ISettingAccessor
{
	readonly ISettingAccessor _previous;
	readonly Warning          _log;

	public ExistAwareSettingAccessor(ISettingAccessor previous, Warning log)
	{
		_previous = previous;
		_log      = log;
	}

	public sealed class Warning : LogWarning
	{
		public Warning(ILogger<Warning> logger) : base(logger, "Setting entity is not found") {}
	}

	public async ValueTask<string?> Get(string parameter)
	{
		try
		{
			return await _previous.Get(parameter).Await();
		}
		catch (SqlException e) when (e.Message.StartsWith($"Invalid object name '{nameof(Setting)}'"))
		{
			_log.Execute();
			return null;
		}
	}

	public ValueTask Get(Pair<string, string?> parameter) => _previous.Get(parameter);

	public IRemove Remove => _previous.Remove;
}