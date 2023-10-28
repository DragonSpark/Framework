using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public interface ICurrentConnection : ICommand, IResult<HubConnection>, IAsyncDisposable {}