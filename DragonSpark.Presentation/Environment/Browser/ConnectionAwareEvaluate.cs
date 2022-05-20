namespace DragonSpark.Presentation.Environment.Browser;

sealed class ConnectionAwareEvaluate : ConnectionAware<string>, IEvaluate
{
	public ConnectionAwareEvaluate(IEvaluate evaluate) : base(evaluate) {}
}