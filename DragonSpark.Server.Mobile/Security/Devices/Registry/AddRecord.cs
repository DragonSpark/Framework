using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using DragonSpark.Runtime;
using DragonSpark.Server.Mobile.Security.Devices.Claims;

namespace DragonSpark.Server.Mobile.Security.Devices.Registry;

sealed class AddRecord : IDepending<DeviceRecord>
{
    readonly Editors _editors;
    readonly ITime   _time;

    public AddRecord(Editors editors) : this(editors, Time.Default) {}

    public AddRecord(Editors editors, ITime time)
    {
        _editors = editors;
        _time    = time;
    }

    public async ValueTask<bool> Get(Stop<DeviceRecord> parameter)
    {
        var (r, stop) = parameter;
        using var editor = _editors.Get(stop);
        var       now    = _time.Get().UtcDateTime;
        editor.Add(new DeviceKey
        {
            Identity       = r.DeviceId, Kty                       = r.Kty, Crv = r.Crv, X = r.X, Y = r.Y,
            IsBlocked      = r.IsBlocked, CreatedAtUtc             = now,
            AttestedAtUtc  = r.AttestedAtUtc ?? now, LastSeenAtUtc = r.LastSeenAtUtc ?? now,
            EvaluationType = r.EvaluationType
        });
        await editor.Off();
        return true;
    }
}