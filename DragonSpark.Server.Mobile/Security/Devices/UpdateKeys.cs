using DragonSpark.Model.Commands;
using DragonSpark.Runtime;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class UpdateKeys : ICommand<UpdateKeysInput>
{
    public static UpdateKeys Default { get; } = new();

    UpdateKeys() : this(Time.Default) {}

    readonly ITime _time;

    public UpdateKeys(ITime time) => _time = time;

    public void Execute(UpdateKeysInput parameter)
    {
        var (key, builder) = parameter;
        var now = _time.Get().UtcDateTime;
        builder.SetProperty(d => d.Kty, _ => key.Kty)
               .SetProperty(d => d.Crv, _ => key.Crv)
               .SetProperty(d => d.X, _ => key.X)
               .SetProperty(d => d.Y, _ => key.Y)
               .SetProperty(d => d.EvaluationType, _ => key.EvaluationType)
               .SetProperty(d => d.AttestedAtUtc, _ => key.AttestedAtUtc ?? now)
               .SetProperty(d => d.LastSeenAtUtc, _ => key.LastSeenAtUtc ?? now);
    }
}