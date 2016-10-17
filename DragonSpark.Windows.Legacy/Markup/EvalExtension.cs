using DragonSpark.ComponentModel;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class EvalExtension : MarkupExtensionBase
	{
		readonly object item;
		readonly string expression;
		
		public EvalExtension( object item, string expression )
		{
			this.item = item;
			this.expression = expression;
		}

		[Required, Service]
		public IExpressionEvaluator Evaluator { [return: Required]get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) => Evaluator.Evaluate( item, expression );
	}
}