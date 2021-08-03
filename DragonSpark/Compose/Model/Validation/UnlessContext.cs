using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class UnlessContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public UnlessContext(ISelect<TIn, TOut> subject) => _subject = subject;

		public UnlessInputContext<TIn, TOut> Input => new UnlessInputContext<TIn, TOut>(_subject);

		public UnlessUsingContext<TIn, TOut> Using(ISelect<TIn, TOut> instead)
			=> new UnlessUsingContext<TIn, TOut>(_subject, instead);

		public ConditionalSelector<TIn, TTo> UsingWhen<TTo>(IConditional<TOut, TTo> select)
			=> new Conditional<TIn, TTo>(_subject.Select(select.Condition).Get, _subject.Select(select).Get).Then();
	}
}