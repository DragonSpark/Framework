using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace DragonSpark.Application.Connections
{
	public sealed class UserNameMappings : ReferenceValueStore<HubCallerContext, ITable<string, string>>,
	                                       IUserNameMappings
	{
		public static UserNameMappings Default { get; } = new UserNameMappings();

		UserNameMappings() : base(_ => new ConcurrentDictionary<string, string>().ToTable()) {}
	}
}