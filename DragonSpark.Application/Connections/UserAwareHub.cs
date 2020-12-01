using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections
{
	public class UserAwareHub : Hub
	{
		readonly ITable<string, string> _names;

		public UserAwareHub(ITable<string, string> names) => _names = names;

		public override Task OnConnectedAsync()
		{
			var name = Context.User?.Identity?.Name;
			if (name != null)
			{
				_names.Assign(name, Context.UserIdentifier.Verify());
			}

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var name = Context.User?.Identity?.Name;
			if (name != null)
			{
				_names.Remove(name);
			}
			return base.OnDisconnectedAsync(exception);
		}
	}
}