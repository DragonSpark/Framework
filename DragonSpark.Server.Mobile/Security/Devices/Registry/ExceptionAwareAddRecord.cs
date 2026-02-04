using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ExceptionAwareAddRecord : IDepending<DeviceRecord>
{
    readonly AddRecord                        _previous;
    readonly ILogger<ExceptionAwareAddRecord> _logger;
    readonly UpdateDevice                     _update;

    public ExceptionAwareAddRecord(AddRecord previous, ILogger<ExceptionAwareAddRecord> logger, UpdateDevice update)
    {
        _previous = previous;
        _logger   = logger;
        _update   = update;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        try
        {
            return await _previous.Off(parameter);
        }
        catch (DbUpdateException ex) when (IsDuplicate.Default.Get(ex))
        {
            var (record, _) = parameter;
            _logger.LogDebug("Upsert race for {DeviceId}; retrying UPDATE.", record.DeviceId);
            return await _update.Off(parameter);
        }
    }
}