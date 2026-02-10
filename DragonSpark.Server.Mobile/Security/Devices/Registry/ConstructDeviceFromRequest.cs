using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class ConstructDeviceFromRequest : IStopAware<string, DeviceRecord?>
{
    readonly ComposeRequestJwk _jwk;
    readonly IUpsertDevice     _upsert;

    public ConstructDeviceFromRequest(ComposeRequestJwk jwk, IUpsertDevice upsert)
    {
        _jwk    = jwk;
        _upsert = upsert;
    }

    public async ValueTask<DeviceRecord?> Get(Stop<string> parameter)
    {
        var (subject, stop) = parameter;
        var jwk = _jwk.Get(subject);
        if (jwk is not null)
        {
            var (kty, crv, x, y) = jwk;
            var result = new DeviceRecord(subject, kty, crv, x, y, false, null, null, null);

            if (await _upsert.Off(new(result, stop)))
            {
                return result;
            }
        }

        return null;
    }
}