using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class SettingsAwareIsAttested : IIsAttested
{
    readonly IIsAttested _previous;
    readonly bool        _enabled;

    public SettingsAwareIsAttested(IIsAttested previous, DeviceValidationSettings settings)
        : this(previous, settings.IncludeAttestation) {}

    public SettingsAwareIsAttested(IIsAttested previous, bool enabled)
    {
        _previous = previous;
        _enabled  = enabled;
    }

    public ValueTask<bool> Get(Stop<string> parameter) => _enabled ? _previous.Get(parameter) : true.ToOperation();
}