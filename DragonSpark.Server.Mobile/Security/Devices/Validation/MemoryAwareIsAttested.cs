using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class MemoryAwareIsAttested : StopAware<string, bool>, IIsAttested
{
    public MemoryAwareIsAttested(IIsAttested previous, IMemoryCache memory)
        : base(previous.Then()
                       .Store()
                       .In(memory)
                       .For(TimeSpan.FromDays(1))
                       .Using<MemoryAwareIsAttested>(x => x.Subject)) {}
}