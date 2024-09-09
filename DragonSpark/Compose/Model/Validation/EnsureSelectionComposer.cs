using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation;

public sealed class EnsureSelectionComposer<TIn, TOut>
{
	readonly ISelect<TIn, TOut> _subject;

	public EnsureSelectionComposer(ISelect<TIn, TOut> select) => _subject = select;

	public ConditionalInputSelectionContext<TIn, TOut> Input => new(_subject);

	public ConditionalOutputSelectionContext<TIn, TOut> Output => new(_subject);
}