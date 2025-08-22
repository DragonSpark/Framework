using System;
using System.Threading.Tasks;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Configuration;

public class RemoteConfigurationMessageBase : IStopAware<Exception>
{
    readonly IExceptionLogger _logger;

    public RemoteConfigurationMessageBase(IExceptionLogger logger) => _logger = logger;

    public async ValueTask Get(Stop<Exception> parameter)
    {
        await _logger.Off(new(GetType(), parameter));
    }
}