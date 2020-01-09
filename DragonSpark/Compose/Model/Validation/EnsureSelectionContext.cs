using DragonSpark.Model.Selection;

namespace DragonSpark.Compose.Model.Validation
{
	public sealed class EnsureSelectionContext<TIn, TOut>
	{
		readonly ISelect<TIn, TOut> _subject;

		public EnsureSelectionContext(ISelect<TIn, TOut> select) => _subject = select;

		public ConditionalInputSelectionContext<TIn, TOut> Input
			=> new ConditionalInputSelectionContext<TIn, TOut>(_subject);

		public ConditionalOutputSelectionContext<TIn, TOut> Output
			=> new ConditionalOutputSelectionContext<TIn, TOut>(_subject);
	}
}