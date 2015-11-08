using DragonSpark.ComponentModel;
using DynamicExpresso;

namespace DragonSpark.Windows.Runtime
{
	public class ExpressionEvaluator : IExpressionEvaluator
	{
		const string Target = "___target___";

		public object Evaluate( object context, string expression )
		{
			var interpreter = new Interpreter().SetVariable( Target, context );

			var result = interpreter.Eval( string.Concat( Target, ".", expression.TrimStart( '.' ) ) );

			return result;
		}
	}
}