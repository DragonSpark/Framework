using System.Collections.Generic;
using NReco.Linq;

namespace DragonSpark.ComponentModel
{
	public class ExpressionEvaluator : IExpressionEvaluator
	{
		const string Context = "context";

		public static IExpressionEvaluator Default { get; } = new ExpressionEvaluator();
		ExpressionEvaluator() {}

		public object Evaluate( object context, string expression ) => new LambdaParser().Eval( string.Concat( Context, ".", expression.TrimStart( '.' ) ), new Dictionary<string, object> { { Context, context } } );
	}
}