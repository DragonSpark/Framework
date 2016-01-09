using System.Collections.Generic;
using NReco.Linq;

namespace DragonSpark.ComponentModel
{
	public interface IExpressionEvaluator
	{
		object Evaluate( object context, string expression );
	}

	public class ExpressionEvaluator : IExpressionEvaluator
	{
		public static ExpressionEvaluator Instance { get; } = new ExpressionEvaluator();

		const string Target = "___target___";

		public object Evaluate( object context, string expression ) => new LambdaParser().Eval( string.Concat( Target, ".", expression.TrimStart( '.' ) ), new Dictionary<string, object> { { Target, context } } );
	}
}