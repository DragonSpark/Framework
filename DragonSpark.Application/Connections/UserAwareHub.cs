using DragonSpark.Compose;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections
{
	public class UserAwareHub : Hub
	{
		readonly IUserNameMappings _names;

		public UserAwareHub(IUserNameMappings names) => _names = names;

		public override Task OnConnectedAsync()
		{
			_names.Get(Context).Assign(Context.User.Verify().UserName(), Context.UserIdentifier.Verify());
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			_names.Get(Context).Remove(Context.User.Verify().UserName());
			return base.OnDisconnectedAsync(exception);
		}
	}
}