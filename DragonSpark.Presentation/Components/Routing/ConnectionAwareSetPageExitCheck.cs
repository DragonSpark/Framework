using DragonSpark.Presentation.Environment.Browser;

namespace DragonSpark.Presentation.Components.Routing;

sealed class ConnectionAwareSetPageExitCheck : ConnectionAware<bool>, ISetPageExitCheck
{
	public ConnectionAwareSetPageExitCheck(ISetPageExitCheck previous) : base(previous) {}
}