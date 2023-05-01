using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.SignalR.Client;

namespace DragonSpark.Application.Connections.Client;

public interface IRestartConnection : IOperation<HubConnection>
{

}