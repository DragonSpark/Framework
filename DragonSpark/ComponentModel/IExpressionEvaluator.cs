namespace DragonSpark.ComponentModel
{
	public interface IExpressionEvaluator
	{
		object Evaluate( object context, string expression );
	}
}