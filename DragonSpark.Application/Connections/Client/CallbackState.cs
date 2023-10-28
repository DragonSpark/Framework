using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections.Client;

public sealed record CallbackState(HubConnection Connection, IDisposable Callback);