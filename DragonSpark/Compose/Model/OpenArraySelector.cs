using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using System;

namespace DragonSpark.Compose.Model
{
	public class ArraySelector<_, T> : OpenArraySelector<_, T>
	{
		readonly ISelect<_, Array<T>> _subject;

		public ArraySelector(ISelect<_, Array<T>> subject) : base(subject.Open()) => _subject = subject;

		public new Selector<_, Array<T>> Subject => new Selector<_, Array<T>>(_subject);
	}

	public class OpenArraySelector<_, T> : CollectionSelector<_, T>
	{
		readonly ISelect<_, T[]> _subject;

		public OpenArraySelector(ISelect<_, T[]> subject) : base(subject) => _subject = subject;

		public new Selector<_, T[]> Subject => new Selector<_, T[]>(_subject);

		public ConditionSelector<_> AllAre(Func<T, bool> condition)
			=> new ConditionSelector<_>(_subject.Select(new AllItemsAre<T>(condition)));

		public OpenArraySelector<_, T> Sort()
			=> new OpenArraySelector<_, T>(_subject.Select(SortAlteration<T>.Default));
	}
}