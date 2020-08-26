using JetBrains.Annotations;

namespace DragonSpark.Presentation.Components.Forms.Validation
{
	public class Expression : Text.Text, IExpression
	{
		public Expression([NotNull] string instance) : base(instance) {}
	}
}