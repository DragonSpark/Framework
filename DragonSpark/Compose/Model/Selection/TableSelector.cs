using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Compose.Model.Selection
{
	public sealed class TableSelector<TIn, TOut> : Selector<TIn, TOut>, IResult<ITable<TIn, TOut>>
	{
		readonly ITable<TIn, TOut> _subject;

		public TableSelector(ITable<TIn, TOut> subject) : base(subject) => _subject = subject;

		public new ITable<TIn, TOut> Get() => _subject;
	}
}
