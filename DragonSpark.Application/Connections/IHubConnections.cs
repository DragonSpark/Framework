using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace DragonSpark.Application.Connections;

public interface IHubConnections : ISelect<Uri, HubConnection> {}