namespace DragonSpark.Presentation.Environment.Browser;

/*sealed class BotAwareIsInitialized : AnyCondition<HttpContext>, IIsInitialized
{
	public BotAwareIsInitialized(IIsInitialized previous)
		: base(previous, IsBotRequest.Default.Then().Accept<HttpContext>(x => x.Request).Get()) {}
}*/