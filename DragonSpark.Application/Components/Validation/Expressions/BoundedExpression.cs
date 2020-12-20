using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public sealed class BoundedExpression : ISelect<Bounds, Expression>
	{
		readonly string _expression;

		public BoundedExpression(string expression) => _expression = expression;

		public Expression Get(Bounds parameter) => new Expression($"^{_expression}{{{parameter.Minimum},{parameter.Maximum}}}$");
	}
}