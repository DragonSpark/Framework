using System;
using System.Net.Http;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Configuration;

public interface IRemoteConfigurationMessage
    : IStopAware<HttpResponseMessage>, DragonSpark.Model.Operations.Stop.IStopAware<Exception>;