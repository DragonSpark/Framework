using System;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.AspNet.SignalR.Hubs;

namespace DragonSpark.Server.ClientHosting
{
	public class HubNameBuilder : Factory<string[]>
	{
		protected override string[] CreateItem( object parameter )
		{
			var result = AppDomain.CurrentDomain.GetAllTypesWith<HubNameAttribute>().Select( x => x.Item1.HubName ).ToArray();
			return result;
		}
	}
}