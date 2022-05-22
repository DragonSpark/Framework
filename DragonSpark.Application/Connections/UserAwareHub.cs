using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections;

public class UserAwareHub : Hub
{
	public override Task OnConnectedAsync()
	{
		var name = Context.User?.Identity?.Name;
		return name != null ? Groups.AddToGroupAsync(Context.ConnectionId, name) : base.OnConnectedAsync();
	}

	protected IClientProxy Identifiers(string user) => Clients.Group(user);

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var name = Context.User?.Identity?.Name;
		return name != null
			       ? Groups.RemoveFromGroupAsync(Context.ConnectionId, name)
			       : base.OnDisconnectedAsync(exception);
	}
}