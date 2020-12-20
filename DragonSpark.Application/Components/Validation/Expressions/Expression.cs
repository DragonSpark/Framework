using JetBrains.Annotations;

namespace DragonSpark.Application.Components.Validation.Expressions
{
	public class Expression : Text.Text, IExpression
	{
		public Expression([NotNull] string instance) : base(instance) {}
	}
}