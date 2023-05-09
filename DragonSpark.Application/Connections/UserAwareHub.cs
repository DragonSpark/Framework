using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections;

public class UserAwareHub : Hub
{
	public override Task OnConnectedAsync()
	{
		var number = Context.User?.Number();
		return number is not null
			       ? Groups.AddToGroupAsync(Context.ConnectionId, number.Value.ToString())
			       : base.OnConnectedAsync();
	}

	public override Task OnDisconnectedAsync(Exception? exception)
	{
		var number = Context.User?.Number();
		return number != null
			       ? Groups.RemoveFromGroupAsync(Context.ConnectionId, number.Value.ToString())
			       : base.OnDisconnectedAsync(exception);
	}
}