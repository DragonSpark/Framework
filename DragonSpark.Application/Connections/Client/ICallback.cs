using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public interface ICallback : ICommand<HubConnection>, IDisposable {}