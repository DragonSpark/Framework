using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Connections
{
	public class UserAwareHub : Hub
	{
		readonly ITable<string, List<UserConnection>> _names;

		protected UserAwareHub() : this(Sessions.Default.Get()) {}

		protected UserAwareHub(ITable<string, List<UserConnection>> names) => _names = names;

		public override Task OnConnectedAsync()
		{
			var name = Context.User?.Identity?.Name;
			if (name != null)
			{
				var user = Context.UserIdentifier.Verify();
				var list = _names.Get(name);
				lock (list)
				{
					list.Add(new UserConnection(user, Context.ConnectionId));
				}
			}

			return base.OnConnectedAsync();
		}

		protected IClientProxy Identifiers(string user)
			=> Clients.Users(_names.IsSatisfiedBy(user)
				                 ? _names.Get(user).Select(x => x.User).Distinct()
				                 : Empty.Enumerable<string>());

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			var name = Context.User?.Identity?.Name;
			if (name != null)
			{
				var list = _names.Get(name);
				lock (list)
				{
					list.RemoveAll(x => x.User == Context.UserIdentifier && x.Connection == Context.ConnectionId);
				}
			}

			return base.OnDisconnectedAsync(exception);
		}
	}
}