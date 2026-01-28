using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class UpsertDevice : IUpsertDevice
{
    readonly UpdateDevice            _update;
    readonly ExceptionAwareAddRecord _add;

    public UpsertDevice(UpdateDevice update, ExceptionAwareAddRecord add)
    {
        _update = update;
        _add    = add;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
        => await _update.Off(parameter) || await _add.Off(parameter);
}