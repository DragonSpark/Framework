using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Maui.Configuration;

sealed class SaveIdentity : IStopAware
{
    readonly RemoteConfigurationSettings _settings;
    readonly IStorageValue<Guid>         _identity;

    public SaveIdentity(RemoteConfigurationSettings settings, IStorageValue<Guid> identity)
    {
        _settings = settings;
        _identity = identity;
    }

    public ValueTask Get(CancellationToken parameter) => _identity.Get(new Stop<Guid>(_settings.Identity, parameter));
}