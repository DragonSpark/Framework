using System.Linq.Expressions;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class InlineVisitor : ExpressionVisitor
	{
		readonly Expression          _argument;
		readonly ParameterExpression _parameter;

		public InlineVisitor(ParameterExpression parameter, Expression argument)
		{
			_parameter = parameter;
			_argument  = argument;
		}

		protected override Expression VisitParameter(ParameterExpression node) => node == _parameter ? _argument : node;
	}
}