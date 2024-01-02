using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Events;

public interface ICallback : ICommand<HubConnection>, IDisposable;