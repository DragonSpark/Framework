using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Compose.Model.Validation;

public sealed class UnlessComposer<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public UnlessComposer(ISelect<TIn, TOut> subject) => _subject = subject;

	public UnlessInputComposer<TIn, TOut> Input => new(_subject);

	public UnlessUsingComposer<TIn, TOut> Using(ISelect<TIn, TOut> instead) => new(_subject, instead);

	public ConditionalComposer<TIn, TTo> UsingWhen<TTo>(IConditional<TOut, TTo> select)
		=> new Conditional<TIn, TTo>(_subject.Select(select.Condition).Get, _subject.Select(select).Get).Then();
}