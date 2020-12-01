using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using Microsoft.AspNetCore.SignalR;

namespace DragonSpark.Application.Connections
{
	public interface IUserNameMappings : ISelect<HubCallerContext, ITable<string, string>> {}
}