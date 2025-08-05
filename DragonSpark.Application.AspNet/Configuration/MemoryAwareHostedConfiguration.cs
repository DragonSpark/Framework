using System;
using System.Collections.Generic;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.AspNet.Configuration;

sealed class MemoryAwareHostedConfiguration : Result<IReadOnlyDictionary<string, object?>>, IHostedConfiguration
{
    public MemoryAwareHostedConfiguration(IHostedConfiguration previous, IMemoryCache memory)
        : base(previous.Then()
                       .Accept()
                       .Store()
                       .In(memory)
                       .For(TimeSpan.FromDays(1).Slide())
                       .Using<MemoryAwareHostedConfiguration>()
                       .Bind()) {}
}