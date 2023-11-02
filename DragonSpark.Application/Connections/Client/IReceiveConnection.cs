using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public interface IReceiveConnection : IResult<HubConnection>, IAsyncDisposable;