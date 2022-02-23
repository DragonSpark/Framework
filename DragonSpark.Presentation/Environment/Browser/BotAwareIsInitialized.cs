using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Presentation.Connections.Initialization;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Presentation.Environment.Browser;

sealed class BotAwareIsInitialized : AnyCondition<HttpContext>, IIsInitialized
{
	public BotAwareIsInitialized(IIsInitialized previous)
		: base(previous, IsBot.Default.Then().Accept<HttpContext>(x => x.Request).Get()) {}
}